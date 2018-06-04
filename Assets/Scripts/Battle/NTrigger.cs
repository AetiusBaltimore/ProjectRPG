using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NTrigger : MonoBehaviour {
	
	private float NDamageOut = 50f;
	
	void OnTriggerEnter2D (Collider2D col){
		col.SendMessageUpwards("NDamage", NDamageOut);
	}
}
