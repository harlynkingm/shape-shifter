using UnityEngine;
using System.Collections;

public class BlackHole : MonoBehaviour {

	public float speed = -45f;

	private TimeController fatherTime;

	void Start () {
		fatherTime = GameObject.FindGameObjectWithTag("TimeController").GetComponent<TimeController>();
	}

	void Update () {
		transform.Rotate(Vector3.forward * speed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Player" && !fatherTime.getIsCollapsing()){
			StartCoroutine(fatherTime.Collapse(transform.position));
		}
	}

}
