/* 
    <copyright file="BGCcCollider3DBoxEditor" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/

using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEditor;
using UnityEngine;

namespace BansheeGz.BGSpline.Editor
{
    [CustomEditor(typeof(BGCcCollider3DBox))]
    public class BGCcCollider3DBoxEditor : BGCcCollider3DAbstractEditor<BoxCollider>
    {
        private static readonly List<BoxCollider> TempColliders = new List<BoxCollider>();

        private BGCcCollider3DBox Collider3DBox
        {
            get { return (BGCcCollider3DBox) cc; }
        }

        protected override void AdditionalParams()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("width"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("height"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("heightOffset"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lengthExtends"));
            
            MaterialEditor();
            IsTriggerEditor();
            UpAxisEditor();
            MeshGeneratorEditor();
            RigidbodyEditor();
            PrefabEditor();
            
            base.AdditionalParams();
        }

        private void MeshGeneratorEditor()
        {
            BGEditorUtility.VerticalBox(() =>
            {
                BGEditorUtility.Horizontal(() =>
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("isMeshGenerationOn"));
                    if (!GUILayout.Button(new GUIContent("Remove meshes", "Remove MeshFilter and MeshRenderer components from all child GameObjects with colliders attached"),
                        GUILayout.Width(120))) return;

                    if (Collider3DBox.IsMeshGenerationOn)
                    {
                        BGEditorUtility.Inform("Error", "Please, turn off 'isMeshGenerationOn' toggle first.");
                        return;
                    }

                    var colliders = new List<BoxCollider>();
                    Collider3DBox.FillChildrenColliders(colliders);
                    foreach (var collider in colliders)
                    {
                        var renderer = collider.GetComponent<MeshRenderer>();
                        if (renderer != null) BGCurve.DestroyIt(renderer);
                        var filter = collider.GetComponent<MeshFilter>();
                        if (filter != null) BGCurve.DestroyIt(filter);
                    }
                });

                if (Collider3DBox.IsMeshGenerationOn) EditorGUILayout.PropertyField(serializedObject.FindProperty("MeshMaterial"));
            });
        }


        [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
        public static void DrawGizmos(BGCcCollider3DBox collider3DBox, GizmoType gizmoType)
        {
            if (!collider3DBox.ShowIfNotSelected) return;

            collider3DBox.FillChildrenColliders(TempColliders);

            if (TempColliders.Count == 0) return;

            BGEditorUtility.SwapGizmosColor(collider3DBox.CollidersColor, () =>
            {
                foreach (var collider in TempColliders)
                {
                    var colliderTransform = collider.transform;

                    var oldMatrix = Gizmos.matrix;
                    Gizmos.matrix *= Matrix4x4.TRS(colliderTransform.position, colliderTransform.rotation, colliderTransform.lossyScale);
                    Gizmos.DrawWireCube(collider.center, collider.size);
                    Gizmos.matrix = oldMatrix;
                }
            });

            TempColliders.Clear();
        }
    }
}