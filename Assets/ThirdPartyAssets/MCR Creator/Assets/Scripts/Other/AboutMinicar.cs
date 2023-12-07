// Description : MCRTestingTrack.cs : This script is used to create a menu that allow to setup the global preferences of MCR Creator
#if (UNITY_EDITOR)
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class AboutMinicar : EditorWindow
{
	public InventoryGlobalPrefs 		inventoryItemList;
	public List<GameObject> 			list_Cars	= new List<GameObject> ();
	public LapCounter 					lapCounter;
	public Game_Manager					game_Manager;
	public DifficultyManager		 	game_ManagerDifficulty;
	public GameObject 					objTextTestMode;

	private Vector2 					scrollPosition = Vector2.zero;


	private Texture2D MakeTex(int width, int height, Color col) {		// use to change the GUIStyle
		Color[] pix = new Color[width * height];
		for (int i = 0; i < pix.Length; ++i) {
			pix[i] = col;
		}
		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();
		return result;
	}

	private Texture2D 		Tex_01;
	private Texture2D 		Tex_02;
	private Texture2D 		Tex_03;
	private Texture2D 		Tex_04;
	private Texture2D 		Tex_05;

	// Add menu item named "Test Mode Panel" to the Window menu
	[MenuItem("Tools/MCR/About Minicar")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(AboutMinicar));
	}

	void OnEnable () {
		
			
	}

	void OnGUI()
	{
		scrollPosition = GUILayout.BeginScrollView (scrollPosition, true, true, GUILayout.Width (position.width), GUILayout.Height (position.height));  



		EditorGUILayout.HelpBox ("Mini car version 1.5.11", MessageType.Info);


		GUILayout.EndScrollView();
	}

	

	void OnInspectorUpdate()
	{
        //Repaint();
	}

}
#endif