using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpOrb : CollectionItem {

    protected override IEnumerator Aquire_Collection() {
        StartCoroutine(base.Aquire_Collection());        
        yield return new WaitForSeconds(0.4f);

        //時間を止めて、チュートリアルウィンドウを表示        
        Time.timeScale = 0;
        for(float t = 0; t < 1.0f; t += 0.016f) { yield return null; }
        //TODO : チュートリアルウィンドウの表示
        Debug.Log("Display Guide Window");
        yield return new WaitUntil(Wait_Input);
        Time.timeScale = 1;
    }


    private bool Wait_Input() {
        if (InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            return true;
        }
        return false;
    }
}
