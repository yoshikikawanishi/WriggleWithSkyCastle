using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBLDefine;

public class PlayerController : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private Animator _anim;
    private PlayerTransition _transition;
    private PlayerTransitionRidingBeetle _transition_Beetle;
    private PlayerJump _jump;
    private PlayerAttack _attack;
    private PlayerShoot _shoot;
    private PlayerGettingOnBeetle _getting_On_Beetle;
    //キー入力用
    private InputManager input;

    //状態
    public bool is_Playable = true;
    public bool is_Landing = true;
    public bool is_Ride_Beetle = false;
    //攻撃の入力識別用
    public bool start_Attack_Frame_Count = false;

    private int attack_Frame_Count = 0;

    private string now_Animator_Parameter = "IdleBool";

    private float SHOOT_INTERVAL = 0.2f;
    private float shoot_Time = 0.2f;


    //Awake
    private void Awake() {
        //取得
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _transition = GetComponent<PlayerTransition>();
        _transition_Beetle = GetComponent<PlayerTransitionRidingBeetle>();
        _jump = GetComponent<PlayerJump>();
        _attack = GetComponent<PlayerAttack>();
        _shoot = GetComponent<PlayerShoot>();
        _getting_On_Beetle = GetComponent<PlayerGettingOnBeetle>();
        input = InputManager.Instance;
    }


    // Update is called once per frame
    void Update () {

        if (!is_Playable) {
            return;
        }

        if (is_Ride_Beetle) {
            Beetle_Controlle();    
        }
        else {
            Normal_Controlle();                   
        }        
	}


    //通常時の操作
    private void Normal_Controlle() {
        //移動
        if (Input.GetAxisRaw("Horizontal") > 0) {
            _transition.Transition(1);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0) {
            _transition.Transition(-1);
        }
        else {
            _transition.Slow_Down();
        }
        //ジャンプ
        if (input.GetKey(Key.Jump) && is_Landing) {
            _jump.Jump();
        }
        if (input.GetKeyUp(Key.Jump)) {
            _jump.Slow_Down();
        }
        //近接攻撃
        Attack();
        //カブトムシに乗る
        if (input.GetKeyDown(Key.Ride) && BeetlePowerManager.Instance.Get_Beetle_Power() > 0) {
            _getting_On_Beetle.Get_On_Beetle(true);
        }        
    }


    //カブトムシ時の操作
    private void Beetle_Controlle() {
        //移動
        Vector2 direction = new Vector2(0, 0);
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (input.GetKey(Key.Slow))
            direction *= 0.3f;
        _transition_Beetle.Transition(direction);
        //ショット
        Shoot();
        //カブトムシから降りる
        if (input.GetKeyDown(Key.Ride) || BeetlePowerManager.Instance.Get_Beetle_Power() <= 0) {
            _getting_On_Beetle.Get_Off_Beetle();
        }
        //パワーの消費
        BeetlePowerManager.Instance.Decrease_In_Update(8.0f);
    }


    //近接攻撃
    //注意：もし、攻撃発生までにis_Playableがfalseになっても、攻撃は続行する
    public void Attack() {
        //入力を受け取ったら、少しだけ待つ
        if (input.GetKeyDown(Key.Attack)) {
            start_Attack_Frame_Count = true;
        }
        //待ってる間に下が押されたらキック、1度も押されなかったら横攻撃
        if (start_Attack_Frame_Count) {
            attack_Frame_Count++;
            if (Input.GetAxis("Vertical") < -0.1f) {
                _attack.Kick();
            }
            else if (attack_Frame_Count > 7) {
                _attack.Attack();
            }
            else return;
            attack_Frame_Count = 0;
            start_Attack_Frame_Count = false;
        }
    }


    //ショット
    public void Shoot() {
        //通常ショット
        if (shoot_Time < SHOOT_INTERVAL) {
            shoot_Time += Time.deltaTime;
        }
        else {
            if (input.GetKeyDown(Key.Shoot)) {
                _shoot.Shoot();
            }
        }
        //チャージショット
        if (input.GetKey(Key.Shoot)) {
            _shoot.Charge();
        }        
        if (input.GetKeyUp(Key.Shoot)) {
            _shoot.Charge_Shoot();
        }
    }


    //アニメーション変更
    //横攻撃とキックはAnyStateからのTriggerで管理
    public void Change_Animation(string next_Parameter) {
        if(now_Animator_Parameter == next_Parameter) {
            return;
        }

        _anim.SetBool("IdleBool", false);
        _anim.SetBool("DashBool", false);
        _anim.SetBool("JumpBool", false);
        _anim.SetBool("RideBeetleBool", false);

        _anim.SetBool(next_Parameter, true);
        now_Animator_Parameter = next_Parameter;
    }


    //Setter
    public void Set_Is_Playable(bool is_Playable) {
        this.is_Playable = is_Playable;
    }

    //Getter
    public bool Get_Is_Ride_Beetle() {
        return is_Ride_Beetle;
    }

    //カブトムシ無効化
    public void To_Disable_Ride_Beetle() {
        _getting_On_Beetle.To_Disable();
    }

    //カブトムシ無効化解除
    public void To_Enable_Ride_Beetle() {
        _getting_On_Beetle.To_Enable();
    }

    //カブトムシ時のスクロール速度変化
    public void Change_Beetle_Scroll_Speed(float speed) {
        _getting_On_Beetle.Set_Scroll_Speed(speed);
    }

    //カブトムシ時の方向変更
    public void Change_Beetle_Direction(int scale_X) {
        _transition_Beetle.Change_Body_Direction(scale_X);
    }

    //カブトムシ時の方向取得
    public int Get_Beetle_Direction() {
        return _transition_Beetle.Get_Beetle_Direction();
    }

    //点滅
    public IEnumerator Blink(float time_Length) {
        Renderer player_Renderer = GetComponent<Renderer>();
        float t = 0;
        while (t < time_Length) {
            player_Renderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            player_Renderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
            t += 0.2f;
        }        
    }
   
}
