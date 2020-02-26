using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    private PlayerManager player_Manager;
    private PlayerController player_Controller;
    private PlayerEffect player_Effect;
    private PlayerSoundEffect player_SE;
    private GameObject main_Camera;
    private CameraShake camera_Shake;

    //弾
    [SerializeField] private GameObject normal_Bullet;
    [SerializeField] private GameObject bee_Bullet;
    [SerializeField] private GameObject butterfly_Bullet;
    [SerializeField] private GameObject mantis_Bullet;
    [SerializeField] private GameObject spider_Bullet;
    [SerializeField] private GameObject charge_Shoot_Obj;

    private float charge_Time = 0;
    private float[] charge_Span = new float[3] { 0.3f, 1.0f, 2.0f };

    //チャージ段階
    private int charge_Phase = 0;
    //パワー
    private int player_Power = 0;


	// Use this for initialization
	void Start () {
        //弾のオブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(normal_Bullet, 5);
        ObjectPoolManager.Instance.Create_New_Pool(bee_Bullet, 5);
        ObjectPoolManager.Instance.Create_New_Pool(butterfly_Bullet, 5);
        ObjectPoolManager.Instance.Create_New_Pool(mantis_Bullet, 5);
        ObjectPoolManager.Instance.Create_New_Pool(spider_Bullet, 5);
        //取得
        player_Manager = PlayerManager.Instance;
        player_Controller = GetComponent<PlayerController>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        main_Camera = GameObject.FindWithTag("MainCamera");
        camera_Shake = main_Camera.GetComponent<CameraShake>();
    }


    //Update
    private void Update() {        
        if (!player_Controller.Get_Is_Ride_Beetle()) {
            if(charge_Time > charge_Span[0]) {
                Charge_Shoot();
            }
        }
    }


    #region NormalShoot

    private ObjectPool bullet_Pool;
    private int shoot_Num;
    private float bullet_Speed;
    private float width;

    //ショットを打つ
    public void Shoot() {
        if(Time.timeScale == 0) {
            return;
        }

        Change_Shoot_Status();

        for (int i = 0; i < shoot_Num; i++) {
            GameObject bullet = bullet_Pool.GetObject();
            bullet.transform.position = transform.position;
            bullet.transform.position += new Vector3(0, (-width * shoot_Num) / 2) + new Vector3(0, width * i);
            bullet.transform.localScale = transform.localScale;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bullet_Speed * transform.localScale.x, 0);
            if (player_Manager.Get_Option() == PlayerManager.Option.spider)
                Add_Diffusion_Shoot_Vel(bullet, shoot_Num, i);
            player_SE.Play_Shoot_Sound();
            bullet.GetComponent<Bullet>().Set_Inactive(10);
        }
    }


    //ショットのステータスを変える
    private void Change_Shoot_Status() {
        PlayerManager.Option option = player_Manager.Get_Option();
        int power = player_Manager.Get_Power();
        
        //弾数
        if (power < 32) 
            shoot_Num = 2;        
        else if (power < 64) 
            shoot_Num = 3;        
        else if (power < 128) 
            shoot_Num = 4;        
        else 
            shoot_Num = 5;        

        //弾の種類、速度、幅、弾        
        switch (option) {
            case PlayerManager.Option.none:
                Set_Shoot_Status(normal_Bullet, 900f, 12f);
                break;
            case PlayerManager.Option.bee:
                Set_Shoot_Status(bee_Bullet, 1000f, 6f);                
                break;
            case PlayerManager.Option.butterfly:                
                Set_Shoot_Status(butterfly_Bullet, 700f, 12f);
                shoot_Num--;
                break;
            case PlayerManager.Option.mantis:                
                Set_Shoot_Status(mantis_Bullet, 700f, 8f);
                break;
            case PlayerManager.Option.spider:
                Set_Shoot_Status(spider_Bullet, 400f, 0f);
                shoot_Num += 2;
                break;
        }

        if (shoot_Num <= 0)
            shoot_Num = 1;
    }


    //弾の種類、速度、幅を代入する
    private void Set_Shoot_Status(GameObject bullet, float speed, float width) {
        bullet_Pool = ObjectPoolManager.Instance.Get_Pool(bullet);
        bullet_Speed = speed;
        this.width = width;
    }


    //蜘蛛拡散弾の縦方向の速度を加算
    private void Add_Diffusion_Shoot_Vel(GameObject bullet, int shoot_Num, int index) {
        float center = (shoot_Num - 1) / 2;
        bullet.GetComponent<Rigidbody2D>().velocity += new Vector2(0, (center - index) * -130f);
    }


    #endregion

    #region ChargeShoot
    //チャージショット
    public void Charge_Shoot() {
        if (charge_Phase == 3) {
            StartCoroutine("Charge_Shoot_Cor");
        }
        charge_Time = 0;
        player_Effect.Start_Shoot_Charge(0);
        player_SE.Stop_Charge_Sound();
    }

    private IEnumerator Charge_Shoot_Cor() {
        if (BeetlePowerManager.Instance.beetle_Power < 50)
            yield break;
        //パワー減らす
        BeetlePowerManager.Instance.Decrease(50);
        //生成
        var obj = Instantiate(charge_Shoot_Obj);
        obj.transform.position = transform.position + new Vector3(transform.localScale.x * 128f, 0);        
        ShootSystem[] shoots = obj.GetComponentsInChildren<ShootSystem>();

        player_SE.Play_Charge_Shoot_Sound();
        camera_Shake.Shake(0.25f, new Vector2(0, 1.2f), false);

        //ショット
        shoots[0].Shoot();
        yield return new WaitForSeconds(1f / 14f);
        shoots[1].Shoot();
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
    #endregion
}
