using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAlertMessage : MonoBehaviour, ISubscriber
{
    public void Init()
    {
        imageAlert = GetComponentInChildren<ImageAlertMessage>();
        imageAlert.Init();

        Subscribe();
        gameObject.SetActive(false);
    }

    public void Subscribe()
    {
        Broker.Subscribe(this, EPublisherType.BOSS_CONTROLLER);
    }

    public void ReceiveMessage(EMessageType _message)
    {
        if (_message.Equals(EMessageType.GIANT_MISSILE_ALERT))
        {
            gameObject.SetActive(true);
            StartCoroutine(AlertDangerCoroutine(0));
        }
        else if (_message.Equals(EMessageType.FIRST_PATTERN_1_ALERT))
        {
            gameObject.SetActive(true);
            StartCoroutine(AlertDangerCoroutine(1));
        }
        else if (_message.Equals(EMessageType.FIRST_PATTERN_2_ALERT))
        {
            gameObject.SetActive(true);
            StartCoroutine(AlertDangerCoroutine(2));
        }
        else if (_message.Equals(EMessageType.LAST_PATTERN_ALERT))
        {
            gameObject.SetActive(true);
            StartCoroutine(AlertDangerCoroutine(3));
        }
    }

    private IEnumerator AlertDangerCoroutine(int _idx)
    {
        imageAlert.AlertDanger(_idx);
        yield return new WaitForSeconds(alertDelays[_idx]);

        gameObject.SetActive(false);
    }

    private ImageAlertMessage imageAlert = null;
    [SerializeField]
    private float[] alertDelays = null;
}
