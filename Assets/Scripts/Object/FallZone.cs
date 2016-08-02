using UnityEngine;
using System.Collections;

public class FallZone : MonoBehaviour {

	TimeController fatherTime;

	void Start () {
		fatherTime = GameObject.FindGameObjectWithTag("TimeController").GetComponent<TimeController>();
	}

	void OnTriggerEnter2D (Collider2D coll) {
		if (!fatherTime.CheckStatus() && coll.gameObject.tag == "Player"){
			fatherTime.ResetLevel();
			Handheld.Vibrate();
		}
	}

}
