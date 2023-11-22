using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldProgressbar : ImageProgressbar
{
    public void RemoveBossShield()
    {
        imageBack.gameObject.SetActive(false);
    }
}
