using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : AnimationControllerBase
{

    public override void Init()
    {
        base.Init();
    }


    public void SetSpeedFloat(float _moveSpeed)
    {
        anim.SetFloat("moveSpeed", _moveSpeed);
    }

    public void SetMousePos(float _mouseX, float _mouseY)
    {
        anim.SetFloat("mouseX", _mouseX);
        anim.SetFloat("mouseY", _mouseY);
    }


}
