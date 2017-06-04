using System;

public class StartData
{
	public bool isStart;
	public int clueNum;
	//单例模式
	private static StartData instance = new StartData ();
	private StartData(){}
	public static StartData getInstance(){
		return instance;
	}
}


