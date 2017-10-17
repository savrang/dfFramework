using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class dfInterfaceManager : msMonoSingleton<dfInterfaceManager>
{
    public GameObject _panelPatch;

    float _per = 0.0f;
	// Use this for initialization
	void Start () 
    {
        //DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (dfGameManager.Instance._gameState == dfDefine.GameState.GAMESTATE_PATCH)
        {
            _per += 0.001f;
            UpdatePatchProgressBar(_per);
        }
	}

    public void UpdatePatchStateString(string str)
    {
        Text label = _panelPatch.GetComponentInChildren<Text>();
        label.text = str;
    }

    public void UpdatePatchProgressBar(float percent)
    {
        Slider progress = _panelPatch.GetComponentInChildren<Slider>();
        progress.value = percent;
    }

    public void SelectCharacterFromLobby()
    {
        dfGameManager.Instance.OnEnterInGame("Peanut");
    }
}
