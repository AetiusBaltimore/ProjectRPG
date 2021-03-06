﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStats : MonoBehaviour {
	/// Houses all of the stats for the enemy that this script is attached to. This code probably contains more than it should ///
	public BattlePlayerController BPC; //reference to player movement controller
	public HUDupdater HUD; 
		
	private EnemyMove enemyMove;
	private Rigidbody2D rb; 

	public float maxBalance;
	public float balance;
	
	public float blockReduction = .75f;
	
	public float lust;
	public float maxLust;
	
	public int health;
	public int maxHealth;

	// Use this for initialization
	void Start () {
		enemyMove = GetComponent<EnemyMove>(); 
		rb = GetComponent<Rigidbody2D>();
		
		maxBalance = 100f;
		
		health = 1;
		maxHealth = 1;
		
		maxLust = 100f;
		
		HUD.eMax_BL = maxBalance;
		HUD.eMax_HP = maxHealth;
		HUD.eMax_Lu = maxLust; 
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.y < -7) {
			Die();
		}

		if (balance <= 0f){
			Unbalanced();
		}
		
		// if (balance < maxBalance){
			// if (!enemyMove.StaggeredStatus()){
				// RestoreBalance(.1f);
			// } else {
				// RestoreBalance(.2f);
			// }
		// }
	}
	
	public void Die(){
		Destroy(gameObject);
		SceneManager.LoadScene("Test");
	}
	
	public void BLDamage (float damageIn){
		///This is Balance damage from normal attacks
		if (enemyMove.Blocking){
			print("I blocked!"); 
			damageIn = damageIn - (damageIn * blockReduction);
		}
		
		print(damageIn); 
		balance -= damageIn;
		HUD.eBL_Value = balance;
		enemyMove.StartCoroutine("HitStun"); 
	}
	
	public void LDamage (float damageIn){
		lust += damageIn; 
		HUD.eLu_Value = lust; 
	}
	
	public void CheckGrabSuccessful(){
		if (enemyMove.Staggered){
			enemyMove.pMatingPress();
			BPC.pMatingPress();
			HUD.SetMinigameStatus(true); 
		} else {
			print ("Gabumon resisted!"); 
		}
	}
	
	public void SDamage (int damageIn){
		//This is HP damage. Also activated Mating Press and the Sex minigame for testing purposes only.
		// if (enemyMove.staggered){
			// health -= damageIn; 
			// HUD.eHP_Value = health; 
			// enemyMove.MatingPress();
			// BPC.MatingPress();
			// HUD.SetMinigameStatus(true); 
		// } else {
			// //haven't tested this yet.
			// print ("Gabumon resisted!"); 
			// enemyMove.Block();
			// BPC.Hitstun();
			// enemyMove.UnBlock();
		// }
	}
	
	void Unbalanced(){
		//"sends message" to movescript that the player is staggered now
		enemyMove.Staggered = true; 
	}
	
	void RestoreBalance(float amountToRestore){
		//The enemy is the avatar. Only they can restore balance.
		balance += amountToRestore;
	}
	
	public int CurrentHealth(){
		return health;
	}
}
