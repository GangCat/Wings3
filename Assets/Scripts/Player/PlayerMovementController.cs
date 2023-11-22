using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovementController : MonoBehaviour
{
    public void Init(PlayerData _playerData,VoidIntDelegate _spUpdateCallback)
    {
        playerData = _playerData;
        playerTr = playerData.tr;
        waitFixedUpdate = new WaitForFixedUpdate();

        moveBackVelocityLimit = playerData.moveBackVelocityLimit;
        moveForwardVelocityLimit = playerData.moveForwardVelocityLimit;
        moveAccel = playerData.moveAccel;

        moveDashAccel = playerData.moveDashAccel;
        moveStopAccel = playerData.moveStopAccel;

        oriCamParent = cam.transform.parent;
        spUpdateDelegate = _spUpdateCallback;

        SpeedEffect.Stop();

        StartCoroutine(ChangeFOV());
        StartCoroutine(FrontMoveCheker());
        StartCoroutine(DashCheker());
        StartCoroutine(StaminaRegen());
    }

    public float MoveSpeed => moveSpeed;
    public bool IsDash => isDash;
    public bool IsLastPattern
    {
        get => isLastPattern;
        set => isLastPattern = value;
    }

    public void ReduceSpeed()
    {
        moveSpeed *= 0.3f;
    }

    public void ChangeCollisionCondition(Collision collision, bool _bool)
    {
        isCollision = _bool;
        coli = collision;
    }


    public void CalcPlayerMove(float _inputZ, bool _inputShift)
    {
        if (playerData.isAction == true || isDodge == true)
        {
            return;
        } else if (isFrontMove == false)
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, moveAccel * Time.deltaTime);
            if(!isKnockBack)
                playerVelocity = moveSpeed * playerTr.forward;
            return;
        }
        
        if (_inputZ != 0f)
        {

            playerData.isDash = isDash;
            moveAccelResult = isDash ? moveAccel + moveDashAccel : moveAccel;
            moveDashSpeed = isDash ? playerData.moveDashSpeed : 0f;

            if (moveSpeed > 20f)
            {
                float forwardY = playerTr.forward.y;
                if (forwardY >= 0.3f)
                {
                    //gravitySpeed = -playerData.gravitySpeed * 0.2f;
                    gravitySpeed = 0f;
                }
                else if (forwardY <= -0.2f)
                {
                    gravitySpeed = playerData.gravitySpeed;
                }
                else
                {
                    gravitySpeed = 0;
                }
            }
            moveSpeed += (moveAccelResult + gravityAccel) * Time.deltaTime * _inputZ;
        }
        else if (moveSpeed > 0f && _inputZ < 0f)
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, moveAccel * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.MoveTowards(moveSpeed, 0, moveAccel * Time.deltaTime);
        }
        
        addVelocity = Mathf.Lerp(addVelocity, moveDashSpeed + gravitySpeed, 0.1F);
        resultForwardVelocityLimit = (moveForwardVelocityLimit + addVelocity);
        //resultForwardVelocityLimit = (moveForwardVelocityLimit + moveDashSpeed + gravitySpeed);
        
        //if (_inputZ > 0f)
        //{
        //    if(resultForwardVelocityLimit > currentForwardVelocityLimit)
        //    currentForwardVelocityLimit = Mathf.Lerp(currentForwardVelocityLimit, resultForwardVelocityLimit, 0.5f* moveAccelResult * Time.deltaTime);
        //    else
        //    currentForwardVelocityLimit = Mathf.Lerp(currentForwardVelocityLimit, resultForwardVelocityLimit, 0.1f * Time.deltaTime);
        //}
        //else
        //{
        //    currentForwardVelocityLimit = Mathf.Lerp(currentForwardVelocityLimit, resultForwardVelocityLimit, 0.5f * moveAccelResult * Time.deltaTime);
        //}
        

        moveSpeed = Mathf.Clamp(moveSpeed, moveBackVelocityLimit, resultForwardVelocityLimit);

        if (!isKnockBack)
            playerVelocity = moveSpeed * playerTr.forward;


        if (isCollision)
        {
            float angle = Vector3.Angle(playerVelocity, coli.contacts[0].normal);
            
            if (angle >= 120)
            {
                CollisionCrash();
            }

            calcMoveSpeed = resultForwardVelocityLimit * Mathf.Clamp01((1 - angle / 170));
            moveSpeed = Mathf.Lerp(moveSpeed, calcMoveSpeed, moveAccel * Time.deltaTime);
            //Debug.Log(moveSpeed);

            playerVelocity = moveSpeed * playerTr.forward;

            if (CheckisSliding())
            {
                Vector3 normal = coli.contacts[0].normal;
                playerVelocity = playerVelocity - Vector3.Project(playerVelocity, normal);
            }
            else
                isCollision = false;
        }

    }

    public void KnockBack(Vector3 _knockBackAmount, float _knockBackDelay)
    {
        if(knockBackCoroutine != null)
            StopCoroutine(knockBackCoroutine);

        knockBackCoroutine = StartCoroutine(KnockBackCoroutine(_knockBackAmount, _knockBackDelay));
    }

    private IEnumerator KnockBackCoroutine(Vector3 _knockBackAmount, float _knockBackDelay)
    {
        moveSpeed = 0f;
        isKnockBack = true;
        float elapsedTime = 0f;
        float curKnockBackDelay = _knockBackDelay > 0 ? _knockBackDelay : knockBackDelay;
        while (elapsedTime < curKnockBackDelay)
        {
            if (!isCollision)
                playerVelocity = Vector3.Lerp(_knockBackAmount, Vector3.zero, elapsedTime / curKnockBackDelay);

            elapsedTime += Time.deltaTime;

            yield return waitFixedUpdate;
        }
        playerVelocity = Vector3.zero;
        isKnockBack = false;
    }


    private void CollisionCrash()
    {
        if (moveSpeed >= 150)
        {
            StartCoroutine(PlayerCrash());
            Debug.Log("강한 충돌");
        }
    }


    private bool CheckisSliding()
    {
        Vector3 vector1 = playerTr.forward;
        Vector3 vector2 = coli.contacts[0].normal;

        float dotProduct = Vector3.Dot(vector1, vector2);
        float magnitude1 = vector1.magnitude;
        float magnitude2 = vector2.magnitude;

        float cosAngle = dotProduct / (magnitude1 * magnitude2);
        float angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;

        return angle >= 100;
    }

    public void PlayerMove()
    {
        if (isLastPattern)
            playerVelocity += suckSpeed * (bossCoreEnterance.position - transform.position).normalized;

        //if(moveSpeed > 150f && SpeedEffect.)

        rb.velocity = playerVelocity;
        playerData.currentMoveSpeed = moveSpeed; // 현재 속도 공유
    }


    public void PlayerDodge(bool _inputQ, bool _inputE)
    {
        if (isDodge == false && StaminaChecke())
        {
            if (_inputQ == true)
            {
                Vector3 forwardLeft = Vector3.Cross(playerTr.forward, Vector3.up);
                isDodge = true;
                playerData.isAction = true;
                StartCoroutine(DecreaseSpeed(SkyAclTime));
                StartCoroutine(MoveToDir(playerData.dodgeSpeed, dodgeDuration, forwardLeft));
                DecreaseStamina(1);
            }

            if (_inputE == true)
            {
                Vector3 forwardRight = Vector3.Cross(playerTr.forward, Vector3.down);
                isDodge = true;
                playerData.isAction = true;
                StartCoroutine(DecreaseSpeed(SkyAclTime));
                StartCoroutine(MoveToDir(playerData.dodgeSpeed, dodgeDuration, forwardRight));
                DecreaseStamina(1);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Vector3 forwardUp = Vector3.up;
                isDodge = true;
                playerData.isAction = true;
                StartCoroutine(DecreaseSpeed(SkyAclTime));
                StartCoroutine(MoveToDir(playerData.dodgeSpeed, dodgeDuration, forwardUp));
                DecreaseStamina(1);
            }
        }
        else
            playerData.isAction = false;

    }

    private IEnumerator MoveToDir(float speed, float duration, Vector3 _dir)
    {
        //cam.transform.parent = transform;

        Vector3 direction = _dir.normalized;
        float distance = speed * duration;
        float elapsedTime = 0f;

        yield return new WaitForSeconds(SkyAclTime);
        
        Vector3 initialPosition = rb.position;
        Vector3 targetPosition = initialPosition + direction * distance;
        while (elapsedTime < duration)
        {
            
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime / duration);
            Vector3 new_pos = Vector3.Lerp(initialPosition, targetPosition, t);

            RaycastHit hit;
            if (rb.SweepTest(direction, out hit, (new_pos - rb.position).magnitude))
            {
                isDodge = false;
                break;
            }

            rb.MovePosition(new_pos);

            elapsedTime += Time.fixedDeltaTime;
            yield return waitFixedUpdate; 
        }
        //cam.transform.parent = oriCamParent;
        isDodge = false;
    }

    private IEnumerator SetVelocityOverTime(float speed, float duration, Vector3 _dir)
    {
        cam.transform.parent = transform;
        float initMoveSpeed = moveSpeed;
        float timeElapsed = 0f;
        yield return new WaitForSeconds(SkyAclTime);
        while (timeElapsed < duration)
        {
            moveSpeed = Mathf.Lerp(initMoveSpeed, speed, timeElapsed/duration);
            playerVelocity = _dir * moveSpeed;
            timeElapsed += Time.fixedDeltaTime;
            yield return null;
            
        }
        cam.transform.parent = oriCamParent;
        isDodge = false;
    }

    private IEnumerator PlayerCrash()
    {
        playerData.isCrash = true;
        moveSpeed = Mathf.MoveTowards(moveSpeed, 0, moveAccel * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);
        playerData.isCrash = false;
    }

    private IEnumerator ChangeFOV()
    {
        float targetFOV = Camera.main.fieldOfView;
        CameraMovement cam = Camera.main.GetComponent<CameraMovement>();
        float offset = cam.offset;
        cameraMinSpeed = playerData.moveForwardVelocityLimit;
        cameraMaxSpeed = playerData.moveForwardVelocityLimit + playerData.moveDashSpeed + playerData.gravitySpeed;
        float lerpSpeedRatio = 0f;
        while (true)
        {
            float speedRatio = Mathf.InverseLerp(cameraMinSpeed, cameraMaxSpeed, moveSpeed);
            lerpSpeedRatio = Mathf.Lerp(lerpSpeedRatio, speedRatio, 5f*Time.fixedDeltaTime);
            targetFOV = Mathf.Lerp(cameraminFOV, cameramaxFOV, lerpSpeedRatio);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.fixedDeltaTime);
            float targetOffset = Mathf.Lerp(10, 3, lerpSpeedRatio);
            //cam.offset = Mathf.Lerp(cam.offset, targetOffset, Time.fixedDeltaTime);
            yield return waitFixedUpdate;
        }
    }

    private IEnumerator DecreaseSpeed(float duration)
    {
        float startSpeed = moveSpeed;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            moveSpeed = Mathf.Lerp(startSpeed, 0, elapsed / duration);
            yield return null;
        }

        moveSpeed = 0;
    }

    private IEnumerator FrontMoveCheker()
    {
        while (true)
        {

            if (playerData.input.InputZ != 0 && !isFrontMove)
            {
                yield return null;
                isFrontMove = true;
            }

            if (playerData.input.InputZ == 0 && moveSpeed <= 0)
            {
                isFrontMove = false;
            }
            yield return null;
        }
    }

    private IEnumerator DashCheker()
    {
        while (true)
        {

            if (playerData.input.InputShift && isFrontMove && StaminaChecke())
            {
                DecreaseStamina(1);
                isDash = true;
                SpeedEffect.Play();
                yield return new WaitForSeconds(1f);
                SpeedEffect.Stop();
                isDash = false;
            }

            yield return null;
        }
    }

    private IEnumerator StaminaRegen()
    {
        while (true)
        {
            if (playerData.stamina<3)
            {
                yield return new WaitForSeconds(5f);
                playerData.stamina++;
                spUpdateDelegate.Invoke(playerData.stamina);
                staminaUp.SetActive(true);
            }
            yield return null;
        }
    }

    private bool StaminaChecke()
    {
        int currentStamina = playerData.stamina;
        if (currentStamina > 0)
        {
            return true;
        }
        return false;

    }
    private void DecreaseStamina(int _decreaseAmount)
    {
        int currentStamina = playerData.stamina;
        if (currentStamina>=1)
        {
            if (currentStamina >= 1 && currentStamina <= 3)
            {
                currentStamina -= _decreaseAmount;
                playerData.stamina = currentStamina;
                spUpdateDelegate.Invoke(currentStamina);
            }
        }
    }


    private Vector3 playerVelocity = Vector3.zero;
    private Vector3 PlayerDir = Vector3.zero;

    private WaitForFixedUpdate waitFixedUpdate = null;

    private float calcMoveSpeed = 0f;

    private float moveBackVelocityLimit = 0f;
    private float moveForwardVelocityLimit = 0f;

    private float addVelocity = 0f;
    private float resultForwardVelocityLimit = 0f;
    private float currentForwardVelocityLimit = 0f;

    private float moveSpeed = 0f;
    private float moveAccel = 0f;

    private float moveDashSpeed = 0f;
    private float moveDashAccel = 0f;

    private float gravityAccel = 0f;
    private float gravitySpeed = 0f;

    private float moveStopAccel = 0f;
    private float moveAccelResult = 0f;

    private float SkyAclTime = 0f;


    private bool isDash = false;
    private bool isDodge = false;
    private bool isCollision = false;
    private bool isKnockBack = false;
    private bool isFrontMove = false;
    private bool isLastPattern = false;


    [SerializeField]
    private Rigidbody rb = null;
    [SerializeField]
    private float dodgeDuration = 1f;
    [SerializeField]
    private float knockBackDelay = 5f;
    [SerializeField]
    private Transform bossCoreEnterance = null;
    [SerializeField]
    private float suckSpeed = 100f;
    [SerializeField]
    private VisualEffect SpeedEffect = null;
    [SerializeField]
    private GameObject staminaUp = null;

    [SerializeField]
    private GameObject cam = null;

    private Transform oriCamParent = null;

    private Transform playerTr = null;
    private PlayerData playerData = null;

    private Collision coli = null;

    private float cameraMinSpeed = 80f;
    private float cameraMaxSpeed = 120f;
    private float cameraminFOV = 70f;
    private float cameramaxFOV = 110f;

    private VoidIntDelegate spUpdateDelegate = null;

    private Coroutine knockBackCoroutine = null;

}
