using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyBullet : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;

    //一番近くの敵
    private GameObject target;


    // Use this for initialization
    void Awake() {
        //コンポーネント
        _rigid = GetComponent<Rigidbody2D>();
    }


    //OnEnable
    private void OnEnable() {
        Find_Nearest_Enemy();
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }


    // Update is called once per frame
    void Update() {
        //敵をホーミング
        //消えていなくて、無敵化していない敵をホーミング
        if (target != null && target.activeSelf && target.layer != 10) {
            To_Homing();
        }
        else {
            _rigid.velocity = transform.right * 500f;
        }
    }


    //霊夢より右にいる一番近くの敵を探す
    private void Find_Nearest_Enemy() {
        target = null;
        float min_Distance = 10000;
        float distance = 0;
        GameObject[] enemy_List = GameObject.FindGameObjectsWithTag("EnemyTag");
        foreach (GameObject enemy in enemy_List) {
            distance = Vector2.Distance(transform.position, enemy.transform.position);
            //一番近くて、無敵化していない敵を探す
            if (distance < min_Distance && enemy.activeSelf && enemy.layer != 10) {
                min_Distance = distance;
                target = enemy;
            }
        }
    }


    //ホーミング
    private void To_Homing() {
        transform.LookAt2D(target.transform, Vector2.right);
        _rigid.velocity += (Vector2)transform.right * 120f;
        _rigid.velocity = _rigid.velocity.normalized * 500f;
        float dirVelocity = Mathf.Atan2(_rigid.velocity.y, _rigid.velocity.x) * Mathf.Rad2Deg;    //進行方向に回転
        transform.rotation = Quaternion.AngleAxis(dirVelocity, new Vector3(0, 0, 1));
    }
}
