using UnityEngine;
using TMPro;

public class lobbyCode : MonoBehaviour
{
    public TMP_InputField field;

    void Awake()
    {
        field.onValidateInput = ValidateInput;
    }

    char ValidateInput(string text, int charIndex, char addedChar)
    {
        return char.ToUpper(addedChar);
    }
}