using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HighlightingSystem;

public class Target : MonoBehaviour {

    public GameObject progressBar;//进度条显示力度
    private Object Progress;
    public GameObject gunBarrel;//枪管用于实时对准以及测距
    public GameObject mainController;//主控制器用于返回射击结果
    public GameObject UIRoot;
    private Object bulletPrefab;
    public float ToDistanceSpeed = 800f;//将按键时间转化为射击距离的比例
    public float bulletFlyingSpeed = 500f;//动画效果中的飞行速度
    public float hitThreshold=300.0f;//用距离测试是否击中，如果用碰撞器无法检测超过情况
    private float percent;
    private float fireDistance;//子弹的预计飞行距离
    private float totalDistance;//相机距靶距离
    public float endThreshold = 50.0f;//完成飞行距离 
    //备注，距离都是千级的
    protected Highlighter h;
    private Shader EmptyShader = null;
    private Shader HitShader=null;
    private Material targetMaterial;
    private bool startAnimation;
    private Vector3 direct;
    private GameObject _bullet;
    private float remainDistance;

    void Awake()
    {
        h = GetComponent<Highlighter>();
        if (h == null) { h = gameObject.AddComponent<Highlighter>();
        }
        UIRoot = GameObject.Find("UI Root");
    }

    void Start()
    {
        startAnimation = false;
        bulletPrefab= Resources.Load<Object>("Prefabs/BulletFire");
        //targetMaterial = GetComponent<Renderer>().material;
        //EmptyShader= Shader.Find("Legacy Shaders/Transparent/Specular");
        //HitShader = Shader.Find("FX/Gem");
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
        Debug.Log("percent"+percent);
        //progressBar.GetComponent<UISlider>().Set(percent);
        //progressBar.GetComponent<Renderer>().material.SetFloat("_Progress", percent);

        //枪管部分
        //gunBarrel.transform.LookAt(transform,-Vector3.forward);//第二个参数为向上方向
        gunBarrel.transform.LookAt(transform.position);
        //gunBarrel.transform.Rotate(new Vector3(90, 0, 0));
    }

    public IEnumerator Fire(float pressTime)
    {
        if (!startAnimation)
        {
            //射击效果
            calcuLateDistance(pressTime);
            direct = transform.position - gunBarrel.transform.position;
            direct.Normalize();
            direct /= 140;
            _bullet = Instantiate(bulletPrefab) as GameObject;
            _bullet.transform.parent = gunBarrel.transform;
            _bullet.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            _bullet.transform.localRotation = Quaternion.identity;
            //_bullet.transform.Rotate(new Vector3(-90, 0, 0));
            remainDistance = fireDistance;
            startAnimation = true;
        }
        while (remainDistance > endThreshold) //未完成飞行
        {
            float step = bulletFlyingSpeed * Time.deltaTime;
            remainDistance -= step;
            //_bullet.transform.Translate(step * direct);
            yield return 0;
        }
        //击中判定
        //////////////////击中动画待添加
        Destroy(_bullet);
        startAnimation = false;
        h.ConstantOffImmediate();
        if ((totalDistance- hitThreshold )< fireDistance && fireDistance < (totalDistance + hitThreshold))
        {
            mainController = GameObject.FindWithTag("MainController_Feed");
            mainController.GetComponent<MainController>().HitTransform();
            mainController.GetComponent<MainController>().AlterToNextTarget();       
            //targetMaterial.shader = HitShader;
            Debug.Log("打中了！！");
        }
        else
        {
            if((totalDistance - hitThreshold) > fireDistance)//less
            {
                UIRoot.GetComponent<ButtonManager_Feed>().showLessText();
             /*   GameObject less = new GameObject("less");
                less.transform.position = new Vector3(0, 0, 150);
                Sprite spr = Resources.Load<Sprite>("image/less");
                less.AddComponent<SpriteRenderer>().sprite = spr;
                less.GetComponent<SpriteRenderer>().sortingOrder = 3;
                Destroy(less, 2);
                print("less");*/

            }
            if(fireDistance > (totalDistance + hitThreshold))
            {
                UIRoot.GetComponent<ButtonManager_Feed>().showMoreText();
                /*   GameObject more = new GameObject("more");
                   more.transform.position = new Vector3(0, 0, 150);
                   Sprite spr = Resources.Load<Sprite>("image/more");
                   more.AddComponent<SpriteRenderer>().sprite = spr;
                   more.GetComponent<SpriteRenderer>().sortingOrder = 3;
                   Destroy(more, 2);
                   print("more");*/
            }
            mainController = GameObject.FindWithTag("MainController_Feed");
            /////////mainController.GetComponent<MainController>().StopTheGame();
            mainController.GetComponent<MainController>().RestartToThisTarget();
            //mainController.GetComponent<MainController>().HitTransform();
            Debug.Log("失败");
        }
    }

    private void calcuLateDistance(float pressTime) {
        fireDistance = ToDistanceSpeed* CiclularProgress.i;
        totalDistance = Vector3.Distance(transform.position, gunBarrel.transform.position);
        Debug.Log("totalDistance"+totalDistance);
    }

}
