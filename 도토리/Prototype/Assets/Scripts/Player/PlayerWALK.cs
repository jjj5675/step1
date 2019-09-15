using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWALK : PlayerFSMController
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            controller.lastMoveDir = Vector3.left;
            controller.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            controller.lastMoveDir = Vector3.right;
            controller.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        else
        {
            controller.isKeyInput = false;
            controller.states[PlayerState.WALK].enabled = false;
        }

        controller.cc.Move(controller.lastMoveDir * controller.walkSpeed * Time.deltaTime);

    }
}
