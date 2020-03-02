using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBLDefine;

public class Hina : TalkCharacter {

    //厄、毛玉ザコ敵生成用
    [SerializeField] private HinaDisaster hina_Disaster;
    //選択ボタン押下後の会話範囲
    private readonly int AFTER_EVENT_START_ID = 4;
    private readonly int AFTER_EVENT_END_ID = 4;

    private GameObject disaster_Effect;


    protected override float Action_Before_Talk() {
        if(start_ID == 1)
            Infect_Disaster_Effect_To_Player();
        return 0;
    }

    protected override void Action_In_End_Talk() {
        //えんがちょイベント終了後
        if (start_ID == 1) {            
            Change_Message_Status("HinaText", AFTER_EVENT_START_ID, AFTER_EVENT_END_ID);
        }        
    }


    //ボタン関数
    //はいボタン押下時
    public void Yes_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            Debug.Log("えんがちょした");
            Delete_Disaster_Effect_In_Player();
        }
    }


    //いいえボタン押下時
    public void No_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            hina_Disaster.Start_Generate();
            Debug.Log("えんがちょしなかった");
        }
    }


    //自機に厄エフェクトをまとわせる
    private void Infect_Disaster_Effect_To_Player() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        GameObject effect = transform.Find("HinaDisaster").gameObject;
        disaster_Effect = Instantiate(effect, player.transform);
    }

    //自機の厄エフェクトをはずす
    private void Delete_Disaster_Effect_In_Player() {
        Destroy(disaster_Effect);
    }
}
