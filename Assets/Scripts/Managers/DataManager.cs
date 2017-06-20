using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataManager : MonoBehaviour,IGameManager {
    public ManagerStatus status { get; private set; }

    private string _filename;//是否会存多个档呢？

    public void Startup()
    {
        Debug.Log("Data manager starting...");
        _filename = Path.Combine(Application.persistentDataPath, "myScene.dat");//构建 game.dat的完整路径
        Debug.Log(_filename);
        status = ManagerStatus.Started;
    }

    public void SaveSceneState()
    {
        Debug.Log("保存场景成功");
        Dictionary<string, object> gamestate = new Dictionary<string, object>();
        //gamestate.Add("inventory", Managers.Inventory.GetData());
        FileStream stream = File.Create(_filename);//在文件路径创建一个文件
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, gamestate);//序列化字典作为创建的文件的内容
        stream.Close();
    }

    public void LoadSceneState()
    {
        Debug.Log("加载场景成功");
        if (!File.Exists(_filename))//只有当文件存在时才继续加载
        {
            Debug.Log("No saved Scene");//这里应该改成界面上的什么东西
            return;
        }
        //Dictionary<string, object> sceneState;//用于放置加载的数据的字典
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = File.Open(_filename, FileMode.Open);
        //sceneState = formatter.Deserialize(stream) as Dictionary<string, object>;
        stream.Close();
        //Managers.Inventory.UpdateData((List<Dictionary<string, int>>) sceneState["inventory"]);

    }
}
