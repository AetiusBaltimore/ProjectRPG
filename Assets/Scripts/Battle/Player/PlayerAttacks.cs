using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttacks : MonoBehaviour {
	///Controls attacks for the player ///
	
	private Animator anim;
	
	private BattlePlayerController BPC; 
	
	private bool attacking = false;
	
	public Collider2D attackNTrigger;
	public Collider2D attackSTrigger; 
	
	private bool NP = false;
	private bool SG = false; 
	
	public bool Attacking {
		get {return attacking;}
		set {attacking = value;}
	}
	
	// Use this for initialization
	void Awake () {
		BPC = gameObject.GetComponent<BattlePlayerController>(); 
		anim = gameObject.GetComponent<Animator> ();
		attackNTrigger.enabled = false;
		attackSTrigger.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("j") && !attacking){
			StartCoroutine("PlayerNormal"); 
		} 
		
		if (Input.GetKeyDown("k") && !attacking){
			SGrab();
		}
		
		// if(attacking){
			// if (attackTimer>0f){
				// attackTimer-=Time.deltaTime;
			// } else {
				// attacking = false;
				// attackNTrigger.enabled = false;
				// NP = false;
				// attackSTrigger.enabled = false; 
				// SG = false; 
			// }
		// }
		//anim.SetBool("SGrab", SG);
	}
	
	IEnumerator PlayerNormal (){
		if (BPC.LastMove.x >= .01f){
			SetColliderPosition("n", .8f, .1f);
		} else {
			 SetColliderPosition("n", -.8f, .1f);
		} 
		attacking = true;
		NP = true;
		BPC.DisableMovement();
		anim.SetBool("NPunch", NP);
		yield return new WaitForSeconds(.2f); 
		NP = false; 
		anim.SetBool("NPunch", NP);
		BPC.EnableMovement();
		attacking = false; 
	}
	
	void SGrab (){
		if (BPC.LastMove.x >= .1f){
			attackSTrigger.offset = new Vector2 (.6f, 0);
		} else {
			attackSTrigger.offset = new Vector2 (-.6f, 0); 
		}
		
		SG = true;
		// attacking = true;
		// attackTimer = attackCD;
		attackSTrigger.enabled = true; 
	}
	
	void SetColliderPosition(string triggerName, float x, float y){
		if (triggerName == "n"){
			print("Attack N trigger set"); 
			attackNTrigger.transform.localPosition = new Vector2(x,y);
		} else if (triggerName == "s") {
			print("Attack S trigger set"); 
			attackSTrigger.transform.localPosition = new Vector2(x,y); 	
		} else {
			print ("Error in setting "+triggerName+" ColliderPosition"); 
		}
	}
}
