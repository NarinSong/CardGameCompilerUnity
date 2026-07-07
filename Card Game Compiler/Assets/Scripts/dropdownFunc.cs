using UnityEngine;

public class dropdownFunc : MonoBehaviour
{
    public editorController eC;
    void Start()
    {
        eC = GameObject.Find("EditorController").GetComponent<editorController>();        
    }

    public void changeStep()
    {
        eC.setStepList();
    }
}
