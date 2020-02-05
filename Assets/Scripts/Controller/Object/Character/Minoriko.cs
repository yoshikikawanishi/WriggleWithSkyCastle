using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minoriko : MonoBehaviour {

    [SerializeField] private GameObject normal_Shoot_Obj;
    [SerializeField] private ShootSystem potate_Shoot;
    [Space]
    [SerializeField] private float start_Potate_Shoot_Position;
    

    private GameObject player;

    private bool start_Potate_Shoot = false;
    private bool is_Visible = false;

    //ノーマルショットの時間計測用
    private float normal_Shoot_Time;
    private float NORMAL_SHOOT_SPAN = 5.5f;

	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        //初期設定
        normal_Shoot_Time = NORMAL_SHOOT_SPAN - 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
        //自機が指定の座標を過ぎたら焼き芋弾発射開始
		if(player.transform.position.x >  start_Potate_Shoot_Position && !start_Potate_Shoot) {
            start_Potate_Shoot = true;
            Start_Potate_Shoot();
        }
        //穣子が画面内に入ったらノーマルショット開始
        if (is_Visible) {
            if(normal_Shoot_Time < NORMAL_SHOOT_SPAN) {
                normal_Shoot_Time += Time.deltaTime;
            }
            else {
                normal_Shoot_Time = 0;
                Shoot_Normal();
            }
        }
	}

    private void OnBecameVisible() {
        is_Visible = true;
        Stop_Potate_Shoot();
        GetComponent<MoveTwoPoints>().Start_Move(transform.position + new Vector3(0, 80f));
    }

    private void OnBecameInvisible() {
        is_Visible = false;
    }


    //通常ショットを打つ
    private void Shoot_Normal() {
        GameObject shoot_Obj = Instantiate(normal_Shoot_Obj);
        shoot_Obj.transform.position = transform.position;
        shoot_Obj.SetActive(true);
        Destroy(shoot_Obj, 2.0f);
    }


    //焼き芋弾を開始する
    //発射する本体はカメラの子供に配置すること
    private void Start_Potate_Shoot() {
        potate_Shoot.transform.position = new Vector3(potate_Shoot.transform.position.x, Random.Range(-32f, 120f));
        potate_Shoot.Shoot();
    }

    private void Stop_Potate_Shoot() {
        potate_Shoot.Stop_Shoot();
    }
}
