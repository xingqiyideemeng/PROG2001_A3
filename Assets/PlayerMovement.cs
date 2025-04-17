using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    [Tooltip("旋转平滑度")]
    public float rotationSmoothness = 10f;

    [Header("Jump Settings")]
    [Tooltip("建议值：3-10")]
    public float jumpHeight = 20f;  // 增大默认值
    [Tooltip("重力倍数")]
    public float gravityMultiplier = 20f;  // 新增参数控制重力强度
    private float _gravity = -9.81f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _gravity *= gravityMultiplier;  // 应用重力倍数
    }

    void Update()
    {
        HandleGrounding();
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    void HandleGrounding()
    {
        _isGrounded = _controller.isGrounded;
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -0.5f;  // 微小的负值保持贴地
        }
    }

    void HandleMovement()
    {
        Vector3 input = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        ).normalized;

        if (input.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(input);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSmoothness * Time.deltaTime
            );
        }

        _controller.Move(input * moveSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            // 更直观的跳跃公式
            _velocity.y = Mathf.Sqrt(2 * Mathf.Abs(_gravity) * jumpHeight);
        }
    }

    void ApplyGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}