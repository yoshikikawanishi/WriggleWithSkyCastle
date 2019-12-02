﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private PlayerController _controller;

    //速度、加速度
    private float MAX_SPEED = 170f;
    private float acc = 20f;

    
    private void Start() {
        _rigid      = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerController>();
    }


    //移動
    public void Transition(int direction) {
        if (Time.timeScale == 0) return;    //時間停止中
        if (_controller.is_Squat) return;   //しゃがみ中
        direction = direction > 0 ? 1 : -1;

        //空中で慣性つける
        acc = _controller.is_Landing ? 40f : 35f;
        
        //移動、加
        if(direction == 1) {            
            _rigid.velocity += new Vector2(acc, 0);
            transform.localScale = new Vector3(1, 1, 1);
            if(_rigid.velocity.x > MAX_SPEED) {
                _rigid.velocity = new Vector2(MAX_SPEED, _rigid.velocity.y);
            }
        }
        if(direction == -1){
            _rigid.velocity += new Vector2(-acc, 0);
            transform.localScale = new Vector3(-1, 1, 1);
            if(_rigid.velocity.x < -MAX_SPEED) {
                _rigid.velocity = new Vector2(-MAX_SPEED, _rigid.velocity.y);
            }
        }
        //アニメーション
        if (_controller.is_Landing) {
            _controller.Change_Animation("DashBool");
        }
        else {
            _controller.Change_Animation("JumpBool");
        }
        //向き
        if(transform.localScale.x != direction) {
            transform.localScale = new Vector3(direction, 1, 1);
        }
        
    }

    //減速
    public void Slow_Down() {
        if (_controller.is_Landing) {
            _rigid.velocity *= new Vector2(0.1f, 1);
            if (!_controller.is_Squat)
                _controller.Change_Animation("IdleBool");
        }
        else {
            _rigid.velocity *= new Vector2(0.8f, 1);
        }
    }
}
