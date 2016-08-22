using UnityEngine;
using System.Collections;

public class ObjectAnimation : MonoBehaviour {

	public Vector2 destination;
	public float speed = 3f;
	public float rotationSpeed = 0f;
	private Vector2 start;
	private Vector2 end;

	void Start () {
		start = (Vector2)transform.position;
		end = (Vector2)transform.position + destination;
		ResetAnim(start, end);
	}

	void Update () {
		transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
	}

	IEnumerator SmoothLerpLoop(Vector2 start, Vector2 end) {
		float t = 0f;
		while (t <= 1f){
			t += Time.deltaTime/speed;
			transform.position = Vector2.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));
			yield return null;
		}
		ResetAnim(end, start);
	}

	void ResetAnim (Vector2 start, Vector2 end) {
		StartCoroutine(SmoothLerpLoop(start, end));
	}

	void OnDrawGizmosSelected () {
		Gizmos.color = Color.gray;
		Vector2 drawDestination = (Vector2)transform.position + destination;
		Gizmos.DrawWireSphere(drawDestination, 0.2f);
		Gizmos.DrawLine(transform.position, drawDestination);
	}
}
