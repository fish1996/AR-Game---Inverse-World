using System;

public class StoryData
{
	public int Dnum; //表示目前的对话进度
	public int Cnum; //表示目前的线索进度
	public int Pnum; //表示目前的关卡进度

	//单例模式
	private static StoryData instance = new StoryData ();
	private StoryData(){}
	public static StoryData getInstance(){
		return instance;
	}
}