using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollider : MonoBehaviour
{
    public void Init()
    {
        bossSphereCollider = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
    }


    public void SetTag(string _tagName)
    {
        gameObject.tag = _tagName;
    }

    public void ResetAll()
    {
        gameObject.tag = "Untagged";
        bossSphereCollider.center = Vector3.zero;
        bossSphereCollider.radius = 1f;
        bossSphereCollider.enabled = false;
        dmg = 0f;
    }

    public void SetPos(Vector3 _pos)
    {
        bossSphereCollider.center = _pos;
    }

    public void SetSize(float _radius)
    {
        bossSphereCollider.radius = _radius;
    }

    public void SetEnableCollider()
    {
        bossSphereCollider.enabled = true;
    }

    public void SetDmg(float _dmg)
    {
        dmg = _dmg;
    }

    public void OnTriggerEnter(Collider _other)
    {
        IPlayerDamageable damageable = _other.GetComponent<IPlayerDamageable>();
        if (damageable != null)
            damageable.GetDamage(dmg);
    }

    private SphereCollider bossSphereCollider = null;
    private Rigidbody rb = null;
    private float dmg = 0f;
}
