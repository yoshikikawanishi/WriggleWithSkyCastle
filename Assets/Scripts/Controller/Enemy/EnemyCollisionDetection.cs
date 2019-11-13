using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の被弾判定を取るクラス
/// </summary>
public class EnemyCollisionDetection : MonoBehaviour {

    protected Dictionary<string, int> damaged_Tag_Dictionary = new Dictionary<string, int>() {
        {"PlayerAttackTag"  , 10 },
        {"PlayerBulletTag"  , 1 },
        {"PlayerChargeBulletTag"  , 10},
        {"PlayerTag"        , 10},
    };

    private Enemy _enemy;

    
    void Awake() {
        _enemy = GetComponent<Enemy>();
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //被弾の判定
        foreach (string key in damaged_Tag_Dictionary.Keys) {
            if (collision.tag == key) {
                Damaged(key);
            }
        }
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        //被弾の判定
        foreach (string key in damaged_Tag_Dictionary.Keys) {
            if (collision.gameObject.tag == key) {
                Damaged(key);
            }
        }
    }


    //被弾の処理
    protected virtual void Damaged(string key) {
        int damage = (int)(damaged_Tag_Dictionary[key] * Damage_Rate());
        _enemy.Damaged(damage);
    }


    //被弾判定の変更
    protected virtual void Change_Damaged_Tag_Dictionary() {
        damaged_Tag_Dictionary.Clear();
        damaged_Tag_Dictionary = new Dictionary<string, int>() {
            {"PlayerAttackTag"  , 10 },
            {"PlayerBulletTag"  , 1 },
            {"PlayerChargeBulletTag"  , 10},
            {"PlayerTag"        , 10},
        };
    }


    //自機のパワーに応じてダメージ増加
    protected float Damage_Rate() {
        int power = PlayerManager.Instance.Get_Power();
        if(power < 16) {
            return 1;
        }
        if(power < 32) {
            return 1.2f;
        }
        else if(power < 64) {
            return 1.5f;
        }
        else if(power < 128) {
            return 1.7f;
        }
        return 1.9f;
    }

}
