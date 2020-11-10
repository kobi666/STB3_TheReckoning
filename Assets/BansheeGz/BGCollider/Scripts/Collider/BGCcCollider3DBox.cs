/* 
    <copyright file="BGCcCollider3DBox" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

namespace BansheeGz.BGSpline.Components
{
    /// <summary>Create a set of box colliders along the spline</summary>
    [HelpURL("http://www.bansheegz.com/BGCurve/Cc/BGCcCollider3DBox")]
    [
        CcDescriptor(
            Description = "Create a set of box colliders along 3D spline.",
            Name = "Collider 3D Box",
            Icon = "BGCcCollider3DBox123")
    ]
    [AddComponentMenu("BansheeGz/BGCurve/Components/BGCcCollider3DBox")]
    public class BGCcCollider3DBox : BGCcCollider3DAbstract<BoxCollider>
    {

        //===============================================================================================
        //                                                    Fields (Persistent)
        //===============================================================================================
        [SerializeField] [Tooltip("Height of the colliders")]
        private float height = 1;

        [SerializeField] [Tooltip("Width of the colliders")]
        private float width = .2f;

        [SerializeField] [Tooltip("Extends for colliders length")]
        private float lengthExtends;

        [SerializeField] [Tooltip("Offset by height")]
        private float heightOffset;

        [SerializeField] [Tooltip("If mesh should be generated along with colliders")]
        private bool isMeshGenerationOn;

        [SerializeField] [Tooltip("Generated mesh material. Note, UVs are not scaled, so only material with single-color texture will work fine")]
        private Material MeshMaterial;

        //===============================================================================================
        //                                                    Properties
        //===============================================================================================

        /// <summary>Extends for colliders length</summary>
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

        /// <summary>Width for colliders </summary>
        public float Width
        {
            get { return width; }
            set { ParamChanged(ref width, value); }
        }


        public float HeightOffset
        {
            get { return heightOffset; }
            set { ParamChanged(ref heightOffset, value); }
        }

        public bool IsMeshGenerationOn
        {
            get { return isMeshGenerationOn; }
            set { ParamChanged(ref isMeshGenerationOn, value); }
        }

        public Material MeshMaterialToGenerate
        {
            get { return MeshMaterial; }
            set { ParamChanged(ref MeshMaterial, value); }
        }

        //===============================================================================================
        //                                                    Fields (not persistent)
        //===============================================================================================

        //===============================================================================================
        //                                                    Private methods
        //===============================================================================================
        protected override void SetUpGoCollider(BoxCollider collider, Vector3 from, Vector3 to)
        {
            //transform position 
            collider.transform.position = from;

            //transform  rotation
            var dir = to - from;
            collider.transform.rotation = Quaternion.LookRotation(dir, RotationUpAxis);
            collider.transform.Rotate(Vector3.forward, HeightAxisRotation);

            //colliders center and size
            var colliderLength = dir.magnitude + LengthExtends;
            collider.center = new Vector3(0, Height * .5f + heightOffset, colliderLength * .5f - LengthExtends * .5f);
            collider.size = new Vector3(width, Height, colliderLength);

            //set is trigger
            collider.isTrigger = IsTrigger;
            //set material
            collider.material = Material;

            //generate mesh along with colliders
            if (isMeshGenerationOn)
            {
                var offset = collider.transform.InverseTransformDirection(dir) * .5f + Vector3.up * (heightOffset + Height * .5f);
                GenerateMesh(collider, offset, width, Height, colliderLength);
            }

            //rigidbody
            SetUpRigidBody(collider.gameObject);
        }

        private void GenerateMesh(BoxCollider collider, Vector3 offset, float boxLength, float boxWidth, float boxHeight)
        {
            var meshRenderer = collider.GetComponent<MeshRenderer>();
            if (meshRenderer == null) meshRenderer = collider.gameObject.AddComponent<MeshRenderer>();
            meshRenderer.sharedMaterial = MeshMaterial;


            var meshFilter = collider.GetComponent<MeshFilter>();
            if (meshFilter == null) meshFilter = collider.gameObject.AddComponent<MeshFilter>();
            var mesh = meshFilter.sharedMesh;
            if (mesh == null)
            {
                mesh = new Mesh();
                meshFilter.sharedMesh = mesh;
            }

            //original code is http://wiki.unity3d.com/index.php/ProceduralPrimitives#C.23_-_Box
            //------ Vertices
            var p0 = new Vector3(-boxLength * .5f, -boxWidth * .5f, boxHeight * .5f) + offset;
            var p1 = new Vector3(boxLength * .5f, -boxWidth * .5f, boxHeight * .5f) + offset;
            var p2 = new Vector3(boxLength * .5f, -boxWidth * .5f, -boxHeight * .5f) + offset;
            var p3 = new Vector3(-boxLength * .5f, -boxWidth * .5f, -boxHeight * .5f) + offset;

            var p4 = new Vector3(-boxLength * .5f, boxWidth * .5f, boxHeight * .5f) + offset;
            var p5 = new Vector3(boxLength * .5f, boxWidth * .5f, boxHeight * .5f) + offset;
            var p6 = new Vector3(boxLength * .5f, boxWidth * .5f, -boxHeight * .5f) + offset;
            var p7 = new Vector3(-boxLength * .5f, boxWidth * .5f, -boxHeight * .5f) + offset;
            var vertices = new[]
            {
                // Bottom
                p0, p1, p2, p3,
                // Left
                p7, p4, p0, p3,
                // Front
                p4, p5, p1, p0,
                // Back
                p6, p7, p3, p2,
                // Right
                p5, p6, p2, p1,
                // Top
                p7, p6, p5, p4
            };

            //------- UV
            var _00 = new Vector2(0f, 0f);
            var _10 = new Vector2(1f, 0f);
            var _01 = new Vector2(0f, 1f);
            var _11 = new Vector2(1f, 1f);
            var uvs = new[]
            {
                // Bottom
                _11, _01, _00, _10,
                // Left
                _11, _01, _00, _10,
                // Front
                _11, _01, _00, _10,
                // Back
                _11, _01, _00, _10,
                // Right
                _11, _01, _00, _10,
                // Top
                _11, _01, _00, _10,
            };

            //------- triangles
            var triangles = new[]
            {
                // Bottom
                3, 1, 0,
                3, 2, 1,
                // Left
                3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
                3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
                // Front
                3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
                3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
                // Back
                3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
                3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
                // Right
                3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
                3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
                // Top
                3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
                3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
            };

            //---- mesh
            mesh.vertices = vertices;
//            mesh.normals = normales;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

    }
}