using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour {

	public float rows;

	void Start () {
		RectTransform parent = gameObject.GetComponent<RectTransform>();
		GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup>();
		float height = parent.rect.height/rows;
		grid.cellSize = new Vector2(height, height);
	}
}
