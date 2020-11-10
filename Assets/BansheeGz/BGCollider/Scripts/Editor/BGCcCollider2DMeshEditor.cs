/* 
    <copyright file="BGCcCollider2DMeshEditor" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using BansheeGz.BGSpline.Components;
using UnityEditor;
using UnityEngine;

namespace BansheeGz.BGSpline.Editor
{
    [CustomEditor(typeof(BGCcCollider2DMesh))]
    public class BGCcCollider2DMeshEditor :BGCcColliderMeshAbstractEditor<PolygonCollider2D>
    {
        protected override void AdditionalParams()
        {
            base.AdditionalParams();
            MeshGenerationToggle();
        }

        [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
        public static void DrawGizmos(BGCcCollider2DMesh polygonCollider, GizmoType gizmoType)
        {
            if (!polygonCollider.ShowIfNotSelected) return;

            var collider = polygonCollider.GetComponent<PolygonCollider2D>();

            if (collider == null) return;

            var points = collider.points;

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
            });
        }
    }
}