using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukaTalk : TalkCharacter {

    //花が攻撃されると背景を暗くする
    protected override float Action_Before_Talk() {
        if(start_ID == 2) {
            BackGroundEffector.Instance.Start_Change_Color(new Color(0.9f, 0.8f, 0.8f), 0.1f);
        }
        else if(start_ID == 3) {
            BackGroundEffector.Instance.Start_Change_Color(new Color(0.7f, 0.6f, 0.6f), 0.1f);
        }
        else if(start_ID == 4) {
            BackGroundEffector.Instance.Start_Change_Color(new Color(0.5f, 0.2f, 0.2f), 0.1f);
        }
        return 0;
    }

    //４番目の会話終了後、戦闘開始
    protected override void Action_In_End_Talk() {
        if(start_ID == 4) {
            Start_Battle();
        }
    }


    //メッセージの変更
    //YukaFlowerの被弾時に呼び出す
    public void Change_Message() {
        if (start_ID == 1) {
            Change_Message_Status("YukaText", 2, 2);
        }
        else if (start_ID == 2) {
            Change_Message_Status("YukaText", 3, 3);
        }
        else if (start_ID == 3){
            Change_Message_Status("YukaText", 4, 4);
        }        
    }

    public void Start_Talk() {
        StartCoroutine(Talk());
    }


    //戦闘開始の処理
    private void Start_Battle() {
        Debug.Log("Start Battle");
        GetComponent<YukaAttack>().Start_Battle();
        mark_Up_Baloon.SetActive(false);
        Destroy(this);
    }
}
