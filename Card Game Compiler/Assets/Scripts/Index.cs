using System;
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

public class editorState
{
    public phase[] phases;
    //add meta
}


public class phase
{
    public string name {get; set;}
    public step[] steps;

    public phase(string n)
    {
        name = n;
    }
}

public class step
{
    public string name {get; set;}
    public block[][] blockstate;

    public step(string n)
    {
        name = n;
    }
}

public class user
{
    public string username {get; set;}
    public string displayName {get; set;}
    public string color {get; set;}

    public user(string nameX,string displayX)
    {
        username = nameX;
        displayName = displayX;
    }

    public string toString()
    {
        return username + " : " + displayName;
    }

    public string getName()
    {
        return username;
    }

    public string getDisplay()
    {
        return displayName;
    }
}

public class variablesType
{
    public string vName {get; set;}
    public int type {get; set;}

    public variablesType()
    {
        vName = "NewVariable";
        type = 0;
    }
    public string returnType()
    {
        switch(type)
        {
            case 0:
                return "Number";
            case 1:
                return "String";
            case 2:
                return "Boolean";
            case 3:
                return "PileLabel";
            case 4:
                return "CounterLabel";
            case 5:
                return "ButtonLabel";
            case 6:
                return "ActionRole";
            case 7:
                return "PileState";
            case 8:
                return "Visibility";
            case 9:
                return "Card";
            case 10:
                return "ID";
            case 11:
                return "Player";
            case 12:
                return "PlayerRole";
            case 13:
                return "Phase";
            case 14:
                return "Step";
            case 15:
                return "Location";
            case 16:
                return "ButtonRange";
            case 17:
                return "Rank";
            case 18:
                return "Suit";
            case 19:
                return "Array";

        }
        return "null";
    }
}

public class pilesType
{
    public string pName {get; set;}
    public vis visibility {get; set;}
    public pileState pileState {get; set;}
    public locationsType location {get; set;}
    public locationsType ownerLocation {get; set;}
    public List<string> actionRoles {get; set;}
    public ownership ownership {get; set;}
    public pilesType(locationsType def)
    {
        pName = "NewPile";
        pileState = pileState.SHUFFLED;
        visibility = vis.FACE_UP;
        location = def;
        ownerLocation = def;
        actionRoles = new List<string>();
        ownership = ownership.BOARD;
    }
    public string returnType()
    {
        if(pileState == pileState.EMPTY)
        {
            return "EMPTY";
        }
        else if(pileState == pileState.SHUFFLED)
        {
            return "SHUFFLED";
        }
        return null;
    }

    public string returnVis()
    {
        if(visibility == vis.FACE_DOWN)
        {
            return "FACE_DOWN";
        }
        else if(visibility ==  vis.FACE_UP)
        {
            return "FACE_UP";
        }
        else if(visibility == vis.INVISIBLE)
        {
            return "INVISIBLE";
        }
        else if(visibility == vis.FACE_DOWN_SPREAD)
        {
            return "FACE_DOWN_SPREAD";
        }
        else if(visibility == vis.FACE_UP_SPREAD)
        {
            return "FACE_UP_SPREAD";
        }
        else if(visibility == vis.PRIVATE)
        {
            return "PRIVATE";
        }
        else if(visibility == vis.PRIVATE_SPREAD)
        {
            return "PRIVATE_SPREAD";
        }
        return null;
    }
}
public class buttonsType
{
    public string bName {get; set;}
    public ButtonType type {get; set;}
    public vis visibility {get; set;}
    public locationsType location {get; set;}
    public locationsType ownerLocation {get; set;}
    public List<string> actionRoles {get; set;}
    public rangeObj range {get; set;}
    public ownership ownership {get; set;}
    public buttonsType(locationsType def)
    {
        bName = "NewButton";
        type = ButtonType.CLICK;
        visibility = vis.FACE_UP;
        location = def;
        ownerLocation = def;
        actionRoles = new List<string>();
        range = new rangeObj();
        ownership = ownership.BOARD;
    }
    public string returnType()
    {
        if(type == ButtonType.CLICK)
        {
            return "CLICK";
        }
        else if(type == ButtonType.NUMBER)
        {
            return "NUMBER";
        }
        return null;
    }
    public string returnVis()
    {
        if(visibility == vis.FACE_DOWN)
        {
            return "FACE_DOWN";
        }
        else if(visibility ==  vis.FACE_UP)
        {
            return "FACE_UP";
        }
        else if(visibility == vis.INVISIBLE)
        {
            return "INVISIBLE";
        }
        else if(visibility == vis.PRIVATE)
        {
            return "PRIVATE";
        }
        return null;
    }
}

public class countersType
{
    public string cName {get; set;}
    public float number {get; set;}
    public vis visibility {get; set;}
    public locationsType location {get; set;}
    public locationsType ownerLocation {get; set;}
    public List<string> actionRoles {get; set;}
    public ownership ownership {get; set;}
    public countersType(locationsType def)
    {
        cName = "NewCounter";
        number = 0;
        visibility = vis.FACE_UP;
        location = def;
        ownerLocation = def;
        actionRoles = new List<string>();
        ownership = ownership.BOARD;
    }
    public string returnVis()
    {
        if(visibility == vis.FACE_DOWN)
        {
            return "FACE_DOWN";
        }
        else if(visibility ==  vis.FACE_UP)
        {
            return "FACE_UP";
        }
        else if(visibility == vis.INVISIBLE)
        {
            return "INVISIBLE";
        }
        else if(visibility == vis.PRIVATE)
        {
            return "PRIVATE";
        }
        return null;
    }
}

public class locationsType
{
    public string lName {get; set;}
    public int index {get; set;}
    public float x {get; set;}
    public float y {get; set;}
    public float xOff {get; set;} 
    public float yOff {get; set;}
    public float wrapAt {get; set;}
    public float wrapTo {get; set;}
    public bool editable {get; set;}
    public locationRenderType vertHori {get; set;}
    public locationsType()
    {
        lName = "NewLocation";
        x = 0;
        y = 0;
        index = 0;
        xOff = 0;
        yOff = 0;
        wrapAt = 0;
        wrapTo = 0;
        editable = true;
        vertHori = locationRenderType.HORIZONTAL;
    }
    public string convertVertHori()
    {
        if(vertHori == locationRenderType.VERTICAL)
        {
            return "VERTICAL";
        }
        if(vertHori == locationRenderType.HORIZONTAL)
        {
            return "HORIZONTAL";
        }
        return null;
    }
}

public class LobbyInfo
{
    public user host {get; set;}
    public user[] players {get; set;}
    public string code {get; set;}
    public string game {get; set;}
    public string gameDescription {get; set;}    
}

public class gameInfo
{
    public string name {get; set;}
    public string description {get; set;}
}

public class myGameInfo
{
    public string name {get; set;}
    public string description {get; set;}
    public int id {get; set;}
}

public class block
{
    public string name {get; set;}
    public string displayName {get; set;}
    public string returnType {get; set;}
    public args[] arguments {get; set;}
    public override string ToString()
    {
        string temp = name + " " + displayName + " " + returnType + " ";
        foreach(args x in arguments)
        {
            temp += x.ToString();
        }
        return temp;

    }
}

[Serializable]
public class args
{
    public string name;
    public string displayName;
    public string type;
    public bool optional;
    public override string ToString()
    {
        return name + " " + displayName + " " + type + " " + optional +", ";
    }

    public args(string n, string t, bool o)
    {
        name = n;
        displayName = n;
        type = t;
        optional = o;
    }
}

public enum ownership
{
    PLAYER,
    BOARD
}

public enum pileState
{
    EMPTY,
    SHUFFLED

}

public enum vis
{
    FACE_DOWN,
    FACE_UP,
    INVISIBLE,
    FACE_UP_SPREAD,
    FACE_DOWN_SPREAD,
    PRIVATE,
    PRIVATE_SPREAD
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

public enum locationRenderType
{
    VERTICAL,
    HORIZONTAL
}

public enum triggerType
{
    AUTO,
    CLICK,
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
    public rangeObj()
    {
        min = float.NaN;
        max = float.NaN;
        increment = 0;
    }

    public override string ToString()
    {
        return "min: " + min + " max: " + max + " inc: " + increment;
    }
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

public class gameExport
{
    public gameMeta gameMeta;
    public Dictionary<string, dynamic> playerDefinition;
    public Dictionary<string, dynamic> boardDefinition;
    public List<phaseExport> phases {get; set;}
    public gameExport()
    {
        gameMeta = new();
        playerDefinition = new();
        boardDefinition = new();
        phases = new();
    }
}

public class gameMeta
{
    public int minPlayers {get; set;}
    public int maxPlayers {get; set;}
    public string name {get; set;}
    public string description {get; set;}
    public Dictionary<string, string> variables;
    public Dictionary<string, dynamic> locations;
    public gameMeta()
    {
        variables = new();
        locations = new();
    }
}

public class phaseExport
{
    public string name {get; set;}
    public List<stepExport> steps {get; set;}
    public phaseExport()
    {
        steps = new();
    }
}

public class stepExport
{
    public string name {get; set;}
    public List<actionExport> actions {get; set;}
    public stepExport()
    {
        actions = new();
    }
}

public class actionExport
{
    public Dictionary<string, dynamic> trigger;
    public dynamic filter;
    public Dictionary<string, dynamic> result;
    public actionExport()
    {
        trigger = new();
        result = new();
    }
}