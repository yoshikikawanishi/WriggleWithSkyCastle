using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceObject : MonoBehaviour {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        GameObject obj1 = GameObject.Instantiate(Resources.Load("CommonScripts") as GameObject);
        obj1.transform.position = new Vector3(1, 1, 0);
    }
}
