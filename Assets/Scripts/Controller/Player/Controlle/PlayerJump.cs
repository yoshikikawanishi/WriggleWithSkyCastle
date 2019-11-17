using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private PlayerController _controller;
    private PlayerSoundEffect player_SE;

   
    private void Awake() {
        _rigid      = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerController>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
    }

    
    public void Jump() {
        _rigid.velocity = new Vector2(_rigid.velocity.x, 320f);
        player_SE.Play_Jump_Sound();
        _controller.Change_Animation("JumpBool");
        _controller.is_Landing = false;
    }


    //減速
    public void Slow_Down() {
        if (_rigid.velocity.y > 0) {
            _rigid.velocity *= new Vector2(1, 0.5f);
        }
    }

	
	
}
