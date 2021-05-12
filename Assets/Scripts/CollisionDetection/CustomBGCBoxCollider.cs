/* 
    <copyright file="BGCcCollider2DBox" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using BansheeGz.BGSpline.Curve;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
    
    /// <summary>Create a set of 2D Box colliders along 2D spline</summary>
    [HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCollider2DBox")]
    [
        CcDescriptor(
            Description = "Create a set of Box 2D colliders along 2D spline.",
            Name = "Custom 2D Box",
            Icon = "BGCcCollider2DBox123")
    ]
    [AddComponentMenu("BansheeGz/BGCurve/Components/BGCcCollider2DBox")][RequireComponent(typeof(CollisionAggregator))]
    public class CustomBGCBoxCollider : BGCcCustomColliderAbstract<BoxCollider2D>
    {
        private int GoCounter;
        protected void Start()
        {
            base.Start();
            if (ParentMyGameObject == null)
            {
                ParentMyGameObject = transform.parent.GetComponent<MyGameObject>();
            }
        }
        protected void Awake()
        {
            CollisionAggregator = GetComponent<CollisionAggregator>();
        }

        [SerializeField] public DetectionTags MyDetectionTag;
        [SerializeField] public List<DetectionTags> TagsICanDetect = new List<DetectionTags>();
        
        //===============================================================================================
        //                                                    Static
        //===============================================================================================
        private static readonly List<BoxCollider2D> TempColliders = new List<BoxCollider2D>();
        private static readonly List<Effector2D> TempEffectors = new List<Effector2D>();

        //===============================================================================================
        //                                                    Editor stuff
        //===============================================================================================

        public override string Error
        {
            get { return ChoseMessage(base.Error, () => Curve.Mode2D != BGCurve.Mode2DEnum.XY ? "Curve should be in XY 2D mode" : null); }
        }

        //===============================================================================================
        //                                                    Fields (Persistent)
        //===============================================================================================
        [SerializeField] [Tooltip("Height of the colliders")]
        private float height = 1;

        [SerializeField] [Tooltip("Height offset for colliders")]
        private float heightOffset;

        [SerializeField] [Tooltip("Set BoxCollider2D usedByEffector flag")]
        private bool usedByEffector;

        [SerializeField] [Tooltip("Material for colliders")]
        private PhysicsMaterial2D material;

        [SerializeField] [Tooltip("Extends for colliders length")]
        private float lengthExtends;

        [SerializeField] [Tooltip("Effector for colliders")]
        private Effector2D effector;

        [SerializeField] [Tooltip("If colliders should be triggers")]
        private bool isTrigger;

        [SerializeField] [Tooltip("Generate kinematic rigidbody for generated colliders. Rigidbody is a must If you plan to move/change colliders at runtime, otherwise do not use it")]
        private bool generateKinematicRigidbody;

        [SerializeField] [Tooltip("Rigidbody for generated colliders. Rigidbody is a must if you plan to move/change colliders at runtime, otherwise do not use it")]
        private Rigidbody2D Rigidbody;


#if UNITY_5_6_OR_NEWER
        [SerializeField] [Tooltip("If colliders are used by composite collider")] private bool usedByComposite;
#endif

#if UNITY_5_6_OR_NEWER
/// <summary>If colliders are used by composite collider</summary>
        public bool UsedByComposite
        {
            get { return usedByComposite; }
            set { ParamChanged(ref usedByComposite, value); }
        }
#endif

        /// <summary>If colliders are triggers</summary>
        public bool IsTrigger
        {
            get { return isTrigger; }
            set { ParamChanged(ref isTrigger, value); }
        }

        public float LengthExtends
        {
            get { return lengthExtends; }
            set { ParamChanged(ref lengthExtends, value); }
        }

        /// <summary>Height for colliders </summary>
        public float Height
        {
            get { return height; }
            set { ParamChanged(ref height, value); }
        }

        public float HeightOffset
        {
            get { return heightOffset; }
            set { ParamChanged(ref heightOffset, value); }
        }

        public bool UsedByEffector
        {
            get { return usedByEffector; }
            set { ParamChanged(ref usedByEffector, value); }
        }

        public PhysicsMaterial2D Material
        {
            get { return material; }
            set { ParamChanged(ref material, value); }
        }

        public Effector2D Effector
        {
            get { return effector; }
            set { ParamChanged(ref effector, value); }
        }

        public Rigidbody2D CustomRigidbody
        {
            get { return Rigidbody; }
            set { ParamChanged(ref Rigidbody, value); }
        }

        public bool GenerateKinematicRigidbody
        {
            get { return generateKinematicRigidbody; }
            set { ParamChanged(ref generateKinematicRigidbody, value); }
        }

        //===============================================================================================
        //                                                    Fields (not persistent)
        //===============================================================================================
        protected override List<BoxCollider2D> WorkingList
        {
            get { return TempColliders; }
        }

        //===============================================================================================
        //                                                    Methods public
        //===============================================================================================
        public override void UpdateUi()
        {
            if (Curve.Mode2D != BGCurve.Mode2DEnum.XY) return;
            base.UpdateUi();
        }

        //===============================================================================================
        //                                                    Private methods
        //===============================================================================================
        protected override void CheckCollider(Component component)
        {
            var effectorType = effector != null ? effector.GetType() : null;

            //get existing
            TempEffectors.Clear();
            component.GetComponents(TempEffectors);

            //find target
            Effector2D targetEffector = null;
            for (var i = TempEffectors.Count - 1; i >= 0; i--)
            {
                var tempEffector = TempEffectors[i];
                if (effectorType == tempEffector.GetType()) targetEffector = tempEffector;
                else BGCurve.DestroyIt(tempEffector);
            }

            TempEffectors.Clear();

            if (effector == null) return;

            if (targetEffector == null) targetEffector = (Effector2D) component.gameObject.AddComponent(effectorType);


            //copy values
            var fields = GetFields(effectorType);
            foreach (var field in fields)
            {
                if (field.IsStatic) continue;
                field.SetValue(targetEffector, field.GetValue(effector));
            }

            var props = GetProperties(effectorType);
            foreach (var prop in props)
            {
                if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
                prop.SetValue(targetEffector, prop.GetValue(effector, null), null);
            }
        }

        protected override void SetUpGoCollider(BoxCollider2D collider, Vector3 from, Vector3 to)
        {
            var dir = to - from;

            var angle = Vector3.Angle(Vector3.right, dir);
            angle = dir.y < 0 ? 360 - angle : angle;


            collider.transform.rotation = Quaternion.Euler(0, 0, angle);
            collider.transform.position = from;

            var colliderLength = dir.magnitude + LengthExtends;

            collider.offset = new Vector3(colliderLength * .5f - LengthExtends * .5f, heightOffset);
            collider.size = new Vector2(colliderLength, height);

            collider.sharedMaterial = Material;

            collider.usedByEffector = usedByEffector;

            

#if UNITY_5_6_OR_NEWER
            if (usedByComposite) collider.usedByComposite = true;
#endif
        }

        private FieldInfo[] GetFields(Type type)
        {
#if NETFX_CORE
            return System.Reflection.TypeExtensions.GetFields(type);
#else
            return type.GetFields();
#endif
        }

        private PropertyInfo[] GetProperties(Type type)
        {
#if NETFX_CORE
            return System.Reflection.TypeExtensions.GetProperties(type);
#else
            return type.GetProperties();
#endif
        }
    }
}