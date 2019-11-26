using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    [SerializeField] protected int life = 3;

    protected List<string> hit_Tag_List = new List<string> {
        "PlayerAttackTag",
        "PlayerBulletTag",
        "PlayerChargeBulletTag"
    };


    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (string tag in hit_Tag_List) {
            if(tag == collision.tag && life > 0) {
                Damaged();               
            }
        }
        if(life == 0) {
            life = -1;
            Action_In_Life_Become_Zero();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        foreach(string tag in hit_Tag_List) {
            if(tag == collision.gameObject.tag && life > 0) {
                Damaged();
            }
        }
        if(life == 0) {
            life = -1;
            Action_In_Life_Become_Zero();
        }
    }


    /// <summary>
    /// 被弾時の処理、ライフの減少、点滅
    /// </summary>
    protected virtual void Damaged() {
        life--;        
        StartCoroutine("Blink");        
    }


    /// <summary>
    /// ライフが0になったときの処理
    /// </summary>
    protected virtual void Action_In_Life_Become_Zero() {
    }


    //点滅
    private IEnumerator Blink() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        if(_sprite == null) {
            yield break;
        }
        _sprite.color = new Color(1, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.3f);
        _sprite.color = new Color(1, 1, 1);
    }

}
