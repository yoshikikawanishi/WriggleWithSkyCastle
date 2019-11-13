using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    public enum ItemKind {
        power,
        score,
        beetle_Power,
        life_Up,
        stock_Up,
    }
    [SerializeField] private ItemKind kind;
    [SerializeField] private int value;

    
	// Update is called once per frame
	void Update () {
        //下まで落ちたら消す
        if (transform.position.y < -200f) {
            gameObject.SetActive(false);
        }
    }


    //自機と当たったら消す
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            switch (kind) {
                case ItemKind.power:        Gain_Power_Item();          break;
                case ItemKind.score:        Gain_Score_Item();          break;
                case ItemKind.beetle_Power: Gain_Beetle_Power_Item();   break;
                case ItemKind.life_Up:      Gain_Life_Up_Item();        break;
                case ItemKind.stock_Up:     Gain_Stock_Up_Item();       break;
            }
            gameObject.SetActive(false);
        }
    }    


    //P取得時
    private void Gain_Power_Item() {
        PlayerManager.Instance.Add_Power();
    }

    //点取得時
    private void Gain_Score_Item() {
        PlayerManager.Instance.Add_Score(100);
    }

    //カブトムシパワー取得時
    private void Gain_Beetle_Power_Item() {
        BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", value);
    }

    //回復取得時
    private void Gain_Life_Up_Item() {
        PlayerManager.Instance.Add_Life();        
    }

    //残機アップアイテム取得時
    private void Gain_Stock_Up_Item() {
        PlayerManager.Instance.Add_Stock();
        Destroy(gameObject);
    }

}
