using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioAnalyzePreset : MonoBehaviour {

	#region <Fields>

    public float SamplingCount;
    public float AnalyzeStep;

	#endregion

	#region <UnityCallbacks>

	void Awake(){
		SamplingCount = 1f/SamplingCount;
    }

	#endregion
}