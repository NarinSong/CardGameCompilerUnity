using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class variableController : MonoBehaviour
{
    public editorBlockManager bM;
    public List<string> masterList;
    public bool inLoad = false;
    public float maxX;
    public float maxY;

    [Header("Variables")]
    public TMP_Dropdown variablesD;
    public TMP_InputField variableName;
    public List<variablesType> variablesList;
    public List<string> variableNames;
    public TMP_Dropdown variableType;
    public int currentVariable;

    [Header("Piles")]
    public TMP_Dropdown pilesD;
    public TMP_InputField pileName;
    public TMP_Dropdown pileType;
    public TMP_Dropdown pileVis;
    public TMP_Dropdown pileLoc;
    public TMP_Dropdown pileOwnerLoc;
    public TMP_Dropdown pileARole;
    public TMP_Dropdown pileOwner;
    public List<pilesType> pilesList;
    public List<string> pileNames;
    public int currentPile;

    [Header("Buttons")]
    public TMP_Dropdown buttonsD;
    public TMP_InputField buttonName;
    public TMP_Dropdown buttonType;
    public TMP_Dropdown buttonVis;
    public TMP_Dropdown buttonLoc;
    public TMP_Dropdown buttonOwnerLoc;
    public TMP_Dropdown buttonARole;
    public TMP_Dropdown buttonRange;
    public TMP_InputField rangeIn;
    public GameObject buttonRangeObj;
    public TMP_Dropdown buttonOwner;
    public List<buttonsType> buttonsList;
    public List<string> buttonNames;
    public int currentButton;

    [Header("Counters")]
    public TMP_Dropdown countersD;
    public TMP_InputField counterName;
    public TMP_InputField counterValue;
    public TMP_Dropdown counterOption;
    public TMP_Dropdown counterVis;
    public TMP_Dropdown counterLoc;
    public TMP_Dropdown counterOwnerLoc;
    public TMP_Dropdown counterARole;
    public TMP_Dropdown counterOwner;
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

    [Header("Maps")]
    public TMP_Dropdown mapsD;
    public TMP_InputField mapName;
    public TMP_Dropdown mapKeyDropdown;
    public TMP_InputField mapKey;
    public TMP_InputField mapValue;
    public int currentMap;
    public int currentMapKey;
    public List<mapsType> mapsList;
    public List<string> mapNames;
    public Button valueAdd;
    public Button valueDelete;

    void Start()
    {
        variableNames = new List<string>();
        variablesList = new List<variablesType>();
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
        mapsList = new List<mapsType>();
        mapNames = new List<string>();
        newLocation();
        locationName.text = "DEFAULT";
        locationsList[0].editable = false;
        modifyLocation();
    }

    public float clamp(float max, float min, float inp)
    {
        if(inp > max)
        {
            return max;
        }
        else if(inp < min)
        {
            return min;
        }
        return inp;
    }

    public int[] bitFieldToInt(int bitfield)
    {
        //Debug.Log(bitfield);
        BitArray bits = new BitArray(new int[] { bitfield });
        int[] result = new int[bits.Count];
        for (int i = 0; i < bits.Count; i++)
        {
            result[i] = bits[i] ? 1 : 0;
        }
        return result;
    }

    public int InttoBitfield(List<string> list)
    {
        BitArray bF = new BitArray(aRolesList.Count);
        for(int i = 0; i < aRolesList.Count; i ++)
        {
            for(int j = 0; j < list.Count; j++)
            {
                if(aRolesList[i] == list[j])
                {
                    bF.Set(i,true);
                }
            }
        }
        int[] x = new int[1];
        bF.CopyTo(x, 0);
        return x[0];
    }

    public void throwError(string er)
    {
        Debug.Log(er);
    }

    public void saveVariables()
    {
        masterList = new List<string>();
        masterList.Clear();
        masterList.AddRange(pileNames);
        masterList.AddRange(buttonNames);
        masterList.AddRange(counterNames);
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
        for(int x = 0; x < variableNames.Count; x++)
        {
            if(variableNames[x] == "" || variableNames[x] == null)
            {
                throwError("A variable is undeclared or unnamed");
                return;
            }
            for(int y = 0; y < variableNames.Count; y++)
            {
                if(y != x && variableNames[x] == variableNames[y])
                {
                    throwError("Two variables share the same name: " + variableNames[x]);
                    return;
                }
            }
        }
        for(int x = 0; x < locationNames.Count; x++)
        {
            if(locationNames[x] == "" || locationNames[x] == null)
            {
                throwError("A variable is undeclared or unnamed");
                return;
            }
            for(int y = 0; y < locationNames.Count; y++)
            {
                if(y != x && locationNames[x] == locationNames[y])
                {
                    throwError("Two variables share the same name: " + locationNames[x]);
                    return;
                }
            }
        }
        for(int x = 0; x < mapNames.Count; x++)
        {
            if(mapNames[x] == "" || mapNames[x] == null)
            {
                throwError("A variable is undeclared or unnamed");
                return;
            }
            for(int y = 0; y < mapNames.Count; y++)
            {
                if(y != x && mapNames[x] == mapNames[y])
                {
                    throwError("Two variables share the same name: " + mapNames[x]);
                    return;
                }
            }
        }

        Debug.Log("variable name check passed");
        masterList.AddRange(variableNames);
        masterList.AddRange(locationNames);
        masterList.AddRange(aRolesList);
        masterList.AddRange(pRolesList);
        foreach(buttonsType x in buttonsList)
        {
            if(x.range.min > x.range.max && x.type == ButtonType.NUMBER)
            {
                throwError("Counter: " + x.bName + " has an invalid range");
                return;
            }
        }
        Debug.Log("counter check passed");
        bM.drawMetaBlocks(variablesList, pilesList, buttonsList, countersList, locationsList, aRolesList, pRolesList,mapsList);
    }

//  VARIABLES =========================
    public void newVariable()
    {
        variablesList.Add(new variablesType());
        variableNames.Add("NewVariable");
        variablesD.ClearOptions();
        variablesD.AddOptions(variableNames);
        currentVariable = variablesList.Count-1;
        variablesD.value = currentVariable;
        loadVariable();
    }

    public void deleteVariable()
    {

        if(variablesList.Count > 0)
        {
            variablesList.RemoveAt(currentVariable);
            variableNames.RemoveAt(currentVariable);
            variablesD.ClearOptions();
            variablesD.AddOptions(variableNames);
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
        if(variablesList.Count > 0 && inLoad == false)
        {
            variableNames[currentVariable] = variableName.text;
            variablesList[currentVariable].vName = variableName.text;
            variablesList[currentVariable].type = variableType.value;
            variablesD.ClearOptions();
            variablesD.AddOptions(variableNames);
            variablesD.value = currentVariable;
            loadVariable();
        }
    }

    public void loadVariable()
    {
        //DO NOT SET VARIABLESD.VALUE IN THIS FUNCTION
        inLoad = true;
        if(variablesList.Count > 0)
        {
            variableName.interactable = true;
            variableType.interactable = true;
            currentVariable = variablesD.value;
            variableName.text = variableNames[currentVariable];
            variableType.value = variablesList[currentVariable].type;
        }
        else
        {
            variableName.interactable = false;
            variableType.interactable = false;
            currentVariable = 0;
            variableType.value = 0;
            variableName.text = "";
        }
        inLoad = false;
    }


//  PILES =========================
    public void newPile()
    {
        pileNames.Add("NewPile");
        pilesList.Add(new pilesType(locationsList[0]));
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
        Debug.Log("pile Modified!");
        if(pilesList.Count > 0 && inLoad == false)
        {
            pileNames[currentPile] = pileName.text;
            pilesList[currentPile].pName = pileName.text;
            if(locationsList.Count > 0)
            {
                pilesList[currentPile].location = locationsList[pileLoc.value];
                pilesList[currentPile].ownerLocation = locationsList[pileOwnerLoc.value];
            }
            if(pileType.value == 0)
            {
                pilesList[currentPile].pileState = pileState.SHUFFLED;
            }
            else if(pileType.value == 1)
            {
                pilesList[currentPile].pileState = pileState.EMPTY;
            }
            if(pileVis.value == 0)
            {
                pilesList[currentPile].visibility = vis.FACE_UP;
            }
            else if(pileVis.value == 1)
            {
                pilesList[currentPile].visibility = vis.FACE_DOWN;
            }
            else if(pileVis.value == 2)
            {
                pilesList[currentPile].visibility = vis.INVISIBLE;
            }
            else if(pileVis.value == 3)
            {
                pilesList[currentPile].visibility = vis.FACE_UP_SPREAD;
            }
            else if(pileVis.value == 4)
            {
                pilesList[currentPile].visibility = vis.FACE_DOWN_SPREAD;
            }
            else if(pileVis.value == 5)
            {
                pilesList[currentPile].visibility = vis.PRIVATE;
            }
            else if(pileVis.value == 6)
            {
                pilesList[currentPile].visibility = vis.PRIVATE_SPREAD;
            }
            if(pileOwner.value == 0)
            {
                pilesList[currentPile].ownership = ownership.BOARD;
                pilesList[currentPile].ownerLocation = locationsList[0];
            }
            else if(pileOwner.value == 1)
            {
                pilesList[currentPile].ownership = ownership.PLAYER;
            }
            int[] r = bitFieldToInt(pileARole.value);
            List<string> temp = new List<string>();
            for(int i = 0; i < aRolesList.Count; i++)
            {
                if(r[i] == 1)
                {
                    temp.Add(aRolesList[i]);
                }
            }
            pilesList[currentPile].actionRoles = temp;
            pilesD.ClearOptions();
            pilesD.AddOptions(pileNames);
            pilesD.value = currentPile;
            loadPile();
        }
    }

    public void loadPile()
    {
        //DO NOT SET PILESD.VALUE IN THIS FUNCTION
        inLoad = true;
        if(pilesList.Count > 0)
        {
            pileName.interactable = true;
            pileLoc.interactable = true;
            pileOwnerLoc.interactable = true;
            pileVis.interactable = true;
            pileOwner.interactable = true;
            pileType.interactable = true;
            pileARole.interactable = true;
            currentPile = pilesD.value;
            pileName.text = pileNames[currentPile];
            if(locationsList.Count > 0)
            {
                pileLoc.value = pilesList[currentPile].location.index;
                pileOwnerLoc.value = pilesList[currentPile].ownerLocation.index;
            }
            if(pilesList[currentPile].pileState == pileState.SHUFFLED)
            {
                pileType.value = 0;
            }
            else if(pilesList[currentPile].pileState == pileState.EMPTY)
            {
                pileType.value = 1;
            }
            if(pilesList[currentPile].visibility == vis.FACE_UP)
            {
                pileVis.value = 0;
            }
            else if(pilesList[currentPile].visibility == vis.FACE_DOWN)
            {
                pileVis.value = 1;
            }
            else if(pilesList[currentPile].visibility == vis.INVISIBLE)
            {
                pileVis.value = 2;
            }
            else if(pilesList[currentPile].visibility == vis.FACE_UP_SPREAD)
            {
                pileVis.value = 3;
            }
            else if(pilesList[currentPile].visibility == vis.FACE_DOWN_SPREAD)
            {
                pileVis.value = 4;
            }
            else if(pilesList[currentPile].visibility == vis.PRIVATE)
            {
                pileVis.value = 5;
            }
            else if(pilesList[currentPile].visibility == vis.PRIVATE_SPREAD)
            {
                pileVis.value = 6;
            }
            if(pilesList[currentPile].ownership == ownership.BOARD)
            {
                pileOwner.value = 0;
                pileOwnerLoc.interactable = false;
            }
            else if(pilesList[currentPile].ownership == ownership.PLAYER)
            {
                pileOwner.value = 1;
                pileOwnerLoc.interactable = true;
            }
            pileARole.value = InttoBitfield(pilesList[currentPile].actionRoles);
        }
        else
        {
            currentPile = 0;
            pileName.text = "";
            pileType.value = 0;
            pileLoc.value = 0;
            pileOwnerLoc.value = 0;
            pileVis.value = 0;
            pileARole.value = 0;
            pileName.interactable = false;
            pileLoc.interactable = false;
            pileOwnerLoc.interactable = false;
            pileVis.interactable = false;
            pileOwner.interactable = false;
            pileType.interactable = false;
            pileARole.interactable = false;
        }
        inLoad = false;
    }


//  BUTTONS =========================
    public void newButton()
    {
        buttonNames.Add("NewButton");
        buttonsList.Add(new buttonsType(locationsList[0]));
        buttonsD.ClearOptions();
        buttonsD.AddOptions(buttonNames);
        currentButton = buttonsList.Count-1;
        buttonsD.value = currentButton;
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
        if(buttonsList.Count > 0 && inLoad == false)
        {
            buttonNames[currentButton] = buttonName.text;
            buttonsList[currentButton].bName = buttonName.text;
            if(buttonType.value == 0)
            {
                buttonsList[currentButton].type = ButtonType.CLICK;
            }
            else
            {
                buttonsList[currentButton].type = ButtonType.NUMBER;
            }
            if(locationsList.Count > 0)
            {
                buttonsList[currentButton].location = locationsList[buttonLoc.value];
                buttonsList[currentButton].ownerLocation = locationsList[buttonOwnerLoc.value];
            }
            if(buttonVis.value == 0)
            {
                buttonsList[currentButton].visibility = vis.FACE_UP;
            }
            else if(buttonVis.value == 1)
            {
                buttonsList[currentButton].visibility = vis.FACE_DOWN;
            }
            else if(buttonVis.value == 2)
            {
                buttonsList[currentButton].visibility = vis.INVISIBLE;
            }
            else if(buttonVis.value == 3)
            {
                buttonsList[currentButton].visibility = vis.PRIVATE;
            }
            if(buttonOwner.value == 0)
            {
                buttonsList[currentButton].ownership = ownership.BOARD;
                buttonsList[currentButton].ownerLocation = locationsList[0];
            }
            else if(buttonOwner.value == 1)
            {
                buttonsList[currentButton].ownership = ownership.PLAYER;
            }
            int[] r = bitFieldToInt(buttonARole.value);
            List<string> temp = new List<string>();
            for(int i = 0; i < aRolesList.Count; i++)
            {
                if(r[i] == 1)
                {
                    temp.Add(aRolesList[i]);
                }
            }
            if(buttonType.value == 1)
            {
                float outVal;
                buttonNames[currentButton] = buttonName.text;
                if(buttonRange.value == 0)
                {
                    if(float.TryParse(rangeIn.text, out outVal))
                    {
                        buttonsList[currentButton].range.min = outVal;
                    }
                    else
                    {
                        buttonsList[currentButton].range.min = float.NaN;
                    }
                }
                else if(buttonRange.value == 1)
                {
                    if(float.TryParse(rangeIn.text,out outVal))
                    {
                        buttonsList[currentButton].range.max = outVal;
                    }
                    else
                    {
                        buttonsList[currentButton].range.max = float.NaN;
                    }
                }
                else if(buttonRange.value == 2)
                {
                    float.TryParse(rangeIn.text, out outVal);
                    buttonsList[currentButton].range.increment = outVal;
                }
            }
            buttonsList[currentButton].actionRoles = temp;
            buttonsD.ClearOptions();
            buttonsD.AddOptions(buttonNames);
            buttonsD.value = currentButton;
            loadButton();
        }
    }

    public void loadButton()
    {
        inLoad = true;
        if(buttonsList.Count > 0)
        {
            buttonARole.interactable = true;
            buttonLoc.interactable = true;
            buttonVis.interactable = true;
            buttonType.interactable = true;
            buttonOwner.interactable = true;
            buttonOwnerLoc.interactable = true;
            buttonName.interactable = true;
            currentButton = buttonsD.value;
            buttonARole.value = InttoBitfield(buttonsList[currentButton].actionRoles);
            if(buttonsList[currentButton].type == ButtonType.CLICK)
            {
                buttonType.value = 0;
                buttonRangeObj.SetActive(false);
            }
            if(buttonsList[currentButton].type == ButtonType.NUMBER)
            {
                buttonType.value = 1;
                buttonRangeObj.SetActive(true);
            }
            if(locationsList.Count > 0)
            {
                buttonLoc.value = buttonsList[currentButton].location.index;
                buttonOwnerLoc.value = buttonsList[currentButton].ownerLocation.index;
            }
            if(buttonsList[currentButton].visibility == vis.FACE_UP)
            {
                buttonVis.value = 0;
            }
            else if(buttonsList[currentButton].visibility == vis.FACE_DOWN)
            {
                buttonVis.value = 1;
            }
            else if(buttonsList[currentButton].visibility == vis.INVISIBLE)
            {
                buttonVis.value = 2;
            }
            else if(buttonsList[currentButton].visibility == vis.PRIVATE)
            {
                buttonVis.value = 3;
            }
            if(buttonsList[currentButton].ownership == ownership.BOARD)
            {
                buttonOwner.value = 0;
                buttonOwnerLoc.interactable = false;
            }
            else if(buttonsList[currentButton].ownership == ownership.PLAYER)
            {
                buttonOwner.value = 1;
                buttonOwnerLoc.interactable = true;
            }
            buttonName.text = buttonNames[currentButton];
            loadMinMaxButton();
        }
        else
        {
            currentButton = 0;
            buttonName.text = "";
            buttonType.value = 0;
            buttonRangeObj.SetActive(false);
            buttonLoc.value = 0;
            buttonOwnerLoc.value = 0;
            buttonVis.value = 0;
            buttonARole.value = 0;
            buttonARole.interactable = false;
            buttonLoc.interactable = false;
            buttonOwnerLoc.interactable = false;
            buttonVis.interactable = false;
            buttonType.interactable = false;
            buttonOwner.interactable = false;
            buttonName.interactable = false;
        }
        inLoad = false;
    }

    public void loadMinMaxButton()
    {
        if(buttonsList.Count > 0)
        {
            currentMMVal = buttonRange.value;
            if(currentMMVal == 0)
            {
                rangeIn.text = buttonsList[currentButton].range.min.ToString();
            }
            else if(currentMMVal == 1)
            {
                rangeIn.text = buttonsList[currentButton].range.max.ToString();
            }
            else if(currentMMVal == 2)
            {
                rangeIn.text = buttonsList[currentButton].range.increment.ToString();
            }
        }
        else
        {
            buttonRange.value = 0;
            rangeIn.text = "";
        }
    }


//  COUNTERS =========================
    public void newCounter()
    {
        counterNames.Add("NewCounter");
        countersList.Add(new countersType(locationsList[0]));
        countersD.ClearOptions();
        countersD.AddOptions(counterNames);
        currentCounter = countersList.Count-1;
        countersD.value = currentCounter;
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
        if(countersList.Count > 0 && inLoad == false)
        {
            counterNames[currentCounter] = counterName.text;
            countersList[currentCounter].cName = counterName.text;
            float outVal;
            float.TryParse(counterValue.text, out outVal);
            countersList[currentCounter].number = outVal; 
            if(locationsList.Count > 0)
            {
                countersList[currentCounter].location = locationsList[counterLoc.value];
                countersList[currentCounter].ownerLocation = locationsList[counterOwnerLoc.value];
            }
            if(counterVis.value == 0)
            {
                countersList[currentCounter].visibility = vis.FACE_UP;
            }
            else if(counterVis.value == 1)
            {
                countersList[currentCounter].visibility = vis.FACE_DOWN;
            }
            else if(counterVis.value == 2)
            {
                countersList[currentCounter].visibility = vis.INVISIBLE;
            }
            else if(counterVis.value == 3)
            {
                countersList[currentCounter].visibility = vis.PRIVATE;
            }
            if(counterOwner.value == 0)
            {
                countersList[currentCounter].ownership = ownership.BOARD;
                countersList[currentCounter].ownerLocation = locationsList[0];
            }
            else if(counterOwner.value == 1)
            {
                countersList[currentCounter].ownership = ownership.PLAYER;
            }
            int[] r = bitFieldToInt(counterARole.value);
            List<string> temp = new List<string>();
            for(int i = 0; i < aRolesList.Count; i++)
            {
                if(r[i] == 1)
                {
                    temp.Add(aRolesList[i]);
                }
            }
            countersList[currentCounter].actionRoles = temp;
            countersD.ClearOptions();
            countersD.AddOptions(counterNames);
            countersD.value = currentCounter;
            loadCounter();
        }
    }

    public void loadCounter()
    {
        inLoad = true;
        if(countersList.Count > 0)
        {
            counterName.interactable = true;
            counterValue.interactable = true;
            counterVis.interactable = true;
            counterLoc.interactable = true;
            counterOwnerLoc.interactable = true;
            counterOwner.interactable = true;
            counterARole.interactable = true;
            currentCounter = countersD.value;
            counterName.text = counterNames[currentCounter];
            counterValue.text = countersList[currentCounter].number.ToString();
            if(locationsList.Count > 0)
            {
                counterLoc.value = countersList[currentCounter].location.index;
                counterOwnerLoc.value = countersList[currentCounter].ownerLocation.index;
            }
            if(countersList[currentCounter].visibility == vis.FACE_UP)
            {
                counterVis.value = 0;
            }
            else if(countersList[currentCounter].visibility == vis.FACE_DOWN)
            {
                counterVis.value = 1;
            }
            else if(countersList[currentCounter].visibility == vis.INVISIBLE)
            {
                counterVis.value = 2;
            }
            else if(countersList[currentCounter].visibility == vis.PRIVATE)
            {
                counterVis.value = 3;
            }
            if(countersList[currentCounter].ownership == ownership.BOARD)
            {
                counterOwner.value = 0;
                counterOwnerLoc.interactable = false;
            }
            else if(countersList[currentCounter].ownership == ownership.PLAYER)
            {
                counterOwner.value = 1;
                counterOwnerLoc.interactable = true;
            }
            counterARole.value = InttoBitfield(countersList[currentCounter].actionRoles);
        }
        else
        {
            currentCounter = 0;
            counterName.text = "";
            counterValue.text = "";
            counterLoc.value = 0;
            counterOwnerLoc.value = 0;
            counterVis.value = 0;
            counterARole.value = 0;
            counterName.interactable = false;
            counterValue.interactable = false;
            counterVis.interactable = false;
            counterLoc.interactable = false;
            counterOwnerLoc.interactable = false;
            counterOwner.interactable = false;
            counterARole.interactable = false;
        }
        inLoad = false;
    }


//  LOCATIONS =========================
    public void newLocation()
    {
        locationNames.Add("NewLocation");
        locationsList.Add(new locationsType());
        locationsD.ClearOptions();
        locationsD.AddOptions(locationNames);
        currentLocation = locationsList.Count-1;
        locationsList[currentLocation].index = currentLocation;
        locationsD.value = currentLocation;
        loadLocation();
    }

    public void deleteLocation()
    {
        if(locationsList.Count > 1 && locationsD.value > 0)
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
        if(locationsList.Count > 0 && inLoad == false)
        {
            float outVal;
            locationNames[currentLocation] = locationName.text;
            locationsList[currentLocation].lName = locationName.text;
            float.TryParse(locationX.text,out outVal);
            locationsList[currentLocation].x = clamp(maxX/2, -maxX/2, outVal);
            float.TryParse(locationY.text,out outVal);
            locationsList[currentLocation].y = clamp(maxY/2, -maxY/2, outVal);
            float.TryParse(locationXOff.text,out outVal);
            locationsList[currentLocation].xOff = clamp(maxX, -maxX, outVal);
            float.TryParse(locationYOff.text,out outVal);
            locationsList[currentLocation].yOff = clamp(maxY, -maxY, outVal);
            float.TryParse(locationWrapAt.text,out outVal);
            if(locationOption.value == 0)
            {
                outVal = clamp(maxX,-maxX,outVal);
            }
            else
            {
                outVal = clamp(maxY,-maxY,outVal);
            }
            locationsList[currentLocation].wrapAt = outVal;
            float.TryParse(locationWrapTo.text,out outVal);
            if(locationOption.value == 0)
            {
                outVal = clamp(maxX,-maxX,outVal);
            }
            else
            {
                outVal = clamp(maxY,-maxY,outVal);
            }
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
        inLoad = true;
        if(locationsList.Count > 0)
        {
            currentLocation = locationsD.value;
            locationsList[currentLocation].index = currentLocation;
            if(locationsList[currentLocation].editable == false)
            {
                locationName.interactable = false;
                locationX.interactable = false;
                locationY.interactable = false;
                locationXOff.interactable = false;
                locationYOff.interactable = false;
                locationWrapAt.interactable = false;
                locationWrapTo.interactable = false;
                locationOption.interactable = false;
                locationName.text = locationNames[currentLocation];
                locationX.text = "-";
                locationY.text = "-";
                locationXOff.text = "-";
                locationYOff.text = "-";
                locationWrapAt.text = "-";
                locationWrapTo.text = "-";
                locationOption.value = 0;
            }
            else
            {
                locationName.interactable = true;
                locationX.interactable = true;
                locationY.interactable = true;
                locationXOff.interactable = true;
                locationYOff.interactable = true;
                locationWrapAt.interactable = true;
                locationWrapTo.interactable = true;
                locationOption.interactable = true;
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
            redrawLocations();
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
        inLoad = false;
    }

    public void redrawLocations()
    {
        pileLoc.ClearOptions();
        pileLoc.AddOptions(locationNames);
        pileOwnerLoc.ClearOptions();
        pileOwnerLoc.AddOptions(locationNames);
        if(pilesList.Count > 0)
        {
            if(pilesList[currentPile].location.index < locationsList.Count)
            {
                pileLoc.value = pilesList[currentPile].location.index;
            }
            if(pilesList[currentPile].ownerLocation.index < locationsList.Count)
            {
                pileOwnerLoc.value = pilesList[currentPile].ownerLocation.index;
            }
        }
        else
        {
            pileLoc.value = 0;
            pileOwnerLoc.value = 0;
        }
        buttonLoc.ClearOptions();
        buttonLoc.AddOptions(locationNames);
        buttonOwnerLoc.ClearOptions();
        buttonOwnerLoc.AddOptions(locationNames);
        if(buttonsList.Count > 0)
        {
            if(buttonsList[currentButton].location.index < locationsList.Count)
            {
                buttonLoc.value = buttonsList[currentButton].location.index;
            }
            if(buttonsList[currentButton].ownerLocation.index < locationsList.Count)
            {
                buttonOwnerLoc.value = buttonsList[currentButton].ownerLocation.index;
            }
        }
        else
        {
            buttonLoc.value = 0;
            buttonOwnerLoc.value = 0;
        }
        counterLoc.ClearOptions();
        counterLoc.AddOptions(locationNames);
        counterOwnerLoc.ClearOptions();
        counterOwnerLoc.AddOptions(locationNames);
        if(countersList.Count > 0)
        {
            if(countersList[currentCounter].location.index < locationsList.Count)
            {
                counterLoc.value = countersList[currentCounter].location.index;
            }
            if(countersList[currentCounter].ownerLocation.index < locationsList.Count)
            {
                counterOwnerLoc.value = countersList[currentCounter].ownerLocation.index;
            }
        }
        else
        {
            counterLoc.value = 0;
            counterOwnerLoc.value = 0;
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
            pRoleName.interactable = true;
            currentPRole = pRolesD.value;
            pRoleName.text = pRolesList[currentPRole];
        }
        else
        {
            pRoleName.interactable = false;
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
            aRoleName.interactable = true;
            currentARole = aRolesD.value;
            aRoleName.text = aRolesList[currentARole];
        }
        else
        {
            aRoleName.interactable = false;
            currentARole = 0;
            aRoleName.text = "";
        }
        redrawARoles();
    }

    public void redrawARoles()
    {
        pileARole.ClearOptions();
        pileARole.AddOptions(aRolesList);
        if(pilesList.Count > 0)
        {
            pileARole.value = InttoBitfield(pilesList[currentPile].actionRoles);
        }
        else
        {
            pileARole.value = 0;
        }
        counterARole.ClearOptions();
        counterARole.AddOptions(aRolesList);
        if(countersList.Count > 0)
        {
            counterARole.value = InttoBitfield(countersList[currentCounter].actionRoles);
        }
        else
        {
            counterARole.value = 0;
        }
        buttonARole.ClearOptions();
        buttonARole.AddOptions(aRolesList);
        if(buttonsList.Count > 0)
        {
            buttonARole.value = InttoBitfield(buttonsList[currentButton].actionRoles);
        }
        else
        {
            buttonARole.value = 0;
        }
    }

    //  MAPS ==============================================
    public void newMap()
    {
        mapsList.Add(new mapsType());
        mapNames.Add("NewMap");
        mapsD.ClearOptions();
        mapsD.AddOptions(mapNames);
        currentMap = mapsList.Count-1;
        mapsD.value = currentMap;
        loadMap(0);
    }

    public void deleteMap()
    {
        if(mapsList.Count > 0)
        {
            mapsList.RemoveAt(currentMap);
            mapNames.RemoveAt(currentMap);
            mapsD.ClearOptions();
            mapsD.AddOptions(mapNames);
            if(mapsList.Count > 0)
            {
                mapsD.value = currentMap;
            }
            else if(mapsList.Count+2 == currentMap)
            {
                currentMap = mapsList.Count-1;
                mapsD.value = currentMap;
            }
            loadMap(0);
        }
    }

    public void modifyMap()
    {
        if(mapsList.Count > 0 && inLoad == false)
        {
            mapNames[currentMap] = mapName.text;
            mapsList[currentMap].mName = mapName.text;
            mapsD.ClearOptions();
            mapsD.AddOptions(mapNames);
            mapsD.value = currentMap;
            loadMap(1);
        }
    }

    public void loadMap(int code)
    {
        inLoad = true;
        if(mapsList.Count > 0)
        {
            mapName.interactable = true;
            mapKeyDropdown.interactable = true;
            valueAdd.interactable = true;
            valueDelete.interactable = true;
            currentMap = mapsD.value;
            mapKeyDropdown.ClearOptions();
            List<string> temp = new List<string>();
            foreach(mapVal x in mapsList[currentMap].map)
            {
                temp.Add(x.key);
            }
            mapKeyDropdown.AddOptions(temp);
            if(mapsList[currentMap].map.Count > 0)
            {
                mapValue.interactable = true;
                mapKey.interactable = true;
                if(code == 0)
                {
                    currentMapKey = 0;
                    loadValue();
                }
            }
            else
            {
                currentMapKey = 0;
                loadValue();
            }
            mapName.text = mapNames[currentMap];
        }
        else
        {
            currentMap = 0;
            currentMapKey = 0;
            mapName.text = "";
            mapValue.text = "";
            mapKey.text = "";
            mapValue.text = "";
            mapName.interactable = false;
            mapKeyDropdown.interactable = false;
            mapValue.interactable = false;
            mapKey.interactable = false;
            valueAdd.interactable = false;
            valueDelete.interactable = false;
        }
        inLoad = false;
    }

    public void newValue()
    {
        mapsList[currentMap].map.Add(new mapVal());
        mapKeyDropdown.ClearOptions();
        List<string> temp = new List<string>();
        foreach(mapVal x in mapsList[currentMap].map)
        {
            temp.Add(x.key);
        }
        mapKeyDropdown.AddOptions(temp);
        currentMapKey = mapsList[currentMap].map.Count-1;
        mapKeyDropdown.value = currentMapKey;
        loadValue();
    }

    public void deleteValue()
    {
        if(mapsList[currentMap].map.Count > 0)
        {
            mapsList[currentMap].map.RemoveAt(currentMapKey);
            mapKeyDropdown.ClearOptions();
            List<string> temp = new List<string>();
            foreach(mapVal x in mapsList[currentMap].map)
            {
                temp.Add(x.key);
            }
            mapKeyDropdown.AddOptions(temp);
            if(mapsList[currentMap].map.Count > 0)
            {
                mapKeyDropdown.value = currentMapKey;
            }
            else if(mapsList[currentMap].map.Count+2 == currentMapKey)
            {
                currentMapKey = mapsList[currentMap].map.Count-1;
                mapKeyDropdown.value = currentMapKey;
            }
            loadValue();
        }
    }

    public void modifyValue()
    {
        if(inLoad == false && mapsList[currentMap].map.Count > 0)
        {
            mapsList[currentMap].map[currentMapKey].key = mapKey.text;
            int x;
            if(int.TryParse(mapValue.text,out x))
            {
                mapsList[currentMap].map[currentMapKey].value = x;
            }
            else
            {
                mapsList[currentMap].map[currentMapKey].value = 0;
            }
            mapKeyDropdown.ClearOptions();
            List<string> temp = new List<string>();
            foreach(mapVal y in mapsList[currentMap].map)
            {
                temp.Add(y.key);
            }
            mapKeyDropdown.AddOptions(temp);
            mapKeyDropdown.value = currentMapKey;
            loadValue();
        }
    }

    public void loadValue()
    {
        inLoad = true;
        if(mapsList[currentMap].map.Count > 0)
        {
            currentMapKey = mapKeyDropdown.value;
            mapValue.interactable = true;
            mapKey.interactable = true;
            mapValue.text = mapsList[currentMap].map[currentMapKey].value.ToString();
            mapKey.text = mapsList[currentMap].map[currentMapKey].key;
        }
        else
        {
            mapValue.text = "0";
            mapKey.text = "";
            mapValue.interactable = false;
            mapKey.interactable = false;
        }
        inLoad = false;
    }
}
