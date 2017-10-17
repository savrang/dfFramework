using System.Collections;

public class dfObject 
{
    dfUnityObject _dfUnityObject = null;

    public string _name { get; set; }

    public delegate void OnMouseEvent();
    public static OnMouseEvent _delegateMouseEvent;

    public dfObject() { }

    public dfObject(dfUnityObject dfObj, string name)
    {
        _dfUnityObject = dfObj;
        _name = name;
    }

    public virtual bool LoadItemInfoFromDB()
    {
        return true;
    }

    public virtual void Release()    { }
}
