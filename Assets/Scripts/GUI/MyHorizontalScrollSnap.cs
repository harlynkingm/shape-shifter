using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MyHorizontalScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler {

	public GameObject[] pages;

	private ScrollRect scroll;
	private List<float> pageLocs= new List<float>();

	private int curPage;
	private float endTime = 0f;
	private float animLength = 0.2f;
	private float startPos;

	void Start () {
		scroll = GetComponent<ScrollRect>();
		float pageScrollWidth = 1f/(pages.Length - 1);
		for (int i = 0; i < pages.Length; i++){
			pageLocs.Add(i * pageScrollWidth);
		}
		curPage = findClosestPage(scroll.horizontalNormalizedPosition);
	}

	void Update () {
		if (endTime > 0f){
			float progress = Mathf.Clamp01(1f - ((endTime - Time.time)/animLength));
			if (progress < 1f){
				scroll.horizontalNormalizedPosition = Mathf.SmoothStep(startPos, pageLocs[curPage], progress);
			} else {
				scroll.horizontalNormalizedPosition = pageLocs[curPage];
				endTime = 0f;
				disableAll(pages[curPage]);
			}
		}
	}

	float diff (float a, float b){
		return Mathf.Abs(a - b);
	}

	int findClosestPage(float loc){
		int pageRec = -1;
		float minDiff = 1f;
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
		endTime = 0f;
	}
		
	public void OnEndDrag (PointerEventData data) {
		if (Mathf.Abs(scroll.velocity.x) < 500f){
			curPage = findClosestPage(scroll.horizontalNormalizedPosition);
		} else {
			if (scroll.velocity.x < 0 && (curPage + 1) < pages.Length){
				curPage++;
			} else if (scroll.velocity.x > 0 && (curPage - 1) >= 0){
				curPage--;
			}
		}
		SetStart();
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
		endTime = Time.time + animLength;
		startPos = scroll.horizontalNormalizedPosition;
	}

	void disableAll (GameObject except) {
		foreach (GameObject page in pages) {
			page.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
		}
		except.GetComponent<Image>().color = Color.white;
	}
}
