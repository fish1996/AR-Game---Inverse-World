using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour {

    public new Camera camera;

	// Use this for initialization
	void Start () {

	}

	void OnGUI() {
		if(GUI.RepeatButton(new Rect(50,350,100,20),"Left")){
			LeftMove();
		}
		if(GUI.RepeatButton(new Rect(500,350,100,20),"Right")){
			RightMove();
		}
	}

    void LeftMove()
    {
        Vector3 nowp = camera.transform.position;
        if (nowp.x > -4707)
            camera.transform.Translate(new Vector3(-100, 0, 0));
    }

    void RightMove()
    {
        Vector3 nowp = camera.transform.position;
        if (nowp.x < 3392)
            camera.transform.Translate(new Vector3(100, 0, 0));
    }
}
