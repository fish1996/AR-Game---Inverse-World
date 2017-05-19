using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Showchat : MonoBehaviour {

	public Text Content;
	public int Dnum; //表示目前的对话进度
	public int Cnum; //表示目前的线索进度
	public int Pnum; //表示目前的关卡进度
	public GameObject d;
	public string mname;
	public Dialogtext other;
	public float t; //用于控制时间间隔
	public Sprite member1;//定义逆灵1的按钮图标
	public Sprite player;//定义玩家的按钮图标
	public Image Member;
	public static bool isgetclue;
	public string factclue;

	// Use this for initialization
	void Start () {
		isgetclue = false;
		Dnum = 1;
		Cnum = 1;
		Pnum = 1;
		factclue = "";

		mname="衡琳";
		PlayerPrefs.SetString("membername", mname);
		d = GameObject.Find("Dialog");
		other = (Dialogtext)d.GetComponent(typeof(Dialogtext));
		other.getText(mname);
		show ();

		List<string> btnsName = new List<string>();
		btnsName.Add("Dialogbox");

		foreach(string btnName in btnsName)
		{
			GameObject btnObj = GameObject.Find(btnName);
			Button btn = btnObj.GetComponent<Button>();
			btn.onClick.AddListener(delegate() {
				this.OnClick(btnObj); 
			});
		}

	}

	public void show(){
		string fdiatxt = other.chattext.ptxt [Pnum - 1].ctxt [Cnum - 1].dtxt [Dnum - 1].diatxt;
		Content.text = fdiatxt;
		int fdnum = other.chattext.ptxt [Pnum - 1].ctxt [Cnum - 1].dtxt [Dnum - 1].num;
		if (fdnum == 0) {
			Member.sprite = player;
		} 
		else if (fdnum == 1) {
			Member.sprite = member1;
		}
	}

	public void OnClick(GameObject sender)
	{
		switch (sender.name)
		{
		case "Dialogbox":
			int fdnum = other.chattext.ptxt [Pnum - 1].ctxt [Cnum - 1].dnum;
			int fcnum = other.chattext.ptxt [Pnum - 1].cnum;
			if (Dnum < fdnum) {
				Dnum++;
				show ();
			} 
			else if (Dnum == fdnum) {
				factclue = other.chattext.ptxt [Pnum - 1].ctxt [Cnum - 1].clue;
				Dnum = 1;
				if (Cnum == fcnum) {
					Cnum = 1;
					Pnum++;
				} 
				else {
					Cnum++;
				}
				isgetclue = true;
				show ();
			}
			break;

		default:
			Debug.Log("none");
			break;
		}
			
	}

	// Update is called once per frame
	void Update () {
		if (t >= 1) {
			int fdnum = other.chattext.ptxt [Pnum - 1].ctxt [Cnum - 1].dnum;
			int fcnum = other.chattext.ptxt [Pnum - 1].cnum;
			if (Input.GetKey (KeyCode.Space) && (Dnum < fdnum)) {
				Dnum++;
				show ();
				t = 0;
			} 
			else if (Input.GetKey (KeyCode.Space) && (Dnum == fdnum)) {
				factclue = other.chattext.ptxt [Pnum - 1].ctxt [Cnum - 1].clue;
				Dnum = 1;
				if (Cnum == fcnum) {
					Cnum = 1;
					Pnum++;
				} 
				else {
					Cnum++;
				}
				isgetclue = true;
				show ();
			}
		}
		t+=Time.deltaTime;

		GameObject.Find("Canvas").SendMessage("Getfactclue", factclue);

	}
}
