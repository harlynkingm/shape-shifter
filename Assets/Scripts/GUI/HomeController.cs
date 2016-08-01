using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HomeController : MonoBehaviour, IPointerClickHandler {

	private RectTransform thisRect;
	private LevelLoader levelLoader;
	private bool animating;
	private float destination = 37.5f;

	void Start () {
		thisRect = GetComponent<RectTransform>();
		levelLoader = GameObject.FindWithTag("LevelLoader").GetComponent<LevelLoader>();
	}

	public void AnimateToggle () {
		animating = true;
		if (destination == 37.5f){
			destination = -37.5f;
		} else {
			destination = 37.5f;
		}
	}

	void Update () {
		if (animating){
			if (Mathf.Abs(thisRect.anchoredPosition.y - destination) > 0.0005f){
				thisRect.anchoredPosition = new Vector2(0f, Mathf.Lerp(thisRect.anchoredPosition.y, destination, 10f * Time.deltaTime));
			} else {
				thisRect.anchoredPosition = new Vector2(0f, destination);
				animating = false;
			}
		}
	}

	public void OnPointerClick( PointerEventData eventData ){
		levelLoader.LoadLevel(0);
	}
}
