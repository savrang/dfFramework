using UnityEngine;
using System.Collections;

public class dfGameManager : msMonoSingleton<dfGameManager> //msSingleton<dfGameManager>
{
    public dfDefine.GameState _gameState { get; set;  }

    public delegate void OnUpdateScene();
    public OnUpdateScene _delegateUpdateScene;

    protected float _interval;
    protected string _selectCharacterName;

    public override void Init()
    {
        _gameState = dfDefine.GameState.GAMESTATE_NONE;// GAMESTATE_LAUNCH;
        Object.DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (_gameState == dfDefine.GameState.GAMESTATE_LAUNCH)
        {
            _gameState = dfDefine.GameState.GAMESTATE_LOGO;

            _delegateUpdateScene += OnUpdateLogoScene;
            _interval = 0.0f;
        }
    }

    void Update()
    {
        if (_delegateUpdateScene != null)
            _delegateUpdateScene();
    }

    void OnUpdateLogoScene()
    {
        _interval += Time.deltaTime;

        if (_interval >= dfDefine.INTERVAL_LOGO)
        {
            _delegateUpdateScene -= OnUpdateLogoScene;
            Application.LoadLevel(1);//"Patch"

            _gameState = dfDefine.GameState.GAMESTATE_PATCH;


            //_delegateUpdateScene += PatchStart;
            //_delegateUpdateScene += OnUpdatePatchScene;
        }
    }


    void PatchStart()
    {
        //_delegateUpdateScene -= PatchStart;

        //dfAssetManager.Instance.PatchAssetBundles();
    }

    public void PatchEnd()
    {
        _gameState = dfDefine.GameState.GAMESTATE_LOBBY;

        Application.LoadLevel(2);//"Patch"
    }

    void OnUpdatePatchScene()
    {
    }

    public void OnEnterInGame(string selectCharacterName)
    {
        _selectCharacterName = selectCharacterName;

        _gameState = dfDefine.GameState.GAMESTATE_INGAME;

        Application.LoadLevel(3);
    }


    void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 1:
                {
                    dfAssetManager.Instance.PatchAssetBundles();
                }
                break;
        }
    }
}
