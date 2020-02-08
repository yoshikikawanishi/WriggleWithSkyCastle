﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minoriko : Enemy {

    [SerializeField] private GameObject normal_Shoot_Obj;
    [SerializeField] private ShootSystem normal_Potate_Shoot;
    [SerializeField] private ShootSystem screen_Potate_Shoot;
    [Space]
    [SerializeField] private float start_Potate_Shoot_Position;
    [Space]
    [SerializeField] private GameObject clear_Effect_Bomb;

    private GameObject player;

    private bool start_Potate_Shoot = false;
    private bool is_Visible = false;

    //ノーマルショットの時間計測用
    private float normal_Shoot_Time;
    private float NORMAL_SHOOT_SPAN = 8.5f;

	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        //初期設定
        normal_Shoot_Time = NORMAL_SHOOT_SPAN - 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
        //自機が自分より右にいるとき何もしない
        if (player.transform.position.x > transform.position.x)
            return;
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
                StartCoroutine("Shoot_Normal_Cor");
            }
        }
	}

    //画面内に入ったとき呼ばれる
    //ショット開始
    private void OnBecameVisible() {
        if (is_Visible || player.transform.position.x > transform.position.x)
            return;
        is_Visible = true;
        Stop_Potate_Shoot();
        GetComponent<MoveTwoPoints>().Start_Move(transform.position + new Vector3(0, 80f));
    }    


    //通常ショットと通常焼き芋弾を撃つ
    private IEnumerator Shoot_Normal_Cor() {
        GameObject shoot_Obj = Instantiate(normal_Shoot_Obj);
        shoot_Obj.transform.position = transform.position;
        shoot_Obj.SetActive(true);
        Destroy(shoot_Obj, 2.0f);

        yield return new WaitForSeconds(2.0f);        
        normal_Potate_Shoot.Shoot();        
    }


    //画面外からの焼き芋弾を開始する
    //発射する本体はカメラの子供に配置すること
    private void Start_Potate_Shoot() {
        screen_Potate_Shoot.transform.position = new Vector3(screen_Potate_Shoot.transform.position.x, Random.Range(-32f, 120f));
        screen_Potate_Shoot.Shoot();
    }

    private void Stop_Potate_Shoot() {
        screen_Potate_Shoot.Stop_Shoot();
    }


    //消滅時
    public override void Vanish() {
        Play_Vanish_Effect();
        Put_Out_Item();
        StopAllCoroutines();

        //無敵化、移動
        GetComponent<MoveTwoPoints>().Start_Move(new Vector3(transform.position.x, -80f));
        this.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");

        //エフェクト
        var effect = Instantiate(clear_Effect_Bomb);
        effect.transform.position = transform.position;
        Destroy(effect, 5.0f);
    }
}
