using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Mana : MonoBehaviour {
    public Text manaText;
    public float mana;
    public float increasingSpeed;
    public float consume;
    public bool ifBeginChat;
    private string confirmText;
    private TimeData timeData = TimeData.getInstance();
	// Use this for initialization
	void Start () {
		consume = 10;
        manaText.text = "������" + ((int)mana).ToString();
        ifBeginChat = false;
	}
	
	// Update is called once per frame
	void Update () {
        timeData.deltaTime += Time.deltaTime;
        if(timeData.deltaTime > 600)
        {
            timeData.deltaTime = 0;
            manaData.mana += 5;
        }
        if (mana < 100)
            mana = mana + Time.deltaTime * increasingSpeed;
        manaText.text = "������" + ((int)mana).ToString();
    }

    public void IfChatCanBegin(bool ischat)
    {
        if (ischat)
        {
            confirmText = "ÿ��������������:" + ((int)consume).ToString();
            if (!EditorUtility.DisplayDialog(confirmText, "ȷ��Ҫ����������", "��", "��"))
                return;
            if (mana - consume < 0)
            {
                EditorUtility.DisplayDialog("��Ǹ��", "����ֵ������", "ȷ��");
                return;
            }
            mana = mana - consume;
            ifBeginChat = true;
        }
    }
}