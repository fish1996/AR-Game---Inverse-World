using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;

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

    public string folder = "data/clue1";
    private int choose1 = -1, choose2 = -1;
    private int choosecluenum = -1;
    private float touchtime = 0;
	private ClueData clueData = ClueData.getInstance();
    private SaveData saveData;////

    private List<Combination> cluecombination = new List<Combination>();
    private Vector3 tPosition = new Vector3(0, 5000, 0);
    private Vector3 bPosition = new Vector3(0, -5000, 0);
    public GameObject FinishButton;
    public GameObject ReturnButton;
    public Dialogtext clue;

    private void GetClueNum(int cluenum)
    {
        choosecluenum = cluenum;
        print(choosecluenum + " " + cluenum);
    }////
    void Awake()
    {
        saveData = new SaveData();
        saveData.Load();
    }/////
    void OnApplicationQuit()
    {
        saveData.Save();
        saveData.CloseConnection();
    }

	// Use this for initialization
	void Start () {
        //获取对话内容
        //clue = Dialogtext.GetInstance();
        //string t = clue.clueIntroduction.cluetxt [0].clueparagraph//获取线索内容

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
            Array[i] = lineArray[i].Split(',');
            a = int.Parse(Array[i][0]);
            c1 = int.Parse(Array[i][1]);
            c2 = int.Parse(Array[i][2]);
            r = int.Parse(Array[i][3]);
            cluecombination.Add(new Combination(a, c1, c2, r));
            if (a == 1)
				clueData.isActive[r] = true;
        }

        //绘制可见性
        string clue = "Clue";
        string temp;

        for (int i = 0; i < clueData.all_num; i++)
        {
            temp = clue + i.ToString();
			if (clueData.isActive[i] == false)
                GameObject.Find(temp).transform.Translate(tPosition);
        }
	}
	
	// Update is called once per frame
    void Update()
    {
        if (touchtime >= 1)
        {
            if (choosecluenum != -1)
            {
                ButtonJudge();
                choosecluenum = -1;
            }
            touchtime = 0;
        }
        touchtime += Time.deltaTime; 
	}

    private void ButtonJudge()
    {
		Debug.Log ("Judge");
        string clue = "Clue";
        string temp = clue + choosecluenum.ToString();
        //Sprite spr1 = Resources.Load<Sprite>("image/fire"); 
        //Sprite spr2 = Resources.Load<Sprite>("image/unfire");
        Color white = GameObject.Find("Clue1").GetComponent<UISprite>().color;
        Color red = new Color(255, 0, 0, (float)0.05);

        if (GameObject.Find(temp).activeSelf == true)
        {
            UISprite choosebutton = GameObject.Find(temp).GetComponent<UISprite>();

            if (choose1 == choosecluenum)
            {
                choose1 = -1;
                choosebutton.color = white;
            }
            else if (choose2 == choosecluenum)
            {
                choose2 = -1;
                choosebutton.color = white;
            }
            else if (choose1 == -1)
            {
                choosebutton.color = red;
                choose1 = choosecluenum;
            }
            else if (choose2 == -1)
            {
                choosebutton.color = red;
                choose2 = choosecluenum;
            }
            else
                print("选择错误");
        }
    }

    private void FinishButtonDown()
    {
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
				clueData.isActive[cluecombination[i].result] = true;
                string temp = "Clue" + cluecombination[i].result.ToString();
                print(temp);
                GameObject.Find(temp).transform.Translate(bPosition);
                Restore();
                return;
            }
        }
        Restore();
        print("线索组合错误");

        return;
    }

    private void Restore()
    {
        Color white = new Color(255, 255, 255);

        string temp = "Clue" + choose1.ToString();
        //Button choosebutton = GameObject.Find(temp).GetComponent<Button>();
        //choosebutton.GetComponent<Image>().color = white;
        temp = "Clue" + choose2.ToString();
        //choosebutton = GameObject.Find(temp).GetComponent<Button>();
        //choosebutton.GetComponent<Image>().color = white;

        choose1 = -1;
        choose2 = -1;
        choosecluenum = -1;

        return;
    }

    private void ReturnButtonDown()
    {
        //***返回并更新数据库（主要是传入isActive数组)***
    }
}
