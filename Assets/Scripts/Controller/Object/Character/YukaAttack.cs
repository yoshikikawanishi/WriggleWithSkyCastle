using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YukaAttack : MonoBehaviour {

    //コンポーネント
    private BossEnemy _boss_Controller;
    private MoveTwoPoints _move_Two_Points;

    //弾幕用オブジェクト
    [SerializeField] private GameObject cross_Shoot_Obj;
    [SerializeField] private GameObject spiral_Shoot_Obj;
    [SerializeField] private GameObject diffusion_Shoot_Obj;
    [SerializeField] private GameObject flower_Bullet;


    // Use this for initialization
    void Start() {
        //取得
        _boss_Controller = GetComponent<BossEnemy>();
        _move_Two_Points = GetComponent<MoveTwoPoints>();
        //ボス戦開始前無敵化
        _boss_Controller.Set_Is_Invincible(true);
        //オブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(flower_Bullet, 4);
    }

    // Update is called once per frame
    void Update() {
        //クリア
        if (_boss_Controller.Clear_Trigger()) {
            StartCoroutine("Clear_Cor");
        }
    }


    //戦闘開始
    public void Start_Battle() {        
        //ボス敵にする
        _boss_Controller.Set_Is_Invincible(false);
        gameObject.tag = "EnemyTag";
        //攻撃開始
        StartCoroutine("Attack_Cor");
        //カメラの固定
        GameObject camera = GameObject.FindWithTag("MainCamera");
        camera.AddComponent<MoveTwoPoints>().Start_Move(new Vector3(transform.position.x, 0, -10f));
        camera.GetComponent<CameraController>().enabled = false;
    }    


    //クリア時の処理
    public IEnumerator Clear_Cor() {
        //攻撃中止
        StopCoroutine("Attack_Cor");
        Stop_Charge_Effect();
        //背景戻す
        BackGroundEffector.Instance.Start_Change_Color(new Color(1, 1, 1), 0.1f);
        //移動
        _move_Two_Points.Start_Move(new Vector3(transform.position.x, -110f), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);
        GetComponent<Animator>().SetBool("AttackBool", false);
        //アイテム
        transform.GetChild(0).gameObject.SetActive(true);
        //カメラの固定を外す
        GameObject camera = GameObject.FindWithTag("MainCamera");
        camera.GetComponent<CameraController>().enabled = true;
    }


    //攻撃
    private IEnumerator Attack_Cor() {
        GetComponent<Animator>().SetBool("AttackBool", true);
        _move_Two_Points.Start_Move(new Vector3(transform.position.x, -32f));
        yield return new WaitUntil(_move_Two_Points.End_Move);

        while (true) {
            Play_Charge_Effect();
            yield return new WaitForSeconds(2.0f);
            Stop_Charge_Effect();

            Play_Burst_Effect();
            Start_Spiral_Shoot();
            yield return new WaitForSeconds(8.0f);

            Shoot_Cross_Bullet(new Vector2());
            yield return new WaitForSeconds(0.5f);
            Shoot_Diffusion_Bullet(new Vector2(64f, 80f), 0);
            yield return new WaitForSeconds(0.5f);
            Shoot_Diffusion_Bullet(new Vector2(-64f, 80f), 1);
            yield return new WaitForSeconds(6.0f);

            for(int i = 0; i < 9; i++) {
                Drop_Flower_Bullet(240f - i * 60f);
                yield return new WaitForSeconds(0.8f);
            }
        }
    }


    //交差弾発射
    private void Shoot_Cross_Bullet(Vector2 offset) {
        ShootSystem[] shoots = cross_Shoot_Obj.GetComponents<ShootSystem>();        
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].offset = offset;
            shoots[i].Shoot();
        }
    }

    //渦巻き弾発射開始
    private void Start_Spiral_Shoot() {
        ShootSystem[] shoots = spiral_Shoot_Obj.GetComponents<ShootSystem>();
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].Shoot();
        }
    }

    //全方位弾発射
    private void Shoot_Diffusion_Bullet(Vector2 offset, int index) {
        var child = diffusion_Shoot_Obj.transform.GetChild(index).gameObject;
        ShootSystem[] shoots = child.GetComponents<ShootSystem>();
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].offset = offset;
            shoots[i].Shoot();
        }
    }

    //花弾を落とす
    private void Drop_Flower_Bullet(float offset_X) {
        var bullet = ObjectPoolManager.Instance.Get_Pool(flower_Bullet).GetObject();
        Debug.Log(bullet);
        bullet.transform.position = new Vector3(transform.position.x + offset_X, 180f);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -80f);
        bullet.GetComponent<Bullet>().Set_Inactive(5.0f);
    }


    //溜め
    private void Play_Charge_Effect() {
        transform.GetChild(5).gameObject.SetActive(true);
    }

    private void Stop_Charge_Effect() {
        transform.GetChild(5).gameObject.SetActive(false);
    }

    //放出
    private void Play_Burst_Effect() {
        transform.GetChild(6).GetComponent<ParticleSystem>().Play();
    }
}
