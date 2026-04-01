using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

public class DynaButton
{
    public int id {get; set;}
    public string name {get; set;}

    public DynaButton(string gName, int Gid)
    {
        id = Gid;
        name = gName;
    }

    public string toString()
    {
        return name + " : " + id;
    }

    public string getName()
    {
        return name;
    }

    public int getId()
    {
        return id;
    }
}

public class buttonCreator : MonoBehaviour
{
    public GameObject gameButton;
    public float incX;
    public float incY;
    public int cols;
    private int counter;
    public Transform buttonParent;
    public string gameList;
    public bool drawnButtons;
    private float startingX;

    void Start()
    {
        startingX = transform.position.x;
    }

    void Update()
    {
        if(drawnButtons == false)
        {
            drawnButtons = true;
            gameList = @"[{'name':'War','id':0},{'name':'Pickup','id':1},{'name':'filler','id':2},{'name':'test','id':3},{'name':'this exist','id':4},{'name':'mahjong','id':5}]";
            List<DynaButton> gL = JsonConvert.DeserializeObject<List<DynaButton>>(gameList);
            foreach(var DB in gL)
            {
                Debug.Log(DB.toString());
            }
            drawHostButtons(gL);
        }
    }

    public void updateGameList(string games)
    {
        drawnButtons = false;
        gameList = games;
    }

    public void drawHostButtons(List<DynaButton> games)
    {
        for(int i = 0; i < games.Count; i++)
        {
            Debug.Log("Drawing Buttons..." + games.Count);
            GameObject newButton = Instantiate(gameButton,buttonParent);
            dynamicButton dB = newButton.GetComponent<dynamicButton>();
            dB.Init(games[i].getName(),games[i].getId(),transform.position);
            transform.position += new Vector3(incX,0,0);
            counter ++;
            if(counter == cols)
            {
                counter = 0;
                transform.position = new Vector3(startingX,transform.position.y+incY,0);
            }
        }
    }
}
