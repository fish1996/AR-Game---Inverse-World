using System;

public class PlaceData
{
	public bool isOnTheMarker;

	//单例模式
	private static PlaceData instance = new PlaceData ();
	private PlaceData(){}
	public static PlaceData getInstance(){
		return instance;
	}
}