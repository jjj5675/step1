using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDASH : PlayerFSMManager
{
    // Update is called once per frame
    void Update()
    {
        controller.transform.position += controller.dashDistance * controller.lastMoveDir;
        controller.SetState(PlayerState.IDLE);
    }
}
