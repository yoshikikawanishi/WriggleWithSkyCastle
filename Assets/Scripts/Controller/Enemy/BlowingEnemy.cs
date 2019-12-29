using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class BlowingEnemy : MonoBehaviour {

    private Rigidbody2D _rigid;

    private bool is_Blowing = false;
    private bool has_Rigidbody = true;

    private List<string> clash_Tags = new List<string> {
        "GroundTag",
        "ThroughGroundTag",
        "SandbackGroundTag",
        "EnemyTag",
    };


    private void Awake() {
        _rigid = GetComponent<Rigidbody2D>();
        if (_rigid == null)
            has_Rigidbody = false;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
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
    public void Blow_Away_Vanish() {
        //ほかのスクリプトを無効にする
        ScriptController sc = new ScriptController();
        sc.Initialize(this);
        sc.Suspend();
        
        //リジッドボディ関連
        if (!has_Rigidbody) {
            _rigid = gameObject.AddComponent<Rigidbody2D>();            
        }
        _rigid.gravityScale = 32f;

        //吹き飛ぶ
        is_Blowing = true;
        transform.position += new Vector3(0, 6f);
        _rigid.velocity = new Vector2(-150f * Player_Direction(), 200f);
        Change_Tag_And_Layer("PlayerAttackTag", "PlayerLayer");        
    }

    
    //着地、壁、敵に当たるとダメージ
    private void Clash() {
        //ほかのスクリプトを有効にする
        ScriptController sc = new ScriptController();
        sc.Initialize(this);
        sc.Resume();

        //リジッドボディ関連
        if (!has_Rigidbody) {
            Destroy(GetComponent<Rigidbody2D>());
        }

        //消滅
        GetComponent<Enemy>().Vanish();        
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
