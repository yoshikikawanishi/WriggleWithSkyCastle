using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3_2Scene : MonoBehaviour {

    private GameObject player;

    private bool is_Passed_Middle_Point = false;
    private bool is_Passed_Final_Point = false;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        //初回時フェードイン
        if (SceneManagement.Instance.Is_First_Visit()) {
            FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.01f);
        }

	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
            return;

        if (player.transform.position.x > 4600f) {
            if (!is_Passed_Final_Point) {
                is_Passed_Final_Point = true;
                BackGroundEffector.Instance.Start_Change_Color(new Color(0.7f, 0.7f, 0.7f), 0.02f);
            }
        }
        else if (player.transform.position.x > 2664f) {
            if (!is_Passed_Middle_Point) {
                is_Passed_Middle_Point = true;
                BackGroundEffector.Instance.Start_Change_Color(new Color(0.4f, 0.4f, 0.4f), 0.02f);
            }
        }
	}
}
