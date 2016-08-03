using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HomeController : MonoBehaviour, IPointerClickHandler {

	private RectTransform thisRect;
	private LevelLoader levelLoader;
	private bool animating;
	private float destination = 37.5f;
	private float goBack = 0f;

	void Start () {
		thisRect = GetComponent<RectTransform>();
		levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();
	}

	public void AnimateToggle () {
		animating = true;
		if (destination == 37.5f){
			destination = -37.5f;
			goBack = Time.time + 3f;
		} else {
			destination = 37.5f;
			goBack = 0f;
		}
	}

	void Update () {
		if (animating){
			if (Mathf.Abs(thisRect.anchoredPosition.y - destination) > 0.05f){
				thisRect.anchoredPosition = new Vector2(0f, Mathf.Lerp(thisRect.anchoredPosition.y, destination, 10f * Time.deltaTime));
			} else {
				thisRect.anchoredPosition = new Vector2(0f, destination);
				animating = false;
			}
		}
		if (goBack > 0f && Time.time > goBack){
			goBack = 0f;
			AnimateToggle();
		}
	}

	public void OnPointerClick( PointerEventData eventData ){
		if (eventData.pressPosition.x/Screen.width < 0.5f){
			levelLoader.LoadLevel(0);
		} else {
			levelLoader.ReloadLevel();
		}
	}
}
