﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mana : MonoBehaviour {
    public Text manaText;
    //public float increasingSpeed;
    public float consume;
    public bool ifBeginChat;
    private TimeData timeData = TimeData.getInstance();
    private ManaData manaData = ManaData.getInstance();

	public Text confirmText;
	public Button yesButton;
	public Button noButton;
	public Button sureButton;
	private GameObject manaConfirm;
	private GameObject error;

	// Use this for initialization
	void Start () {
		consume = 10;
        manaText.text = "灵力：" + ((int)manaData.mana).ToString();
        //ifBeginChat = false;

		confirmText.text = "每次聊天消耗灵力:" + ((int)consume).ToString();
		manaConfirm = GameObject.Find("ManaConfirm");
		error = GameObject.Find("Error");
		manaConfirm.SetActive(false);
		error.SetActive(false);

		yesButton.onClick.AddListener(DialogueYes); //防止被注册监听多次
		noButton.onClick.AddListener(DialogueNo);
	}
	
	// Update is called once per frame
	void Update () {
        timeData.deltaTime += Time.deltaTime;
        if(timeData.deltaTime > 600)
        {
            timeData.deltaTime = 0;
            manaData.mana += 5;
        }
        /*if (manaData.mana < 100)
            manaData.mana = manaData.mana + Time.deltaTime * increasingSpeed;*/
		manaText.text = "灵力：" + ((int)manaData.mana).ToString();
    }

	void DialogueYes()
	{
		manaConfirm.SetActive(false);

		if (manaData.mana - consume < 0)
		{
			error.SetActive(true);
			sureButton.onClick.AddListener(DialogueSure);
			Debug.Log ("is" + manaData.mana);
			return;
		}
		manaData.mana = manaData.mana - consume;
		ifBeginChat = true;

		GameObject.Find ("controlchat").GetComponent<Beginchat> ().ifcanchat (ifBeginChat);
	}

	void DialogueNo()
	{
		manaConfirm.SetActive(false);
		GameObject.Find ("controlchat").GetComponent<Beginchat> ().ifcanchat (ifBeginChat);
	}

	void DialogueSure()
	{
		error.SetActive(false);
	}

	public void IfChatCanBegin(bool ischat)
	{
		ifBeginChat = false;
		if (ischat)
		{
			manaConfirm.SetActive(true);
			/*yesButton.onClick.AddListener(DialogueYes);
			noButton.onClick.AddListener(DialogueNo);*/
		}
	}


    /*public void IfChatCanBegin(bool ischat)
    {
        if (ischat)
        {
            confirmText = "每次聊天消耗灵力:" + ((int)consume).ToString();
            if (!EditorUtility.DisplayDialog(confirmText, "确定要消耗灵力吗？", "是", "否"))
                return;
            if (manaData.mana - consume < 0)
            {
                EditorUtility.DisplayDialog("抱歉！", "灵力值不够！", "确认");
                return;
            }
            manaData.mana = manaData.mana - consume;
            ifBeginChat = true;
        }
    }*/
}
