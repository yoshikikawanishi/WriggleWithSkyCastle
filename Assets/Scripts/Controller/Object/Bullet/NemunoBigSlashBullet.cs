using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoBigSlashBullet : MonoBehaviour {

    [SerializeField] private GameObject normal_Appear_Blocks;
    [SerializeField] private GameObject fly_Appear_Blocks;

    private SpriteRenderer _sprite;


	// Use this for initialization
	void Start () {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
        //ブロックを配置していく
        StartCoroutine("Deposit_Blocks_Cor");
	}

    private void Update() {
        //移動と透明化
        if(transform.position.x > -190f) {
            transform.position += new Vector3(-1f, 0, 0);
            _sprite.color += new Color(0, 0, 0, -0.002f);
        }
        else {
            GetComponent<ShootSystem>().Shoot();            
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).SetParent(null);
            UsualSoundManager.Instance.Play_Shoot_Sound();
            Destroy(gameObject);
        }
    }

    private IEnumerator Deposit_Blocks_Cor() {        
        //ブロックの生成
        for (int i = 0; i < 5; i++) {
            yield return new WaitForSeconds(1.5f);
            GameObject blocks;
            if (i % 2 == 0) 
                blocks = Instantiate(fly_Appear_Blocks);                            
            else 
                blocks = Instantiate(normal_Appear_Blocks);            
            blocks.transform.position = transform.position;
        }
    }
}
