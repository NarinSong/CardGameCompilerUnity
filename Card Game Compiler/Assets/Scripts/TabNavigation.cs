using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TabNavigation : MonoBehaviour
{
    public Selectable[] fields;
    public Selectable[] preAuthMain;
    public Selectable[] postAuthMain;
    public Selectable[] signUp;
    public Selectable[] signIn;
    public Selectable[] credits;
    public Selectable[] lobbyHost;
    public Selectable[] lobbyHostGame;
    public Selectable[] lobbyUser;
    public Selectable[] join;
    public Selectable[] error;
    public Selectable[] selectGame;
    public Selectable[] editorContainer;
    public Selectable[] editor;
    public Selectable[] game;
    public Selectable[] gameMenu;
    public Selectable[] editorMenu;
    public Selectable[] userSettings;
    public Selectable[] var;
    public Selectable[] lobbyVar;
    public Button signUpB;
    public Button signInB;
    public Button userSettingsB;
    public string selectedField;

    void Start()
    {
        var = fields;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool reverse = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            GameObject current = EventSystem.current.currentSelectedGameObject;
            int index = -1;

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] != null && fields[i].gameObject == current)
                {
                    index = i;
                    break;
                }
            }

            int nextIndex;
            if (index == -1)
            {
                nextIndex = 0;
            }
            else if (reverse)
            {
                nextIndex = (index - 1 + fields.Length) % fields.Length;
            }
            else
            {
                nextIndex = (index + 1) % fields.Length;
            }

            Selectable next = fields[nextIndex];
            if (next != null)
            {
                next.Select();

                TMP_InputField tmpField = next.GetComponent<TMP_InputField>();
                if (tmpField != null)
                {
                    tmpField.ActivateInputField();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(selectedField == "signIn")
            {
                signInB.onClick.Invoke(); 
            }
            else if(selectedField == "signUp")
            {
                signUpB.onClick.Invoke(); 
            }
            else if(selectedField == "userSettings")
            {
                userSettingsB.onClick.Invoke(); 
            }
        }
    }

    public void swapFields(string newFields)
    {
        selectedField = newFields;
        if(newFields != "var")
        {
            var = fields;
        }
        switch(newFields)
        {
            case "preAuth":
                fields = preAuthMain;
                break;
            case "postAuth":
                fields = postAuthMain;
                break;
            case "signUp":
                fields = signUp;
                break;
            case "signIn":
                fields = signIn;
                break;
            case "credits":
                fields = credits;
                break;
            case "lobbyHost":
                fields = lobbyHost;
                lobbyVar = lobbyHost;
                break;
            case "lobbyHostGame":
                fields = lobbyHostGame;
                lobbyVar = lobbyHostGame;
                break;
            case "lobbyUser":
                fields = lobbyUser;
                lobbyVar = lobbyHostGame;
                break;
            case "join":
                fields = join;
                break;
            case "error":
                fields = error;
                break;
            case "selectGame":
                fields = selectGame;
                break;
            case "editorContainer":
                fields = editorContainer;
                break;
            case "editor":
                fields = editor;
                break;
            case "game":
                fields = game;
                break;
            case "gameMenu":
                fields = gameMenu;
                break;
            case "editorMenu":
                fields = editorMenu;
                break;
            case "userSettings":
                fields = userSettings;
                break;
            case "var":
                fields = var;
                break;
            case "lobbyVar":
                fields = lobbyVar;
                break;

        }
        bool reverse = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            GameObject current = EventSystem.current.currentSelectedGameObject;
            int index = -1;

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] != null && fields[i].gameObject == current)
                {
                    index = i;
                    break;
                }
            }

            int nextIndex;
            if (index == -1)
            {
                nextIndex = 0;
            }
            else if (reverse)
            {
                nextIndex = (index - 1 + fields.Length) % fields.Length;
            }
            else
            {
                nextIndex = (index + 1) % fields.Length;
            }

            Selectable next = fields[nextIndex];
            if (next != null)
            {
                next.Select();

                TMP_InputField tmpField = next.GetComponent<TMP_InputField>();
                if (tmpField != null)
                {
                    tmpField.ActivateInputField();
                }
            }
    }
}