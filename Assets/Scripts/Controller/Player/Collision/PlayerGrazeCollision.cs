using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrazeCollision : MonoBehaviour {

    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "EnemyBulletTag") {            
            BeetlePowerManager.Instance.Increase_In_Update(60);
            PlayerManager.Instance.Add_Score(1);
        }
    }

}
