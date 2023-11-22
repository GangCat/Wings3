using System;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;
//using Cysharp.Threading.Tasks;

public class BossController : MonoBehaviour, IPublisher
{
    public delegate BossShieldGeneratorSpawnPoint[] GetRandomSpawnPointDelegate();
    private SoundManager soundManager = null;

    public void Init(
        Transform _playerTr, 
        VoidIntDelegate _cameraActionCallback, 
        VoidFloatDelegate _hpUpdateCallback, 
        VoidFloatDelegate _shieldUpdateCallback, 
        GetRandomSpawnPointDelegate _getRandomSpawnPointCallback, 
        VoidVoidDelegate _bossClearCalblack, 
        VoidVoidDelegate _removeShieldCallback)
    {
        soundManager = SoundManager.Instance;
        soundManager.AddAudioComponent(gameObject);
        curPhaseNum = 0;
        animCtrl = GetComponentInChildren<BossAnimationController>();
        bossCollider = GetComponentInChildren<BossCollider>();
        statHp = GetComponent<BossStatusHp>();
        shield = GetComponentInChildren<BossShield>();
        timeBombPatternCtrl = GetComponentInChildren<BombPatternController>();
        bossRb = GetComponent<Rigidbody>();
        lastPatternCtrl = GetComponentInChildren<LastPatternController>();

        getRandomSpawnPointCallback = _getRandomSpawnPointCallback;

        animCtrl.Init();
        bossCollider.Init();
        statHp.Init(StartPhaseChange, _hpUpdateCallback);
        shield.Init(RestorShieldFinish, _shieldUpdateCallback, _removeShieldCallback);
        timeBombPatternCtrl.Init(FinishPhaseChange, value => { isBossStartRotation = value; }, ()=> { animCtrl.ReloadTimeBomb(); }, AlertFirstPatternLaser, value => { eyeGo.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", value * 5); },_playerTr);
        lastPatternCtrl.Init(_bossClearCalblack);
        RegisterBroker();
        InitMemoryPools();

        myRunner = GetComponent<BehaviourTreeRunner>();
        myRunner.Init(_playerTr, bossCollider, giantHomingMissileSpawnTr, arrGroupHomingMissileSpawnPos, this);

        curShieldGeneratorPoint = new List<GameObject>();
        waitFixedUpdate = new WaitForFixedUpdate();

        //InitShieldGeneratorPoint();

        cameraActionCallback = _cameraActionCallback;
        playerTr = _playerTr;
        //myRunner.FinishCurrentPhase();
        //StartPhaseChange();

        //StartCoroutine("UpdateCoroutine");

    }

    public BossShieldGeneratorSpawnPoint[] CurSpawnPoints => arrCurShieldGeneratorSpawnPoints;
    public GameObject GatlingHolder => gatlingHolderGo;
    public GameObject GatlingHead => gatlingHeadGo;
    public GameObject GatlinGun => gatlinGunGo;
    public GameObject AirPush => airPush;
    public Transform GunMuzzle => gunMuzzleTr;
    public CannonMemoryPool CannonMemoryPool => cannonMemoryPool;
    public GroupMissileMemoryPool GroupMissileMemoryPool => groupMissileMemoryPool;
    public GatlinMemoryPool GatlinMemoryPool => gatlinMemoryPool;
    public CannonRainMemoryPool CannonRainMemoryPool => cannonRainMemoryPool;
    public Transform[] FootWindTr => arrFootWindSpawnTr;
    public BossAnimationController BossAnim => animCtrl;
    public GameObject FootWindGo => footWindGo;
    public GameObject SitDownGo => sitDownGo;
    public Transform RotateTr => rotateBody.transform;
    public GameObject[] CannonSoundSpawnGOs => cannonSoundSpawnGOs;
    public GameObject AirPushSoundSpawnGO => airPushSoundSpawnGO;
    public GameObject WindBlowSoundSpawnGO => windBlowSoundSpawnGO;
    public GameObject SitDownSoundSpawnGO => sitDownSoundSpawnGO;
    public GameObject GatlingLaunchSoundSpawnGO => gatlingLaunchSoundSpawnGO;
    public GameObject GatlingRotationSoundSpawnGO => gatlingRotationSoundSpawnGO;
    public GameObject TornadoSoundSpawnGO => tornadoSoundSpawnGO;
    public GameObject GiantTornadeSoundSpawnGO => giantTornadeSoundSpawnGO;

    public void AlertGiantMissileLaunch()
    {
        PushMessageToBroker(EMessageType.GIANT_MISSILE_ALERT);
    }

    private void AlertFirstPatternBomb()
    {
        PushMessageToBroker(EMessageType.FIRST_PATTERN_1_ALERT);
    }

    private void AlertFirstPatternLaser()
    {
        PushMessageToBroker(EMessageType.FIRST_PATTERN_2_ALERT);
    }

    private void AlertLastPattern()
    {
        PushMessageToBroker(EMessageType.LAST_PATTERN_ALERT);
    }

    public void SetBossRotationBoolean(bool _canRotation)
    {
        isBossStartRotation = _canRotation;
    }

    public void GameStart()
    {
        myRunner.FinishCurrentPhase();
        StartPhaseChange();

        StartCoroutine("UpdateCoroutine");
    }

    public void ClearShieldGenerator()
    {
        int curGenCnt = curShieldGeneratorPoint.Count;
        for (int i = 0; i < curGenCnt; ++i)
            curShieldGeneratorPoint[0].GetComponent<IDamageable>().GetDamage(999);

        //foreach (GameObject go in curShieldGeneratorPoint)
        //    go.GetComponent<BossShieldGenerator>().GetDamage(999);
    }

    public void JumpToNextPhase()
    {
        ClearShieldGenerator();
        statHp.GetDamage(statHp.GetMaxHp * 0.51f);
    }

    private void InitMemoryPools()
    {
        cannonRainMemoryPool.Init();
        cannonMemoryPool.Init();
        gatlinMemoryPool.Init();
        groupMissileMemoryPool.Init();

    }

    private IEnumerator UpdateCoroutine()
    {
        float maxHp = statHp.GetMaxHp;

        while (true)
        {
            //보스 페이즈 스위치로 검사 > 각 페이즈에 맞는 브금 재생 3페이즈의 경우 폭풍우 브금 등
            if (isBossStartRotation)
            {
                RotateToTarget();
                PlayRotateSound();
            }
            else
            {
                StopRotateSound();
            }

            myRunner.RunnerUpdate();
            yield return waitFixedUpdate;
        }
    }
    private void PlayRotateSound()
    {
        //로테이트 오디오 소스가 isPlaying인지 검사
        //isPlaying이 false면 로테이트 사운드 루프 재생

    }
    private void StopRotateSound()
    {
        //로테이트 오디오 소스가 isPlaying인지 검사
        //isPlaying이 true면  로테이트 사운드 루프 스탑
    }
    private void RotateToTarget()
    {
        // 플레이어의 위치와 보스의 위치 사이의 벡터를 계산
        Vector3 directionToPlayer = playerTr.position - transform.position;

        // y축 회전을 제외한 방향 벡터를 얻기 위해 y값을 0으로 만듭니다.
        directionToPlayer.y = 0f;

        // 방향 벡터를 사용하여 보스를 회전시킵니다.
        if (curPhaseNum >= 2)
            rotateBody.transform.localRotation *= Quaternion.Euler(new Vector3(0f, autoRotateDegree * Time.deltaTime, 0f));
        else if (directionToPlayer != Vector3.zero)
            rotateBody.transform.rotation = Quaternion.Slerp(rotateBody.transform.rotation, Quaternion.LookRotation(directionToPlayer), rotationSpeed * Time.fixedDeltaTime);

    }

    private void StartPhaseChange()
    {
        // 현재 페이즈 검사 > 알맞는 소리 재생 // 1페이즈 시작 연출 , 2페이즈 시작 연출, 3페이즈 시작 연출
        if (isChangingPhase)
            return;

        myRunner.FinishCurrentPhase();
        myRunner.RunnerUpdate();
        isChangingPhase = true;
        StopCoroutine("ResapwnShieldGeneratorCoroutine");
        foreach (GameObject go in arrModelGo)
            go.layer = LayerMask.NameToLayer("BossInvincible");
        cameraActionCallback?.Invoke(curPhaseNum);
        PushMessageToBroker(EMessageType.PHASE_CHANGE);
        soundManager.StopBGM();
        if (curPhaseNum == 0)
        {
            soundManager.PlayAudio(GetComponent<AudioSource>(),(int)SoundManager.ESounds.PHASECHANGESOUND_01,true,false);
        }
        else if (curPhaseNum == 1)
        {
            shield.StopRestorShield();
            StartCoroutine(TempBossUP());
            soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.PHASECHANGESOUND_02_01, true, false);
        }
        else if(curPhaseNum == 2)
        {
            animCtrl.OpenBodyUnder();
            soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.PHASECHANGESOUND_03, true, false);
        }

    }

    IEnumerator TempBossUP()
    {
        //거대 보스가 움직이는 소리 & 물결 치는 소리 등
        float t = 0f;
        float oriY = transform.position.y; // 60
        float targetY = 370f;
        float startTime = Time.time;
        Vector3 oriPos = transform.position;
        Vector3 targetPos = new Vector3(oriPos.x, targetY, oriPos.z);
        while (t < 1)
        {
            t = (Time.time - startTime) / 3.5f;
            transform.position = Vector3.Lerp(oriPos, targetPos, t);

            yield return new WaitForFixedUpdate();
        }

        animCtrl.BossStandUp();
    }

    public void PatternStart()
    {
        // 연출 종료시 호출
        if (!isChangingPhase)
            return;

        soundManager.PlayBGM(curPhaseNum);
        soundManager.StopAudio(GetComponent<AudioSource>());
        if (curPhaseNum == 1)
        {
            //soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.PHASESOUND_01);
            AlertFirstPatternBomb();
            timeBombPatternCtrl.StartPattern();
            return;
        }
        
        if (curPhaseNum >= 2)
        {
            //soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.PHASESOUND_02);
            AlertLastPattern();
            animCtrl.ResetBoss();
            playerTr.GetComponent<PlayerMovementController>().IsLastPattern = true;
            lastPatternCtrl.StartPattern();
        }

        FinishPhaseChange();
    }


    public void FinishPhaseChange()
    {
        if (!isChangingPhase)
            return;

        isBossStartRotation = false;
        isChangingPhase = false;
        if (curPhaseNum == 0)
        {
            shield.RespawnGenerator();
            InitShieldGeneratorPoint();
        }
        else if (curPhaseNum == 1)
        {
            foreach (GameObject go in arrModelGo)
                go.layer = LayerMask.NameToLayer("BossBody");

            foreach (GameObject go in coreGo)
                go.layer = LayerMask.NameToLayer("Boss");
        }
        else if(curPhaseNum >= 2)
        {
            foreach (GameObject go in arrModelGo)
                go.layer = LayerMask.NameToLayer("Boss");

            isBossStartRotation = true;
        }

        ++curPhaseNum;
        myRunner.StartNextPhase(curPhaseNum);
    }

    private void RestorShieldFinish()
    {
        InitShieldGeneratorPoint();
        myRunner.IsShieldDestroy(false);
    }

    private void InitShieldGeneratorPoint()
    {
        curShieldGeneratorPoint.Clear();
        arrCurShieldGeneratorSpawnPoints = getRandomSpawnPointCallback?.Invoke();

        foreach (BossShieldGeneratorSpawnPoint wp in arrCurShieldGeneratorSpawnPoints)
        {
            wp.Init();
            curShieldGeneratorPoint.Add(Instantiate(bossShieldGeneratorPrefab, wp.GetPos(), Quaternion.identity));
        }

        foreach (GameObject go in curShieldGeneratorPoint)
        {
            go.GetComponent<BossShieldGenerator>().Init(RemoveSheildGeneratorFromList, transform.position);
        }
    }

    private void RemoveSheildGeneratorFromList(GameObject _go)
    {
        curShieldGeneratorPoint.Remove(_go);

        if (curShieldGeneratorPoint.Count < 1)
        {
            PushMessageToBroker(EMessageType.SHIELD_BROKEN);
            foreach (GameObject go in arrModelGo)
                go.layer = LayerMask.NameToLayer("Boss");
            myRunner.IsShieldDestroy(true);
        }

        shield.GeneratorDestroy();
    }

    public void RegisterBroker()
    {
        Broker.Regist(EPublisherType.BOSS_CONTROLLER);
    }

    public void PushMessageToBroker(EMessageType _message)
    {
        Broker.AlertMessageToSub(_message, EPublisherType.BOSS_CONTROLLER);
    }

    [Header("-InformationForContext")]
    [SerializeField]
    private GameObject gatlingHolderGo = null;
    [SerializeField]
    private GameObject gatlingHeadGo = null;
    [SerializeField]
    private GameObject gatlinGunGo = null;
    [SerializeField]
    private Transform gunMuzzleTr = null;
    [SerializeField]
    private Transform giantHomingMissileSpawnTr = null;
    [SerializeField]
    private GameObject giantHomingMissilePrefab = null;
    [SerializeField]
    private GroupHomingMissileSpawnPos[] arrGroupHomingMissileSpawnPos = null;
    [SerializeField]
    private CannonRainMemoryPool cannonRainMemoryPool = null;
    [SerializeField]
    private CannonMemoryPool cannonMemoryPool = null;
    [SerializeField]
    private GatlinMemoryPool gatlinMemoryPool = null;
    [SerializeField]
    private GroupMissileMemoryPool groupMissileMemoryPool = null;
    [SerializeField]
    private GameObject airPush= null;
    [SerializeField]
    private Transform[] arrFootWindSpawnTr = null;
    [SerializeField]
    private GameObject footWindGo = null;
    [SerializeField]
    private GameObject sitDownGo = null;
    [SerializeField]
    private CannonAudioManager cannonAudioManager = null;
    [SerializeField]
    private CustomAudioManager airPushAudioManager = null;
    [SerializeField]
    private GameObject[] cannonSoundSpawnGOs = null;
    [SerializeField]
    private GameObject airPushSoundSpawnGO = null;
    [SerializeField]
    private GameObject windBlowSoundSpawnGO = null;
    [SerializeField]
    private GameObject sitDownSoundSpawnGO = null;
    [SerializeField]
    private GameObject gatlingLaunchSoundSpawnGO = null;
    [SerializeField]
    private GameObject gatlingRotationSoundSpawnGO = null;
    [SerializeField]
    private GameObject tornadoSoundSpawnGO = null;
    [SerializeField]
    private GameObject giantTornadeSoundSpawnGO = null;

    [Header("-InformationForBossController")]
    [SerializeField]
    private GameObject bossShieldGeneratorPrefab = null;
    [SerializeField]
    private float rotationSpeed = 20f;
    [SerializeField]
    private float respawnShieldGeneratorTime = 0f;
    [SerializeField]
    private GameObject[] arrModelGo = null;
    [SerializeField]
    private GameObject[] coreGo = null;
    [SerializeField]
    private float autoRotateDegree = 30f;
    [SerializeField]
    private GameObject rotateBody = null;
    [SerializeField]
    private GameObject eyeGo = null;


    private Transform playerTr = null;
    private BossCollider bossCollider = null;
    private List<GameObject> curShieldGeneratorPoint = null;
    private BehaviourTreeRunner myRunner = null;
    private int curPhaseNum = 0;
    private bool isChangingPhase = false;
    private Rigidbody bossRb = null;

    private WaitForFixedUpdate waitFixedUpdate = null;
    private BossAnimationController animCtrl = null;
    private VoidIntDelegate cameraActionCallback = null;
    private BossStatusHp statHp = null;
    private BossShield shield = null;
    private BombPatternController timeBombPatternCtrl = null;
    private GetRandomSpawnPointDelegate getRandomSpawnPointCallback = null;
    private BossShieldGeneratorSpawnPoint[] arrCurShieldGeneratorSpawnPoints = null;
    private LastPatternController lastPatternCtrl = null;

    private bool isBossStartRotation = false;
}
