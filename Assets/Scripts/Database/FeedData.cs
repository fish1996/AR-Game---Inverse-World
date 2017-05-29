using System;

public class FeedData
{
	public bool[] hadChoose;

	//单例模式
	private static FeedData instance = new FeedData ();
	private FeedData(){}
	public static FeedData getInstance(){
		return instance;
	}
}