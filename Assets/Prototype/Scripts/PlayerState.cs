using UnityEngine;

public abstract class PlayerState
{
    public abstract void OnUpdate();
}

public sealed class PlayerMoveState : PlayerState
{
    public PlayerMovement Movement;
    public PlayerRotatingControl RotateControl;

    public override void OnUpdate()
    {
        Movement.OnUpdate();
        RotateControl.OnUpdate();
    }
}