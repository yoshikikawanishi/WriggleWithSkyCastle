using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

    [SerializeField] private GameObject enemy_Prefab;
    [Space]
    [SerializeField] private float start_Gen_Distance_From_Player;
    [Space]
    [SerializeField] private int num;
    [SerializeField] private float span;
    [SerializeField] private Vector2 position_Noise;

    private ObjectPoolManager pool_Manager;
    private ObjectPool pool;
    private GameObject player;

    private bool is_Enable_Generator = true;


	// Use this for initialization
	void Start () {
        //取得
        pool_Manager = ObjectPoolManager.Instance;
        player = GameObject.FindWithTag("PlayerTag");

        //オブジェクトプール
        pool = pool_Manager.Get_Pool(enemy_Prefab);
    }
	

	// Update is called once per frame
	void Update () {
        if(player == null) {
            return;
        }
	    if(is_Enable_Generator) {
            if(Mathf.Abs(player.transform.position.x - transform.position.x) < start_Gen_Distance_From_Player) {
                is_Enable_Generator = false;
                StartCoroutine("Generate_Enemy_Cor");                
            }
        }
	}

    //敵の生成
    private IEnumerator Generate_Enemy_Cor() {
        for(int i = 0; i < num; i++) {
            var enemy = pool.GetObject();
            enemy.transform.position = transform.position;
            enemy.transform.position += (Vector3)Random_Vector2(-position_Noise, position_Noise);
            yield return new WaitForSeconds(span);
        }        
    }


    /// <summary>
    /// Vector2の乱数
    /// </summary>
    /// <returns></returns>
    private Vector2 Random_Vector2(Vector2 start, Vector2 end) {
        float x = Random.Range(start.x, end.x);
        float y = Random.Range(start.y, end.y);
        return new Vector2(x, y);
    }


}
