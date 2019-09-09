using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeftWalk : PlayerFSMManager
{
    // Update is called once per frame
    void Update()
    {
        controller.lastMoveDir = Vector3.left;
        controller.GetComponentInChildren<SpriteRenderer>().flipX = true;
        if (Input.GetKey(KeyCode.A))
            controller.cc.Move(Vector2.left * controller.walkSpeed * Time.deltaTime);
        else
            controller.SetState(PlayerState.IDLE);
    }
}
