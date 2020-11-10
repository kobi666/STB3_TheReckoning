/* 
    <copyright file="BGCcColliderCapsule" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
    /// <summary>Create a set of capsule colliders along the spline</summary>
    [HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcColliderCapsule")]
    [
        CcDescriptor(
            Description = "Create a set of capsule colliders along 3D spline.",
            Name = "Collider Capsule",
            Icon = "BGCcColliderCapsule123")
    ]
    [AddComponentMenu("BansheeGz/BGCurve/Components/BGCcColliderCapsule")]
    public class BGCcColliderCapsule : BGCcCollider3DAbstract<CapsuleCollider>
    {
        //===============================================================================================
        //                                                    Fields (Persistent)
        //===============================================================================================
        [SerializeField] [Tooltip("Capsule radius ")]
        private float radius = .1f;

        [SerializeField] [Tooltip("Extends for colliders length")]
        private float lengthExtends;

        //===============================================================================================
        //                                                    Properties
        //===============================================================================================

        /// <summary>Capsule radius</summary>
        public float Radius
        {
            get { return radius; }
            set { ParamChanged(ref radius, value); }
        }
        
        /// <summary>Extends for colliders length</summary>
        public float LengthExtends
        {
            get { return lengthExtends; }
            set { ParamChanged(ref lengthExtends, value); }
        }
        //===============================================================================================
        //                                                    Private methods
        //===============================================================================================
        protected override void SetUpGoCollider(CapsuleCollider collider, Vector3 @from, Vector3 to)
        {
            //transform position 
            collider.transform.position = from;

            //transform  rotation
            var dir = to - from;
            collider.transform.rotation = Quaternion.LookRotation(dir, RotationUpAxis);
            collider.transform.Rotate(Vector3.forward, HeightAxisRotation);

            //colliders center and size
            collider.direction = 2; //z axis
            var length = dir.magnitude;
            var height = length + lengthExtends * 2;
            collider.height = height;
            collider.center = new Vector3(0, 0, length * .5f);
            collider.radius = radius;

            //set is trigger
            collider.isTrigger = IsTrigger;
            //set material
            collider.material = Material;


            //rigidbody
            SetUpRigidBody(collider.gameObject);
        }
    }
}