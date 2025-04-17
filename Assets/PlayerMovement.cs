using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    [Tooltip("��תƽ����")]
    public float rotationSmoothness = 10f;

    [Header("Jump Settings")]
    [Tooltip("����ֵ��3-10")]
    public float jumpHeight = 20f;  // ����Ĭ��ֵ
    [Tooltip("��������")]
    public float gravityMultiplier = 20f;  // ����������������ǿ��
    private float _gravity = -9.81f;

    private CharacterController _controller;
    private Vector3 _velocity;
    private bool _isGrounded;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _gravity *= gravityMultiplier;  // Ӧ����������
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
            _velocity.y = -0.5f;  // ΢С�ĸ�ֵ��������
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
            // ��ֱ�۵���Ծ��ʽ
            _velocity.y = Mathf.Sqrt(2 * Mathf.Abs(_gravity) * jumpHeight);
        }
    }

    void ApplyGravity()
    {
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}