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
		manaText.text = "������" + ((int)manaData.mana).ToString();
		ifBeginChat = false;
	}

	// Update is called once per frame
	void Update () {
		if (manaData.mana < 100)
			manaData.mana = manaData.mana + Time.deltaTime * increasingSpeed;
		manaText.text = "������" + ((int)manaData.mana).ToString();
	}

	public void IfChatCanBegin(bool ischat)
	{
		if (ischat)
		{
			confirmText = "ÿ��������������:" + ((int)consume).ToString();
			if (!EditorUtility.DisplayDialog(confirmText, "ȷ��Ҫ����������", "��", "��"))
				return;
			if (manaData.mana - consume < 0)
			{
				EditorUtility.DisplayDialog("��Ǹ��", "����ֵ������", "ȷ��");
				return;
			}
			manaData.mana = manaData.mana - consume;
			ifBeginChat = true;
		}
	}
}