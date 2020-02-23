﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour {

    private bool is_Hit_Attack = false;
    private BoxCollider2D _collider;
    private GameObject player;

    private List<string> hit_Attack_Tag_List = new List<string> {
        "EnemyTag",
        "SandbackTag",
        "SandbackGroundTag"
    };


    private void Awake() {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        player = transform.parent.gameObject;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (string tag in hit_Attack_Tag_List) {
            if (collision.tag == tag && !is_Hit_Attack) {
                is_Hit_Attack = true;
            }
        }
    }


    public bool Hit_Trigger() {
        if (is_Hit_Attack) {
            is_Hit_Attack = false;
            return true;
        }
        return false;
    }


    //攻撃の生成
    public void Make_Collider_Appear(float lifeTime) {        
        is_Hit_Attack = false;        
        GetComponent<BoxCollider2D>().enabled = true;
        Change_Size();
        Change_Tag();
        Play_Animation();
        Invoke("Make_Collider_Disappear", lifeTime);
    }


    public void Make_Collider_Disappear() {
        GetComponent<BoxCollider2D>().enabled = false;
    }


    //オプションによるサイズの変更
    private void Change_Size() {        
        switch (PlayerManager.Instance.Get_Option()) {
            case PlayerManager.Option.none:         Set_Size(1.0f, new Vector2(10, 0)); break;
            case PlayerManager.Option.bee:          Set_Size(0.6f, new Vector2(7, 0));  break;
            case PlayerManager.Option.butterfly:    Set_Size(1.0f, new Vector2(10, 0)); break;
            case PlayerManager.Option.mantis:       Set_Size(1.5f, new Vector2(14, 0)); break;
        }
    }

    private void Set_Size(float scale, Vector2 position) {
        int direction = transform.localScale.x.CompareTo(0);
        transform.localScale = new Vector3(direction, 1, 1) * scale;
        transform.localPosition = position;
        //のビールアーム入手後範囲広げる
        if (CollectionManager.Instance.Is_Collected("Nitori")) {
            Debug.Log("Test");
            transform.localScale *= 1.2f;
        }
    }


    //オプションとパワーによるアニメーションの変更
    private void Play_Animation() {
        int power = PlayerManager.Instance.Get_Power();
        PlayerManager.Option option = PlayerManager.Instance.Get_Option();

        if (option == PlayerManager.Option.none || option == PlayerManager.Option.mantis) {
            if (power < 32) {
                GetComponent<Animator>().SetTrigger("AttackTrigger");
            }
            else if (power < 64) {
                GetComponent<Animator>().SetTrigger("AttackTrigger2");
            }
            else {
                GetComponent<Animator>().SetTrigger("AttackTrigger3");
            }
        }
        else if (option == PlayerManager.Option.spider) {
            if (power < 32) {
                GetComponent<Animator>().SetTrigger("BlueAttackTrigger");
            }
            else if (power < 64) {
                GetComponent<Animator>().SetTrigger("BlueAttackTrigger2");
            }
            else {
                GetComponent<Animator>().SetTrigger("BlueAttackTrigger3");
            }
        }
        else {
            if (power < 32) {
                GetComponent<Animator>().SetTrigger("YellowAttackTrigger");
            }
            else if (power < 64) {
                GetComponent<Animator>().SetTrigger("YellowAttackTrigger2");
            }
            else {
                GetComponent<Animator>().SetTrigger("YellowAttackTrigger3");
            }
        }
    }


    //オプションによるタグの変更
    private void Change_Tag() {
        switch (PlayerManager.Instance.Get_Option()) {
            case PlayerManager.Option.none:         Set_Tag("PlayerAttackTag"); break;
            case PlayerManager.Option.bee:          Set_Tag("PlayerAttackTag"); break;
            case PlayerManager.Option.butterfly:    Set_Tag("PlayerButterflyAttackTag"); break;
            case PlayerManager.Option.mantis:       Set_Tag("PlayerAttackTag"); break;
            case PlayerManager.Option.spider:       Set_Tag("PlayerSpiderAttackTag");    break;
        }
    }

    private void Set_Tag(string tag) {
        gameObject.tag = tag;
    }


    /// <summary>
    /// 当たり判定の範囲を返す
    /// </summary>
    /// <returns>長さ２のVector2配列、要素0 : 左下の座標、要素1 : 右上の座標</returns>
    public Vector2[] Get_Collision_Range() {        
        Vector2 center = (Vector2)transform.position + _collider.offset * player.transform.localScale.x;
        Vector2 left_Bottom = center - new Vector2(_collider.size.x, _collider.size.y) * (Vector2)transform.localScale / 2;
        Vector2 right_Top = center + new Vector2(_collider.size.x, _collider.size.y) * (Vector2)transform.localScale / 2;        
        return new Vector2[2] { left_Bottom, right_Top };
    }
}
