using System;

public class ManaData
{
	public float mana;

	//����ģʽ
	private static ManaData instance = new ManaData ();
	private ManaData(){}
	public static ManaData getInstance(){
		return instance;
	}
}