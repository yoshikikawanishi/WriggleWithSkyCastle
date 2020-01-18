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
    [SerializeField] private bool is_Blowed = false;
    [Space]
    [SerializeField] private int life = 5;
    [SerializeField] private int power_Value = 0;
    [SerializeField] private int score_Value = 0;
    [SerializeField] private float drop_Life_Probability = 1;    

    private SpriteRenderer _sprite;
    private Color default_Color;
    private BlowingEnemy blowing_Enemy;
    private SpiderFootingEnemy spider_Footing_Enemy;
    private PoisonedEnemy poisoned_Enemy;

    private bool is_Exist = true;
    private int default_Life;
       

	// Use this for initialization
	void Awake () {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
        default_Color = _sprite.color;
        default_Life = life;
        blowing_Enemy = gameObject.AddComponent<BlowingEnemy>();
        spider_Footing_Enemy = gameObject.AddComponent<SpiderFootingEnemy>();
        poisoned_Enemy = gameObject.AddComponent<PoisonedEnemy>();
    }


    private void OnEnable() {
        if (is_Pooled) {
            _sprite.color = default_Color;
            life = default_Life;
            is_Exist = true;
        }
    }


    //被弾時の処理
    public virtual void Damaged(int damage, string attacked_Tag) {
        life -= damage;
        //毒ダメージ発生
        if(attacked_Tag == "PlayerSpiderAttackTag") {
            poisoned_Enemy.Start_Poisoned_Damaged(false);
        }
        //消滅
        if (life <= 0 && is_Exist) {
            Vanish_Action(attacked_Tag);
            is_Exist = false;
            return;
        }
        //被弾時の点滅
        if (is_Exist) {
            StartCoroutine("Blink");
        }
    }


    //消滅前のアクション
    // EnemyCollisionDetectionのDamagedで変更
    private void Vanish_Action(string attacked_Tag) {
        if(attacked_Tag == "PlayerButterflyAttackTag" && is_Blowed) {
            blowing_Enemy.Blow_Away_Vanish();
        }
        else if(attacked_Tag == "PlayerSpiderAttackTag") {
            spider_Footing_Enemy.Generate_Footing_Vanish();
        }
        else {
            Vanish();
        }
    }


    //消滅時の処理
    public virtual void Vanish() {
        Play_Vanish_Effect();
        Put_Out_Item();
        StopAllCoroutines();
        
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
    protected void Put_Out_Item() {
        gameObject.AddComponent<PutOutSmallItems>().Put_Out_Item(power_Value, score_Value);
        
        if (Random.Range(1, 100) <= drop_Life_Probability) {
            ObjectPoolManager.Instance.Create_New_Pool(Resources.Load("Object/LifeUpItem") as GameObject, 2);
            var life_Item = ObjectPoolManager.Instance.Get_Pool("LifeUpItem").GetObject();
            life_Item.transform.position = transform.position;
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
