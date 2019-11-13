using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageDisplay : MonoBehaviour {
    
    //テキストの複数列を入れる2次元配列
    public string[,] textWords;
    //テキスト内の行数を取得する変数
    private int rowLength = 0;
    //テキスト内の列数を取得する変数
    private int columnLength = 0;

    //表示する用のパネル
    GameObject messagePanel;
    //メッセージ表示のテキストコンポーネント
    private Text messageText;
    //キャラ名表示のテキストコンポーネント
    private Text nameText;

    //表示するID番号
    private int start_ID = 1;
    private int end_ID = 1;

    //表示終了、
    private bool endMessage = false;
    
    //メッセージ表示の速度
    private float textSpeed = 0.07f;
   

    // Update is called once per frame
    private void Update() {
        //表示スピード
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            textSpeed = 0.005f;
        }
        if (InputManager.Instance.GetKeyUp(MBLDefine.Key.Jump)) {
            textSpeed = 0.07f;
        }
    }


    //表示開始
    public void Start_Display(string fileName, int start_ID, int end_ID) {
        //テキストファイルの読み込み
        Read_Text(fileName);
        //セリフ枠の表示、テキスト、アイコンの取得
        Display_Panel();
        //番号の代入
        this.start_ID = start_ID;
        this.end_ID = end_ID;
        //セリフの表示
        StartCoroutine("Print_Message");
    }

    //表示開始
    public void Start_Display_Auto(string fileName, int start_ID, int end_ID, float waitingTime, float speed) {
        //テキストファイルの読み込み
        Read_Text(fileName);
        //セリフ枠の表示、テキスト、アイコンの取得
        Display_Panel();
        //番号の代入
        this.start_ID = start_ID;
        this.end_ID = end_ID;
        //セリフの表示
        StartCoroutine(Print_Message_Auto(waitingTime, speed));
    }



    //テキストファイルの読み込み
    private void Read_Text(string fileName) {
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(fileName);
        textWords = text.textWords;
        columnLength = text.columnLength;
        rowLength = text.rowLength;
    }


    //メッセージパネルの表示
    private void Display_Panel() {
        GameObject canvas = GameObject.Find("Canvas");
        messagePanel = canvas.transform.Find("MessagePanel").gameObject;
        messagePanel.SetActive(true);
        //テキストを取得
        messageText = messagePanel.transform.GetChild(0).GetComponent<Text>();
        messageText.text = "";
        //キャラ名表示のテキストを取得
        nameText = messagePanel.transform.GetChild(1).GetComponent<Text>();
        nameText.text = "";
    }


    //メッセージ表示
    private IEnumerator Print_Message() {
        //効果音の取得
        AudioSource sound = messagePanel.GetComponent<AudioSource>();
        //1行ずつ表示
        for (int i = start_ID; i <= end_ID; i++) {
            //名前とアイコン
            nameText.text = textWords[i, 1];
            //セリフ
            int lineLength = textWords[i, 3].Length;
            for(int j = 0; j < lineLength; j++) {
                if (textWords[i, 3][j] == '/') {
                    messageText.text += "\n";
                }
                else {
                    messageText.text += textWords[i, 3][j];
                    sound.Play();
                }
                for (float t = 0; t < textSpeed; t += 0.016f) { yield return null; }
            }
            //1行分表示後決定が押されるのを待つ
            yield return new WaitUntil(Wait_Input_Z);
            //次の行へ
            messageText.text = "";
        }
        //表示終了
        messagePanel.SetActive(false);
        endMessage = true;
    }


    //Zが入力されるのを待つ
    private bool Wait_Input_Z() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            return true;
        }
        return false;
    }


    //メッセージ表示
    private IEnumerator Print_Message_Auto(float waitingTime, float speed) {
        //効果音の取得
        AudioSource sound = messagePanel.GetComponent<AudioSource>();
        //1行ずつ表示
        for (int i = start_ID; i <= end_ID; i++) {
            //名前とアイコン
            nameText.text = textWords[i, 1];
            //セリフ
            int lineLength = textWords[i, 3].Length;
            for (int j = 0; j < lineLength; j++) {
                if (textWords[i, 3][j] == '/') {
                    messageText.text += "\n";
                }
                else {
                    messageText.text += textWords[i, 3][j];
                    sound.Play();
                }
                for (float t = 0; t < speed; t += 0.016f) { yield return null; }
            }
            //1行分表示後決定が押されるのを待つ
            yield return new WaitForSeconds(waitingTime);
            //次の行へ
            messageText.text = "";
        }
        //表示終了
        messagePanel.SetActive(false);
        endMessage = true;
    }


    //メッセージ表示終了を他スクリプトで検知する用
    public bool End_Message() {
        if (endMessage) {
            endMessage = false;
            return true;
        }
        return false;
    }
    
}
