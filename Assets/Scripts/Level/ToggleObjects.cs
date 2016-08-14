using UnityEngine;
using System.Collections;

public class ToggleObjects : MonoBehaviour {

	public GameObject A;
	public GameObject B;

	public void Toggle(){
		A.SetActive(!A.activeSelf);
		B.SetActive(!B.activeSelf);
	}
}
