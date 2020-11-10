using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace BansheeGz.BGSpline.Example
{
	public class BGSceneLoader : MonoBehaviour
	{
		//to let know that scenes are loaded via menu
		public static bool Inited;

		//set it to true for main menu scene
		public bool IsMainMenu;
		
		// Use this for initialization
		void Start()
		{
			if(IsMainMenu) Inited = true;
		}

		//for loading scenes
		public void LoadScene(string scene)
		{
			SceneManager.LoadScene(scene);
		}
		
		private void OnGUI()
		{
			if (!Inited || "BGCollidersMainMenu".Equals(SceneManager.GetActiveScene().name)) return;
			
			GUILayout.BeginHorizontal();

			if (GUILayout.Button("To Main Menu")) SceneManager.LoadScene("BGCollidersMainMenu");

			GUILayout.EndHorizontal();
		}

	}
}