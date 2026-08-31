using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.UI;

public class buttonCreator : MonoBehaviour
{
    public GameObject gameButton;
    public float incX;
    public float incY;
    public int cols;
    private int counter;
    public int rows;
    public Transform buttonParent;
    public string gameList;
    private float startingX;
    private float startingY;
    public RectTransform buttonPane;
    public List<GameObject> buttons;
    public Scrollbar sb;

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
        gameList = gameList.Replace('"', '\'');
        gameList = gameList[1..^1];
        //Debug.Log(gameList);
        DynaButton[] gL = JsonConvert.DeserializeObject<DynaButton[]>(gameList);
        drawHostButtons(gL);
    }

    public void drawHostButtons(DynaButton[] games)
    {
        transform.position = new Vector3(startingX,startingY,0);
        counter = 0;
        rows = 1;
        buttonPane.sizeDelta = new Vector2(1375.5f, 840);
        foreach (Transform child in buttonParent) 
        {
            Destroy(child.gameObject);
        }
        if(games.Length/5 > 4)
        {
            buttonPane.sizeDelta = new Vector2(1400f, 700 + 300*(games.Length/5-4));
            sb.value = 1;
        }
        for(int i = 0; i < games.Length; i++)
        {
            //Debug.Log("Drawing Buttons..." + games.Length);
            GameObject newButton = Instantiate(gameButton,buttonParent);
            buttons.Add(newButton);
            dynamicButton dB = newButton.GetComponent<dynamicButton>();
            dB.Init(games[i].getName(),games[i].getId(),transform.position);
            transform.position += new Vector3(incX,0,0);
            counter ++;
            if(counter == cols)
            {
                counter = 0;
                rows ++;
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
            //Debug.Log("destroying button");
            Destroy(b);
        }
    }
}
