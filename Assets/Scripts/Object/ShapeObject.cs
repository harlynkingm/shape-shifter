using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Threading;

public class ShapeObject : MonoBehaviour {

	public bool isSelectable;
	public SpriteRenderer arrows;
	public SpriteRenderer shadow;

	private RaycastHit hit; 
	private Vector3 startDistance;
	private Vector3 startPos;
	private bool selected = false;

	private Vector3 bottomLeft;
	private Vector3 topRight;

	private TimeController fatherTime;
	private bool cancelled;
	
	void Start () {
		fatherTime = GameObject.FindGameObjectWithTag("TimeController").GetComponent<TimeController>();
		arrows.gameObject.transform.rotation = Quaternion.identity;
		Camera main = Camera.main;
		bottomLeft = main.ScreenToWorldPoint(new Vector3(0, main.pixelHeight * 0.2F));
		topRight = main.ScreenToWorldPoint(new Vector3(main.pixelWidth, main.pixelHeight * 0.9F));
		RefreshSelectable();
	}

	void Update () {
		if (selected && shadow.color.a < 1.0F) {
			Color color = shadow.color;
			color.a += .05F;
			shadow.color = color;
		} else if (!selected && shadow.color.a > 0.5F){
			Color color = shadow.color;
			color.a -= .05F;
			shadow.color = color;
		}
	}

	public void StartSelect (Vector3 worldPoint) {
		startDistance = worldPoint - transform.position;
		startPos = transform.position;
		selected = true;
	}

	public void EndSelect () {
		selected = false;
		if (cancelled) {
			transform.position = startPos;
		}
	}

	public void Move(Vector3 worldPoint) {
		Vector3 newPos = new Vector3(worldPoint.x, worldPoint.y, 0) - startDistance;
		newPos = new Vector3(
			Mathf.Clamp(newPos.x, bottomLeft.x, topRight.x),
			Mathf.Clamp(newPos.y, bottomLeft.y, topRight.y),
			0
		);
		transform.position = newPos;
	}

	public void RefreshSelectable () {
		if (isSelectable){
			arrows.color = new Color(1F, 1F, 1F, 1F);
			shadow.color = new Color(1F, 1F, 1F, 0.5F);
		} else {
			arrows.color = new Color(1F, 1F, 1F, 0F);
			shadow.color = new Color(1F, 1F, 1F, 0F);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		cancelled = true;
		fatherTime.Cancel();
	}

	void OnTriggerStay2D(Collider2D coll) {
		cancelled = true;
		fatherTime.Cancel();
	}

	void OnTriggerExit2D(Collider2D coll) {
		cancelled = false;
		fatherTime.StopCancel();
	}
}
