using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class k514SoundStorage : MonoBehaviour {
    public enum SFX_EFFECT{
        A,I,U,E,O,SA,SHI,SU,SE,SO,TA,KA,KI,KU,KE,KO,REMIUP,REMIDOWN,END
    }

    public enum SFX_RANDOM{
        END
    }

    public enum BGM
    {
        END
    }


    public AudioClip[] m_effects;
    public AudioClip[] m_bgms;
    public AudioClip[] m_shots, m_explosions;
    public RandomAudioStorage[] m_randoms;

    void Start() {
        m_randoms = new RandomAudioStorage[(int)SFX_RANDOM.END];
        AudioClip[] l_target = null;
        for (int i = 0; i < m_randoms.Length; i++) {
            switch ((SFX_RANDOM)i) {
                // case SFX_RANDOM.SHOT:
                //     l_target = m_shots;
                //     break;
                // case SFX_RANDOM.EXPLOSION:
                //     l_target = m_explosions;
                //     break;
            }
                     
            m_randoms[i] = new RandomAudioStorage()
            {
                m_storage = l_target
            };
        }
    }

    public class RandomAudioStorage {
        public AudioClip[] m_storage = null;
    }
}