using System;
using System.Collections.Generic;
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

    public void Start()
    {
        stepNames = new List<List<string>>();
        steps = new List<List<GameObject>>();
        List<string> nL = new List<string>();
        stepNames.Add(nL);
        stepNames[0].Add("Step 1");
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
        phaseNames.Add("Phase " + x);
        GameObject newPhase = Instantiate(phasePrefab, phaseParent);
        phases.Add(newPhase);
        stepParent = phases[phases.Count-1].transform.Find("Steps/StepsPanel Parent").GetComponent<Transform>();
        GameObject newStep = Instantiate(stepPrefab, stepParent);
        List<GameObject> sL = new List<GameObject>();
        stepParent = newPhase.transform.Find("Steps/StepsPanel Parent").GetComponent<Transform>();
        sL.Add(newStep);
        steps.Add(sL);
        List<string> sN = new List<string>();
        sN.Add("Step 1");
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
        stepNames[currentPhase].Add("Step " + x);
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
        for(int x = 0; x < phases.Count; x++)
        {
            setPhase(x);
            for(int y = 0; y < steps[x].Count; y++)
            {
                setStep(y);
                actionBlockController[] actionBlocks = steps[x][y].GetComponentsInChildren<actionBlockController>();
                foreach(actionBlockController block in actionBlocks)
                {
                    block.GetComponent<UIDraggableBlock>().evalutate();
                }
            }
        }
    }
}
