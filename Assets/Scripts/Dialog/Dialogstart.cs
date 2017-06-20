using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Dialogstart : MonoBehaviour {
	public string memberName;
	public SChattext schattext;

	//单例模式
	private static Dialogstart instance = new Dialogstart ();
	private Dialogstart(){}
	public static Dialogstart GetInstance(){
		if (!instance) {
			instance = (Dialogstart)GameObject.FindObjectOfType (typeof(Dialogstart));
			if (!instance) {
				Debug.LogError("There needs to be one active Music class script on a GameObject in your scene.");
			}
		}
		return instance;
	}

	void Awake()
	{
		string n = "衡琳";
		PlayerPrefs.SetString ("membername",n);
		string name = PlayerPrefs.GetString("membername")+"开场";
		getText(name);
	}

	//获取当前用户选取的人物对话剧本
	public void getText(string memberName){	//musicName不带后缀
		//读取文件
		Dialogstart.GetInstance().memberName = memberName;
		string memberPath = Path.Combine("Member", memberName);
		Dialogstart.GetInstance().schattext = new SChattext (memberPath);
	}

	//开场对话类
	public class SChattext{
		public int needspecial = 0;
		public struct Schattxt
		{
			public int ordernum;
			public bool special;
			public string chattxt;
		}

		public List<Schattxt> schattxt;

		public SChattext(string Path){
			int first=1000;
			schattxt = new List<Schattxt>();
			TextAsset binAsset = Resources.Load(Path, typeof(TextAsset)) as TextAsset;
			string [] lineArray = binAsset.text.Split("\n"[0]);
			for (int i = 0; i < lineArray.Length-1; i++) {
				string line = lineArray[i];
				Schattxt temptxt=Classify(line);
				schattxt.Add(temptxt);
				if (temptxt.special == true && i < first) {
					first = i + 1;
					needspecial = first;
				}
			}
		}

		//分离线索和对应的对话
		public static Schattxt Classify(string line)
		{
			Schattxt temp;
			temp.chattxt = "";
			temp.special = false;
			temp.ordernum = 0;

			if (line.StartsWith ("衡琳：")) {
				temp.ordernum = 1;
				temp.chattxt = SGetdialog (line);
				temp.special = SGetspecial (line);
			}
			if (line.StartsWith ("玩家：")) {
				temp.ordernum = 0;
				temp.chattxt = SGetdialog (line);
				temp.special = SGetspecial (line);
			}

			return temp;
		}

		//得到每句对话
		public static string SGetdialog(string line){
			int length = line.Length - line.IndexOf ("：") - 1;
			string dialog = line.Substring(line.IndexOf("：") + 1,length);
			return dialog;
		}

		public static bool SGetspecial(string line){
			bool isspecial;
			int n = line.IndexOf ("XX");

			if (n >= 0) {
				isspecial = true;
			} 
			else {
				isspecial = false;
			}
			return isspecial;
		}
			
	}
}
