using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionBox : MonoBehaviour {

    [SerializeField] private Texture2D option_Box_Texture;

    private enum Kind {
        bee = 2,
        butterfly = 1,
        mantis = 0,
        spider = 3,
        random = 4,
    }
    [SerializeField] private Kind kind;

    private List<string> open_Tags = new List<string> {
        "PlayerAttackTag",
        "PlayerChargeAttackTag",
        "PlayerButterflyAttackTag",
        "PlayerSpiderAttackTag"
    };

    private List<string> ground_Tags = new List<string> {
        "GroundTag",
        "ThroughGroundTag",
        "DamagedGroundTag",
        "SandbackGroundTag",
    };

    private Rigidbody2D _rigid;
    private bool is_Falling = true;


    //start
    private void Start() {
        _rigid = GetComponent<Rigidbody2D>();
    }


    //update
    private void Update() {
        if (is_Falling) {
            _rigid.MovePosition(transform.position + new Vector3(0, -40f) * Time.deltaTime);
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //箱を開ける判定
        foreach (string tag in open_Tags) {
            if(collision.tag == tag) {
                StartCoroutine("Open_Cor");
            }
        }
        //着地の判定
        foreach(string tag in ground_Tags) {
            if(collision.tag == tag) {
                is_Falling = false;
                _rigid.velocity = Vector2.zero;
            }
        }
    }


    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        foreach (string tag in ground_Tags) {
            if (collision.tag == tag) {
                is_Falling = true;
                _rigid.velocity = Vector2.zero;
            }
        }
    }


    //箱を開ける
    private IEnumerator Open_Cor() {
        //判定を消す
        _rigid.velocity = Vector2.zero;
        is_Falling = false;
        GetComponent<BoxCollider2D>().enabled = false;
        //効果音
        GetComponent<AudioSource>().Play();
        //画像をスライスして、アニメーション再生する
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        TextureSlicer tex_Slice = new TextureSlicer();
        Sprite[] sprites = tex_Slice.Slice_Sprite(option_Box_Texture, new Vector2Int(35, 26));
        int start_Index = 5 * (int)kind;
        for(int i = start_Index; i < start_Index + 5; i++) {
            _sprite.sprite = sprites[i];
            yield return new WaitForSeconds(0.08f);
        }
        //オプションを出す
        if (kind == Kind.random) {
            int r = Random.Range(0, 4);
            PlayerManager.Option po = new PlayerManager.Option();
            switch (r) {
                case 0: po = PlayerManager.Option.bee; break;
                case 1: po = PlayerManager.Option.butterfly; break;
                case 2: po = PlayerManager.Option.mantis; break;
                case 3: po = PlayerManager.Option.spider; break;
            }
            transform.GetChild(0).GetComponent<OptionItem>().option = po;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).SetParent(null);

        yield return new WaitForSeconds(3.0f);
        //点滅して消える
        Renderer _renderer = GetComponent<Renderer>();
        for(int i = 1; i < 20; i++) {
            _renderer.enabled = false;
            yield return new WaitForSeconds(0.01f);
            _renderer.enabled = true;
            yield return new WaitForSeconds(1.0f / i);
        }
        Destroy(gameObject);
    }

}
