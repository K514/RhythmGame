using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514Test : MonoBehaviour {

    float elapsed = 0f;
	bool m_onceTrig = false;
	AudioSource m_audioSource;
    Vector2 m_Spectrum;

    public bool m_testSoundManager = false;

	void Start(){
		m_audioSource = GetComponent<AudioSource>();
	}
	void Update () {
        if (!m_onceTrig)
        {
            k514SystemManager.SYSTEM_AUDIO_ANALYZER.InitGraph();
            m_onceTrig = true;
        }
        else {
            //Choking();
        }

        if (m_testSoundManager) {
            m_testSoundManager = false;
            // k514SystemManager.SYSTEM_SOUND_MANAGER.CastSFX(k514SoundStorage.SFX_EFFECT.SHOT0);
        }
	}

    void Choking() {
        elapsed += Time.deltaTime;
        if (elapsed > k514SystemManager.SYSTEM_YOUTUBE_DATA.m_sampleRate)
        {
            elapsed -= k514SystemManager.SYSTEM_YOUTUBE_DATA.m_sampleRate;
            k514SystemManager.SYSTEM_PATTERN_MANAGER.AddPatterndata(
                k514SystemManager.SYSTEM_AUDIO_ANALYZER.AnaylzeSource(m_audioSource,out m_Spectrum)
            );
        }
	}
}
