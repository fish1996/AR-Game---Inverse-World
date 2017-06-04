using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour {

    public Camera camera;
    public Button LeftButton;
    public Button RightButton;

	// Use this for initialization
	void Start () {
        //LeftButton.on
        LeftButton.onClick.AddListener(delegate()
        {
            LeftMove();
        });
        RightButton.onClick.AddListener(delegate()
        {
            RightMove();
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LeftMove()
    {
        Vector3 nowp = camera.transform.position;
        if (nowp.x > -1719785)
            camera.transform.Translate(new Vector3(-100, 0, 0));
    }

    void RightMove()
    {
        Vector3 nowp = camera.transform.position;
        if (nowp.x < 1293211)
            camera.transform.Translate(new Vector3(100, 0, 0));
    }
}
