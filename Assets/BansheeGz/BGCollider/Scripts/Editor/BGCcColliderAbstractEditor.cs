/* 
    <copyright file="BGCcColliderAbstractEditor" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/


using BansheeGz.BGSpline.Components;
using UnityEditor;
using UnityEngine;

namespace BansheeGz.BGSpline.Editor
{
    public abstract class BGCcColliderAbstractEditor : BGCcSplitterPolylineEditor
    {
        protected override void AdditionalParams()
        {
            BGEditorUtility.VerticalBox(() =>
            {
                var inheritLayerProperty = serializedObject.FindProperty("inheritLayer");
                EditorGUILayout.PropertyField(inheritLayerProperty);
                if (!inheritLayerProperty.boolValue)
                {
                    var layerProperty = serializedObject.FindProperty("layer");
                    var newLayer = EditorGUILayout.LayerField(new GUIContent("Layer", "The layer for children generated colliders"), layerProperty.intValue);
                    if (newLayer != layerProperty.intValue) layerProperty.intValue = newLayer;
                }
            });
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxNumberOfColliders"));
        }

        protected void PrefabEditor()
        {
            var property = serializedObject.FindProperty("childPrefab");
            var oldValue = property.objectReferenceValue;
            EditorGUILayout.PropertyField(property);
            if (oldValue != property.objectReferenceValue)
            {
                var collider = cc as BGCcColliderI;
                if (collider != null && collider.RequireGameObjects) collider.ChildPrefab = property.objectReferenceValue as GameObject;
            }
        }

        protected override void ShowHandlesSettings()
        {
            base.ShowHandlesSettings();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("showIfNotSelected"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("collidersColor"));
        }
    }
}