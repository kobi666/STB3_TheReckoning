/* 
    <copyright file="BGCcCollider2DBoxEditor" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using UnityEditor;
using UnityEngine;

namespace BansheeGz.BGSpline.Editor
{
    [CustomEditor(typeof(BGCcCollider2DBox))]
    public class BGCcCollider2DBoxEditor : BGCcColliderAbstractEditor
    {
        private static readonly List<BoxCollider2D> TempColliders = new List<BoxCollider2D>();

        private BGCcCollider2DBox Collider2DBox
        {
            get { return (BGCcCollider2DBox) cc; }
        }

        protected override void AdditionalParams()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lengthExtends"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("height"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("heightOffset"));


#if UNITY_5_6_OR_NEWER
            EditorGUILayout.PropertyField(serializedObject.FindProperty("usedByComposite"));
            if (!Collider2DBox.UsedByComposite) MiscParams();
#else
            MiscParams();
#endif

            BGEditorUtility.VerticalBox(() =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("generateKinematicRigidbody"));
                if (!Collider2DBox.GenerateKinematicRigidbody) EditorGUILayout.PropertyField(serializedObject.FindProperty("Rigidbody"));    
            });
            
            PrefabEditor();

            base.AdditionalParams();
        }

        private void MiscParams()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("material"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isTrigger"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("usedByEffector"));
            BGEditorUtility.Horizontal(() =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("effector"), true);
                if (GUILayout.Button("Sync")) Collider2DBox.UpdateUi();
            });
        }


        [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
        public static void DrawGizmos(BGCcCollider2DBox collider2DBox, GizmoType gizmoType)
        {
            if (!collider2DBox.ShowIfNotSelected) return;

            collider2DBox.FillChildrenColliders(TempColliders);

            if (TempColliders.Count == 0) return;

            BGEditorUtility.SwapGizmosColor(collider2DBox.CollidersColor, () =>
            {
                foreach (var collider in TempColliders)
                {
                    var colliderTransform = collider.transform;

                    var oldMatrix = Gizmos.matrix;
                    Gizmos.matrix *= Matrix4x4.TRS(colliderTransform.position, colliderTransform.rotation, colliderTransform.lossyScale);
                    Gizmos.DrawWireCube(collider.offset, collider.size);
                    Gizmos.matrix = oldMatrix;
                }
            });

            TempColliders.Clear();
        }
    }
}