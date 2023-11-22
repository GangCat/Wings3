using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class BombPatternController : MonoBehaviour
{
    public delegate void VoidColorDelegate(Color _color);
    VoidColorDelegate changeBodyEyeColorCallback = null;

    public void Init(
        VoidVoidDelegate _patternFinishDelegate, 
        VoidBoolDelegate _bossRotationCallback, 
        VoidVoidDelegate _reloadCannonCallback,
        VoidVoidDelegate _alertFirstPatternCallback,
        VoidColorDelegate _changeBodyEyeColorCallback,
        Transform _targetTr)
    {
        soundManager = SoundManager.Instance;
        soundManager.Init(gameObject);
        patternFinishCallback = _patternFinishDelegate;
        bossRotationCallback = _bossRotationCallback;
        reloadCannonCallback = _reloadCannonCallback;
        alertFirstPatternCallback = _alertFirstPatternCallback;
        changeBodyEyeColorCallback = _changeBodyEyeColorCallback;
        targetTr = _targetTr;

        windBlowHolder.Init();
        arrBombGo = new GameObject[4];
        waitFixedTime = new WaitForFixedUpdate();

    }

    public void StartPattern()
    {
        if (isDebugMode)
        {
            FinishPattern();
            return;
        }

        StartWindBlow();
        StartCoroutine(PatternCoroutine());
        Debug.Log("PatterStart");
        //Invoke("FinishPattern", 5f);
    }

    private void StartWindBlow()
    {
        WindBlowPoint[] arrWindBlowPoints = windBlowHolder.WindBlowPoints;

        foreach(WindBlowPoint wbp in arrWindBlowPoints)
        {
            wbp.StartGenerateSecond(windBlowCylinderPrefab);
        }    
    }

    private void FinishPattern()
    {
        WindBlowPoint[] arrWindBlowPoints = windBlowHolder.WindBlowPoints;

        foreach (WindBlowPoint wbp in arrWindBlowPoints)
        {
            wbp.FinishGenerate();
        }

        Debug.Log("PatterFinish");
        patternFinishCallback?.Invoke();
    }

    private IEnumerator PatternCoroutine()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            // �Ͼ�� �ִϸ��̼�
            // �ִϸ��̼� ����Ǹ� Ż��
            yield return waitFixedTime;
            break;
        }

        float spawnTime = Time.time;
        int bombIdx = 0;
        while (bombIdx < 4)
        {
            if (Time.time - spawnTime > spawnBombDelay)
            {
                //������ź �߻� ���� ���
                GameObject bombGo = Instantiate(timeBombPrefab, timeBombSpawnTr.position, Quaternion.identity);
                bombGo.GetComponent<Bomb>().Init(timeBombDestTr[bombIdx].position, launchAngle, gravity, targetTr, colors[bombIdx], bombIdx);
                arrBombGo[bombIdx] = bombGo;
                ++bombIdx;
                spawnTime = Time.time;
                reloadCannonCallback?.Invoke();
            }

            yield return waitFixedTime;
        }

        alertFirstPatternCallback?.Invoke();
        // ���� ��ź�� �� ����������� ��ٸ��� �� �÷��̾� Ȯ���϶�� �ǹ��� ���ð�
        yield return new WaitForSeconds(4f);
        bossRotationCallback?.Invoke(true);

        // 0~3 ���ڸ� �������� �����ؼ� ������ ����� ranSelect�迭�� ���� / 0 - 2 - 3 - 1 �� ���� �����
        GenerateUniqueRandomNumbers();

        float laserStartTime = Time.time;
        int laserCount = 0;
        GameObject laserGo = null;
        GameObject ChargeGo = null;
        Debug.Log("StartLaserCharge");
        changeBodyEyeColorCallback?.Invoke(colors[ranSelect[laserCount]]);
        ChargeGo = ChargeLaser(colors[ranSelect[laserCount]]);
        ChargeGo.transform.parent = laserLaunchTr;
        //������ ������� ���� ���(����)
        soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.LASERPREPERATIONSOUND,true);

        while (true)
        {
            if (Time.time - laserStartTime > laserDelay - 1f)
            {
                //������ ������� ���� ����
                if (soundManager.IsPlaying(GetComponent<AudioSource>()))
                {
                    soundManager.StopAudio(GetComponent<AudioSource>());
                }
                bossRotationCallback?.Invoke(false);
                Quaternion laserRotation = CalcLaserRotation();

                while (Time.time - laserStartTime < laserDelay)
                    yield return waitFixedTime;

                laserGo = LaunchLaser(laserRotation, colors[ranSelect[laserCount]], ranSelect[laserCount]);
            }

            if (laserGo)
            {
                while (laserGo)
                    yield return waitFixedTime;

                if (arrBombGo[ranSelect[laserCount]] != null)
                    arrBombGo[ranSelect[laserCount]].GetComponent<Bomb>().Explosion();

                ++laserCount;
                if (laserCount >= 4)
                    break;

                //������ ������� ���� ���(����)
                soundManager.PlayAudio(GetComponent<AudioSource>(), (int)SoundManager.ESounds.LASERPREPERATIONSOUND,true);
                Debug.Log("StartLaserCharge");
                changeBodyEyeColorCallback?.Invoke(colors[ranSelect[laserCount]]);
                ChargeGo = ChargeLaser(colors[ranSelect[laserCount]]);
                ChargeGo.transform.parent = laserLaunchTr;
                laserStartTime = Time.time;
                bossRotationCallback?.Invoke(true);
            }

            yield return waitFixedTime;
        }

        changeBodyEyeColorCallback?.Invoke(Color.red);
        bossRotationCallback?.Invoke(false);
        //bool isPatternFinish = true;
        //while (true)
        //{
        //    isPatternFinish = true;
        //    foreach(GameObject go in arrBombGo)
        //    {
        //        if (go != null)
        //        {
        //            isPatternFinish = false;
        //            break;
        //        }
        //    }

        //    if (isPatternFinish)
        //        break;

        //    yield return waitFixedTime;
        //}

        FinishPattern();
    }

    private GameObject LaunchLaser(Quaternion _laserRotation, Color _curColor, int _idx)
    {

        GameObject laserGo = Instantiate(laserPrefab, laserLaunchTr.position, laserLaunchTr.rotation * _laserRotation);
        laserGo.GetComponent<LaserController>().Init(laserDuration, laserLengthPerSec, initWidth, initHeight, _curColor, _idx);

        Destroy(laserGo, laserDuration);
        return laserGo;
    }

    private GameObject ChargeLaser(Color _curColor)
    {
        GameObject laserChargeGo = Instantiate(laserChargePrefab, laserLaunchTr.position+ laserLaunchTr.forward*80f, laserLaunchTr.rotation,transform);
        VisualEffect vfx = laserChargeGo.GetComponent<VisualEffect>();

        vfx.SetFloat("Duration", laserDelay);
        float decreaseChargeDuration = laserDelay - 2f;
        vfx.SetFloat("ChargeDuration", decreaseChargeDuration);
        vfx.SetGradient("Color", SetColorGradient(_curColor));
        vfx.SetGradient("Core Color", SetColorGradient(_curColor * 30f));
        vfx.Reinit();
        Destroy(laserChargeGo, laserDelay); //���ӽð� 10�� ��� ���ε� ���� ���� ������ ���°Ű���.
        return laserChargeGo;
    }

    private Gradient SetColorGradient(Color _curColor)
    {
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[4];

        colorKeys[0].color = _curColor;
        colorKeys[0].time = 0f;
        colorKeys[1].color = _curColor;
        colorKeys[1].time = 0f;

        alphaKeys[0].alpha = 0f;
        alphaKeys[0].time = 0f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[1].time = 0.1f;
        alphaKeys[2].alpha = 0.8f;
        alphaKeys[2].time = 0.8f;
        alphaKeys[3].alpha = 0f;
        alphaKeys[3].time = 1f;

        var gradient = new Gradient();
        gradient.SetKeys(colorKeys, alphaKeys);
        return gradient;
    }

    private Quaternion CalcLaserRotation()
    {
        float angleToPlayer = Mathf.Asin((targetTr.position.y - laserLaunchTr.position.y) / Vector3.Distance(laserLaunchTr.position, targetTr.position));
        angleToPlayer = Mathf.Clamp(angleToPlayer * Mathf.Rad2Deg, -launchAngleLimit, launchAngleLimit);
        return Quaternion.Euler(Vector3.left * angleToPlayer);
    }

    private void GenerateUniqueRandomNumbers()
    {
        List<int> availableNumbers = new List<int>() { 0, 1, 2, 3 }; // ������ ���ڵ��� ����Ʈ

        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, availableNumbers.Count); // ������ ���� �߿��� �����ϰ� ����

            ranSelect[i] = availableNumbers[randomIndex]; // ���õ� ���ڸ� �迭�� �߰�
            availableNumbers.RemoveAt(randomIndex); // ���� ���ڴ� ������ ���� ����Ʈ���� ����
        }
    }


    [Header("-Bomb")]
    [SerializeField]
    private GameObject timeBombPrefab = null;
    [SerializeField]
    private Transform[] timeBombDestTr = null;
    [SerializeField]
    private Transform timeBombSpawnTr = null;
    [SerializeField]
    private float spawnBombDelay = 0f; // ��ź 4�� ���� �����ϴ� ������
    [SerializeField]
    private float launchAngle = 45f;
    [SerializeField]
    private float gravity = 9.81f;
    [SerializeField]
    //[ColorUsage(true,true)]
    private Color[] colors = null;


    [Header("-Laser")]
    [SerializeField]
    private GameObject laserPrefab = null;
    [SerializeField]
    private GameObject laserChargePrefab = null;
    [SerializeField]
    private Transform laserLaunchTr = null;
    [SerializeField]
    private float laserDelay = 0f;
    [SerializeField]
    private float laserDuration = 0f;
    [SerializeField]
    private float laserLengthPerSec = 0f;
    [SerializeField]
    private float initWidth = 20f;
    [SerializeField]
    private float initHeight = 20f;
    [SerializeField]
    private float launchAngleLimit = 30f;

    [Header("-E.T.C")]
    [SerializeField]
    private GameObject windBlowCylinderPrefab = null;
    [SerializeField]
    private float startDelay = 10f;
    [SerializeField]
    private WindBlowHolder windBlowHolder = null;
    [SerializeField]
    private bool isDebugMode = false;

    private GameObject[] arrBombGo = null;
    private VoidVoidDelegate patternFinishCallback = null;
    private VoidBoolDelegate bossRotationCallback = null;
    private VoidVoidDelegate reloadCannonCallback = null;
    private VoidVoidDelegate alertFirstPatternCallback = null;
    private WaitForFixedUpdate waitFixedTime = null;
    private Transform targetTr = null;
    private int[] ranSelect = new int[4];

    private SoundManager soundManager = null;
    private float curLaserLength = 0f;
}
