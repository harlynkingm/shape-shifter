using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LevelLoader : MonoBehaviour {

	public Image loading;

	void Start () {
		for(int i = 0; i < transform.childCount; i++){
			GameObject g = transform.GetChild(i).gameObject;
			g.AddComponent<LevelObj>().parent = this;
		}
		loading.color = Color.white;
		StartCoroutine(Fade(loading, 0F, -1));
	}

	public void LoadLevel(int level){
		StartCoroutine(Fade(loading, 1.0f, level));
	}

	public void ReloadLevel(){
		int curScene = SceneManager.GetActiveScene().buildIndex;
		LoadLevel(curScene);
	}

	public IEnumerator Fade(Image target, float finalVal, int level) {
		while (target.color.a != finalVal){
			Color c = target.color;
			c.a = Mathf.MoveTowards(target.color.a, finalVal, 0.1F);
			target.color = c;
			yield return null;
		}
		if (level > -1){
			SceneManager.LoadScene(level);
		}
	}
}

public class LevelObj : MonoBehaviour, IPointerClickHandler {

	public LevelLoader parent;

	public void OnPointerClick( PointerEventData eventData ){
		if (GetComponent<Image>().color.a == 1f){
			parent.LoadLevel(int.Parse(gameObject.name));
		}
	}

}