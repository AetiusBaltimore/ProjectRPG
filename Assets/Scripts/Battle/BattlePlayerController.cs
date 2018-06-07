using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerController : MonoBehaviour {
	
	private PlayerStats ps; 
	private Animator anim; 
	private Rigidbody2D playerRB;
	
	private bool playerMoving; 
	
	[SerializeField]
	private bool playerPaused = false, canMove = true;
	private bool blocking = false;
	
	private float timer = 1.0f; 
	private float start_time;
	float animDuration;
	
	private static bool playerExists; 

	private int playerSpeed = 12;
	private int playerAirSpeed = 400; //how fast characters move in either direction while in air
	private int playerJumpHeight = 7500;
	private int playerAirJumpHeight = 6000; //jump height for second jump 
	private float moveX;
	private float moveY = 0.0f; //only needed for blend space in animator
	private Vector2 lastMove;  //used to set idle direction
	private bool isGrounded; //is on a surface they're supposed to be on. 
	private int airJumps = 1;  
	private int maxAirJumps = 1;
	
	private bool MatingPress; 
		
	void Start () {
		ps = GetComponent<PlayerStats>();
		anim = GetComponent<Animator>();
		playerRB = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {
		if (Input.GetButton ("Block")){ 
			Block();
		} else {
			UnBlock();
		}
		
		moveX = Input.GetAxis ("Horizontal");
		
		if(playerPaused){ //if player is paused, play an animation
			AnimationDurationTimer(animDuration);
		}

		if(canMove){
			player_move();
		}
	}

	void player_move(){		
		if (Input.GetButtonDown ("Jump") && (isGrounded || airJumps > 0)) {
				Jump ();
			}

		if (Input.GetKey ("down")){
			//short hop
			//#TODO: Crouching
			playerRB.AddForce(Vector2.down*5000); 
		}
		
		playerMoving = false; //set player moving to false while calculating. Not sure if this is needed.
		
		if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f){
			//move character if analog stick is moved past half left or right. 
			playerMoving = true;
			lastMove = new Vector2 (moveX, 0f);
			playerRB.velocity = new Vector2(moveX*playerSpeed, playerRB.velocity.y);
		} 
		
		if (Input.GetAxisRaw("Horizontal") < 0.5f && Input.GetAxisRaw("Horizontal") > -0.5f){
			//if less than half, stop character
			playerRB.velocity = new Vector2 (0f, playerRB.velocity.y); 
		}				
		//set animator stuff
		anim.SetFloat ("moveX", moveX);
		anim.SetFloat ("moveY", moveY); 
		
		anim.SetBool ("playerMoving", playerMoving);
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);
	}
	
	void Jump(){
		//controls jumps and air jumps
		if (!isGrounded){
			if (airJumps > 0){
				playerRB.AddForce (Vector2.up * playerAirJumpHeight);
				airJumps--;
			}
		}
		//controls direction of jump
		if (isGrounded){
			if (Input.GetAxisRaw("Horizontal") > 0.1f){
				playerRB.AddForce (Vector2.up * playerJumpHeight);
				playerRB.AddForce (Vector2.right * playerAirSpeed);
				isGrounded = false;
			}else if (Input.GetAxisRaw("Horizontal") < -0.1f){
				playerRB.AddForce (Vector2.up * playerJumpHeight);
				playerRB.AddForce (Vector2.left * playerAirSpeed);
				isGrounded = false;
			} else {
				playerRB.AddForce (Vector2.up * playerJumpHeight);
				isGrounded = false;
			}
		} 
	}
	
	void Block(){
		playerPaused = true;
		anim.SetBool("Blocking", true);
		blocking = true; 
	}
	
	void UnBlock(){
		blocking = false;
		playerPaused = false; 
		anim.SetBool("Blocking", false);
	}
	
	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.tag == "ground") {
			isGrounded = true; 
			airJumps = maxAirJumps;
		}
		if (col.gameObject.tag == "Enemy") {
			isGrounded = true; 
		}
			
		if (col.gameObject.tag == "NAttack"){
			//#TODO: Check if in air and restore jumps if so
			EnemyAttacks es = col.gameObject.GetComponent<EnemyAttacks>();
			ps.BLDamage(es.BLDamageOut);
		}
	}
	
	public void pMatingPress(){
		//Activate Mating press
		SetCanMove(false); 
		MatingPress = true;
		anim.SetBool("MatingPress", MatingPress);
		anim.SetBool("Finishing", false);
	}
	
	public void Hitstun(){
		//Activate hit stun
		print ("Player was hitstunned");
		ps.PausePlayer();
		ResetAnimationTimer(.5f);
	}
	
	void AnimationDurationTimer(float duration){
		//Timer for animations that have specific time. I will die if this is capable in the animator because omfg. This is a headache.
		duration = timer - (Time.time - start_time); 
		if (duration <= 0.0f){
			EndAnimations();
		}
	}
	
	void ResetAnimationTimer(float time){
		timer = time; 
		start_time = Time.time;
	}
	
	public void EndAnimations(){
		//this should end all current animations and set player back to a playing state.
		if (MatingPress){
			MatingPress = false;
			anim.SetBool("MatingPress", MatingPress);
			anim.SetBool("Finishing", true); 
		}
		
		SetPlayerPaused(false);
		SetCanMove(true);
	}
	
	public bool CheckPlayerPaused(){
		return playerPaused;
	}
	
	public void SetPlayerPaused(bool status){
		playerPaused = status;
	}
	
	public float ReturnLastMoveX(){
		return lastMove.x;
	}
	
	public float ReturnLastMoveY(){
		return lastMove.y; 
	}
	
	public bool ReturnBlockStatus(){
		return blocking;
	}
	
	public void SetCanMove(bool status){
		canMove = status; 
	}
}