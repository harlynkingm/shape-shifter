using UnityEngine;
using System.Collections;
using UnityEngine.Analytics;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class AnalyticsController : MonoBehaviour {

	private float timeStarted;
	private int attempts = 0;

	void Start () {
		timeStarted = Time.time;
	}
	
	public void addAttempt () {
		attempts++;
	}

	public void EndLevel () {
		Analytics.CustomEvent("Level" + SceneManager.GetActiveScene().name, new Dictionary<string, object>{
			{"playDuration", Time.time - timeStarted},
			{"attempts", attempts}
		});
	}
}
