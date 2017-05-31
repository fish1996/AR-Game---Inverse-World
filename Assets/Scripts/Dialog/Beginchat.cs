using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Beginchat : MonoBehaviour {
	private GameObject MemberObject;
	private GameObject Totalbox; //表示整个与闲聊相关的ui
	public static bool ischat;
	public float t; //用于控制时间间隔
	public bool isachievedialog; //是否完成对话
	public bool isgetmember; //是否已经得到了人物

	// Use this for initialization
	void Awake () {
//		Debug.Log ("is new");
		isgetmember=false;
		ischat = false;
		Totalbox = GameObject.Find ("Canvas");
		Totalbox.SetActive (false);
		isachievedialog = true;
	}

	public void ifcanchat(bool iscanchat)
	{
		if (iscanchat) { //是否足够灵力
			Totalbox.SetActive (true);
			isachievedialog = false;
		} 
		else {
			ischat = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!isgetmember) {
			t = 0;
		}
		if (t >= 1 && isgetmember) {
			/*if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);*/
			if (Input.GetMouseButton(0)) {
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
				if (isachievedialog) { //是否已完成上一段对话
					GameObject.Find ("ManaText").GetComponent<Mana> ().IfChatCanBegin (ischat);
				}
				else {
					Totalbox.SetActive (true);
				}
			}
			else { 
				if (isachievedialog) {
					Totalbox.SetActive (false);
				} //当已完成一段对话时关闭对话框
			}
		}
		t+=Time.deltaTime;
	}
}
