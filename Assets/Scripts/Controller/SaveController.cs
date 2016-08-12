using UnityEngine;
using System.Collections;

public class SaveController : MonoBehaviour {

	void Start () {
		SetBlank();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	void SetBlank() {
		if (!PlayerPrefs.HasKey("HighestLevel")){
			PlayerPrefs.SetInt("HighestLevel", 0);
		}
		if (!PlayerPrefs.HasKey("isMuted")){
			PlayerPrefs.SetInt("isMuted", 0);
		}
	}

	public void Reset () {
		PlayerPrefs.DeleteAll();
		SetBlank();
	}

	public bool canPlay (int level) {
		return level <= PlayerPrefs.GetInt("HighestLevel") + 1;
	}

	public bool hasBeaten (int level){
		return level <= PlayerPrefs.GetInt("HighestLevel");
	}

	public void FinishedLevel (int level) {
		int curHighest = PlayerPrefs.GetInt("HighestLevel");
		if (level > curHighest){
			PlayerPrefs.SetInt("HighestLevel", level);
		}
	}

	public void ToggleMute () {
		int curMuted = PlayerPrefs.GetInt("isMuted");
		if (curMuted == 1){
			curMuted = 0;
		} else {
			curMuted = 1;
		}
		PlayerPrefs.SetInt("isMuted", curMuted);
	}
}
