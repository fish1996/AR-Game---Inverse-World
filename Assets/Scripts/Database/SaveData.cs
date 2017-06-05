using UnityEngine;  
using System.Collections;  
using Mono.Data.Sqlite;  
using System.IO;  
using System;

public class SaveData{
	private const int TABLENUM = 7;
	private SqliteConnection dbConnection;//数据库连接定义
	private SqliteCommand dbCommand;//SQL命令
	private SqliteDataReader dataReader;//读取
	private StoryData storyData = StoryData.getInstance ();
	private FeedData feedData = FeedData.getInstance ();
	private ClueData clueData = ClueData.getInstance ();
    private ManaData manaData = ManaData.getInstance ();
    private TimeData timeData = TimeData.getInstance ();
	private PlaceData placeData = PlaceData.getInstance ();
    private NameData nameData = NameData.getInstance();
	private string[] tableName = new string[TABLENUM];
	private bool isTableExist(int id) {		
		string query = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = '"
			+ tableName[id] + "'";
		dataReader = ExecuteQuery (query);
		int ans = 0;
		while (dataReader.Read()) {
			ans = dataReader.GetInt32 (0);
		}
		if (ans == 0) {
			return false;
		}
		else return true;
	}

	private SqliteDataReader ExecuteQuery(string queryString){
		SqliteDataReader dataReader = null;
		Debug.Log (queryString);
		try{
		    dbCommand = dbConnection.CreateCommand ();
		    dbCommand.CommandText = queryString;
		    dataReader = dbCommand.ExecuteReader ();
		}
		catch(Exception e){
			Debug.Log (e.ToString ());
		}
		return dataReader;
	}

	private void LoadPlayerData(){
		//加载story data
		string query = "SELECT * FROM "
			+ tableName[0];
		dataReader = ExecuteQuery (query);
		while (dataReader.Read()) {
			storyData.Dnum = dataReader.GetInt32 (dataReader.GetOrdinal("dialogNum"));
			storyData.Pnum = dataReader.GetInt32 (dataReader.GetOrdinal("chapterNum"));
			storyData.Cnum = dataReader.GetInt32 (dataReader.GetOrdinal("clueNum"));
		}
		Debug.Log (storyData.Dnum + " " + storyData.Pnum + " " + storyData.Cnum);

		//加载feed data
		query = "SELECT * FROM "
			+ tableName[1];
		dataReader = ExecuteQuery (query);
		int len = 12,data = 0;
		while (dataReader.Read()) {
			len = dataReader.GetInt32 (dataReader.GetOrdinal("num"));
			data = dataReader.GetInt32 (dataReader.GetOrdinal("data"));
		}
		Debug.Log ("len = " + len + " data = " + data);
		feedData.hadChoose = new bool[len];
		for (int i = 0; i < len; i++) {
			feedData.hadChoose [i] = (data & 1) > 0;
			Debug.Log ("feed " + i + " " + feedData.hadChoose [i]);
			data >>= 1;
		}

		//加载clue data
		query = "SELECT * FROM "
			+ tableName[2];
		dataReader = ExecuteQuery (query);

		while (dataReader.Read()) {
			clueData.all_num = dataReader.GetInt32 (dataReader.GetOrdinal("cluenum"));
			clueData.combinationnum = dataReader.GetInt32 (dataReader.GetOrdinal("combinationnum"));
			clueData.index = dataReader.GetInt32 (dataReader.GetOrdinal("indice"));
			data = dataReader.GetInt32 (dataReader.GetOrdinal("isactive"));
		}
		Debug.Log ("cluenum = " + clueData.all_num
			+ " combinationnum = " + clueData.combinationnum
			+ "index = " + clueData.index + " data = " + data);
		clueData.isActive = new bool[clueData.all_num];
		for (int i = 0; i < clueData.all_num; i++) {
			clueData.isActive [i] = (data & 1) > 0;
			Debug.Log ("feed " + i + " " + clueData.isActive [i]);
			data >>= 1;
		}
        //加载mana data
        query = "SELECT * FROM "
            + tableName[3];
        dataReader = ExecuteQuery(query);
        while (dataReader.Read())
        {
            manaData.mana = dataReader.GetFloat(dataReader.GetOrdinal("mana"));
        }

        //加载time data
        query = "SELECT * FROM "
            + tableName[4];
        dataReader = ExecuteQuery(query);
        String time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        while (dataReader.Read())
        {
            time = dataReader.GetString(dataReader.GetOrdinal("time"));
        }
		Debug.Log ("last time=" + time);
        timeData.time = Convert.ToDateTime(time);

		//加载isonthremarker data
		query = "SELECT * FROM "
			+ tableName[5];
		dataReader = ExecuteQuery(query);

		while (dataReader.Read())
		{
			data = dataReader.GetInt32(dataReader.GetOrdinal("isonthemarker"));
		}
		Debug.Log ("isonthemarker=" + data);
		if (data == 0) {
			placeData.isOnTheMarker = false;
		} else {
			placeData.isOnTheMarker = true;
		}

        //加载name data
        query = "SELECT * FROM "
            + tableName[6];
        dataReader = ExecuteQuery(query);

        while (dataReader.Read())
        {
            nameData.playerName = dataReader.GetString(dataReader.GetOrdinal("name"));
            Debug.Log("name=" + nameData.playerName);
        }
    }

	private void CreateTable(int id) {
		string query;
		switch(id){
		case 0:
                {
                    //建表 : story
                    query = "CREATE TABLE " + tableName[0] + "(id INT PRIMARY KEY NOT NULL,dialogNum INT NOT NULL," +
                    "clueNum INT NOT NULL, chapterNum INT NOT NULL);";
                    ExecuteQuery(query);
                    //向story存入初始数据
                    query = "INSERT INTO " + tableName[0] + " values(1,1,1,1);";
                    ExecuteQuery(query);
                    break;
                }
            case 1:
                {
                    //建表 : feed
                    query = "CREATE TABLE " + tableName[1] + "(id INT PRIMARY KEY NOT NULL,num INT NOT NULL," +
                        "data INT NOT NULL);";
                    ExecuteQuery(query);

                    //向feed存入初始数据
                    query = "INSERT INTO " + tableName[1] + " values(1,12,0);";
                    ExecuteQuery(query);
                    break;
                }
            case 2:
                {
                    //建表 : clue
                    query = "CREATE TABLE " + tableName[2] + "(id INT PRIMARY KEY NOT NULL,cluenum INT NOT NULL," +
                        "indice INT NOT NULL, combinationnum INT NOT NULL, isactive INT NOT NULL);";
                    ExecuteQuery(query);

                    //向clue存入初始数据
                    query = "INSERT INTO " + tableName[2] + " values(1,14,0,12,0);";
                    ExecuteQuery(query);
                    break;
                }
            case 3:
                {
                    //建表 : mana
                    query = "CREATE TABLE " + tableName[3] + "(id INT PRIMARY KEY NOT NULL,mana FLOAT NOT NULL);";
                    ExecuteQuery(query);

                    //向mana存入初始数据
                    query = "INSERT INTO " + tableName[3] + " values(1,80);";
                    ExecuteQuery(query);
                    break;
                }
            case 4:
                {
                    //建表 : time
                    query = "CREATE TABLE " + tableName[4] + "(id INT PRIMARY KEY NOT NULL,time TEXT NOT NULL);";
                    ExecuteQuery(query);

                    String time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //向time存入初始数据
                    query = "INSERT INTO " + tableName[4] + " values(1,'" + time + "');";
                    ExecuteQuery(query);
                    break;
                }
		   case 5:
		    	{
		    		//建表 : place
		    		query = "CREATE TABLE " + tableName[5] + "(id INT PRIMARY KEY NOT NULL,isonthemarker INT NOT NULL);";
		    		ExecuteQuery(query);
		    
		    		//向place存入初始数据
		    		query = "INSERT INTO " + tableName[5] + " values(1,1);";
		    		ExecuteQuery(query);
		    		break;
		    	}
            case 6:
                {
                    //建表 : place
                    query = "CREATE TABLE " + tableName[6] + "(id INT PRIMARY KEY NOT NULL,name TEXT NOT NULL);";
                    ExecuteQuery(query);
                    break;
                }
        }
	}
		
	public void Save() {
		// 保存story信息
		string updateString = "UPDATE " + tableName[0] + " SET dialogNum = "
		                      + storyData.Dnum + ",clueNum = " 
			                  + storyData.Cnum + ",chapterNum = " 
		                   	  + storyData.Pnum + " WHERE id = 1;";
		ExecuteQuery(updateString);

		// 保存feed信息
		int data = 0;
		for (int i = 0; i < feedData.hadChoose.Length; i++) {
			if (feedData.hadChoose [i]) {
				data += (1 << i);
			}
		}
		updateString = "UPDATE " + tableName[1] + " SET data = "
			+ data + " WHERE id = 1;";
		ExecuteQuery(updateString);

		// 保存clue信息
		data = 0;
		for (int i = 0; i < clueData.isActive.Length; i++) {
			if (clueData.isActive [i]) {
				data += (1 << i);
			}
		}
		updateString = "UPDATE " + tableName[2] + " SET isactive = "
			+ data + ",indice = " + clueData.index + " WHERE id = 1;";
		ExecuteQuery(updateString);

        // 保存mana信息
        updateString = "UPDATE " + tableName[3] + " SET mana = "
            + manaData.mana + " WHERE id = 1;";
        ExecuteQuery(updateString);

        // 保存时间
        updateString = "UPDATE " + tableName[4] + " SET time = '" 
            + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE id = 1;";
        ExecuteQuery(updateString);

		if (placeData.isOnTheMarker) {
			data = 1;
		} 
		else {
			data = 0;
		}
		// 保存位置
		updateString = "UPDATE " + tableName[5] + " SET isonthemarker = " 
			+ data + " WHERE id = 1;";
		ExecuteQuery(updateString);

    }

    public void SavePlayerName()
    {
        String query = "SELECT count(*) FROM " + tableName[6];
        ExecuteQuery(query);
        int count = 0;
        while (dataReader.Read())
        {
            count = dataReader.GetInt32(0);
        }
        if(count > 0)
        {
            return;
        }
        //向place存入初始数据
        query = "INSERT INTO " + tableName[6] + " values(1,'" + nameData.playerName + "');";
        ExecuteQuery(query);
    }


    public SaveData () {
		tableName [0] = "story";
		tableName [1] = "feed";
		tableName [2] = "clue";
        tableName [3] = "mana";
        tableName [4] = "time";
		tableName [5] = "place";
        tableName [6] = "name";
	}

	public void Load(){
//		Debug.Log ("save");
		try{
			dbConnection = new SqliteConnection("data source = playerdata.db");
			dbConnection.Open();
		}
		catch(Exception e){
			Debug.Log (e.ToString());
		}
		for (int i = 0; i < TABLENUM; i++) {
			if (!isTableExist (i)) {
				Debug.Log ("noexist: " + i);
				CreateTable (i);
			} 
		}
		LoadPlayerData ();
	}

	public void CloseConnection(){
		if (dbCommand != null) {
			dbCommand.Cancel ();
		}
		dbCommand = null;

		if (dataReader != null) {
			dataReader.Close ();
		}
		dataReader = null;

		if (dbConnection != null) {
			dbConnection.Close ();
		}
		dbConnection = null;
	}
}