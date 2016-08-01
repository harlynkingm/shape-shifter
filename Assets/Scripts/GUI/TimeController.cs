﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {

	public bool canPause = false;
	public GameObject play;
	public GameObject pause;
	public GameObject stop;
	public GameObject cancel;
	public GameObject completed;
	public Image white;

	private GameObject player;
	private Vector3 initPos;
	private String state = "Stop";
	private List<GameObject> states = new List<GameObject>();
	private GameObject[] shapes;
	private List<GameObject> reenable = new List<GameObject>();
	private bool ended = false;

	private Image thisBg;
	private float targetFade = 0.14F;

	void Start () {
		thisBg = GetComponent<Image>();
		shapes = GameObject.FindGameObjectsWithTag("ShapeObject");
		player = GameObject.FindWithTag("Player");
		initPos = player.transform.position;
		states.Add(play);
		states.Add(pause);
		states.Add(stop);
		states.Add(cancel);
		states.Add(completed);
		white.color = new Color(1.0F, 1.0F, 1.0F, 1.0F);
		StartCoroutine(Fade(white, 0F, -1));
	}

	void Update () {
		if (thisBg.color.a != targetFade){
			thisBg.color = new Color(1F, 1F, 1F, Mathf.MoveTowards(thisBg.color.a, targetFade, 0.05F));
		}
	}

	public void PrePress () {
		targetFade = 0.28F;
	}

	public void OnPress () {
		targetFade = 0.14F;
		switch (state) {
		case "Stop":
			Play();
			break;
		case "Play":
			if (canPause){
				Pause();
			} else {
				Stop();
			}
			break;
		case "Pause":
			Play();
			break;
		case "Cancel":
			break;
		case "Completed":
			NextLevel();
			break;
		}
	}

	void disableAll (GameObject except) {
		foreach (GameObject stateObj in states) {
			stateObj.SetActive(false);
		}
		except.SetActive(true);
	}

	void FreezeShapes () {
		foreach (GameObject shape in shapes) {
			ShapeObject s = shape.GetComponent<ShapeObject>();
			if (s.isSelectable) {
				reenable.Add(shape);
				ToggleShape(s, false);
			}
		}
	}

	void UnfreezeShapes () {
		foreach (GameObject shape in reenable) {
			ShapeObject s = shape.GetComponent<ShapeObject>();
			ToggleShape(s, true);
		}
		reenable.Clear();
	}

	void SetShapeTriggers (bool enabled) {
		foreach (GameObject shape in shapes){
			shape.GetComponent<Collider2D>().isTrigger = enabled;
		}
	}

	void ToggleShape (ShapeObject shape, bool isSelectable) {
		shape.isSelectable = isSelectable;
		shape.RefreshSelectable();
	}

	void Play() {
		if (canPause){
			disableAll(pause);
		} else {
			disableAll(stop);
		}
		state = "Play";
		player.GetComponent<Rigidbody2D>().isKinematic = false;
		player.GetComponent<CircleCollider2D>().isTrigger = false;
		SetShapeTriggers(false);
		FreezeShapes();
	}

	void Stop() {
		disableAll(play);
		state = "Stop";
		player.transform.position = initPos;
		player.GetComponent<Rigidbody2D>().isKinematic = true;
		player.GetComponent<CircleCollider2D>().isTrigger = true;
		SetShapeTriggers(true);
		UnfreezeShapes();
	}

	void Pause() {
		disableAll(play);
		state = "Pause";
		player.GetComponent<Rigidbody2D>().isKinematic = true;
		player.GetComponent<CircleCollider2D>().isTrigger = true;
		SetShapeTriggers(true);
		UnfreezeShapes();
	}

	public void Cancel() {
		if (!ended){
			disableAll(cancel);
			state = "Cancel";
		}
	}

	public void StopCancel() {
		if (!ended){
			disableAll(play);
			state = "Stop";
		}
	}

	public void EndLevel() {
		disableAll(completed);
		state = "Completed";
		ended = true;
	}

	public void ResetLevel() {
//		int curScene = SceneManager.GetActiveScene().buildIndex;
//		StartCoroutine(Fade(white, 1.0F, curScene));
		Stop();
	}

	public bool CheckStatus () {
		return ended;
	}

	void NextLevel () {
		int curScene = SceneManager.GetActiveScene().buildIndex;
		int totalScenes = SceneManager.sceneCountInBuildSettings;
		if (curScene + 1 < totalScenes){
			StartCoroutine(Fade(white, 1.0F, curScene + 1));
		}
	}

	IEnumerator Fade(Image target, float finalVal, int level) {
		while (target.color.a != finalVal){
			Color c = target.color;
			c.a = Mathf.MoveTowards(target.color.a, finalVal, 0.02F);
			target.color = c;
			yield return null;
		}
		if (level > -1){
			SceneManager.LoadScene(level);
		}
	}

}