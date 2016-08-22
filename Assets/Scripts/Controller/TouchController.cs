using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchController : MonoBehaviour {

	public bool zoom = false;

	private ShapeObject selected;
//	private bool hasForceTouch;
	private HomeController homeController;
	private float prevDist;

	void Start () {
		homeController = GameObject.FindGameObjectWithTag("HomeBar").GetComponent<HomeController>();
//		hasForceTouch = Input.touchPressureSupported;
	}

	void Update () {
		if (Input.touchCount > 0){
			Touch currentTouch = Input.GetTouch(0);
			if (Input.touchCount == 1 && currentTouch.phase == TouchPhase.Began){
				BeginSelect(currentTouch);
			} else if (selected && currentTouch.phase == TouchPhase.Moved){
				MoveSelection(currentTouch);
			} else if (selected && currentTouch.phase == TouchPhase.Ended){
				EndSelect();
			}
			if (zoom && Input.touchCount == 2){
				Touch secondTouch = Input.GetTouch(1);
				if (secondTouch.phase == TouchPhase.Began){
					prevDist = Vector2.Distance(currentTouch.position, secondTouch.position);
				} else if (secondTouch.phase == TouchPhase.Moved || currentTouch.phase == TouchPhase.Moved){
					float curDist = Vector2.Distance(currentTouch.position, secondTouch.position);
					Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + (prevDist - curDist) * Time.deltaTime * 0.3f, 5f, 7.5f);
					prevDist = curDist;
				}
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
