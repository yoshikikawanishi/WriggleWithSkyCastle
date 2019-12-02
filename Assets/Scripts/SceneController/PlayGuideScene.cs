using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuideScene : MonoBehaviour {

    [SerializeField] private ControlleGuideText guide_Text;
    [SerializeField] private GameObject guide_Arrow;


    // Update is called once per frame
    void Update() {
        if (guide_Text.End_Guide_Trigger()) {
            StartCoroutine("Gen_Guide_Arrow");
        }
    }

    private IEnumerator Gen_Guide_Arrow() {
        yield return new WaitForSeconds(1.0f);
        guide_Arrow.SetActive(true);
    }

}
