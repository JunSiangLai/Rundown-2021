using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

/// <summary>
/// Class that handles manipulating Player's transform.position
/// </summary>
public class PlayerActions : NetworkBehaviour, PlayerInput.ICharacterActions
{
    [Range(1, 100)]
    [Tooltip("Change to adjust speed of Player")]
    public int moveSpd;

    // Character Animator
    public Animator animator;

    // Current direction player is moving
    private Vector2 moveDirection;

    // Player Input
    private PlayerInput input;

    private Dictionary<string,AudioSource> sources;

    //private AudioManager audioManager;

    private void Start()
    {
        if (!isLocalPlayer)
            return;

        // Init
        Init();

        // EventListeners for accepting input 
       input.Character.SetCallbacks(this);

        // Enable Input
        input.Enable();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!isLocalPlayer)
            return;

        // Update position of player
        UpdateMovement();
        // Update animation of player
        UpdateAnimator();
    }

    private void Init()
    {
        moveSpd = 10;
        moveDirection = Vector2.zero;
        input = new PlayerInput();

        //audioManager = AudioManager.instance;
        sources = new Dictionary<string, AudioSource>();

        foreach (AudioElement source in gameObject.transform.Find("AudioSources").GetComponentsInChildren<AudioElement>())
        {
            sources.Add(source.gameObject.name, source.gameObject.GetComponent<AudioSource>());
        }

        GameObject audioListenterPrefab = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<NetManager>().audioListenterPrefab;
        GameObject.Instantiate(audioListenterPrefab, transform);
    }

    /// <summary>
    /// Updates player's transform.position
    /// </summary>
    private void UpdateMovement()
    {
        transform.position += moveSpd * Time.deltaTime * new Vector3(moveDirection.x, 0, moveDirection.y);
    }

    /// <summary>
    /// Updates player's animator
    /// </summary>
    private void UpdateAnimator()
    {
        if (moveDirection == Vector2.zero)
        {
            animator.SetBool("isWalking", false);
            //EventSystem.RaiseEvent(EventSystem.EventsTYPE.soundMade, 1);
        }
        else
        {
            animator.SetBool("isWalking", true);
            //EventSystem.RaiseEvent(EventSystem.EventsTYPE.soundMade, 3);
        }
    }

    /// <summary>
    /// Handles WASD input
    /// <para>WASD read as Vector 2</para>
    /// </summary>
    /// <param name="context">Info from Input System</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Handles SHIFT input
    /// <para>SHIFT read as Button</para>
    /// </summary>
    /// <param name="context">Info from Input System</param>
    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            moveSpd = 20;
            animator.SetBool("isRunning", true);
            bool[] Play = { true };
            if (sources.TryGetValue("FootSteps", out AudioSource source))
                AudioManager.instance.AudioControlCommand(netId, "FootSteps", true, Play);
        }
        else
        {
            animator.SetBool("isRunning", false);
            moveSpd = 10;
            bool[] Stop = { false, true };
            if (sources.TryGetValue("FootSteps", out AudioSource source))
                AudioManager.instance.AudioControlCommand(netId, "FootSteps", true, Stop);
        }
    }
}
