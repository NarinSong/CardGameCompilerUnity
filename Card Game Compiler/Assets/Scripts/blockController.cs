using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class blockController : MonoBehaviour
{
    public string bname;
    public string returnType;
    public args[] argumentsList;
    public TMP_Text display;
    public void Init(string n, string dN, string rT, args[] arg)
    {
        bname = n;
        returnType = rT;
        argumentsList = arg;
        display.text = dN;
    }
}
