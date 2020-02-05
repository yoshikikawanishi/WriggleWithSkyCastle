using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGround : MonoBehaviour {

    private GameObject player;
    private bool is_Landing = false;


    // Use this for initialization
    void Start() {
        player = GameObject.FindWithTag("PlayerTag");
    }

    private void LateUpdate() {
        if (is_Landing) {
            Rotate_Player();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            player.transform.SetParent(transform);
            is_Landing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerFootTag") {
            player.transform.SetParent(null);
            player.transform.localScale = new Vector3(1, 1, 1);
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
            is_Landing = false;            
        }
    }


    //自機を斜面に合わせて回転させる
    private void Rotate_Player() {
        AngleCalculater _angle = new AngleCalculater();
        float angle = _angle.Cal_Angle_Two_Points(transform.position, player.transform.position);
        player.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}
