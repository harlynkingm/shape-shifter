using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SetReset : MonoBehaviour, IPointerUpHandler, IEndDragHandler {

	public MyHorizontalScrollSnap scroll;

	private int lastSelected;

	public void OnPointerUp (PointerEventData data) {
		attemptReset();
	}

	public void OnEndDrag (PointerEventData data) {
		attemptReset();
	}

	void attemptReset () {
		int curSelected = scroll.currentPage();
		if (curSelected != lastSelected){
			scroll.Reset();
		}
		lastSelected = curSelected;
	}
}
