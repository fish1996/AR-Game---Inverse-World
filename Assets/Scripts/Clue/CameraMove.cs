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
		if(GUI.RepeatButton(new Rect(0,350,50,80),"Left")){
			LeftMove();
		}
		if(GUI.RepeatButton(new Rect(1250,350,50,80),"Right")){
			RightMove();
		}
	}

    void LeftMove()
    {
        Vector3 nowp = camera.transform.position;
        if (nowp.x > -4507)
            camera.transform.Translate(new Vector3(-100, 0, 0));
    }

    void RightMove()
    {
        Vector3 nowp = camera.transform.position;
        if (nowp.x < 3892)
            camera.transform.Translate(new Vector3(100, 0, 0));
    }
}
