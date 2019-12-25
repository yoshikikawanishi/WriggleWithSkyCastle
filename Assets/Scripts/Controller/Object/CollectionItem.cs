using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 収集アイテム
/// </summary>
public class CollectionItem : MonoBehaviour {

    [SerializeField] private string collection_Name;


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            StartCoroutine(Aquire_Collection());
        }
    }


    protected virtual IEnumerator Aquire_Collection() {        

        GetComponent<VerticalVibeMotion>().enabled = false;
        GetComponent<Animator>().SetBool("RaiseBool", true);
        GetComponent<AudioSource>().Play();
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");

        Display_Guide_In_First_Item();

        yield return new WaitForSeconds(0.5f);
        
        CollectionManager.Instance.Aquire_Collection(collection_Name);

        Destroy(gameObject);
    }


    private void Display_Guide_In_First_Item() {        
        //アイテムを初めて取得するかどうか
        var data_Dictionary = CollectionManager.Instance.Get_Collections_Data();
        List<string> key_List = new List<string>(data_Dictionary.Keys);        

        foreach (string key in key_List) {
            if(data_Dictionary[key]) {                
                return;
            }
        }

        //ガイドウィンドウの表示
        GetComponent<GuideWindowDisplayer>().Open_Window("UI/GuideInFirstItem");
        
    }
}
