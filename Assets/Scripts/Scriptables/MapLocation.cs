using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapLocation", menuName ="Village/Location/MapLocation")]
public class MapLocation : ScriptableObject
{
	public List<Action> basicActions;
}
