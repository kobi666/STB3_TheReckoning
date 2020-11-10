/* 
    <copyright file="BGCcColliderMeshAbstractEditor" company="BansheeGz">
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
    public class BGCcColliderMeshAbstractEditor<T> : BGCcSplitterPolylineEditor
    {
        private BGCcColliderMeshAbstract<T> MeshColliderAbstract
        {
            get { return (BGCcColliderMeshAbstract<T>) cc; }
        }
        protected override void AdditionalParams()
        {
            base.AdditionalParams();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty("showIfNotSelected"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("collidersColor"));
        }

        protected void MeshGenerationToggle()
        {
            BGEditorUtility.Horizontal(() =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("isMeshGenerationOn"));
                if (!GUILayout.Button(new GUIContent("Remove meshes", "Remove MeshFilter and MeshRenderer components from all child GameObjects with colliders attached"),
                    GUILayout.Width(120))) return;

                if (MeshColliderAbstract.IsMeshGenerationOn)
                {
                    BGEditorUtility.Inform("Error", "Please, turn off 'isMeshGenerationOn' toggle first.");
                    return;
                }

                var renderer = MeshColliderAbstract.GetComponent<MeshRenderer>();
                if (renderer != null) BGCurve.DestroyIt(renderer);
                var filter = MeshColliderAbstract.GetComponent<MeshFilter>();
                if (filter != null) BGCurve.DestroyIt(filter);
            });
        }
    }
}