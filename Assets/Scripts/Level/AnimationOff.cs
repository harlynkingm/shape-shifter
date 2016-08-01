using UnityEngine;
using System.Collections;

public class AnimationOff : MonoBehaviour {

	ShapeObject thisGuy;

	void Start () {
		thisGuy = GetComponent<ShapeObject>();
	}

	void Update () {
		if (thisGuy.isSelected()){
			Destroy(thisGuy.GetComponent<Animator>());
			thisGuy.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}
}
