using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class editorController : MonoBehaviour
{
    public List<GameObject> phases;
    public List<List<GameObject>> steps;
    public List<string> phaseNames;
    public List<List<string>> stepNames;
    public int currentPhase;
    public int currentStep;
    public TMP_Dropdown phaseList;
    public TMP_Dropdown stepList;
    public GameObject phasePrefab;
    public GameObject stepPrefab;
    public Transform phaseParent;
    public Transform stepParent;
    public editorBlockManager bM;
    public variableController vC;
    public TMP_InputField gameName;
    public TMP_InputField gameDescription;
    public TMP_InputField minPlayers;
    public TMP_InputField maxPlayers;
    public websocketController wS;

    public void Start()
    {
        stepNames = new List<List<string>>();
        steps = new List<List<GameObject>>();
        List<string> nL = new List<string>();
        stepNames.Add(nL);
        stepNames[0].Add("Step1");
        currentPhase = 0;
        currentStep = 0;
        List<GameObject> startList = new List<GameObject>();
        startList.Add(GameObject.Find("Step"));
        steps.Add(startList);
        bM.updateStepsPhasesBlocks(stepNames[currentPhase],phaseNames);
    }

    public void drawBlocksFromSP()
    {
        List<string> max = new List<string>();
        foreach(List<string> x in stepNames)
        {
            if(x.Count > max.Count)
            {
                max = x;
            }
        }
        bM.updateStepsPhasesBlocks(max,phaseNames);
    }

    public void removePhase()
    {
        Debug.Log("Removing Phase at P:" + currentPhase);
        if(phases.Count > 1)
        {
            Destroy(phases[currentPhase]);
            phases.RemoveAt(currentPhase);
            phaseNames.RemoveAt(currentPhase);
            steps.RemoveAt(currentPhase);
            if(currentPhase >= phases.Count)
            {
                currentPhase = phases.Count-1;
            }
            setPhase(currentPhase);
            phaseList.ClearOptions();
            phaseList.AddOptions(phaseNames);
            phaseList.value = currentPhase;
            stepList.ClearOptions();
            stepList.AddOptions(stepNames[currentPhase]);
            drawBlocksFromSP();
        }
    }

    public void removeStep()
    {
        Debug.Log("Removing Step at P:" + currentPhase + " S:" + currentStep);
        if(steps[currentPhase].Count > 1)
        {
            Destroy(steps[currentPhase][currentStep]);
            steps[currentPhase].RemoveAt(currentStep);
            stepNames[currentPhase].RemoveAt(currentStep);
            if(currentStep >= steps[currentPhase].Count)
            {
                currentStep = steps[currentPhase].Count-1;
            }
            setStep(currentStep);
            stepList.ClearOptions();
            stepList.AddOptions(stepNames[currentPhase]);
            stepList.value = currentStep;
            drawBlocksFromSP();
        }
    }

    public void addPhase()
    {
        int x = phases.Count+1;
        phaseNames.Add("Phase" + x);
        GameObject newPhase = Instantiate(phasePrefab, phaseParent);
        phases.Add(newPhase);
        stepParent = phases[phases.Count-1].transform.Find("Steps/StepsPanel Parent").GetComponent<Transform>();
        GameObject newStep = Instantiate(stepPrefab, stepParent);
        List<GameObject> sL = new List<GameObject>();
        stepParent = newPhase.transform.Find("Steps/StepsPanel Parent").GetComponent<Transform>();
        sL.Add(newStep);
        steps.Add(sL);
        List<string> sN = new List<string>();
        sN.Add("Step1");
        stepNames.Add(sN);
        setPhase(phases.Count-1);
        phaseList.ClearOptions();
        phaseList.AddOptions(phaseNames);
        phaseList.value = phases.Count-1;
        stepList.ClearOptions();
        stepList.AddOptions(stepNames[currentPhase]);
        //phaseList.value = phases.Count-1;
        drawBlocksFromSP();
    }

    public void addStep()
    {
        Debug.Log("Attempting step creation at " + currentPhase + " s : " + currentStep);
        int x = steps[currentPhase].Count+1;
        stepNames[currentPhase].Add("Step" + x);
        GameObject newStep = Instantiate(stepPrefab, stepParent);
        steps[currentPhase].Add(newStep);
        stepList.ClearOptions();
        stepList.AddOptions(stepNames[currentPhase]);
        stepList.value = steps[currentPhase].Count-1;
        setStep(steps[currentPhase].Count-1);
        //stepList.value = steps[currentPhase].Count-1;
        drawBlocksFromSP();
    }

    public void setPhase(int c)
    {
        currentPhase = c;
        Debug.Log("setting phase P:" + currentPhase);
        foreach(GameObject x in phases)
        {
            x.SetActive(false);
        }
        phases[c].SetActive(true);
        stepParent = phases[c].transform.Find("Steps/StepsPanel Parent").GetComponent<Transform>();
        setStep(0);
        stepList.ClearOptions();
        stepList.AddOptions(stepNames[currentPhase]);
    }

    public void setStep(int c)
    {
        currentStep = c;
        Debug.Log("setting step P:" + currentPhase + " S:" + currentStep);
        foreach(GameObject x in steps[currentPhase])
        {
            x.SetActive(false);
        }
        steps[currentPhase][c].SetActive(true);
    }

    public void setStepList()
    {
        Debug.Log("called");
        setStep(stepList.value);
    }

    public void setPhaseList()
    {
        Debug.Log("called");
        setPhase(phaseList.value);
    }

    public void compile()
    {
        gameExport game = new gameExport();
        game.gameMeta.minPlayers = int.Parse(minPlayers.text);
        game.gameMeta.maxPlayers = int.Parse(maxPlayers.text);
        game.gameMeta.name = gameName.text;
        game.gameMeta.description = gameDescription.text;
        foreach(variablesType v in vC.variablesList)
        {
            game.gameMeta.variables.Add(v.vName, v.returnType());
        }
        foreach(locationsType l in vC.locationsList)
        {
            game.gameMeta.locations.Add(l.lName, new {anchor = new {x = l.x, y = l.y}, direction = l.convertVertHori(), verticalOffset = l.yOff, horizontalOffset = l.xOff, wrapAt = l.wrapAt, wrapTo = l.wrapTo});
        }
        List<dynamic> boardPiles = new List<dynamic>();
        List<dynamic> playerPiles = new List<dynamic>();
        foreach(pilesType p in vC.pilesList)
        {
            if(p.ownership == ownership.BOARD)
            {
                boardPiles.Add(new {label = p.pName, actionRoles = p.actionRoles , initialState = p.returnType(), visibility = p.returnVis(), location = new {locationType = "relative", location = p.location.lName}});
            }
            else if(p.ownership == ownership.PLAYER)
            {
                playerPiles.Add(new {label = p.pName, actionRoles = p.actionRoles , initialState = p.returnType(), visibility = p.returnVis(), location = new {locationType = "relative", location = p.location.lName}});
            }
        }
        game.boardDefinition.Add("piles", boardPiles);
        game.playerDefinition.Add("piles", playerPiles);
        List<dynamic> boardButtons = new List<dynamic>();
        List<dynamic> playerButtons = new List<dynamic>();
        foreach(buttonsType b in vC.buttonsList)
        {
            if(b.ownership == ownership.BOARD)
            {
                if(b.type == ButtonType.CLICK)
                {
                    boardButtons.Add(new {label = b.bName, actionRoles = b.actionRoles , type = b.type, location = new {locationType = "relative", location = b.location.lName}, visibility = b.returnVis()});
                }
                else if(b.type == ButtonType.NUMBER)
                {
                    boardButtons.Add(new {label = b.bName, actionRoles = b.actionRoles , range = b.range, type = b.type, location = new {locationType = "relative", location = b.location.lName}, visibility = b.returnVis()});
                }
            }
            else if(b.ownership == ownership.PLAYER)
            {
                if(b.type == ButtonType.CLICK)
                {
                    playerButtons.Add(new {label = b.bName, actionRoles = b.actionRoles , type = b.type, location = new {locationType = "relative", location = b.location.lName}, visibility = b.returnVis()});
                }
                else if(b.type == ButtonType.NUMBER)
                {
                    playerButtons.Add(new {label = b.bName, actionRoles = b.actionRoles , range = b.range, type = b.type, location = new {locationType = "relative", location = b.location.lName}, visibility = b.returnVis()});
                }
            }
        }
        game.boardDefinition.Add("buttons", boardButtons);
        game.playerDefinition.Add("buttons", playerButtons);
        List<dynamic> boardCounters = new List<dynamic>();
        List<dynamic> playerCounters = new List<dynamic>();
        foreach(countersType c in vC.countersList)
        {
            if(c.ownership == ownership.BOARD)
            {
                boardCounters.Add(new {label = c.cName, actionRoles = c.actionRoles , number = c.number, visibility = c.returnVis(), location = new {locationType = "relative", location = c.location.lName}});
            }
            else if(c.ownership == ownership.PLAYER)
            {
                boardCounters.Add(new {label = c.cName, actionRoles = c.actionRoles , number = c.number, visibility = c.returnVis(), location = new {locationType = "relative", location = c.location.lName}});
            }
        }
        game.boardDefinition.Add("counters", boardCounters);
        game.playerDefinition.Add("counters", playerCounters);
        for(int x = 0; x < phases.Count; x++)
        {
            setPhase(x);
            game.phases.Add(new phaseExport());
            game.phases[x].name = phaseNames[x];
            for(int y = 0; y < steps[x].Count; y++)
            {
                game.phases[x].steps.Add(new stepExport());
                game.phases[x].steps[y].name = stepNames[x][y];
                setStep(y);
                actionBlockController[] actionBlocks = steps[x][y].GetComponentsInChildren<actionBlockController>();
                Debug.Log(actionBlocks.Length + " Action blocks found in step");
                for(int z = 0; z < actionBlocks.Length; z++)
                {
                    game.phases[x].steps[y].actions.Add(new actionExport());
                    game.phases[x].steps[y].actions[z].trigger.Add("type", actionBlocks[z].returnType());
                    Debug.Log(z);
                    if(actionBlocks[z].returnType() == "CLICK")
                    {
                        if(actionBlocks[z].GetComponent<UIDraggableBlock>().myParts[0].transform.GetComponentInChildren<blockController>() != null)
                        {
                            game.phases[x].steps[y].actions[z].trigger.Add("target", actionBlocks[z].GetComponent<UIDraggableBlock>().myParts[0].transform.GetComponentInChildren<blockController>().bname);
                        }
                    }
                    if(actionBlocks[z].GetComponent<UIDraggableBlock>().myParts[1].transform.GetComponentInChildren<blockController>() != null)
                    {
                        game.phases[x].steps[y].actions[z].filter = actionBlocks[z].GetComponent<UIDraggableBlock>().myParts[1].transform.GetComponentInChildren<UIDraggableBlock>().evalutate()[0];
                    }
                    else
                    {
                        game.phases[x].steps[y].actions[z].filter = null;
                    }
                    game.phases[x].steps[y].actions[z].result.Add("type", "SEQUENCE");
                    if(actionBlocks[z].GetComponent<UIDraggableBlock>().myParts[2].transform.GetComponentInChildren<UIDraggableBlock>() != null)
                    {
                        List<dynamic> evalResult = actionBlocks[z].GetComponent<UIDraggableBlock>().myParts[2].transform.GetComponentInChildren<UIDraggableBlock>().evalutate();
                        game.phases[x].steps[y].actions[z].result.Add("primary", evalResult);
                    }
                }
            }
        }
        wS.sendGame(game);
    }
}
