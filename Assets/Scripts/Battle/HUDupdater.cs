using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HUDupdater : MonoBehaviour {
	
	public PlayerStats ps;
	public EnemyStats es;
	
	private float pBL_fill;
	private float pLu_fill;
	
	private float eBL_fill;
	private float eLu_fill;
	
	public Image[] pHUD_hearts;
	public Image[] eHUD_hearts;
	
	public Sprite[] heartSprites; 
	
	[SerializeField]
	private Image pLu_bar, pBL_bar, eLu_bar, eBL_bar; 
	
	public float pMax_BL, pBL_Value, pMax_Lu, pLu_Value;
	public float eMax_BL, eBL_Value, eMax_Lu, eLu_Value;
	public int pHP_Value, pMax_HP, eHP_Value, eMax_HP;  	
	
	private float stageTimer;
	private float stageTimerRate; 
	private float stageTimerFill; 
	private float stageTimerMax; 
	private float stageTimerIncreasePerStage; 
	
	public Image stageTimerBar;
	public GameObject stageTimerBack;
	public GameObject PlayerSpace;
	public GameObject EnemySpace;
	
	private int stageCount;
	private int stageCountMax; 
	
	private bool minigameActive;
	
	// Use this for initialization
	void Start () {
		pMax_BL = ps.maxBalance;
		pMax_HP = ps.maxHealth;
		
		pBL_bar.fillAmount = 1.0f;
		pLu_bar.fillAmount = 0f;
		
		eMax_BL = es.maxBalance;
		eMax_HP = es.maxHealth;
		
		eBL_bar.fillAmount = 1.0f;
		eLu_bar.fillAmount = 0f;
		
		stageCount = 0;
		stageCountMax = 3; 
		
		stageTimerBar.fillAmount = 0f; 
		stageTimerMax = 1f;
		stageTimerRate = .005f;
		stageTimerIncreasePerStage = .002f;

		SetMinigameStatus(false); 
	}
	
	// Update is called once per frame
	void Update () {
		if (pBL_fill != pBL_bar.fillAmount || eBL_fill != eBL_bar.fillAmount){
			UpdateBLbar();
		}
		
		UpdatePlHealth();
		UpdateENHealth();
		
		
		if (minigameActive){
			MinigameMain();
		}
		
	}
	
	void MinigameMain(){
	//decisions for minigame. Meant to be run every frame
		if (stageTimer >= stageTimerMax){
			IncreaseStageCount();
			if (stageCount >= stageCountMax){
				SetMinigameStatus(false);
				ps.ResumePlayer();
			}
		} else {
			UpdateStageTimer();
		} 
		UpdateELust();
	}
	
	void UpdateStageTimer(){
		stageTimer += stageTimerRate; 
		stageTimerBar.fillAmount = stageTimer; 
	}
	
	void IncreaseStageTimerRate (float Val){
		stageTimerRate += Val;
	}
	
	void IncreaseStageCount(){
		stageCount++;
		print ("Stage: " + stageCount); 
		stageTimer = 0f; 
		IncreaseStageTimerRate(stageTimerIncreasePerStage); 
	}
	
	void UpdateBLbar (){
		pBL_Value = (ps.balance/pMax_BL);
		pBL_bar.fillAmount = pBL_Value;
		
		eBL_Value = (es.balance/eMax_BL);
		eBL_bar.fillAmount = eBL_Value;
	}
	
	void UpdatePlHealth (){
		pHP_Value = ps.health;
		
		if (pHP_Value > pMax_HP){
			pHP_Value = pMax_HP;
		}
		
		for (int i = 0; i < pHUD_hearts.Length; i++){
			if (i < pHP_Value){
				pHUD_hearts[i].sprite = heartSprites[1];
			} else {
				pHUD_hearts[i].sprite = heartSprites[0];
			}
			
			if (i < pMax_HP){
				pHUD_hearts[i].enabled = true;
			} else {
				pHUD_hearts[i].enabled = false; 
			}
		}
	}
	
	void UpdateENHealth(){
		eHP_Value = es.health;
		
		if (eHP_Value > eMax_HP){
			eHP_Value = eMax_HP;
		}
		
		for (int i = 0; i < eHUD_hearts.Length; i++){
			if (i < eHP_Value){
				eHUD_hearts[i].sprite = heartSprites[1];
			} else {
				eHUD_hearts[i].sprite = heartSprites[0];
			}
			
			if (i < eMax_HP){
				eHUD_hearts[i].enabled = true;
			} else {
				eHUD_hearts[i].enabled = false; 
			}
		}
	}
	
	public void SetMinigameStatus(bool status){
		stageTimerBack.SetActive(status); 
		PlayerSpace.SetActive(status);
		EnemySpace.SetActive(status); 
		
		minigameActive = status;
	}
	
	void UpdateELust(){
		eLu_Value = (es.lust/eMax_Lu);
		eLu_bar.fillAmount = eLu_Value;		
	}
}
