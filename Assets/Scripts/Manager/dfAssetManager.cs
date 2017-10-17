using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class dfAssetManager :  msSingleton<dfAssetManager>
{
    Dictionary<string, GameObject> _assetDictionary;
    Dictionary<string, List<dfUnityObject>> _assetListDictionary;

    public dfAssetManager()
    {
        _assetDictionary = new Dictionary<string, GameObject>();
        _assetListDictionary = new Dictionary<string, List<dfUnityObject>>();

        if (Caching.maximumAvailableDiskSpace != 209715200)
        {
            Caching.maximumAvailableDiskSpace = 209715200; //200mb size
        }
    }

    public void PatchAssetBundles()
    {
        dfInterfaceManager.Instance.UpdatePatchStateString("업데이트 준비 중입니다. 잠시만 기다려주세요.");
        dfAssetBundleManager.Instance.CheckPatchInfo();
        dfAssetBundleManager.Instance.LoadBundle();
    }

    public GameObject CreateAssetBundleGameObject(string name, GameObject obj)
    {
        GameObject value;
        if(_assetDictionary.TryGetValue(name, out value))
        {
            //출력 - 중복된 오브젝트 이름이 있습니다.
            return value;
        }

        value = GameObject.Instantiate(obj);
        value.name = name;

        _assetDictionary.Add(name, value);

        return value;
    }

    public void GetGameObject(string name, ref GameObject obj)
    {
        _assetDictionary.TryGetValue(name, out obj);
    }

    public void AddGameObject(string name, GameObject obj)
    {
        GameObject value;
        if (_assetDictionary.TryGetValue(name, out value))
        {
            //출력 - 중복된 오브젝트 이름이 있습니다.
            return;
        }

        _assetDictionary.Add(name, obj);
    }

    public void DestroyGameObject(string name)
    {
        GameObject value;
        if (_assetDictionary.TryGetValue(name, out value))
        {
            GameObject.Destroy(value);

            _assetDictionary.Remove(name);
        }
    }

    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    public dfUnityObject PushAsset(string name, GameObject obj, bool isOnly, bool isActive)
    {
        if(isOnly)
        {
            dfUnityObject[] uniObj = FindAsset(name);
            if ( (uniObj != null) && (uniObj.Length > 0))
                return uniObj[0];
        }

        GameObject unityObj = GameObject.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;

        dfUnityObject dfUnityObj = unityObj.GetComponent<dfUnityObject>();
        if (dfUnityObj == null)
        {
            dfUnityObj._name = name;
            unityObj.AddComponent<dfUnityObject>();
        }

        AddAsset(name, dfUnityObj, isOnly);

        dfUnityObj.SetActive(isActive);

        return dfUnityObj;
    }

    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    protected void AddAsset(string name, dfUnityObject obj, bool isOnly)
    {
        List<dfUnityObject> lstObj;
        if (_assetListDictionary.TryGetValue(name, out lstObj) == false)
        {
            lstObj = new List<dfUnityObject>();
            _assetListDictionary.Add(name, lstObj);
        }

        if (isOnly == false)
        {
            lstObj.Add(obj);
        }
    }

    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    protected dfUnityObject[] FindAsset(string name)
    {
        List<dfUnityObject> lstObj;
        if (_assetListDictionary.TryGetValue(name, out lstObj))
        {
            if (lstObj.Count == 0)
                return null;

            return lstObj.ToArray();
        }

        return null;
    }


    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    public dfUnityObject[] PopAsset(string name, bool isActive)
    {
        dfUnityObject[] charObj = FindAsset(name);

        if (charObj == null)
            return null;

        foreach(dfUnityObject obj in charObj)
        {
            obj.SetActive(isActive);
        }

        return charObj;
    }

    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    public void RemoveAssetAllByName(string name)
    {
        _assetListDictionary.Remove(name);     
    }

    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    public void RemoveAssetByGameObject(GameObject obj)
    {
        dfUnityObject dfUnityObj = obj.GetComponent<dfUnityObject>();
        if (dfUnityObj == null)
            return;

        RemoveAssetByUnityObject(dfUnityObj);
    }

    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    public void RemoveAssetByUnityObject(dfUnityObject uniObj)
    {
        List<dfUnityObject> lstObj;
        if (_assetListDictionary.TryGetValue(uniObj._name, out lstObj))
        {
            lstObj.Remove(uniObj);
        }
    }


    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    public void SetActiveAllByName(string name, bool isActive)
    {
        List<dfUnityObject> lstObj;
        if (_assetListDictionary.TryGetValue(name, out lstObj))
        {
            foreach (dfUnityObject obj in lstObj)
            {
                obj.SetActive(isActive);
            }
        }
    }

    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    public void SetActiveByGameObject(GameObject obj, bool isActive)
    {
        dfUnityObject dfUnityObj = obj.GetComponent<dfUnityObject>();
        if (dfUnityObj == null)
            return;

        SetActiveByUnityObject(dfUnityObj, isActive);
    }

    //--------------------------------------------------------------------------------------------
    //
    //--------------------------------------------------------------------------------------------
    public void SetActiveByUnityObject(dfUnityObject uniObj, bool isActive)
    {
        List<dfUnityObject> lstObj;
        if (_assetListDictionary.TryGetValue(uniObj._name, out lstObj))
        {
            foreach (dfUnityObject obj in lstObj)
            {
                obj.SetActive(isActive);
            }
        }
    }
    
}
