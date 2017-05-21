using UnityEngine;  
using System.Collections;  
using Mono.Data.Sqlite;  
using System.IO;  
using System;

public class SaveData{
	
	private SqliteConnection dbConnection;//数据库连接定义
	private SqliteCommand dbCommand;//SQL命令
	private SqliteDataReader dataReader;//读取
	private StoryData data = StoryData.getInstance();
	private string tableName = "story";
	private bool isTableExist() {		
		string query = "SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = '"
			+ tableName + "'";
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
		string query = "SELECT * FROM "
			+ tableName;
		dataReader = ExecuteQuery (query);
		while (dataReader.Read()) {
			data.Dnum = dataReader.GetInt32 (dataReader.GetOrdinal("dialogNum"));
			data.Pnum = dataReader.GetInt32 (dataReader.GetOrdinal("chapterNum"));
			data.Cnum = dataReader.GetInt32 (dataReader.GetOrdinal("clueNum"));
		}
		Debug.Log (data.Dnum + " " + data.Pnum + " " + data.Cnum);
	}

	private void CreateTable() {
		string query = "CREATE TABLE " + tableName + "(id INT PRIMARY KEY NOT NULL,dialogNum INT NOT NULL," +
			"clueNum INT NOT NULL, chapterNum INT NOT NULL);";
		ExecuteQuery (query);
		query = "INSERT INTO " + tableName + " values(1,1,1,1);";
		ExecuteQuery (query);
	}

	void OnGUI(){
		GUI.Label(new Rect(10,10,200,20),Convert.ToString(data.Pnum));
	}

	public void Save() {
		string updateString = "UPDATE story SET dialogNum = "
		                      + data.Dnum + ",clueNum = " 
			                  + data.Cnum + ",chapterNum = " 
		                   	  + data.Cnum + " WHERE id = 1;";
		ExecuteQuery(updateString);
		CloseConnection ();
	}

	public SaveData () {
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
		if (!isTableExist ()) {
			Debug.Log ("noexist");
			CreateTable ();
			data.Dnum = 1;
			data.Cnum = 1;
			data.Pnum = 1;
		} 
		else {
			Debug.Log ("exist");
			LoadPlayerData ();
		}
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