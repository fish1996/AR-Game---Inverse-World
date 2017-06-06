using System;

public class BeginData
{
	public bool isBegin = false;

	//单例模式
	private static BeginData instance = new BeginData ();
	private BeginData(){}
	public static BeginData getInstance(){
		return instance;
	}
}


