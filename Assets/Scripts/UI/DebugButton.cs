using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugButton : MonoBehaviour {

	private StoryData storyData = StoryData.getInstance ();
	private ManaData manaData = ManaData.getInstance();
	private PlaceData placeData = PlaceData.getInstance ();
	private FeedData feedData = FeedData.getInstance ();
	private ClueData clueData = ClueData.getInstance ();
	// Use this for initialization
	void Start () {
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(OnClick);
	}

	// Update is called once per frame
	void Update () {

	}

	void MousePick()
	{
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit))
			{
				Debug.Log(hit.transform.name);
				//Debug.Log(hit.transform.tag);
			}
		}
	}

	private void OnClick()
	{
		storyData.Pnum = 1;
		storyData.Dnum = 1;
		storyData.Cnum = 1;
		placeData.isOnTheMarker = true;
		manaData.mana = 80;
		for (int i = 0; i < feedData.hadChoose.Length; i++) {
			feedData.hadChoose [i] = false;
		}
		for (int i = 0; i < clueData.isActive.Length; i++) {
			clueData.isActive [i] = false;
		}
		clueData.index = 0;
	}
}