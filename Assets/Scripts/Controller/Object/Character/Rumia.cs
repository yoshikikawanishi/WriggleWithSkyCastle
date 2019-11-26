using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rumia : Character {


    //被弾時の処理
    //パーティクルの停止、ルーミア発見
    protected override void Damaged() {
        base.Damaged();
        Stage1_1Scene.Instance.rumia_State = Stage1_1Scene.Rumia.find;
        GetComponent<ParticleSystem>().Stop();
    }


    //ライフがなくなったときの処理
    //点滅して消滅、ルーミア消滅
    protected override void Action_In_Life_Become_Zero() {                
        StartCoroutine("Vanish_Cor");
        Stage1_1Scene.Instance.rumia_State = Stage1_1Scene.Rumia.delete;
    }


    //点滅して消滅
    private IEnumerator Vanish_Cor() {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Animator>().SetBool("VanishBool", true);

        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 10; i++) {
            _sprite.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds((10 - i) * 0.01f);
            _sprite.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds((10 - i) * 0.02f);
        }
        _sprite.color = new Color(1, 1, 1, 0);

        gameObject.AddComponent<PutOutSmallItems>().Put_Out_Item(0, 10);

        gameObject.SetActive(false);
    }

}
