using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Dialogtext : MonoBehaviour {

	public Chattext chattext;
	public ClueIntroduction clueIntroduction;
	public string memberName;
	private static Dialogtext instance;

	void Awake()
	{
		string n = "衡琳";
		PlayerPrefs.SetString ("membername",n);
		string name = PlayerPrefs.GetString("membername");
		getText(name);
	}

	public static Dialogtext GetInstance(){
		if (!instance) {
			instance = (Dialogtext)GameObject.FindObjectOfType (typeof(Dialogtext));
			if (!instance) {
				Debug.LogError("There needs to be one active Music class script on a GameObject in your scene.");
			}
		}
		return instance;
	}

	//获取当前用户选取的人物对话剧本
	public void getText(string memberName){	//musicName不带后缀
		//读取文件
		Dialogtext.GetInstance().memberName = memberName;
		string memberPath = Path.Combine("Member", memberName);
		Dialogtext.GetInstance().chattext = new Chattext (memberPath);

		string cluePath = Path.Combine("Clue", memberName);
		Dialogtext.GetInstance().clueIntroduction = new ClueIntroduction (cluePath);
	}
		 
	//对话类
	public class Chattext{
		public struct Dtxt 
		{  
			public int num;//表示对话者身份（如玩家0，逆灵1为1，特殊要求为9） 
			public string diatxt;//对应的对话 
		} //对应每个线索的对话

		public struct Ctxt
		{
			public string clue; //表示线索
			public List<Dtxt> dtxt; //表示该线索对应的对话
			public int dnum; //表示该线索对应的对话数目
		} //对应每个线索

		public struct Ptxt
		{
			public List<Ctxt> ctxt;
			public int cnum; //表示线索数目
		} //对应每个关卡

		public int pnum;
		public List<Ptxt> ptxt;

		public Chattext(string memberPath){
			pnum=0;
			TextAsset binAsset = Resources.Load(memberPath, typeof(TextAsset)) as TextAsset;

			ptxt = new List<Ptxt>();
			string [] PassArray;
			PassArray=SplitWithString(binAsset.text,"关卡："); //根据关卡分割字符串
			pnum=PassArray.Length;

			for (int i = 0; i < PassArray.Length; i++){
				Ptxt tempptxt=Classify_pass(PassArray[i]);
				ptxt.Add(tempptxt);
			}
		}

		//根据关卡分类
		public static Ptxt Classify_pass(string text)
		{
			Ptxt temp;
			string [] ClueArray;
			ClueArray=SplitWithString(text,"\n\n"); //根据线索分割字符串，此处选择空行区分每一线索

			temp.ctxt = new List<Ctxt>();

			for (int i = 0; i < ClueArray.Length-1; i++) {
				Ctxt tempctxt=Classify_clue(ClueArray [i]);
				temp.ctxt.Add(tempctxt);
			}

			temp.cnum = ClueArray.Length-1;

			return temp;
		}

		//根据线索分类
		public static Ctxt Classify_clue(string text)
		{
			Ctxt temp;
			temp.clue = "";
			temp.dtxt = new List<Dtxt> ();
			temp.dnum = 0;

			string [] DialogArray;
			DialogArray=SplitWithString(text,"\n"); //根据线索分割字符串，此处选择空格区分每一线索

			for (int i = 0; i < DialogArray.Length; i++) {
				Dtxt tempdtxt=new Dtxt();
				string line=DialogArray[i];
				if (line.StartsWith (" ")) {
					continue;
				} 
				else {
					if (line.StartsWith ("玩家：")) {
						tempdtxt.diatxt = Getdialog (line);
						tempdtxt.num = 0;
						temp.dnum++;
						temp.dtxt.Add (tempdtxt);
					} 
					else if (line.StartsWith ("衡琳：")) {
						tempdtxt.diatxt = Getdialog (line);
						tempdtxt.num = 1;
						temp.dnum++;
						temp.dtxt.Add (tempdtxt);
					} 
					else if (line.StartsWith ("（")) {
						tempdtxt.diatxt = line;
						tempdtxt.num = 9;
						temp.dnum++;
						temp.dtxt.Add (tempdtxt);
					} 
					else {
						string fclue = line.Substring (0, line.Length - 1);
						temp.clue = fclue;
					}
				}
			}
			return temp;
		}

		//得到每句对话
		public static string Getdialog(string line){
			int length = line.Length - line.IndexOf ("：") - 1;
			string dialog = line.Substring(line.IndexOf("：") + 1,length);
			return dialog;
		}
	}

	//线索类
	public class ClueIntroduction{
		public struct Clue
		{
			public bool isshow;
			public string cluewords;
			public string clueparagraph;
		}

		public List<Clue> cluetxt;

		public ClueIntroduction(string cluePath){
			TextAsset binAsset = Resources.Load(cluePath, typeof(TextAsset)) as TextAsset;

			cluetxt = new List<Clue>();
			string [] PassArray;
			PassArray=SplitWithString(binAsset.text,"\n\n"); //根据空行分割字符串

			for (int i = 0; i < PassArray.Length; i++){
				Clue tempcluetxt=Classify(PassArray[i]);
				cluetxt.Add(tempcluetxt);
			}
		}

		//分离线索和对应的对话
		public static Clue Classify(string text)
		{
			Clue temp;
			string [] ClueArray;
			ClueArray=SplitWithString(text,"\n"); //根据线索分割字符串，此处选择空行区分每一线索

			temp.cluewords = ClueArray [0];

			int length = text.Length - ClueArray [0].Length - 1;
			temp.clueparagraph = text.Substring (ClueArray [0].Length + 1,length);

			temp.isshow = false;

			return temp;
		}


	}

	//用于以特定字符串分离字符串
	public static string[] SplitWithString(string sourceString, string splitString){  

		List<string> arrayList = new List<string>();  
		string s = string.Empty;  
		while (sourceString.IndexOf(splitString) > -1)  
		{  
			s = sourceString.Substring(0, sourceString.IndexOf(splitString));  
			sourceString = sourceString.Substring(sourceString.IndexOf(splitString) + splitString.Length); 
			if (s != "") {
				arrayList.Add (s);  
			}
		}  
		arrayList.Add(sourceString);  
		return arrayList.ToArray();  
	}
}
