using UnityEngine;
using System.Collections;

public class ExpandShrink : MonoBehaviour {

	public GameObject[] list;

	private float speed = 4f;
	private float scaleFactor = 1.3f;

	void Start() {
		StartCoroutine(growShrink(0));
	}

	IEnumerator growShrink (int index){
		Transform cur = list[index].transform;
		float origScale = cur.localScale.x;
		float maxScale = cur.localScale.x * scaleFactor;
		while (list[index].transform.localScale.x < maxScale * 0.98f){
			cur.localScale = Vector3.Lerp(cur.localScale, Vector3.one * maxScale, Time.deltaTime * speed);
			yield return null;
		}
		cur.localScale = Vector3.one * maxScale;
		while (list[index].transform.localScale.x > origScale * 1.02f){
			cur.localScale = Vector3.Lerp(cur.localScale, Vector3.one * origScale, Time.deltaTime * speed);
			yield return null;
		}
		cur.localScale = Vector3.one * origScale;
		StartCoroutine(growShrink((index + 1) % list.Length));
	}
}
