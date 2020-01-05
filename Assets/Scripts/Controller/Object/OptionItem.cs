using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionItem : MonoBehaviour {

    public PlayerManager.Option option;

    private void Start() {
        Animator _anim = GetComponent<Animator>();
        switch (option) {
            case PlayerManager.Option.bee:          _anim.SetTrigger("BeeTrigger"); break;
            case PlayerManager.Option.butterfly:    _anim.SetTrigger("ButterflyTrigger"); break;
            case PlayerManager.Option.mantis:       _anim.SetTrigger("MantisTrigger"); break;
            case PlayerManager.Option.spider:       _anim.SetTrigger("SpiderTrigger"); break;
            case PlayerManager.Option.none:         _anim.SetTrigger("NoneTrigger"); break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            PlayerManager.Instance.Set_Option(option);
            Destroy(gameObject);
        }
    }
}
