using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

	public Image loading;

	public void LoadLevel(int level){
		StartCoroutine(Fade(loading, 1.0f, level));
	}

	IEnumerator Fade(Image target, float finalVal, int level) {
		while (target.color.a != finalVal){
			Color c = target.color;
			c.a = Mathf.MoveTowards(target.color.a, finalVal, 0.02F);
			target.color = c;
			yield return null;
		}
		if (level > -1){
			SceneManager.LoadScene(level);
		}
	}
}
