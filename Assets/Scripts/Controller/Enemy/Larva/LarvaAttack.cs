using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaAttack : MonoBehaviour {

    //コンポーネント
    private LarvaController _controller;
    private LarvaShootObj shoot_Obj;
    private MoveTwoPoints _move;

    //自機
    private GameObject player;

    private bool start_Phase1 = true;
    private bool start_Phase2 = true;    


    private void Awake() {
        //取得
        _controller = GetComponent<LarvaController>();
        shoot_Obj = GetComponentInChildren<LarvaShootObj>();
        _move = GetComponent<MoveTwoPoints>();

        player = GameObject.FindWithTag("PlayerTag");
    }




    #region Phase1
    //フェーズ1
    public void Do_Phase1() {
        if (start_Phase1) {
            start_Phase1 = false;
            StartCoroutine("Phase1_Cor");
        }
        //自機の方向を向く
        if(player != null) {
            Direct_At_Player();
        }
    }

    private IEnumerator Phase1_Cor() {
        //初期設定
        _controller.Play_Battle_Effect();

        while (true) {
            //弾幕1
            _controller.Play_Charge_Effect(2.0f);
            yield return new WaitForSeconds(2.0f);
            _controller.Play_Burst_Effect();
            shoot_Obj.StartCoroutine("Shoot_Green_Bullet_Cor");
            for (int i = 0; i < 2; i++) {
                yield return new WaitForSeconds(1.5f);
                shoot_Obj.Shoot_Red_Bullet();
            }

            //自機を追従、鱗粉弾発射
            Start_Trace_Player();
            for (int i = 0; i < 3; i++) {
                yield return new WaitForSeconds(2.0f);
                _controller.StartCoroutine("Pre_Action_Blink");
                _controller.Play_Small_Charge_Effect();
                yield return new WaitForSeconds(1.0f);
                shoot_Obj.Shoot_Scales_Bullet(12, 180f);
                _controller.Play_Scales_Effect();
            }
            yield return new WaitForSeconds(3.0f);

            //移動
            Quit_Trace_Player();
            _move.Set_Speed(0.01f, 1.2f, 0.95f);
            _move.Start_Move(new Vector3(130f, 0), 0, false);
            yield return new WaitUntil(_move.End_Move);                       
        }
    }

    //フェーズ1終了
    public void Stop_Phase1() {        
        _controller.Quit_Battle_Effect();
        _controller.Stop_Charge_Effect();
        StopCoroutine("Phase1_Cor");
        _move.StopAllCoroutines();
        shoot_Obj.StopAllCoroutines();
        Quit_Trace_Player();
    }
    #endregion





    #region Phase2
    //フェーズ2
    public void Do_Phase2() {
        if (start_Phase2) {
            start_Phase2 = false;
            Stop_Phase1();
            StartCoroutine("Phase2_Cor");
        }
        if(player != null) {
            Direct_At_Player();
        }
    }

    private IEnumerator Phase2_Cor() {
        //無敵化、移動
        GetComponent<BossCollisionDetection>().Become_Invincible();
        _move.Start_Move(new Vector3(180f, 32f), 0, false);
        yield return new WaitUntil(_move.End_Move);
        yield return new WaitForSeconds(1.0f);
        GetComponent<BossCollisionDetection>().Release_Invincible();

        //初期設定
        _controller.Play_Battle_Effect();

        while (true) {
            //自機を追従、鱗粉弾発射
            Start_Trace_Player();
            for (int i = 0; i < 3; i++) {
                yield return new WaitForSeconds(3.0f);
                _controller.StartCoroutine("Pre_Action_Blink");
                _controller.Play_Small_Charge_Effect();
                yield return new WaitForSeconds(1.0f);
                for (int j = 0; j < 2; j++) {
                    shoot_Obj.Shoot_Scales_Bullet((j+1) * 20, (j+1) * 150f);
                    _controller.Play_Scales_Effect();
                    yield return new WaitForSeconds(0.5f);
                }
            }
            yield return new WaitForSeconds(3.0f);

            //移動
            Quit_Trace_Player();
            _controller.Play_Charge_Effect(2.0f);
            _move.Set_Speed(0.01f, 1.2f, 0.95f);
            _move.Start_Move(new Vector3(0, 110f), 0, false);
            yield return new WaitForSeconds(2.0f);

            //弾幕2            
            shoot_Obj.Shoot_Dif_Bullet();
            _controller.Play_Burst_Effect();
            yield return new WaitForSeconds(1.0f);
            _controller.Play_Burst_Effect();
            shoot_Obj.StartCoroutine("Shoot_Green_Bullet_Cor");
            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < 3; i++) {
                shoot_Obj.Shoot_Red_Bullet2();
                _controller.Play_Small_Burst_Effect();
                yield return new WaitForSeconds(1.5f);
            }
            
            //移動
            _move.Start_Move(new Vector3(130f, -32f), 0, false);
            yield return new WaitUntil(_move.End_Move);
        }
    }
    
    //フェーズ2終了
    public void Stop_Phase2() {
        _controller.Quit_Battle_Effect();
        _controller.Stop_Charge_Effect();
        StopCoroutine("Phase2_Cor");
        _move.StopAllCoroutines();
        shoot_Obj.StopAllCoroutines();
        Quit_Trace_Player();
    }
    #endregion





    //自機の方向を向く
    private void Direct_At_Player() {
        if(player.transform.position.x <= transform.position.x) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


    //自機の追従
    private void Start_Trace_Player() {
        GetComponent<GravitatePlayer>().enabled = true;
    }

    private void Quit_Trace_Player() {
        GetComponent<GravitatePlayer>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

}
