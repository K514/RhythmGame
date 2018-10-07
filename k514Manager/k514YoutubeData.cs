using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514YoutubeData : MonoBehaviour {

    public float m_samplingCount;
    public float m_analyzeInterval;
    [System.NonSerialized]public float m_sampleRate = 0f;

    public static k514YoutubeData singleton = null;
	void Awake(){
		if(singleton == null) singleton = this;
		else if(singleton != this) Destroy(gameObject);
    }
    
    void Start(){
        Init();
		Debug.Log("Youtube Data Loaded");
    }

    void Init() {
        m_sampleRate = 1f/m_samplingCount;
    }

}