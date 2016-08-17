using UnityEngine;
using System.Collections;

public class FallZone : MonoBehaviour {

	TimeController fatherTime;

	void Start () {
		fatherTime = GameObject.FindGameObjectWithTag("TimeController").GetComponent<TimeController>();
		transform.position = new Vector3(0, -10f);
	}

	void OnTriggerEnter2D (Collider2D coll) {
		if (!fatherTime.CheckStatus() && coll.gameObject.tag == "Player"){
			fatherTime.ResetLevel();
		}
	}

}
