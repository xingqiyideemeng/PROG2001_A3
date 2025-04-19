using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f; // 面向移动方向的旋转速度
    public bool cameraRelativeMovement = true; // 基于相机的移动

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float gravityMultiplier = 2f;
    private float _gravity = -9.81f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;
    private Transform _cameraTransform;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _gravity *= gravityMultiplier;
        _cameraTransform = Camera.main.transform;
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
            _velocity.y = -0.5f;
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
            // 基于相机的移动方向
            Vector3 moveDirection = cameraRelativeMovement
                ? GetCameraRelativeDirection(input)
                : input;

            // 移动角色
            _controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // 平滑转向移动方向
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    Vector3 GetCameraRelativeDirection(Vector3 input)
    {
        // 获取相机前方方向（忽略Y轴）
        Vector3 cameraForward = Vector3.Scale(_cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        return input.z * cameraForward + input.x * _cameraTransform.right;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(2 * Mathf.Abs(_gravity) * jumpHeight);
        }
    }

    void ApplyGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}