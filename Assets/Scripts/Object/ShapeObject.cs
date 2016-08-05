using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Threading;

public class ShapeObject : MonoBehaviour {

	public bool isSelectable;
	public bool isBouncy;
	public SpriteRenderer arrows;
	public SpriteRenderer shadow;
	public SpriteRenderer bounceBorder;

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
		if (isBouncy){
			bounceBorder.color = new Color(1f, 1f, 1f, 0.68f);
		} else {
			GameObject.Destroy(bounceBorder.gameObject);
		}
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
		if (cancelled) {
			transform.position = startPos;
		}
		selected = false;
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
		
	void OnCollisionEnter2D(Collision2D coll) {
		if (isBouncy && coll.gameObject.tag == "Player"){
//			coll.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 200f);
//			Vector2 currentDirection = coll.transform.forward;
//			Vector2 collNormal = coll.contacts[0].normal * -1;
//			Vector2 bounceDirection = Vector2.Reflect(currentDirection, collNormal);
			coll.rigidbody.AddForce(coll.contacts[0].normal * -1 * 200f);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (selected){
			cancelled = true;
			fatherTime.Cancel();
		}
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (selected){
			cancelled = true;
			fatherTime.Cancel();
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		cancelled = false;
		fatherTime.StopCancel();
	}

	public bool isSelected (){
		return selected;
	}
}
