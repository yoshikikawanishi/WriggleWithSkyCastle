using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    private PlayerManager player_Manager;
    private PlayerController player_Controller;
    private PlayerEffect player_Effect;

    //弾
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject charge_Bullet;
    private ObjectPool bullet_Pool;
    private ObjectPool charge_Bullet_Pool;

    private float interval_Time = 0;
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
        for (int i = 0; i < 2; i++) {
            GameObject bullet = bullet_Pool.GetObject();
            bullet.transform.position = transform.position;
            bullet.transform.position += new Vector3(0, -8f) + new Vector3(0, 16f * i);            
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(900f * transform.localScale.x, 0);            
        }
    }


    //チャージショット
    public void Charge_Shoot() {
        if (charge_Phase == 3) {
            GameObject bullet = charge_Bullet_Pool.GetObject();
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(500f * transform.localScale.x, 0);
        }
        charge_Time = 0;
        player_Effect.Start_Shoot_Charge(0);
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
            }
        }
        //2段階目
        else if(charge_Time < charge_Span[2]) {
            if (charge_Phase != 2) {
                charge_Phase = 2;
                player_Effect.Start_Shoot_Charge(2);
            }
        }
        //チャージ完了
        else {
            if (charge_Phase != 3) {
                charge_Phase = 3;
                player_Effect.Start_Shoot_Charge(3);
            }
        }
        charge_Time += Time.deltaTime;
    }


    //パワーによってチャージ時間を変える
    public void Change_Charge_Span() {
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
