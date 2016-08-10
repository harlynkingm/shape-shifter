using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayLevelButton : MonoBehaviour, IPointerClickHandler {

	public MyHorizontalScrollSnap levelset;
	public MyHorizontalScrollSnap level;

	private LevelLoader loader;

	void Start () {
		loader = GetComponent<LevelLoader>();
		GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.14f);
	}

	public void OnPointerClick ( PointerEventData eventData ) {
		int tens = (levelset.currentPage()) * 10;
		int ones = (level.currentPage() + 1);
		SaveController player = Camera.main.GetComponent<SaveController>();
		if (player.canPlay(tens + ones)){
			loader.LoadLevel(tens + ones);
		}
	}

	public void PrePress () {
		GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.28f);
	}

	public void ToNormal () {
		GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.14f);
	}


}
