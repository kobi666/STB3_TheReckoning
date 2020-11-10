/* 
    <copyright file="BGCcCollider3DMeshEditor" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using BansheeGz.BGSpline.Components;
using UnityEditor;
using UnityEngine;

namespace BansheeGz.BGSpline.Editor
{
    [CustomEditor(typeof(BGCcCollider3DMesh))]
    public class BGCcCollider3DMeshEditor :BGCcColliderMeshAbstractEditor<MeshCollider>
    {
        private BGCcCollider3DMesh MeshCollider
        {
            get { return (BGCcCollider3DMesh) cc; }
        }

        protected override void AdditionalParams()
        {
            base.AdditionalParams();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("flip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("doubleSided"));
            MeshGenerationToggle();
            BGEditorUtility.DisableGui(() =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("meshDoubleSided"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("meshFlip"));
            }, !MeshCollider.IsMeshGenerationOn);
           
        }
        
        [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
        public static void DrawGizmos(BGCcCollider3DMesh polygonCollider, GizmoType gizmoType)
        {
            if (!polygonCollider.ShowIfNotSelected) return;

            var collider = polygonCollider.GetComponent<MeshCollider>();

            if (collider == null) return;

            var mesh = collider.sharedMesh;
            if (mesh == null) return;
            
            var points = mesh.vertices;

            if (points == null || points.Length < 2) return;

            BGEditorUtility.SwapGizmosColor(polygonCollider.CollidersColor, () =>
            {
                var matrix = polygonCollider.transform.localToWorldMatrix;
                var prevPoint = matrix.MultiplyPoint(points[0]);
                for (var i = 1; i < points.Length; i++)
                {
                    var nextPoint = matrix.MultiplyPoint(points[i]);
                    Gizmos.DrawLine(prevPoint, nextPoint);
                    prevPoint = nextPoint;
                }
                if(polygonCollider.Curve.Closed)Gizmos.DrawLine(prevPoint, points[0]);
            });
        }
    }
}