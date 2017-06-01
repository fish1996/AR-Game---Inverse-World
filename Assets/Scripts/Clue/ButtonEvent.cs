using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour {
	private const int NUM = 15;
	private float acc = 0.1f;
	private float[] speed = new float[NUM];
	private Vector2[] vector = new Vector2[NUM];
	GameObject[] obj = new GameObject[NUM];
	private StartData startData = StartData.getInstance ();
	void Awake(){
		for (int i = 0; i < NUM; i++) {
			speed [i] = 0.0f;
			obj[i] = GameObject.Find ("UI Root (3D)/Canvas/Clue" + i);
			UIEventListener.Get (obj[i]).onClick += ButtonClick;
			UIEventListener.Get (obj[i]).onDrag += ButtonDrag;
			obj[i].AddComponent<Rigidbody> ();
			obj[i].GetComponent<Rigidbody> ().useGravity = false;
		}
	}

	void FixedUpdate(){
		if (startData.isStart)
			return;
		for (int i = 0; i < NUM; i++) {
			if (speed [i] > 0) {
				speed [i] -= acc;
				Vector3 LocalPos = obj[i].transform.position;
				if (LocalPos.y < -400) {
					vector [i].y = -vector [i].y;
					LocalPos.y = -400;
				}
				else if (LocalPos.y > 600) {
					vector [i].y = -vector [i].y;
					LocalPos.y = 600;
				}
				if (LocalPos.x < 1) {
				} 
				else if (LocalPos.x > 4650) {
					vector [i].x = -vector [i].x;
					LocalPos.x = 4650;
				}
					
				Vector3 LocalForward = transform.TransformPoint (Vector3.forward * speed [i]);
				obj[i].GetComponent<Rigidbody> ().velocity = new Vector3 (vector [i].x*speed[i], vector[i].y*speed[i], 0);
			}

		}

	}

	void ButtonDrag(GameObject obj,Vector2 vec){
		if (startData.isStart)
			return;
		string snum = obj.name.Substring(4, obj.name.Length - 4);
		int cluenum = int.Parse(snum);
		speed [cluenum] = 10.0f;
		vector [cluenum] = vec;
	}
	void ButtonClick(GameObject obj){
		if (!startData.isStart)
			return;
		Debug.Log (obj.name);
		string snum = obj.name.Substring(4, obj.name.Length - 4);
		int cluenum = int.Parse(snum);
		//GameObject.Find("Canvas").SendMessage("GetClueNum", cluenum);
	}
}
