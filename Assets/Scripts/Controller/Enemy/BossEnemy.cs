using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossCollisionDetection))]
public class BossEnemy : MonoBehaviour {

    //コンポーネント
    private PutOutSmallItems _put_Out_Item;
    private SpriteRenderer _sprite;

    //体力
    public List<int> life = new List<int>();
    [Space]
    //パワースコア
    public int power_Value = 0;
    public int score_Value = 0;
    [Space]
    //エフェクト
    [SerializeField] private GameObject phase_Change_Bomb_Prefab;
    [SerializeField] private GameObject clear_Effect_Prefab;

    //体力
    private List<int> DEFAULT_LIFE = new List<int>();
    //現在のフェーズ
    private int now_Phase = 1;
    //クリア検知用
    private bool clear_Trigger = false;
    private bool is_Cleared = false;


    private void Awake() {
        //取得
        _put_Out_Item   = gameObject.AddComponent<PutOutSmallItems>();
        _sprite         = GetComponent<SpriteRenderer>();
        //初期値代入
        DEFAULT_LIFE = new List<int>(life);
    }    


    //被弾時の処理
    public void Damaged(int damage) {
        if (is_Cleared) {
            return;
        }
        //被弾
        if(life[now_Phase - 1] > 0) {
            life[now_Phase - 1] -= damage;
            Play_Damaged_Effect();
        }
        //フェーズ終了
        if(life[now_Phase - 1] <= 0) {            
            if (now_Phase < life.Count)
                Change_Phase();            
            else if (now_Phase == life.Count)
                Clear();
        }        
    }


    //被弾時のエフェクト
    private void Play_Damaged_Effect() {
        StartCoroutine("Blink");
        //TODO: 被弾時のエフェクト
    }


    //点滅
    private IEnumerator Blink() {
        _sprite.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        if (_sprite.color == new Color(1, 0.5f, 0.5f))
            _sprite.color = new Color(1, 1, 1);
    }


    //フェーズの切り替え
    private void Change_Phase() {
        var effect = Instantiate(phase_Change_Bomb_Prefab);
        effect.transform.position = transform.position;

        _put_Out_Item.Put_Out_Item(power_Value, score_Value);

        Set_Now_Phase(now_Phase + 1);
    }


    //クリア時の処理
    private void Clear() {
        clear_Trigger = true;
        is_Cleared = true;

        var effect = Instantiate(clear_Effect_Prefab);
        effect.transform.position = transform.position;

        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
    }


    //Getter
    public int Get_Now_Phase() {
        return now_Phase;
    }

    public int Get_Default_Life(int phase) {
        return DEFAULT_LIFE[phase - 1];
    }

    //Setter
    public void Set_Now_Phase(int phase) {
        if(phase <= life.Count) {
            now_Phase = phase;
        }
    }


    //クリア検知用
    public bool Clear_Trigger() {
        if (clear_Trigger) {
            clear_Trigger = false;
            return true;
        }
        return false;
    }

}
