using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour {
	private const int NUM = 39;
	private float acc = 0.1f;
	private float[] speed = new float[NUM];
	private Vector2[] vector = new Vector2[NUM];
	GameObject[] obj = new GameObject[NUM];
	private StartData startData = StartData.getInstance ();
	private ClueJudge clueJudge = new ClueJudge ();
	void Awake(){
		for (int i = 0; i < NUM; i++) {
			speed [i] = 0.0f;
			obj [i] = GameObject.Find ("UI Root (3D)/Canvas/Clue" + (i + 1));
			UIEventListener.Get (obj[i]).onClick += ButtonClick;
			UIEventListener.Get (obj[i]).onDrag += ButtonDrag;
			obj [i].AddComponent<Rigidbody> ();
			obj [i].GetComponent<Rigidbody> ().useGravity = false;
		}
	}

	void FixedUpdate(){
		
		for (int i = 0; i < NUM; i++) {
			if (speed [i] > 0) {
				speed [i] -= acc;
				Vector3 LocalPos = obj[i].transform.position;
				if (LocalPos.y < -400) {
					vector [i].y = -vector [i].y;
					obj [i].GetComponent<Rigidbody> ().MovePosition (new Vector3(LocalPos.x, -398, LocalPos.z));
				}
				else if (LocalPos.y > 600) {
					vector [i].y = -vector [i].y;
					obj [i].GetComponent<Rigidbody> ().MovePosition (new Vector3(LocalPos.x, 598, LocalPos.z));
				}
				if (LocalPos.x < -5050) { 
					vector [i].x = -vector [i].x;
					obj [i].GetComponent<Rigidbody> ().MovePosition (new Vector3(-5048, LocalPos.y, LocalPos.z));
				} 
				else if (LocalPos.x > 4650) {
					vector [i].x = -vector [i].x;
					obj [i].GetComponent<Rigidbody> ().MovePosition (new Vector3(4648, LocalPos.y, LocalPos.z));
				}
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
		startData.clueNum = int.Parse(snum);
		GameObject.Find("Canvas").GetComponent<ClueJudge>().ButtonJudge();
	}
}
