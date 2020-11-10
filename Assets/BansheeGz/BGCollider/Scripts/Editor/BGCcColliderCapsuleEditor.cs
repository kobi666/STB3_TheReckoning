/* 
    <copyright file="BGCcColliderCapsuleEditor" company="BansheeGz">
        Copyright (c) 2016-2020 All Rights Reserved
   </copyright>
*/


using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
using UnityEditor;
using UnityEngine;

namespace BansheeGz.BGSpline.Editor
{
	[CustomEditor(typeof(BGCcColliderCapsule))]
	public class BGCcColliderCapsuleEditor : BGCcCollider3DAbstractEditor<CapsuleCollider>
	{
		protected override void AdditionalParams()
		{
			EditorGUILayout.PropertyField(serializedObject.FindProperty("radius"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("lengthExtends"));
			
			
			MaterialEditor();
			IsTriggerEditor();
			UpAxisEditor();
			RigidbodyEditor();
			PrefabEditor();
			
			base.AdditionalParams();
		}

	}
}