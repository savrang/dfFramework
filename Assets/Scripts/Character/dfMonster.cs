using UnityEngine;
using System.Collections;

public class dfMonster : dfCharacter
{
    //몬스터가 반응하는 플레이어와의 거리
    protected float _AiDistance = 10.0f;
    protected float _attackDelay = 2.0f;
    protected float _hp = 10.0f;

    public dfMonster() { }

    public dfMonster(dfUnityObject dfObj, string name) : base(dfObj, name)
    {
    }

    public override bool LoadItemInfoFromDB()
    {
        return true;
    }

    public float GetAiDistance() { return _AiDistance; }
    public float GetAttackDelay() { return _attackDelay; }
}
