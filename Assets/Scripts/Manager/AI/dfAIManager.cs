using UnityEngine;
using System.Collections;

public class dfAIManager : msSingleton<dfAIManager>
{
    protected int _maxAIObject = dfDefine.MAX_AI_OBJECT;
    protected int _curAIObject = 0;

    protected int _playerLayerMask = -1;

    public GameObject _plyer
    {
        get; set;
    }

    public int GetPlayerLayerMask()
    {
        if (_playerLayerMask == -1)
            _playerLayerMask = LayerMask.NameToLayer("Player");

        return _playerLayerMask;
    }
    

    public void SetMaxAIObject(int max)
    {
        _maxAIObject = max;
    }

    public int GetMaxAIObject()
    {
        return _maxAIObject;
    }

    public int CalculateAiObject(int spotMax)
    {
        int max = _maxAIObject - _curAIObject;
        if ( (spotMax == 0) || (max <= 0) )
            return 0;

        max = (max <= spotMax) ? max : spotMax;
        int cnt = Random.Range(1, max + 1);

        _curAIObject += cnt;
        if (_curAIObject > _maxAIObject)
        {
            cnt = _maxAIObject - _curAIObject;
            _curAIObject = _maxAIObject;
        }

        return cnt;

    }

    public void DestroyAiObject()
    {
        _maxAIObject++;
    }
}
