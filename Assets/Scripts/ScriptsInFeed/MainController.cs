using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using HighlightingSystem;

public class MainController : MonoBehaviour
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
    [SerializeField]
    private GameObject ImageTarget;//目标图避免提前显示
    [SerializeField]
    private GameObject HitTargetPrefab;//目标图避免提前显示
    [SerializeField]
    private UILabel guit_energy;
    

    private List<GameObject> mTargetList;
    private int indexOfThisTarget;
    private TrackableBehaviour mTrackableBehaviour;
    private int tryTime=0;
    private float scaleRank;
    public static int add_energy;
    private int temp_energy=10;
    Object addEnergySuCai;
    GameObject addEnergy;

    void Start()
    {
        addEnergySuCai = Resources.Load<Object>("Prefabs/addEnergy_obj");
        mTargetList = new List<GameObject>();
        indexOfThisTarget = -1;
        progressBar.SetActive(false);
        pressButton.SetActive(false);
        gunBarrel.SetActive(false);
    }

    public void Aim(float pressTime)
    {
        mTargetList[indexOfThisTarget].GetComponent<Target>().Aiming(pressTime);
    }

    public void Fire(float pressTime){
        progressBar.SetActive(false);//先隐藏
        //pressButton.SetActive(false);
        StartCoroutine(mTargetList[indexOfThisTarget].GetComponent<Target>().Fire(pressTime));
    }


    public void SetTargetPos(List<ClassOfTarget> list)
    {//初始化各个靶的位置
        //add_energy = 0;
        progressBar.SetActive(true);//它们是出现在2DUI选择结束后的
        pressButton.SetActive(true);
        gunBarrel.SetActive(true);
        foreach (ClassOfTarget importTarget in list)
        {
            GameObject thisTarget=Instantiate(TargetPrefab) as GameObject;//索引以List的顺序表示，位置以
            thisTarget.gameObject.layer = LayerMask.NameToLayer("SetActiveFalse");
            thisTarget.transform.parent = Locator.transform;
            thisTarget.transform.localPosition = importTarget.objPosition;
            thisTarget.transform.localScale = new Vector3(importTarget.scaleAmount, importTarget.scaleAmount, importTarget.scaleAmount);
            scaleRank = importTarget.scaleAmount;
            thisTarget.AddComponent<Target>();
            thisTarget.AddComponent<Highlighter>();
            thisTarget.GetComponent<Target>().progressBar = progressBar;
            thisTarget.GetComponent<Target>().gunBarrel = gunBarrel;
            mTargetList.Add(thisTarget);
        }
        AlterToNextTarget();//第一次调用相当于开启游戏
        ImageTarget.GetComponent<mTrackableEventHandler>().gameStart();
    }

    public void AlterToNextTarget()
    {
        progressBar.SetActive(true);//准备好家伙
        pressButton.SetActive(true);
        if (indexOfThisTarget == mTargetList.Count-1)
        {
            //灵力值添加  Managers.Player.
            addEnergy = Instantiate(addEnergySuCai) as GameObject;
            addEnergy.transform.position = gunBarrel.transform.position;
            print("position"+gunBarrel.transform.position);
            addEnergy.transform.Rotate(new Vector3(0, 110, -45));
            addEnergy.transform.Translate(new Vector3(0.4f, 0, 0));

            Destroy(addEnergy,1);

            //从数据库获取灵力值temp_energy=???;temp_energy=temp_energy+add_energy;
            temp_energy = temp_energy + add_energy;
            guit_energy.text="灵力值："+temp_energy;
            StopTheGame();
            Debug.Log("Win");
            //GameObject.Find("Camera").SendMessage("Result", true);
            GameObject.Find("Camera").SendMessage("Result", indexOfThisTarget + 1);
            print("indexOfThisTarget:" + indexOfThisTarget);

            GameObject.Find("UI Root").GetComponent<ButtonManager_Feed>().getBack();
        }
        else
        {
            tryTime = 0;
            mTargetList[++indexOfThisTarget].GetComponent<Target>().Ready();
        }
    }

    public void RestartToThisTarget()
    {
        tryTime++;
        progressBar.SetActive(true);//准备好家伙
        pressButton.SetActive(true);
        if (tryTime == 7)
        {
            StopTheGame();//fail
            Debug.Log("You don't have chance!");
            //GameObject.Find("Camera").SendMessage("Result", false);
            GameObject.Find("Camera").SendMessage("Result", indexOfThisTarget);
        }
        else
        {
            mTargetList[indexOfThisTarget].GetComponent<Target>().Ready();
        }
    }

    public void HitTransform()
    {
        mTargetList[indexOfThisTarget].gameObject.layer = LayerMask.NameToLayer("SetActiveFalse");
        GameObject thisHitTarget = Instantiate(HitTargetPrefab) as GameObject;
        thisHitTarget.transform.parent = Locator.transform;
        thisHitTarget.transform.localPosition = mTargetList[indexOfThisTarget].transform.localPosition;
        thisHitTarget.transform.localScale = new Vector3(scaleRank, scaleRank, scaleRank);
        thisHitTarget.AddComponent<Target>();
        thisHitTarget.AddComponent<Highlighter>();
        thisHitTarget.GetComponent<Target>().progressBar = progressBar;
        thisHitTarget.GetComponent<Target>().gunBarrel = gunBarrel;
        Destroy(mTargetList[indexOfThisTarget]);
    }

    public void StopTheGame()
    {
        progressBar.SetActive(false);//它们是出现在2DUI选择结束后的
        pressButton.SetActive(false);
        gunBarrel.SetActive(false);
        Locator.SetActive(false);
        ImageTarget.GetComponent<mTrackableEventHandler>().gameEnd();
    }

    public void ShowTarget()
    {
        progressBar.SetActive(true);//它们是出现在2DUI选择结束后的
        pressButton.SetActive(true);
        gunBarrel.SetActive(true);
        foreach (Transform targ in Locator.gameObject.GetComponentsInChildren<Transform>())
        {//遍历当前物体及其所有子物体  
            targ.gameObject.layer = LayerMask.NameToLayer("Default");//更改物体的Layer层  
        }
    }

    public void HideTarget()
    {
        progressBar.SetActive(false);//它们是出现在2DUI选择结束后的
        pressButton.SetActive(false);
        gunBarrel.SetActive(false);
        foreach (Transform targ in Locator.gameObject.GetComponentsInChildren<Transform>())
        {//遍历当前物体及其所有子物体  
            targ.gameObject.layer = LayerMask.NameToLayer("SetActiveFalse");//更改物体的Layer层  
        }
    }

}
