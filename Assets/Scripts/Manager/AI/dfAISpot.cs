using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class dfAISpot : MonoBehaviour
{
    public List<GameObject> _aiObjectList;
    public int _maxSpotAiObject = dfDefine.MAX_AI_OBJECT;

    List<dfMonsterUnityObject> _monObject = new List<dfMonsterUnityObject>();

	// Use this for initialization
	void Start ()
    {
        int cnt = dfAIManager.Instance.CalculateAiObject(_maxSpotAiObject);

        for (int i = 0; i < cnt; i++)
        {
            GameObject obj = _aiObjectList[Random.Range(0, _aiObjectList.Count)];

            dfMonsterUnityObject mon = dfCharacterManager.Instance.CreateMonster(obj.name, obj, true, false);
            
            _monObject.Add(mon);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
            return;

        
        for (int i = 0; i < _monObject.Count; i++)
        {
            dfMonsterUnityObject uObj = _monObject[i];
            uObj.SetActive(true);

            if (uObj.gameObject != null)
            {
                uObj.gameObject.transform.position = transform.position + new Vector3(Random.Range(-3.0f, 3.0f), 0.0f, Random.Range(-3.0f, 3.0f));
                uObj.gameObject.transform.rotation = transform.rotation;

                _maxSpotAiObject--;
                if(_maxSpotAiObject < 0)
                {
                    _maxSpotAiObject = 0;
                    break;
                }
            }
        }

        _monObject.Clear();
    }


}
