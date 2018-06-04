using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour {

	private float timer = 10.0f; 
	private float start_time; 
	private float duration; 
	
	// Use this for initialization
	void Start () {
		start_time = Time.time; 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void ResetAnimationDuration(){
		timer = 10.0f; 
	}
	
	void AnimationDurationTimer(float duration){
		// Debug.Log("Duration: " + duration); 
		// Debug.Log("Timer: " + timer);
		// Debug.Log("start_time: " + start_time); 
		duration = timer - (Time.time - start_time); 
		//Debug.Log ("Duration: " + duration); 
		if (duration <= 0.0f){
			return;
		}
	}
	
}
