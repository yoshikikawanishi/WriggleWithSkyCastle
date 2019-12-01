using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mystia : TalkCharacter {

    private Stage1_1Scene.Rumia now_Rumia_State = Stage1_1Scene.Rumia.delete;
	
	// Update is called once per frame
	new void Update () {
        base.Update();
		if(now_Rumia_State != Stage1_1Scene.Instance.rumia_State) {
            now_Rumia_State = Stage1_1Scene.Instance.rumia_State;
            Change_Message();
        }

        if (End_Talk()) {
            StartCoroutine("Action_After_Talking_Cor");
        }
	}


    //会話のパターンを変える
    private void Change_Message() {
        switch (now_Rumia_State) {
            case Stage1_1Scene.Rumia.not_find:
                Change_Message_Status("MystiaText", 1, 5);
                break;
            case Stage1_1Scene.Rumia.find:
                Change_Message_Status("MystiaText", 7, 12);
                break;
            case Stage1_1Scene.Rumia.delete:
                Change_Message_Status("MystiaText", 14, 21);
                break;
        }
    }


    //会話終了後
    private IEnumerator Action_After_Talking_Cor() {
        //当たり判定消す
        GetComponent<BoxCollider2D>().enabled = false;

        //飛び去る
        MoveTwoPoints _move = gameObject.AddComponent<MoveTwoPoints>();
        _move.Set_Speed(0.01f, 1.05f, 0.995f); 
        _move.Start_Move(transform.position + new Vector3(-300f, 150f), 16f, false);
        GetComponent<Animator>().SetTrigger("FlyTrigger");

        //ルーミアの状態によってアイテム変更
        switch (now_Rumia_State) {
            case Stage1_1Scene.Rumia.not_find:
                Put_Out_Score(1);
                break;
            case Stage1_1Scene.Rumia.find:
                Put_Out_Score(10);
                Put_Out_Life_Item();
                break;
            case Stage1_1Scene.Rumia.delete:
                Put_Out_Score(10);
                break;
        }

        //ルーミアの隣に配置
        yield return new WaitForSeconds(5.0f);        
        transform.position = new Vector3(824f, 18f, 0);
    }
    

}
