using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayerController : MonoBehaviour {
	
	private PlayerStats ps; 
	private Animator anim; 
	private Rigidbody2D playerRB;
	private SpriteRenderer playerSR; 
	
	private GameObject enemyGameObject; 
	private Transform enemyTransform; 
	private Rigidbody2D enemyRB;
	private EnemyMove enemyMove; 
	
	[SerializeField]
	private Vector2 enemyDirection;
	
	private bool playerMoving; 
	private bool canMoveLeft = true;
	private bool canMoveRight = true; 
	
	[SerializeField]
	private bool playerPaused = false, canMove = true;
	private bool blocking = false;
	
	private float timer = 1.0f; 
	private float start_time;
	float animDuration;
	
	private static bool playerExists; 

	private int playerSpeed = 12;
	private int playerAirSpeed = 400; //how fast characters move in either direction while in air
	private int playerJumpHeight = 150;
	private int playerAirJumpHeight = 150; //jump height for second jump 
	private bool canAirJump; 
	[SerializeField]
	private float moveX;
	private float moveY = 0.0f; //only needed for blend space in animator
	[SerializeField]
	private Vector2 lastMove;  //used to set idle direction
	private bool isGrounded; //is on a surface they're supposed to be on. 
	private int airJumps = 1;  
	private int maxAirJumps = 1;
	private bool canBlock = true; 
	
	public Vector2 LastMove{
		get {return lastMove;}
	}
	
	public bool CanMove{
		get {return canMove;}
		set {canMove = value;}
	}
	
	public bool CanBlock{
		get {return canBlock;}
		set {canBlock = value;}
	}
	
	public bool Blocking{
		get {return blocking;}
	}
	
	private enum moveState{
		Attack,
		Stun,
		Block,
		Move, 
		Stagger
	}
	
	private moveState curState = moveState.Move; 
	
	private bool MatingPress; 
	//
		//get {return MatingPress;}
		//set {MatingPress = value;}
	//}

	void Start () {
		ps = GetComponent<PlayerStats>();
		anim = GetComponent<Animator>();
		playerRB = GetComponent<Rigidbody2D> ();
		playerSR = GetComponent<SpriteRenderer>(); 
		
		enemyGameObject = GameObject.FindWithTag("Enemy");
		enemyTransform = enemyGameObject.GetComponent<Transform>(); 
		enemyRB = enemyGameObject.GetComponent<Rigidbody2D>(); 
		enemyMove = enemyGameObject.GetComponent<EnemyMove>();
		
		canAirJump = false; 
	}
	
	void FixedUpdate () {
		if (Input.GetButtonDown("Block") && canBlock){ 
			Block();
		} 
		
		if (Input.GetButtonUp("Block") && canBlock){
			UnBlock();
		}

		if(canMove){
			playerMove();
		}
	}

	void playerMove(){
		moveX = Input.GetAxis ("Horizontal");		
		if (Input.GetButtonDown ("Jump") && (isGrounded || airJumps > 0)) {
				Jump ();
			}

		if (Input.GetKey ("down") && !isGrounded){
			//short hop
			//#TODO: Crouching
			playerRB.AddForce(Vector2.down*5000); 
		}
		
		playerMoving = false; //set player moving to false while calculating. Not sure if this is needed.
		
		if (moveX > 0.1f || moveX < -0.1f){
			playerMoving = true;
			if (!canMoveLeft){
				lastMove = new Vector2 (-1f, 0f); 
			} else {
				lastMove = new Vector2 (moveX, 0f);
			}
			CheckEnemyCloseness(); 
			if ((!canMoveLeft && moveX < -0.1f) || (!canMoveRight && moveX > 0.1f)){
				playerMoving = false; 
				moveX = 0f; 
			} else {
				playerRB.velocity = new Vector2(moveX*playerSpeed, playerRB.velocity.y);
			}
		} else {
			playerMoving = false; 
			playerRB.velocity = new Vector2 (0f, playerRB.velocity.y);
		}
		
		// if (moveX < 0.5f && moveX > -0.5f){
			// //if less than half, stop character 
		// }				
		//set animator stuff
		anim.SetFloat ("moveX", moveX);
		anim.SetFloat ("moveY", moveY); 
		
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);
		anim.SetBool ("playerMoving", playerMoving);
	}
	
	void CheckEnemyCloseness(){
		enemyDirection = enemyTransform.position - transform.position;
		if (enemyDirection.x < 1.2f && enemyDirection.x >= 0){
			if (enemyDirection.y <-.9f && enemyDirection.y > -2f){
				canMoveRight = false;
			}				
		} 
		
		if (enemyDirection.x > -1.2f && enemyDirection.x <= 0){
			if (enemyDirection.y <-.9f && enemyDirection.y > -2f){
				canMoveLeft = false;
			}				
		}
		
		if ((enemyDirection.x > 1.2f || enemyDirection.x < -1.2f) && (!canMoveLeft || !canMoveRight)){
			print ("canMove's reset to true"); 
			canMoveLeft = true;
			canMoveRight = true; 
		}
	}	
	
	IEnumerator Jump(){
		//controls jumps and air jumps
		if (!isGrounded){
			if (airJumps > 0 && canAirJump){
				airJumps--;
				playerRB.velocity = new Vector2(playerRB.velocity.x, 0f); 
				playerRB.AddForce (Vector2.up * playerAirJumpHeight, ForceMode2D.Impulse);
				if (airJumps <= 0){
					canAirJump = false;
				}
			}
		}
		//controls direction of jump
		if (isGrounded){
			if (Input.GetAxisRaw("Horizontal") > 0.1f){
				playerRB.AddForce (Vector2.up * playerJumpHeight, ForceMode2D.Impulse);
				playerRB.AddForce (Vector2.right * playerAirSpeed, ForceMode2D.Impulse);
				isGrounded = false;
			}else if (Input.GetAxisRaw("Horizontal") < -0.1f){
				playerRB.AddForce (Vector2.up * playerJumpHeight, ForceMode2D.Impulse);
				playerRB.AddForce (Vector2.left * playerAirSpeed, ForceMode2D.Impulse);
				isGrounded = false;
			} else {
				playerRB.AddForce (Vector2.up * playerJumpHeight, ForceMode2D.Impulse);
				isGrounded = false;
			}
			yield return new WaitForSeconds(.2f); 
			canAirJump = true; 
		}
	}
	
	void Block(){
		playerRB.velocity = new Vector2 (0f, 0f); 
		if (lastMove.x < 0){
			playerSR.flipX = true; 
		}
		canMove = false;
		playerRB.mass = 1000f;
		anim.SetBool("Blocking", true);
		blocking = true; 
	}
	
	void UnBlock(){
		if (lastMove.x < 0){
			playerSR.flipX = false; 
		}
		playerRB.velocity = new Vector2 (0f, 0f); 
		anim.SetBool("Blocking", false);
		blocking = false; 
		playerRB.mass = 10f;
		canMove = true; 
	}
	
	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.CompareTag("ground")) {
			isGrounded = true; 
			airJumps = maxAirJumps;
			canAirJump = false; 
		}
		if (col.gameObject.CompareTag("Enemy")) {
			isGrounded = true; 
		}
		if (col.gameObject.tag == "NAttack"){
			//EnemyAttacks es = col.gameObject.GetComponent<EnemyAttacks>();
			//ps.BLDamage(es.BLDamageOut);
		}
	}
	
	void OnCollisionExit2D (Collision2D col){	
		if (col.gameObject.CompareTag("Enemy")){
			enemyMove.CanMoveLeft = true;
			enemyMove.CanMoveRight = true; 
			canMoveLeft = true;
			canMoveRight = true; 
		}
	}
	
	public void pMatingPress(){
		//Activate Mating press
		canMove = false; 
		MatingPress = true;
		anim.SetBool("MatingPress", MatingPress);
		anim.SetBool("Finishing", false);
	}
	
	public void StartTimedFunction(string functionName){
		StartCoroutine(functionName); 
	}
	
	IEnumerator Hitstun(){
		//Activate hit stun
		print ("Player was hitstunned");
		DisableMovement(); 
		anim.SetBool("Stunned", true); 
		yield return new WaitForSeconds(.4f);
		anim.SetBool("Stunned", false); 
		EnableMovement(); 
	}
	
	public void DisableMovement(){
		canMove = false; 
		canBlock = false; 
		playerRB.velocity = new Vector2(0f,0f); 
		
	}
	
	public void EnableMovement(){
		canMove = true; 
		canBlock = true; 
	}
}