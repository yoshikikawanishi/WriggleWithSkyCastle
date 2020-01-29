using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2_BossMovie : MonoBehaviour {

    //スクリプト
    private MessageDisplay _message;
    //自機
    private GameObject player;
    //ネムノ
    private GameObject nemuno;

    private bool is_First_Visit = true;


    private void Awake() {
        //取得
        _message = GetComponent<MessageDisplay>();
        player = GameObject.FindWithTag("PlayerTag");
        nemuno = GameObject.Find("Nemuno");
    }


    //ボス前ムービー開始
    public void Start_Before_Boss_Movie() {
        StartCoroutine("Play_Before_Boss_Movie_Cor");
    }


    private IEnumerator Play_Before_Boss_Movie_Cor() {
        //初期設定
        player.GetComponent<PlayerController>().Set_Is_Playable(false);
        PauseManager.Instance.Set_Is_Pausable(false);
        is_First_Visit = SceneManagement.Instance.Is_First_Visit();
        BGMManager.Instance.Stop_BGM();

        //自機登場
        MoveTwoPoints player_Move = player.AddComponent<MoveTwoPoints>();
        player_Move.Change_Paramter(0.021f, 0, 0);
        player_Move.Change_Transition_Curve(AnimationCurve.Linear(0, 0, 1, 1), 0);
        player.GetComponent<PlayerController>().Change_Animation("DashBool");
        player_Move.Start_Move(new Vector3(-120f, -83f), 0);
        yield return new WaitUntil(player_Move.End_Move);
        player.GetComponent<PlayerController>().Change_Animation("IdleBool");

        //会話
        if (is_First_Visit) {
            _message.Start_Display("NemunoText", 1, 1);
            yield return new WaitUntil(_message.End_Message);
        }

        //終了設定
        player.GetComponent<PlayerController>().Set_Is_Playable(true);
        PauseManager.Instance.Set_Is_Pausable(true);

        //戦闘開始
        nemuno.GetComponent<NemunoController>().Start_Battle();
        BGMManager.Instance.Change_BGM(BGMManager.Instance.stage2_Boss_BGM);
    }


    public void Start_Clear_Movie() {
        Debug.Log("TODO:Clear Movie");
    }
}
