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
    public GameObject ChooseNumText;
    public GameObject Camare;
    public GameObject ProgressBar;
    public GameObject LessText;
    public GameObject MoreText;

    private bool isPressed;
    private float pressTime;
    private List<Vector3> choose_points = new List<Vector3>();
    private const float scaledTargetSize=0.25f;

    private void Awake()

    {
        quitPanel.SetActive(false);
        UIEventListener.Get(clueBtn).onClick += setList;//这段是与2D线索对接的
        UIEventListener.Get(quitBtn).onClick += quitClicked;
        UIEventListener.Get(yesBtn).onClick += yesClicked;
        UIEventListener.Get(noBtn).onClick += noClicked;
        hiddenLessText();
        hiddenMoreText();
        ProgressBar = GameObject.Find("Progress");
        isPressed = false;
        pressTime = 0;
    }

    public void GetPoints(List<Vector3> choose_point)
    {
        print("pass:" + choose_point.Count);
        for (int i = 0; i < choose_point.Count; i++)
            choose_points.Add(choose_point[i]);
    }

    void setList(GameObject clueBtn)//这段是与2D线索对接的
    {
        
  
        Camare.GetComponent<Choose>().StartButtonDown();
        print("COUNT_choose_points:" + choose_points.Count);
        if (choose_points.Count > 0)
        {
            Camare.GetComponent<Choose>().destroyChoose();
            List <ClassOfTarget> list;
            list = new List<ClassOfTarget>();
            for (int i = 0; i < choose_points.Count; i++)
            {
                list.Add(new ClassOfTarget(0, choose_points[i], scaledTargetSize));
                print("setListchoose_points:" + choose_points[i]);
            }
            /*ClassOfTarget tmp = new ClassOfTarget(0, new Vector3(0.0f, -1.0f, 0.0f), 0.1f);
            list.Add(tmp);
            tmp = new ClassOfTarget(0, new Vector3(0.1f, -0.8f, 0.2f), 0.1f);
            list.Add(tmp);
            tmp = new ClassOfTarget(0, new Vector3(0.3f, 0.2f, 0.3f), 0.1f);
            list.Add(tmp);*/
            MainController.GetComponent<MainController>().SetTargetPos(list);
            clueBtn.SetActive(false);
            ChooseNumText.SetActive(false);
         }
        else
        {
            Camare.GetComponent<Choose>().showNoChoose();

        }
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
        ProgressBar.SetActive(true);
        ProgressBar.AddComponent<CiclularProgress>();
        hiddenLessText();
        hiddenMoreText();
    }

    public void pressBtnUp()
    {
        isPressed = false;
        Debug.Log("松按钮");
      
        MainController.GetComponent<MainController>().Fire(pressTime);
       
        ProgressBar.SetActive(false);
        pressTime = 0;
    }

    void quitClicked(GameObject quitBtn)
    {
        quitPanel.SetActive(true);
    }

    public void getBack()
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
    public void showLessText()
    {
        LessText.SetActive(true);
    }
    public void hiddenLessText()
    {
        LessText.SetActive(false);
    }
    public void showMoreText()
    {
        MoreText.SetActive(true);
    }
    public void hiddenMoreText()
    {
        MoreText.SetActive(false);
    }
}
