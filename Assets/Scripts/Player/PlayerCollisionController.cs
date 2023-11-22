using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCollisionController : MonoBehaviour
{
    public delegate void ChangeCollisionConditionDelegate(Collision _coli);
    public delegate void KnockBackDelegate(GameObject _colliderGo);

    private ChangeCollisionConditionDelegate collisionEnterCallback = null;
    private VoidVoidDelegate collisionExitCallback = null;
    private KnockBackDelegate knockBackCallback = null;
    private VoidBoolDelegate boundaryCallback = null;

    public void Init(ChangeCollisionConditionDelegate _collisionEnterCallback, VoidVoidDelegate _collisionExitCallback, KnockBackDelegate _knockBackCallback, PlayerData _playerData, VoidBoolDelegate _boundaryCallback)
    {
        collisionEnterCallback = _collisionEnterCallback;
        collisionExitCallback = _collisionExitCallback;
        knockBackCallback = _knockBackCallback;
        boundaryCallback = _boundaryCallback;
        playerData = _playerData;
        oriLayer = gameObject.layer;
        waitInvincibleTime = new WaitForSeconds(invincibleTime);
    }


    private void OnCollisionEnter(Collision collision)
    {
        collisionEnterCallback?.Invoke(collision);
        //if(playerData.currentMoveSpeed > 150f)
        //    knockBackCallback?.Invoke(collision.gameObject);

        //Invincible();
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionExitCallback?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HealMarble"))
            return;
        if (other.CompareTag("Boundary"))
        // 즉사 코루틴 정지
        {
            boundaryCallback?.Invoke(false);
            return;
        }
        knockBackCallback?.Invoke(other.gameObject);

        Invincible();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boundary"))
            //10초 뒤에 즉사
            boundaryCallback(true);
    }

    public void Invincible()
    {
        gameObject.layer = LayerMask.NameToLayer(playerInvincibleLayer);
        StopCoroutine("FinishInvincible");
        StartCoroutine("FinishInvincible");
        //Invoke("FinishInvincible", invincibleTime);
    }

    private IEnumerator FinishInvincible()
    {
        yield return waitInvincibleTime;

        gameObject.layer = oriLayer;
    }

    [SerializeField]
    private string playerInvincibleLayer;
    [SerializeField]
    private float invincibleTime = 0f;

    private LayerMask oriLayer;
    private WaitForSeconds waitInvincibleTime = null;
    private PlayerData playerData = null;
}
