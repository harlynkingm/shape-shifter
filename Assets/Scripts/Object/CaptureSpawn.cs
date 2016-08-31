using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CaptureSpawn : MonoBehaviour {

	public GameObject[] prefabs;

	void Start () {
		int curScene = SceneManager.GetActiveScene().buildIndex - 1;
		GameObject capture = prefabs[curScene % prefabs.Length];
		GameObject newShape = (GameObject) Instantiate(capture, transform.position, Quaternion.identity);
		if (GetComponent<ObjectAnimation>()){
			ObjectAnimation initial = GetComponent<ObjectAnimation>();
			ObjectAnimation copy = newShape.AddComponent<ObjectAnimation>();
			copy.destination = initial.destination;
			copy.speed = initial.speed;
			copy.rotationSpeed = initial.rotationSpeed;
		}
		GameObject.Destroy(gameObject);
	}
}
