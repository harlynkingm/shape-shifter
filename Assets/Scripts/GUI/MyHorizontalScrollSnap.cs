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
	private bool lerp = false;

	private float startDragTime;

	void Start () {
		scroll = GetComponent<ScrollRect>();
		float pageScrollWidth = 1f/(pages.Length - 1);
		for (int i = 0; i < pages.Length; i++){
			pageLocs.Add(i * pageScrollWidth);
		}
		curPage = findClosestPage(scroll.horizontalNormalizedPosition);
	}

	void Update () {
		if (lerp){
			if (Mathf.Abs(scroll.horizontalNormalizedPosition - pageLocs[curPage]) > 0.0005f){
				scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, pageLocs[curPage], 10f * Time.deltaTime);
			} else {
				scroll.horizontalNormalizedPosition = pageLocs[curPage];
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
		lerp = false;
		startDragTime = Time.time;
	}
		
	public void OnEndDrag (PointerEventData data) {
		float scrollVelocity = (data.position.x - data.pressPosition.x)/(Time.time - startDragTime);
		if (Mathf.Abs(scrollVelocity) < 2000f){
			curPage = findClosestPage(scroll.horizontalNormalizedPosition);
		} else {
			if (scrollVelocity < 0f){
				NextPage();
			} else if (scrollVelocity > 0f){
				PrevPage();
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
		lerp = true;
	}

	void disableAll (GameObject except) {
		foreach (GameObject page in pages) {
			page.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
		}
		except.GetComponent<Image>().color = Color.white;
	}
}
