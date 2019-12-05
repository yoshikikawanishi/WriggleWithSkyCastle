using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKickCollision : MonoBehaviour {
    
    public bool is_Hit_Kick = false;

    private List<string> hit_Attack_Tag_List = new List<string> {
        "EnemyTag",
        "SandbackTag",
        "SandbackGroundTag"
    };

    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (string tag in hit_Attack_Tag_List) {
            if (collision.tag == tag && !is_Hit_Kick) {
                is_Hit_Kick = true;
            }
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
        Stop_Animation();
    }


    private void Play_Animation() {
        GetComponent<Animator>().SetBool("KickBool", true);
        int power = PlayerManager.Instance.Get_Power();        
        if (power >= 64) {
            transform.GetChild(0).GetComponent<Animator>().SetBool("KickBool2", true);            
        }
        else if (power >= 32) {
            transform.GetChild(0).GetComponent<Animator>().SetBool("KickBool1", true);
        }
    }

    private void Stop_Animation() {
        GetComponent<Animator>().SetBool("KickBool", false);
        transform.GetChild(0).GetComponent<Animator>().SetBool("KickBool1", false);
        transform.GetChild(0).GetComponent<Animator>().SetBool("KickBool2", false);
    }

}
