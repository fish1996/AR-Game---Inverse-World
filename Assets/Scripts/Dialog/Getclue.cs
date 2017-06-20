using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Getclue : MonoBehaviour {
	public GameObject Dialogbox; //表示对话框
	public GameObject Clue; //表示线索框
	public bool iscanclue; //表示是否得到线索
	public string _factclue; //表示目前得到的线索
	public Text FactClue;

	// Use this for initialization
	void Start () {
		List<string> btnsName = new List<string>();
		btnsName.Add("Seebutton");
		btnsName.Add("Backbutton");

		foreach(string btnName in btnsName)
		{
			GameObject btnObj = GameObject.Find(btnName);
			Button btn = btnObj.GetComponent<Button>();
			btn.onClick.AddListener(delegate() {
				this.OnClick(btnObj); 
			});
		}

		iscanclue = false;
		Clue.SetActive (false);
	}

	void Getfactclue(string factclue){
		_factclue = factclue;
	}

	// Update is called once per frame
	void Update () {
		iscanclue = Showchat.isgetclue;
		if (iscanclue) {
			FactClue.text = _factclue;
			Dialogbox.SetActive (false);
			Clue.SetActive (true);
		} 
		else {
			Dialogbox.SetActive (true);
			Clue.SetActive (false);
		}
	}


	public void OnClick(GameObject sender)
	{
		switch (sender.name)
		{
		case "Seebutton":
			break;

		case "Backbutton":
			Showchat.isgetclue = false;
			Beginchat.ischat = false;
			GameObject.Find ("controlchat").GetComponent<Beginchat> ().isachievedialog = true;
			break;

		default:
			Debug.Log("none");
			break;
		}

	}

}
