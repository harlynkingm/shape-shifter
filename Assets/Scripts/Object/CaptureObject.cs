using UnityEngine;
using System.Collections;
using System;

public class CaptureObject : MonoBehaviour {

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
		if (collected && sprite.color.a > .01){
			transform.localScale *= 1.1F;
			sprite.color = new Color(1F, 1F, 1F, sprite.color.a * 0.75F);
		} else if (collected) {
			gameObject.SetActive(false);
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Player" && !collected){
			collected = true;
			speed = 666F;
			fatherTime.EndLevel();
		}
	}
}
