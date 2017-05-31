using System;

public class TimeData
{
    public DateTime time;
    public float deltaTime;
    //单例模式
    private static TimeData instance = new TimeData();
    private TimeData() { }
    public static TimeData getInstance()
    {
        return instance;
    }
}