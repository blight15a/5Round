using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFaction : MonoBehaviourPun
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing,
        InitComponentFinished,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] public int playerId;
    [SerializeField] public bool isLocalPlayer;
    [SerializeField] public bool isCom;
    [SerializeField] public Playerboard pBoard_Cp;
    [SerializeField] public Battleboard bBoard_Cp;
    [SerializeField] public Mihariboard mBoard_Cp;
    [SerializeField] public TokensData tokensData = new TokensData();
    [SerializeField] public MarkersData markersData = new MarkersData();

    //-------------------------------------------------- public fields
    [ReadOnly] public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly] public RoundData roundsData = new RoundData();
    [ReadOnly] public BattleInfo battleInfo = new BattleInfo();
    [ReadOnly] public int playerAp;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    UI_GameCanvas gameUI_Cp;

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public GameState_En mainGameState
    {
        get { return gameStates[0]; }
        set { gameStates[0] = value; }
    }

    public List<Unit_Bb> bbUnit_Cps { get { return bBoard_Cp.bbUnit_Cps; } }
    public List<UnitCardData> mUnitsData { get { return mBoard_Cp.mUnitsData; } }

    //-------------------------------------------------- private properties

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //-------------------------------------------------- Start is called before the first frame update
    void Start()
    {

    }

    //-------------------------------------------------- Update is called once per frame
    void Update()
    {

    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Manage gameStates
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region ManageGameStates

    //--------------------------------------------------
    public void AddMainGameState(GameState_En value = GameState_En.Nothing)
    {
        if (gameStates.Count == 0)
        {
            gameStates.Add(value);
        }
    }

    //--------------------------------------------------
    public void AddGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value_tp in values)
        {
            gameStates.Add(value_tp);
        }
    }

    //--------------------------------------------------
    public bool ExistGameStates(params GameState_En[] values)
    {
        bool result = true;
        foreach (GameState_En value in values)
        {
            if (!gameStates.Contains(value))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public bool ExistAnyGameStates(params GameState_En[] values)
    {
        bool result = false;
        foreach (GameState_En value in values)
        {
            if (gameStates.Contains(value))
            {
                result = true;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public int GetExistGameStatesCount(GameState_En value)
    {
        int result = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == value)
            {
                result++;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public void RemoveGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value in values)
        {
            gameStates.RemoveAll(gameState_tp => gameState_tp == value);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Initialize
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        StartCoroutine(Corou_Init());
    }

    IEnumerator Corou_Init()
    {
        AddMainGameState(GameState_En.Nothing);

        //
        SetComponents();
        InitUI();
        InitTokensData();
        InitMarkersData();

        InitComponents();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.InitComponentFinished));
        RemoveGameStates(GameState_En.InitComponentFinished);

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        gameUI_Cp = controller_Cp.ui_gameCanvas_Cp;
    }

    //--------------------------------------------------
    void InitUI()
    {
        if (PhotonNetwork.IsConnected)
        {
            gameUI_Cp.SetPlayerName(playerId, PhotonNetwork.PlayerList[playerId].NickName);
        }
    }

    //--------------------------------------------------
    void InitTokensData()
    {
        
    }

    //--------------------------------------------------
    void InitMarkersData()
    {
        
    }

    //--------------------------------------------------
    void InitComponents()
    {
        StartCoroutine(Corou_InitComponents());
    }

    IEnumerator Corou_InitComponents()
    {
        pBoard_Cp.Init(this);
        yield return new WaitUntil(() => pBoard_Cp.mainGameState == Playerboard.GameState_En.Inited);

        bBoard_Cp.Init(this);
        yield return new WaitUntil(() => bBoard_Cp.mainGameState == Battleboard.GameState_En.Inited);

        mBoard_Cp.Init(this);

        AddGameStates(GameState_En.InitComponentFinished);
    }

    #endregion

    //--------------------------------------------------
    public void Play()
    {
        mainGameState = GameState_En.Playing;
    }

}