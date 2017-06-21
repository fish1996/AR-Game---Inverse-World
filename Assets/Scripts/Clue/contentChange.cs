using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class contentChange : MonoBehaviour {

    public GameObject ShowButton;
    public Text Content;
    public GameObject ShowClue;

    private ClueData clueData = ClueData.getInstance();
    private Dialogtext clueText;

    private Vector3 tPosition = new Vector3(0, 5000, 0);
    private Vector3 bPosition = new Vector3(0, -5000, 0);
    private int Choose1;
    private int Choose2;
    private int ClueNum;
    private string clue;
    private bool flag;

	// Use this for initialization
	void Start () {
        clueText = Dialogtext.GetInstance();

        Content.text = "";

        ShowClue.transform.Translate(tPosition);

        Button finishone = (Button)ShowButton.GetComponent<Button>();
        finishone.onClick.AddListener(ShowButtonDown);

        flag = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GetChoose1(int choose1)
    {
        Choose1 = choose1;
    }

    public void GetChoose2(int choose2)
    {
        Choose2 = choose2;
    }

    private void GetShowClue(string t)
    {
        string insertStr = "\n";
        string show = "";

        string[] lineArray = t.Split("\n"[0]);

        for (int i = 1; i < lineArray.Length - 1; i++)
        {
            print(i + " " + lineArray[i].Length + " " + lineArray[i]);
            int j = 9;
            while (j < lineArray[i].Length)
            {
                lineArray[i] = lineArray[i].Insert(j, insertStr);
                j += 10;
            }
            show += lineArray[i];
            show += insertStr;
        }
        Content.text = show;
        //this.GetComponent<Text>().text = show;
    }

    private void ShowButtonDown()
    {
        if ((Choose1 == -1 && Choose2 == -1) || (Choose1 != -1 && Choose2 != -1))
            return;

        if (Choose1 != -1)
            ClueNum = Choose1 - 1;
        else
            ClueNum = Choose2 - 1;

        if (flag == false)
        {
            clue = clueText.clueIntroduction.cluetxt[ClueNum].clueparagraph;//获取线索内容
            GetShowClue(clue);
            ShowClue.transform.Translate(bPosition);
            flag = true;
        }
        else
        {
            ShowClue.transform.Translate(tPosition);
            flag = false;
        }
    }
}
