using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    IDLE=0,
    WALK,
    JUMP,
    DOWN,
    DASH
}


public class PlayerController : MonoBehaviour
{
    public bool isKeyInput = false;
    private float gravity;
    public float verticalVelocity;
    public float walkSpeed;
    public float dashSpeed;
    public float jumpForce;

    private Vector2 moveDirection;
    public Vector3 lastMoveDir = Vector3.zero;
    private BoxCollider ground;
    public CharacterController cc;
    public Animator anim;

    public PlayerState startState;
    public PlayerState curState;

    public Dictionary<PlayerState, PlayerFSMController> states =
        new Dictionary<PlayerState, PlayerFSMController>();


    private void Awake()
    {
        jumpForce =10f;
        walkSpeed = 3f;
        dashSpeed = 3f;
        gravity = 10f;
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        ground = GameObject.FindGameObjectWithTag("Ground").GetComponent<BoxCollider>();
        states.Add(PlayerState.IDLE, GetComponent<PlayerIDLE>());
        states.Add(PlayerState.WALK, GetComponent<PlayerWALK>());
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
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.D)))
            SetState(PlayerState.WALK);
        if (Input.GetKey(KeyCode.S))
            SetState(PlayerState.DOWN);
        if (Input.GetKeyDown(KeyCode.LeftShift))
            SetState(PlayerState.DASH);

        if (!isKeyInput)
            SetState(PlayerState.IDLE);

        Gravity();
    }


    public void SetState(PlayerState newState)
    {
        if (newState != PlayerState.IDLE)
        {
            isKeyInput = true;
            states[PlayerState.IDLE].enabled = false;
        }

        if (!isKeyInput)
        {
            foreach (PlayerFSMController fsm in states.Values)
            {
                fsm.enabled = false;
            }
        }

        curState = newState;
        states[curState].enabled = true;

        if (states[PlayerState.JUMP].enabled)
            anim.SetInteger("curState", (int)PlayerState.JUMP);
        else
            anim.SetInteger("curState", (int)curState);
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
            isKeyInput = false;
            states[PlayerState.JUMP].enabled = false;
        }
    }
}
