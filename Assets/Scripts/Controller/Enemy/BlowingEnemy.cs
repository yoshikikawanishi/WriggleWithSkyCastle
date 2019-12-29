using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowingEnemy : MonoBehaviour {

    private bool is_Blowing = false;

    private List<string> blowed_Tags = new List<string> {
        "PlayerButterflyAttackTag",
    };

    private List<string> clash_Tags = new List<string> {
        "GroundTag",
        "SandbackGroundTag",
        "EnemyTag",
    };


    private void OnTriggerEnter2D(Collider2D collision) {
        //吹き飛ぶ判定
        foreach (string tag in blowed_Tags) {
            if (collision.tag == tag && !is_Blowing) {
                Blow_Away();
                is_Blowing = true;
            }
        }
        //衝突する判定
        foreach (string tag in clash_Tags) {
            if (collision.tag == tag && is_Blowing) {
                Clash();
                is_Blowing = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        //衝突する判定
        foreach (string tag in clash_Tags) {
            if (collision.gameObject.tag == tag && is_Blowing) {
                Clash();
                is_Blowing = false;
            }
        }
    }


    //吹き飛ぶ
    private void Blow_Away() {
        Rigidbody2D _rigid = GetComponent<Rigidbody2D>();
        if (_rigid == null) {
            Debug.Log("BlowingEnemy need Rigidbody2D");  // RquireComponent()は自動でアタッチされて困るからやらないこと
            return;
        }

        transform.position += new Vector3(0, 6f);
        _rigid.velocity = new Vector2(-150f * Player_Direction(), 200f);
        Change_Tag_And_Layer("PlayerAttackTag", "PlayerLayer");        
    }

    
    //着地、壁、敵に当たるとダメージ
    private void Clash() {
        Enemy enemy = GetComponent<Enemy>();
        if(enemy == null) {
            Debug.Log("BlowingEnemy need Enemy Class");
            return;
        }

        enemy.Vanish();
    }


    //自機が右にいたら1、左にいたら-1を返す
    private int Player_Direction() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null) return 0;

        int direction = (player.transform.position.x - transform.position.x).CompareTo(0);
        return direction;
    }


    //タグとレイヤーを変える
    private void Change_Tag_And_Layer(string tag, string layer) {
        gameObject.tag = tag;
        gameObject.layer = LayerMask.NameToLayer(layer);
    }
}
