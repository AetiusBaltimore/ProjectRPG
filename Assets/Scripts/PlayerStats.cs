using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 


public class PlayerStats : MonoBehaviour {
	/// Contains all the stats (and possibly more...) for the character ///
	private Rigidbody2D playerRB;
	private BoxCollider2D playercol;
	private BattlePlayerController BPC; 
	
	public HUDupdater HUD;
	
	private bool pausedStatus;

	public float maxBalance;
	public float balance;
	
	public int health;
	public int maxHealth;
	
	public float lust;
	public float maxLust; 
	
	private float blockReduction;
	
	void Start () {
		playercol = GetComponent<BoxCollider2D> ();
		playerRB = GetComponent<Rigidbody2D> ();
		BPC = GetComponent<BattlePlayerController>(); 
		
		maxBalance = 100.0f;
		balance = maxBalance; 
		
		health = 3;
		maxHealth = 3;
	
		HUD.pMax_BL = maxBalance;
		HUD.pMax_HP = maxHealth;
		
		blockReduction = .75f;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.y < -7 || health <= 0) {
			Die();
		}
		
		if (balance < maxBalance){
			RestoreBalance(.1f);
		}
	}
	
	void Die () {
		//#TODO: Restart or Quit splash screen
		SceneManager.LoadScene("TestBattle"); 
	}
	
	public void PausePlayer(){
		//Send message to BattlePlayerController script for the player to be paused. It may or may not need more code to complete that
		pausedStatus = true; 
		BPC.SetPlayerPaused(pausedStatus);  
		//playercol.enabled = false; 
		//playerRB.bodyType = RigidbodyType2D.Static; 
	}
	
	public void ResumePlayer(){
		BPC.EndAnimations();
	}
	
	public void BLDamage (float DamageIn){
		//Balance damage calculation. Called from enemy hitbox
		if (BPC.ReturnBlockStatus()){
			DamageIn = DamageIn - (DamageIn*blockReduction);
		}
		balance -= DamageIn; 
		HUD.pBL_Value = balance;	
	}
	
	void ResetBL (){
		balance = maxBalance;
		HUD.pBL_Value = balance; 
	}
	
	public void HPDamage (int DamageIn){
		//HP damage calculation. Not fleshed-out. Player is very subby. 
		health -= DamageIn; 
	}
	
	void ResetHP (){
		health = maxHealth; 
		HUD.pHP_Value = health; 
	}
	
	void RestoreBalance(float amountToRestore){
		//The player is the true avatar. Only he can restore balance to the world. 
		balance += amountToRestore;
	}
}
