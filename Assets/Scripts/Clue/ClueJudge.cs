using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public struct Combination
{
    public int active;
    public int clue1;
    public int clue2;
    public int result;

    public Combination(int a, int c1, int c2, int r) //带参数的构造函数
    {
        this.active = a;
        this.clue1 = c1;
        this.clue2 = c2;
        this.result = r;
    }
}

public class ClueJudge : MonoBehaviour {

    public string folder;
    private int choose1 = -1, choose2 = -1;
    private ClueData clueData = ClueData.getInstance();
	private StartData startData = StartData.getInstance();
    private SaveData saveData;////
	private bool isAppear = false;
	private const int MAXCOUNT = 100;

    private List<Combination> cluecombination = new List<Combination>();
 
    private Vector3 tPosition = new Vector3(0, 5000, 0);
    private Vector3 bPosition = new Vector3(0, -5000, 0);
	private Color white;
	private Color originalColor;
	private GameObject appearObj;
	private int appearCount;
    public GameObject FinishButton;
    public GameObject ReturnButton;
    public Dialogtext clueText;
    public GameObject Content;

    public Text test;

    void Awake()
    {
        saveData = new SaveData();
        saveData.Load();
    }

    void OnApplicationQuit()
    {
        saveData.Save();
        saveData.CloseConnection();
    }

	// Use this for initialization
	void Start () {

        white = GameObject.Find("Clue1").GetComponent<UISprite>().color;
        //监听完成和返回按钮
        Button finishone = (Button)FinishButton.GetComponent<Button>();
        Button returnone = (Button)ReturnButton.GetComponent<Button>();
        finishone.onClick.AddListener(FinishButtonDown);
        returnone.onClick.AddListener(ReturnButtonDown);

		clueData.index = 5;
        //判断一级线索状态
		for (int i = 0; i < clueData.all_num; i++)
        {
			if (i < clueData.index)
				clueData.isActive[i] = true;
            else
				clueData.isActive[i] = false;
        }
			
        //读入二级三级线索状态
        //读取文件  
        folder = Path.Combine("Data", "clue1");
        TextAsset binAsset = Resources.Load(folder, typeof(TextAsset)) as TextAsset;//bug?

        //读取每一行的内容  
        string[] lineArray = binAsset.text.Split("\r"[0]);

        //创建二维数组  
		clueData.combinationnum = lineArray.Length;
		string[][]Array = new string[clueData.combinationnum][];

        int a, c1, c2, r;
        //把csv中的数据储存在二位数组中  
		for (int i = 0; i < clueData.combinationnum; i++)
        {
            if (i == 0)
                test.text = lineArray[i];
            Array[i] = lineArray[i].Split(',');
            if (clueData.isActive[i] == true)
                a = 1;
            else
                a = 0;
            a = int.Parse(Array[i][0]);
            c1 = int.Parse(Array[i][1]);
            c2 = int.Parse(Array[i][2]);
            r = int.Parse(Array[i][3]);
            cluecombination.Add(new Combination(a, c1, c2, r));
            /*if (a == 1)
				clueData.isActive[r] = true;*/
            //print(i + " " + clueData.isActive[r]);
        }

        //绘制可见性
        string clue = "Clue";
        string temp;

        for (int i = 0; i < clueData.all_num; i++)
        {
            temp = clue + (i + 1).ToString();
			if (clueData.isActive[i] == false)
                GameObject.Find(temp).transform.Translate(tPosition);
        }
	}

	void Update()
	{
		if (isAppear) {
			if (appearCount == MAXCOUNT) {
				isAppear = false;
			}
			appearObj.GetComponent<UISprite> ().color = new Color(originalColor.r,originalColor.g,originalColor.b,
				originalColor.a*(1.0f*appearCount / MAXCOUNT));
			appearCount++;
		}

	}


    public void ButtonJudge()
    {
		Debug.Log ("Judge");
        string clue = "Clue";
		string temp = clue + startData.clueNum.ToString();
        //Sprite spr1 = Resources.Load<Sprite>("image/fire"); 
        //Sprite spr2 = Resources.Load<Sprite>("image/unfire");
        
        Color red = new Color(255, 0, 0, (float)0.05);

        if (GameObject.Find(temp).activeSelf == true)
        {
            UISprite choosebutton = GameObject.Find(temp).GetComponent<UISprite>();
			Debug.Log ("choose1=" + choose1 + " choose2=" + choose2 + " cluenum=" + startData.clueNum);
			if (choose1 == startData.clueNum)
			{
				choose1 = -1;
				choosebutton.color = white;
			}
			else if (choose2 == startData.clueNum)
			{
				choose2 = -1;
				choosebutton.color = white;
			}
			else if (choose1 == -1)
			{
				choosebutton.color = red;
				choose1 = startData.clueNum;
			}
			else if (choose2 == -1)
			{
				choosebutton.color = red;
				choose2 = startData.clueNum;
			}

            else
                print("选择错误");
        }

        Content.SendMessage("GetChoose1", choose1);
        Content.SendMessage("GetChoose2", choose2);
    }

	private void clearColor(){
		string temp2 = "Clue" + choose1.ToString();
		UISprite choosebutton = GameObject.Find(temp2).GetComponent<UISprite>();
		choosebutton.color = white;
		temp2 = "Clue" + choose2.ToString();
		choosebutton = GameObject.Find(temp2).GetComponent<UISprite>();
		choosebutton.color = white;
		choose1 = choose2 = -1;
	}

    private void FinishButtonDown()
    {
		if (choose1 == -1 || choose2 == -1) {
			return;
		}
        if (choose1 > choose2)
        {
            int tempnum = choose1;
            choose1 = choose2;
            choose2 = tempnum;
        }
        //判断两个choose是否可以拼合
		for (int i = 0; i < clueData.combinationnum; i++)
        {
			if (choose1 == cluecombination[i].clue1 && choose2 == cluecombination[i].clue2 && clueData.isActive[cluecombination[i].result] == false)
            {
				clearColor ();

				clueData.isActive[cluecombination[i].result] = true;
                string temp = "Clue" + cluecombination[i].result.ToString();
                print(temp);
				appearObj = GameObject.Find (temp);
				appearObj.transform.Translate(bPosition); 
				originalColor = appearObj.GetComponent<UISprite> ().color;
				appearCount = 0;
				isAppear = true;

                if (cluecombination[i].result >= 40)
                {
                    //****************************************过场动画******************************************************************
                }
                return;
            }
        }
		clearColor ();
        print("线索组合错误");
        return;
    }

    private void ReturnButtonDown()
    {
        //界面跳转
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene"); 
    }
		
}
