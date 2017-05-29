using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager_Main : MonoBehaviour {


    public GameObject feedBtn;
	public GameObject backBtn;



    private void Awake()

    {

        //必须为 UIButton  类型
        UIEventListener.Get(feedBtn).onClick += feedClicked;
		UIEventListener.Get(backBtn).onClick += backClicked;

    }



    void feedClicked(GameObject feedBtn)
    {
        Managers.Scene.LoadFeedGame();
    }

	void backClicked(GameObject feedBtn)
	{
		Managers.Scene.ReturnFromFeedGame();
	}


}
