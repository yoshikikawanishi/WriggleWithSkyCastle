using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AyaMovie : MonoBehaviour {

    //画面エフェクト用
    [SerializeField] private AyaCameraFrame camera_Frame_Effect;

    //セリフのID
    [SerializeField] private Vector2Int[]   start_Message_ID        = new Vector2Int[3];
    [SerializeField] private float[]        on_The_Way_Message_Line = new float[2];
    [SerializeField] private Vector2Int[]   on_The_Way_Message_ID   = new Vector2Int[2];    
    [SerializeField] private Vector2Int[]   damaged_Message_ID      = new Vector2Int[3];

    private GameObject main_Camera;
    private MessageDisplay _message;

    //何回目のムービーか
    private int movie_Count = 0;
    //自機のライフ確認用
    private int player_Life;
    private int damaged_Count = 0;        

    
    void Awake () {        
        _message = GetComponent<MessageDisplay>();        
	}

    private void Start() {
        main_Camera = GameObject.FindWithTag("MainCamera");
        if (on_The_Way_Message_Line.Length != on_The_Way_Message_ID.Length)
            Debug.Log("OnTheWayMessage[] Size Error");
    }


    //ムービーを開始する
    public void Play_Aya_Movie() {
        //ムービーの回数を取得
        if (!PlayerPrefs.HasKey("Aya")) {
            PlayerPrefs.SetInt("Aya", 0);
        }
        movie_Count = PlayerPrefs.GetInt("Aya") + 1;
        PlayerPrefs.SetInt("Aya", movie_Count);        
        //ムービー開始
        StartCoroutine("Aya_Movie_Cor");
    }


    //ムービー本体
    private IEnumerator Aya_Movie_Cor() {
        //初期設定
        damaged_Count = 0;
        player_Life = PlayerManager.Instance.Get_Life();
        //カメラエフェクト
        camera_Frame_Effect.Appear();

        //開始セリフ
        switch (movie_Count) {
            case 1: Display_Message(start_Message_ID[0]); break;
            case 2: Display_Message(start_Message_ID[1]); break;
            case 3: Display_Message(start_Message_ID[2]); break;
        }
        yield return new WaitUntil(_message.End_Message);

        //一定x座標を越えた時のセリフ
        StartCoroutine("On_The_Way_Message_Cor", 0);
        //自機被弾時セリフ
        StartCoroutine("Player_Damaged_Movie_Cor");        
    }


    //一定のx座標を超えた時のセリフ
    private IEnumerator On_The_Way_Message_Cor(int line_Index) {        
        //待つ
        while(main_Camera.transform.position.x < on_The_Way_Message_Line[line_Index]) {
            yield return null;
        }
        //セリフ
        Display_Message(on_The_Way_Message_ID[line_Index]);
        yield return new WaitUntil(_message.End_Message);
        //次の道中セリフを待つ
        if(line_Index + 1 < on_The_Way_Message_Line.Length)
            StartCoroutine("On_The_Way_Message", line_Index + 1);
    }


    //自機被弾時セリフ    
    private IEnumerator Player_Damaged_Movie_Cor() {           
        //待つ
        while (true) {
            if (Is_Player_Damaged()) {
                damaged_Count++;
                break;    
            }
            yield return null;
        }
        //セリフ
        Display_Message(damaged_Message_ID[damaged_Count - 1]);
        yield return new WaitUntil(_message.End_Message);
        //次の被弾時セリフを待つ        
        if(damaged_Count < damaged_Message_ID.Length)
            StartCoroutine("Player_Damaged_Movie_Cor");                
    }


    //自機のライフが減ったときにtrueを返す    
    private bool Is_Player_Damaged() {
        if(PlayerManager.Instance.Get_Life() < player_Life) {
            player_Life = PlayerManager.Instance.Get_Life();
            return true;
        }
        return false;
    }



    private void Display_Message(Vector2Int ID) {
        _message.Start_Display_Auto("AyaText", ID.x, ID.y, 1, 0.05f, false);
    }
}
