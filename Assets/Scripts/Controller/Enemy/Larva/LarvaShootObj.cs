using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaShootObj : MonoBehaviour {    

    //弾
    [SerializeField] private GameObject scales_Bullet;

    private ObjectPoolManager pool_Manager;
    

	// Use this for initialization
	void Start () {
        //オブジェクトプール
        pool_Manager = ObjectPoolManager.Instance;
        pool_Manager.Create_New_Pool(scales_Bullet, 10);
	}


    //鱗粉弾
    public void Shoot_Scales_Bullet(int num) {
        for(int i = 0; i < num; i++) {
            //弾生成
            GameObject bullet = pool_Manager.Get_Pool(scales_Bullet).GetObject();
            bullet.transform.position = transform.position;
            //発射
            var v = new Vector2(Random.Range(-200f, 200f), Random.Range(50f, 200f));
            bullet.GetComponent<Rigidbody2D>().velocity = v;
            bullet.GetComponent<Bullet>().Set_Inactive(5.0f);
        }
    }
	

}
