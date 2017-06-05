using System;

public class NameData
{
    public String playerName;

    //单例模式
    private static NameData instance = new NameData();
    private NameData() { }
    public static NameData getInstance()
    {
        return instance;
    }
}