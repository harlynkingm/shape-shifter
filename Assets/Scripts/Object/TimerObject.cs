using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimerObject : MonoBehaviour {

	public SpriteRenderer[] countdown;
	public float delay = 1f;
	public float waitTime = 2f;
	public bool disappear = true;
	public GameObject trigger;
	public GameObject cllider;

	private float grey = 0.7f;

	private int curTime = 0;

	void Start () {
		Invoke("AddTime", delay);
		trigger.transform.parent = null;
		for (int i = 0; i < countdown.Length; i++){
			countdown[i].color = new Color(grey, grey, grey, 1f);
		}
	}

	void AddTime () {
		if (curTime < countdown.Length){
			countdown[curTime].color = new Color(grey, grey, grey, 0f);
		}
		curTime++;
		if (curTime > countdown.Length){
			DoAction();
		} else {
			Invoke("AddTime", 1f);
		}
	}

	void DoAction () {
		if (disappear){
			StartCoroutine(Disappear());
		}
	}

	IEnumerator Disappear(){
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		while (transform.localScale.x > 0.665f){
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0.66f, Time.deltaTime * 7f);
			cllider.transform.localScale = Vector3.Lerp(cllider.transform.localScale, Vector3.zero, Time.deltaTime * 20f);
			renderer.color = Color.Lerp(renderer.color, new Color(1f, 1f, 1f, 0.2f), Time.deltaTime * 7f);
			yield return null;
		}
		transform.localScale = Vector3.one * 0.66f;
		cllider.GetComponent<Collider2D>().enabled = false;
		renderer.color = new Color(1f, 1f, 1f, 0.2f);
		yield return new WaitForSeconds(waitTime);
		StartCoroutine(Reappear());
	}

	IEnumerator Reappear(){
		cllider.GetComponent<Collider2D>().enabled = true;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		for (int i = 0; i < countdown.Length; i++){
			countdown[i].color = new Color(0f, 0f, 0f, 0f);
		}
		while (transform.localScale.x < 0.995f){
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 7f);
			cllider.transform.localScale = Vector3.Lerp(cllider.transform.localScale, Vector3.one, Time.deltaTime * 20f);
			renderer.color = Color.Lerp(renderer.color, Color.white, Time.deltaTime * 7f);
			for (int i = 0; i < countdown.Length; i++){
				countdown[i].color = Color.Lerp(countdown[i].color, new Color(grey, grey, grey, 1f), Time.deltaTime * 7f);
			}
			yield return null;
		}
		transform.localScale = Vector3.one;
		renderer.color = Color.white;
		for (int i = 0; i < countdown.Length; i++){
			countdown[i].color = new Color(grey, grey, grey, 1f);
		}
		yield return new WaitForSeconds(1f);
		Reset();
	}

	void Reset(){
		curTime = 0;
		AddTime();
	}

}
