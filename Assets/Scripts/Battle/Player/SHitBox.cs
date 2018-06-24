using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHitBox : MonoBehaviour { 
	
	void OnTriggerEnter2D (Collider2D col){
		col.SendMessageUpwards("CheckGrabSuccessful"); 
	}
}
