using System;
using System.Collections.Generic;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine.UI;
using System.Text.Json;

public class websocketController : MonoBehaviour
{
    public string username;
    public string displayName;
    public SocketIOUnity socket;
    public buttonCreator bC;
    public gamestateController gsC; 
    public lobbyController lC;
    public PageManager PM;
    public editorBlockManager eBM;
    public TabNavigation tN;
    public TMP_Text userText;
    public TMP_Text ErrorSignUp;
    public TMP_Text ErrorLogIn;
    public TMP_Text errorTechMessage;
    public TMP_Text lobbyCode;
    public TMP_Text lobbyGameText;
    public TMP_Text lobbyGameInfo;
    public TMP_Text connInfo;
    public TMP_InputField SUusername;
    public TMP_InputField SUpassword;
    public TMP_InputField SUdisplayName;
    public TMP_InputField LIusername;
    public TMP_InputField LIpassword;
    public TMP_InputField LobbyCodeRequest;
    public TMP_InputField changeDisplayName;
    public GameObject signUpPanel;
    public GameObject logInPanel;
    public GameObject authButtons;
    public GameObject signedInButtons;
    public GameObject errorBoard;
    public GameObject lobbyPanel;
    public GameObject joinPanel;
    public GameObject gameSelector;
    public GameObject menuButton;
    public GameObject userSettingsPanel;
    public Slider rSlider;
    public Slider gSlider;
    public Slider bSlider;
    public colorPicker colorBlock;
    public List<block> blocks;
    public int selectedGameID;
    private int reconTries;
    public GameObject popupPane;

    // Start is called before the first frame update
    void Start()
    {
        //Connects to the websocket using the socketIO plugin for unity
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
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log("socket.OnConnected");
                connInfo.text = "Connected to Server";
            });
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
            connInfo.text = "Disconnected from Server";
        };
        socket.OnReconnectAttempt += (sender, e) =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log($"{DateTime.Now} Reconnecting: attempt = {e}");
                if(reconTries == 3)
                {
                    connInfo.text = "Attempting Reconnect";
                    reconTries = 0;
                }
                if(reconTries == 2)
                {
                    connInfo.text = "Attempting Reconnect . . .";
                    reconTries ++;
                }
                if(reconTries == 1)
                {
                    connInfo.text = "Attempting Reconnect . .";
                    reconTries ++;
                }
                if(reconTries == 0)
                {
                    connInfo.text = "Attempting Reconnect .";
                    reconTries ++;
                }
            });
        };

        ////
        
        socket.On("gamestate", message =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                //Debug.Log(message);
                PM.setGame();
                menuButton.SetActive(true);
                tN.swapFields("game");
                gsC.updateGamestate(message);
            });
        });

        socket.On("lobbyClosed", message =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                lobbyPanel.SetActive(false);
                tN.swapFields("postAuth");
            });
        });

        socket.On("lobbyStatus", message =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {         
                Debug.Log(message);
                lC.updateLobbyInfo(message.GetValue<LobbyInfo>(0), username);
            });
        });

        socket.On("gameEnded", message =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {   
                popupPane.GetComponent<popupPane>().ResetPanel();   
                PM.setMain();
                tN.swapFields("lobbyVar");
            });
        });

        socket.On("popup", message =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {         
                popupPane.SetActive(true);
                popupPane.GetComponent<popupPane>().startPanel(message.GetValue<string>());
            });
        });

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
                bC.updateGameList(Message.ToString());
            });
        });
    }

    public void EmitFetchGameInfo(int id)
    {
        socket.Emit("getGameInfo",(Message) => 
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log(Message);
                gameInfo temp = Message.GetValue<gameInfo>(0);
                selectedGameID = id;
                tN.swapFields("lobbyVar");
                //lobbyGameText.text = temp.name;
                //lobbyGameInfo.text = temp.description;
            });
        },id);

        socket.Emit("selectGame",(Callback) =>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if(Callback.GetValue<bool>(0) == false)
                {
                    throwError("Select Game");
                }
            });
        },id);
    }

    //fetches the block JSON from the server and calls parseblocks
    public void EmitFetchBlocks()
    {
        Debug.Log("Fetching Blocks...");
        socket.Emit("getAvailableBlocks",Message=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log(Message);
                string blockList = Message.ToString();
                blockList = blockList[1..^1];
                blocks = JsonSerializer.Deserialize<List<block>>(blockList);
                eBM.setBlockList(blocks);
                eBM.drawBlocks();
                Debug.Log(blocks.Count);
            });
        });
    }

    //sends a click event to the server
    public void EmitPlayerClickEvent(int id, string label)
    {
        Debug.Log("Clicked!");
        socket.Emit("playerClickEvent",(Callback)=>{Debug.Log("Click Recieved");},label,id);
    }

    public void EmitClientRequestSignUp()
    {
        string usernameD = SUusername.text.ToLower();
        string password = SUpassword.text;
        string displayNameD = SUdisplayName.text;
        //Debug.Log(usernameD + " " + password + " " + displayNameD);
        if(usernameD.Length < 3)
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
        if(displayNameD.Length < 3)
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
                    loginSuccess(usernameD, displayNameD);
                }
            });
        },usernameD,password,displayNameD);
        //signUpPanel.SetActive(false); loginSuccess(displayName);
    }

    public void EmitClientLoginRequest()
    {
        string usernameD = LIusername.text;
        string password = LIpassword.text;
        if(usernameD.Length < 3)
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
                    loginSuccess(usernameD, Callback.GetValue<string>(1));
                }
            });
        },usernameD,password);
    }

    public void EmitClientSignOut()
    {
        socket.Emit("signOut",(Callback)=>{Debug.Log("Signed Out");});
        userText.text = "Not Signed In";
        authButtons.SetActive(true);
        signedInButtons.SetActive(false);
        tN.swapFields("preAuth");
    }

    public void loginSuccess(string usernameD, string displayNameD)
    {
        userText.text = "Signed in as " + displayNameD;
        authButtons.SetActive(false);
        signedInButtons.SetActive(true);
        username = usernameD;
        displayName = displayNameD;
        Debug.Log("Welcome User " + displayNameD);
        tN.swapFields("postAuth");
    }

    public void hostLobby()
    {
        socket.Emit("hostLobby",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if(Callback.ToString() == "[{}]")
                {
                    throwError("Host Lobby");
                }
                else
                {
                    tN.swapFields("lobbyHost");
                    string lobbyID = Callback.GetValue<string>(0);
                    Debug.Log(lobbyID);
                    lobbyPanel.SetActive(true);
                    lobbyCode.text = "Lobby Code - " + lobbyID;
                }
            });
        });
    }

    public void joinLobby()
    {
        string code = LobbyCodeRequest.text;
        if(code.Length != 6)
        {
            throwError("Invalid Lobby Code");
        }
        socket.Emit("joinLobby",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log(Callback);
                if(Callback.GetValue<bool>(0) == false)
                {
                    throwError("Join Lobby");
                }
                else
                {
                    joinPanel.SetActive(false);
                    lobbyPanel.SetActive(true);
                    tN.swapFields("lobbyUser");
                    lobbyCode.text = "Lobby Code - " + code;
                }
            });
        },code);
    }

    private void throwError(string callstack)
    {
        errorTechMessage.text = "Stack Trace - " + callstack; 
        errorBoard.SetActive(true);
        tN.swapFields("error");
        Debug.Log(callstack);
    }

    public void leaveLobby()
    {
        socket.Emit("leaveLobby",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if(Callback.GetValue<bool>(0) == false)
                {
                    throwError("Leave Lobby");
                }
                lobbyPanel.SetActive(false);
                tN.swapFields("postAuth");
            });
        });
    }

    public void removeFromLobby(string usernameX)
    {
        socket.Emit("removeFromLobby",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if(Callback.GetValue<bool>(0) == false)
                {
                    throwError("Remove From Lobby");
                }
                else
                {
                    
                }
            });
        },usernameX);
    }

    public void closeGameList()
    {
        gameSelector.SetActive(false);
        tN.swapFields("lobbyVar");
    }

    public void startGame()
    {
        Debug.Log("Starting Game");
        socket.Emit("startNewGame",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.Log(Callback);
                if(Callback.ToString() == "[false]")
                {
                    throwError("Start Game");
                }
                else
                {
                }
            });
        });
    }

    public void emitSetColor()
    {
        string color = rgbToHex(rSlider.value,gSlider.value,bSlider.value);
        Debug.Log("Changing Color to" + color);
        tN.swapFields("postAuth");
        socket.Emit("setColor",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                //CODE GOES HERE
            });
        },color);
    }

    public void emitChangeDisplayName()
    {
        string newName = changeDisplayName.text;
        if(newName.Length < 3)
        {
            ErrorSignUp.text = "Display Name is too short";
            return;
        }
        socket.Emit("setDisplayName",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                if(Callback.GetValue<bool>(0) == false)
                {
                    throwError("Change Display Name");
                    userSettingsPanel.SetActive(false);
                }
                else
                {
                    tN.swapFields("postAuth");
                    displayName = newName;
                    userText.text = "Signed in as " + newName;
                }
            });
        },newName);
    }

    public void emtiGetColor()
    {
        socket.Emit("getColor",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                float[] color = hexToRGB(Callback.GetValue<string>(0));
                Debug.Log("Converted " + Callback.GetValue<string>(0) + " to " + color[0] + "," + color[1] + "," + color[2]);
                rSlider.value = color[0];
                gSlider.value = color[1];
                bSlider.value = color[2];
                colorBlock.init(color[0],color[1],color[2]);
                changeDisplayName.text = displayName;
            });
        });
    }

    public void leaveGame()
    {
        socket.Emit("leaveGame",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                PM.setMain();
                tN.swapFields("lobbyVar");
            });
        });
    }

    public string rgbToHex(float r, float g, float b)
    {
        int rdec = (int)(r*255);
        string rHex = rdec.ToString("x2");
        int gdec = (int)(g*255);
        string gHex = gdec.ToString("x2");
        int bdec = (int)(b*255);
        string bHex = bdec.ToString("x2");
        return "#" + rHex + gHex + bHex;
    }

    public float[] hexToRGB(string hex)
    {
        hex = hex.Substring(1);
        int rVal = Convert.ToInt32(hex.Substring(0,2),16);
        int gVal = Convert.ToInt32(hex.Substring(2,2),16);
        int bVal = Convert.ToInt32(hex.Substring(4,2),16);
        return new float[] {rVal/255f,gVal/255f,bVal/255f};
    }

    /*
    public void socketTemplate()
    {
        socket.Emit("SIGNALNAME",(Callback)=>
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                //CODE GOES HERE
            });
        },EXTRA PARAMS);
    }
    */
}