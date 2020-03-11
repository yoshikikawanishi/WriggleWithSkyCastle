using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BGM {
    public readonly string name;
    public AudioClip clip;
    public float volume;

    public BGM(string name) {
        this.name = name;
    }
}

[System.Serializable]
public class BGMManager : MonoBehaviour{      
    
    public List<BGM> BGM_List = new List<BGM>() {
        new BGM("Stage1"),
        new BGM("Stage1_Boss"),
        new BGM("Stage2"),
        new BGM("Stage2_Boss"),
        new BGM("Stage3"),
    };

    private BGM now_BGM;
    private AudioSource audio_Source;   


    //シングルトン用
    public static BGMManager Instance;
    void Awake() {        
        //シングルトン
        if (Instance != null) {
            Destroy(this.gameObject);
        }
        else if (Instance == null) {
            Instance = this;
        }
        //シーンを遷移してもオブジェクトを消さない
        DontDestroyOnLoad(gameObject);
    }


    // Use this for initialization
    void Start () {
        audio_Source = GetComponent<AudioSource>();

        #if UNITY_EDITOR
        if (DebugModeManager.Instance.Is_Delete_BGM) {
            audio_Source.enabled = false;
        }
        #endif
    }
	

    //名前でリストからBGMを取得する
    private BGM Get_BGM(string name) {
        foreach(BGM b in BGM_List) {
            if (b.name == name)
                return b;
        }
        Debug.Log(name + " BGM is not Exist");
        return null;
    }


    /// <summary>
    /// BGMを変更する
    /// </summary>
    /// <param name="name">変更先のBGM名</param>
    public void Change_BGM(string name) {
        StopCoroutine("Fade_Out_Cor");

        BGM next_BGM = Get_BGM(name);        

        if(now_BGM != next_BGM) {
            audio_Source.clip = next_BGM.clip;
            audio_Source.volume = next_BGM.volume;
            audio_Source.Play();
            now_BGM = next_BGM;
        }
    }

   
    /// <summary>
    /// BGMを止める
    /// </summary>
    public void Stop_BGM() {
        audio_Source.Stop();
        now_BGM = null;
    }


    /// <summary>
    /// BGMのフェードアウト
    /// </summary>
    public void Fade_Out() {
        StartCoroutine("Fade_Out_Cor");
    }

    private IEnumerator Fade_Out_Cor() {
        while(audio_Source.volume > 0) {
            audio_Source.volume -= 0.005f;
            yield return null;
        }
        audio_Source.Stop();
    }
    
}
