using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BottomBehindSetter : MonoBehaviour {

	void Start () {
		GetComponent<Image>().color = Camera.main.backgroundColor;
	}

}
