using System;

class UpdateMana
{
    TimeData timeData = TimeData.getInstance();
    ManaData manaData = ManaData.getInstance();

    public void Start()
    {
        DateTime currentTime = DateTime.Now;
        double minute = (currentTime - timeData.time).TotalMinutes;
        double second = (currentTime - timeData.time).TotalSeconds;
        manaData.mana += (int)(minute / 10);
        timeData.deltaTime = (float)(second % 600);
    }
}
