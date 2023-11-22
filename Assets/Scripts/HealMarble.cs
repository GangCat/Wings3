using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMarble : MonoBehaviour
{
    public void SetActive(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (_other.TryGetComponent<PlayerStatusHp>(out var component))
        {
            component.HealHp(healAmount);
            gameObject.SetActive(false);
        }
    }

    public bool IsActive()
    {
        return gameObject.activeSelf;
    }

    [SerializeField]
    private float healAmount = 30f;
}
