using UnityEngine;
using System.Collections;

public class dfCharacter : dfObject
{
    protected float _maxHP = 10.0f;

    public dfCharacter() { }

    public dfCharacter(dfUnityObject dfObj, string name) : base(dfObj, name)
    {
    }


    public virtual float GetMaxHp() { return _maxHP; }



    public override bool LoadItemInfoFromDB()
    {
        return true;
    }

    

  
}
