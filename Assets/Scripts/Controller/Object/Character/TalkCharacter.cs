using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkCharacter : MonoBehaviour {

    
    //スクリプト
    protected MessageDisplay _message;

    //吹き出し
    private GameObject mark_Up_Baloon;
    [SerializeField] private Vector3 baloon_Pos;

    //読み込むテキストファイル
    public string fileName;
    public int start_ID;
    public int end_ID;

    //会話中か
    protected bool is_Talking = false;
    //会話終了検知用
    protected bool end_Talk = false;


    //Start
    protected void Start() {
        //スクリプト
        _message = gameObject.AddComponent<MessageDisplay>();
        //吹き出しの生成
        mark_Up_Baloon = Instantiate(Resources.Load("Object/MarkUpBaloon") as GameObject, transform);
        mark_Up_Baloon.transform.position += baloon_Pos;        
        mark_Up_Baloon.SetActive(false);
    }


    protected void Update() {
        if (mark_Up_Baloon.activeSelf) {
            //会話開始
            if (!is_Talking && Input.GetAxisRaw("Vertical") > 0) {
                StartCoroutine("Talk");
            }
        }
    }


    //OnTriggerStay
    /*
    protected void OnTriggerStay2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            if (!is_Talking && Input.GetAxisRaw("Vertical") > 0) {
                StartCoroutine("Talk");
            }
        }
    }
    */


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            mark_Up_Baloon.SetActive(true);            
        }
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            mark_Up_Baloon.SetActive(false);
        }
    }


    //会話
    virtual protected IEnumerator Talk() {
        is_Talking = true;
        end_Talk = false;
        
        GameObject player = GameObject.FindWithTag("PlayerTag");
        PlayerController player_Controller = player.GetComponent<PlayerController>();

        //会話開始
        mark_Up_Baloon.SetActive(false);
        if (player_Controller.Get_Is_Playable()) {
            //自機を止める
            player_Controller.Set_Is_Playable(false);
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            player_Controller.Change_Animation("IdleBool");
            //ポーズ禁止
            PauseManager.Instance.Set_Is_Pausable(false);
            //メッセージ表示
            _message.Start_Display(fileName, start_ID, end_ID);
            yield return new WaitUntil(_message.End_Message);
            //終了
            yield return new WaitForSeconds(0.1f);
            player_Controller.Set_Is_Playable(true);
            PauseManager.Instance.Set_Is_Pausable(true);
        }

        mark_Up_Baloon.SetActive(true);

        Action_In_End_Talk();
        end_Talk = true;
        is_Talking = false;
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
}
