using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public struct Points
{
    public int x2d;
    public int y2d;
    public float x3d;
    public float y3d;
    public float z3d;

    public Points(int x2d,int y2d,float x3d,float y3d,float z3d) //带参数的构造函数
    {
        this.x2d = x2d;
        this.y2d = y2d;
        this.x3d = x3d;
        this.y3d = y3d;
        this.z3d = z3d;
    }
}

public class Choose : MonoBehaviour {

    public UILabel guit;
    public Camera camera;
    public GameObject UI;
    public GameObject ShowClue;

    private string[][] Array;
    private StoryData data = StoryData.getInstance();
    private int Pnum;
    private string stringPnum;
    private string folder="data/position";
    public List<Points> this_point = new List<Points>();
    public bool[] isChoose;
	private FeedData feedData = FeedData.getInstance ();
	public bool[] failChoose;//***需要存入数据库内所有点之前是否已经被选择的记录***
    private bool hasFull = true;
    private bool showClue = true;
    private int choose_num = 0;
    private int max_choose = 4;
    private int deviation = 30;
    private int width = Screen.width, height = Screen.height;
    public List<Vector3> choose_point = new List<Vector3>();

	// Use this for initialization
	void Start () {
        //print("start");
        NumShow(choose_num);

        Pnum = data.Pnum;
        //Pnum = 1;
        System.Random ran = new System.Random();
        int RandKey = ran.Next(1, 3);
        stringPnum = Pnum.ToString() + "_" + RandKey.ToString();
        folder = folder + stringPnum;
        print(folder);

        //读取文件  
        TextAsset binAsset = Resources.Load(folder, typeof(TextAsset)) as TextAsset;//bug?

        //读取每一行的内容  
        string[] lineArray = binAsset.text.Split("\r"[0]);

        //创建二维数组  
        Array = new string[lineArray.Length][];
        isChoose = new bool[lineArray.Length];
        failChoose = new bool[lineArray.Length];//***hasChoose成功读取后删除***
        //print(lineArray.Length);

        int x1, y1;
        float x2, y2, z2;
        Sprite nochoose = Resources.Load<Sprite>("image/unfire");
        Sprite haschoose = Resources.Load<Sprite>("image/choosefire");
        //把csv中的数据储存在二位数组中  
        for (int i = 0; i < lineArray.Length; i++)
        {
            Array[i] = lineArray[i].Split(',');
            isChoose[i] = false;
			feedData.hadChoose[i] = true;//***hasChoose成功读取后删除***
			failChoose[i] = feedData.hadChoose[i];//***hasChoose成功读取后删除***
            x1 = (int)((float.Parse(Array[i][0]) / 1280) * width);
            y1 = (int)((float.Parse(Array[i][1]) / 720) * height);
            x2 = float.Parse(Array[i][2]);
            y2 = float.Parse(Array[i][3]);
            z2 = float.Parse(Array[i][4]);
            this_point.Add(new Points(x1, y1, x2, y2, z2));

            Vector3 transform_point = camera.ScreenToWorldPoint(new Vector3(x1, y1, 300));
            GameObject pic = new GameObject((i).ToString());
            //print(transform_point);
            pic.transform.position = transform_point;
            if (failChoose[i] == true)
                pic.AddComponent<SpriteRenderer>().sprite = haschoose;
            else
                pic.AddComponent<SpriteRenderer>().sprite = nochoose;
            pic.GetComponent<SpriteRenderer>().sortingOrder = 2;
			print ("id" + i);

			if (feedData.hadChoose[i] == false && showClue == true)
                showClue = false;
        }

        if (showClue == true)
        {
            ShowClue.SetActive(true);
            ShowClue.SendMessage("GetPnum", stringPnum);
        }
        else
            ShowClue.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            int nowx, nowy, nowz;
            nowx = (int)Input.mousePosition.x;
            nowy = (int)Input.mousePosition.y;
            nowz = (int)Input.mousePosition.z;
            ClickJudgment(nowx, nowy, nowz);
        }
        //if (Input.GetMouseButtonDown(1))
            //print("position:" + Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IPHONE
               if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
               {
                    int nowx, nowy;
                    nowx = (int)Input.touches[0].position.x;
                    nowy = (int)Input.touches[0].position.y;
                    ClickJudgment(nowx, nowy, 0);
                }
#endif
        NumShow(choose_num);
	}

    //判断选择点的状态
    void ClickJudgment(int x, int y,int z)
    {
        GameObject choose_one;
        Sprite spr;

        for (int i = 0; i < this_point.Count; i++)
        {
            if (this_point[i].x2d - deviation < x && this_point[i].x2d + deviation > x && this_point[i].y2d - deviation < y && this_point[i].y2d + deviation > y)
            {
                if (isChoose[i] == false)
                {
                    if (choose_num < max_choose)
                    {
                        choose_num++;
                        isChoose[i] = !isChoose[i];
                        choose_one = GameObject.Find(i.ToString());
                        spr = Resources.Load<Sprite>("image/fire");
                        choose_one.GetComponent<SpriteRenderer>().sprite = spr;
                        //print("x:" + this_point[i].x3d + " y:" + this_point[i].y3d + " z:" + this_point[i].y3d);
                        //print("id:" + i);
                    }
                    else
                    {
                        GameObject warning = new GameObject("warning");
                        warning.transform.position = new Vector3(0, 0, 150);
                        spr = Resources.Load<Sprite>("image/warning");
                        warning.AddComponent<SpriteRenderer>().sprite = spr;
                        warning.GetComponent<SpriteRenderer>().sortingOrder = 3;
                        Destroy(warning, 1);
                        print("选择点数已经达到最大");
                    }
                }
                else
                {
                    choose_num--;
                    isChoose[i] = !isChoose[i];
                    choose_one = GameObject.Find(i.ToString());
                    if(failChoose[i]==true)
                        spr = Resources.Load<Sprite>("image/choosefire");
                    else
                        spr = Resources.Load<Sprite>("image/unfire");
                    choose_one.GetComponent<SpriteRenderer>().sprite = spr;
                }
                return;
            }
        }
        print("not find!");
        return;
    }

    //开始游戏
    public void StartButtonDown()
    {
        float outx, outy, outz;
        for (int i = 0; i < this_point.Count; i++)
        {
            Destroy(GameObject.Find(i.ToString()));
            if (isChoose[i] == true)
            {
				feedData.hadChoose[i] = true;
                outx = this_point[i].x3d;
                outy = this_point[i].y3d;
                outz = this_point[i].z3d;
                choose_point.Add(new Vector3(outx, outy, outz));
            }
        }
        for (int i = 0; i < this_point.Count; i++)
        {
			if (feedData.hadChoose[i] == false && hasFull == true)
                hasFull = false;
        }
        UI.SendMessage("GetPoints",choose_point);
        return;
    }

    //显示当前选择点数目
    void NumShow(int num)
    {
        guit.text = "已选择点数:" + num;
        return; 
    }

    //游戏结束后的数据存储和显示线索
    public void Result(bool result)
    {
        print("resive");
        if (result == true)
        {
            print("success");
            if (hasFull == true)
            {
                GameObject clueImage = new GameObject(stringPnum);
                clueImage.transform.position = new Vector3(0, 0, 150);
                clueImage.transform.localScale = new Vector3(15, 15, 15);
                Sprite spr = Resources.Load<Sprite>("image/" + stringPnum);
                clueImage.AddComponent<SpriteRenderer>().sprite = spr;
                clueImage.GetComponent<SpriteRenderer>().sortingOrder = 4;
                Destroy(clueImage, 2);
            }
            //***存入hadChoose中的内容***
        }
        else
        {
            print("fail");
            //***存入failChoose中的内容***
        }
    }
}
