using UnityEngine;
using System.Collections;

public class GameDataManager : Singleton<GameDataManager>
{
    [SerializeField]
    private SubjectDatabaseAsset m_SubjectDatabase;
    [SerializeField]
    private ClueDatabaseDataAsset m_ClueDatabase;
    private static PlayerStateData mPlayerState;

    public SubjectDatabaseAsset SubjectDatabase { get { return m_SubjectDatabase; } }
    public ClueDatabaseDataAsset ClueDatabase { get { return m_ClueDatabase; } }

    public PlayerStateData PlayerState
    {
        get
        {
            return mPlayerState;
        }
        set
        {
            mPlayerState = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mPlayerState = new PlayerStateData(); //TODO: Creating a dummy state right now, need to load saved state
    }
}
