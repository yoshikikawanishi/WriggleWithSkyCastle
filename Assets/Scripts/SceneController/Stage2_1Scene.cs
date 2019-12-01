using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_1Scene : MonoBehaviour {

	// Use this for initialization
	void Start () {        
        if (SceneManagement.Instance.Is_First_Visit()) {
            //フェードイン
            FadeInOut.Instance.Start_Fade_In(new Color(1, 1, 1), 0.015f);
            
            //セーブ
            DataManager.Instance.Save_Player_Data(new Vector2(-134f, 46f));
            GameObject.Find("Canvas").GetComponent<GameUIController>().Display_Save_Text();

            //自機をカブトムシに乗せる
            GameObject player = GameObject.FindWithTag("PlayerTag");
            PlayerGettingOnBeetle player_Ride_Beetle = player.GetComponent<PlayerGettingOnBeetle>();
            player_Ride_Beetle.Get_On_Beetle(false);
        }

        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
