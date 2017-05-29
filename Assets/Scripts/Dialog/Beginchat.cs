using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Beginchat : MonoBehaviour {
	private GameObject MemberObject;
	private GameObject Totalbox; //表示整个与闲聊相关的ui
	public static bool ischat;
	public float t; //用于控制时间间隔

	// Use this for initialization
	void Awake () {
		Debug.Log ("is new");
		ischat = false;
		Totalbox = GameObject.Find ("Canvas");
		Totalbox.SetActive (false);
	}


	// Update is called once per frame
	void Update () {
		if (t >= 1) {
			if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit))
				{
					MemberObject = hit.transform.gameObject;
					if (MemberObject.transform.tag == "MainCharacter") {
						ischat = !ischat;
						t = 0;
					}
				}
			}
			if (ischat) {
				Totalbox.SetActive (true);
			}
			else {
				Totalbox.SetActive (false);
			}
		}
		t+=Time.deltaTime;
	}
}
