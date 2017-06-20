using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showclue : MonoBehaviour {

	public Dialogtext d; 
	public ClueData data; 

	public void showclueif(int Pnum, int Cnum)
	{
		int ordernum = 0;

		for (int p = 0; p < Pnum - 1; p++) {
			ordernum += d.chattext.ptxt [p].cnum; //加上前几关的线索总数
		}

		ordernum += Cnum;

		data.isActive [ordernum - 1] = true;

	}

	void Start()
	{
		d = Dialogtext.GetInstance ();
		data = ClueData.getInstance();
	}
}
