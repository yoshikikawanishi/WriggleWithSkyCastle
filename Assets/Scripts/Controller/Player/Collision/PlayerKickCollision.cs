using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKickCollision : MonoBehaviour {
    
    private bool is_Hit_Kick = false;    


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "EnemyTag" && !is_Hit_Kick) {
            is_Hit_Kick = true;
        }    
    }


    public bool Hit_Trigger() {
        if (is_Hit_Kick) {
            is_Hit_Kick = false;
            return true;
        }
        return false;
    }


    public void Make_Collider_Appear() {
        GetComponent<CircleCollider2D>().enabled = true;
        Play_Animation();
    }

    public void Make_Collider_Disappear() {
        GetComponent<CircleCollider2D>().enabled = false;
    }


    private void Play_Animation() {
        GetComponent<Animator>().SetTrigger("KickTrigger");
        int power = PlayerManager.Instance.Get_Power();        
        if (power >= 64) {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("KickTrigger2");            
        }
        else if (power >= 32) {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("KickTrigger1");
        }
    }

}
