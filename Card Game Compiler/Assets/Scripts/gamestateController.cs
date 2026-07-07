using UnityEngine;
using SocketIOClient;
using System.Collections.Generic;
using System;
public enum vis
{
    FACE_DOWN,
    FACE_UP,
    INVISIBLE
}

public enum playerType
{
    HUMAN,
    ROBOT,
    AI
}

public enum ButtonType
{
    CLICK,
    NUMBER
}

public class loc
{
    public float x {get; set;}
    public float y {get; set;}
}

public class card
{
    public int rank {get; set;}
    public int suit {get; set;}
    public int id {get; set;}
}

public class player
{
    public string type {get; set;}
    public int id {get; set;}
}


public class counter
{
    public int owner {get; set;}
    public vis visibility {get; set;}
    public int value {get; set;}
    public loc location {get; set;}
    public string label {get; set;}
    public string displayName {get; set;}
    public string[] actionRoles {get; set;}
}
public class pile
{
    public int owner {get; set;}
    public vis visibility {get; set;}
    public card[] cards {get; set;}
    public loc location {get; set;}
    public string label {get; set;}
    public string displayName {get; set;}
    public string[] actionRoles {get; set;}
}

public class rangeObj
{
    public float min {get; set;}
    public float max {get; set;}
    public float increment {get; set;}
}

public class button
{
    public int owner {get; set;}
    public vis visibility {get; set;}
    public string label {get; set;}
    public loc location {get; set;}
    public string[] actionRoles {get; set;}
    public string displayName {get; set;}
    public ButtonType type {get; set;}
    public rangeObj range {get; set;}
}

public class board
{
    
}

public class gamestate
{
    public pile[] piles {get; set;}
    public counter[] counters {get; set;}
    public button[] buttons {get; set;}
    public player[] players {get; set;}
    //public board boardstate {get; set;}
}

public class gamestateController : MonoBehaviour
{

    public gamestate currentGamestate;
    public List<GameObject> counterObjects;
    public List<GameObject> pileObjects;
    public List<GameObject> buttonObjects;
    public Transform counterParent;
    public Transform pileParent;
    public Transform buttonParent;
    public GameObject counterPrefab;
    public GameObject pilePrefab;
    public GameObject buttonPrefab;
    public void updateGamestate(SocketIOResponse x)
    {
        Debug.Log(x);
        currentGamestate = x.GetValue<gamestate>(0);
        drawCounters();
        drawPiles();
        drawButtons();
    }

    public void drawCounters()
    {
        foreach(Transform child in counterParent) 
        {
            Destroy(child.gameObject);
        }
        for(int i = 0; i < currentGamestate.counters.Length; i++)
        {
            float f = i*0.5f;
            GameObject y = Instantiate(counterPrefab, counterParent.position + new Vector3(f,0,0), counterParent.rotation, counterParent);
            counterController CC = y.GetComponent<counterController>();
            counterObjects.Add(y);
            CC.Init(currentGamestate.counters[i].owner,currentGamestate.counters[i].visibility,currentGamestate.counters[i].value,currentGamestate.counters[i].label,currentGamestate.counters[i].displayName,currentGamestate.counters[i].actionRoles,currentGamestate.counters[i].location);
        }
    }

    public void drawPiles()
    {
        foreach(Transform child in pileParent) 
        {
            Destroy(child.gameObject);
        }
        float fy = 0f;
        float fx = 0f;
        for(int i = 0; i < currentGamestate.piles.Length; i++)
        {
            if(i%16 == 0 && i != 0)
            {
                fy -= 2f;
                fx = 0f;
            }
            GameObject y = Instantiate(pilePrefab, pileParent.position + new Vector3(fx,fy,0), pileParent.rotation, pileParent);
            pileController PC = y.GetComponent<pileController>();
            pileObjects.Add(y);
            PC.Init(currentGamestate.piles[i].owner,currentGamestate.piles[i].visibility,currentGamestate.piles[i].cards,currentGamestate.piles[i].location,currentGamestate.piles[i].label,currentGamestate.piles[i].displayName,currentGamestate.piles[i].actionRoles);
            fx += 1f;
        }
    }

    public void drawButtons()
    {
        foreach(Transform child in buttonParent) 
        {
            Destroy(child.gameObject);
        }
        float fx = 0f;
        for(int i = 0; i < currentGamestate.buttons.Length; i++)
        {
            GameObject y = Instantiate(buttonPrefab, buttonParent.position + new Vector3(fx,0,0), buttonParent.rotation, buttonParent);
            buttonController BC = y.GetComponent<buttonController>();
            buttonObjects.Add(y);
            BC.Init(currentGamestate.buttons[i].owner,currentGamestate.buttons[i].visibility,currentGamestate.buttons[i].label,currentGamestate.buttons[i].displayName,currentGamestate.buttons[i].actionRoles,currentGamestate.buttons[i].range,currentGamestate.buttons[i].location);
            fx += 1f;
        }
    }

    public void endGame()
    {
        foreach(Transform child in counterParent) 
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in pileParent) 
        {
            Destroy(child.gameObject);
        }
    }
}
