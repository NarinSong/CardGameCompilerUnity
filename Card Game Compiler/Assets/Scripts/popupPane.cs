using TMPro;
using UnityEngine;

public class popupPane : MonoBehaviour
{
    public float timer;
    public float timerStart;
    public bool timerActive;
    public TMP_Text tb;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timerActive)
        {
            timer -= 1;
            if(timer < 0)
            {
                ResetPanel();
            }
        }
    }

    public void startPanel(string text)
    {
        if(timerActive == true)
        {
            tb.text = text + "\n" + tb.text;
            timer = timerStart;
        }
        else
        {
            tb.text = text;
            timer = timerStart;
            timerActive = true;
        }
    }

    public void ResetPanel()
    {
        timerActive = false;
        this.gameObject.SetActive(false);
    }
}
