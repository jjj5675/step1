using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRightWalk : PlayerFSMManager
{
    // Update is called once per frame
    void Update()
    {

        controller.lastMoveDir = Vector3.right;
        controller.GetComponentInChildren<SpriteRenderer>().flipX = false;
        if (Input.GetKey(KeyCode.D))
            controller.cc.Move(Vector2.right * controller.walkSpeed * Time.deltaTime);
        else
            controller.SetState(PlayerState.IDLE);
    }
}
