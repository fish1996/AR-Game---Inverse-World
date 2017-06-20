using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class InventoryManager : MonoBehaviour,IGameManager {
    public ManagerStatus status { get; private set; }

    public void Startup()
    {
        status = ManagerStatus.Started;

    }

    private void initInventory()
    {
    }

}
