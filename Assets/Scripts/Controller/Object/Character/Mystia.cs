using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mystia : TalkCharacter {

    private Stage1_1Scene.Rumia now_Rumia_State;


    //会話のパターンを変える
    private void Change_Message() {
        now_Rumia_State = Stage1_1Scene.Instance.rumia_State;
        switch (now_Rumia_State) {
            case Stage1_1Scene.Rumia.not_find:
                Change_Message_Status("MystiaText", 3, 3);
                break;
            case Stage1_1Scene.Rumia.find:
                Change_Message_Status("MystiaText", 4, 6);
                break;
        }
    }

    protected override float Action_Before_Talk() {
        if(talk_Count != 1) {
            Change_Message();
        }
        return 0;
    }

    protected override void Action_In_End_Talk() {       
        StartCoroutine("Action_After_Talking_Cor");        
    }


    //会話終了時
    private IEnumerator Action_After_Talking_Cor() {
        //収集アイテム出す
        if (transform.childCount > 1 && start_ID == 1) {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).SetParent(null);            
            yield break;
        }
       
        //以降ルーミアに関して
        if (now_Rumia_State == Stage1_1Scene.Rumia.not_find) {
            yield break;        
        }
        //当たり判定消す
        GetComponent<BoxCollider2D>().enabled = false;
        //飛び去る
        MoveTwoPoints _move = GetComponent<MoveTwoPoints>();        
        _move.Start_Move(transform.position + new Vector3(-300f, 150f));
        GetComponent<Animator>().SetTrigger("FlyTrigger");
        mark_Up_Baloon.SetActive(false);
        //アイテムを出す
        if (now_Rumia_State == Stage1_1Scene.Rumia.find) {
            Put_Out_Score(15);
        }
        //ルーミアの隣に配置
        yield return new WaitForSeconds(5.0f);        
        transform.position = new Vector3(824f, 18f, 0);
    }
    

}
