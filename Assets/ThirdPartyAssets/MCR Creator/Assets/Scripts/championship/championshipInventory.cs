
// Descrition : Use to create list of car that could be used by players in a race
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;


[CreateAssetMenu(fileName = "ChampionshipData", menuName = "Inventory/championshipData", order = 1)]
public class championshipInventory : ScriptableObject {
	[System.Serializable]
	public class _Championship{
		public string 			championshipName 	= "New Championship";
		public List<string> scenesName 			= new List<string> (1);
        public List<Sprite>     scenesSprite = new List<Sprite>(1);
        public List<string>     TracksName = new List<string>(1);

		public List<int> 		AI_Difficulty 		= new List<int> ();
		public bool				showInfoInCustomEditor = true;
		public Sprite			championshipIconOn;
		public Sprite			championshipIconOff;
		public Vector2 			championshipIconSize = new Vector2 (.5f, .5f);
        public bool             UnlockChampionship = true;

	}


	public List<_Championship> listOfChampionship = new List<_Championship>();


	public  int CheckModification = 0;
    public bool UnlockTrackInArcadeAndTimeTrial = true;

/*	public bool				b_mobile = false;
	public float 			mobileMaxSpeedOffset = 0;
	public float 			mobileWheelStearingOffsetReactivity = 0;
	public bool				b_Countdown = true;
	public bool				b_LapCounter = true;*/
}
