using System;

public class ClueData
{
	public int all_num ;
	public int index ;
	public int combinationnum;
	public bool[] isActive;//***来自梦迪的线索状态，现在暂时从一个txt中获取得到***14

	//单例模式
	private static ClueData instance = new ClueData ();
	private ClueData(){}
	public static ClueData getInstance(){
		return instance;
	}
}


