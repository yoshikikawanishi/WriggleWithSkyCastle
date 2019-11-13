using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MBLDefine;

public class PauseManager : SingletonMonoBehaviour<PauseManager> {

    public enum STATE {
        normal,
        pause,
    }
    private STATE state = STATE.normal;

    private float time_Scale_Before_Pause = 1.0f;

    private bool is_Pausable = true;


	// Use this for initialization
	void Start () {
        //シーン読み込みのデリケート
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
	
	// Update is called once per frame
	void Update () {
        if (!is_Pausable) {
            return;
        }
        if (InputManager.Instance.GetKeyDown(Key.Pause)) {
            if(state == STATE.normal) {
                Pause_Game();
            }
            else if(state == STATE.pause) {
                Release_Pause_Game();
            }
        }
	}


    //シーン読み込み時に呼ばれる関数
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //ポーズのバグ防止用
        state = STATE.normal;
        Time.timeScale = 1;
        Set_Is_Pausable(true);
    }


    //ポーズ
    private void Pause_Game() {
        if(Time.timeScale == 0) {
            Debug.Log("Can't_Pause");
            return;
        }
        state = STATE.pause;
        time_Scale_Before_Pause = Time.timeScale;
        Time.timeScale = 0;
        //自機の操作無効化
        GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>().Set_Is_Playable(false);
    }

    
    //ポーズ解除
    private void Release_Pause_Game() {
        state = STATE.normal;
        Time.timeScale = time_Scale_Before_Pause;
        //自機の操作有効化
        GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>().Set_Is_Playable(true);
    }


    //Setter
    public void Set_Is_Pausable(bool is_Pausable) {
        this.is_Pausable = is_Pausable;
    }
}
