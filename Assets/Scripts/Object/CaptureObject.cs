using UnityEngine;
using System.Collections;
using System;

public class CaptureObject : MonoBehaviour {

	public Sprite nextShape;

	private float speed = 30F;
	private bool collected = false;
	private SpriteRenderer sprite;
	private TimeController fatherTime;

	void Start () {
		sprite = gameObject.GetComponent<SpriteRenderer>();
		fatherTime = GameObject.FindGameObjectWithTag("TimeController").GetComponent<TimeController>();
	}

	void Update () {
		transform.Rotate(Vector3.forward * speed * Time.deltaTime);
//		if (collected && sprite.color.a > .01){
//			transform.localScale *= 1.1F;
//			sprite.color = new Color(1F, 1F, 1F, sprite.color.a * 0.75F);
//		} else if (collected) {
//			gameObject.SetActive(false);
//		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player" && !collected){
			collected = true;
//			speed = 666F;
			fatherTime.EndLevel();
			StartCoroutine(AnimateOut());

		}
	}

	IEnumerator AnimateOut() {
		float maxScale = transform.localScale.x * 1.4f;
		while (speed < 999f){
			speed = Mathf.Lerp(speed, 1000f, Time.deltaTime * 16f);
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * maxScale, Time.deltaTime * 4f);
			yield return null;
		}
		sprite.sprite = nextShape;
		while (speed > 100f){
			speed = Mathf.Lerp(speed, 80f, Time.deltaTime * 3f);
			yield return null;
		}
		while (transform.localScale.x > 0.02f){
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 7.5f);
			yield return null;
		}
		transform.localScale = Vector3.zero;
	}
}
