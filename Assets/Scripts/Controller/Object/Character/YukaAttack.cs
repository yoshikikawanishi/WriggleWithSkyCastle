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
            YukaMovie.Instance.Start_Clear_Movie();
        }
    }


    //戦闘開始
    public void Start_Battle() {        
        //ボス敵にする
        _boss_Controller.Set_Is_Invincible(false);
        gameObject.tag = "EnemyTag";        
        //攻撃開始
        StartCoroutine("Attack_Cor");
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
    }


    //攻撃
    private IEnumerator Attack_Cor() {
        GetComponent<Animator>().SetBool("AttackBool", true);
        _move_Two_Points.Start_Move(new Vector3(transform.position.x + 96f, -32f));
        yield return new WaitUntil(_move_Two_Points.End_Move);

        bool is_First_Loop = true;
        while (true) {
            Play_Charge_Effect();
            yield return new WaitForSeconds(1.5f);
            Stop_Charge_Effect();

            Play_Burst_Effect();
            if (is_First_Loop) { StartCoroutine(Play_Guide_Message(18, 18)); }
            Start_Spiral_Shoot();
            yield return new WaitForSeconds(5.0f);            
            
            Shoot_Diffusion_Bullet(new Vector2(-48, 96f), 0);
            yield return new WaitForSeconds(0.5f);
            Shoot_Diffusion_Bullet(new Vector2(-32, -48f), 1);
            Play_Small_Charge_Effect();
            yield return new WaitForSeconds(0.5f);
            Shoot_Cross_Bullet(new Vector2());
            yield return new WaitForSeconds(5.0f);

            _move_Two_Points.Start_Move(new Vector3(transform.position.x, -80f));
            yield return new WaitForSeconds(1.0f);
            
            for(int i = 0; i < 3; i++) {
                Drop_Flower_Bullet(240f - i * 120f);
                yield return new WaitForSeconds(1.0f);
            }
            if (is_First_Loop) { StartCoroutine(Play_Guide_Message(19, 19)); }
            for (int i = 3; i < 5; i++) {
                Drop_Flower_Bullet(240f - i * 120f);
                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(1.0f);
            for(int i = 0; i < 5; i++) {
                Drop_Flower_Bullet(-180f + i * 120f);
                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(2.5f);

            _move_Two_Points.Start_Move(new Vector3(transform.position.x, -32f));
            is_First_Loop = false;
        }
    }
    

    //ラルバの助言入れる
    private IEnumerator Play_Guide_Message(int start_ID, int end_ID) {
        MessageDisplay _message = GetComponent<MessageDisplay>();
        Time.timeScale = 0;
        _message.Start_Display_Auto("YukaText", start_ID, end_ID, 1.0f, 0.05f);
        yield return new WaitUntil(_message.End_Message);
        Time.timeScale = 1;
    }


    //交差弾発射
    private void Shoot_Cross_Bullet(Vector2 offset) {
        UsualSoundManager.Instance.Play_Shoot_Sound();
        ShootSystem[] shoots = cross_Shoot_Obj.GetComponents<ShootSystem>();        
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].offset = offset;
            shoots[i].Shoot();
        }
    }

    //渦巻き弾発射開始
    private void Start_Spiral_Shoot() {
        UsualSoundManager.Instance.Play_Shoot_Sound();
        ShootSystem[] shoots = spiral_Shoot_Obj.GetComponents<ShootSystem>();
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].Shoot();
        }
    }

    //全方位弾発射
    private void Shoot_Diffusion_Bullet(Vector2 offset, int index) {
        UsualSoundManager.Instance.Play_Shoot_Sound();
        var child = diffusion_Shoot_Obj.transform.GetChild(index).gameObject;
        ShootSystem[] shoots = child.GetComponents<ShootSystem>();
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].offset = offset;
            shoots[i].Shoot();
        }
    }

    //花弾を落とす
    private void Drop_Flower_Bullet(float offset_X) {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        var bullet = ObjectPoolManager.Instance.Get_Pool(flower_Bullet).GetObject();        
        bullet.transform.position = new Vector3(camera.transform.position.x + offset_X, 180f, 0);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -80f);
        bullet.GetComponent<Bullet>().Set_Inactive(5.0f);

        UsualSoundManager.Instance.Play_Shoot_Sound();
    }


    //溜め
    private void Play_Charge_Effect() {
        transform.GetChild(5).gameObject.SetActive(true);
    }

    private void Stop_Charge_Effect() {
        transform.GetChild(5).gameObject.SetActive(false);
    }

    //小ためエフェクト
    private void Play_Small_Charge_Effect() {        
        transform.GetChild(7).GetComponent<ParticleSystem>().Play();
    }

    //放出
    private void Play_Burst_Effect() {
        transform.GetChild(6).GetComponent<ParticleSystem>().Play();
    }
}
