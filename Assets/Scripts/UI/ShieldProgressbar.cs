using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProgressbar : ImageProgressbar
{
    public void RemoveBossShield()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
