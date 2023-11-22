using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAlertMessage : MonoBehaviour
{
    public void Init()
    {
        text = GetComponent<TMP_Text>();
    }

    public void AlertDanger(string _text)
    {
        text.text = _text;
    }


    private TMP_Text text = null;
}
