using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	public enum AIState{
		Idle, 
		Attack,
		RunAway,
		Chase,
		SAmg,
		Block
	}
	
	public bool ToggleAI; 		
	
	private bool updateState;
	private AIState curState = AIState.Idle;
	private AIState prevState = AIState.Idle;
	
	private GameObject playerGameObject;
	private PlayerAttacks playerAttacks;
	private Transform playerTransform; 
	
	private Transform enemyTransform; 
	private EnemyMove enemyMove;
	private EnemyAttacks enemyAttacks; 
	
	[SerializeField]
	private float distance; 
	[SerializeField]
	private Vector2 direction; 
	private bool pClose; 
	private float decisionRate; 
	
	private bool canAttack; 
	
	public bool CanAttack{
		get {return canAttack;}
		set {canAttack = value;}
	}
	
	public Vector2 PlayerDirection{
		get {return direction;}
		set {direction = value;}
	}
//}

	void Start(){
		playerGameObject = GameObject.FindWithTag("Player");
		playerAttacks = playerGameObject.GetComponent<PlayerAttacks>();
		playerTransform = playerGameObject.GetComponent<Transform>(); 
		
		enemyMove = GetComponent<EnemyMove>(); 
		enemyAttacks = GetComponent<EnemyAttacks>(); 
		enemyTransform = GetComponent<Transform>(); 
		ToggleAI = true;  
		updateState = true; 
		canAttack = true; 
		decisionRate = 1f; 
	}
	
	void Update(){
		if (updateState){
			print ("Start Coroutine MakeDecision()"); 
			StartCoroutine("MakeDecision");
			if (prevState != curState){
				prevState = curState; 
				UpdateStateSwitch(); 
			}
		}
		
		if (curState == AIState.Attack && enemyAttacks.Attacking == false && canAttack){
			//decide attack function here
			AttackState(); 
		}
	}
	
	IEnumerator MakeDecision(){
		print ("Coroutine MakeDecision started"); 
		updateState = false; 
		distance = Vector2.Distance(enemyTransform.position,  playerTransform.position); 
		direction = playerTransform.position - enemyTransform.position;
		if(ToggleAI){
			if(!enemyMove.Fucked){ 
				if (distance<3.5f && distance>1f){//place a trigger here instead. change a bool when player enters. 
					if(!playerAttacks.Attacking){
						curState = AIState.Attack;
					} else {
						curState = AIState.Block;
					}
				} else {
					curState = AIState.Chase; 
				}
			} else {
				curState = AIState.SAmg; 
			}
		} else { 
			curState = AIState.Idle; 
		}
		print("Enemy state = "+curState); 
		yield return new WaitForSeconds(decisionRate); 
		updateState = true; 
	}
	
	void UpdateStateSwitch(){
		switch (curState){
			case AIState.Idle:
				IdleState();
				break;
				
			case AIState.Attack:
				AttackState();  
				break;
				
			case AIState.RunAway:
				RunState();
				break;
				
			case AIState.Chase:
				StartCoroutine("ChaseState"); 
				break;
				
			case AIState.SAmg:
				SAmgState();
				break;
				
			case AIState.Block:
				BlockState();
				break;
		}
	}
	
	
	void IdleState(){
		print ("Enemy is idling");
		enemyMove.StopEnemyMovement(); 
		
	}
	
	IEnumerator ChaseState(){
		print ("Enemy is chasing");
		enemyMove.ChasingPlayer = true;
		yield return new WaitForSeconds(.6f); 
		enemyMove.ChasingPlayer = false; 
	}
	
	void AttackState(){
		enemyAttacks.StartFunction("EnemyNormal");
	}
	
	void RunState(){
		print ("Enemy is running away");
	}
	
	void SAmgState(){
		print ("Enemy is having sex");
	}
	
	void BlockState(){
		print ("Enemy is blocking");
		enemyMove.Block(); 
	}
}
