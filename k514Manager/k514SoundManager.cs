using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514SoundManager : MonoBehaviour {
    
    public static k514SoundManager singleton = null;
    public k514pooledPanningAudio m_prefab;
    public k514SoundStorage m_storage;

    List<k514pooledPanningAudio> m_audioTank = null;
    List<k514pooledPanningAudio> m_usedAudioTank = null;

    void Awake(){
        if(singleton == null) singleton = this;
        else if(singleton != this) Destroy(this);
    }

    void Start(){
        init();
        Debug.Log("SoundManager Loaded.");
    }

    void init(){
        m_audioTank = new List<k514pooledPanningAudio>();
        m_usedAudioTank = new List<k514pooledPanningAudio>();
    }

    k514pooledPanningAudio GetAvailable() {
        k514pooledPanningAudio l_result = null;
        if (m_audioTank.Count > 0)
        {
            l_result = m_audioTank[m_audioTank.Count - 1];
            m_audioTank.Remove(l_result);
            l_result.gameObject.SetActive(true);
        }
        else
        {
            l_result = GameObject.Instantiate<k514pooledPanningAudio>(m_prefab);
            l_result.transform.parent = transform;
        }

        m_usedAudioTank.Add(l_result);
        return l_result;
    }

    public k514pooledPanningAudio CastSFX(k514SoundStorage.SFX_EFFECT p_type, Transform p_target = null) {
        k514pooledPanningAudio l_result = GetAvailable();
        l_result.Play(m_storage.m_effects[(int)p_type],k514SystemManager.SOUND_TYPE.SFX,p_target);
        return l_result;
    }

    public k514pooledPanningAudio CastRandomSFX(k514SoundStorage.SFX_RANDOM p_type, Transform p_target = null)
    {
        k514pooledPanningAudio l_result = GetAvailable();
        l_result.Play(m_storage.m_randoms[(int)p_type].m_storage[Random.Range(0, m_storage.m_randoms[(int)p_type].m_storage.Length)], k514SystemManager.SOUND_TYPE.SFX, p_target);
        return l_result;
    }

    public k514pooledPanningAudio CastBGM(k514SoundStorage.BGM p_type, Transform p_target = null) {
        k514pooledPanningAudio l_result = GetAvailable();
        l_result.Play(m_storage.m_bgms[(int)p_type], k514SystemManager.SOUND_TYPE.BGM, p_target);
        return l_result;
    }

    public void ReturnPooledAudio(k514pooledPanningAudio p_pooled) {
        m_usedAudioTank.Remove(p_pooled);
        m_audioTank.Add(p_pooled);
        p_pooled.transform.parent = transform;
        p_pooled.gameObject.SetActive(false);
        
    }


}