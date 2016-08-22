using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CaptureSpawn : MonoBehaviour {

	public GameObject[] prefabs;

	void Start () {
		int curScene = SceneManager.GetActiveScene().buildIndex - 1;
		GameObject capture = prefabs[curScene % prefabs.Length];
		GameObject.Instantiate(capture, transform.position, Quaternion.identity);
		GameObject.Destroy(gameObject);
	}
}
