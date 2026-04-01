using UnityEngine;

public class deckController : MonoBehaviour
{
    public int size;
    public int displayType;
    public bool hidden;
    public CardController childPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        for(int i = 0; i < size; i++)
        {
            if(displayType == 0)
            {
                CardController newCard = Instantiate(childPrefab, new Vector3(x,y,0), new Quaternion(0,0,0,0), this.transform);
                newCard.Initialize(hidden);
                x += 0.005f;
                y += 0.005f;
            }
            if(displayType == 1)
            {
                CardController newCard = Instantiate(childPrefab, new Vector3(x,y,0), new Quaternion(0,0,0,0), this.transform);
                newCard.Initialize(hidden);
                x += 0.5f;
            }
        }
    }
}
