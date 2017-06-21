using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//注意这个不能少
//using UnityEngine.SceneManagement;  
//using UnityEditor.Sprites ;
using System.IO;

public class ButtonControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		List<string> btnsName = new List<string>();
		btnsName.Add("MenuButton");
		btnsName.Add("ClueButton");
		btnsName.Add("FoodButton");

		foreach(string btnName in btnsName)
		{
			GameObject btnObj = GameObject.Find(btnName);
			Button btn = btnObj.GetComponent<Button>();
			btn.onClick.AddListener(delegate() {
				this.OnClick(btnObj); 
			});
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnClick(GameObject sender)
	{
		switch (sender.name)
		{
		case "MenuButton":
			break;

		case "ClueButton":
            UnityEngine.SceneManagement.SceneManager.LoadScene("Cues"); 
			break;
		
		case "FoodButton":
			//Managers.Scene.LoadFeedGame ();
			UnityEngine.SceneManagement.SceneManager.LoadScene ("FeedScene"); 
			//Application.LoadLevel (n);
			
			break;

		default:
			Debug.Log("none");
			break;
		}

	}
}

