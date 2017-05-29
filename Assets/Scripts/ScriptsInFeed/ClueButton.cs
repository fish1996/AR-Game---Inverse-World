using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueButton : MonoBehaviour {

    private Sprite Image; 
    public UILabel guit;

    bool push = false;

    public void GetPnum(string stringPnum)
    {
        Image = Resources.Load<Sprite>("image/" + stringPnum);
    }

    public void Show()
    {
        if (push == false)
        {
            GameObject clueImage = new GameObject("clue");
            clueImage.transform.position = new Vector3(0, 0, 150);
            clueImage.transform.localScale = new Vector3(15, 15, 15);
            clueImage.AddComponent<SpriteRenderer>().sprite = Image;
            clueImage.GetComponent<SpriteRenderer>().sortingOrder = 4;
            guit.text = "点击返回";
            push = true;

        }
        else
        {
            GameObject clueImage = GameObject.Find("clue");
            Destroy(clueImage);
            push = false; 
            guit.text = "查看线索";
        }
    }
}
