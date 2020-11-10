/* 
    <copyright file="BGCcCollider3DAbstractEditor" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/


using BansheeGz.BGSpline.Components;
using UnityEditor;
using UnityEngine;

namespace BansheeGz.BGSpline.Editor
{
    public class BGCcCollider3DAbstractEditor<T> : BGCcColliderAbstractEditor where T:Collider
    {
        private BGCcCollider3DAbstract<T> Collider
        {
            get { return (BGCcCollider3DAbstract<T>) cc; }
        }

        
        protected void UpAxisEditor()
        {
            BGEditorUtility.VerticalBox(() =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("heightAxisMode"));
                if (Collider.HeightAxisMode == BGCcCollider3DAbstract<T>.HeightAxisModeEnum.Custom) EditorGUILayout.PropertyField(serializedObject.FindProperty("customHeightAxis"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("heightAxisRotation"));
            });
        }

        protected void IsTriggerEditor()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isTrigger"));
        }

        protected void MaterialEditor()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("material"));
        }


        protected void RigidbodyEditor()
        {
            BGEditorUtility.VerticalBox(() =>
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("generateKinematicRigidbody"));
                if (!Collider.GenerateKinematicRigidbody) EditorGUILayout.PropertyField(serializedObject.FindProperty("Rigidbody"));
            });
        }

    }
}