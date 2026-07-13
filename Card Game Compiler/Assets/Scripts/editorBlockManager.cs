using System;
using System.Collections.Generic;
using UnityEngine;

public class editorBlockManager : MonoBehaviour
{
    public List<block> blockList;
    public List<block> variableBlockList;
    public List<block> labelBlockList;
    public List<block> locationBlockList;
    public Transform blockParent;
    public Transform variableBlockParent;
    public Transform objectBlockParent;
    public Transform sPBlockParent;
    public GameObject mainBlockPrefab;
    public GameObject variableBlockPrefab;
    public GameObject buttonBlockPrefab;
    public GameObject pileBlockPrefab;
    public GameObject counterBlockPrefab;
    public GameObject stepBlockPrefab;
    public GameObject phaseBlockPrefab;
    public GameObject actionRoleBlockPrefab;
    public GameObject playerRoleBlockPrefab;
    public GameObject locationBlockPrefab;
    public float y;
    public float v;
    public float o;
    public float sP;

    //all blocks are literal except main and location

    public void setBlockList(List<block> bL)
    {
        blockList = bL;
        drawBlocks();
    }

    public void drawBlocks()
    {
        y = 0;
        foreach(Transform child in blockParent) 
        {
            Destroy(child.gameObject);
        }
        foreach(block b in blockList)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            newBlock.GetComponent<blockController>().Init(b.name,b.displayName,b.returnType,b.arguments);
            y -= 0.75f;
        }
    }

    public void drawMetaBlocks(List<variablesType> variables, List<pilesType> piles, List<buttonsType> buttons, List<countersType> counters, List<locationsType> locations, List<string> pRoles, List<string> aRoles)
    {
        v = 0;
        o = 0;
        foreach(Transform child in variableBlockParent) 
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in objectBlockParent) 
        {
            Destroy(child.gameObject);
        }
        foreach(variablesType b in variables)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, variableBlockParent.position + new Vector3(0,v,0), Quaternion.identity, variableBlockParent);
            newBlock.GetComponent<blockController>().Init(b.vName,b.vName,b.returnType(),null);
            v -= 0.75f;
        }
        foreach(pilesType b in piles)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init(b.pName,b.pName,"Pile",null);
            o -= 0.75f;
        }
        foreach(buttonsType b in buttons)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init(b.bName,b.bName,"Button",null);
            o -= 0.75f;
        }
        foreach(countersType b in counters)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init(b.cName,b.cName,"Counter",null);
            o -= 0.75f;
        }
        foreach(locationsType b in locations)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init(b.lName,b.lName,"Location",null);
            o -= 0.75f;
        }
        foreach(string b in aRoles)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init(b,b,"ActionRole",null);
            o -= 0.75f;
        }
        foreach(string b in pRoles)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init(b,b,"PlayerRole",null);
            o -= 0.75f;
        }
    }

    public void updateStepsPhasesBlocks(List<string> steps, List<string> phases)
    {
        sP = 0;
        foreach(Transform child in sPBlockParent) 
        {
            Destroy(child.gameObject);
        }
        foreach(string b in steps)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, sPBlockParent.position + new Vector3(0,sP,0), Quaternion.identity, sPBlockParent);
            newBlock.GetComponent<blockController>().Init(b,b,"Step",null);
            sP -= 0.75f;
        }
        foreach(string b in phases)
        {
            GameObject newBlock = Instantiate(mainBlockPrefab, sPBlockParent.position + new Vector3(0,sP,0), Quaternion.identity, sPBlockParent);
            newBlock.GetComponent<blockController>().Init(b,b,"Phase",null);
            sP -= 0.75f;
        }
    }
}
