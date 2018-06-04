using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SHitBox : MonoBehaviour { 
	
	private int SDamageOut = 1;
	
	void OnTriggerEnter2D (Collider2D col){
		col.SendMessageUpwards("SDamage", SDamageOut); 
	}
}
