using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour {

    private PlayerController player_Controller;
    private PlayerTransition player_Transition;
    private Rigidbody2D player_Rigid;

   
    private float player_Default_Speed;

    private float player_Speed_In_Hit_WaterFall = 50f;
    private float water_Fall_Power = 1000f;

    private bool is_Hit_Player = false;


	// Use this for initialization
	void Start () {
        //取得
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if(player == null) {
            Debug.Log("Player is not found");
            gameObject.SetActive(false);
            return;
        }
        player_Controller = player.GetComponent<PlayerController>();
        player_Transition = player.GetComponent<PlayerTransition>();
        player_Rigid = player.GetComponent<Rigidbody2D>();       
        player_Default_Speed = player_Transition.Get_Max_Speed();
	}
	

	// Update is called once per frame
	void Update () {
        if(player_Controller == null) 
            return;
        if (player_Controller.Get_Is_Ride_Beetle()) 
            return;
        
        //下に押さえつける
        if (is_Hit_Player) {
            player_Rigid.AddForce(new Vector2(0, -water_Fall_Power));                        
        }        
	}


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            is_Hit_Player = true;
            //移動速度
            player_Transition.Set_Max_Speed(player_Speed_In_Hit_WaterFall);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            is_Hit_Player = false;
            //移動速度
            player_Transition.Set_Max_Speed(player_Default_Speed);            
        }
    }


}
