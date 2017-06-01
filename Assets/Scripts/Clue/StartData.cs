using System;

public class StartData
{
	public bool isStart;

	//单例模式
	private static StartData instance = new StartData ();
	private StartData(){}
	public static StartData getInstance(){
		return instance;
	}
}


