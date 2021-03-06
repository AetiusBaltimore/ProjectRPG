﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float moveSpeed;

	private Animator anim; 
	private Rigidbody2D playerRB; 
	private SpriteRenderer SR; 
	
	public TransitionManager transitionManager;

	private bool playerMoving; 
	public Vector2 lastMove; 

	private static bool playerExists; 
	
	private bool canMove;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> (); 
		playerRB = GetComponent<Rigidbody2D> ();
		SR = GetComponent<SpriteRenderer>(); 
		
		canMove = true;
		
		//if (!playerExists) {
			//playerExists = true; 
			//DontDestroyOnLoad (transform.gameObject); 
		//} else {
			//Destroy (gameObject); 
		//}
	}
	
	// Update is called once per frame
	void Update () {
		if (canMove){
			MovePlayer(); 
		}

	}
	
	void MovePlayer(){
		playerMoving = false;
		if (Input.GetAxisRaw ("Horizontal") > 0.5f || Input.GetAxisRaw ("Horizontal") < -0.5f) {
			//transform.Translate (new Vector3 (Input.GetAxisRaw ("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f)); 
			playerRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal")*moveSpeed, playerRB.velocity.y); 
			playerMoving = true;
			lastMove = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0f);  
		}
		if (Input.GetAxisRaw ("Vertical") > 0.5f || Input.GetAxisRaw ("Vertical") < -0.5f) {
			//transform.Translate (new Vector3 (0f, Input.GetAxisRaw ("Vertical") * moveSpeed * Time.deltaTime, 0f));
			playerRB.velocity = new Vector2(playerRB.velocity.x, Input.GetAxisRaw("Vertical")*moveSpeed); 
			playerMoving = true;
			lastMove = new Vector2 (0f, Input.GetAxisRaw ("Vertical"));
		}

		if (Input.GetAxisRaw ("Horizontal") < 0.5f && Input.GetAxisRaw ("Horizontal") > -0.5f) {
			playerRB.velocity = new Vector2 (0f, playerRB.velocity.y); 
		}

		if (Input.GetAxisRaw ("Vertical") < 0.5f && Input.GetAxisRaw ("Vertical") > -0.5f) {
			playerRB.velocity = new Vector2 (playerRB.velocity.x, 0f); 
		}

		SR.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
		
		anim.SetFloat ("MoveX", Input.GetAxisRaw ("Horizontal"));
		anim.SetFloat ("MoveY", Input.GetAxisRaw ("Vertical")); 

		anim.SetBool ("playerMoving", playerMoving); 
		anim.SetFloat ("LastMoveX", lastMove.x);
		anim.SetFloat ("LastMoveY", lastMove.y);
	}
	
	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.tag == "Enemy"){
			transitionManager.LoadScene("TestBattle");
		}
	}
}
