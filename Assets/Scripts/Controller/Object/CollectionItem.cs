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
        CollectionManager.Instance.Aquire_Collection(collection_Name);
        GetComponent<VerticalVibeMotion>().enabled = false;
        GetComponent<Animator>().SetBool("RaiseBool", true);
        GetComponent<AudioSource>().Play();
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");        
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
