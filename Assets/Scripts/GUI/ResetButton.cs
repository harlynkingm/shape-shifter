using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour, IPointerClickHandler {

	public Sprite confirm;
	private int status;

	public void OnPointerClick (PointerEventData data){
		status++;
		if (status == 1){
			GetComponent<Image>().sprite = confirm;
		} else if (status == 2){
			Camera.main.gameObject.GetComponent<SaveController>().Reset();
			Camera.main.gameObject.GetComponent<LevelLoader>().ReloadLevel();
		}
	}

}
