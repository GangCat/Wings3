using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private float inputX;
    private float inputZ;
    private float inputMouseX;
    private float inputMouseY;
    private bool inputShift;
    private bool inputQ;
    private bool inputE;


    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputZ = Input.GetAxisRaw("Vertical");
        inputMouseX = Input.GetAxis("Mouse X");
        inputMouseY = Input.GetAxis("Mouse Y");
        inputShift = Input.GetKey(KeyCode.LeftShift);
        inputQ = Input.GetKey(KeyCode.Q);
        inputE = Input.GetKey(KeyCode.E);
    }

    public float InputX => inputX;
    public float InputZ => inputZ;
    public float InputMouseX => inputMouseX;
    public float InputMouseY => inputMouseY;
    public bool InputShift => inputShift;
    public bool InputQ => inputQ;
    public bool InputE => inputE;

}
