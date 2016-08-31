using UnityEngine;
using System.Collections;

public class TiltObject : MonoBehaviour {

	public Vector3 destination;
	private Vector3 start;

	private bool done = false;
	private bool rotating = false;

	void Start(){
		start = transform.eulerAngles;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Player" && !done){
			rotating = true;
			done = true;
		}
	}

	void Update(){
		if (rotating && Mathf.Abs(transform.eulerAngles.z - destination.z) > 0.01f){
			transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, destination, Time.deltaTime * 10f);
		} else if (rotating) {
			transform.eulerAngles = destination;
			rotating = false;
		}
	}

	public void Reset(){
		transform.eulerAngles = start;
		done = false;
		rotating = false;
	}

}
