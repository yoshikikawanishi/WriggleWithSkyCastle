﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1_BossMovie : MonoBehaviour {

    //スクリプト
    private MessageDisplay _message;
    //自機
    private GameObject player;
    //ラルバ
    private GameObject larva;

    private bool is_First_Visit = true;


    private void Awake() {
        //取得
        _message = GetComponent<MessageDisplay>();
        player = GameObject.FindWithTag("PlayerTag");
        larva = GameObject.Find("Larva");
    }
	

    //ボス前ムービー開始
    public void Start_Before_Boss_Movie() { 
        StartCoroutine("Play_Before_Boss_Movie_Cor");
    }

    //ボス前ムービー
    private IEnumerator Play_Before_Boss_Movie_Cor() {
        //初期設定
        player.GetComponent<PlayerController>().Set_Is_Playable(false);
        PauseManager.Instance.Set_Is_Pausable(false);
        is_First_Visit = SceneManagement.Instance.Is_First_Visit();

        //フェードイン
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.05f);
        yield return new WaitForSeconds(1.0f);

        //セリフ1
        if (is_First_Visit) {
            _message.Start_Display("LarvaText", 1, 1);
            yield return new WaitUntil(_message.End_Message);
        }

        //ラルバ登場
        MoveTwoPoints _move = larva.GetComponent<MoveTwoPoints>();
        _move.Set_Speed(0.015f, 1.2f, 0.85f);
        _move.Start_Move(new Vector3(128f, 0), -16f, false);
        yield return new WaitUntil(_move.End_Move);

        //セリフ2
        if (is_First_Visit) {
            _message.Start_Display("LarvaText", 1, 1);
            yield return new WaitUntil(_message.End_Message);
        }

        //終了設定
        player.GetComponent<PlayerController>().Set_Is_Playable(true);
        PauseManager.Instance.Set_Is_Pausable(true);

        //戦闘開始
        larva.GetComponent<LarvaController>().Start_Battle();
    }

}
