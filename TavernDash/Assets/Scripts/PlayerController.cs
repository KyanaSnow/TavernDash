using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    // properties
    [Header("Components")]
    [SerializeField]
    private Transform _bodyTransform;
    private Transform _transform;
    [SerializeField]
    private Animator animator;
    private Dialogue dialogue;

    // states
    public enum States
    {
        Moving,
    }
    private States previousState;
    private States currentState;

    public delegate void UpdateState();
    UpdateState updateState;

    private float timeInState = 0f;

    // lerps
    Vector3 lerpInitialPos;
    Vector3 lerpInitalRot;

    [Header("Movement")]
    // movement
    [SerializeField]
    private float rotationSpeed = 50f;
    [SerializeField]
    private float moveSpeed = 2f;

    float currentSpeed = 0f;
    [SerializeField]
    private float acceleration = 1f;
    [SerializeField]
    private float collisionDistance = 1f;
    [SerializeField]
    private LayerMask collisionLayerMask;

    // Use this for initialization
    void Start()
    {
        _transform = this.transform;

        dialogue = GetComponentInChildren<Dialogue>();

        ChangeState(States.Moving);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (updateState != null)
        {
            updateState();
            timeInState += Time.deltaTime;
        }
    }

    #region go to table
    Vector3 initRot = Vector3.zero;
    float lerpTimer = 0f;
    private void Moving_Start()
    {
        //
    }
	public float runMoveSpeed = 1f;
	public float runAnimSpeed = 1.5f;
	float targetAnimSpeed = 0f;
	float currentAnimSpeed = 0f;

    private void Moving_Update()
    {
        Vector3 direction = InputDirection;
        float targetSpeed = moveSpeed;
		if ( Input.GetKey(KeyCode.LeftShift) ) {
			targetSpeed = runMoveSpeed;
		}

        if (PressingInput)
        {
            BodyTransform.forward = Vector3.MoveTowards(BodyTransform.forward, direction, rotationSpeed*Time.deltaTime);
        }
        else
        {
            targetSpeed = 0f;
        }

        if (Physics.Raycast(GetTransform.position + Vector3.up * 0.5f, direction, collisionDistance, collisionLayerMask))
            targetSpeed = 0f;

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        GetAnimator.SetFloat( "Movement", currentSpeed / moveSpeed );
		GetAnimator.speed = Input.GetKey(KeyCode.LeftShift) ? runAnimSpeed : 1f;

        GetTransform.Translate(BodyTransform.forward * currentSpeed * Time.deltaTime);
    }
    private void Moving_Exit()
    {
        //
    }
    private void ApplyMovement ()
    {
        
    }
    #endregion

    private Vector3 InputDirection
    {
        get
        {
            return new Vector3( Input.GetAxis("Horizontal") , 0f , Input.GetAxis ("Vertical") );
        }
    }
    private bool PressingInput
    {
        get
        {
            return Input.GetAxis ("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        }
    }

    #region state machine
    public void ChangeState(States newState)
    {
        previousState = currentState;
        currentState = newState;

        lerpInitalRot = BodyTransform.forward;
        lerpInitialPos = GetTransform.position;

        timeInState = 0f;

        ExitState();
        EnterState();
    }
    private void EnterState()
    {
        switch (currentState)
        {
            case States.Moving:
                updateState = Moving_Update;
                Moving_Start();
                break;
        }
    }
    private void ExitState()
    {
        switch (previousState)
        {
            case States.Moving:
                Moving_Exit();
                break;
        }
    }
    #endregion

    #region properties
    public Transform GetTransform
    {
        get
        {
            return _transform;
        }
    }
    public Animator GetAnimator
    {
        get
        {
            return animator;
        }
    }
    public Transform BodyTransform
    {
        get
        {
            return _bodyTransform;
        }
    }
    public States CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }
    #endregion

}
