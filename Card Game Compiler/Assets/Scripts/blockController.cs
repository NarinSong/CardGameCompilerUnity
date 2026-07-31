using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class blockController : MonoBehaviour
{
    public string bname;
    public string returnType;
    public args[] argumentsList;
    public TMP_Text display;
    public TMP_InputField text;
    public string litVal;
    public void Init(string n, string dN, string rT, args[] arg)
    {
        bname = n;
        returnType = rT;
        argumentsList = arg;
        display.text = dN;
        if(n == "LITERAL")
        {
            argumentsList = new args[]{new args("primary")};
        }
    }

    public void updateLitVal()
    {
        litVal = text.text;
    }
}
