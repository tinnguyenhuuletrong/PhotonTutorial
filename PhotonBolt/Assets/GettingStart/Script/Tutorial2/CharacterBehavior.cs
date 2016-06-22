using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Cameras;
using Bolt;

public class CharacterBehavior : Bolt.EntityBehaviour<ICharacter>
{
    private TutorialCharacterController m_Character; // A reference to the ThirdPersonCharacter on the object
    private Rigidbody m_Rigidbody;
    Animator m_Animator;

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;

    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private float m_Horizontal;
    private float m_Vertical;
    private bool m_Crouch;
    private bool m_Walk;

    public override void Attached()
    {
        state.SetTransforms(state.Transform, transform);
        state.SetAnimator(GetComponent<Animator>());

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<TutorialCharacterController>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        m_Cam = Camera.main.transform;

        //Disable Component for Guest object
        if(!entity.isControllerOrOwner)
        {
            m_Character.enabled = false;
        }

    }

    public override void ControlGained()
    {
        m_Character.enabled = true;

        base.ControlGained();
    }

    public override void ControlLost()
    {
        m_Character.enabled = false;

        base.ControlLost();
    }

    private void Update()
    {
        UpdateInput();
    }

    /// <summary>
    /// Update Loop for Controller Mode
    ///     Generate and Send Input Command to Owner
    /// </summary>
    public override void SimulateController()
    {
        GenerateInputCommand();

        base.SimulateController();
    }

    /// <summary>
    /// Execute Command
    ///     Called on Owner for authorize and Controller for predictable
    /// </summary>
    /// <param name="command"></param>
    /// <param name="resetState"></param>
    public override void ExecuteCommand(Command command, bool resetState)
    {
        CharacterCmd cmd = command as CharacterCmd;

        if (resetState)
        {
            m_Character.SetState(cmd.Result.Position, cmd.Result.Rotation, cmd.Result.Velocity, cmd.Result.IsGround, cmd.Result.JumpFrame);
        }
        else
        {
            TutorialCharacterController.State outputState = m_Character.Move(cmd.Input.Move, cmd.Input.Jump);

            //Update cmd Result
            cmd.Result.Position = outputState.position;
            cmd.Result.Velocity = outputState.velocity;
            cmd.Result.Rotation = outputState.rotation;
            cmd.Result.IsGround = outputState.isGrounded;
            cmd.Result.JumpFrame = outputState.jumpFrames;

            if (cmd.IsFirstExecution)
            {
                AnimatePlayer(cmd, outputState);
            }
        }

        base.ExecuteCommand(command, resetState);
    }

    //Animating player
    private void AnimatePlayer(CharacterCmd cmd, TutorialCharacterController.State outputState)
    {
        // FWD <> BWD movement
        Vector3 relativeMove = transform.InverseTransformDirection(cmd.Input.Move);
        m_Animator.SetFloat("Forward", relativeMove.z, 0.1f, Time.deltaTime);

        float m_TurnAmount = Mathf.Atan2(relativeMove.x, relativeMove.z);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);

        if(outputState.isGrounded)
        {
            m_Animator.SetBool("OnGround", true);
            m_Animator.SetFloat("Jump", 0);
        } else
        {
            m_Animator.SetBool("OnGround", false);
            m_Animator.SetFloat("Jump", outputState._jumpProgress);
        }
        

    }

    /// <summary>
    /// Update Input Key
    /// </summary>
    private void UpdateInput()
    {
        if (!m_Jump)
        {
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        }

        m_Horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        m_Vertical = CrossPlatformInputManager.GetAxis("Vertical");
        m_Crouch = Input.GetKey(KeyCode.C);
        m_Walk = Input.GetKey(KeyCode.LeftShift);
    }

    /// <summary>
    /// Generate Input Command from Controller
    /// </summary>
    private void GenerateInputCommand()
    {
        // read inputs
        float h = m_Horizontal;
        float v = m_Vertical;
        bool crouch = m_Crouch;

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
#if !MOBILE_INPUT
        // walk speed multiplier
        if (m_Walk) m_Move *= 0.5f;
#endif

        ICharacterCmdInput input = CharacterCmd.Create();

        input.Move = m_Move;
        input.Jump = m_Jump;
        input.Crouch = crouch;

        entity.QueueInput(input);

        m_Jump = false;
    }
}
