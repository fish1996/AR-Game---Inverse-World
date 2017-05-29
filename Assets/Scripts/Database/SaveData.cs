using UnityEngine;  
using System.Collections;  
using Mono.Data.Sqlite;  
using System.IO;  
using System;

public class SaveData{
	private const int TABLENUM = 2;
	private SqliteConnection dbConnection;//数据库连接定义
	private SqliteCommand dbCommand;//SQL命令
	private SqliteDataReader dataReader;//读取
	private StoryData storyData = StoryData.getInstance();
	private FeedData feedData = FeedData.getInstance ();
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
	}

	private void CreateTable(int id) {
		string query;
		switch(id){
		case 0:
			{
				//建表 : story
				query = "CREATE TABLE " + tableName [0] + "(id INT PRIMARY KEY NOT NULL,dialogNum INT NOT NULL," +
				"clueNum INT NOT NULL, chapterNum INT NOT NULL);";
				ExecuteQuery (query);
				//向story存入初始数据
				query = "INSERT INTO " + tableName[0] + " values(1,1,1,1);";
				ExecuteQuery (query);
				break;
			}
		case 1:
			{
				//建表 : feed
				query = "CREATE TABLE " + tableName[1] + "(id INT PRIMARY KEY NOT NULL,num INT NOT NULL," +
					"data INT NOT NULL);";
				ExecuteQuery (query);

				//向feed存入初始数据
				query = "INSERT INTO " + tableName[1] + " values(1,12,0);";
				ExecuteQuery (query);
				break;
			}
		}
	}
		
	public void Save() {
		string updateString = "UPDATE " + tableName[0] + " SET dialogNum = "
		                      + storyData.Dnum + ",clueNum = " 
			                  + storyData.Cnum + ",chapterNum = " 
		                   	  + storyData.Pnum + " WHERE id = 1;";
		ExecuteQuery(updateString);

		int data = 0;
		for (int i = 0; i < feedData.hadChoose.Length; i++) {
			if (feedData.hadChoose [i]) {
				data += (1 << i);
			}
		}
		updateString = "UPDATE " + tableName[1] + " SET data = "
			+ data + " WHERE id = 1;";
		ExecuteQuery(updateString);
	}

	public SaveData () {
		tableName [0] = "story";
		tableName [1] = "feed";
	}

	public void Load(){
		Debug.Log ("save");
		try{
			dbConnection = new SqliteConnection("data source = playerdata.db");
			dbConnection.Open();
		}
		catch(Exception e){
			Debug.Log (e.ToString());
		}
		if (!isTableExist (0)) {
			Debug.Log ("noexist story");
			CreateTable (0);
		} 
		if (!isTableExist (1)) {
			Debug.Log ("noexist feed");
			CreateTable (1);
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