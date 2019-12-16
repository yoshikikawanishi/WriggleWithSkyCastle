using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlowerFairyWhite : FairyEnemy {

    private GameObject sun_Flower;    
    private Rigidbody2D _rigid;


    private void Start() {      
        sun_Flower = transform.GetChild(0).gameObject;
        _rigid = GetComponent<Rigidbody2D>();
    }


    //消滅時の処理
    public override void Vanish() {
        //ひまわりを出す
        sun_Flower.SetActive(true);
        sun_Flower.transform.SetParent(null);
        sun_Flower.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 100f);

        base.Vanish();

        //４秒後に復活
        Invoke("Revive", 4.0f);
    }
    

    //復活
    private void Revive() {
        if(gameObject == null) {
            return;
        }
        gameObject.SetActive(true);
        StartCoroutine("Revive_Cor");
    }


    private IEnumerator Revive_Cor() {        
        Change_Exist_Component(false, false, false);

        sun_Flower.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.0f);

        transform.position = sun_Flower.transform.position + new Vector3(7.5f, 2f);
        Change_Exist_Component(true, true, true);        

        //ひまわりを戻す
        sun_Flower.transform.SetParent(transform);
        sun_Flower.SetActive(false);
    }


    //RendererとColliderとrigidbodyの変更
    private void Change_Exist_Component(bool renderer, bool collider, bool rigid) {
        GetComponent<Renderer>().enabled = renderer;
        GetComponent<CircleCollider2D>().enabled = collider;
        _rigid.simulated = rigid;
    }

}
