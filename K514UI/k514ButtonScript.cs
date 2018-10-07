using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514ButtonScript : MonoBehaviour {
	public k514EventListener m_ev;
    public enum BUTTON_TYPE {
        SEARCH,BACK,LEFT,RIGHT
    }
    public BUTTON_TYPE m_Button;

	public void Search() {
		Debug.Log(m_Button.ToString()+ "  Button Clicked");
        switch (m_Button)
        {
            case BUTTON_TYPE.SEARCH:
                m_ev.InvokeSearchButtonCallBack();
                break;
            case BUTTON_TYPE.BACK:
                m_ev.BackToSearch();
                break;
            case BUTTON_TYPE.LEFT:
                m_ev.m_leftSlide = true;
                break;
            case BUTTON_TYPE.RIGHT:
                m_ev.m_rightSlide = true;
                break;
        }
	}

}
