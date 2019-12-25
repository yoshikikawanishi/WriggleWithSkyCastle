using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFairy : Character {


    protected override void Damaged() {        
        StartCoroutine("Action_In_Damaged_Cor");
    }

    private IEnumerator Action_In_Damaged_Cor() {
        GetComponent<BoxCollider2D>().enabled = false;

        yield return new WaitForSeconds(0.1f);
        Start_Setting();

        //発見
        MessageDisplay _message = GetComponent<MessageDisplay>();
        _message.Start_Display("LightFairyText", 1, 1);
        yield return new WaitUntil(_message.End_Message);
        Start_Setting();    //念のためもう１回

        //登場
        StartCoroutine("Appear");
        yield return new WaitForSeconds(2.0f);
        
        //登場セリフ
        _message.Start_Display("LightFairyText", 2, 7);
        yield return new WaitUntil(_message.End_Message);
        Play_Star_Effect();
        yield return new WaitForSeconds(1.0f);

        //退散セリフ
        _message.Start_Display("LightFairyText", 8, 10);
        yield return new WaitUntil(_message.End_Message);
        Put_Out_Item();
        transform.position += new Vector3(0, 300f);

        End_Setting();
    }


    //登場
    private IEnumerator Appear() {
        SpriteMask _mask = GetComponentInChildren<SpriteMask>();
        for (int i = 0; i < 8; i++) {
            _mask.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _mask.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        _mask.enabled = false;
    }


    //登場エフェクト
    private void Play_Star_Effect() {
        GetComponent<ParticleSystem>().Play();
        GetComponent<AudioSource>().Play();
    }


    //アイテム放出
    private void Put_Out_Item() {
        transform.GetChild(4).gameObject.SetActive(true);
        transform.GetChild(4).SetParent(null);
    }


    //開始設定
    private void Start_Setting() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        player.GetComponent<PlayerController>().Set_Is_Playable(false);
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        player.GetComponentInChildren<PlayerBodyCollision>().Become_Invincible();
        PauseManager.Instance.Set_Is_Pausable(false);
    }


    //終了設定
    private void End_Setting() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        player.GetComponent<PlayerController>().Set_Is_Playable(true);
        player.GetComponentInChildren<PlayerBodyCollision>().Release_Invincible();
        PauseManager.Instance.Set_Is_Pausable(true);
    }

}
