using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ColorController : MonoBehaviour {

	public Color[] setColors;

	void Awake () {
		int curScene = SceneManager.GetActiveScene().buildIndex;
		if (curScene % 10 == 0){
			Camera.main.backgroundColor = setColors[Mathf.FloorToInt(curScene / 10f) - 1];
		} else {
			Camera.main.backgroundColor = setColors[Mathf.FloorToInt(curScene / 10f)];
		}
	}
}
