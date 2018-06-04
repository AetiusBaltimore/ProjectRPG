using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyStats : MonoBehaviour {
	/// Houses all of the stats for the enemy that this script is attached to. This code probably contains more than it should ///
	public BattlePlayerController BPC; //reference to player movement controller
	public HUDupdater HUD; 
		
	private EnemyMove moveScript;
	private Rigidbody2D rb; 

	public float maxBalance;
	public float balance;
	
	public float blockReduction = .75f;
	
	public int health;
	public int maxHealth;

	// Use this for initialization
	void Start () {
		moveScript = GetComponent<EnemyMove>(); 
		rb = GetComponent<Rigidbody2D>();
		
		maxBalance = 100f;
		
		health = 1;
		maxHealth = 1;
		
		HUD.eMax_BL = maxBalance;
		HUD.eMax_HP = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.y < -7) {
			Die();
		}

		if (balance <= 0f){
			Unbalanced();
		}
		
		if (balance < maxBalance){
			if (!moveScript.StaggeredStatus()){
				RestoreBalance(.1f);
			} else {
				RestoreBalance(.2f);
			}
		}
	}
	
	public void Die(){
		Destroy(gameObject);
		SceneManager.LoadScene("Test");
	}
	
	public void NDamage (float damageIn){
		///This is Balance damage from normal attacks
		if (moveScript.blocking){
			print("I blocked!"); 
			damageIn = damageIn - (damageIn - blockReduction);
		}
		
		balance -= damageIn;
		HUD.eBL_Value = balance;
		moveScript.HitStun();
	}
	
	public void SDamage (int damageIn){
		//This is HP damage. Also activated Mating Press and the Sex minigame for testing purposes only.
		if (moveScript.staggered){
			health -= damageIn; 
			HUD.eHP_Value = health; 
			moveScript.MP();
			BPC.MP();
			SceneManager.LoadScene("SMinigame", LoadSceneMode.Additive); 
		} else {
			//haven't tested this yet.
			print ("Gabumon resisted!"); 
			moveScript.Block();
			BPC.Hitstun();
			moveScript.UnBlock();
		}
	}
	
	void Unbalanced(){
		//"sends message" to movescript that the player is staggered now
		moveScript.Staggered();
	}
	
	void RestoreBalance(float amountToRestore){
		//The enemy is the avatar. Only they can restore balance.
		balance += amountToRestore;
	}
	
	public int CurrentHealth(){
		return health;
	}
}
