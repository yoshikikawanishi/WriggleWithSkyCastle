using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ConfigButton : MonoBehaviour {

    [SerializeField] private GameObject jump_Button;
    [SerializeField] private GameObject shot_Button;
    [SerializeField] private GameObject fly_Button;
    [SerializeField] private GameObject pause_Button;
    [SerializeField] private GameObject BGM_Set_Slider;
    [SerializeField] private GameObject SE_Set_Slider;

    [SerializeField] private AudioMixer audio_Mixer;


    private bool wait_Input = false;


    //ジャンプ、決定ボタン
    public void Jump_Button() {
        if (!wait_Input && InputManager.Instance.GetKeyDown(MBLDefine.Key.Jump)) {
            StartCoroutine(Change_Key_Config(MBLDefine.Key.Jump, jump_Button));
        }
    }

   


    //入力待ち、コンフィグ変更
    private IEnumerator Change_Key_Config(MBLDefine.Key changed_Key, GameObject button) {
        //色の変更
        button.GetComponent<Image>().color = new Color(1, 0.4f, 0.4f);
        //テキストの変更
        button.GetComponentInChildren<Text>().text = "";
        //入力待ち
        wait_Input = true;
        yield return null;
        while (true) {
            button.GetComponent<Button>().Select();
            if (Input.anyKeyDown) {
                //矢印キーは受け付けない
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                    yield return null;
                    continue;
                }
                //押されたキーコードの取得
                KeyCode put_Button = new KeyCode();
                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode))) {
                    if (Input.GetKeyDown(code)) {
                        put_Button = code;
                        break;
                    }
                }
                //ゲームパッド
                bool is_GamePad = false;
                InputManager.KeyConfigSetting key_Setting = InputManager.KeyConfigSetting.Instance;
                for (int i = 0; i < 16; i++) {
                    if (Input.GetKeyDown("joystick button " + i.ToString())) {
                        key_Setting.SetKey(changed_Key, new List<KeyCode> { key_Setting.GetKeyCode(changed_Key)[0] , put_Button });
                        button.GetComponentInChildren<Text>().text = key_Setting.GetKeyCode(changed_Key)[0].ToString() + "\t|\tbutton " + i.ToString();
                        is_GamePad = true;
                        break;
                    }
                }
                //キーボード
                if (!is_GamePad) {
                    key_Setting.SetKey(changed_Key, new List<KeyCode> { put_Button, key_Setting.GetKeyCode(changed_Key)[1] });
                    button.GetComponentInChildren<Text>().text = put_Button.ToString() + "\t|\t" + key_Setting.GetKeyCode(changed_Key)[1].ToString();
                }
                break;
            }
            yield return null;
        }
        wait_Input = false;
        //色を戻す
        button.GetComponent<Image>().color = new Color(1, 1, 1);
        yield return null;
    }

    
}
