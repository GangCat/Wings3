using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualMouse : MonoBehaviour
{
    public void Init(PlayerData _playerData)
    {
        playerData = _playerData;
        returnSpeed = playerData.returnMouseSpeed;
        sensitive = playerData.mouseSensitive;
        maxMouseSpeed = playerData.maxMouseSpeed;
        maxRadius = playerData.maxMouseRadius;
        returnRadius = playerData.returnMouseRadius;
    }


    private void Start()
    {
        centerPosition = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        mousePos = Vector2.zero;
    }

    public void FixedUpdateMouseInput()
    {
        MouseMove(mousePos);
    }

    public void UpdateMouseInput()
    {
        if (!playerData.isFreeLock)
        {
            newInput.x = Mathf.Clamp(Input.GetAxis("Mouse X") * sensitive, -maxMouseSpeed, maxMouseSpeed);
            newInput.y = Mathf.Clamp(Input.GetAxis("Mouse Y") * sensitive, -maxMouseSpeed, maxMouseSpeed);
            mousePos += newInput;
        }
        LockVirtualMousePos();
        playerData.currentMousePos = mousePos;
    }


    private void LockVirtualMousePos()
    {
        mousePosMagnitude = mousePos.magnitude;
        if (mousePosMagnitude > maxRadius)
        {
            mousePos = mousePos.normalized * maxRadius;
        }

        if (mousePosMagnitude < returnRadius)
        {
            mousePos = Vector2.MoveTowards(mousePos, Vector2.zero, returnSpeed * Time.deltaTime);
        }
    }


    private void MouseMove(Vector2 _virtualMousePos)
    {
        virtualCursor.transform.position = centerPosition + _virtualMousePos;
        virtualCursor.color = new Color(virtualCursor.color.r, virtualCursor.color.g, virtualCursor.color.b, mousePosMagnitude / maxRadius);
    }

    [SerializeField]
    private Image virtualCursor;

    [SerializeField]
    private float returnSpeed = 100f;
    [SerializeField]
    public float sensitive = 3f;
    [SerializeField]
    private float maxMouseSpeed = 3f;
    [SerializeField]
    private float maxRadius = 100f;
    [SerializeField]
    private float returnRadius = 90f;


    private float mousePosMagnitude = 0f;

    private Vector2 centerPosition;
    private Vector2 mousePos;
    private Vector2 newInput;

    private PlayerData playerData;





}
