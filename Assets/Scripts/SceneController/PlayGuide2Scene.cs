using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGuide2Scene : MonoBehaviour {

    [SerializeField] private ControlleGuideText guide_Text;
    [SerializeField] private GameObject guide_Arrow;
	
	
	// Update is called once per frame
	void Update () {
        if (guide_Text.End_Guide_Trigger()) {
            guide_Arrow.SetActive(true);            
        }
	}
}
