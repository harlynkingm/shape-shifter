using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour {

	private ShapeObject selected;
//	private bool hasForceTouch;
	private HomeController homeController;

	void Start () {
		homeController = GameObject.FindGameObjectWithTag("HomeBar").GetComponent<HomeController>();
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
		if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)){
			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
			if (hit && hit.transform.gameObject.GetComponent<ShapeObject>()){
				selected = hit.transform.gameObject.GetComponent<ShapeObject>();
				if (selected.isSelectable){
					selected.StartSelect(pos);
				} else {
					ResetSelection();
				}
			} else {
				homeController.AnimateToggle();
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
