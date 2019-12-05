﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashBlockController : MonoBehaviour {


    private List<string> destroyer_Tag_List = new List<string> {
        "PlayerBulletTag",
        "PlayerChargeBulletTag",
        "PlayerAttackTag"
    };

    [SerializeField] private int life = 1;
    [Space]
    [SerializeField] private Sprite damaged_Sprite;


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        foreach(string tag in destroyer_Tag_List) {
            if(collision.tag == tag) {
                Damaged();
            }
        }        
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        foreach (string tag in destroyer_Tag_List) {
            if (collision.gameObject.tag == tag) {
                Damaged();
            }
        }
    }


    //攻撃を受けた時の処理
    private void Damaged() {
        life--;
        if (life > 0) {
            StartCoroutine("Shake_Cor");
            GetComponent<SpriteRenderer>().sprite = damaged_Sprite;
        }
        else if(life == 0) {
            Play_Effect();
            Destroy(gameObject);
        }
    }


    //揺れる
    private IEnumerator Shake_Cor() {
        Vector2 default_Pos = transform.position;
        for (float t = 0; t < 0.25f; t += 0.016f) {
            transform.position = default_Pos + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * Time.timeScale;
            yield return null;
        }
        transform.position = default_Pos;
    }


    //消滅エフェクト
    private void Play_Effect() {
        if(transform.childCount == 0) {
            return;
        }
        GameObject effect = transform.GetChild(0).gameObject;
        effect.transform.SetParent(null);
        effect.SetActive(true);
        Destroy(effect, 1.0f);
    }


}
