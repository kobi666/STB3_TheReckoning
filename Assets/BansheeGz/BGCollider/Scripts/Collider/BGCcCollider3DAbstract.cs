/* 
    <copyright file="BGCcCollider3DAbstract" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
    public abstract class BGCcCollider3DAbstract<T> : BGCcColliderAbstract<T> where T : Collider
    {
        //===============================================================================================
        //                                                    enums
        //===============================================================================================
        public enum HeightAxisModeEnum
        {
            Y,
            X,
            Z,
            Custom
        }

        //===============================================================================================
        //                                                    Static
        //===============================================================================================
        private static readonly List<T> TempColliders = new List<T>();


        //===============================================================================================
        //                                                    Fields Persistent
        //===============================================================================================
        [SerializeField] [Tooltip("Material for colliders")]
        private PhysicMaterial material;

        [SerializeField] [Tooltip("Height Axis direction.")]
        private HeightAxisModeEnum heightAxisMode = HeightAxisModeEnum.Y;

        [SerializeField] [Tooltip("Custom Height Axis for Custom height axis mode")]
        private Vector3 customHeightAxis = Vector3.up;

        [Range(0, 360)] [SerializeField] [Tooltip("Height axis rotation in degrees. Default is 0, which is Vector.up. 90 is Vector.right, 180 is Vector.down and 270 is Vector.left.")]
        private float heightAxisRotation;

        [SerializeField] [Tooltip("If colliders should be triggers")]
        private bool isTrigger;

        [SerializeField] [Tooltip("Generate kinematic rigidbody for generated colliders")]
        private bool generateKinematicRigidbody;

        [SerializeField] [Tooltip("Custom Rigidbody for generated colliders")]
        private Rigidbody Rigidbody;

        //===============================================================================================
        //                                                    Properties
        //===============================================================================================
        protected override List<T> WorkingList
        {
            get { return TempColliders; }
        }

        /// <summary>If colliders are triggers</summary>
        public bool IsTrigger
        {
            get { return isTrigger; }
            set { ParamChanged(ref isTrigger, value); }
        }

        /// <summary>Physic Material for colliders</summary>
        public PhysicMaterial Material
        {
            get { return material; }
            set { ParamChanged(ref material, value); }
        }

        public HeightAxisModeEnum HeightAxisMode
        {
            get { return heightAxisMode; }
            set { ParamChanged(ref heightAxisMode, value); }
        }

        public Vector3 CustomHeightAxis
        {
            get { return customHeightAxis; }
            set { ParamChanged(ref customHeightAxis, value); }
        }

        public float HeightAxisRotation
        {
            get { return heightAxisRotation; }
            set { ParamChanged(ref heightAxisRotation, value); }
        }

        public bool GenerateKinematicRigidbody
        {
            get { return generateKinematicRigidbody; }
            set { ParamChanged(ref generateKinematicRigidbody, value); }
        }

        public Rigidbody CustomRigidbody
        {
            get { return Rigidbody; }
            set { ParamChanged(ref Rigidbody, value); }
        }

        protected Vector3 RotationUpAxis
        {
            get
            {
                Vector3 upDirection;
                switch (heightAxisMode)
                {
                    case HeightAxisModeEnum.Y:
                        upDirection = Vector3.up;
                        break;
                    case HeightAxisModeEnum.X:
                        upDirection = Vector3.right;
                        break;
                    case HeightAxisModeEnum.Z:
                        upDirection = Vector3.forward;
                        break;
                    case HeightAxisModeEnum.Custom:
                        upDirection = customHeightAxis;
                        if (upDirection == Vector3.zero) upDirection = Vector3.up;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("HeightAxisMode");
                }

                return upDirection;
            }
        }
        
        //===============================================================================================
        //                                                    Methods
        //===============================================================================================
        protected void SetUpRigidBody(GameObject go)
        {
            var colliderRigidbody = go.GetComponent<Rigidbody>();
            if (generateKinematicRigidbody || Rigidbody != null)
            {
                if (colliderRigidbody == null) colliderRigidbody = go.AddComponent<Rigidbody>();
                if (generateKinematicRigidbody)
                {
                    colliderRigidbody.isKinematic = true;
                }
                else
                {
                    colliderRigidbody.mass = Rigidbody.mass;
                    colliderRigidbody.drag = Rigidbody.drag;
                    colliderRigidbody.angularDrag = Rigidbody.angularDrag;
                    colliderRigidbody.useGravity = Rigidbody.useGravity;
                    colliderRigidbody.isKinematic = Rigidbody.isKinematic;
                    colliderRigidbody.interpolation = Rigidbody.interpolation;
                    colliderRigidbody.collisionDetectionMode = Rigidbody.collisionDetectionMode;
                    colliderRigidbody.constraints = Rigidbody.constraints;
                }
            }
            else if (colliderRigidbody != null) BGCurve.DestroyIt(colliderRigidbody);
        }
    }
}