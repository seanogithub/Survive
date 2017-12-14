using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml; 
using System.Xml.Serialization; 
using System.IO; 
using System.Text;
using System.Linq;


public class PlayerManager : MonoBehaviour {

	public GameObject UIManager;
	public GameObject Player;
	public string VersionNumber = "";
	string _FileLocation, _PlayerDataFileName;
	string _data; 
	public SavePlayerData UserPlayerData;
	public PlayerData CurrentPlayer;
	public int CurrentPlayerIndex;
	public int MaxNumberOfPlayers = 30;

	void Awake ()
	{
		VersionNumber = "1.0";
		_FileLocation = Application.persistentDataPath;
		//Debug.Log(_FileLocation);
		_PlayerDataFileName = "PlayerData.xml";  // temporarily get these locally. change this to be downloaded from the server
		UserPlayerData = new SavePlayerData();
	}

	// Use this for initialization
	void Start () {
		if (File.Exists(_FileLocation+"/"+ _PlayerDataFileName))
		{
			LoadPlayerData();
		}		
		else
		{
			PlayerDataInit PlayerDataString = new PlayerDataInit();
			CreateFileFromString (_PlayerDataFileName, PlayerDataString.InitData);
			LoadPlayerData();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetCurrentPlayerByIndexString(string NewPlayerIndex)
	{
		int Index = int.Parse(NewPlayerIndex);
		if(UserPlayerData.Player[Index] != null)
		{
			CurrentPlayer = UserPlayerData.Player[Index];
			CurrentPlayerIndex = Index;
		}
	}

	public void UpdateCurrentPlayer(int Index)
	{
		if(UserPlayerData.Player[Index] != null)
		{
			UserPlayerData.Player[Index] = CurrentPlayer;
		}
	}

	public void SavePlayer()
	{
		//		myPlayerData.UserPlayerData = UserPlayerData;
		SavePlayerDataXML();
	}

	public void LoadPlayerData()
	{
		_data = LoadXML(_FileLocation + "/" + _PlayerDataFileName); 
		if(_data.ToString() != "") 
		{ 
			UserPlayerData = (SavePlayerData)DeserializeObject(_data, "SavePlayerData"); 
		}		
	}

	public void SavePlayerDataXML()
	{
		// Time to creat our XML! 
		_data = SerializeObject(UserPlayerData, "SavePlayerData"); 

		// This is the final resulting XML from the serialization process 
		CreateXML(_PlayerDataFileName, _data); 
	}

	/* The following metods came from the referenced URL */
	string UTF8ByteArrayToString(byte[] characters) 
	{      
		UTF8Encoding encoding = new UTF8Encoding(); 
		string constructedString = encoding.GetString(characters); 
		return (constructedString); 
	} 

	byte[] StringToUTF8ByteArray(string pXmlString) 
	{ 
		UTF8Encoding encoding = new UTF8Encoding(); 
		byte[] byteArray = encoding.GetBytes(pXmlString); 
		return byteArray; 
	} 

	// Here we deserialize it back into its original form 
	object DeserializeObject(string pXmlizedString, string myType) 
	{ 
		// type of need to be the type of data we are saving
		//		XmlSerializer xs = new XmlSerializer(typeof(MapBuildable[])); 
		XmlSerializer xs = new XmlSerializer(typeof(SavePlayerData)); 
		switch (myType) 
		{
		case "SavePlayerData":
			xs = new XmlSerializer(typeof(SavePlayerData)); 
			break;
		}
		MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(pXmlizedString)); 
		return xs.Deserialize(memoryStream); 
	} 

	string SerializeObject(object pObject, string myType) 
	{ 
		string XmlizedString = null; 
		MemoryStream memoryStream = new MemoryStream(); 
		// type of need to be the type of data we are saving
		XmlSerializer xs = new XmlSerializer(typeof(SavePlayerData)); 
		switch (myType) 
		{
		case "SavePlayerData":
			xs = new XmlSerializer(typeof(SavePlayerData)); 
			break;
		}

		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8); 
		xmlTextWriter.Settings.Indent = true;
		xmlTextWriter.Formatting = Formatting.Indented ;
		xs.Serialize(xmlTextWriter, pObject); 
		memoryStream = (MemoryStream)xmlTextWriter.BaseStream; 
		XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray()); 
		return XmlizedString; 
	} 

	void CreateXML(string myFileName, string myDataString) 
	{ 
		StreamWriter writer; 
		FileInfo t = new FileInfo(_FileLocation+"/"+ myFileName); 
		if(!t.Exists) 
		{ 
			writer = t.CreateText(); 
		} 
		else 
		{ 
			t.Delete ();
			writer = t.CreateText(); 
		} 
		writer.Write(myDataString); 
		writer.Close(); 
		//		Debug.Log("File written."); 
	} 

	string LoadXML(string myFileName) 
	{ 
		StreamReader r = File.OpenText(myFileName); 
		string _info = r.ReadToEnd(); 
		r.Close(); 
		return _info;
		//		Debug.Log("File Read"); 
	} 	

	void CreateFileFromString(string myFileName, string myDataString) 
	{ 
		StreamWriter writer; 
		FileInfo t = new FileInfo(_FileLocation+"/"+ myFileName); 
		if(!t.Exists) 
		{ 
			writer = t.CreateText(); 
		} 
		else 
		{ 
			t.Delete ();
			writer = t.CreateText(); 
		} 
		writer.Write(myDataString); 
		writer.Close(); 
		//		Debug.Log("File written."); 
	}
}
