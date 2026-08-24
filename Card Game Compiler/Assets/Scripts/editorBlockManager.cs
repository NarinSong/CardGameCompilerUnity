using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
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
    public GameObject actionPrefab;
    public GameObject noArgPrefab;
    public GameObject logicPrefab;
    public GameObject logicWPrefab;
    public GameObject oneArgPrefab;
    public GameObject twoArgPrefab;
    public GameObject threeArgPrefab;
    public GameObject fourArgPrefab;
    public GameObject fiveArgPrefab;
    public GameObject sixArgPrefab;
    public GameObject sevenArgPrefab;
    public GameObject eightArgPrefab;
    public GameObject objectPrefab;
    public GameObject literalPrefab;
    public GameObject arrayPrefab;
    public GameObject oneArgVarPrefab;
    public GameObject twoArgVarPrefab;
    public float y;
    public float v;
    public float o;
    public float sP;

    public void setBlockList(List<block> bL)
    {
        blockList = bL;
        drawBlocks();
    }

    //Fix magic numbers and find better way to do block offsets after rendering please! - N
    
    //Add better handling for scrollable list to dynamically size the list based on the # of blocks - N

    public void drawBlocks()
    {
        y = 0;
        foreach(Transform child in blockParent) 
        {
            Destroy(child.gameObject);
        }
        GameObject newBlock = null;
        newBlock = Instantiate(actionPrefab, blockParent.position + new Vector3(0.75f,y,0), Quaternion.identity, blockParent);
        newBlock.GetComponent<blockController>().Init("ACTION","Action",null,null);
        y -= 1.5f;
        newBlock = Instantiate(literalPrefab, blockParent.position + new Vector3(0.7f,y,0), Quaternion.identity, blockParent);
        newBlock.GetComponent<blockController>().Init("LITERAL","Literal","Number",null);
        y -= 0.75f;
        newBlock = Instantiate(arrayPrefab, blockParent.position + new Vector3(0.6f,y,0), Quaternion.identity, blockParent);
        newBlock.GetComponent<blockController>().Init("ARRAY","Array","Array",null);
        y -= 0.75f;
        args[] variableArgs = new args[]{new args("name","variable",false), new args("value","variable",true)};
        args[] getVariableArgs = new args[]{new args("name","variable",false)};
        newBlock = Instantiate(oneArgVarPrefab, blockParent.position + new Vector3(0f,y,0f),Quaternion.identity, blockParent);
        newBlock.GetComponent<blockController>().Init("GET_VARIABLE","Get Variable","Number",getVariableArgs);
        y -= 0.75f;
        newBlock = Instantiate(twoArgVarPrefab, blockParent.position + new Vector3(0f,y,0f),Quaternion.identity, blockParent);
        newBlock.GetComponent<blockController>().Init("UPDATE_VARIABLE","Set Variable","Number",variableArgs);
        y -= 0.75f;
        foreach(block b in blockList)
        {
            if(b.name == "IF")
            {
                y -= 0.05f;
                newBlock = Instantiate(logicPrefab, blockParent.position + new Vector3(-0.1f,y,0), Quaternion.identity, blockParent);
                y -= 1.05f;
            }
            else if(b.name == "WHILE")
            {
                y -= 0.05f;
                newBlock = Instantiate(logicWPrefab, blockParent.position + new Vector3(-0.1f,y,0), Quaternion.identity, blockParent);
                y -= 1.05f;
            }
            else if(b.arguments.Length == 0)
            {
                newBlock = Instantiate(noArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
            else if(b.arguments.Length == 1)
            {
                newBlock = Instantiate(oneArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
            else if(b.arguments.Length == 2)
            {
                newBlock = Instantiate(twoArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
            else if(b.arguments.Length == 3)
            {
                newBlock = Instantiate(threeArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
            else if(b.arguments.Length == 4)
            {
                newBlock = Instantiate(fourArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
            else if(b.arguments.Length == 5)
            {
                newBlock = Instantiate(fiveArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
            else if(b.arguments.Length == 6)
            {
                newBlock = Instantiate(sixArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
            else if(b.arguments.Length == 7)
            {
                newBlock = Instantiate(sevenArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
            else if(b.arguments.Length == 8)
            {
                newBlock = Instantiate(eightArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
            else
            {
                newBlock = Instantiate(noArgPrefab, blockParent.position + new Vector3(0,y,0), Quaternion.identity, blockParent);
            }
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
            GameObject newBlock = Instantiate(objectPrefab, variableBlockParent.position + new Vector3(0,v,0), Quaternion.identity, variableBlockParent);
            newBlock.GetComponent<blockController>().Init("LITERAL",b.vName,"String",null);
            v -= 0.75f;
        }
        foreach(pilesType b in piles)
        {
            GameObject newBlock = Instantiate(objectPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init("LITERAL",b.pName,"PileLabel",null);
            o -= 0.75f;
        }
        foreach(buttonsType b in buttons)
        {
            GameObject newBlock = Instantiate(objectPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init("LITERAL",b.bName,"ButtonLabel",null);
            o -= 0.75f;
        }
        foreach(countersType b in counters)
        {
            GameObject newBlock = Instantiate(objectPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init("LITERAL",b.cName,"CounterLabel",null);
            o -= 0.75f;
        }
        foreach(locationsType b in locations)
        {
            GameObject newBlock = Instantiate(objectPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init("LITERAL",b.lName,"Location",null);
            o -= 0.75f;
        }
        foreach(string b in aRoles)
        {
            GameObject newBlock = Instantiate(objectPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init("LITERAL",b,"ActionRole",null);
            o -= 0.75f;
        }
        foreach(string b in pRoles)
        {
            GameObject newBlock = Instantiate(objectPrefab, objectBlockParent.position + new Vector3(0,o,0), Quaternion.identity, objectBlockParent);
            newBlock.GetComponent<blockController>().Init("LITERAL",b,"PlayerRole",null);
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
            GameObject newBlock = Instantiate(objectPrefab, sPBlockParent.position + new Vector3(0,sP,0), Quaternion.identity, sPBlockParent);
            newBlock.GetComponent<blockController>().Init("LITERAL",b,"Step",null);
            sP -= 0.75f;
        }
        foreach(string b in phases)
        {
            GameObject newBlock = Instantiate(objectPrefab, sPBlockParent.position + new Vector3(0,sP,0), Quaternion.identity, sPBlockParent);
            newBlock.GetComponent<blockController>().Init("LITERAL",b,"Phase",null);
            sP -= 0.75f;
        }
    }
}
