using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class variableController : MonoBehaviour
{
    [Header("Variables")]
    public TMP_Dropdown variablesD;
    public TMP_InputField variableName;
    public List<string> variablesList;
    public int currentVariable;

    [Header("Piles")]
    public TMP_Dropdown pilesD;
    public TMP_InputField pileName;
    public TMP_Dropdown pileType;
    public List<pilesType> pilesList;
    public int currentPile;

    [Header("Buttons")]
    public TMP_Dropdown buttonsD;
    public TMP_InputField buttonName;
    public TMP_Dropdown buttonType;
    public List<buttonsType> buttonsList;
    public int currentButton;

    [Header("Counters")]
    public TMP_Dropdown countersD;
    public TMP_InputField counterName;
    public TMP_Dropdown counterOption;
    public TMP_InputField counterOptionIN;
    public List<countersType> countersList;
    public int currentCounter;

    [Header("Locations")]
    public TMP_Dropdown locationsD;
    public TMP_InputField locationName;
    public TMP_InputField locationX;
    public TMP_InputField locationY;
    public List<locationsType> locationsList;
    public int currentLocation;

    [Header("Player Roles")]
    public TMP_Dropdown pRolesD;
    public TMP_InputField pRoleName;
    public List<string> pRolesList;
    public int currentPRole;

    [Header("Action Roles")]
    public TMP_Dropdown aRolesD;
    public TMP_InputField aRoleName;
    public List<string> aRolesList;
    public int currentARole;

    void Start()
    {
        variablesList = new List<string>();
        pilesList = new List<pilesType>();
        buttonsList = new List<buttonsType>();
        countersList = new List<countersType>();
        locationsList = new List<locationsType>();
        pRolesList = new List<string>();
        aRolesList = new List<string>();
    }

//  VARIABLES =========================
    public void newVariable()
    {
        
    }

    public void deleteVariable()
    {
        
    }

    public void modifyVariable()
    {
        
    }

    public void loadVariable()
    {
        
    }


//  PILES =========================
    public void newPile()
    {
        
    }

    public void deletePile()
    {
        
    }

    public void modifyPile()
    {
        
    }

    public void loadPile()
    {
        
    }


//  BUTTONS =========================
    public void newButton()
    {
        
    }

    public void deleteButton()
    {
        
    }

    public void modifyButton()
    {
        
    }

    public void loadButton()
    {
        
    }


//  COUNTERS =========================
    public void newCounter()
    {
        
    }

    public void deleteCounter()
    {
        
    }

    public void modifyCounter()
    {
        
    }

    public void loadCounter()
    {
        
    }


//  LOCATIONS =========================
    public void newLocation()
    {
        
    }

    public void deleteLocation()
    {
        
    }

    public void modifyLocation()
    {
        
    }

    public void loadLocation()
    {
        
    }


//  PLAYER ROLES =========================
    public void newPlayerRole()
    {
        
    }

    public void deletePlayerRole()
    {
        
    }

    public void modifyPlayerRole()
    {
        
    }

    public void loadPlayerRole()
    {
        
    }


//  ACTION ROLES =========================
    public void newActionRole()
    {
        
    }

    public void deleteActionRole()
    {
        
    }

    public void modifyActionRole()
    {
        
    }

    public void loadActionRole()
    {
        
    }
}
