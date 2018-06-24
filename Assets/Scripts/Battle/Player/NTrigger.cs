using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTrigger : MonoBehaviour {
	
	private float BLDamageOut = 10f;
	
	void OnTriggerEnter2D (Collider2D col){
		print("NTrigger hit "+col.name); 
		col.SendMessageUpwards("BLDamage", BLDamageOut);
	}
}
