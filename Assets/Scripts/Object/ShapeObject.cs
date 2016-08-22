using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Threading;

public class ShapeObject : MonoBehaviour {

	public bool isSelectable;
	public bool isBouncy;
	public float bounceMultiplier = 1.0f;
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
	private SpriteRenderer selfImage;

	private Vector3 lastPos;
	private int numCollisions;
	
	void Start () {
		arrows.color = Camera.main.backgroundColor;
		selfImage = GetComponent<SpriteRenderer>();
		fatherTime = GameObject.FindGameObjectWithTag("TimeController").GetComponent<TimeController>();
		arrows.gameObject.transform.rotation = Quaternion.identity;
		RefreshBorders();
		RefreshSelectable();
		if (isBouncy){
			bounceBorder.color = new Color(bounceBorder.color.r, bounceBorder.color.g, bounceBorder.color.b, 1f);
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
		RefreshBorders();
		startDistance = worldPoint - transform.position;
		startPos = transform.position;
		selected = true;
	}

	public void EndSelect () {
		if (numCollisions > 0) {
			transform.position = startPos;
		}
		selected = false;
	}

	public void Move(Vector3 worldPoint) {
		if (numCollisions <= 0){
			lastPos = transform.position;
		}
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
			arrows.color = new Color(arrows.color.r, arrows.color.g, arrows.color.b, 1F);
			shadow.color = new Color(1F, 1F, 1F, 0.5F);
		} else {
			arrows.color = new Color(arrows.color.r, arrows.color.g, arrows.color.b, 0F);
			shadow.color = new Color(1F, 1F, 1F, 0F);
		}
	}

	IEnumerator BounceBack(){
		Vector3 top = transform.localScale;
		transform.localScale = top * 0.9f;
		while (transform.localScale.x < top.x * 0.99f){
			transform.localScale = Vector3.Lerp(transform.localScale, top, Time.deltaTime * 10f);
			yield return null;
		}
		transform.localScale = top;
	}
		
	void OnCollisionEnter2D(Collision2D coll) {
		if (isBouncy && coll.gameObject.tag == "Player"){
			coll.rigidbody.AddForce(coll.contacts[0].normal * -1 * 200f * bounceMultiplier);
			StartCoroutine(BounceBack());
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (selected){
			startPos = lastPos;
			selfImage.color = new Color(1F, 1F, 1F, 0.5F);
			fatherTime.Cancel();
		}
		numCollisions++;
	}

	void OnTriggerStay2D(Collider2D coll) {
		if (selected){
			selfImage.color = new Color(1F, 1F, 1F, 0.5F);
			fatherTime.Cancel();
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		selfImage.color = Color.white;
		fatherTime.StopCancel();
		numCollisions--;
	}

	public bool isSelected (){
		return selected;
	}

	private void RefreshBorders(){
		Camera main = Camera.main;
		bottomLeft = main.ScreenToWorldPoint(new Vector3(0, main.pixelHeight * 0.17F));
		topRight = main.ScreenToWorldPoint(new Vector3(main.pixelWidth, main.pixelHeight * 0.9F));
	}
}
