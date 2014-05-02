using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.message;
using com.shephertz.app42.gaming.multiplayer.client.transformer;

//using UnityEditor;

using AssemblyCSharp;

public class appwarp : MonoBehaviour 
{
	public string apiKey = "your api key";
	public string secretKey = "your secret Key";
	public string username = "";
	
	Listener listen;

	private int state = 0; 
	public string roomType = "";
	public string roomId = "";
	public bool inChat = false;
	private string userField = "";
	private string chatField = "";
	private string msgField = "";
	private int width = 640;
	private int height = 480;
	
	void Start () {
		WarpClient.initialize(apiKey,secretKey/*, "54.246.103.117"*/);
		listen = GetComponent<Listener>();
		WarpClient.GetInstance().AddConnectionRequestListener(listen);
		WarpClient.GetInstance().AddChatRequestListener(listen);
		WarpClient.GetInstance().AddLobbyRequestListener(listen);
		WarpClient.GetInstance().AddNotificationListener(listen);
		WarpClient.GetInstance().AddRoomRequestListener(listen);
		WarpClient.GetInstance().AddUpdateRequestListener(listen);
		WarpClient.GetInstance().AddZoneRequestListener(listen);

		//WarpClient.setRecoveryAllowance (60);
		username = System.DateTime.UtcNow.Ticks.ToString();
		
		//EditorApplication.playmodeStateChanged += OnEditorStateChanged;

		width = Screen.width;
		height = Screen.height;
	}
	
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
        	Application.Quit();
    	}
		WarpClient.GetInstance().Update();
	}
	
	void OnGUI()
	{
		int layoutHeight = height - 10 * 2;
		GUI.BeginGroup (new Rect (width / 2 - 512 / 2 + 10, 10, 512, layoutHeight));
		
		if (state == 0) {
			userField = GUI.TextField (new Rect (0, 10, 512, 32), userField);
			if (GUI.Button (new Rect (512 / 2 - 128 / 2, 52, 128, 32), "Connect")) {
				//state = 1;
				if(userField == "")
					userField = "Please enter a name";
				else
				{
					username = userField;
					WarpClient.GetInstance().Connect(userField);
					userField = "Connecting. Please wait...";
				}
			}
		} else if (state == 1) {
			GUI.Label (new Rect (0, 10, 512, 32), "Select a type of room you want to join");
			if (GUI.Button (new Rect (512 / 2 - 128 / 2, 52, 128, 32), "Books")) {
				state = 2;
				Dictionary<string,object> dic = new Dictionary<string, object>();
				dic.Add("type","books");
				roomType = "books";
				WarpClient.GetInstance().JoinRoomWithProperties(dic);
				setMessage("Joing Room");
			}
			if (GUI.Button (new Rect (512 / 2 - 128 / 2, 94, 128, 32), "Technology")) {
				state = 2;
				Dictionary<string,object> dic = new Dictionary<string, object>();
				dic.Add("type","technology");
				roomType = "technology";
				WarpClient.GetInstance().JoinRoomWithProperties(dic);
				setMessage("Joing Room");
			}
			if (GUI.Button (new Rect (512 / 2 - 128 / 2, 136, 128, 32), "Movies")) {
				state = 2;
				Dictionary<string,object> dic = new Dictionary<string, object>();
				dic.Add("type","movies");
				roomType = "movies";
				WarpClient.GetInstance().JoinRoomWithProperties(dic);
				setMessage("Joing Room");
			}
			if (GUI.Button (new Rect (512 / 2 - 128 / 2, 178, 128, 32), "Music")) {
				state = 2;
				Dictionary<string,object> dic = new Dictionary<string, object>();
				dic.Add("type","music");
				roomType = "music";
				WarpClient.GetInstance().JoinRoomWithProperties(dic);
				setMessage("Joing Room");
			}
		} else if (state == 2) {
			chatField = GUI.TextField(new Rect(0,0,512,layoutHeight - 32 - 10*2),chatField);
			msgField = GUI.TextField(new Rect(0,layoutHeight - 32 - 10, 364,32), msgField);
			if(GUI.Button(new Rect(374,layoutHeight-32-10,64,32), "Send"))
			{
				if(inChat == false)
				{
					setMessage("Not Ready to start sending chat");
				}
				else
				{
					WarpClient.GetInstance().SendChat(msgField);
					msgField = "";
				}
			}
			if(GUI.Button(new Rect(448,layoutHeight-32-10,64,32), "Quit"))
			{
				WarpClient.GetInstance().UnsubscribeRoom(roomId);
				setMessage("Unsubscribing from room");
			}
		}
		
		GUI.EndGroup ();
	}
	
	/*void OnEditorStateChanged()
	{
    	if(EditorApplication.isPlaying == false) 
		{
			WarpClient.GetInstance().Disconnect();
    	}
	}*/
	
	void OnApplicationQuit()
	{
		//WarpClient.GetInstance().Disconnect();
	}

	public void connect(string user)
	{
		username = user;
		WarpClient.GetInstance ().Connect (user);
	}

	public void setMessage(string msg)
	{
		chatField = msg + "\n" + chatField; 
	}

	public void reset()
	{
		state = 0;
		userField = "";
		msgField = "";
		chatField = "";
	}

	public void onConnect(byte res)
	{
		if(state == 0)
		{
			if (res == 0)
				state = 1;
			else {
				userField = "Error in Connecting...Error : " + res.ToString();
			}
		}
	}
	
}
