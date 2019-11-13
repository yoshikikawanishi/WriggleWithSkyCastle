using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour {

    private bool is_Hit_Attack = false;    


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "EnemyTag" && !is_Hit_Attack) {
            is_Hit_Attack = true;
        }        
    }


    public bool Hit_Trigger() {
        if (is_Hit_Attack) {
            is_Hit_Attack = false;
            return true;
        }
        return false;
    }


    public void Make_Collider_Appear(float lifeTime) {        
        is_Hit_Attack = false;        
        GetComponent<BoxCollider2D>().enabled = true;        
        Play_Animation();
        Invoke("Make_Collider_Disappear", lifeTime);
    }

    public void Make_Collider_Disappear() {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void Play_Animation() {
        int power = PlayerManager.Instance.Get_Power();
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
}
