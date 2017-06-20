using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour,IGameManager {
    public ManagerStatus status { get; private set; }
    public void Startup()
    {
        status = ManagerStatus.Started;
    }
}
