using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartButton : MonoBehaviour {

	private StartData startData = StartData.getInstance();
	void Start () {
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(OnClick);
	}
	void OnClick(){
		Debug.Log ("click");
		if (startData.isStart) {
			startData.isStart = false;
		} 
		else {
			startData.isStart = true;
		}
	}
}
