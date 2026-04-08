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
    private float startingY;
    public List<GameObject> buttons;

    void Start()
    {
        startingX = transform.position.x;
        startingY = transform.position.y;
    }

    void Update()
    {
    }

    public void updateGameList(string gameList)
    {
        drawnButtons = true;
        gameList = gameList.Replace('"', '\'');
        gameList = gameList[1..^1];
        Debug.Log(gameList);
        DynaButton[] gL = JsonConvert.DeserializeObject<DynaButton[]>(gameList);
        foreach(var DB in gL)
        {
            Debug.Log(DB.toString());
        }
        drawHostButtons(gL);
    }

    public void drawHostButtons(DynaButton[] games)
    {
        for(int i = 0; i < games.Length; i++)
        {
            //Debug.Log("Drawing Buttons..." + games.Count);
            GameObject newButton = Instantiate(gameButton,buttonParent);
            buttons.Add(newButton);
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

    public void destroyButtons()
    {
        counter = 0;
        transform.position = new Vector3(startingX,startingY,0);
        foreach(GameObject b in buttons)
        {
            Debug.Log("destroying button");
            Destroy(b);
        }
    }
}
