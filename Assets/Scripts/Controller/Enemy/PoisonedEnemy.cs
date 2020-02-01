﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonedEnemy : MonoBehaviour {

    private SpriteRenderer _sprite;

    private Color default_Color;
    private Color poisoned_Color;
    private bool is_Poisoned = false;


	// Use this for initialization
	void Awake () {
        _sprite = GetComponent<SpriteRenderer>();
        default_Color = _sprite.color;
        poisoned_Color = default_Color * new Color(0.9f, 0.8f, 1.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
        //紫色にする
		if(is_Poisoned && Compare_Color(_sprite.color, default_Color)) {
            _sprite.color = poisoned_Color;
        }
	}

    void OnEnable() {
        is_Poisoned = false;   
    }

    /// <summary>
    /// 毒の継続ダメージを発生させる、色を変える
    /// </summary>
    public void Start_Poisoned_Damaged(bool is_Boss_Enemy) {
        if (is_Poisoned)
            return;
        if (is_Boss_Enemy)
            StartCoroutine("Poisoned_Damaged_Boss_Cor");
        else
            StartCoroutine("Poisoned_Damaged_Cor");
    }


    private IEnumerator Poisoned_Damaged_Cor() {        
        Enemy enemy_Controller = GetComponent<Enemy>();
        if (enemy_Controller == null)
            yield break;

        is_Poisoned = true;
        for (int i = 0; i < 16; i++) {
            enemy_Controller.Damaged(1, "");
            yield return new WaitForSeconds(0.4f);
        }
        is_Poisoned = false;
        if (Compare_Color(_sprite.color, poisoned_Color))
            _sprite.color = default_Color;
    }


    private IEnumerator Poisoned_Damaged_Boss_Cor() {
        BossEnemy _boss = GetComponent<BossEnemy>();
        if (_boss == null)
            yield break;

        is_Poisoned = true;
        for (int i = 0; i < 8; i++) {
            _boss.Damaged(1, "");
            yield return new WaitForSeconds(0.8f);
        }
        is_Poisoned = false;
        if (Compare_Color(_sprite.color, poisoned_Color))
            _sprite.color = default_Color;
    }


    //二つの色が同じならtrue
    private bool Compare_Color(Color c1, Color c2) {
        if(!Mathf.Approximately(c1.r, c2.r)) {
            return false;
        }
        if(!Mathf.Approximately(c1.g, c2.g)) {
            return false;
        }
        if (!Mathf.Approximately(c1.b, c2.b)) {
            return false;
        }
        if (!Mathf.Approximately(c1.a, c2.a)) {
            return false;
        }
        return true;
    }
}