using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoAttack : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
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

    private float default_Gravity;    

    private bool[] start_Phase = { true, true };


    private void Awake() {
        //取得
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _controller = GetComponent<NemunoController>();
        _shoot = GetComponentInChildren<NemunoShoot>();
        _barrier = GetComponentInChildren<NemunoBarrier>();
        _sound = GetComponentInChildren<NemunoSoundEffect>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();

        _barrier.gameObject.SetActive(false);

        player = GameObject.FindWithTag("PlayerTag");

        default_Gravity = _rigid.gravityScale;
    }


    #region Phase1
    //フェーズ１
    public void Phase1() {
        if (start_Phase[0]) {
            start_Phase[0] = false;
            StartCoroutine("Phase1_Cor");
        }
    }

    private IEnumerator Phase1_Cor() {
        AttackKind next_Attack = AttackKind.close_Slash;
        AttackKind pre_Attack = AttackKind.jump_Slash;
        AttackKind two_Pre_Attack = AttackKind.jump_Slash;              
        while (true) {            
            //移動
            for (int i = 1; i <= 2; i++) {
                float distance = ((int)Random.Range(0, 2) - 0.5f) * 128f;                
                StartCoroutine("Dash_Cor", distance);
                yield return new WaitForSeconds(0.7f);
            }
            yield return new WaitForSeconds(0.5f);
            //攻撃
            next_Attack = Select_Next_Attack(pre_Attack, two_Pre_Attack);           
            switch (next_Attack) {
                case AttackKind.close_Slash:
                    Jump_Next_Player();
                    yield return new WaitForSeconds(1.1f);
                    StartCoroutine("Close_Slash_Cor");
                    yield return new WaitForSeconds(1.3f);
                    break;
                case AttackKind.long_Slash:
                    StartCoroutine("Back_Jump_Cor");
                    yield return new WaitForSeconds(1.3f);
                    StartCoroutine("Long_Slash_Cor");
                    yield return new WaitForSeconds(1.5f);
                    break;
                case AttackKind.barrier:
                    StartCoroutine("Barrier_Walk_Cor");
                    yield return new WaitForSeconds(5.5f);
                    break;
            }
            two_Pre_Attack = pre_Attack;
            pre_Attack = next_Attack;            
        }
    }
    
    private void Stop_Phase1() {
        StopCoroutine("Phase1_Cor");
        _rigid.gravityScale = default_Gravity;
    }
    #endregion


    #region Phase2
    //フェーズ２
    public void Phase2() {
        if (start_Phase[1]) {
            start_Phase[1] = false;
            Stop_Phase1();
            StartCoroutine("Phase2_Cor");
        }
    }

    private IEnumerator Phase2_Cor() {
        yield return null;
    }

    public void Stop_Phase2() {
        StopCoroutine("Phase2_Cor");
    }
    #endregion


    #region AttackFunctions   

    //次の攻撃を選ぶ、前回は0、前々回は25, それ以外は75ででる
    private AttackKind Select_Next_Attack(AttackKind pre_Attack, AttackKind two_Pre_Attack) {
        List<AttackKind> list = new List<AttackKind>{ AttackKind.close_Slash, AttackKind.long_Slash, AttackKind.barrier };
        list.Remove(pre_Attack);
        list.Remove(two_Pre_Attack);
        AttackKind other_Attack = list[0];

        int rate = Random.Range(0, 100);
        if (rate < 75)
            return other_Attack;
        else
            return two_Pre_Attack;
    }


    //前方向にジャンプする
    private IEnumerator Forward_Jump_Cor(float jump_Distance) {
        _controller.Change_Animation("ForwardJumpBool");
        yield return new WaitForSeconds(0.2f);
        _rigid.gravityScale = 0;

        float x = transform.position.x + jump_Distance;
        if (Mathf.Abs(x) > 200f)
            x = x.CompareTo(0) * 200f;

        _move_Two_Points.Start_Move(new Vector3(x, transform.position.y), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Animation("IdleBool");
        _rigid.gravityScale = default_Gravity;
    }

    //橋の決まった座標にバックジャンプする
    // 自機と反対側に飛ぶ
    private IEnumerator Back_Jump_Cor() {
        int direction = (transform.position.x - player.transform.position.x).CompareTo(0);        
        transform.localScale = new Vector3(direction, 1, 1);

        _controller.Change_Animation("BackJumpBool");
        yield return new WaitForSeconds(0.2f);
        _rigid.gravityScale = 0;

        _move_Two_Points.Start_Move(new Vector3(160f * direction, transform.position.y), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);

        _controller.Change_Animation("IdleBool");
        _rigid.gravityScale = default_Gravity;
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

        _sound.Play_Slash_Sound();
        yield return new WaitForSeconds(0.5f);

        _controller.Change_Animation("IdleBool");
    }


    //遠距離攻撃、２回点滅後攻撃、ショット
    private IEnumerator Long_Slash_Cor() {        
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
        _shoot.Shoot_Shotgun();
        yield return new WaitForSeconds(0.5f);

        _controller.Change_Animation("IdleBool");
    }


    //バリアを張って歩く
    // direction == 1 で左端まで歩く、1で右端
    private IEnumerator Barrier_Walk_Cor() {       
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
        _controller.Change_Animation("IdleBool");
        _barrier.Start_Barrier();
        yield return new WaitForSeconds(1.0f);

        //歩く
        int direction = (transform.position.x - player.transform.position.x).CompareTo(0);
        transform.localScale = new Vector3(direction, 1, 1);

        _controller.Change_Animation("DashBool");
        _move_Two_Points.Start_Move(new Vector3(transform.position.x -160f * direction, transform.position.y), 1);
        yield return new WaitForSeconds(2.0f);

        //バリア解除
        _controller.Change_Animation("IdleBool");
        _barrier.Stop_Barrier();
    }


    //ジャンプして弾幕出す
    private IEnumerator Jump_Slash_Cor() {
        //ジャンプ
        _controller.Change_Animation("ForwardJumpBool");
        _rigid.gravityScale = 0;
        _move_Two_Points.Start_Move(transform.position + new Vector3(0, 80f), 2);
        yield return new WaitUntil(_move_Two_Points.End_Move);
        //ショット
        _controller.Change_Animation("SlashBool");
        _shoot.StartCoroutine("Shoot_Jump_Slash_Cor");
        yield return new WaitForSeconds(1.3f);
        //落下
        _controller.Change_Animation("IdleBool");
        _rigid.gravityScale = default_Gravity;
    }

  

    #endregion
}
