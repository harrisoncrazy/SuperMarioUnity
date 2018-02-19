using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitySettings : MonoBehaviour {

	void Awake () {
		QualitySettings.vSyncCount = 1;
		Application.targetFrameRate = 60;
		Time.timeScale = 0.75f;
	}
}
