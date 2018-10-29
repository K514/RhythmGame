using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514PatternCatchManager : MonoBehaviour {

    [System.NonSerialized]public float GREEN_SPECTRUM_FACTOR = 256;
    public Image m_PatternPanel;
    public k514EnemySpawnerSequence m_SpawnerSequence;
    public bool m_rgbPatternTrig = true, m_playNullPattern = true;
	public static k514PatternCatchManager singleton = null;

    int m_sequence = 0;
    float m_seqInterval = 0f;

    #region RGB_Analyze
    private int[] m_distribution;
    float m_tangent = 0f,m_lastTangent = 0f;
    float m_evaluated_nextPattern = 0f, m_evaluated_prevPattern = 0f;
    #endregion

    #region Spectrum_Analyze
    Vector2 m_prevSpectrum, m_nextSpectrum,m_offset;
    float m_leftMax = float.MinValue, m_leftMin = float.MaxValue, m_rightMax = float.MinValue, m_rightMin = float.MaxValue;
    public float m_sensitiveInterval = 5f;
    private float m_inversedSensitiveInterval = 0f;
    public float m_recognize_lowerBound = 5f;
    int m_invoked = 0,m_quaterSum = 0,m_evaluated = 0;
    #endregion

    void Awake(){
		if(singleton == null) singleton = this;
		else if(singleton != this) Destroy(gameObject);
    }

    void Start(){
        Init();
		Debug.Log("Pattern Manager Loaded");
    }

    void Init() {
        m_distribution = new int[(int)k514SystemManager.PATTERN.END];
        m_inversedSensitiveInterval = 1 / m_sensitiveInterval;
        m_seqInterval = k514SystemManager.SYSTEM_YOUTUBE_DATA.m_analyzeInterval;
    }

    // 스펙트럼 값을 추가함.
    public void AddSpectrumData(Vector2 p_spectrum) {
        Vector2 l_offset;
        
        m_nextSpectrum = p_spectrum;
        l_offset = m_nextSpectrum - m_prevSpectrum;

            // left spectrum
                if (m_offset.x > m_leftMax) m_leftMax = m_offset.x;
                if (m_offset.x < m_leftMin) m_leftMin = m_offset.x;

                float l_offsetElement = (l_offset.x - m_offset.x) * m_inversedSensitiveInterval;
                if (Mathf.Abs(l_offsetElement) >= m_recognize_lowerBound) {
                    // 1 2 4 8
                    m_quaterSum += k514SystemManager.SYSTEM_MATH_MANAGER.Pow(2, m_invoked);
                }

                            #region DEBUG
                            //  Debug.Log("left spectrum offset : " + m_offset.x + "  max : " + m_leftMax + "  min : " + m_leftMin);
                            #endregion
        // right spectrum

        m_prevSpectrum = m_nextSpectrum;
        m_offset = l_offset;
        m_invoked++;
        if (m_invoked == 4)
        {
            switch (m_quaterSum) {
                case 0:
                    //if(m_playNullPattern) k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.A, null);
                    break;
                case 1:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.I, null);
                    break;
                case 2:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.U, null);
                    break;
                case 3:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.E, null);
                    break;
                case 4:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.O, null);
                    break;
                case 5:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SA, null);
                    break;
                case 6:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SHI, null);
                    break;
                case 7:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SU, null);
                    break;
                case 8:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SE, null);
                    break;
                case 9:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SO, null);
                    break;
                case 10:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.TA, null);
                    break;
                case 11:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.KA, null);
                    break;
                case 12:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.KI, null);
                    break;
                case 13:
                    //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.KU, null);
                    break;
                case 14:
                   //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.KE, null);
                    break;
                default:
                    //if(m_playNullPattern) k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.KO, null);
                    break;
            }
            m_evaluated += m_quaterSum;
            // k514Distributor.m_singleton.AddData(m_evaluated);
            m_quaterSum = 0;
            m_invoked = 0;
        }
    }

    // 스펙트럼 값을 분석함.
    public void AnalyzeSpectrum() {
        m_sequence++;
        k514enemyController l_tmp = null;
        float l_timeoffset = 0f,l_timeToAdd = 0f;
        k514SystemManager.DIRECT l_direction = Random.Range(0, 2) == 0 ? k514SystemManager.DIRECT.LEFT : k514SystemManager.DIRECT.RIGHT;
        // Debug.Log("evaluated value" + m_evaluated);
        switch (m_evaluated)
        {
            case 15:
                l_tmp = k514SystemManager.SYSTEM_PATTERN_GENERATOR.RandomGenerateUpDownPattern(out l_timeoffset,Vector3.right*Random.Range(-24f,24f), l_direction);
                l_timeToAdd = m_sequence * m_seqInterval - l_timeoffset - 0.1f; // 0.1 is jitter expected
                break;
        }
        if (l_timeToAdd > 0f)
        {
            int l_up = (int)(10 * l_timeToAdd);
            l_tmp.gameObject.name = "enemy_at_" + l_up;
            m_SpawnerSequence.AddPatternToMap(l_up, () => { l_tmp.StartPattern(); });
        }

        m_evaluated = 0;
    }

    // 스펙트럼 분석 결과를 추가함.
    public void AddPatterndata(k514SystemManager.PATTERN l_pattern){
        m_distribution[(int)l_pattern]++;
        switch (l_pattern)
        {   
            // C-G 계열. 빈도수 가장 높음.
            // C-G-R-B
            case k514SystemManager.PATTERN.BGR:
                m_PatternPanel.color = Color.yellow;
                m_evaluated_nextPattern += 16;
                break;
            // C-G-B-R
            case k514SystemManager.PATTERN.RGB:
                m_PatternPanel.color = Color.green;
                m_evaluated_nextPattern += 2;
                break;

            // B-R 계열. 빈도수 높음.
            case k514SystemManager.PATTERN.BRG:
                m_PatternPanel.color = Color.blue;
                GREEN_SPECTRUM_FACTOR += 12.8f;
                m_evaluated_nextPattern += 4;
                
                //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SHOT1, null);
                break;
            case k514SystemManager.PATTERN.GBR:
                m_PatternPanel.color = Color.magenta;
                GREEN_SPECTRUM_FACTOR -= 12.8f;
                m_evaluated_nextPattern += 4;
                //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SHOT2, null);
                break;
            
            // R-B 계열. 빈도수 낮음.
            case k514SystemManager.PATTERN.RBG:
                m_PatternPanel.color = Color.red;
                GREEN_SPECTRUM_FACTOR += 12.8f;
                m_evaluated_nextPattern += 8;
                //k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SHOT0,null);
                break;
            case k514SystemManager.PATTERN.GRB:
                m_PatternPanel.color = Color.cyan;
                GREEN_SPECTRUM_FACTOR -= 12.8f;
                m_evaluated_nextPattern += 8;
                break;
        }
    }

    // 1초 혹은 적당한 시간 단위의 블록이 종료된 경우 호출, 이제까지 샘플링한 데이터를 가지고 패턴을 분석함.
    //
    // m_evaluate_next 값의 경우 등장할 수 있는 정수 구간
    //
    // youtube data 매니저의 샘플링 레이트(초당 샘플링 횟수) * 어날라이즈 간격(초단위)
    // 각 샘플링 별로 등장할 수 있는 값은 1에서 6이므로
    // [1,6] * sample_rate * analyze_interval
    // 기본값은 40 * 0.25 해서 10이므로
    // [10,60] 사이의 값이 등장한다.
    //
    public void AnalyzePattern(){
        
        m_tangent = (m_evaluated_prevPattern - m_evaluated_nextPattern);
        if (m_tangent * m_lastTangent != 0f && m_rgbPatternTrig) {
            // k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SHOT0,null);
        }

        #region DEBUG
            // Debug.Log(m_evaluated_nextPattern);
            //for (int i = 0; i < m_distribution.Length; i++) {
            //    l_tmp = (k514SystemManager.PATTERN)i;
            //    Debug.Log(i.ToString() + " : " + m_distribution[i] );
            //    m_distribution[i] = 0;
            //}
        #endregion

        float max = k514SystemManager.SYSTEM_YOUTUBE_DATA.SamplingCount * k514SystemManager.SYSTEM_YOUTUBE_DATA.m_analyzeInterval * 6;
        max = k514SystemManager.SYSTEM_SPECTRUM_MANAGER.m_GraphDomainMax / max;

        k514SystemManager.SYSTEM_SPECTRUM_MANAGER.AddPointW(new Vector2(max * m_evaluated_nextPattern,0f));

        m_lastTangent = (m_evaluated_prevPattern - m_evaluated_nextPattern);
        m_evaluated_prevPattern = m_evaluated_nextPattern;
        m_evaluated_nextPattern = 0f;
    }

}
