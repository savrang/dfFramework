using UnityEngine;
using System.Collections;

public class dfItem : dfObject 
{
    int _level = 1;

    public dfItem(dfUnityObject dfObj, string name) : base(dfObj, name)
    {        
    }

    public override bool LoadItemInfoFromDB()
    {
        return true;
    }
}
