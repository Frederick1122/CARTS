// Descrition : Use to create list of car that could be used by players in a race
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor;


[CreateAssetMenu(fileName = "GripDatas", menuName = "Inventory/Grip Datas", order = 1)]
public class grip_Datas : ScriptableObject {
	[System.Serializable]
    public class _Surface{
        public string 		_Tag 	= "Asphalt";
        public float        CoeffZWhenCarIsSlow = 1.5f;         // 1.5 = the car stop very quickly / 5 = the car stop slowly                    
        public float        GripForward = 1;                    // Reduce the speed of the car 0 to 1
        public float        BrakeForce = 20;                    // Reduce or increase break force 
        public float        SlideCoeff = .12f;                   // Slide coefficient .1f very little slide / .001 very long slide
        public float        SlideEulerAngleY = 10;              // add rotation to the wheel
	}


    public List<_Surface> listOfSurface = new List<_Surface>();

}
