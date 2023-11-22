using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDownController : AttackableObject
{
    private void OnTriggerEnter(Collider other)
    {
        AttackDmg(other);
    }
}
