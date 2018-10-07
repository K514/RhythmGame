using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514UIAnimator : MonoBehaviour {
	Animator m_anim;
    public enum UI{
        BG,SAKU,REMI,PATT        
    }
    public UI toPlay;

    void Start(){
        m_anim = GetComponent<Animator>();
        m_anim.Play(toPlay.ToString());
    }

    private void OnEnable()
    {
        if(m_anim!=null )m_anim.Play(toPlay.ToString());
    }
}
