using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossDamageable : IDamageable
{
    public void GetDamage(float _dmg, GameObject _attackGo);
}
