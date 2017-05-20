using UnityEngine;  
using System.Collections;  
using Mono.Data.Sqlite;  
using System.IO;  
using System;

public class SaveData : MonoBehaviour {
	private SqliteConnection dbConnection;//数据库连接定义
	private SqliteCommand dbCommand;//SQL命令
	private SqliteDataReader dataReader;//读取
	public void save() {
	}
	public SaveData () {
		try{
			dbConnection = new SqliteConnection();
			dbConnection.Open();
		}
		catch(Exception e){
			Debug.Log (e.Message);
		}
	}

	public SqliteDataReader ExecuteQuery(string queryString){
		dbCommand = dbConnection.CreateCommand();
		dbCommand.CommandText = queryString;
		dataReader = dbCommand.ExecuteReader ();
		return dataReader;
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
	


