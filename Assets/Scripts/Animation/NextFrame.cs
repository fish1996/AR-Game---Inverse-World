using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.IO;

public class NextFrame : MonoBehaviour {

    public float near = 0.01f;
    public float far = 100.0f;

    GameObject[] textureObj = new GameObject[150];
    GameObject btnObj;
    public Camera camera;//This script is to be bound on a camera
    public BrightnessSaturationAndContrast bscCamera;
    float initialBrightness;
    public Vector3 cameraInitialPosZ;
    public int framecount;
    public bool isNextFrameButtonDown;
    public float fSpeed;

    float pixel2units;
    Vector3 targetPos;
    Vector3 currentPos;
    public bool isAbruptTrans;
    public int level;
    int firstFrame;
    int maxFrameNum;
    const int totalFrameNum = 56;
    bool fadeInorOut;
    // Use this for initialization
    private void Awake()//called only once before start
    {
        //Texture Layout
        isNextFrameButtonDown = false;

        btnObj = GameObject.Find("NextFrameButton");
        //pixel2units=btnObj.GetComponent<Image>().sprite.pixelsPerUnit;
        pixel2units = 85;
        level = 5;//may changed elsewhere in game
        switch (level) {
            case 1:
                firstFrame = 1;
                maxFrameNum = 18;
                break;
            case 2:
                firstFrame = 21;
                maxFrameNum = 29;
                break;
            case 3:
                firstFrame = 31;
                maxFrameNum = 36;
                break;
            case 4:
                firstFrame = 41;
                maxFrameNum = 53;
                break;
            case 5:
                firstFrame = 55;
                maxFrameNum = 64;
                break;
            default:
                firstFrame = 1;
                maxFrameNum = 18;
                //to another scene
                break;
        }
        framecount = firstFrame;
        UITexture tmptex;
        Vector3 p;
        for (int i = firstFrame; i <= maxFrameNum; i++)
        {
            textureObj[i] = GameObject.Find("Texture" + i);
            textureObj[i].AddComponent<UITexture>();
            tmptex = textureObj[i].GetComponent<UITexture>();
            tmptex.SetDimensions(2560, 1440);
            p = 2560 / 2 / pixel2units * (i - 1) * Vector3.right;
            tmptex.transform.SetPositionAndRotation(p, Quaternion.identity);

            if (GameObject.Find("Texture" + i + "_2") == true)
            {//assign to textureObj if exist
                textureObj[i + totalFrameNum] = GameObject.Find("Texture" + i + "_2");
                textureObj[i + totalFrameNum].AddComponent<UITexture>();
            }
        }

        for (int i = firstFrame + totalFrameNum, j; i <= maxFrameNum + totalFrameNum; i++)
        {
            //front frames
            j = i - totalFrameNum;//i is index in textureObj[],j is corresponding background frame
            if (GameObject.Find("Texture" + j + "_2") == false) continue;
            
            tmptex = textureObj[i].GetComponent<UITexture>();
            tmptex.SetDimensions(2048, 1152);
            p = 2560 / 2 / pixel2units * (j - 1) * Vector3.right + 1000 / 2 / pixel2units * Vector3.back;
            tmptex.transform.SetPositionAndRotation(p, Quaternion.identity);

        }

        //camera initialization
        camera = GameObject.Find("Camera").GetComponent<Camera>();
        camera.transform.SetPositionAndRotation(2700 / 2 / pixel2units * Vector3.back + textureObj[firstFrame].transform.position, Quaternion.identity);
        cameraInitialPosZ = Vector3.forward * camera.transform.position.z;//手动设置

        //PostEffects initialization
        bscCamera = camera.GetComponent<BrightnessSaturationAndContrast>();
        bscCamera.brightness = 0.0f;
        
    }

    void Start() {
        isAbruptTrans = true;
        fSpeed = 0.5f;

        Button btn = btnObj.GetComponent<Button>();
        btn.onClick.AddListener(delegate () {
            this.OnClick(btnObj);//event
        });
        //}
    }

    // Update is called once per frame
    void FixedUpdate() {
        //frame transition
        targetPos = cameraInitialPosZ + textureObj[framecount].transform.position;

        currentPos = this.transform.position;


        if (currentPos.x > targetPos.x - 0.5f&& currentPos.x < targetPos.x-0.01f) {
            if (isAbruptTrans == true) {
                FadeInnOut(false);//fadeout 
                Debug.Log(framecount+"fadeout");
            }
        }
        //else if (currentPos.x < targetPos.x - 2560 / 2 / pixel2units + 0.5f)
        //    FadeInnOut(true);//fadein
        else
        {
            FadeInnOut(true);//fadein
            Debug.Log(framecount + "fadein");
            if (level == 5 && framecount == maxFrameNum) {
                if(bscCamera.brightness<5.0f)
                    bscCamera.brightness += 0.01f;
            }
        }

        if (isNextFrameButtonDown) {
            //Gradual Transition
            if (isAbruptTrans == false)
            {
                if (fSpeed > 0.2f) fSpeed -= 0.01f;
                else fSpeed = 0.2f;

                if (currentPos.x < targetPos.x + 0.1f)
                {
                    this.transform.Translate(fSpeed * Vector3.right);//transform.TransformPoint(fSpeed * Vector3.right)
                }
                else
                {//reach targetpos
                    fSpeed = 0.5f;
                    this.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
                    isNextFrameButtonDown = false;
                    isAbruptTrans = true;
                }

            }
            //Abrupt Transition
            else
            {           
                this.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
                isNextFrameButtonDown = false;      
            }

            //size (orthographicSize)，size的含义为屏幕高度的一半，不过单位不是像素而是unit坐标，即通过pixels to units换算的坐标。例如：屏幕高度为640，pixels to 
        }

    }

    public void OnClick(GameObject sender)
    {
        switch (sender.name)
        {

            case "NextFrameButton":
                framecount++;
                //if (framecount > maxFrameNum)
                //{
                //    level++ ;
                //}
                if (framecount == 12) { framecount += 5; isAbruptTrans = false; }//和小女孩在山上的场景
                if (framecount == 42) { framecount += 3; isAbruptTrans = false; }//城市场景
                if (framecount == 24|| framecount == 28|| framecount == 34|| framecount == 45|| framecount == 48|| framecount == 50) { isAbruptTrans = false; }
                if (framecount == 52) { framecount += 1; isAbruptTrans = false; }//城市场景
                if (framecount == 56) { framecount += 2; isAbruptTrans = false; }
                if (framecount == 62) { framecount += 1; isAbruptTrans = false; }
                isNextFrameButtonDown = true;
                bscCamera.brightness = 0.0f;
                break;

            default:
                Debug.Log("none");
                break;
        }

    }

    void FadeInnOut(bool inorout) {//true-in false-out
        if (inorout == true)
        {//fade in
            if (bscCamera.brightness < 1.0f) {
                bscCamera.brightness += 0.02f;
            }
            
        }
        else {//fade out
            if (bscCamera.brightness > 0.0f)
            {
                bscCamera.brightness -= 0.02f;
            }

        }
        

    }

}
