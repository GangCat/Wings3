using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Danger : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private LayerMask layer;
    public float offset = 30f;
    private Image dangerImage;

    void Start()
    {
        dangerImage = GetComponent<Image>();
    }

    void Update()
    {
        Collider[] missiles = Physics.OverlapSphere(player.position, 10000f, layer);
        if (missiles != null)
        {
            foreach (Collider missile in missiles)
            {
                Vector3 missilePosition = missile.transform.position;
                float distance = Vector3.Distance(player.position, missilePosition);
                Vector3 toMissile = missilePosition - player.position;
                float angle = Vector3.Angle(player.transform.forward, toMissile);

                if (angle > 90 && angle < 270 && distance < 800)
                {
                    dangerImage.enabled = true;
                    Vector3 middlePoint = (player.position + missilePosition) / 2f;
                    Vector3 uiPosition = Camera.main.WorldToScreenPoint(middlePoint);

                    // UI 요소의 위치 업데이트
                    dangerImage.transform.position = new Vector3(uiPosition.x*0.7f,0f,0f);
                }
                else
                {
                    dangerImage.enabled = false;
                }
            }
            
        }


    }


}
