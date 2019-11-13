using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

	public void Set_Inactive(float lifeTime) {
        StartCoroutine("Set_Inactive_Routine", lifeTime);
    }

    private IEnumerator Set_Inactive_Routine(float lifeTime) {
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            gameObject.SetActive(false);
        }
    }

}
