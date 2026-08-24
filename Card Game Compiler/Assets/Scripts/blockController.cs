using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class blockController : MonoBehaviour
{
    public string bname;
    public string dname;
    public string returnType;
    [SerializeField]
    public List<args> argumentsList;
    public TMP_Text display;
    public TMP_InputField text;
    public TMP_Dropdown litDrop;
    public TMP_Dropdown litOption;
    public TMP_Dropdown litOption2;
    public GameObject litOptionO;
    public GameObject litOption2O;
    public GameObject selectorO;
    public GameObject textO;
    public string litValS;
    public float litValN;
    public Boolean litValB;
    public void Init(string n, string dN, string rT, args[] arg)
    {
        bname = n;
        dname = dN;
        returnType = rT;
        if(arg != null)
        {
            argumentsList = arg.ToList<args>();
        }
        else
        {
            argumentsList = null;
        }
        if(returnType != "Number")
        {
            litValS = dN;
        }
        display.text = dN;
    }

    public void updateVariableType()
    {
        returnType = getReturnType(litDrop.value);
    }

    public void updateType()
    {
        if(litDrop.value == 0)
        {
            returnType = "Number";
            textO.SetActive(true);
            litOptionO.SetActive(false);
            litOption2O.SetActive(false);
            text.text = "0";
            updateLitVal();
        }
        else if(litDrop.value == 1)
        {
            returnType = "Boolean";
            textO.SetActive(false);
            litOptionO.SetActive(true);
            litOption2O.SetActive(false);
            litOption.ClearOptions();
            litOption.AddOptions(new List<string> {"True", "False"});
            updateLitVal();
        }
        else if(litDrop.value == 2)
        {
            returnType = "String";
            textO.SetActive(true);
            litOptionO.SetActive(false);
            litOption2O.SetActive(false);
            text.text = "";
            updateLitVal();
        }
        else if(litDrop.value == 3)
        {
            returnType = "Visibility";
            textO.SetActive(false);
            litOptionO.SetActive(true);
            litOption2O.SetActive(false);
            litOption.ClearOptions();
            litOption.AddOptions(new List<string> {"Face Up", "Face Down", "Invisible", "Face Up Spread", "Face Down Spread", "Private", "Private Spread"});
            updateLitVal();
        }
        else if(litDrop.value == 4)
        {
            returnType = "Pilestate";
            textO.SetActive(false);
            litOptionO.SetActive(true);
            litOption2O.SetActive(false);
            litOption.ClearOptions();
            litOption.AddOptions(new List<string> {"Shuffled", "Empty"});
            updateLitVal();
        }
        else if(litDrop.value == 5)
        {
            returnType = "Rank";
            textO.SetActive(false);
            litOptionO.SetActive(true);
            litOption2O.SetActive(false);
            litOption.ClearOptions();
            litOption.AddOptions(new List<string> {"Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King"});
            updateLitVal();
        }
        else if(litDrop.value == 6)
        {
            returnType = "Suit";
            textO.SetActive(false);
            litOptionO.SetActive(true);
            litOption2O.SetActive(false);
            litOption.ClearOptions();
            litOption.AddOptions(new List<string> {"Hearts", "Spades", "Diamonds", "Clubs"});
            updateLitVal();
        }
        else if(litDrop.value == 7)
        {
            returnType = "Card";
            textO.SetActive(false);
            litOptionO.SetActive(true);
            litOption2O.SetActive(true);
            litOption.ClearOptions();
            litOption.AddOptions(new List<string> {"Ace", "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King"});
            updateLitVal();
        }
    }

    public void updateLitVal()
    {
        if(returnType == "Number")
        {
            litValN = float.Parse(text.text);
        }
        else if(returnType == "Boolean")
        {
            if(litOption.value == 0)
            {
                litValB = true;
            }
            else if(litOption.value == 1)
            {
                litValB = false;
            }
        }
        else if(returnType == "String")
        {
            litValS = text.text;
        }
        else if(returnType == "Visibility")
        {
            if(litOption.value == 0)
            {
                litValS = "FACE_UP";
            }
            else if(litOption.value == 1)
            {
                litValS = "FACE_DOWN";
            }
            else if(litOption.value == 2)
            {
                litValS = "INVISIBLE";
            }
            else if(litOption.value == 3)
            {
                litValS = "FACE_UP_SPREAD";
            }
            else if(litOption.value == 4)
            {
                litValS = "FACE_DOWN_SPREAD";
            }
            else if(litOption.value == 5)
            {
                litValS = "PRIVATE";
            }
            else if(litOption.value == 6)
            {
                litValS = "PRIVATE_SPREAD";
            }
        }
        else if(returnType == "Pilestate")
        {
            if(litOption.value == 0)
            {
                litValS = "SHUFFLED";
            }
            else if(litOption.value == 1)
            {
                litValS = "EMPTY";
            }
        }
        else if(returnType == "Rank")
        {
            if(litOption.value == 0)
            {
                litValS = "Ace";
            }
            else if(litOption.value == 1)
            {
                litValS = "Two";
            }
            else if(litOption.value == 2)
            {
                litValS = "Three";
            }
            else if(litOption.value == 3)
            {
                litValS = "Four";
            }
            else if(litOption.value == 4)
            {
                litValS = "Five";
            }
            else if(litOption.value == 5)
            {
                litValS = "Six";
            }
            else if(litOption.value == 6)
            {
                litValS = "Seven";
            }
            else if(litOption.value == 7)
            {
                litValS = "Eight";
            }
            else if(litOption.value == 8)
            {
                litValS = "Nine";
            }
            else if(litOption.value == 9)
            {
                litValS = "Ten";
            }
            else if(litOption.value == 10)
            {
                litValS = "Jack";
            }
            else if(litOption.value == 11)
            {
                litValS = "Queen";
            }
            else if(litOption.value == 12)
            {
                litValS = "King";
            }
        }
        else if(returnType == "Suit")
        {
            if(litOption.value == 0)
            {
                litValS = "Heart";
            }
            else if(litOption.value == 1)
            {
                litValS = "Spade";
            }
            else if(litOption.value == 2)
            {
                litValS = "Diamond";
            }
            else if(litOption.value == 3)
            {
                litValS = "Club";
            }
        }
    }

    public string getReturnType(int x)
    {
        switch(x)
        {
            case 0:
                return "Number";
            case 1:
                return "String";
            case 2:
                return "Boolean";
            case 3:
                return "PileLabel";
            case 4:
                return "CounterLabel";
            case 5:
                return "ButtonLabel";
            case 6:
                return "ActionRole";
            case 7:
                return "PileState";
            case 8:
                return "Visibility";
            case 9:
                return "Card";
            case 10:
                return "ID";
            case 11:
                return "Player";
            case 12:
                return "PlayerRole";
            case 13:
                return "Phase";
            case 14:
                return "Step";
            case 15:
                return "Location";
            case 16:
                return "ButtonRange";
            case 17:
                return "Rank";
            case 18:
                return "Suit";
            case 19:
                return "Array";

        }
        return "null";
    }
}
