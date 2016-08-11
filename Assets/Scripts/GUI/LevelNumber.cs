using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelNumber : MonoBehaviour {

	public Sprite[] numbers = new Sprite[10];
	public bool isSet;

	void Start () {
		int curScene = SceneManager.GetActiveScene().buildIndex;
		if (isSet){
			if (curScene % 10 == 0){
				GetComponent<Image>().sprite = numbers[Mathf.FloorToInt(curScene / 10f)];
			} else {
				GetComponent<Image>().sprite = numbers[Mathf.FloorToInt(curScene / 10f) + 1];
			}
		} else {
			GetComponent<Image>().sprite = numbers[curScene % numbers.Length];
		}
	}

}
