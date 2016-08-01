using UnityEngine;
using System.Collections;

public class TouchController : MonoBehaviour {

	private ShapeObject selected;
//	private bool hasForceTouch;

	void Start () {
//		hasForceTouch = Input.touchPressureSupported;
	}

	void Update () {
		if (Input.touchCount > 0){
			Touch currentTouch = Input.GetTouch(0);
			if (currentTouch.phase == TouchPhase.Began){
				BeginSelect(currentTouch);
			} else if (selected && currentTouch.phase == TouchPhase.Moved){
				MoveSelection(currentTouch);
			} else if (selected && currentTouch.phase == TouchPhase.Ended){
				EndSelect();
			}
		}
	}

	void BeginSelect (Touch touch) {
		Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
		if (hit && hit.transform.gameObject.GetComponent<ShapeObject>()){
			selected = hit.transform.gameObject.GetComponent<ShapeObject>();
			if (selected.isSelectable){
				selected.StartSelect(pos);
			} else {
				ResetSelection();
			}
		}
	}

	void MoveSelection (Touch touch){
		Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
		selected.Move(worldPoint);
	}

	void EndSelect () {
		selected.EndSelect();
		ResetSelection();
	}

	public void ResetSelection () {
		selected = null;
	}
}
