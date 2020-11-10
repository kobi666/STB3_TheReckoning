/* 
    <copyright file="BGCcColliderAbstract" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System;
using UnityEngine;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;

namespace BansheeGz.BGSpline.Components
{
    /// <summary>Abstract collider generator</summary>
    [DisallowMultipleComponent]
    public abstract class BGCcColliderAbstract<T> : BGCcSplitterPolyline, BGCcColliderI where T : Component
    {
        //===============================================================================================
        //                                                    Editor stuff
        //===============================================================================================
        [SerializeField] [Tooltip("Show even if not selected")]
        private bool showIfNotSelected;

        [SerializeField] [Tooltip("Colliders color then showing not selected")]
        private Color collidersColor = Color.white;

        public bool ShowIfNotSelected
        {
            get { return showIfNotSelected; }
            set { showIfNotSelected = value; }
        }

        public Color CollidersColor
        {
            get { return collidersColor; }
            set { collidersColor = value; }
        }

        public override string Error
        {
            get { return ChoseMessage(base.Error, () => maxExceeded ? "Maximum number of colliders ("+maxNumberOfColliders+") is exceeded. Increase maximum to get rid of this error." : null); }
        }

        //===============================================================================================
        //                                                    Fields (Persistent)
        //===============================================================================================
        [SerializeField] [Tooltip("Maximum number of colliders")]
        private int maxNumberOfColliders = 200;

        //------------------------ not editable by user
        [SerializeField] private bool maxExceeded;

        [SerializeField] [Tooltip("Assign the layer of this GameObject to children colliders")]
        private bool inheritLayer;

        [SerializeField] [Tooltip("The layer of children GameObjects")]
        private int layer;

        [SerializeField] [Tooltip("The prefab to use for children objects")]
        private GameObject childPrefab;


        //===============================================================================================
        //                                                    Properties
        //===============================================================================================
        public bool InheritLayer
        {
            get { return inheritLayer; }
            set { ParamChanged(ref inheritLayer, value); }
        }

        public int Layer
        {
            get { return layer; }
            set { ParamChanged(ref layer, value); }
        }

        /// <summary> Maximum number of colliders </summary>
        public int MaxNumberOfColliders
        {
            get { return maxNumberOfColliders; }
            set { ParamChanged(ref maxNumberOfColliders, value); }
        }

        /// <summary>The prefab to use for children objects</summary>
        public GameObject ChildPrefab
        {
            get { return childPrefab; }
            set
            {
                if (ParamChanged(ref childPrefab, value))
                {
                    if (RequireGameObjects)
                    {
                        List<T> workingList = null;
                        try
                        {
                            workingList = WorkingList;
                            FillChildrenColliders(workingList);
                            foreach (var component in workingList) BGCurve.DestroyIt(component.gameObject);
                            UpdateUi();
                        }
                        finally
                        {
                            if (workingList != null) workingList.Clear();
                        }
                    }
                }
            }
        }

        //===============================================================================================
        //                                                    Fields (Not persistent)
        //===============================================================================================

        /// <summary> If colliders require new GameObjects </summary>
        public virtual bool RequireGameObjects
        {
            get { return true; }
        }

        protected abstract List<T> WorkingList { get; }

        protected virtual bool LocalSpace
        {
            get { return false; }
        }

        //===============================================================================================
        //                                                    Unity Callbacks
        //===============================================================================================

        //===============================================================================================
        //                                                    Public functions
        //===============================================================================================
        public override void AddedInEditor()
        {
            UpdateUi();
        }

        /// <summary>rebuilds colliders</summary>
        public virtual void UpdateUi()
        {
            var requireGameObjects = RequireGameObjects;

            var workingList = WorkingList;

            if (requireGameObjects)
            {
                //get children colliders for reusing
                FillChildrenColliders(workingList);
            }

            //get positions
            if (LocalSpace && !UseLocal)
            {
                useLocal = true;
                dataValid = false;
            }

            var positions = Positions;
            var positionsCount = positions.Count;
            maxExceeded = false;

            var cursor = 0;
            //at least 2 points are needed
            if (positionsCount > 1)
            {
                var count = Mathf.Min(maxNumberOfColliders + 1, positionsCount);
                if (maxNumberOfColliders + 1 < positionsCount) maxExceeded = true;

                if (requireGameObjects)
                {
                    var isPlaying = Application.isPlaying;
                    var baseName = gameObject.name + " Collider[";
                    var baseLayer = gameObject.layer;
                    for (var i = 1; i < count; i++)
                    {
                        Component collider;

                        //------------ get/create collider
                        if (workingList.Count <= cursor)
                        {
                            if (childPrefab == null)
                            {
                                var go = new GameObject();
                                go.transform.parent = transform;
                                collider = go.AddComponent<T>();
                            }
                            else
                            {
                                var go = Instantiate(childPrefab, transform);
                                collider = go.GetComponent<T>();
                                if (collider == null) collider = go.AddComponent<T>();
                            }
                        }
                        else collider = workingList[cursor];

                        CheckCollider(collider);

                        //------------  assign a layer
                        collider.gameObject.layer = inheritLayer ? baseLayer : layer;


                        //------------  set name (in Editor only)
                        if (!isPlaying)
                        {
                            try
                            {
                                collider.gameObject.name = baseName + (i - 1) + ']';
                            }
                            catch (MissingReferenceException)
                            {
                                return;
                            }
                        }

                        //------------- set up collider
                        SetUpGoCollider((T) collider, positions[i - 1], positions[i]);

                        cursor++;
                    }
                }
                else
                {
                    FillSingleCollider(positions, count);
                }
            }

            if (requireGameObjects)
            {
                //destroy not used GO
                var collidersCount = workingList.Count;
                if (cursor < collidersCount)
                {
                    //temp list is used to properly handle undo operations
                    for (var i = collidersCount - 1; i >= cursor; i--)
                    {
                        var collider = workingList[i];
                        workingList.RemoveAt(i);

                        BGCurve.DestroyIt(collider.gameObject);
                    }
                }

                workingList.Clear();
            }
        }


        /// <summary>returns first level children Box colliders</summary>
        public void FillChildrenColliders(List<T> resultList)
        {
            resultList.Clear();
            try
            {
                GetComponentsInChildren(resultList);
            }
            catch (MissingReferenceException)
            {
                return;
            }

            var collidersCount = resultList.Count;
            if (collidersCount == 0) return;

            var myTransform = transform;
            for (var i = collidersCount - 1; i >= 0; i--)
                if (resultList[i].transform.parent != myTransform)
                    resultList.RemoveAt(i);
        }

        //===============================================================================================
        //                                                    Private functions
        //===============================================================================================
        // check if everything is ok 
        protected virtual void CheckCollider(Component component)
        {
        }

        //set up collider data
        protected virtual void SetUpGoCollider(T collider, Vector3 from, Vector3 to)
        {
        }

        //fill in data for single collider
        protected virtual void FillSingleCollider(List<Vector3> positions, int count)
        {
        }

        //curve was updated
        protected override void UpdateRequested(object sender, EventArgs e)
        {
            base.UpdateRequested(sender, e);

            UpdateUi();
        }

    }
    //===============================================================================================
    //                                                    nested
    //===============================================================================================
    public interface BGCcColliderI
    {
         GameObject ChildPrefab { get; set; }
         bool RequireGameObjects { get; }
    }

}