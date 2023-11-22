using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, IPublisher
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        Screen.SetResolution(1920, 1080, true);
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1f;
        }

        Broker.Clear();
        StartGame();
    }

    public void StartGame()
    {
        FindManager();
        InitManagers();
        if(isAutoStart)
            bossMng.GameStart();
    }

    private void FindManager()
    {
        audioMng = FindFirstObjectByType<AudioManager>();
        bossMng = FindFirstObjectByType<BossManager>();
        uiMng = FindFirstObjectByType<UIManager>();
        camMng = FindFirstObjectByType<CameraManager>();
        playerMng = FindFirstObjectByType<PlayerManager>();
        obstacleMng = FindFirstObjectByType<ObstacleManager>();
        marbleMng = FindFirstObjectByType<MarbleManager>();
        pauseMng = FindFirstObjectByType<PauseManager>();
    }

    private void InitManagers()
    {

        audioMng.Init();
        uiMng.Init(pauseMng.TogglePause);
        camMng.Init(playerTr, playerMng.PData, ActionFinish);
        bossMng.Init(playerTr, CameraAction, value => { uiMng.BossHpUpdate(value); }, value => { uiMng.BossShieldUpdate(value); }, () => { uiMng.RemoveBossShield(); }, GetRandomSpawnPoint, BossClear);
        playerMng.Init(value => { uiMng.UpdateSp(value); }, value => { uiMng.UpdateHp(value); }, audioMng.PlayPlayerAudio, GameOver); 
        obstacleMng.Init();
        marbleMng.Init();
        pauseMng.Init(value => { uiMng.SetPauseManger(value); }); 
    }

    public void CameraAction(int _curPhaseNum)
    {
        camMng.CameraAction(_curPhaseNum);
    }

    public void ActionFinish(bool _isGameClear = false)
    {
        if (!_isGameClear)
            bossMng.ActionFinish();
        else
        {
            uiMng.GameClear();
            pauseMng.SetGameOver(true);
        }
    }

    public void RegisterBroker()
    {
        Broker.Regist(EPublisherType.GAME_MANAGER);
    }

    public void GameOver()
    {
        uiMng.GameOver();
        pauseMng.SetGameOver(true);
    }
    public void PushMessageToBroker(EMessageType _message)
    {
        Broker.AlertMessageToSub(_message, EPublisherType.GAME_MANAGER);
    }

    public BossShieldGeneratorSpawnPoint[] GetRandomSpawnPoint()
    {
        return obstacleMng.GetRandomSpawnPoint();
    }

    private void BossClear()
    {
        camMng.CameraAction(3);
    }

    private void Update()
    {
        if (Input.GetKeyDown(nextPhaseKeyCode))
            bossMng.ClearCurPhase();
        else if (Input.GetKeyDown(startGameKeyCode))
            bossMng.GameStart();
        else if (Input.GetKeyDown(jumpToNextPatternKeyCode))
            bossMng.JumpToNextPattern();
        else if (Input.GetKeyDown(finishGameKeyCode))
            bossMng.FinishiDebug();
    }

    [SerializeField]
    private Transform playerTr = null;
    [SerializeField]
    private KeyCode startGameKeyCode = KeyCode.Home;
    [SerializeField]
    private KeyCode nextPhaseKeyCode = KeyCode.PageUp;
    [SerializeField]
    private KeyCode jumpToNextPatternKeyCode = KeyCode.PageDown;
    [SerializeField]
    private KeyCode finishGameKeyCode = KeyCode.Backspace;

    [SerializeField]
    private bool isAutoStart = false;

    private AudioManager audioMng = null;
    private BossManager bossMng = null;
    private UIManager uiMng = null;
    private CameraManager camMng = null;
    private PlayerManager playerMng = null;
    private ObstacleManager obstacleMng = null;
    private PauseManager pauseMng = null;
    private MarbleManager marbleMng = null;

}
