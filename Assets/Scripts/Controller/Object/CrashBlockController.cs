using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashBlockController : MonoBehaviour {


    private List<string> destroyer_Tag_List = new List<string> {
        "PlayerBulletTag",
        "PlayerChargeBulletTag",
        "PlayerAttackTag"
    };


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        foreach(string tag in destroyer_Tag_List) {
            if(collision.tag == tag) {
                Play_Effect();
                Destroy(gameObject);
            }
        }        
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        foreach (string tag in destroyer_Tag_List) {
            if (collision.gameObject.tag == tag) {
                Play_Effect();
                Destroy(gameObject);
            }
        }
    }


    //エフェクト
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
