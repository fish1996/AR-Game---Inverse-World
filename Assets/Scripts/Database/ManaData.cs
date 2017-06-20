using System;

public class ManaData
{
	public float mana;

	//µ¥ÀıÄ£Ê½
	private static ManaData instance = new ManaData ();
	private ManaData(){}
	public static ManaData getInstance(){
		return instance;
	}
}