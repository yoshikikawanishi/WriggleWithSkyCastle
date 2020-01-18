using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoShoot : MonoBehaviour {

    [SerializeField] private ShootSystem slash_Shotgun_Odd;
    [SerializeField] private ShootSystem slash_Shootgun_Forward;


	// Use this for initialization
	void Start () {
		
	}
	
	
    //遠距離斬撃用のショットガン
    public void shoot_Shotgun() {       
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null)
            return;

        //自機が前方にいたら自機狙い 
        if ((player.transform.position.x - transform.position.x) * transform.parent.localScale.x < 0)
            slash_Shotgun_Odd.Shoot();
        //それ以外は前方に出す
        else {
            if (transform.parent.localScale.x > 0) {
                slash_Shootgun_Forward.center_Angle_Deg = 180f;
            }
            else {
                slash_Shootgun_Forward.center_Angle_Deg = 0;
            }
            slash_Shootgun_Forward.Shoot();
        }
    }
}
