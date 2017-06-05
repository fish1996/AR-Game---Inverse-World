using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
/*
 *用于：1.在最初进入游戏时，扫码出现主角，点击主角挂至相机
 * 2.在再次进入游戏时，直接出现在相机 
 */
public class TouchTranslate : MonoBehaviour
{
    private GameObject hitObject;

    private Vector3 _targetPos = new Vector3(100, -300, 600);

    public float rotSpeed = 0.002f;      //旋转速度
    public float transSpeed =200.0f;
    public float targetBuffer = 10.0f;
    private bool TranslateStart = false;

    void Start()
    {

    }

    void Update()
    {
        if (TranslateStart)
        {
			GameObject.Find ("controlchat").GetComponent<Beginfirstchat> ().Beginfirst ();
        }
#if UNITY_EDITOR
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IPHONE
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary){
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
#endif
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hitObject = hit.transform.gameObject;
                if (hitObject.transform.tag == "MainCharacter") {
                    TranslateStart = true;
                    TranslateToCamera();
                }
            }
        }
    }

    public void TranslateToCamera()
    {

        transform.parent = Camera.main.transform;
        transform.localPosition = _targetPos;
        transform.localRotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
    }
}
