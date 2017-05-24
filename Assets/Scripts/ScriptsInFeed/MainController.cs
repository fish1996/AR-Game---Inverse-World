using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using HighlightingSystem;

public class MainController : MonoBehaviour, ITrackableEventHandler
{
    [SerializeField]
    private GameObject Locator;
    [SerializeField]
    private GameObject TargetPrefab;
    [SerializeField]
    private GameObject progressBar;//进度条显示力度
    [SerializeField]
    private GameObject pressButton;//进度条显示力度
    [SerializeField]
    private GameObject gunBarrel;//枪管用于实时对准以及测距
    private List<GameObject> mTargetList;
    private int indexOfThisTarget;

    void Start()
    {
        mTargetList = new List<GameObject>();
        indexOfThisTarget = -1;
        progressBar.SetActive(false);
        pressButton.SetActive(false);
        gunBarrel.SetActive(false);
        Locator.SetActive(false);
    }

    public void Aim(float pressTime)
    {
        mTargetList[indexOfThisTarget].GetComponent<Target>().Aiming(pressTime);
    }

    public void Fire(float pressTime){
        progressBar.SetActive(false);//先隐藏
        pressButton.SetActive(false);
        StartCoroutine(mTargetList[indexOfThisTarget].GetComponent<Target>().Fire(pressTime));
    }

    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            progressBar.SetActive(true);//它们是出现在2DUI选择结束后的
            pressButton.SetActive(true);
            gunBarrel.SetActive(true);
            Locator.SetActive(true);
        }
        else
        {
            //progressBar.SetActive(false);//它们是出现在2DUI选择结束后的
            //pressButton.SetActive(false);
            //gunBarrel.SetActive(false);
            //Locator.SetActive(false);
        }
    }

    public void SetTargetPos(List<ClassOfTarget> list)
    {//初始化各个靶的位置
        progressBar.SetActive(true);//它们是出现在2DUI选择结束后的
        pressButton.SetActive(true);
        gunBarrel.SetActive(true);
        Locator.SetActive(true);
        foreach (ClassOfTarget importTarget in list)
        {
            GameObject thisTarget=Instantiate(TargetPrefab) as GameObject;//索引以List的顺序表示，位置以
            thisTarget.transform.parent = Locator.transform;
            thisTarget.transform.localPosition = importTarget.objPosition;
            thisTarget.transform.localScale = new Vector3(importTarget.scaleAmount, importTarget.scaleAmount, importTarget.scaleAmount);
            thisTarget.AddComponent<Target>();
            thisTarget.AddComponent<Highlighter>();
            thisTarget.GetComponent<Target>().progressBar = progressBar;
            thisTarget.GetComponent<Target>().gunBarrel = gunBarrel;
            mTargetList.Add(thisTarget);
        }
        AlterToNextTarget();//第一次调用相当于开启游戏
    }

    public void AlterToNextTarget()
    {
        progressBar.SetActive(true);//准备好家伙
        pressButton.SetActive(true);
        if (indexOfThisTarget == mTargetList.Count-1)
        {
            //灵力值添加  Managers.Player.
            StopTheGame();
            Debug.Log("Win");
        }
        else
        {
            mTargetList[++indexOfThisTarget].GetComponent<Target>().Ready();
        }
    }

    public void StopTheGame()
    {
        progressBar.SetActive(false);//它们是出现在2DUI选择结束后的
        pressButton.SetActive(false);
        gunBarrel.SetActive(false);
        Locator.SetActive(false);
    }


}
