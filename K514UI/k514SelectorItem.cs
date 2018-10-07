using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514SelectorItem : MonoBehaviour {
    
    [System.NonSerialized]public Image m_main = null,m_thumbWaku = null,m_thumbImg = null;
    [System.NonSerialized]public Text  m_nodataText = null;
    void Start(){
        Init();
    }

    void Init(){
        if(m_main!=null) return;
        Image[] l_imgs = GetComponentsInChildren<Image>();
        m_main = l_imgs[0];
        m_thumbWaku = l_imgs[1];
        m_thumbImg = l_imgs[2];

        Text[] l_txts = GetComponentsInChildren<Text>();
        m_nodataText = l_txts[0];
        m_thumbImg.gameObject.SetActive(false);
    }

    void OnEnable(){
        Init();
    }

    public void Hide() {
        float l_alpha = 0.5f;
        m_main.color = m_thumbWaku.color = m_thumbImg.color = new Color(1f, 1f, 1f, l_alpha);
        l_alpha = 0f;
        m_nodataText.color = new Color(0f, 0f, 0f, l_alpha);
    }

    public void Open() {
        m_main.color = m_thumbWaku.color = m_thumbImg.color = new Color(1f, 1f, 1f, 1f);
        m_nodataText.color = new Color(0f, 0f, 0f, 1f);
    }

    public void SafeDelete() {
        m_thumbImg.sprite = null;
        m_thumbImg.gameObject.SetActive(false);
    }

}
