﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoAttack : MonoBehaviour {

    //コンポーネント
    private SpriteRenderer _sprite;
    private NemunoController _controller;
    private NemunoShoot _shoot;
    private NemunoBarrier _barrier;
    private NemunoSoundEffect _sound;
    private MoveTwoPoints _move_Two_Points;
    //自機
    private GameObject player;

    private enum AttackKind {
        close_Slash,
        long_Slash,
        barrier,
        jump_Slash
    }  

    private bool[] start_Phase = { true, true };


    private void Awake() {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
        _controller = GetComponent<NemunoController>();
        _shoot = GetComponentInChildren<NemunoShoot>();
        _barrier = GetComponentInChildren<NemunoBarrier>();
        _sound = GetComponentInChildren<NemunoSoundEffect>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();

        _barrier.gameObject.SetActive(false);

        player = GameObject.FindWithTag("PlayerTag");        
    }


    #region Phase1
    //==========================================フェーズ１===================================
    public void Phase1() {
        if (start_Phase[0]) {
            start_Phase[0] = false;
            _controller.Play_Battle_Effect();
            StartCoroutine("Phase1_Cor");
        }
    }

    private IEnumerator Phase1_Cor() {                   
        //Aメロ(約22秒)
        for (int i = 0; i < 4; i++) {
            //移動
            for (int j = 1; j <= 2; j++) {
                float distance = ((int)Random.Range(0, 2) - 0.5f) * 80f;
                StartCoroutine("Dash_Cor", distance);
                yield return new WaitForSeconds(0.7f);
            }
            yield return new WaitForSeconds(0.5f);

            //攻撃            
            switch (i % 3) {
                case 0:
                    Jump_Next_Player();
                    yield return new WaitForSeconds(1.1f);
                    StartCoroutine("Close_Slash_Cor");
                    yield return new WaitForSeconds(1.3f);
                    break;
                case 1:
                    StartCoroutine("Back_Jump_Cor");
                    yield return new WaitForSeconds(1.3f);
                    StartCoroutine("Long_Slash_Cor", 8);
                    yield return new WaitForSeconds(1.5f);
                    break;
                case 2:
                    StartCoroutine("Barrier_Walk_Cor", 160f);
                    yield return new WaitForSeconds(5.5f);
                    break;
            }
        }

        yield return new WaitForSeconds(1.1f);

        //Bメロ(約30秒)
        for (int i = 0; i < 4; i++) {
            StartCoroutine("Back_Jump_Cor");
            yield return new WaitForSeconds(1.0f);
            StartCoroutine("Jump_Slash_Cor", 32);
            yield return new WaitForSeconds(3.7f);
            if (i == 3)
                transform.localScale = new Vector3(-1, 1, 1);
            StartCoroutine("High_Jump_Cor", transform.localScale.x);
            yield return new WaitForSeconds(3.0f);
        }
        
        //サビ前移動(サビまでの時間調整)
        _controller.Change_Fly_Parameter();
        _controller.Change_Animation("ForwardJumpBool");
        transform.localScale = new Vector3(-1, 1, 1);
        _move_Two_Points.Start_Move(new Vector3(160f, 16f), 4);
        yield return new WaitForSeconds(1.0f);
        _controller.Change_Animation("ShootBool");
        transform.localScale = new Vector3(1, 1, 1);
        //サビ前溜め
        _controller.Play_Charge_Effect(1.5f);
        yield return new WaitForSeconds(1.5f);

        //サビ弾幕(曲開始から56秒)
        for (int i = 0; i < 2; i++) {
            _controller.Play_Burst_Effect();
            _shoot.Start_Knife_Shoot();
            yield return new WaitForSeconds(8.0f);
            _shoot.Stop_Knife_Shoot();
            if (i == 1) {
                _controller.Change_Land_Paramter();
                _controller.Change_Animation("IdleBool");
            }
            yield return new WaitForSeconds(4.0f);
        }
        
        StartCoroutine("Phase1_Cor");
    }
    
    private void Stop_Phase1() {
        StopCoroutine("Phase1_Cor");
        StopAllCoroutines();
        _controller.Change_Animation("IdleBool");
        _controller.Change_Land_Paramter();
        _controller.Stop_Charge_Effect();
        _controller.Quit_Battle_Effect();
        _barrier.Stop_Barrier();
        _shoot.Stop_Knife_Shoot();
    }
    #endregion


    #region Phase2
    //================================フェーズ２======================================
    public void Phase2() {
        if (start_Phase[1]) {
            start_Phase[1] = false;
            Stop_Phase1();            
            StartCoroutine("Phase2_Cor");
        }
    }

    private IEnumerator Phase2_Cor() {
        yield return new WaitForSeconds(2.0f);

        _controller.Play_Battle_Effect();
        Raise_Move_Speed();
        AttackKind next_Attack = AttackKind.close_Slash;
        AttackKind pre_Attack = AttackKind.barrier;
        AttackKind two_Pre_Attack = AttackKind.long_Slash;       

        //通常攻撃
        for (int i = 0; i < 4; i++) {
            //移動
            for (int j = 1; j <= 2; j++) {
                float distance = ((int)Random.Range(0, 2) - 0.5f) * 120f;
                StartCoroutine("Dash_Cor", distance);
                yield return new WaitForSeconds(0.65f);
            }

            //攻撃     
            next_Attack = Select_Next_Attack(pre_Attack, two_Pre_Attack);
            switch (next_Attack) {
                case AttackKind.close_Slash:
                    Jump_Next_Player();
                    yield return new WaitForSeconds(0.8f);
                    StartCoroutine("Close_Slash_Cor");
                    yield return new WaitForSeconds(1.3f);
                    break;
                case AttackKind.long_Slash:
                    StartCoroutine("Back_Jump_Cor");
                    yield return new WaitForSeconds(1.0f);
                    StartCoroutine("Long_Slash_Cor", 14);
                    yield return new WaitForSeconds(1.3f);
                    StartCoroutine("Long_Slash_Cor", 14);
                    yield return new WaitForSeconds(1.3f);
                    break;
                case AttackKind.barrier:
                    StartCoroutine("Barrier_Walk_Cor", 192f);
                    yield return new WaitForSeconds(4.5f);
                    break;
                case AttackKind.jump_Slash:
                    for(int j = 0; j < 2; j++) {
                        StartCoroutine("Back_Jump_Cor");
                        yield return new WaitForSeconds(1.0f);
                        StartCoroutine("Jump_Slash_Cor", 50);
                        yield return new WaitForSeconds(3.8f);
                        StartCoroutine("High_Jump_Cor", transform.localScale.x);
                        yield return new WaitForSeconds(2.5f);
                    }
                    break;
            }
            two_Pre_Attack = pre_Attack;
            pre_Attack = next_Attack;
        }

        //弾幕前溜め
        StartCoroutine("High_Jump_Cor", -1);
        yield return new WaitForSeconds(2.5f);

        _controller.Change_Fly_Parameter();
        _controller.Change_Animation("ShootBool");
        transform.localScale = new Vector3(1, 1, 1);
        _controller.Play_Charge_Effect(2.0f);

        _move_Two_Points.Start_Move(new Vector3(160f, 8f), 4);
        yield return new WaitForSeconds(2.0f);

        //弾幕攻撃
        _controller.Change_Animation("ShootBool");
        _controller.Play_Burst_Effect();
        _shoot.Start_Kunai_Shoot();

        yield return new WaitForSeconds(9.0f);
        _controller.Change_Land_Paramter();
        yield return new WaitForSeconds(1.0f);
        StartCoroutine("Phase2_Cor");
    }

    public void Stop_Phase2() {
        StopCoroutine("Phase2_Cor");
        StopAllCoroutines();
        _controller.Change_Animation("IdleBool");
        _controller.Change_Land_Paramter();
        _controller.Stop_Charge_Effect();
        _controller.Quit_Battle_Effect();
        _barrier.Stop_Barrier();
        _shoot.Stop_Kunai_Shoot();
    }

    //移動速度の上昇
    private void Raise_Move_Speed() {
        _move_Two_Points.Change_Paramter(0.027f, 48f, 0);    //通常ジャンプ用
        _move_Two_Points.Change_Paramter(0.014f, 0, 1);     //バリア突進用
        _move_Two_Points.Change_Paramter(0.03f, 0, 3);      //ダッシュ用
    }
    #endregion


    #region AttackFunctions   
    //=======================================攻撃、移動用関数===========================================
    //次の攻撃を乱数で選ぶ、前回は0%、前々回は20%, それ以外2種類は40%づつででる
    private AttackKind Select_Next_Attack(AttackKind pre_Attack, AttackKind two_Pre_Attack) {
        List<AttackKind> list = new List<AttackKind>{ AttackKind.close_Slash, AttackKind.long_Slash, AttackKind.barrier, AttackKind.jump_Slash };
        list.Remove(pre_Attack);
        list.Remove(two_Pre_Attack);

        int rate = Random.Range(0, 100);
        if (rate < 40)
            return list[0];
        else if (rate < 80)
            return list[1];
        return two_Pre_Attack;        
    }


    //前方向にジャンプする
    private IEnumerator Forward_Jump_Cor(float jump_Distance) {
        _controller.Change_Animation("ForwardJumpBool");
        _controller.Change_Fly_Parameter();
        yield return new WaitForSeconds(0.2f);        

        float x = transform.position.x + jump_Distance;
        if (Mathf.Abs(x) > 200f)
            x = x.CompareTo(0) * 200f;

        _move_Two_Points.Start_Move(new Vector3(x, transform.position.y), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Land_Paramter();
        _controller.Change_Animation("IdleBool");
    }

    //橋の決まった座標にバックジャンプする
    // 自機と反対側に飛ぶ
    private IEnumerator Back_Jump_Cor() {
        int direction = (transform.position.x - player.transform.position.x).CompareTo(0);        
        transform.localScale = new Vector3(direction, 1, 1);

        _controller.Change_Fly_Parameter();
        _controller.Change_Animation("BackJumpBool");
        yield return new WaitForSeconds(0.2f);        

        _move_Two_Points.Start_Move(new Vector3(160f * direction, transform.position.y), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Land_Paramter();
        _controller.Change_Animation("IdleBool");
    }

    //大ジャンプ
    //　direction == 1で左側に飛ぶ、-1で右
    private IEnumerator High_Jump_Cor(int direction) {
        transform.localScale = new Vector3(direction, 1, 1);
        _controller.Change_Animation("ForwardJumpBool");
        _controller.Change_Fly_Parameter();
        yield return new WaitForSeconds(0.2f);

        _move_Two_Points.Start_Move(new Vector3(190f * -direction, -80f), 5);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Land_Paramter();
        _controller.Change_Animation("IdleBool");
    }

    //自機の隣にジャンプする
    private void Jump_Next_Player() {
        float jump_Distance = player.transform.position.x - transform.position.x;
        //自機が右にいるとき
        if (jump_Distance > 0) {
            transform.localScale = new Vector3(-1, 1, 1);
            jump_Distance -= 32f;
        }
        //自機が左にいるとき
        else {
            transform.localScale = new Vector3(1, 1, 1);
            jump_Distance += 32f;
        }
        StartCoroutine("Forward_Jump_Cor", jump_Distance);
    }

    //一定距離移動する
    private IEnumerator Dash_Cor(float dash_Distance) {
        float x = transform.position.x + dash_Distance;
        if (Mathf.Abs(x) > 200f)
            dash_Distance = -dash_Distance;

        transform.localScale = new Vector3(1, 1, 1);
        if (dash_Distance > 0)
            transform.localScale = new Vector3(-1, 1, 1);

        _controller.Change_Animation("DashBool");
        _move_Two_Points.Start_Move(new Vector2(transform.position.x + dash_Distance, transform.position.y), 3);
        yield return new WaitUntil(_move_Two_Points.End_Move);
        _controller.Change_Animation("IdleBool");
    }

    //自機の隣まで移動する
    private void Dash_Next_Player_Cor() {
        float dash_Distance = player.transform.position.x - transform.position.x;
        //自機が右にいるとき
        if (dash_Distance > 0) {
            transform.localScale = new Vector3(-1, 1, 1);
            dash_Distance -= 40f;
        }
        //自機が左にいるとき
        else {
            transform.localScale = new Vector3(1, 1, 1);
            dash_Distance += 40f;
        }
        StartCoroutine("Dash_Cor", dash_Distance);
    }


    //近接攻撃、一回点滅後攻撃
    private IEnumerator Close_Slash_Cor() {        
        _controller.Change_Animation("SlashBool");

        _sound.Play_Before_Slash_Sound();
        yield return new WaitForSeconds(0.10f);
        _sprite.color = new Color(0.7f, 0.7f, 0.7f);        
        yield return new WaitForSeconds(0.10f);
        _sprite.color = new Color(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.20f);

        _controller.Play_Slash_Effect();
        _sound.Play_Slash_Sound();
        yield return new WaitForSeconds(0.5f);

        _controller.Change_Animation("IdleBool");
    }


    //遠距離攻撃、２回点滅後攻撃、ショット
    private IEnumerator Long_Slash_Cor(int num) {        
        _controller.Change_Animation("SlashBool");

        for(int i = 0; i < 2; i++) {
            _sound.Play_Before_Slash_Sound();
            yield return new WaitForSeconds(0.1f);
            _sprite.color = new Color(0.7f, 0.7f, 0.7f);            
            yield return new WaitForSeconds(0.1f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f);
        }
        yield return new WaitForSeconds(0.1f);

        _sound.Play_Slash_Sound();
        _shoot.Shoot_Shotgun(num);
        _controller.Play_Purple_Circle_Effect();
        yield return new WaitForSeconds(0.5f);

        _controller.Change_Animation("IdleBool");
    }


    //バリアを張って歩く
    // walk_Time_Span秒歩く
    private IEnumerator Barrier_Walk_Cor(float walk_Length) {       
        //溜めモーション
        _controller.Change_Animation("BeforeBarrierTrigger");        
        for (int i = 0; i < 3; i++) {
            _sound.Play_Before_Slash_Sound();
            yield return new WaitForSeconds(0.15f);
            _sprite.color = new Color(0.7f, 0.7f, 0.7f);
            yield return new WaitForSeconds(0.15f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f);
        }

        //バリアを張る
        _controller.Play_Small_Charge_Effect();
        _controller.Change_Animation("IdleBool");
        _barrier.Start_Barrier();
        yield return new WaitForSeconds(1.0f);
        _controller.Play_Small_Burst_Effect();

        //歩く
        int direction = (transform.position.x - player.transform.position.x).CompareTo(0);
        transform.localScale = new Vector3(direction, 1, 1);

        _controller.Change_Animation("DashBool");
        _move_Two_Points.Start_Move(new Vector3(transform.position.x - walk_Length * direction, transform.position.y), 1);
        yield return new WaitForSeconds(2.0f);

        //バリア解除
        _controller.Change_Animation("IdleBool");
        _barrier.Stop_Barrier();
    }


    //ジャンプして弾幕出す
    private IEnumerator Jump_Slash_Cor(int num) {
        //ジャンプ
        _controller.Change_Animation("ForwardJumpBool");
        _controller.Change_Fly_Parameter();
        _move_Two_Points.Start_Move(transform.position + new Vector3(0, 80f), 2);
        yield return new WaitUntil(_move_Two_Points.End_Move);
        yield return null;
        _controller.Change_Fly_Parameter();
        //ショット
        _controller.Change_Animation("SlashBool");
        _controller.Play_Yellow_Circle_Effect();
        _shoot.StartCoroutine("Shoot_Jump_Slash_Cor", num);        
        yield return new WaitForSeconds(1.3f);
        //落下
        _controller.Change_Animation("IdleBool");
        _controller.Change_Land_Paramter();
    } 

    #endregion
}