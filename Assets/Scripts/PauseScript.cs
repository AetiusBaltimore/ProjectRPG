using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class PauseScript : MonoBehaviour {

	public static bool gamePaused = false; 
	public TransitionManager transitionManager; 
	
	public GameObject pauseMenu; 
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			if(gamePaused){
				Resume();
			} else {
				Pause(); 
			}
		}
	}
	
	public void Pause(){
		pauseMenu.SetActive(true); 
		Time.timeScale = 0f; 
		gamePaused = true; 
	}
	
	public void Resume(){
		pauseMenu.SetActive(false);
		Time.timeScale = 1f; 
		gamePaused = false; 
	}
	
	public void QuitToMenu(){
		Time.timeScale = 1f;
		gamePaused = false;
		transitionManager.LoadScene("MainMenu"); 
	}
	
	public void QuitBattle(){
		Time.timeScale = 1f;
		gamePaused = false; 
		transitionManager.LoadScene("Test"); 
	}
	
}
