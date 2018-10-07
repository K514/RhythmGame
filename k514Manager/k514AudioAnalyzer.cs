using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514AudioAnalyzer : MonoBehaviour {

    public Image graphPanel;
	public Image Bar1,Bar2;
	public bool m_MaxSpectrumTrig = false;
    public enum OUT_SPECTRUM{
        R,G,B,END
    }
    public OUT_SPECTRUM m_outSpectrum = OUT_SPECTRUM.R;
    [Header("Must Be Pow of two")]
    public int BLOCK_NUMBER = 64;
    private List<Image> Bars1,Bars2;
    private float[] m_spectrum_left;
    private float[] m_spectrum_right;
	
	public static k514AudioAnalyzer singleton = null;
	void Awake(){
		if(singleton == null) singleton = this;
		else if(singleton != this) Destroy(gameObject);
    }

    void Start(){
        Init();
		Debug.Log("Audio Analyzer Loaded");
    }

    void Init() {
        Bars1 = new List<Image>();
        Bars2 = new List<Image>();
        m_spectrum_left = new float[BLOCK_NUMBER];
        m_spectrum_right = new float[BLOCK_NUMBER];
    }

    public void AnaylzeSourceLight(AudioSource _audioSource, out Vector2 out_Spectrum)
    {
        float result_bx = 0f, result_by = 0f;
        _audioSource.GetSpectrumData(m_spectrum_left, 0, FFTWindow.Hamming);
        _audioSource.GetSpectrumData(m_spectrum_right, 1, FFTWindow.Hamming);
        for (int i = 0; i < BLOCK_NUMBER; i++)
        {
            m_spectrum_left[i] *= k514SystemManager.SYSTEM_MATH_MANAGER.Funtion_Y_equal_X(i, 0.05f, 0.05f);
            m_spectrum_right[i] *= k514SystemManager.SYSTEM_MATH_MANAGER.Funtion_Y_equal_X(i, 0.05f, 0.05f);
            result_bx += (256 - i) * 0.005f * BLOCK_NUMBER * m_spectrum_left[i];
            result_by += (256 - i) * 0.005f * BLOCK_NUMBER * m_spectrum_right[i];
        }
            out_Spectrum = new Vector2(result_bx, result_by);
    }

    public k514SystemManager.PATTERN AnaylzeSource(AudioSource _audioSource, out Vector2 out_Spectrum) {
        //Debug.Log("channels : "+_audioSource.clip.channels);
        //Debug.Log("duration : " +_audioSource.clip.length);

        float result_x = 0f, result_y = 0f,result_ax=0f,result_ay = 0f, result_bx = 0f, result_by = 0f;

        _audioSource.GetSpectrumData(m_spectrum_left,0,FFTWindow.Hamming);
        _audioSource.GetSpectrumData(m_spectrum_right,1,FFTWindow.Hamming);

		for(int i = 0 ; i < BLOCK_NUMBER ; i++){
            //Debug.Log(i +" 's  [left : ]" + m_spectrum_left[i] + "[right : ]" + m_spectrum_right[i]);

            #region

                #region A graph
                // result a for a graph
                    result_ax += k514SystemManager.SYSTEM_PATTERN_MANAGER.GREEN_SPECTRUM_FACTOR*m_spectrum_left[i];
                    result_ay += k514SystemManager.SYSTEM_PATTERN_MANAGER.GREEN_SPECTRUM_FACTOR*m_spectrum_right[i];
                #endregion

            // convolution weight function from MathManager
            // weight function1 : f(x) = 0.05f*x + 0.05f
            // weight function2 : f(x) = 0.0005f*x*x + 0.025f
            m_spectrum_left[i] *= k514SystemManager.SYSTEM_MATH_MANAGER.Funtion_Y_equal_X(i,0.05f,0.05f);
            m_spectrum_right[i] *= k514SystemManager.SYSTEM_MATH_MANAGER.Funtion_Y_equal_X(i,0.05f,0.05f);

                #region B graph
                // result a for a graph
                    result_bx += (256-i) * 0.005f * BLOCK_NUMBER *m_spectrum_left[i];
                    result_by += (256-i) * 0.005f * BLOCK_NUMBER *m_spectrum_right[i];
                #endregion
            #endregion


            result_x += (i+1) * 0.01f * BLOCK_NUMBER * m_spectrum_left[i];
            result_y += (i+1) * 0.01f * BLOCK_NUMBER * m_spectrum_right[i];

            Bars1[i].fillAmount = m_MaxSpectrumTrig && Bars1[i].fillAmount >= m_spectrum_left[i] ? Bars1[i].fillAmount : m_spectrum_left[i]; 
            Bars2[i].fillAmount = m_MaxSpectrumTrig && Bars2[i].fillAmount >= m_spectrum_right[i] ? Bars2[i].fillAmount : m_spectrum_right[i];
        }
        
        k514SystemManager.SYSTEM_SPECTRUM_MANAGER.AddPointR(new Vector2(result_x, result_y));
        k514SystemManager.SYSTEM_SPECTRUM_MANAGER.AddPointG(new Vector2(result_ax, result_ay));
        k514SystemManager.SYSTEM_SPECTRUM_MANAGER.AddPointB(new Vector2(result_bx, result_by));

        out_Spectrum = new Vector2(result_bx, result_by);
        switch(m_outSpectrum){
            case OUT_SPECTRUM.R :
                out_Spectrum = new Vector2(result_x, result_y);
            break;
            case OUT_SPECTRUM.G :
                out_Spectrum = new Vector2(result_ax, result_ay);
            break;
        }

        float R = result_x + result_y;
        float G = result_ax + result_ay;
        float B = result_bx + result_by;
        
        if( R >= G ){
            if(B >= R){
                return k514SystemManager.PATTERN.BRG;
            }else if(B >= G){
                return k514SystemManager.PATTERN.RBG;
            }else{
                return k514SystemManager.PATTERN.RGB;
            }
        }else{
            if(B >= G){
                return k514SystemManager.PATTERN.BGR;
            }else if(B >= R){
                return k514SystemManager.PATTERN.GBR;
            }else{
                return k514SystemManager.PATTERN.GRB;
            }
        }
    }

	public void InitGraph(){
        Image bar = null;
        RectTransform canvas = graphPanel.GetComponentInParent<RectTransform>();
        float width = canvas.rect.width, height = canvas.rect.height;
        float gap = 100f/BLOCK_NUMBER;
        float bar_width = width/ BLOCK_NUMBER - gap;
        for (int i = 0; i < BLOCK_NUMBER; i++) {
                bar = Instantiate<Image>(Bar1,graphPanel.transform);
                bar.rectTransform.anchoredPosition = new Vector2(width*-0.5f + (bar_width+gap)*i + bar_width*0.5f ,0f);
                bar.rectTransform.sizeDelta = new Vector2(bar_width, height);
                bar.fillAmount = 0f;
				Bars1.Add(bar);
        }
        for (int i = 0; i < BLOCK_NUMBER; i++)
        {
            bar = Instantiate<Image>(Bar2, graphPanel.transform);
            bar.rectTransform.anchoredPosition = new Vector2(width * -0.5f + (bar_width + gap) * i + bar_width * 0.5f, 0f);
            bar.rectTransform.sizeDelta = new Vector2(bar_width, height);
			bar.fillAmount = 0f;
			Bars2.Add(bar);
        }
    }

	
}
