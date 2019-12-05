using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuide2Scene : MonoBehaviour {

    [SerializeField] private ControlleGuideText guide_Text;
    [SerializeField] private GameObject guide_Arrow;

    private void Start() {
        FadeInOut.Instance.Start_Fade_In(new Color(0, 0, 0), 0.02f);
    }

    // Update is called once per frame
    void Update () {
        if (guide_Text.End_Guide_Trigger()) {
            guide_Arrow.SetActive(true);            
        }
	}
}
