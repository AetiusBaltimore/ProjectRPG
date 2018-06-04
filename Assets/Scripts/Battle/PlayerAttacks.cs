using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttacks : MonoBehaviour {
	///Controls attacks for the player ///
	
	private Animator anim;
	
	private BattlePlayerController MovementController; 
	
	private bool attacking = false;
	
	private float attackTimer = 0f;
	private float attackCD = .5f;
	
	public Collider2D attackNTrigger;
	public Collider2D attackSTrigger; 
	
	private bool NP = false;
	private bool SG = false; 
	
	// Use this for initialization
	void Awake () {
		MovementController = gameObject.GetComponent<BattlePlayerController>(); 
		anim = gameObject.GetComponent<Animator> ();
		attackNTrigger.enabled = false;
		attackSTrigger.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("j") && !attacking){
			NPunch();
		} 
		
		if (Input.GetKeyDown("k") && !attacking){
			SGrab();
		}
		
		if(attacking){
			if (attackTimer>0f){
				attackTimer-=Time.deltaTime;
			} else {
				attacking = false;
				attackNTrigger.enabled = false;
				NP = false;
				attackSTrigger.enabled = false; 
				SG = false; 
			}
		}
		anim.SetBool("NPunch", NP);
		anim.SetBool("SGrab", SG);
	}
	
	void NPunch (){
		
		if (MovementController.ReturnLastMoveX() >= .1f){
			attackNTrigger.offset = new Vector2 (.6f, 0); 
		} else {
			attackNTrigger.offset = new Vector2 (-.6f, 0); 
		}
		
		NP = true;
		attacking = true;
		attackTimer = attackCD;
		attackNTrigger.enabled = true; 
	}
	
	void SGrab (){
		SG = true;
		attacking = true;
		attackTimer = attackCD;
		attackSTrigger.enabled = true; 
	}
}
