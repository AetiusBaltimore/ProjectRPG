using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour {

	public float BLDamageOut = 10.0f;
	public int HPDamageOut = 1;
	
	private Animator anim;
	private Rigidbody2D enemyRB; 
	private EnemyMove enemyMove; 
	
	private bool attacking;
	
	private float playerDirection;
	
	public bool Attacking{
		get {return attacking;}
		set {attacking = value;}
	}
	
	public Collider2D attackNTrigger;
	public Collider2D attackSTrigger;
	
	private bool EN = false;
	private bool ES = false; 
	
	void Start(){
		anim = GetComponent<Animator>();
		enemyRB = GetComponent<Rigidbody2D>(); 
		enemyMove = GetComponent<EnemyMove>(); 
		
		attackNTrigger.enabled = false;
		attackSTrigger.enabled = false; 
	}
	
	public void StartFunction(string FunctionName){
		print ("Started Coroutine "+FunctionName); 
		StartCoroutine(FunctionName);  
	}
	
	IEnumerator EnemyNormal(){
		if (enemyMove.XMoveDirection < 0){
			anim.SetFloat("Facing", 2f); 
		} else {
			anim.SetFloat("Facing", 1f); 
		}
		StopMovement(); 
		attacking = true; 
		EN = true; 
		anim.SetBool("GabNPunch", EN);
		yield return new WaitForSeconds(.7f);  
		attacking = false; 
		EN = false; 
		anim.SetBool("GabNPunch", EN);
		
	}
	
	public void EnemyCombo(){
		print ("EnemyCombo");
	}
	
	public void EnemyDownNormal(){
		print ("EnemyDownNormal");
	}
	
	public void EnemyUpNormal(){
		print ("EnemyUpNormal");
	}
	
	public void EnemyForwardNormal(){
		print ("EnemyForwardNormal");
	}
	
	public void EnemyBackNormal(){
		print ("EnemyBackNormal");
	}
	
	public void EnemySpecial(){
		print ("EnemySpecial");
	}
	
	public void EnemyDownSpecial(){
		print ("EnemyDownSpecial");
	}
	
	public void EnemyUpSpecial(){
		print ("EnemyUpSpecial");
	}
	
	public void EnemySideSpecial(){
		print ("EnemySideSpecial");
	}
	
	void StopMovement(){
		enemyRB.velocity = new Vector2(0f,0f); 
	}
}
