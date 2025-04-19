using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f; // �����ƶ��������ת�ٶ�
    public bool cameraRelativeMovement = true; // ����������ƶ�

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
            // ����������ƶ�����
            Vector3 moveDirection = cameraRelativeMovement
                ? GetCameraRelativeDirection(input)
                : input;

            // �ƶ���ɫ
            _controller.Move(moveDirection * moveSpeed * Time.deltaTime);

            // ƽ��ת���ƶ�����
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
        // ��ȡ���ǰ�����򣨺���Y�ᣩ
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