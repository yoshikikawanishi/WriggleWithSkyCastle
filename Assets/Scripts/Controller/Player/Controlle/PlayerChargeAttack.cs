using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeAttack : MonoBehaviour {

    //コンポーネント
    private PlayerManager player_Manager;
    private PlayerSoundEffect player_SE;
    private PlayerEffect player_Effect;
    private PlayerAttack _attack;

    private int player_Power;
    private int charge_Phase = 1;
    private float charge_Time = 0;
    private float[] charge_Span = new float[3] { 0.3f, 1.0f, 2.0f };
    

    private void Start() {
        //取得
        player_Manager = PlayerManager.Instance;
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
        _attack = GetComponent<PlayerAttack>();
    }


    //溜め攻撃用溜め、chargePhaseとchargeTimeを変える
    public void Charge() {
        Change_Charge_Span();
        //0段階目
        if (charge_Time < charge_Span[0]) {
            if (charge_Phase != 0) {
                charge_Phase = 0;
                player_Effect.Start_Shoot_Charge(0);
            }
        }
        //1段階目
        else if (charge_Time < charge_Span[1]) {
            if (charge_Phase != 1) {
                charge_Phase = 1;
                player_Effect.Start_Shoot_Charge(1);
                player_SE.Start_Charge_Sound();
            }
        }
        //2段階目
        else if (charge_Time < charge_Span[2]) {
            if (charge_Phase != 2) {
                charge_Phase = 2;
                player_Effect.Start_Shoot_Charge(2);
                player_SE.Change_Charge_Sound_Pitch(1.15f);
            }
        }
        //チャージ完了
        else {
            if (charge_Phase != 3) {
                charge_Phase = 3;
                player_Effect.Start_Shoot_Charge(3);
                player_SE.Change_Charge_Sound_Pitch(1.3f);
            }
        }
        charge_Time += Time.deltaTime;
    }


    //チャージ攻撃,ChargePhase == 3で強攻撃、それ以下で通常攻撃
    public void Charge_Attack() {
        if(charge_Phase == 3) {
            StartCoroutine("Charge_Attack_Cor");
        }
        else {
            _attack.Attack();
        }
        charge_Time = 0;
        player_Effect.Start_Shoot_Charge(0);
        player_SE.Stop_Charge_Sound();
    }

    //強攻撃
    private IEnumerator Charge_Attack_Cor() {
        Debug.Log("Charge_Attack");
        yield return null;
    }


    //パワーによってチャージ時間を変える
    private void Change_Charge_Span() {
        //値が変化したときだけ判別
        if (player_Manager.Get_Power() == player_Power) {
            return;
        }
        player_Power = player_Manager.Get_Power();

        if (player_Power < 16) {
            charge_Span = new float[3] { 0.3f, 1.0f, 2.0f };
        }
        else if (player_Power < 32) {
            charge_Span = new float[3] { 0.27f, 0.85f, 1.7f };
        }
        else if (player_Power < 64) {
            charge_Span = new float[3] { 0.24f, 0.7f, 1.4f };
        }
        else if (player_Power < 128) {
            charge_Span = new float[3] { 0.21f, 0.55f, 1.1f };
        }
        else {
            charge_Span = new float[3] { 0.2f, 0.4f, 0.8f };
        }
    }
    
}
