using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MyHorizontalScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {

	public GameObject[] pages;

	private ScrollRect scroll;
	private Transform screensContainer;
	private List<float> pageLocs= new List<float>();

	private int curPage;
	private bool lerp = false;

	private float startDragTime;

	void Start () {
		scroll = GetComponent<ScrollRect>();
		screensContainer = scroll.content;
		float pageScrollWidth = 1f/(pages.Length - 1);
		for (int i = 0; i < pages.Length; i++){
			scroll.horizontalNormalizedPosition = i * pageScrollWidth;
			pageLocs.Add(screensContainer.localPosition.x);
		}
		scroll.horizontalNormalizedPosition = pageScrollWidth;
		curPage = findClosestPage(screensContainer.localPosition.x);
	}

	void Update () {
		if (lerp){
			if (Mathf.Abs(screensContainer.localPosition.x - pageLocs[curPage]) > 1f){
				screensContainer.localPosition = new Vector2(Mathf.Lerp(screensContainer.localPosition.x, pageLocs[curPage], 12f * Time.deltaTime), screensContainer.localPosition.y);
			} else {
				screensContainer.localPosition = new Vector2(pageLocs[curPage], screensContainer.localPosition.y);
				lerp = false;
				disableAll(pages[curPage]);
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
		lerp = false;
		startDragTime = Time.time;
	}
		
	public void OnEndDrag (PointerEventData data) {
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

	public void OnPointerClick (PointerEventData data) {
		if (Mathf.Abs(data.position.x - data.pressPosition.x) < 1f){
			NextPage();
		}
	}

	public void NextPage(){
		if ((curPage + 1) < pages.Length){
			curPage++;
			SetStart();
		}
	}

	public void PrevPage(){
		if ((curPage - 1) >= 0){
			curPage--;
			SetStart();
		}
	}

	void SetStart () {
		lerp = true;
	}

	void disableAll (GameObject except) {
		foreach (GameObject page in pages) {
			page.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
		}
		except.GetComponent<Image>().color = Color.white;
	}
}
