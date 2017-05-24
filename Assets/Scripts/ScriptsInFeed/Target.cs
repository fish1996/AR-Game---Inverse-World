using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class Target : MonoBehaviour {

    public GameObject progressBar;//进度条显示力度
    public GameObject gunBarrel;//枪管用于实时对准以及测距
    public GameObject mainController;//主控制器用于返回射击结果
    private Object bulletPrefab;
    public float pressTimeToDistanceSpeed = 600f;//将按键时间转化为射击距离的比例
    public float bulletFlyingSpeed = 600f;//动画效果中的飞行速度
    public float hitThreshold=500.0f;//用距离测试是否击中，如果用碰撞器无法检测超过情况
    private float percent;
    private float fireDistance;//子弹的预计飞行距离
    private float totalDistance;//相机距靶距离
    public float endThreshold = 50.0f;//完成飞行距离 
    //备注，距离都是千级的
    protected Highlighter h;
    private bool startAnimation;
    private Vector3 direct;
    private GameObject _bullet;
    private float remainDistance;

    void Awake()
    {
        h = GetComponent<Highlighter>();
        if (h == null) { h = gameObject.AddComponent<Highlighter>(); }
    }

    void Start()
    {
        startAnimation = false;
        bulletPrefab= Resources.Load<Object>("Prefabs/Bullet");
    }

    public void Ready()
    {
        Color col = new Color(0.2f, 0.2f, 0.8f, 1f);
        h.ConstantOnImmediate(col);
    }

    public void Aiming(float pressTime)
    {
        //力度条部分
        calcuLateDistance(pressTime);
        percent = fireDistance / totalDistance;
        progressBar.GetComponent<UISlider>().Set(percent);
        //progressBar.GetComponent<Renderer>().material.SetFloat("_Progress", percent);

        //枪管部分
        gunBarrel.transform.LookAt(transform,-Vector3.forward);//第二个参数为向上方向

        gunBarrel.transform.Rotate(new Vector3(90, 0, 0));
    }

    public IEnumerator Fire(float pressTime)
    {
        if (!startAnimation)
        {
            //射击效果
            calcuLateDistance(pressTime);
            direct = transform.position - gunBarrel.transform.position;
            direct.Normalize();
            direct /= 120;
            _bullet = Instantiate(bulletPrefab) as GameObject;
            _bullet.transform.position = gunBarrel.transform.position;
            remainDistance = fireDistance;
            startAnimation = true;
        }
        while (remainDistance > endThreshold) //未完成飞行
        {
            float step = bulletFlyingSpeed * Time.deltaTime;
            remainDistance -= step;
            _bullet.transform.Translate(step * direct);
            yield return 0;
        }
        //击中判定
        //////////////////击中动画待添加
        startAnimation = false;
        Destroy(_bullet);
        h.ConstantOffImmediate();
        if ((totalDistance - fireDistance) < hitThreshold)
        {
            mainController = GameObject.FindWithTag("MainController_Feed");
            mainController.GetComponent<MainController>().AlterToNextTarget();
            Debug.Log("打中了！！");
        }
        else
        {
            mainController = GameObject.FindWithTag("MainController_Feed");
            mainController.GetComponent<MainController>().StopTheGame();
            Debug.Log("失败");
        }
    }

    private void calcuLateDistance(float pressTime) {
        fireDistance = pressTime * pressTimeToDistanceSpeed;
        totalDistance = Vector3.Distance(transform.position, gunBarrel.transform.position);
    }

}
