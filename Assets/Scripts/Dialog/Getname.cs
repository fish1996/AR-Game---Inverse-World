using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Getname : MonoBehaviour {
	private string userName;//用户名
	private Text input;
	public Dialogstart Sd;
	public NameData namedata = NameData.getInstance();

	public void Inputname()
	{
		input = GameObject.Find ("InputText").GetComponent<Text> ();
		userName = input.text;
	}

	void Awake () {
		Sd = Dialogstart.GetInstance();
		Button btn = GameObject.Find ("okButton").GetComponent<Button> ();
		btn.onClick.AddListener (OnClick);
	}

	private void OnClick(){
		Inputname ();
		namedata.playerName = userName;

		GameObject.Find ("controlchat").GetComponent<Beginfirstchat> ().Playerstart.SetActive (false);
		GameObject.Find ("controlchat").GetComponent<Beginfirstchat> ().Sdnum ++;
		GameObject.Find ("controlchat").GetComponent<Beginfirstchat> ().show();
		GameObject.Find ("controlchat").GetComponent<Beginfirstchat> ().isnameing = false;
	}
}
