/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
        print("button start");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void MousePick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                //Debug.Log(hit.transform.tag);
            }
        }
    }

    private void OnClick()
    {
        print("choose button success");
        int cluenum = -1;
        string name = this.name;
        string snum = name.Substring(4, name.Length - 4);
        cluenum = int.Parse(snum);
        GameObject.Find("background").SendMessage("GetClueNum", cluenum);
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ClueClick : MonoBehaviour 
{
    public Camera mycamera;
    // Use this for initialization
    void Start()
    {
        /*List<string> btnsName = new List<string>();
        btnsName.Add("Sprite (_0002_portrait1)");
        btnsName.Add("Sprite (_0049_---思念)");
        btnsName.Add("Sprite (_0051_---圣诞)");
        btnsName.Add("Sprite (_0045_相见)");
        btnsName.Add("Sprite (_0048_报复)");
        btnsName.Add("Sprite (_0047_疾病)");

        foreach (string btnName in btnsName)
        {
            print(btnName);
            GameObject btnObj = GameObject.Find(btnName);
            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(delegate()
            {
                this.OnClick(btnObj);
            });
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Stationary)) || Input.GetMouseButton(0))
        {
            Vector3 screen = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 160);
            print(mycamera.WorldToScreenPoint(new Vector3(3640,-585,0)));
            //Vector3 worldpoint = Camera.main.ScreenToWorldPoint(screen);
            Vector3 worldpoint = mycamera.ScreenToWorldPoint(screen);
            print(worldpoint);
            Collider2D[] col = Physics2D.OverlapPointAll(worldpoint);
            /*print(screen);
            print(Input.mousePosition);
            print(worldpoint);*/

            if (col.Length > 0)
            {
                Collider2D choosebutton = col[0];

                print(choosebutton.name);
                //do what you want
                print("choose success");
                int cluenum = -1;
                string name = choosebutton.name;
                string snum = name.Substring(4, name.Length - 4);
                print(snum);
                cluenum = int.Parse(snum);
                GameObject.Find("Canvas").SendMessage("GetClueNum", cluenum);
                /*foreach (Collider2D choosebutton in col)*/
            }
            else
                print("not find");
        }
    }

    public void OnClick(GameObject sender)
    {
        /*switch (sender.name)
        {
            case "_0002_portrait1":
                Debug.Log("aa");
                break;
            case "1":
                Debug.Log("1");
                break;

            default:
                Debug.Log("none");
                break;
        }*/
    }

}
