using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrog : MonoBehaviour {

    private BossEnemy boss_Enemy;

    private bool is_Start_Battle = false;


	// Use this for initialization
	void Start () {
        boss_Enemy = GetComponent<BossEnemy>();
	}
	

	// Update is called once per frame
	void Update () {
        //戦闘
        if (is_Start_Battle) {

        }
        //クリア
        if (boss_Enemy.Clear_Trigger()) {
            Stop_Battle();
            BigFrogMovie.Instance.Start_Clear_Movie();
        }	
	}


    //戦闘開始時の処理
    public void Start_Battle() {
        Debug.Log("StartBattle");
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }


    //戦闘終了時の処理
    public void Stop_Battle() {
        Debug.Log("StopBattle");
    }
}
