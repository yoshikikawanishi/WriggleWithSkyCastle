using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoShoot : MonoBehaviour {

    [SerializeField] private GameObject yellow_Rice_Bullet;
    [Space]
    [SerializeField] private ShootSystem slash_Shotgun_Odd;
    [SerializeField] private ShootSystem slash_Shootgun_Forward;
    [SerializeField] private ShootSystem knife_Shoot;

    //コンポーネント
    private BulletAccelerator _accelerator;


	// Use this for initialization
	void Start () {
        //取得
        _accelerator = GetComponent<BulletAccelerator>();
        //オブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(yellow_Rice_Bullet, 32);
	}
	
	
    //遠距離斬撃用のショットガン
    public void Shoot_Shotgun() {       
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null)
            return;

        //自機が前方にいたら自機狙い 
        if ((player.transform.position.x - transform.position.x) * transform.parent.localScale.x < 0)
            slash_Shotgun_Odd.Shoot();
        //それ以外は前方に出す
        else {
            if (transform.parent.localScale.x > 0) {
                slash_Shootgun_Forward.center_Angle_Deg = 180f;
            }
            else {
                slash_Shootgun_Forward.center_Angle_Deg = 0;
            }
            slash_Shootgun_Forward.Shoot();
        }
    }


    //空中斬撃用
    public IEnumerator Shoot_Jump_Slash_Cor() {
        List<GameObject> bullet_List = new List<GameObject>();
        Vector3 pos = transform.position + new Vector3(transform.parent.localScale.x * -24f, 0);

        //弾の生成
        for (int i = 0; i < 32; i++) {
            var bullet = ObjectPoolManager.Instance.Get_Pool(yellow_Rice_Bullet).GetObject();
            bullet.transform.position = pos + new Vector3(0, -21f + i * 1.5f);
            bullet_List.Add(bullet);
            yield return new WaitForSeconds(0.005f);
        }
        yield return new WaitForSeconds(0.3f);

        //回転と加速
        float angle = 0, speed = 0;
        for (int i = 0; i < 32; i++) {
            if (bullet_List[i].activeSelf == false)
                continue;
            angle = Random.Range(-180f, 180f);
            bullet_List[i].transform.Rotate(new Vector3(0, 0, angle));
            speed = Random.Range(0.8f, 1.5f);
            bullet_List[i].GetComponent<Rigidbody2D>().velocity = bullet_List[i].transform.right * speed;
        }
        _accelerator.Accelerat_Bullet(bullet_List, 1.05f, 1.5f);
    }


    //ナイフ渦巻き弾開始
    public void Start_Knife_Shoot() {
        knife_Shoot.Shoot();
    }

    //ナイフ渦巻き弾終了
    public void Stop_Knife_Shoot() {
        knife_Shoot.Stop_Shoot();
    }
}
