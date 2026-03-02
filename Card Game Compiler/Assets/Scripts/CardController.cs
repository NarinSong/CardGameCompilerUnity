using UnityEngine;

public class CardController : MonoBehaviour
{
    public int value;
    public int suit;
    public bool hidden;
    public bool isJoker;
    public int backValue;
    private SpriteRenderer spR;
    public Sprite[] cardSprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spR = GetComponent<SpriteRenderer>();
        updateCard(value,suit,hidden,isJoker);
    }

    // Update is called once per frame in constant time
    void FixedUpdate()
    {

    }

    //updates itself based on its native values
    public void selfUpdate()
    {
        updateCard(value,suit,hidden,isJoker);
    }

    /*
    VALID RANGES
    newValue 0-12
    newSuit 0-3
    (newSuit if newJoker is true) 0-1

    changes the card to a new sprite based on the given input
    if newHidden is true it will hide the card and all other values are overlooked.
    if newJoker is true it will change the card to joker colored based on newSuit and ingnore newValue
    if the other two are false it will change the card face to the given value and suit.
    */

    public bool updateCard(int newValue, int newSuit, bool newHidden, bool newJoker)
    {
        if(newValue >= 13 || newSuit >= 4 || backValue >= 5 || (newJoker && (newSuit >= 2)))
        {
            Debug.Log("Error Out of bounds");
            return false;
        }

        hidden = newHidden;
        if(newHidden == true)
        {
            spR.sprite = cardSprites[54+backValue];
            value = 0;
            suit = 0;
        }

        else if(newJoker == true)
        {
            spR.sprite = cardSprites[52+newSuit];
            value = 0;
            suit = newSuit;
        }

        else
        {
            spR.sprite = cardSprites[newValue*newSuit];
            value = newValue;
            suit = newSuit;
        }
        return true;
    }
}
