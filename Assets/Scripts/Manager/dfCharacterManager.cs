using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class dfCharacterManager : msSingleton<dfCharacterManager>
{
    Dictionary<string, dfCharacter> _characterDictionary;

    public dfCharacterManager()
    {
        _characterDictionary = new Dictionary<string, dfCharacter>();
    }

    public dfCharacterUnityObject CreateCharacter(string name, GameObject obj, bool isOnly, bool isActive)
    {
        dfCharacterUnityObject dfCharUnityObj = (dfCharacterUnityObject)dfAssetManager.Instance.PushAsset(name, obj, isOnly, isActive);
        if(dfCharUnityObj)
        {
            dfCharacter ch = GetCharacterProperty(name, dfCharUnityObj);
            dfCharUnityObj.CreatedByCharacterManager(ch);
        }

        return dfCharUnityObj;
    }

    public void DestroyCharacterByGameObject(GameObject obj, bool isDelete)
    {
        dfAssetManager.Instance.RemoveAssetByGameObject(obj);
    }




    public dfMonsterUnityObject CreateMonster(string name, GameObject obj, bool isOnly, bool isActive)
    {
        dfMonsterUnityObject dfMonUnityObj = (dfMonsterUnityObject)dfAssetManager.Instance.PushAsset(name, obj, isOnly, isActive);
        if (dfMonUnityObj)
        {
            dfMonster mon = GetMonsterProperty(name, dfMonUnityObj);
            dfMonUnityObj.CreatedByCharacterManager(mon);
        }

        return dfMonUnityObj;
    }

    public dfCharacter GetCharacterProperty(string name, dfUnityObject obj)
    {
        dfCharacter value;
        if (_characterDictionary.TryGetValue(name, out value))
        {
            return value;
        }

        value = new dfCharacter(obj, obj.name);

        _characterDictionary.Add(name, value);

        return value;
    }

    public dfMonster GetMonsterProperty(string name, dfUnityObject obj)
    {
        dfCharacter value;
        if (_characterDictionary.TryGetValue(name, out value))
        {
            //출력 - 중복된 오브젝트 이름이 있습니다.
            return (dfMonster)value;
        }

        value = new dfMonster(obj, obj.name);

        _characterDictionary.Add(name, value);

        return (dfMonster)value;
    }    

    /*
    protected void DestroyCharacter(string name)
    {
        dfCharacter value;
        if (_characterDictionary.TryGetValue(name, out value))
        {
            value.Release();

            _characterDictionary.Remove(name);
        }
    }
    */
}
