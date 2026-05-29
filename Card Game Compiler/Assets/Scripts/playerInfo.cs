using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class playerInfo : MonoBehaviour
{
    public string displayName;
    public string username;
    public bool isHost;
    public bool Host;
    public GameObject b;
    public TMP_Text userText;
    public websocketController websocket;
    public GameObject background;

    public void Init(string dn, string un, bool iH, bool h,string colorN)
    {
        displayName = dn;
        username = un;
        Host = h;
        userText.text = displayName;
        isHost = iH;
        Image x = background.GetComponent<Image>();
        float[] colorA = hexToRGB(colorN);
        x.color = new Color(colorA[0],colorA[1],colorA[2]);
        websocket = GameObject.Find("Websocketer").GetComponent<websocketController>();
        if(isHost == true)
        {
            b.SetActive(false);
        }
        if(Host == false)
        {
            b.SetActive(false);
        }
    }

    public void setHost()
    {
        if(isHost == true)
        {
            b.SetActive(false);
        }
        else
        {
            b.SetActive(true);
        }
    }

    public void clickEvent()
    {
        websocket.removeFromLobby(username);
    }

    public float[] hexToRGB(string hex)
    {
        hex = hex.Substring(1);
        int rVal = Convert.ToInt32(hex.Substring(0,2),16);
        int gVal = Convert.ToInt32(hex.Substring(2,2),16);
        int bVal = Convert.ToInt32(hex.Substring(4,2),16);
        return new float[] {rVal/255f,gVal/255f,bVal/255f};
    }
}
