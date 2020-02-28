using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBLDefine;

public class Hina : TalkCharacter {

    //選択ボタン押下後の会話範囲
    private readonly int AFTER_EVENT_START_ID = 4;
    private readonly int AFTER_EVENT_END_ID = 4;


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
        }
    }

    //いいえボタン押下時
    public void No_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            Debug.Log("えんがちょしなかった");
        }
    }
}
