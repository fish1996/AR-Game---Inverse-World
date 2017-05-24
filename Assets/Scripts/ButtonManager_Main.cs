using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager_Main : MonoBehaviour {


    public GameObject feedBtn;



    private void Awake()

    {

        //必须为 UIButton  类型
        UIEventListener.Get(feedBtn).onClick += feedClicked;

    }



    void feedClicked(GameObject feedBtn)
    {
        Managers.Scene.LoadFeedGame();
    }



}
