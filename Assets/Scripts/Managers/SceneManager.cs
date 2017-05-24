using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour, IGameManager {
	public ManagerStatus status {get; private set;}
	
	public int curLevel {get; private set;}
	public int maxLevel {get; private set;}
	
	public void Startup() {
		
		UpdateData(0, 3);
		status = ManagerStatus.Started;
	}

	public void UpdateData(int curLevel, int maxLevel) {
		this.curLevel = curLevel;
		this.maxLevel = maxLevel;
	}

	public void ReachObjective() {
		// could have logic to handle multiple objectives
		//Messenger.Broadcast(GameEvent.LEVEL_COMPLETE);
	}

	public void GoToNext() {
		if (curLevel < maxLevel) {
			curLevel++;
			string name = "Level" + curLevel;
			Debug.Log("Loading " + name);
			Application.LoadLevel(name);//为什么我的SceneManager.LoadScene报错？
		} else {
			Debug.Log("Last level");
			//Messenger.Broadcast(GameEvent.GAME_COMPLETE);
		}
	}

    public void LoadFeedGame()
    {
        Application.LoadLevel("FeedScene");
    }
    public void ReturnFromFeedGame()
    {
        //string name = "Level" + curLevel;//如果有很多场景用这行
        string name = "MainScene";
        Application.LoadLevel(name);
    }

	public void RestartCurrent() {
		string name = "Level" + curLevel;
		Debug.Log("Loading " + name);
		Application.LoadLevel(name);
	}
}
