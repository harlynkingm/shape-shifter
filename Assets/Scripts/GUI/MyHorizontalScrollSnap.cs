using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MyHorizontalScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {

	public GameObject[] pages;
	public int startPage;
	public bool scale = false;

	private ScrollRect scroll;
	private Transform screensContainer;
	private List<float> pageLocs= new List<float>();
	private List<RectTransform> screens = new List<RectTransform>();

	private int curPage;
	private bool lerp = false;

	private float startDragTime;

	void Start () {
		scroll = GetComponent<ScrollRect>();
		for (int i = 0; i < scroll.content.transform.childCount; i++){
			RectTransform curRect = scroll.content.transform.GetChild(i).GetComponent<RectTransform>();
			screens.Add(curRect);
			if (scale) curRect.gameObject.AddComponent<ScaleControl>().scale = 0.75f;
		}
		screensContainer = scroll.content;
		float pageScrollWidth = 1f/(screens.Count - 1);
		for (int i = 0; i < screens.Count; i++){
			scroll.horizontalNormalizedPosition = i * pageScrollWidth;
			pageLocs.Add(screensContainer.localPosition.x);
		}
		scroll.horizontalNormalizedPosition = pageScrollWidth * startPage;
		curPage = findClosestPage(screensContainer.localPosition.x);
		if (scale){
			screens[curPage].GetComponent<ScaleControl>().scale = 1f;
			screens[curPage].localScale = Vector3.one;
		}
	}

	void Update () {
		if (lerp){
			if (Mathf.Abs(screensContainer.localPosition.x - pageLocs[curPage]) > 1f){
				screensContainer.localPosition = new Vector2(Mathf.Lerp(screensContainer.localPosition.x, pageLocs[curPage], 7.5f * Time.deltaTime), screensContainer.localPosition.y);
			} else {
				screensContainer.localPosition = new Vector2(pageLocs[curPage], screensContainer.localPosition.y);
				lerp = false;
				if (pages.Length > 0){
					disableAll(pages[curPage]);
				}
			}
		}
	}

	float diff (float a, float b){
		return Mathf.Abs(a - b);
	}

	int findClosestPage(float loc){
		int pageRec = -1;
		float minDiff = Mathf.Infinity;
		for (int page = 0; page < pageLocs.Count; page++){
			float curDiff = diff(loc, pageLocs[page]);
			if (curDiff < minDiff){
				minDiff = curDiff;
				pageRec = page;
			}
		}
		return pageRec;
	}

	public void OnBeginDrag (PointerEventData data) {
		if (data.pointerId < 1){
			lerp = false;
			startDragTime = Time.time;
			if (scale) screens[curPage].GetComponent<ScaleControl>().Shrink();
		}
	}
		
	public void OnEndDrag (PointerEventData data) {
		if (data.pointerId < 1){
			float scrollVelocity = (data.position.x - data.pressPosition.x)/(Time.time - startDragTime);
			if (Mathf.Abs(scrollVelocity) < 2000f){
				curPage = findClosestPage(screensContainer.localPosition.x);
			} else {
				if (scrollVelocity < 0f){
					NextPage();
				} else {
					PrevPage();
				}
			}
			SetStart();
		}
	}

	public void OnPointerClick (PointerEventData data) {
		if (data.pointerId < 1){
			if (Mathf.Abs(data.position.x - data.pressPosition.x) < 1f){
				NextPage();
			}
		}
	}

	public void NextPage(){
		if ((curPage + 1) < screens.Count){
			if (scale) screens[curPage].GetComponent<ScaleControl>().Shrink();
			curPage++;
			SetStart();
		}
	}

	public void PrevPage(){
		if ((curPage - 1) >= 0){
			if (scale) screens[curPage].GetComponent<ScaleControl>().Shrink();
			curPage--;
			SetStart();
		}
	}

	void SetStart () {
		lerp = true;
		if (scale) screens[curPage].GetComponent<ScaleControl>().Grow();
	}

	void disableAll (GameObject except) {
		foreach (GameObject page in pages) {
			page.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
		}
		except.GetComponent<Image>().color = Color.white;
	}

	public int currentPage () {
		return curPage;
	}

	public void Reset () {
		if (scale) screens[curPage].GetComponent<ScaleControl>().Shrink();
		lerp = true;
		curPage = startPage;
		if (scale){
			screens[curPage].GetComponent<ScaleControl>().scale = 1f;
			screens[curPage].localScale = Vector3.one;
		}
	}

}

public class ScaleControl : MonoBehaviour {
	public float scale;

	private RectTransform self;

	void Start () {
		self = GetComponent<RectTransform>();
		self.localScale = Vector3.one * scale;
	}

	void Update () {
		if (self.localScale.x != scale){
			self.localScale = Vector3.Lerp(self.localScale, Vector3.one * scale, Time.deltaTime * 10f);
		}
	}

	public void Shrink () {
		scale = 0.75f;
	}

	public void Grow () {
		scale = 1f;
	}
}
