using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour {
	
	private EnemyStats enemyStats;
	private Animator anim;
	private Rigidbody2D enemyRB;
	private BoxCollider2D boxco;
	private SpriteRenderer enemySR; 
	private EnemyAI enemyAI; 
	
	private Transform enemyTransform; 
	private Transform playerTransform; 
	
	private Vector2 playerDirection;
	
	[SerializeField]
	private bool canMove = true, enemy_paused = false, Moving; 
	
	private bool stunned = false; 
	private float hitStunDuration = .3f; 
	private bool staggered = false; 
	private bool fucked = false; 
	private bool blocking = false;
	private bool chasingPlayer = false;
	[SerializeField]
	private bool facingLeft = false; 
	
	private bool canMoveRight;
	private bool canMoveLeft;
	
	private bool tooClose;
	
	private static bool enemyExists; 
	
	private int eSpeed; 
	[SerializeField]
	private int xMoveDirection; //xMoveDirection is the direction the enemy move. < -1 || 1 >
	
	private float moveX;
	private Vector2 lastMove;
	
	public int XMoveDirection {
		get {return xMoveDirection;}
		set {xMoveDirection = value;}
	}
	
	public Vector2 LastMove{
		get{return lastMove;}
		set{lastMove = value;}
	}
	
	public bool CanMoveRight{
		get {return canMoveRight;}
		set {canMoveRight = value;}	
	}
	
	public bool CanMoveLeft{
		get {return canMoveLeft;}
		set {canMoveLeft = value;}	
	}
	
	public bool Stunned{
		get {return stunned;}
		set {stunned = value;}
	}
	
	public bool Staggered{
		get {return staggered;}
		set {staggered = value;}
	}
	
	public bool Fucked {
		get {return fucked;}
		set {fucked = value;}
	} 
	
	public bool Blocking{
		get {return blocking;}
		set {blocking = value;}
	}
	
	public bool ChasingPlayer{
		get {return chasingPlayer;}
		set {chasingPlayer = value;}
	}
	
	public bool CanMove{
		get {return canMove;}
		set {canMove = value;}
	}
	
	public float HitStunDuration{
		get {return hitStunDuration;}
		set {hitStunDuration = value;}
	}

//}

	void Start (){
		enemyStats = GetComponent<EnemyStats>(); 
		anim = GetComponent<Animator>();
		enemyRB = GetComponent<Rigidbody2D>();
		boxco = GetComponent<BoxCollider2D>();
		enemySR = GetComponent<SpriteRenderer>(); 
		enemyAI = GetComponent<EnemyAI>(); 
		
		enemyTransform = GetComponent<Transform>(); 
		playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>(); 
		
		eSpeed = 5;
		xMoveDirection = 0;		
		canMoveLeft = true;
		canMoveRight = true; 
	}
	
	void FixedUpdate () {
		GetPlayerDirection(); 
		if (canMove){
			if (chasingPlayer){
				ChasePlayerDirection();
				enemy_move();
			}
		}
	}
	
	void enemy_move(){
		moveX = xMoveDirection;
		if (xMoveDirection > 0.5f || xMoveDirection <-0.5f){
			if (CanMoveLeft && xMoveDirection <-0.5f){
				facingLeft = true; 
				enemyRB.velocity = new Vector2 (moveX, 0f) * eSpeed;
			} 
			
			if (CanMoveRight && xMoveDirection > .5f){
				facingLeft = false;
				enemyRB.velocity = new Vector2 (moveX, 0f) * eSpeed;
			}
			lastMove = new Vector2(moveX, 0f); 
			Moving = true; 
		}
		
		if (xMoveDirection < 0.5f && xMoveDirection > -0.5f){
			enemyRB.velocity = new Vector2 (0f,0f); 
			lastMove = new Vector2 (0f, 0f); 
			Moving = false;
		}
		
		SetCharacterFace();
		anim.SetFloat("moveX", moveX);
		anim.SetFloat("LastMoveX", lastMove.x);
		
		anim.SetBool ("Moving", Moving);
	}
	
	public void SetCharacterFace(){
		if (facingLeft){
			enemySR.flipX = true; 
		} else {
			enemySR.flipX = false; 
		}
	}
	
	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.CompareTag("Player")) {
			enemyRB.velocity = new Vector2(0f,0f); 
		}
	}
	
	void OnCollisionStay2D (Collision2D col){
		if (col.gameObject.CompareTag("Player")){
			enemyRB.velocity = new Vector2(0f,0f); 
		}
	}
	
	public void pMatingPress(){
		fucked = true; 
		enemySR.enabled = false; 
	}
	
	// public void Staggered(){
		// print ("Oh no! "+gameObject.name+" has been staggered!"); 
		// canMove = false;
		// staggered = true;
		// anim.SetBool("staggered", staggered);
	// }
	
	IEnumerator HitStun(){
		print ("Enemy HitStun applied");
		canMove = false;
		enemyAI.CanAttack = false; 
		anim.SetBool("stunned", true);
		if (playerDirection.x > .1f){
			enemyRB.AddForce(Vector2.left*10f, ForceMode2D.Impulse);
		} else {
			enemyRB.AddForce(Vector2.right*10f, ForceMode2D.Impulse); 
		}
		yield return new WaitForSeconds(hitStunDuration);  
		anim.SetBool("stunned", false);
		enemyAI.CanAttack = true; 
		canMove = true; 
	}
	
	void GetPlayerDirection(){
		playerDirection = enemyAI.PlayerDirection; 
	}
	
	void ChasePlayerDirection(){ 
		if (playerDirection.x > 0){
			xMoveDirection = 1; 
		} else if (playerDirection.x < 0){
			xMoveDirection = -1; 
		} else {
			print ("Error in chasePlayer at EnemyMove"); 
		} 
	}
	
	public void Block(){
		canMove = false;
		enemyAI.CanAttack = false; 
		enemyRB.mass = 200f; 
		anim.SetBool("Blocking", true);
		blocking = true;
		StartCoroutine("UnBlock"); 
	}
	
	IEnumerator UnBlock(){
		yield return new WaitForSeconds(1f);
		blocking = false;
		anim.SetBool("Blocking", false);
		enemyRB.mass = 10f; 
		canMove = true; 
		enemyAI.CanAttack = true; 
	}

	public void StopEnemyMovement(){
		canMove = false;
		chasingPlayer = false;
		xMoveDirection = 0;
		moveX = 0f; 
		enemyRB.velocity = new Vector2(moveX, 0f); 
		Moving = false; 
		anim.SetBool("Moving", Moving); 
	}
	
	public void ResumeEnemy(){
		boxco.enabled = true;
		enemyRB.bodyType = RigidbodyType2D.Dynamic; 
		enemy_paused = false;
	}
	
}