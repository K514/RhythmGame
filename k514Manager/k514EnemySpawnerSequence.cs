using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Debug = UnityEngine.Debug;

public class k514EnemySpawnerSequence : MonoBehaviour {

    public bool m_OnceTrig = true;

    public VideoPlayer m_videoPlayer,m_frontPlayer;
    public AudioSource m_audioSource,m_frontAudio;
    public RawImage m_img;

    int m_first = 0,m_second = 0;
    Dictionary<int,Action> m_patternMap;

    public  const float m_playBackInterval = 5f;

    void Start(){
        m_patternMap = new Dictionary<int,Action>();
    }

    void Update(){
        if(m_OnceTrig)
        {
            m_OnceTrig = false;
            StartCoroutine(LoadUrl(AudioPlay));
        }
    }

    IEnumerator LoadUrl(Action p_callback) {
        k514SystemManager.SYSTEM_YOUTUBE_MANAGER.GetUrl();
        float l_elapsed = 0f;
        while (k514YoutubeManager.SelectedUrl == null && l_elapsed < 3f) {
            l_elapsed += Time.deltaTime;
            Debug.Log(l_elapsed);
            yield return new WaitForEndOfFrame(); // busy waiting
        }
        if (l_elapsed >= 3f)
        {
            m_OnceTrig = true;
        }
        else
        {
            Debug.Log(k514YoutubeManager.SelectedUrl);
            p_callback.Invoke();
        }
    }

    void AudioPlay() {
        m_videoPlayer.source = m_frontPlayer.source = VideoSource.Url;
        m_frontPlayer.url = k514YoutubeManager.SelectedUrl;
        m_videoPlayer.url = k514YoutubeManager.SelectedUrl;
        m_frontPlayer.Prepare();
        m_videoPlayer.Prepare();

        StartCoroutine(PlaySource(AudioPlay));
    }

    IEnumerator PlaySource(Action p_fail) {
        float elapsed = 0f;
        while (!m_frontPlayer.isPrepared && !m_videoPlayer.isPrepared && elapsed < 3f) {
            elapsed += Time.deltaTime;
            Debug.Log(elapsed);
            yield return new WaitForEndOfFrame();
        }

        if (elapsed >= 3f) {
            p_fail.Invoke();
            yield break;
        }

        Debug.Log("Play" + Time.time);
        m_videoPlayer.Play();
        StartCoroutine(playBackedPlaySource());

        int invoked = 0;
        float lastStamp = 0;
        float elasped_s = 0f, lastWaited = 0f;
        Stopwatch m_RTcounter = new Stopwatch();
        Vector2 l_weight;
        while (m_videoPlayer.isPlaying) {
            invoked++;

            m_RTcounter.Start();
                    k514SystemManager.SYSTEM_AUDIO_ANALYZER.AnaylzeSourceLight(m_audioSource, out l_weight);
                    k514SystemManager.SYSTEM_PATTERN_MANAGER.AddSpectrumData(l_weight);

            m_RTcounter.Stop();
                    lastWaited = k514SystemManager.SYSTEM_YOUTUBE_DATA.m_sampleRate - (float)m_RTcounter.Elapsed.TotalMilliseconds * 0.001f;
                    elasped_s += k514SystemManager.SYSTEM_YOUTUBE_DATA.m_sampleRate;

            m_RTcounter.Reset();

                    if (invoked >= 3)
                    {
                        m_first++;
                        invoked = 0;
                        elasped_s -= k514SystemManager.SYSTEM_YOUTUBE_DATA.m_analyzeInterval;
                        //Debug.Log("Analyzed at : "+ m_first + " : " + Time.time + " : " + (Time.time - lastStamp));
                        lastStamp = Time.time;
                        k514SystemManager.SYSTEM_PATTERN_MANAGER.AnalyzeSpectrum();
                    }

            yield return new WaitForSeconds(lastWaited);

        }
    }

    IEnumerator playBackedPlaySource()
    {
        yield return new WaitForSeconds(m_playBackInterval);
        Debug.Log("Front Play Launched : " + Time.time);
        m_img.texture = m_frontPlayer.texture;
        m_frontPlayer.Play();
        float l_interval = k514SystemManager.SYSTEM_YOUTUBE_DATA.m_analyzeInterval;
        float lastWaited = 0f;
        Stopwatch m_RTcounter = new Stopwatch();
        while (m_frontPlayer.isPlaying)
        {
            m_RTcounter.Start();
            Active();
            m_second++;
            m_RTcounter.Stop();
            lastWaited = l_interval - (float)m_RTcounter.Elapsed.TotalMilliseconds * 0.001f;
            m_RTcounter.Reset();

            yield return new WaitForSeconds(lastWaited);
        }
    }

    public void AddPatternToMap(int p_up, Action p_data) {
        if (!m_patternMap.ContainsKey(p_up))
        {
        //Debug.Log("Saved At : " + p_up);
            m_patternMap.Add(p_up, p_data);
        }
    }

    public void Active() {
        int l_tmp = m_first - m_second;
        // if (m_second % 10 == 0) k514Distributor.m_singleton.BurstForth(m_second/10);
        Debug.Log(l_tmp);
        //Debug.Log("Actived At : " + p_time + " : " + Time.time);
        if (m_patternMap.ContainsKey(m_second))
        {
            m_patternMap[m_second].Invoke();
        }
        if(l_tmp > 50){
            m_second++;
            if (m_patternMap.ContainsKey(m_second))
            {
                m_patternMap[m_second].Invoke();
            }
        }
    }

}
