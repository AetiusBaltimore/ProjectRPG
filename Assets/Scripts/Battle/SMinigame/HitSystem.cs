using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSystem : MonoBehaviour {

	public Collider2D SS;
	public Collider2D GS; 
	private Vector2 temp; 
	
	//Locations are written in the inspector. 
	public Vector2[] LocationList; 
	
	int locationSwitch; 
	
	void Awake(){
		//LocationList.Add(new Vector2(1,1));
		
		DisableSpots();
		PlayerSetSpots();
		EnableSpots(); 
	}
	
	void Update(){
		
	}
	
	void PlayerSetSpots(){
		locationSwitch = Random.Range(0,8);
		
		GS.transform.localPosition = LocationList[locationSwitch]; 
	}
	
	void EnableSpots(){
		GS.enabled = true;
		SS.enabled = true; 
	}
	
	void DisableSpots(){
		GS.enabled = false;
		SS.enabled = false; 
	}
	
	
}
