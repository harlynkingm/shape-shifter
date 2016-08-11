using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour {

	public Texture filled;

	private int levelSet;

	void Start () {
		levelSet = int.Parse(gameObject.name);
		for (int i = 0; i < transform.childCount; i++){
			GameObject child = transform.GetChild(i).gameObject;
			LevelSelectButton newOne = child.AddComponent<LevelSelectButton>();
			newOne.levelSet = levelSet;
			newOne.filled = filled;
			newOne.Initialize();
		}
	}

}

public class LevelSelectButton : MonoBehaviour, IPointerClickHandler {

	public int levelSet;
	public Texture filled;

	private int levelNum;
	private LevelLoader loader;
	private SaveController save;

	public LevelSelectButton(int set, Texture fill){
		levelSet = set;
		filled = fill;
	}

	public void Initialize () {
		levelNum = int.Parse(gameObject.name) + ((levelSet - 1) * 10);
		loader = Camera.main.gameObject.GetComponent<LevelLoader>();
		save = Camera.main.gameObject.GetComponent<SaveController>();
		Color parentColor = gameObject.transform.parent.parent.GetComponent<Image>().color;
		if (save.canPlay(levelNum)){
			if (save.hasBeaten(levelNum)){
				GetComponent<RawImage>().texture = filled;
				transform.GetChild(0).GetComponent<Image>().color = parentColor;
			}
		} else {
			GetComponent<RawImage>().color = new Color(1f, 1f, 1f, 0.2f);
			transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
		}
	}
	
	public void OnPointerClick (PointerEventData data){
		if (save.canPlay(levelNum)){
			StartCoroutine(ReadyPlayer());
		} else {
			StartCoroutine(Flip());
		}
	}

	IEnumerator ReadyPlayer () {
		while (transform.localScale.x < 1.09f){
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.1f, Time.deltaTime * 7.5f);
			transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(transform.eulerAngles.z, 10f, Time.deltaTime * 7.5f));
			yield return null;
		}
		loader.LoadLevel(levelNum);
	}

	IEnumerator Flip () {
		float speed = Random.Range(1.5f, 2.5f);
		if (transform.rotation == Quaternion.identity){
			while (transform.eulerAngles.z < 359f){
				transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(transform.eulerAngles.z, 359.9f, Time.deltaTime * speed));
				yield return null;
			}
		}
		transform.rotation = Quaternion.identity;
	}

}
