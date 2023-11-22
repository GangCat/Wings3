using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerDamageable : IDamageable
{
    public void ForceGetDmg(float _dmg);
}
