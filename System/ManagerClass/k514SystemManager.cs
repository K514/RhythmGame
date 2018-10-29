using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514SystemManager : MonoBehaviour {

    // 1G

	public static k514AudioAnalyzer SYSTEM_AUDIO_ANALYZER = null;
    public static k514MathManager SYSTEM_MATH_MANAGER = null;
    public static k514SpectrumManager SYSTEM_SPECTRUM_MANAGER = null;
    public static k514PatternCatchManager SYSTEM_PATTERN_MANAGER = null;
	public static k514YoutubeData SYSTEM_YOUTUBE_DATA = null;
    public static k514SoundManager SYSTEM_SOUND_MANAGER = null;
    public static k514EnemySpawner SYSTEM_ENEMY_SPAWNER = null;
    public static k514PatternGenerator SYSTEM_PATTERN_GENERATOR = null;

    // 2G

    public static k514YoutubeApiConnector SYSTEM_YOUTUBE_CONNECTOR = null;
    public static k514YoutubeManager SYSTEM_YOUTUBE_MANAGER = null;


    // Use this for initialization
    void Start () {

        // 1G

		SYSTEM_AUDIO_ANALYZER = k514AudioAnalyzer.singleton;
        SYSTEM_MATH_MANAGER = k514MathManager.singleton;
        SYSTEM_SPECTRUM_MANAGER = k514SpectrumManager.singleton;
        SYSTEM_PATTERN_MANAGER = k514PatternCatchManager.singleton;
		SYSTEM_YOUTUBE_DATA = k514YoutubeData.singleton;
        SYSTEM_SOUND_MANAGER = k514SoundManager.singleton;
        SYSTEM_ENEMY_SPAWNER = k514EnemySpawner.singleton;
        SYSTEM_PATTERN_GENERATOR = k514PatternGenerator.singleton;

        // 2G

        SYSTEM_YOUTUBE_CONNECTOR = k514YoutubeApiConnector.singleton;
        SYSTEM_YOUTUBE_MANAGER = k514YoutubeManager.singleton;


        Debug.Log("System Manager loaded");
	}

	public enum PATTERN{
		RGB , RBG , GRB , GBR , BRG , BGR , END 
	}

	public enum DIRECT{
		LEFT = -1 , RIGHT = 1 
	}

	public enum SOUND_TYPE{
		SFX , BGM , END
	}

    public enum ENEMY_TYPE
    {
        REMILIA , END
    }

    public enum SPAWN_PATTERN
    {
        TEST0, TEST1, TEST2,END
    }

}
