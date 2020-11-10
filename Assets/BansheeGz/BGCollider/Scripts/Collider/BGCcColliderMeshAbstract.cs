/* 
    <copyright file="BGCcColliderMeshAbstract" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
    public abstract class BGCcColliderMeshAbstract<T> : BGCcSplitterPolyline
    {
        public override string Error
        {
            get { return !Curve.Mode2DOn ? "Curve should be in 2D mode" : null; }
        }

        [SerializeField] [Tooltip("Show even if not selected")]
        private bool showIfNotSelected;

        [SerializeField] [Tooltip("Colliders color then showing not selected")]
        private Color collidersColor = Color.white;

        [SerializeField] [Tooltip("If mesh should be generated along with colliders")]
        private bool isMeshGenerationOn;

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

        public bool IsMeshGenerationOn
        {
            get { return isMeshGenerationOn; }
            set { ParamChanged(ref isMeshGenerationOn, value); }
        }

        private void Reset()
        {
            UpdateRequested(this, new EventArgs());
        }
        
        protected override void UpdateRequested(object sender, EventArgs e)
        {
            base.UpdateRequested(sender, e);

            if (Error != null) return;
			
            var collider = GetComponent<T>();
            if (collider == null) return;

            if (!UseLocal)
            {
                useLocal = true;
                dataValid = false;
            }
            var positions = Positions;

            Build(collider, positions);
        }

        protected T Get<T>() where T : Component
        {
            var component = GetComponent<T>();
            if (component == null) component = gameObject.AddComponent<T>();
            return component;

        }

        protected abstract void Build(T collider, List<Vector3> positions);
    }
}