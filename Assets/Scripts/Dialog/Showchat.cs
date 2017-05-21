using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Showchat : MonoBehaviour {

	public Text Content;
	public GameObject d;
	public string mname;
	public Dialogtext other;
	public float t; //用于控制时间间隔
	public Sprite member1;//定义逆灵1的按钮图标
	public Sprite player;//定义玩家的按钮图标
	public Image Member;
	public static bool isgetclue;
	public string factclue;
	public StoryData data = StoryData.getInstance();

	// Use this for initialization
	void Start () {
		isgetclue = false;
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
		string fdiatxt = other.chattext.ptxt [data.Pnum - 1].ctxt [data.Cnum - 1].dtxt [data.Dnum - 1].diatxt;
		Content.text = fdiatxt;
		int fdnum = other.chattext.ptxt [data.Pnum - 1].ctxt [data.Cnum - 1].dtxt [data.Dnum - 1].num;
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
			int fdnum = other.chattext.ptxt [data.Pnum - 1].ctxt [data.Cnum - 1].dnum;
			int fcnum = other.chattext.ptxt [data.Pnum - 1].cnum;
			if (data.Dnum < fdnum) {
				data.Dnum++;
				show ();
			} 
			else if (data.Dnum == fdnum) {
				factclue = other.chattext.ptxt [data.Pnum - 1].ctxt [data.Cnum - 1].clue;
				data.Dnum = 1;
				if (data.Cnum == fcnum) {
					data.Cnum = 1;
					data.Pnum++;
				} 
				else {
					data.Cnum++;
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
			int fdnum = other.chattext.ptxt [data.Pnum - 1].ctxt [data.Cnum - 1].dnum;
			int fcnum = other.chattext.ptxt [data.Pnum - 1].cnum;
			if (Input.GetKey (KeyCode.Space) && (data.Dnum < fdnum)) {
				data.Dnum++;
				show ();
				t = 0;
			} 
			else if (Input.GetKey (KeyCode.Space) && (data.Dnum == fdnum)) {
				factclue = other.chattext.ptxt [data.Pnum - 1].ctxt [data.Cnum - 1].clue;
				data.Dnum = 1;
				if (data.Cnum == fcnum) {
					data.Cnum = 1;
					data.Pnum++;
				} 
				else {
					data.Cnum++;
				}
				isgetclue = true;
				show ();
			}
		}
		t+=Time.deltaTime;

		GameObject.Find("Canvas").SendMessage("Getfactclue", factclue);

	}
}
