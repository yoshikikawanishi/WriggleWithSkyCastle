using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    private PlayerManager player_Manager;
    private PlayerController player_Controller;
    private PlayerEffect player_Effect;
    private PlayerSoundEffect player_SE;

    //弾
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject charge_Bullet;
    private ObjectPool bullet_Pool;
    private ObjectPool charge_Bullet_Pool;

    private float charge_Time = 0;
    private float[] charge_Span = new float[3] { 0.3f, 1.0f, 2.0f };

    //チャージ段階
    private int charge_Phase = 0;
    //パワー
    private int player_Power = 0;


	// Use this for initialization
	void Start () {
        //弾のオブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(bullet, 10);
        ObjectPoolManager.Instance.Create_New_Pool(charge_Bullet, 2);
        bullet_Pool = ObjectPoolManager.Instance.Get_Pool(bullet);
        charge_Bullet_Pool = ObjectPoolManager.Instance.Get_Pool(charge_Bullet);
        //取得
        player_Manager = PlayerManager.Instance;
        player_Controller = GetComponent<PlayerController>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
	}


    //Update
    private void Update() {        
        if (!player_Controller.Get_Is_Ride_Beetle()) {
            if(charge_Time > charge_Span[0]) {
                Charge_Shoot();
            }
        }
    }


    //通常ショット
    public void Shoot() {
        if(Time.timeScale == 0) {
            return;
        }
        int num = Shoot_Num();
        for (int i = 0; i < num; i++) {
            GameObject bullet = bullet_Pool.GetObject();
            bullet.transform.position = transform.position;
            bullet.transform.position += new Vector3(0, (-12f * num) / 2) + new Vector3(0, 12f * i);            
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(900f * transform.localScale.x, 0);
            player_SE.Play_Shoot_Sound();
        }
    }


    //パワーによって弾の数を変える
    private int Shoot_Num() {
        int power = player_Manager.Get_Power();
        if (power < 32) {
            return 2;
        }
        else if (power < 64) {
            return 3;
        }
        else if (power < 128) {
            return 4;
        }
        return 5;
    }


    //チャージショット
    public void Charge_Shoot() {
        if (charge_Phase == 3) {
            GameObject bullet = charge_Bullet_Pool.GetObject();
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(500f * transform.localScale.x, 0);
            player_SE.Play_Charge_Shoot_Sound();
        }
        charge_Time = 0;
        player_Effect.Start_Shoot_Charge(0);
        player_SE.Stop_Charge_Sound();
    }


    //チャージショットのチャージ
    public void Charge() {
        Change_Charge_Span();
        //0段階目
        if(charge_Time < charge_Span[0]) {
            if (charge_Phase != 0) {
                charge_Phase = 0;
                player_Effect.Start_Shoot_Charge(0);                
            }
        }
        //1段階目
        else if(charge_Time < charge_Span[1]) {
            if(charge_Phase != 1) {
                charge_Phase = 1;
                player_Effect.Start_Shoot_Charge(1);
                player_SE.Start_Charge_Sound();
            }
        }
        //2段階目
        else if(charge_Time < charge_Span[2]) {
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


    //パワーによってチャージ時間を変える
    private void Change_Charge_Span() {
        //値が変化したときだけ判別
        if(player_Manager.Get_Power() == player_Power) {
            return;
        }
        player_Power = player_Manager.Get_Power();

        if (player_Power < 16) {
            charge_Span = new float[3] { 0.3f, 1.0f, 2.0f };            
        }
        else if(player_Power < 32) {
            charge_Span = new float[3] { 0.27f, 0.85f, 1.7f };
        }
        else if(player_Power < 64) {
            charge_Span = new float[3] { 0.24f, 0.7f, 1.4f };
        }
        else if(player_Power < 128) {
            charge_Span = new float[3] { 0.21f, 0.55f, 1.1f };
        }
        else {
            charge_Span = new float[3] { 0.2f, 0.4f, 0.8f };
        }
    }

}
