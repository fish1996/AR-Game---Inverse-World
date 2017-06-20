using System;

public class ChooseData
{
	public bool isChooseName;

	//单例模式
	private static ChooseData instance = new ChooseData ();
	private ChooseData(){}
	public static ChooseData getInstance(){
		return instance;
	}
}


