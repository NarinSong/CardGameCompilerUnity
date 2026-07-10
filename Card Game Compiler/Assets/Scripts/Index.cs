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

public class pilesType
{
    public string pName {get; set;}
    public int type {get; set;}
    public pilesType()
    {
        pName = "NewPile";
        type = 0;
    }
}

public class buttonsType
{
    public string bName {get; set;}
    public int type {get; set;}
    public buttonsType()
    {
        bName = "NewButton";
        type = 0;
    }
}

public class countersType
{
    public string cName {get; set;}
    public rangeObj range {get; set;}
    public countersType()
    {
        cName = "NewCounter";
        range = new rangeObj();
    }
}

public class locationsType
{
    public string lName {get; set;}
    public float x {get; set;}
    public float y {get; set;}
    public float xOff {get; set;} 
    public float yOff {get; set;}
    public float wrapAt {get; set;}
    public float wrapTo {get; set;}
    public locationRenderType vertHori {get; set;}
    public locationsType()
    {
        x = 0;
        y = 0;
        xOff = 0;
        yOff = 0;
        wrapAt = 0;
        wrapTo= 0;
        vertHori = locationRenderType.HORIZONTAL;
    }
}

public class LobbyInfo
{
    public user host {get; set;}
    public user[] players {get; set;}
    public string code {get; set;}
    public string game {get; set;}    
}

public class gameInfo
{
    public string name {get; set;}
    public string description {get; set;}
}

public class block
{
    public string name {get; set;}
    public string displayName {get; set;}
    public string returnType {get; set;}
    public args[] arguments {get; set;}
    public override string ToString()
    {
        return name + " " + displayName + " " + returnType + " " + arguments + "\n";
    }
}

public class args
{
    public string name {get; set;}
    public string displayName {get; set;}
    public string type {get; set;}
    public bool optional {get; set;}
    public override string ToString()
    {
        return name + " " + displayName + " " + type + " " + optional +"\n";
    }
}

public enum vis
{
    FACE_DOWN,
    FACE_UP,
    INVISIBLE
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