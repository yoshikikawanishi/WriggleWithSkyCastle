using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MBLDefine;

public class TitleSceneButton : MonoBehaviour {

    //ひな形
    /*
     if(InputManager.Instance.GetKeyDown(Key.Jump)){
        //処理
     }
     */


    //初めからボタン
    public void Start_Button_Function() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            SceneManagement.Instance.Delete_Visit_Scene();
            DataManager.Instance.Initialize_Player_Data();
            DataManager.Instance.Load_Player_Data();
        }
    }


    //続きからボタン
    public void Load_Button_Function() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            DataManager.Instance.Load_Player_Data();
        }        
    }


    //設定ボタン
    public void Setting_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            SceneManager.LoadScene("ConfigScene");
        }
    }


    //遊びからボタン
    public void Play_Guide_Button() {
        if (InputManager.Instance.GetKeyDown(Key.Jump)) {
            SceneManager.LoadScene("PlayGuideScene");
        }
    }


}
