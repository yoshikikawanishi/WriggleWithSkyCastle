﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonoBehaviour {

    //コンポーネント
    private LarvaAttack _attack;
    private BossEnemy _boss;

    //バックデザイン
    [SerializeField] private GameObject back_Design;

    //戦闘開始
    private bool is_Start_Battle = false;


    private void Awake() {
        //取得
        _attack = GetComponent<LarvaAttack>();
        _boss = GetComponent<BossEnemy>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (is_Start_Battle) {

            switch (_boss.Get_Now_Phase()) {
                case 1: _attack.Do_Phase1(); break;
                case 2: _attack.Do_Phase2(); break;
            }

        }

	}


    //戦闘開始
    public void Start_Battle() {
        is_Start_Battle = true;
    }


    //クリア
    public void Clear() {
        _attack.Stop_Phase2();
    }


    //戦闘中の画面エフェクト
    public void Play_Battle_Effect() {
        back_Design.SetActive(true);
        back_Design.transform.localScale = new Vector3(0, 0, 1);
        BackGroundEffector.Instance.Start_Change_Color(new Color(0.45f, 0.4f, 0.35f), 0.1f);
    }
    
    //戦闘中の画面エフェクト消す
    public void Quit_Battle_Effect() {
        back_Design.SetActive(false);
        BackGroundEffector.Instance.Start_Change_Color(new Color(0.85f, 0.8f, 0.7f), 0.1f);
    }


    //点滅溜めエフェクト
    public IEnumerator Pre_Action_Blink() {
        for(int i = 0; i < 4; i++) {
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.5f, 0.3f);
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
    }


    //溜めエフェクト
    public void Play_Charge_Effect(float lifeTime) {
        transform.GetChild(1).gameObject.SetActive(true);
        Invoke("Stop_Charge_Effect", lifeTime);
    }

    public void Stop_Charge_Effect() {
        transform.GetChild(1).gameObject.SetActive(false);
    }


}
