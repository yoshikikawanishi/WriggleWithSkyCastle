using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaAttack : MonoBehaviour {

    private bool start_Phase1 = true;
    private bool start_Phase2 = true;

    [SerializeField] private GameObject phase1_Shoot_Obj;
    [SerializeField] private GameObject phase2_Shoot_Obj;


    // Use this for initialization
    void Start () {
		
	}

	
    //フェーズ1
    public void Do_Phase1() {
        if (start_Phase1) {
            start_Phase1 = false;
            StartCoroutine("Phase1_Cor");
        }
    }

    private IEnumerator Phase1_Cor() {
        yield return null;
    }

    //フェーズ1終了
    public void Stop_Phase1() {

    }


    //フェーズ2
    public void Do_Phase2() {
        if (start_Phase2) {
            start_Phase2 = false;
            StartCoroutine("Phase2_Cor");
        }
    }

    private IEnumerator Phase2_Cor() {
        yield return null;
    }
    
    //フェーズ2終了
    public void Stop_Phase2() {

    }

}
