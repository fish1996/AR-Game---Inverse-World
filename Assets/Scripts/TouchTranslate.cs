using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TouchTranslate : MonoBehaviour
{
    private GameObject hitObject;
    private Vector3 _targetPos = new Vector3(100, 0, 600);
    public float rotSpeed = 0.002f;      //旋转速度
    public float transSpeed =200.0f;
    public float targetBuffer = 10.0f;
    private bool TranslateStart = false;
    // Update is called once per frame
    void Update()
    {
        if (TranslateStart)
        {
        }
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Stationary)
       {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                hitObject = hit.transform.gameObject;
                if (hitObject.transform.tag == "MainCharacter") {
                    TranslateStart = true;
                    StartCoroutine(TranslateToCamera());
                }
            }
        }
    }

    private IEnumerator TranslateToCamera()
    {

        yield return new WaitForSeconds(0.5f);

        hitObject.transform.parent = Camera.main.transform;
        hitObject.transform.localPosition = _targetPos;
        hitObject.transform.localRotation = Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
    }
}
