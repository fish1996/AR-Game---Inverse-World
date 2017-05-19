using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beginchat : MonoBehaviour {
	private GameObject MemberObject;
	private GameObject Totalbox; //表示整个与闲聊相关的ui
	public static bool ischat;

	// Use this for initialization
	void Start () {
		ischat = false;
		Totalbox = GameObject.Find ("Canvas");
		Totalbox.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				MemberObject = hit.transform.gameObject;
				if (MemberObject.transform.tag == "MainCharacter") {
					ischat = !ischat;
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
}
