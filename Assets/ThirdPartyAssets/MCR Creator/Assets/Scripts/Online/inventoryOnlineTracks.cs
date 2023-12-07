// Descrition : Use to create list of car that could be used by players in a race
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Inventory/listTrackOnline", order = 1)]
public class inventoryOnlineTracks : ScriptableObject {
    public List<string> MultiPlayerTrackDisplayedNameList = new List<string>();
    public List<string> MultiPlayerTrackNameList = new List<string> ();
    public List<Sprite> MultiPlayerTrackImageList = new List<Sprite>();
}
