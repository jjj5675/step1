using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE=0,
    LEFT_WALK,
    RIGHT_WALK,
    JUMP,
    DOWN,
    DASH
}


public class PlayerController : MonoBehaviour
{
    private float gravity;
    public float verticalVelocity;
    public float walkSpeed;
    public float dashDistance;
    public float jumpForce;

    private Vector2 moveDirection;
    public Vector3 lastMoveDir;
    private BoxCollider ground;
    public CharacterController cc;

    public PlayerState startState;
    public PlayerState currentState;

    Dictionary<PlayerState, PlayerFSMManager> states =
        new Dictionary<PlayerState, PlayerFSMManager>();


    private void Awake()
    {
        jumpForce=10f;
        walkSpeed = 3f;
        dashDistance = 4f;
        gravity = 10f;
        lastMoveDir = Vector3.right;
        cc = GetComponent<CharacterController>();
        ground = GameObject.FindGameObjectWithTag("Ground").GetComponent<BoxCollider>();
        states.Add(PlayerState.IDLE, GetComponent<PlayerIDLE>());
        states.Add(PlayerState.LEFT_WALK, GetComponent<PlayerLeftWalk>());
        states.Add(PlayerState.RIGHT_WALK, GetComponent<PlayerRightWalk>());
        states.Add(PlayerState.JUMP, GetComponent<PlayerJUMP>());
        states.Add(PlayerState.DOWN, GetComponent<PlayerDOWN>());
        states.Add(PlayerState.DASH, GetComponent<PlayerDASH>());
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(startState);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            SetState(PlayerState.LEFT_WALK);
        else if (Input.GetKeyDown(KeyCode.D))
            SetState(PlayerState.RIGHT_WALK);

        if (Input.GetKeyDown(KeyCode.S))
            SetState(PlayerState.DOWN);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            SetState(PlayerState.DASH);

        Gravity();
    }


    public void SetState(PlayerState newState)
    {
        foreach(PlayerFSMManager fsm in states.Values)
        {
            fsm.enabled = false;
        }

        currentState = newState;
        states[currentState].enabled = true;
    }

    public void Gravity()
    {
        if (cc.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.W))
            {
                verticalVelocity = jumpForce;
                SetState(PlayerState.JUMP);
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        moveDirection = Vector2.zero;
        moveDirection.y = verticalVelocity;
        cc.Move(moveDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((hit.gameObject.tag == "Ground") &&
            (verticalVelocity <= -gravity))
        {
            SetState(PlayerState.IDLE);
        }
    }
}
