using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrog : MonoBehaviour {

    [SerializeField] private ShootSystem bubble_Shoot;

    private BossEnemy boss_Enemy;
    private GameObject player;

    private bool is_Start_Battle = false;


	// Use this for initialization
	void Start () {
        boss_Enemy = GetComponent<BossEnemy>();          
	}
	

	// Update is called once per frame
	void Update () {
        //戦闘
        if (is_Start_Battle) {
            StartCoroutine("Attack_Cor");
            is_Start_Battle = false;
        }
        //クリア
        if (boss_Enemy.Clear_Trigger()) {
            Stop_Battle();
            BigFrogMovie.Instance.Start_Clear_Movie();
        }	
	}


    //戦闘開始時の処理
    public void Start_Battle() {
        is_Start_Battle = true;
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
    }


    //戦闘終了時の処理
    public void Stop_Battle() {
        Debug.Log("StopBattle");
    }


    private IEnumerator Attack_Cor() {
        Roar();
        yield return new WaitForSeconds(1.0f);
    }


    //バブルショット用
    private IEnumerator Bubble_Shoot_Cor() {
        int num = 15;
        float span = 0.1f;

        for(int i = 0; i < num; i++) {
            bubble_Shoot.max_Speed = Random.Range(15f, 130f);
            bubble_Shoot.Shoot();
            yield return new WaitForSeconds(span - Time.deltaTime);
        }
    }

    //咆哮用
    private void Roar() {
        GetComponent<Roaring>().Roar(160f, 2.5f, 5000f);
    }
}
