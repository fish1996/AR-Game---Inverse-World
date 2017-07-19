using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CiclularProgress : MonoBehaviour {

	public int timeToComplete = 5;
	public static float i = 0;
	//manipulate power text
	public float progress;
	public UILabel powerLabel;
	public Text powerText;
	public bool isAutoProgressing;
	private ManaData manaData = ManaData.getInstance();

	// Use this for initialization
	void Start () {
		if (GameObject.Find ("energy")) {
			powerLabel = GameObject.Find ("energy").GetComponent<UILabel> ();
		}//else do nothing

		//Use this to Start progress
		if (isAutoProgressing) {
			StartCoroutine (RadialProgress (timeToComplete));
		} //else progress according to Mana Value
	}

	void Update(){
		progress=100.0f*gameObject.GetComponent<Renderer> ().material.GetFloat ("_Progress");
		if (GameObject.Find ("energy")) {
			powerLabel.text = progress.ToString ("灵力值 #");
		} //else is controlled by Mana.cs
		if(!isAutoProgressing){
			//gameObject.GetComponent<Renderer>().material.SetFloat("_Progress", manaData.mana/80.0f);
		}
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