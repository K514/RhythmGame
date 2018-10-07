using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514Distributor : MonoBehaviour {

    public static k514Distributor m_singleton = null;
    Dictionary<int,int> m_dictionary = null;
    Image[] m_UnitArray = null;
    #region inspector

    public Image m_graphMain;
    public Image m_Unit;
    public int m_range = 16;
    
    #endregion

    void Awake(){
        m_singleton = this;
    }

    void Start(){
        m_dictionary = new Dictionary<int,int>();
        Init();
    }

    void Init(){
        for(int i = 0 ; i < m_range ; i ++) m_dictionary.Add(i,0);
    
        float l_unitWidth = m_graphMain.rectTransform.rect.width/(m_range+4);
        float l_hGap = 120f / (m_range+4);
            
        m_UnitArray = new Image[m_range];
        for (int i = 0; i < m_range; i++) {
            m_UnitArray[i] = Instantiate<Image>(m_Unit,m_graphMain.transform);
            m_UnitArray[i].rectTransform.sizeDelta = new Vector2(l_unitWidth - l_hGap, m_UnitArray[i].rectTransform.rect.height - 50f);
            m_UnitArray[i].GetComponentsInChildren<Text>()[0].text = i.ToString();
            m_UnitArray[i].rectTransform.anchoredPosition = new Vector2(m_UnitArray[i].rectTransform.rect.width*0.6f + i*(l_hGap+l_unitWidth),25f);
        }    
    }

    public void AddData(int k){
        if(!m_dictionary.ContainsKey(k)){
            m_dictionary.Add(k,1);
        }else{
            m_dictionary[k]++;
        }
    }

    public void BurstForth(int timeStamp){
        var l_array = m_dictionary.Keys.ToArray();
        string l_burstForth = "";
        for(int i = 0 ; i < l_array.Length ; i++){
            l_burstForth += l_array[i] + " : " + m_dictionary[l_array[i]] + " , ";
        }
            Debug.Log("TimeStamps : " + timeStamp + " : " +l_burstForth);
        DrawGraph();
    }

    public void DrawGraph() {
        var array = m_dictionary.Keys.ToArray();
        int max_index, max_value = -1;
        for (int i = 0; i < array.Length; i++) {
            if (m_dictionary[array[i]] > max_value) {
                max_value = m_dictionary[array[i]];
                max_index = i;
            }
        }


        for(int i = 0 ; i < m_range ; i++){
            m_UnitArray[i].fillAmount = (float)m_dictionary[array[i]]/max_value;
            m_UnitArray[i].GetComponentsInChildren<Text>()[1].rectTransform.anchoredPosition = new Vector2(0f,m_UnitArray[i].fillAmount*(m_graphMain.rectTransform.rect.height-100f));
            m_UnitArray[i].GetComponentsInChildren<Text>()[1].text = m_dictionary[array[i]].ToString();
        }
    }

}