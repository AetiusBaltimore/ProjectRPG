using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HUDupdater : MonoBehaviour {
	
	public PlayerStats ps;
	public EnemyStats es;
	
	private float pBL_fill;
	private float pExp_fill;
	
	private float eBL_fill;
	private float eExp_fill;
	
	public Image[] pHUD_hearts;
	public Image[] eHUD_hearts;
	
	public Sprite[] heartSprites; 
	
	[SerializeField]
	private Image pEXP_bar, pBL_bar, eEXP_bar, eBL_bar; 
	
	public float pMax_BL, pBL_Value, eMax_BL, eBL_Value;
	public int pHP_Value, pMax_HP, eHP_Value, eMax_HP;  	
	
	// Use this for initialization
	void Start () {
		pMax_BL = ps.maxBalance;
		pMax_HP = ps.maxHealth;
		pBL_bar.fillAmount = 1.0f;
		pEXP_bar.fillAmount = 0f;
		
		eMax_BL = es.maxBalance;
		eMax_HP = es.maxHealth;
		eBL_bar.fillAmount = 1.0f;
		eEXP_bar.fillAmount = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (pBL_fill != pBL_bar.fillAmount || eBL_fill != eBL_bar.fillAmount){
			UpdateBLbar();
		}
		
		UpdatePlHealth();
		UpdateENHealth();
		
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
}
