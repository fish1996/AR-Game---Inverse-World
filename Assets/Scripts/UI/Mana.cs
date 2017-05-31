using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Mana : MonoBehaviour {
	public Text manaText;
	public float increasingSpeed;
	public float consume;
	public bool ifBeginChat;
	private string confirmText;
	private ManaData manaData = ManaData.getInstance();
	// Use this for initialization
	void Start () {
	        manaData.mana = 80;
		increasingSpeed = 0.01f;
		consume = 10;
		manaText.text = "灵力：" + ((int)manaData.mana).ToString();
		ifBeginChat = false;
	}

	// Update is called once per frame
	void Update () {
		if (manaData.mana < 100)
			manaData.mana = manaData.mana + Time.deltaTime * increasingSpeed;
		manaText.text = "灵力：" + ((int)manaData.mana).ToString();
	}

	public void IfChatCanBegin(bool ischat)
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
	}
}