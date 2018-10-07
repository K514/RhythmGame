using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514pooledPanningAudio : MonoBehaviour {
    
    bool m_pauseTrig = false, m_shutDownTrig = false;
    AudioSource m_audioSource = null;
    Transform m_panningTarget = null;

    public void Play(AudioClip p_clip,k514SystemManager.SOUND_TYPE p_soundType,Transform p_panningTarget = null){
        if(m_audioSource==null) m_audioSource = GetComponent<AudioSource>();
        if(p_soundType == k514SystemManager.SOUND_TYPE.BGM){
            this.m_audioSource.loop = true;
        }
        this.m_audioSource.clip = p_clip;
        this.m_panningTarget = p_panningTarget;
        StartCoroutine(PlaySound());
    }

    public void SetPause(bool p_pause){
        m_pauseTrig = p_pause;
    }

    public void ShutDown(){
        m_shutDownTrig = true;
    }

    IEnumerator PlaySound(){
        m_audioSource.Play();
        while(m_audioSource.isPlaying){
            Panning();
            while(this.m_pauseTrig || this.m_shutDownTrig){
                m_audioSource.Stop();
                if(m_shutDownTrig){
                    EndProcess();
                    yield break;
                }
                yield return null;
            }
            yield return null;
        }
        EndProcess();
    }

    void Panning(){
        if(m_panningTarget == null) return;
        transform.parent = null;
        transform.position = m_panningTarget.position;
        Vector3 viewport = Camera.main.WorldToViewportPoint(transform.position); // [0f,0f] ~ [1f,1f] viewport coordinate
        float distanceFromCenter = Mathf.InverseLerp(.75f, .25f, Vector2.Distance(viewport, new Vector2(.5f, .5f)));
        m_audioSource.volume =  distanceFromCenter;
        m_audioSource.panStereo = viewport.x * 2f - 1f;
    }

    void EndProcess(){
        transform.position = Vector3.zero;
        m_panningTarget = null;
        m_pauseTrig = false;
        m_shutDownTrig = false;
        m_audioSource.clip = null;
        m_audioSource.volume = 1f;
        m_audioSource.panStereo = 0f;
        m_audioSource.loop = false;
        k514SystemManager.SYSTEM_SOUND_MANAGER.ReturnPooledAudio(this);
    }

}