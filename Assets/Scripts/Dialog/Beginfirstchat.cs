using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Beginfirstchat : MonoBehaviour {

	public GameObject Playerstart;
	public GameObject Dialogstartbox;
	public Dialogstart Sd;
	private Button Memberbutton;
	public Text Content;
	public int Sdnum=1;
	public Sprite member1;//定义逆灵1的按钮图标
	public Sprite member2;//定义逆灵1的按钮图标状态2
	public Sprite player;//定义玩家的按钮图标
	public bool isnameing; //表明是否进入取名阶段（true表示在该阶段）
	public NameData namedata = NameData.getInstance();
	public ChooseData data = ChooseData.getInstance();

	// Use this for initialization
	void Start () {
		List<string> btnsName = new List<string>();
		btnsName.Add("Dialogstartbox");

		foreach(string btnName in btnsName)
		{
			GameObject btnObj = GameObject.Find(btnName);
			Button btn = btnObj.GetComponent<Button>();
			btn.onClick.AddListener(delegate() {
				this.OnClick(btnObj); 
			});
		}
			
		Sd = Dialogstart.GetInstance ();
		Memberbutton = Dialogstartbox.GetComponent<Button> ();

		Playerstart.SetActive (false);
		Dialogstartbox.SetActive (false);
		isnameing = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Beginfirst()
	{
		Dialogstartbox.SetActive (true);
		show ();
	}

	private string Changename(string str, string name)
	{
		str=str.Replace("XX",name);

		return str;
	}

	public void show(){
		string Sfdiatxt = Sd.schattext.schattxt [Sdnum - 1].chattxt;
		bool isspecial = Sd.schattext.schattxt [Sdnum - 1].special;

		if (isspecial) 
		{
			Sfdiatxt=Changename (Sfdiatxt,namedata.playerName);
		}
		Content.text = Sfdiatxt;
		int Sfdnum = Sd.schattext.schattxt [Sdnum - 1].ordernum;
		if (Sfdnum == 0) {
			Memberbutton.GetComponent<Image>().sprite=player ;
			Content.color = Color.black;
		} 
		else if (Sfdnum == 1) {
			Memberbutton.GetComponent<Image>().sprite=member1 ;
			Content.color = Color.white;
		}
	}

	public void OnClick(GameObject sender)
	{
		switch (sender.name)
		{
		case "Dialogstartbox":
			int Sfdnum = Sd.schattext.schattxt.Count;
			int speclianum = Sd.schattext.needspecial;
			if (Sdnum == Sfdnum) {
				Playerstart.SetActive (false);
				Dialogstartbox.SetActive (false);
				GameObject.Find ("controlchat").GetComponent<Beginchat> ().isgetmember = true;
				data.isChooseName = true;
			} 
			else if (Sdnum == speclianum - 1) {
				isnameing = true;
				Playerstart.SetActive (true);
			}
			else if (!isnameing) {
				Sdnum = Sdnum + 1;
				show ();
			}
			break;

		default:
			Debug.Log("none");
			break;
		}
	}
}
