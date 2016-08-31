using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeController : MonoBehaviour {

	public GameObject play;
	public GameObject stop;
	public GameObject cancel;
	public GameObject completed;

	private GameObject player;
	private LevelLoader levelLoader;
	private AnalyticsController analytics;
	private SaveController save;
	private Vector3 initPos;
	private String state = "Stop";
	private List<GameObject> states = new List<GameObject>();
	private GameObject[] shapes;
	private GameObject[] rotates;
	private List<GameObject> reenable = new List<GameObject>();
	private bool ended = false;

	private Image thisBg;
	private float targetFade = 0.14F;
	private bool isCollapsing;

	void Start () {
		thisBg = GetComponent<Image>();
		shapes = GameObject.FindGameObjectsWithTag("ShapeObject");
		rotates = GameObject.FindGameObjectsWithTag("TiltObject");
		player = GameObject.FindWithTag("Player");
		levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();
		analytics = Camera.main.gameObject.GetComponent<AnalyticsController>();
		save = Camera.main.gameObject.GetComponent<SaveController>();
		initPos = player.transform.position;
		states.Add(play);
		states.Add(stop);
		states.Add(cancel);
		states.Add(completed);
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
		if (!ended && !isCollapsing){
			targetFade = 0.14F;
			switch (state) {
			case "Stop":
				Play();
				break;
			case "Play":
				Stop();
				break;
			case "Pause":
				Play();
				break;
			case "Cancel":
				break;
			}
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
			if (s != null && s.isSelectable) {
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

	void UnrotateShapes () {
		foreach (GameObject shape in rotates) {
			TiltObject s = shape.GetComponent<TiltObject>();
			s.Reset();
		}
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
		disableAll(stop);
		state = "Play";
		player.GetComponent<Rigidbody2D>().isKinematic = false;
		player.GetComponent<CircleCollider2D>().isTrigger = false;
		SetShapeTriggers(false);
		FreezeShapes();
		analytics.addAttempt();
	}

	void Stop() {
		disableAll(play);
		state = "Stop";
		player.transform.position = initPos;
		player.GetComponent<Rigidbody2D>().isKinematic = true;
		player.GetComponent<CircleCollider2D>().isTrigger = true;
		SetShapeTriggers(true);
		UnfreezeShapes();
		UnrotateShapes();
	}

	void ResetLoad(){
		levelLoader.loading.color = new Color(1f, 1f, 1f, 0f);
	}

	public void Cancel() {
		if (!ended){
			disableAll(cancel);
			state = "Cancel";
		}
	}

	public void StopCancel() {
		if (!ended && state != "Play"){
			disableAll(play);
			state = "Stop";
		}
	}

	public void EndLevel() {
		analytics.EndLevel();
		disableAll(completed);
		state = "Completed";
		ended = true;
		Invoke("NextLevel", 2f);
	}

	public void ResetLevel() {
		Stop();
	}

	public bool CheckStatus () {
		return ended;
	}

	void NextLevel () {
		int curScene = SceneManager.GetActiveScene().buildIndex;
		save.FinishedLevel(curScene);
		int totalScenes = SceneManager.sceneCountInBuildSettings;
		if (curScene + 1 < totalScenes){
			levelLoader.LoadLevel(curScene + 1);
		}

	}

	public bool getIsCollapsing () {
		return isCollapsing;
	}

	public IEnumerator Collapse (Vector3 moveTo) {
		isCollapsing = true;
		player.GetComponent<Rigidbody2D>().isKinematic = true;
		while (player.transform.localScale.x > 0.03f){
			player.transform.localScale = Vector3.Lerp(player.transform.localScale, Vector3.zero, Time.deltaTime * 10f);
			player.transform.position = Vector3.Lerp(player.transform.position, moveTo, Time.deltaTime * 3f);
			yield return null;
		}
		if (!ended){
			Stop();
			while (player.transform.localScale.x < 0.99f){
				player.transform.localScale = Vector3.Lerp(player.transform.localScale, Vector3.one, Time.deltaTime * 10f);
				yield return null;
			}
		}
		isCollapsing = false;
	}

}
