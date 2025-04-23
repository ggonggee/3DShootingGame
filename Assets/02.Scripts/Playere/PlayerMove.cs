using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //[Header("UI")]
    //public Slider StaminaSlider;

    [Header("Jump")]
    public float JumpPower = 8f;
    public int MaxJumpCount = 2;
    private int _jumpCount = 0;

    [Header("Movement")]
    public float WalkSpeed = 5f;
    public float SprintSpeed = 9f;
    public float ClimbSpeed = 3f;

    [Header("Stamina")]
    public float Stamina = 5f;
    public float MaxStamina = 5f;
    public float DrainPerSecondClimb = 1f;
    public float DrainPerSecondSprint = 1.5f;
    public float RecoverPerSecond = 2f;

    [Header("Roll")]
    public float RollSpeed = 15f;
    public float RollDuration = 0.3f;
    public float RollStaminaCost = 1.5f;
    private bool _isRolling = false;
    private float _rollTimer = 0f;
    private Vector3 _rollDirection;

    private const float _gravity = -9.8f;
    private float _yVelocity = 0f;

    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        UIManager.Instance.SetStamina(Stamina, MaxStamina);
    }

    private void Update()
    {
        UIManager.Instance.SetStamina(Stamina, MaxStamina);

        bool isTouchingWall = (_controller.collisionFlags & CollisionFlags.Sides) != 0;
        bool isGrounded = (_controller.collisionFlags & CollisionFlags.Below) != 0;
        bool isClimbing = isTouchingWall && Input.GetKey(KeyCode.W) && Stamina > 0f;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v).normalized;
        dir = Camera.main.transform.TransformDirection(dir);

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && Stamina > 0f && dir.magnitude > 0.1f;
        float moveSpeed = isSprinting ? SprintSpeed : WalkSpeed;

        if (Input.GetKeyDown(KeyCode.E) && Stamina >= RollStaminaCost && !_isRolling && dir.magnitude > 0.1f)
        {
            _isRolling = true;
            _rollTimer = RollDuration;
            _rollDirection = dir;
            Stamina -= RollStaminaCost;
        }

        if (_isRolling)
        {
            dir = _rollDirection * RollSpeed;
            _rollTimer -= Time.deltaTime;

            if (_rollTimer <= 0f)
            {
                _isRolling = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && _jumpCount < MaxJumpCount)
        {
            _yVelocity = JumpPower;
            _jumpCount++;
        }

        if (isSprinting)
        {
            Stamina -= DrainPerSecondSprint * Time.deltaTime;
        }

        if (isClimbing)
        {
            dir = Vector3.up * ClimbSpeed;
            Stamina -= DrainPerSecondClimb * Time.deltaTime;

            if (Stamina <= 0f)
            {
                dir.y = 0;
            }
        }
        else
        {
            _yVelocity += _gravity * Time.deltaTime;
            dir.y = _yVelocity;
        }

        if (isGrounded)
        {
            _jumpCount = 0;

            if (!isSprinting)
            {
                Stamina += RecoverPerSecond * Time.deltaTime;
                if (Stamina > MaxStamina)
                {
                    Stamina = MaxStamina;
                }
            }
        }

        _controller.Move(dir * moveSpeed * Time.deltaTime);
    }
}
