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
	// Use this for initialization
	void Start () {
        mana = 80;
        manaText.text = "灵力：" + ((int)mana).ToString();
        ifBeginChat = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (mana < 100)
            mana = mana + Time.deltaTime * increasingSpeed;
        manaText.text = "灵力：" + ((int)mana).ToString();
    }

    public void IfChatCanBegin(bool ischat)
    {
        if (ischat)
        {
            confirmText = "每次聊天消耗灵力:" + ((int)consume).ToString();
            if (!EditorUtility.DisplayDialog(confirmText, "确定要消耗灵力吗？", "是", "否"))
                return;
            if (mana - consume < 0)
            {
                EditorUtility.DisplayDialog("抱歉！", "灵力值不够！", "确认");
                return;
            }
            mana = mana - consume;
            ifBeginChat = true;
        }
    }
}
