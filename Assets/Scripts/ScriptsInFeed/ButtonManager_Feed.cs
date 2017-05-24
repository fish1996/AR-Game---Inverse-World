using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager_Feed : MonoBehaviour {

    public GameObject clueBtn;//这段是与2D线索对接的
    public GameObject pressBtn;
    public GameObject quitBtn;
    public GameObject quitPanel;
    public GameObject yesBtn;
    public GameObject noBtn;
    public GameObject MainController;

    private bool isPressed;
    private float pressTime;

    private void Awake()

    {
        quitPanel.SetActive(false);
        UIEventListener.Get(clueBtn).onClick += setList;//这段是与2D线索对接的
        UIEventListener.Get(quitBtn).onClick += quitClicked;
        UIEventListener.Get(yesBtn).onClick += yesClicked;
        UIEventListener.Get(noBtn).onClick += noClicked;
        isPressed = false;
        pressTime = 0;
    }

    void setList(GameObject clueBtn)//这段是与2D线索对接的
    {
        List<ClassOfTarget> list;
        list = new List<ClassOfTarget>();
        ClassOfTarget tmp = new ClassOfTarget(0, new Vector3(-0.6f, -1.6f, -0.3f), 0.1f);
        list.Add(tmp);
        tmp = new ClassOfTarget(0, new Vector3(-0.4f, -1.4f, -0.3f), 0.1f);
        list.Add(tmp);
        tmp = new ClassOfTarget(0, new Vector3(-0.15f, -1.6f, -0.6f), 0.1f);
        list.Add(tmp);
        MainController.GetComponent<MainController>().SetTargetPos(list);
        clueBtn.SetActive(false);
    }

    void Update()
    {
        if (isPressed)
        {
            pressTime += Time.deltaTime;
            MainController.GetComponent<MainController>().Aim(pressTime);
        }
    }

    public void pressBtnDown()
    {
        isPressed = true;
    }

    public void pressBtnUp()
    {
        isPressed = false;
        Debug.Log("松按钮");
        MainController.GetComponent<MainController>().Fire(pressTime);
        pressTime = 0;
    }

    void quitClicked(GameObject quitBtn)
    {
        quitPanel.SetActive(true);
    }
    void yesClicked(GameObject yesBtn)
    {
        MainController.GetComponent<MainController>().StopTheGame();
        quitPanel.SetActive(false);
        Managers.Scene.ReturnFromFeedGame();
    }
    void noClicked(GameObject noBtn)
    {
        quitPanel.SetActive(false);
    }
}
