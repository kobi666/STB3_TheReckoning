/* 
    <copyright file="BGCcCollider3DMesh" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
    /// <summary>Fill in 3d Mesh collider inside 2D spline</summary>
    [HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCollider3DMesh")]
    [
        BGCc.CcDescriptor(
            Description = "Create a mesh collider inside 2D spline.",
            Name = "Collider 3D Mesh",
            Icon = "BGCcCollider3DMesh123")
    ]
    [RequireComponent(typeof(MeshCollider))]
    [AddComponentMenu("BansheeGz/BGCurve/Components/BGCcCollider3DMesh")]
    public class BGCcCollider3DMesh : BGCcColliderMeshAbstract<MeshCollider>
    {
        
        [SerializeField] [Tooltip("Double sided")] private bool doubleSided;
        [SerializeField] [Tooltip("Flip triangles")] private bool flip;
        [SerializeField] [Tooltip("Generated mesh double sided")] private bool meshDoubleSided;
        [SerializeField] [Tooltip("Generated mesh flipped")] private bool meshFlip;
        

        /// <summary>Should faces be flipped </summary>
        public bool Flip
        {
            get { return flip; }
            set
            {
                if (flip == value) return;
                ParamChanged(ref flip, value);
            }
        }

        /// <summary>Generate triangles for backside</summary>
        public bool DoubleSided
        {
            get { return doubleSided; }
            set { ParamChanged(ref doubleSided, value); }
        }
        
        public bool MeshDoubleSided
        {
            get { return meshDoubleSided; }
            set { ParamChanged(ref meshDoubleSided, value); }
        }
        public bool MeshFlip
        {
            get { return meshFlip; }
            set { ParamChanged(ref meshFlip, value); }
        }

        private BGTriangulator2D triangulator;

        protected override void Build(MeshCollider collider, List<Vector3> positions)
        {
            //collider
            var mesh = collider.sharedMesh;
            if (mesh == null) mesh = new Mesh();

            if (triangulator == null) triangulator = new BGTriangulator2D();

            triangulator.Bind(mesh, positions, new BGTriangulator2D.Config
            {
                Closed = Curve.Closed,
                Mode2D = Curve.Mode2D,
                Flip = flip,
                ScaleUV = Vector2.one,
                OffsetUV = Vector2.zero,
                DoubleSided = doubleSided,
                ScaleBackUV = Vector2.one,
                OffsetBackUV = Vector2.zero,
            });
            collider.sharedMesh = mesh;
            
            //mesh
            if (IsMeshGenerationOn)
            {
                var renderer = Get<MeshRenderer>();
                var filter = Get<MeshFilter>();
                var filterMesh = filter.sharedMesh;
                if (filterMesh == null) filterMesh = new Mesh();
                
                triangulator.Bind(filterMesh, positions, new BGTriangulator2D.Config
                {
                    Closed = Curve.Closed,
                    Mode2D = Curve.Mode2D,
                    Flip = meshFlip,
                    ScaleUV = Vector2.one,
                    OffsetUV = Vector2.zero,
                    DoubleSided = meshDoubleSided,
                    ScaleBackUV = Vector2.one,
                    OffsetBackUV = Vector2.zero,
                });
                
                filter.sharedMesh = filterMesh;
            }
        }

    }
}