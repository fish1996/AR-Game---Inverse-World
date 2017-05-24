using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(InventoryManager))]
[RequireComponent(typeof(DataManager))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(SceneManager))]

public class Managers : MonoBehaviour {
    
    public static InventoryManager Inventory { get; private set; }
    public static DataManager Data{ get; private set; }
    public static PlayerManager Player { get; private set; }
    public static SceneManager Scene { get; private set; }

    private List<IGameManager> _startSequence;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Data = GetComponent<DataManager>();
        Inventory = GetComponent<InventoryManager>();
        Player = GetComponent<PlayerManager>();
        Scene = GetComponent<SceneManager>();

        _startSequence = new List<IGameManager>();
        _startSequence.Add(Inventory);
        _startSequence.Add(Player);
        _startSequence.Add(Scene);
        _startSequence.Add(Data);//DataManager使用其他管理器，所以应该确保其他管理器在启动序列中出现在DataManager之前

        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        foreach(IGameManager manager in _startSequence)
        {
            manager.Startup();
        }
        yield return null;
        int numModules = _startSequence.Count;
        int numReady = 0;
        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;
            foreach(IGameManager manager in _startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }
            if (numReady > lastReady)
            {
                yield return null;
            }
        }
    }
}
