using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour {
	
	//calls to other scripts and components on the current gameObject
	private EnemyStats es;
	private Animator anim;
	private Rigidbody2D enemyRB;
	private BoxCollider2D boxco;
	private SpriteRenderer SR; 
	
	[SerializeField]
	private bool can_move = true, enemy_paused = false, Moving; //Sorry, bad form. SerializeField is dumb. 
	//these are movement related booleans
	
	private bool stunned = false; //is enemy stunned?
	public bool staggered = false; //is enemy staggered?
	
	private bool fucked = false; //is enemy being fucked?
	
	public bool blocking = false; 
	
	private static bool enemyExists; //this will be used when the enemy dies to make sure more than one gameObject isn't in the scene. 
	
	[SerializeField]
	private int EnemySpeed, XMoveDirection; //XMoveDirection is the direction the enemy move. < -1 || 1 >
	//These are for enemy movement

	private float moveX; //this is for the animator (moving)
	private Vector2 LastMove; //this is for the animator (idle)
	
	private float timer = 1.0f; //this is for animation timing
	private float start_time; //time of animation starting
	float animDuration; //length (in seconds) that you want the animation to be. 
	
	void Start (){
		es = GetComponent<EnemyStats>(); 
		anim = GetComponent<Animator>();
		enemyRB = GetComponent<Rigidbody2D>();
		boxco = GetComponent<BoxCollider2D>();
		SR = GetComponent<SpriteRenderer>(); 
		EnemySpeed = 5;
		XMoveDirection = 0; 
	}
	
	// Update is called once per frame
	void Update () {
		if (enemy_paused){ //if enemy is paused, player animation for timer
			AnimationDurationTimer(animDuration);
		} else if (staggered){ //is enemy still staggered? 
			StaggerDurationTimer();
		} else {
			enemy_move();
		}
	}
	
	void enemy_move(){
		moveX = (float)XMoveDirection;
		if(can_move){
			if (XMoveDirection > 0.5f || XMoveDirection <-0.5f){
				enemyRB.velocity = new Vector2 (XMoveDirection, 0f) * EnemySpeed;
				LastMove = new Vector2(moveX, 0f); 
				Moving = true; 
			}
			
			if (XMoveDirection < 0.5f && XMoveDirection > -0.5f){
				LastMove = new Vector2 (0f, 0f); 
				Moving = false;
			}
			
			if (moveX < 0f){
				moveX*=-2;
				LastMove.x = moveX;
				anim.SetFloat("moveX", moveX);
			} else {
				anim.SetFloat("moveX", moveX);
				anim.SetFloat("LastMoveX", LastMove.x); 
			}
			anim.SetBool ("Moving", Moving);
		}
	}
	
	void FlipCharacter(){
		if (XMoveDirection > 0){
			XMoveDirection = -1; 
		} else {
			XMoveDirection = 1;
		}
	}
	
	void OnCollisionEnter2D (Collision2D col){
		//Debug.Log ("Gabumon has collided with" + col.collider.name);
		//if (col.gameObject.tag == "ground") {
			//FlipCharacter();
		//}
		
		if (col.gameObject.tag == "Player") {
			//print ("Touched by player");
			enemyRB.velocity = Vector2.zero; 
		}
	}
	
	void OnCollisionStay2D (Collision2D col){
		if (col.gameObject.tag == "Player"){
			enemyRB.velocity = Vector2.zero; 
		}
	}
	
	public void MP(){
		//Debug.Log ("Running Mating Press"); 
		ResetAnimationTimer(10f);
		fucked = true; 
		//anim.SetBool("MatingPress", true);
		SR.enabled = false; 
	}
	
	public void Staggered(){
		print ("Oh no! "+gameObject.name+" has been staggered!"); 
		can_move = false;
		staggered = true;
		anim.SetBool("staggered", staggered);
	}
	
	public void HitStun(){
		print ("Oof! (HitStun)");
		ResetAnimationTimer(.5f);
		PauseEnemy();
		anim.SetBool("stunned", true); 
	}
	
	public void Block(){
		can_move = false;
		anim.SetBool("Blocking", true);
		blocking = true;
	}
	
	public void UnBlock(){
		anim.SetBool("Blocking", false);
		blocking = false;
		can_move = true; 
	}
	
	void AnimationDurationTimer(float duration){
		duration = timer - (Time.time - start_time); 
		if (duration <= 0.0f){
			EndAnimations();
		}
	}
	
	void StaggerDurationTimer (){
		if (es.balance >= es.maxBalance){
			EndAnimations();
		}
	}
	
	void EndAnimations(){
		staggered = false;
		
		if (fucked){
			UnloadSM(); 
		}
		
		fucked = false;
		anim.SetBool("MatingPress", false);
		anim.SetBool("staggered", staggered);
		anim.SetBool("stunned", false); 
		ResumeEnemy();
		if (es.CurrentHealth() <= 0){
			es.Die();
		}
	}
	
	void UnloadSM(){
		SceneManager.UnloadSceneAsync("SMinigame"); 
	}
	
	public void PauseEnemy(){
		boxco.enabled = false;
		enemyRB.bodyType = RigidbodyType2D.Static;
		enemy_paused = true;
	}
	
	public void ResumeEnemy(){
		boxco.enabled = true;
		enemyRB.bodyType = RigidbodyType2D.Dynamic; 
		enemy_paused = false;
	}
	
	public bool StaggeredStatus(){
		return staggered; 
	}
	
	public bool FuckedStatus(){
		return fucked; 
	}
	
	void ResetAnimationTimer(float time){
		timer = time; 
		start_time = Time.time;
	}
	
}