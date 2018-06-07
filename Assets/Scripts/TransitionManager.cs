using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {

	public Animator transAnim; 

	public void LoadScene(string sceneName){
		StartCoroutine(SceneTransition(sceneName));  
	}
	
	IEnumerator SceneTransition (string sceneName){
		transAnim.SetTrigger("FadeOut");
		yield return new WaitForSeconds(1.5f); 
		SceneManager.LoadScene(sceneName); 
	}
}
