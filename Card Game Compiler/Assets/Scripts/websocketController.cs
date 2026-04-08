using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using TMPro;
using System.Threading.Tasks;

public class websocketController : MonoBehaviour
{
    public SocketIOUnity socket;
    public buttonCreator bC; 
    public TMP_Text userText;
    public TMP_Text ErrorSignUp;
    public TMP_Text ErrorLogIn;
    public TMP_InputField SUusername;
    public TMP_InputField SUpassword;
    public TMP_InputField SUdisplayName;
    public TMP_InputField LIusername;
    public TMP_InputField LIpassword;
    public GameObject signUpPanel;
    public GameObject logInPanel;
    public GameObject authButtons;
    public GameObject signedInButtons;

    // Start is called before the first frame update
    void Start()
    {
        //Connects to the websocket using the socketIO plugin for unity
        DontDestroyOnLoad(this.gameObject);
        var uri = new Uri("https://cg.smach.us/");
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
                {
                    {"token", "UNITY" }
                }
            ,
            EIO = EngineIO.V4
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();

        ///// reserved socketio events
        socket.OnConnected += (sender, e) =>
        {
            Debug.Log("socket.OnConnected");
        };
        socket.OnPing += (sender, e) =>
        {
            //Debug.Log("Ping");
        };
        socket.OnPong += (sender, e) =>
        {
            //Debug.Log("Pong: " + e.TotalMilliseconds);
        };
        socket.OnDisconnected += (sender, e) =>
        {
            Debug.Log("disconnect: " + e);
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            Debug.Log($"{DateTime.Now} Reconnecting: attempt = {e}");
        };
        ////

        Debug.Log("Connecting...");
        socket.Connect();

        socket.OnAnyInUnityThread((name, response) =>
        {
            
        });
    }

    public static bool IsJSON(string str)
    {
        if (string.IsNullOrWhiteSpace(str)) { return false; }
        str = str.Trim();
        if ((str.StartsWith("{") && str.EndsWith("}")) || //For object
            (str.StartsWith("[") && str.EndsWith("]"))) //For array
        {
            try
            {
                var obj = JToken.Parse(str);
                return true;
            }catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    //disconnects the socket on close
    async void OnApplicationQuit()
    {
        if (socket != null && socket.Connected) 
        {
            await socket.DisconnectAsync();
        }
        socket?.Dispose();
    }

    //test connection by sending a ping
    public void EmitTest()
    {
        Debug.Log("Sending ping");
        socket.Emit("ping",(Message)=>{Debug.Log("Received message " + Message);});
    }

    //fetches the games from the DB and calls parse games
    public void EmitFetchGames()
    {
        Debug.Log("Fetching Games...");
        socket.Emit("getAvailableGames",(Message) => 
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                ParseGames(Message.ToString());
            });
        });
    }

    //parses the games into button objects that will load the game scene on press and call the start game function sending the game data
    public void ParseGames(string gameList)
    {
        Debug.Log(gameList);
        bC.updateGameList(gameList);
    }

    //fetches the block JSON from the server and calls parseblocks
    public void EmitFetchBlocks()
    {
        Debug.Log("Fetching Blocks...");
        socket.Emit("getAvailableBlocks",Message=>{ParseBlocks(Message.ToString());});
    }

    //parses the blocks into block objects that will be loaded in the editior scene maybe save the last loaded blocks to a local state for future use?
    public void ParseBlocks(string blockList)
    {
        Debug.Log(blockList);
    }

    //Emits the stat came to the server and once the started ping is recieved load the game scene with the game info
    public void EmitStartGame(int id)
    {
        Debug.Log("Starting Game...");
        socket.Emit("startNewGame",(Callback)=>{Debug.Log(Callback);}, id);
    }

    //sends a click event to the server
    public void EmitPlayerClickEvent()
    {
        Debug.Log("Clicked!");
        socket.Emit("playerClickEvent",(Callback)=>{Debug.Log("Click Recieved");});
    }

    public void EmitClientRequestSignUp()
    {
        string username = SUusername.text.ToLower();
        string password = SUpassword.text;
        string displayName = SUdisplayName.text;
        Debug.Log(username + " " + password + " " + displayName);
        if(username.Length < 3)
        {
            ErrorSignUp.text = "Username is too short";
            Debug.Log("error");
            return;
        }
        if(password.Length < 4)
        {
            ErrorSignUp.text = "Password is too short";
            return;
        }
        if(displayName.Length < 3)
        {
            ErrorSignUp.text = "Display Name is too short";
            return;
        }
        Debug.Log("Attempting Sign Up...");
        socket.Emit("signUp",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if(Callback.ToString() == "[null]") 
                {
                    ErrorSignUp.text = "Error";
                } 
                else 
                {
                    signUpPanel.SetActive(false); 
                    loginSuccess(displayName);
                }
            });
        },username,password,displayName);
        //signUpPanel.SetActive(false); loginSuccess(displayName);
    }

    public void EmitClientLoginRequest()
    {
        string username = LIusername.text;
        string password = LIpassword.text;
        if(username.Length < 3)
        {
            ErrorLogIn.text = "Username is too short";
            return;
        }
        if(password.Length < 4)
        {
            ErrorLogIn.text = "Password is too short";
            return;
        }
        Debug.Log("Attempting Log In...");
        socket.Emit("signIn",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if(Callback.ToString() == "[null]") 
                {
                    ErrorLogIn.text = "Error";
                } 
                else 
                {
                    logInPanel.SetActive(false); 
                    Debug.Log(Callback.GetValue<string>(1));
                    loginSuccess(Callback.GetValue<string>(1));
                }
            });
        },username,password);
        //logInPanel.SetActive(false); loginSuccess(username);
    }

    public void EmitClientSignOut()
    {
        socket.Emit("signOut",(Callback)=>{Debug.Log("Signed Out");});
        userText.text = "Not Signed In";
        authButtons.SetActive(true);
        signedInButtons.SetActive(false);
    }

    public void loginSuccess(string displayName)
    {
        userText.text = "Signed in as " + displayName;
        authButtons.SetActive(false);
        signedInButtons.SetActive(true);
        Debug.Log("Welcome User " + displayName);
    }
}