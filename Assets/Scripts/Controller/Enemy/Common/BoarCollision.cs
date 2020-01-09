using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarCollision : EnemyCollisionDetection {

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {          
        //被弾の判定
        foreach (string key in damaged_Tag_Dictionary.Keys) {
            if (collision.tag == key) {
                if(collision.transform.position.x <= transform.position.x) {
                   GetComponent<Boar>().Guard(1);
                }
                else {
                    GetComponent<Boar>().Guard(-1);
                }
                Damaged(key);
            }
        }
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) { 
        //被弾の判定
        foreach (string key in damaged_Tag_Dictionary.Keys) {
            if (collision.gameObject.tag == key) {
                if (collision.transform.position.x <= transform.position.x) {
                    GetComponent<Boar>().Guard(1);
                }
                else {
                    GetComponent<Boar>().Guard(-1);
                }
                Damaged(key);
            }
        }
    }


    

}
