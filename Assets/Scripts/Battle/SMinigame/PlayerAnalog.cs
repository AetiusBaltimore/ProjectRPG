using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnalog : MonoBehaviour {
	/// This class contains all functions for the Player's controls during the sex minigame. ///
	
	private Rigidbody2D pAS; //Player's analog stick Rigidbody
	private SpriteRenderer SR; //Sprite Renderer for the analog stick
	private BattlePlayerController BPC; //Controller script for battle player. May not need to be here.
	
	private GameObject[] player; //This is a reference to the player as a gameObject but I don't think I need it...?
	
	private float radius = 1.3f; //radius of the background of the Analog stick
	private Vector2 center = new Vector2 (0,0); //center of the play area: "anchors" the analog stick icons
	
	[SerializeField] 
	private Vector2 position = new Vector2 (0,0); //Position we want the pAS to go to 
	
	private float actualDistance; //This will hold the distance (in unity units) between the center of the player area to pAS
	private Vector2 centerToPosition; //holds the value for the subtraction between the center Vector2 and the current position of pAS 
	private float x,y = 0f; //x and y input of the player's analog stick
	
	private bool inZone = false; //Is the character in the Good Spot zone?
	
	void Start(){
		//Sets initial stuff
		pAS = GetComponent<Rigidbody2D>(); //Gets reference to rigidbody on attached gameObject
		SR = GetComponent<SpriteRenderer>();
		
		//Idk
		// //player = GameObject.FindGameObjectsWithTag("Player"); 
		// //print (player);
		// //BPC = player.GetComponent<BattlePlayerController>(); 
		// //BPC.SetPlayerPaused(true); 
	}
	
	void Update(){
		//Runs this every frame
		actualDistance = Vector2.Distance(center, position); 
		MoveAnalog();
		
		if (x == 0.0f && y == 0.0f){ //Is there no input from the player?
			ReturnToCenter(); 
		}
	}
	
	void MoveAnalog(){
		//Moves the player analog stick icon, but keeps it within a specified radius.
		//This is meant to be run every frame.
		
		x = Input.GetAxis("Horizontal"); 
		y = Input.GetAxis("Vertical"); 
		
		//This game object is a child of another object, the player space. We need the origin relative to the parent's position.
		position = gameObject.transform.localPosition; 
		
		if (actualDistance > radius){  //Is distance of pAS longer than radius?
			centerToPosition = position - center; 
			centerToPosition.Normalize(); //not sure? Does math stuff with circles. makes numbers take up less memory
			gameObject.transform.localPosition = center + centerToPosition *radius; //set the position of pAS to the edge of the radius
		} 
		pAS.velocity = new Vector2 (x,y)*20; //sets position of the pAS
	}
	
	void ReturnToCenter(){
		gameObject.transform.localPosition = center; 
	}
	
	void OnTriggerEnter2D (Collider2D col){
		//calls this when the game object this is attached to goes through some trigger.
		//Passes in whatever collider entered the trigger zone
		if (col.gameObject.tag == "GS"){
			TurnPlayerRed();
		}
	}
	
	void OnTriggerExit2D (Collider2D col){
		//calls this when the game object leaves the trigger zone
		if (col.gameObject.tag == "GS"){
			TurnPlayerWhite(); 
		}
	}
	
	void TurnPlayerRed(){
		SR.color = Color.red;
	}
	
	void TurnPlayerWhite(){
		SR.color = Color.white; 
	}
}

 
