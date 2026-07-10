using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class variableController : MonoBehaviour
{
    public editorBlockManager bM;
    public List<string> masterList;
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
    public List<string> pileNames;
    public int currentPile;

    [Header("Buttons")]
    public TMP_Dropdown buttonsD;
    public TMP_InputField buttonName;
    public TMP_Dropdown buttonType;
    public List<buttonsType> buttonsList;
    public List<string> buttonNames;
    public int currentButton;

    [Header("Counters")]
    public TMP_Dropdown countersD;
    public TMP_InputField counterName;
    public TMP_Dropdown counterOption;
    public TMP_InputField counterOptionIN;
    public List<countersType> countersList;
    public List<string> counterNames;
    public int currentCounter;
    public int currentMMVal;

    [Header("Locations")]
    public TMP_Dropdown locationsD;
    public TMP_InputField locationName;
    public TMP_InputField locationX;
    public TMP_InputField locationY;
    public TMP_InputField locationXOff;
    public TMP_InputField locationYOff;
    public TMP_InputField locationWrapAt;
    public TMP_InputField locationWrapTo;
    public TMP_Dropdown locationOption;
    public List<locationsType> locationsList;
    public List<string> locationNames;
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
        pileNames = new List<string>();
        buttonsList = new List<buttonsType>();
        buttonNames = new List<string>();
        countersList = new List<countersType>();
        counterNames = new List<string>();
        locationsList = new List<locationsType>();
        locationNames = new List<string>();
        pRolesList = new List<string>();
        aRolesList = new List<string>();
    }

    public void throwError(string er)
    {
        Debug.Log(er);
    }

    public void saveVariables()
    {
        masterList = new List<string>();
        masterList.Clear();
        masterList.AddRange(variablesList);
        masterList.AddRange(pileNames);
        masterList.AddRange(buttonNames);
        masterList.AddRange(counterNames);
        masterList.AddRange(locationNames);
        masterList.AddRange(aRolesList);
        masterList.AddRange(pRolesList);
        for(int x = 0; x < masterList.Count; x++)
        {
            if(masterList[x] == "" || masterList[x] == null)
            {
                throwError("A variable is undeclared or unnamed");
                return;
            }
            for(int y = 0; y < masterList.Count; y++)
            {
                if(y != x && masterList[x] == masterList[y])
                {
                    throwError("Two variables share the same name: " + masterList[x]);
                    return;
                }
            }
        }
        Debug.Log("variable name check passed");
        foreach(countersType x in countersList)
        {
            if(x.range.min > x.range.max)
            {
                throwError("Counter: " + x.cName + " has an invalid range");
                return;
            }
        }
        Debug.Log("counter check passed");
        bM.drawMetaBlocks(variablesList, pileNames, buttonNames, counterNames, locationNames, aRolesList, pRolesList);
    }

//  VARIABLES =========================
    public void newVariable()
    {
        variablesList.Add("NewVariable");
        variablesD.ClearOptions();
        variablesD.AddOptions(variablesList);
        currentVariable = variablesList.Count-1;
        variablesD.value = currentVariable;
        loadVariable();
    }

    public void deleteVariable()
    {

        if(variablesList.Count > 0)
        {
            variablesList.RemoveAt(currentVariable);
            variablesD.ClearOptions();
            variablesD.AddOptions(variablesList);
            if(variablesList.Count > 0)
            {
                variablesD.value = currentVariable;
            }
            else if(variablesList.Count+2 == currentVariable)
            {
                currentVariable = variablesList.Count-1;
                variablesD.value = currentVariable;
            }
            loadVariable();
        }
    }

    public void modifyVariable()
    {
        if(variablesList.Count > 0)
        {
            variablesList[currentVariable] = variableName.text;
            variablesD.ClearOptions();
            variablesD.AddOptions(variablesList);
            variablesD.value = currentVariable;
            loadVariable();
        }
    }

    public void loadVariable()
    {
        //DO NOT SET VARIABLESD.VALUE IN THIS FUNCTION
        if(variablesList.Count > 0)
        {
            currentVariable = variablesD.value;
            variableName.text = variablesList[currentVariable];
        }
        else
        {
            currentVariable = 0;
            variableName.text = "";
        }
    }


//  PILES =========================
    public void newPile()
    {
        pileNames.Add("NewPile");
        pilesList.Add(new pilesType());
        pilesD.ClearOptions();
        pilesD.AddOptions(pileNames);
        currentPile = pilesList.Count-1;
        pilesD.value = currentPile;
        loadPile();
    }

    public void deletePile()
    {
        if(pilesList.Count > 0)
        {
            pilesList.RemoveAt(currentPile);
            pileNames.RemoveAt(currentPile);
            pilesD.ClearOptions();
            pilesD.AddOptions(pileNames);
            if(pilesList.Count > 0)
            {
                pilesD.value = currentPile;
            }
            else if(pilesList.Count+2 == currentPile)
            {
                currentPile = pilesList.Count-1;
                pilesD.value = currentPile;
            }
            loadPile();
        }
    }

    public void modifyPile()
    {
        if(pilesList.Count > 0)
        {
            pileNames[currentPile] = pileName.text;
            pilesList[currentPile].type = pileType.value;
            pilesD.ClearOptions();
            pilesD.AddOptions(pileNames);
            pilesD.value = currentPile;
            loadPile();
        }
    }

    public void loadPile()
    {
        //DO NOT SET PILESD.VALUE IN THIS FUNCTION
        if(pilesList.Count > 0)
        {
            currentPile = pilesD.value;
            pileName.text = pileNames[currentPile];
            pileType.value = pilesList[currentPile].type;
        }
        else
        {
            currentPile = 0;
            pileName.text = "";
            pileType.value = 0;
        }
    }


//  BUTTONS =========================
    public void newButton()
    {
        Debug.Log("Adding Button");
        buttonNames.Add("NewButton");
        buttonsList.Add(new buttonsType());
        buttonsD.ClearOptions();
        buttonsD.AddOptions(buttonNames);
        currentButton = buttonsList.Count-1;
        buttonsD.value = currentButton;
        Debug.Log("Button Added");
        loadButton();
    }

    public void deleteButton()
    {
        if(buttonsList.Count > 0)
        {
            buttonsList.RemoveAt(currentButton);
            buttonNames.RemoveAt(currentButton);
            buttonsD.ClearOptions();
            buttonsD.AddOptions(buttonNames);
            if(buttonsList.Count > 0)
            {
                buttonsD.value = currentButton;
            }
            else if(buttonsList.Count+2 == currentButton)
            {
                currentButton = buttonsList.Count-1;
                buttonsD.value = currentButton;
            }
            loadButton();
        }
    }

    public void modifyButton()
    {
        if(buttonsList.Count > 0)
        {
            buttonNames[currentButton] = buttonName.text;
            buttonsList[currentButton].type = buttonType.value;
            buttonsD.ClearOptions();
            buttonsD.AddOptions(buttonNames);
            buttonsD.value = currentButton;
            loadButton();
        }
    }

    public void loadButton()
    {
        if(buttonsList.Count > 0)
        {
            currentButton = buttonsD.value;
            buttonName.text = buttonNames[currentButton];
            buttonType.value = buttonsList[currentButton].type;
        }
        else
        {
            currentButton = 0;
            buttonName.text = "";
            buttonType.value = 0;
        }
    }


//  COUNTERS =========================
    public void newCounter()
    {
        counterNames.Add("NewCounter");
        countersList.Add(new countersType());
        countersD.ClearOptions();
        countersD.AddOptions(counterNames);
        currentCounter = countersList.Count-1;
        countersD.value = currentCounter;
        counterOption.value = 0;
        loadCounter();
    }

    public void deleteCounter()
    {
        if(countersList.Count > 0)
        {
            countersList.RemoveAt(currentCounter);
            counterNames.RemoveAt(currentCounter);
            countersD.ClearOptions();
            countersD.AddOptions(counterNames);
            if(countersList.Count > 0)
            {
                countersD.value = currentCounter;
            }
            else if(countersList.Count+2 == currentCounter)
            {
                currentCounter = countersList.Count-1;
                countersD.value = currentCounter;
            }
            loadCounter();
        }
    }

    public void modifyCounter()
    {
        if(countersList.Count > 0)
        {
            float outVal;
            counterNames[currentCounter] = counterName.text;
            if(counterOption.value == 0)
            {
                if(float.TryParse(counterOptionIN.text, out outVal))
                {
                    countersList[currentCounter].range.min = outVal;
                }
                else
                {
                    countersList[currentCounter].range.min = float.NaN;
                }
            }
            else if(counterOption.value == 1)
            {
                if(float.TryParse(counterOptionIN.text,out outVal))
                {
                    countersList[currentCounter].range.max = outVal;
                }
                else
                {
                    countersList[currentCounter].range.max = float.NaN;
                }
            }
            else if(counterOption.value == 2)
            {
                float.TryParse(counterOptionIN.text, out outVal);
                countersList[currentCounter].range.increment = outVal;
            }
            countersD.ClearOptions();
            countersD.AddOptions(counterNames);
            countersD.value = currentCounter;
            loadCounter();
        }
    }

    public void loadCounter()
    {
        //DO NOT SET COUNTERSD.VALUE IN THIS FUNCTION
        if(countersList.Count > 0)
        {
            currentCounter = countersD.value;
            loadMinMaxCounter();
            counterName.text = counterNames[currentCounter];
        }
        else
        {
            currentCounter = 0;
            counterName.text = "";
            counterOption.value = 0;
            counterOptionIN.text = "";
        }
    }

    public void loadMinMaxCounter()
    {
        if(countersList.Count > 0)
        {
            currentMMVal = counterOption.value;
            if(currentMMVal == 0)
            {
                counterOptionIN.text = countersList[currentCounter].range.min.ToString();
            }
            else if(currentMMVal == 1)
            {
            counterOptionIN.text = countersList[currentCounter].range.max.ToString();
            }
            else if(currentMMVal == 2)
            {
                counterOptionIN.text = countersList[currentCounter].range.increment.ToString();
            }
        }
    }


//  LOCATIONS =========================
    public void newLocation()
    {
        locationNames.Add("NewLocation");
        locationsList.Add(new locationsType());
        locationsD.ClearOptions();
        locationsD.AddOptions(locationNames);
        currentLocation = locationsList.Count-1;
        locationsD.value = currentLocation;
        loadLocation();
    }

    public void deleteLocation()
    {
        if(locationsList.Count > 0)
        {
            locationsList.RemoveAt(currentLocation);
            locationNames.RemoveAt(currentLocation);
            locationsD.ClearOptions();
            locationsD.AddOptions(locationNames);
            if(locationsList.Count > 0)
            {
                locationsD.value = currentLocation;
            }
            else if(locationsList.Count+2 == currentLocation)
            {
                currentLocation = locationsList.Count-1;
                locationsD.value = currentLocation;
            }
            loadLocation();
        }
    }

    public void modifyLocation()
    {
        if(locationsList.Count > 0)
        {
            float outVal;
            locationNames[currentLocation] = locationName.text;
            float.TryParse(locationX.text,out outVal);
            locationsList[currentLocation].x = outVal;
            float.TryParse(locationY.text,out outVal);
            locationsList[currentLocation].y = outVal;
            float.TryParse(locationXOff.text,out outVal);
            locationsList[currentLocation].xOff = outVal;
            float.TryParse(locationYOff.text,out outVal);
            locationsList[currentLocation].yOff = outVal;
            float.TryParse(locationWrapAt.text,out outVal);
            locationsList[currentLocation].wrapAt = outVal;
            float.TryParse(locationWrapTo.text,out outVal);
            locationsList[currentLocation].wrapTo = outVal;
            if(locationOption.value == 0)
            {
                locationsList[currentLocation].vertHori = locationRenderType.HORIZONTAL;
            }
            if(locationOption.value == 1)
            {
                locationsList[currentLocation].vertHori = locationRenderType.VERTICAL;
            }
            locationsD.ClearOptions();
            locationsD.AddOptions(locationNames);
            locationsD.value = currentLocation;
            loadLocation();
        }
    }

    public void loadLocation()
    {
        if(locationsList.Count > 0)
        {
            currentLocation = locationsD.value;
            locationName.text = locationNames[currentLocation];
            locationX.text = locationsList[currentLocation].x.ToString();
            locationY.text = locationsList[currentLocation].y.ToString();
            locationXOff.text = locationsList[currentLocation].xOff.ToString();
            locationYOff.text = locationsList[currentLocation].yOff.ToString();
            locationWrapAt.text = locationsList[currentLocation].wrapAt.ToString();
            locationWrapTo.text = locationsList[currentLocation].wrapTo.ToString();
            if(locationsList[currentLocation].vertHori == locationRenderType.HORIZONTAL)
            {
                locationOption.value = 0;
            }
            if(locationsList[currentLocation].vertHori == locationRenderType.VERTICAL)
            {
                locationOption.value = 1;
            }
        }
        else
        {
            currentLocation = 0;
            locationName.text = "";
            locationX.text = "";
            locationY.text = "";
            locationXOff.text = "";
            locationYOff.text = "";
            locationWrapAt.text = "";
            locationWrapTo.text = "";
            locationOption.value = 0;
        }
    }


//  PLAYER ROLES =========================
    public void newPlayerRole()
    {
        pRolesList.Add("NewRole");
        pRolesD.ClearOptions();
        pRolesD.AddOptions(pRolesList);
        currentPRole = pRolesList.Count-1;
        pRolesD.value = currentPRole;
        loadPlayerRole();
    }

    public void deletePlayerRole()
    {
        if(pRolesList.Count > 0)
        {
            pRolesList.RemoveAt(currentPRole);
            pRolesD.ClearOptions();
            pRolesD.AddOptions(pRolesList);
            if(pRolesList.Count > 0)
            {
                pRolesD.value = currentPRole;
            }
            else if(pRolesList.Count+2 == currentPRole)
            {
                currentPRole = pRolesList.Count-1;
                pRolesD.value = currentPRole;
            }
            loadPlayerRole();
        }
    }

    public void modifyPlayerRole()
    {
        if(pRolesList.Count > 0)
        {
            pRolesList[currentPRole] = pRoleName.text;
            pRolesD.ClearOptions();
            pRolesD.AddOptions(pRolesList);
            pRolesD.value = currentPRole;
            loadPlayerRole();
        }
    }

    public void loadPlayerRole()
    {
        if(pRolesList.Count > 0)
        {
            currentPRole = pRolesD.value;
            pRoleName.text = pRolesList[currentPRole];
        }
        else
        {
            currentPRole = 0;
            pRoleName.text = "";
        }
    }


//  ACTION ROLES =========================
    public void newActionRole()
    {
        aRolesList.Add("NewRole");
        aRolesD.ClearOptions();
        aRolesD.AddOptions(aRolesList);
        currentARole = aRolesList.Count-1;
        aRolesD.value = currentARole;
        loadActionRole();
    }

    public void deleteActionRole()
    {
        if(aRolesList.Count > 0)
        {
            aRolesList.RemoveAt(currentARole);
            aRolesD.ClearOptions();
            aRolesD.AddOptions(aRolesList);
            if(aRolesList.Count > 0)
            {
                aRolesD.value = currentARole;
            }
            else if(aRolesList.Count+2 == currentARole)
            {
                currentARole = aRolesList.Count-1;
                aRolesD.value = currentARole;
            }
            loadActionRole();
        }
    }

    public void modifyActionRole()
    {
        if(aRolesList.Count > 0)
        {
            aRolesList[currentARole] = aRoleName.text;
            aRolesD.ClearOptions();
            aRolesD.AddOptions(aRolesList);
            aRolesD.value = currentARole;
            loadActionRole();
        }
    }

    public void loadActionRole()
    {
        if(aRolesList.Count > 0)
        {
            currentARole = aRolesD.value;
            aRoleName.text = aRolesList[currentARole];
        }
        else
        {
            currentARole = 0;
            aRoleName.text = "";
        }
    }
}
