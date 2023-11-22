using UnityEngine;
using TheKiwiCoder;

public class GatilingActionNode : ActionNode
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private int maxBulletCnt = 80;
    [SerializeField]
    private float rotationSpeed = 30f;
    [SerializeField]
    private float fireRate = 10f;
    [SerializeField]
    private float headRotationSpeed = 20f;
    [SerializeField]
    private float rebound = 2f;
    [SerializeField]
    private float autoDestroyTime = 20f;
    [SerializeField]
    private float minDegreeToRotateGun = -30f;
    [SerializeField]
    private bool isRandomShoot = false;


    private Transform playerTr;
    private Transform gunMuzzleTr;
    private GameObject gatlingHolder;
    private Transform gatlinGeadTr;

    private float curBulletCnt;
    private float lastFireTime;
    private float diffY;
    private float cetha;
    private Vector3 rndRebound;

    private SoundManager soundManager = null;


    protected override void OnStart() {

        curBulletCnt = maxBulletCnt;
        playerTr = context.playerTr;
        gunMuzzleTr = context.gunMuzzleTr;
        gatlingHolder = context.gatlingHolderGo;
        gatlinGeadTr = context.gatlingHeadGo.transform;
        soundManager = SoundManager.Instance;
        soundManager.Init(context.gatlingLaunchSoundSpawnGO);
        soundManager.Init(context.gatlingRotationSoundSpawnGO);

        soundManager.PlayAudio(context.gatlingRotationSoundSpawnGO.GetComponent<AudioSource>(), (int)SoundManager.ESounds.GATLINGROTATESOUND, true);
        soundManager.PlayAudio(context.gatlingLaunchSoundSpawnGO.GetComponent<AudioSource>(), (int)SoundManager.ESounds.GATLINGSHOOTINGSOUND, true);

        context.bossCtrl.GatlinGun.GetComponent<GatilinSpinController>().StartSpin();


    }

    protected override void OnStop() {
        context.bossCtrl.GatlinGun.GetComponent<GatilinSpinController>().StopSpin();

        if (soundManager)
        {
            if (soundManager.IsPlaying(context.gatlingLaunchSoundSpawnGO.GetComponent<AudioSource>()))
            {
                soundManager.StopAudio(context.gatlingLaunchSoundSpawnGO.GetComponent<AudioSource>());
            }
            if (soundManager.IsPlaying(context.gatlingRotationSoundSpawnGO.GetComponent<AudioSource>()))
            {
                soundManager.StopAudio(context.gatlingRotationSoundSpawnGO.GetComponent<AudioSource>());
            }
        }
    }

    protected override State OnUpdate() {
        rndRebound = new Vector3(Random.Range(-rebound, rebound), Random.Range(-rebound, rebound), Random.Range(-rebound, rebound));

        if (!isRandomShoot)
        {
            RotateTurretToPlayer();
            RotateTurretHeadToPlayer();
            // 플레이어와의 거리계산 > 가까울수록 큼 > 게틀린건 회전하는 기어 혹은 기계음 소리
        }
        else
        {
            RotateTurretHeadRandom();
            // 플레이어와의 거리계산 > 가까울수록 큼 > 게틀린건 회전하는 기어 혹은 기계음 소리
        }

        if (CanFire())
        {
            FireBullet();
            //플레이어와의 거리계산 > 가까울수록 큼 > 총알 발사하는 소리재생
        }

        if (blackboard.isPhaseEnd)
            return State.Success;

        if (curBulletCnt > 0)
            return State.Running;

        return State.Success;
    }

    private void RotateTurretToPlayer()
    {
        if (playerTr != null)
        {
            Vector3 playerDirection = new Vector3(playerTr.position.x - gatlingHolder.transform.position.x, 0f, playerTr.position.z - gatlingHolder.transform.position.z);
            Quaternion targetRotation = Quaternion.LookRotation(playerDirection);

            // 부드럽게 회전하기 위해 Lerp 사용
            gatlingHolder.transform.rotation = targetRotation;
            //핑퐁써서 하기
        }
    }

    private void RotateTurretHeadToPlayer()
    {
        if (context.playerTr != null)
        {
            diffY = playerTr.position.y - context.gatlingHeadGo.transform.position.y;
            cetha = Mathf.Asin(diffY / Vector3.Distance(playerTr.position, context.gatlingHeadGo.transform.position)) * Mathf.Rad2Deg;
            cetha = Mathf.Clamp(cetha, minDegreeToRotateGun, 80);
            gatlinGeadTr.localRotation = Quaternion.Euler(Vector3.left * cetha);

            //Quaternion targetRotation = Quaternion.LookRotation(playerDirection);
            // 부드럽게 회전하기 위해 Lerp 사용
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void RotateTurretHeadRandom()
    {
        gatlinGeadTr.localRotation = Quaternion.Euler(Vector3.left * (Mathf.Sin(Time.time) * 15f * Time.deltaTime));
    }

    private bool CanFire()
    {
        return curBulletCnt > 0 && Time.time - lastFireTime >= 1 / fireRate;
    }

    private void FireBullet()
    {
        lastFireTime = Time.time;
        //curBulletCnt--;
        --curBulletCnt;

        Vector3 tmp = gunMuzzleTr.up;

        //float angle = Random.Range(0, 360);
        //float radians = angle * Mathf.Deg2Rad;
        //Vector3 spawnPosition = playerTr.position + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians)) * randomRange;

        Quaternion rot = Quaternion.AngleAxis(Random.Range(0, 360), gunMuzzleTr.forward);
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, rot, Vector3.one);
        Vector3 targetPos = rotationMatrix.MultiplyPoint3x4(tmp) + playerTr.position;

        GameObject bullet= context.gatlinMemoryPool.ActivateBullet();
        Vector3 spawnPos = gunMuzzleTr.position;
        Quaternion spawnRot = gunMuzzleTr.rotation * Quaternion.Euler(rndRebound);
        bullet.GetComponent<BulletController>().Init(autoDestroyTime, spawnPos, spawnRot, context.gatlinMemoryPool);
    }
}
