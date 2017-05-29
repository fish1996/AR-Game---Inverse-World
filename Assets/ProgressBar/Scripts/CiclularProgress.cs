using UnityEngine;
using System.Collections;

public class CiclularProgress : MonoBehaviour {
	
	public int timeToComplete = 5;
    public static float i = 0;

    // Use this for initialization
    void Start () {
		//Use this to Start progress
		StartCoroutine(RadialProgress(timeToComplete));
	}
	
	IEnumerator RadialProgress(float time)
	{
		float rate = 1 / time;
		while (true)
		{
            if (i < 1)
            {
                i += Time.deltaTime * rate;
                gameObject.GetComponent<Renderer>().material.SetFloat("_Progress", i);
                yield return 0;
            }
            else
            {
                i = 0;
            }
		}
	}
}