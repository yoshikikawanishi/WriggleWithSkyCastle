using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の体力を管理
/// 敵の被弾時、消滅時の処理を行う、継承で処理変更
/// </summary>
[RequireComponent(typeof(EnemyCollisionDetection))]
public class Enemy : MonoBehaviour {
    
    [SerializeField] private bool is_Pooled = false;    
    [SerializeField] private int life = 5;
    [SerializeField] private int power_Value = 0;
    [SerializeField] private int score_Value = 0;
    [SerializeField] private float drop_Life_Probability = 1;

    private SpriteRenderer _sprite;
    private Color default_Color;

    private bool is_Exist = true;


	// Use this for initialization
	void Start () {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
        default_Color = _sprite.color;
	}


    //被弾時の処理
    public virtual void Damaged(int damage) {
        life -= damage;
        if(life <= 0 && is_Exist) {
            Vanish();
            is_Exist = false;
            return;
        }
        //TODO:エフェクト
        StartCoroutine("Blink");
    }


    //消滅時の処理
    public virtual void Vanish() {
        Play_Vanish_Effect();
        Put_Out_Item();
        if (is_Pooled) {
            gameObject.SetActive(false);
            return;
        }
        Destroy(gameObject);
    }


    //消滅時のエフェクト
    public virtual void Play_Vanish_Effect() {        
        GameObject effect_Prefab = Resources.Load("Effect/EnemyVanishEffect") as GameObject;
        var effect = Instantiate(effect_Prefab);
        effect.transform.position = transform.position;
        Destroy(effect, 1.5f);
    }


    //アイテムの放出
    private void Put_Out_Item() {
        gameObject.AddComponent<PutOutSmallItems>().Put_Out_Item(power_Value, score_Value);
        //TODO:回復アイテムのドロップ
        Debug.Log("TODO: Drop_Life");
        if (Random.Range(1, 100) <= drop_Life_Probability) {

        }
    }

    //点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        //色が点滅中に変わっていないことを確認
        if (_sprite.color == new Color(1, 0.5f, 0.5f))        
            _sprite.color = default_Color;
    }	

}
