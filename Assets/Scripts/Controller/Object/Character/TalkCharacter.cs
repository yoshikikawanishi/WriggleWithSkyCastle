﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCharacter : MonoBehaviour {
    
    //スクリプト
    protected MessageDisplay _message;

    //読み込むテキストファイル
    public string fileName;
    public int start_ID;
    public int end_ID;
    //会話マークの位置
    public Vector2 baloon_Pos = new Vector2(8f, 16f);

    //会話中か
    protected bool is_Talking = false;
    //会話終了検知用
    protected bool end_Talk = false;
    //会話マーク
    protected GameObject mark_Up_Baloon;
    //会話の回数
    protected int talk_Count = 0;

    private List<string> talk_Tags = new List<string> {
        "PlayerAttackTag",
        "PlayerChargeAttackTag",               
    };    


    //Start
    protected void Start() {
        //スクリプト取得
        _message = GetComponent<MessageDisplay>();
        if(_message == null)
            _message = gameObject.AddComponent<MessageDisplay>();
        //会話マークの生成
        mark_Up_Baloon = Instantiate(Resources.Load("Object/MarkUpBaloon") as GameObject);
        mark_Up_Baloon.transform.position = transform.position + (Vector3)baloon_Pos;
        mark_Up_Baloon.transform.SetParent(transform);
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (is_Talking)
            return;
        foreach(string tag in talk_Tags) {
            if(collision.tag == tag) {
                StartCoroutine("Talk");
            }
        }
    }


    //会話
    virtual protected IEnumerator Talk() {
        is_Talking = true;
        end_Talk = false;
        
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();
        PlayerBodyCollision player_Collision = player.GetComponentInChildren<PlayerBodyCollision>();

        talk_Count++;

        //会話開始        
        if (player_Controller.Get_Is_Playable()) {
            //自機を止める
            player_Controller.Set_Is_Playable(false);
            yield return null;
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player_Controller.Change_Animation("IdleBool");
            //自機無敵化
            player_Collision.Become_Invincible();
            //ポーズ禁止
            PauseManager.Instance.Set_Is_Pausable(false);

            yield return new WaitForSeconds(Action_Before_Talk());
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

            //メッセージ表示
            _message.Start_Display(fileName, start_ID, end_ID);
            yield return new WaitUntil(_message.End_Message);
            //終了
            yield return new WaitForSeconds(0.1f);
            player_Controller.Set_Is_Playable(true);
            player_Collision.Release_Invincible();
            PauseManager.Instance.Set_Is_Pausable(true);
        }        

        Action_In_End_Talk();
        end_Talk = true;
        is_Talking = false;
    }


    /// <summary>
    /// 会話開始前に行う処理
    /// </summary>
    /// <returns>処理の後会話開始までの時間[s]</returns>
    protected virtual float Action_Before_Talk() {        
        return 0;
    }

    /// <summary>
    /// 会話終了時に行う処理
    /// </summary>
    protected virtual void Action_In_End_Talk() {
    }


    //会話終了検知用
    public bool End_Talk() {
        if (end_Talk) {
            end_Talk = false;
            return true;
        }
        return false;
    }


    /// <summary>
    /// 会話ステータス変更
    /// </summary>
    /// <param name="file_Name"></param>
    /// <param name="start_ID"></param>
    /// <param name="end_ID"></param>
    public void Change_Message_Status(string file_Name, int start_ID, int end_ID) {
        this.fileName = file_Name;
        this.start_ID = start_ID;
        this.end_ID = end_ID;
    }


    /// <summary>
    /// スコアの放出
    /// </summary>
    /// <param name="num">放出する数</param>
    protected void Put_Out_Score(int num) {
        gameObject.AddComponent<PutOutSmallItems>().Put_Out_Item(0, num);
    }

    /// <summary>
    /// 回復アイテムの放出
    /// </summary>
    protected void Put_Out_Life_Item() {
        ObjectPoolManager.Instance.Create_New_Pool(Resources.Load("Object/LifeUpItem") as GameObject, 2);
        var life_Item = ObjectPoolManager.Instance.Get_Pool("LifeUpItem").GetObject();
        life_Item.transform.position = transform.position + new Vector3(0, 16f);
    }


    /// <summary>
    /// 収集アイテムの放出、１番目の子要素を出す
    /// </summary>
    protected void Put_Out_Collection_Box() {
        var child = transform.GetChild(0);
        if (child.GetComponent<CollectionBox>() != null) {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).SetParent(null);
        }
    }
    
    
}
