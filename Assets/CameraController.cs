using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public float distance = 4f;
    public float height = 1.8f;
    public float damping = 5f;
    public float rotationDamping = 3f;

    [Header("Collision")]
    public LayerMask obstacleMask;
    public float minDistance = 1f;

    private Vector3 _offset;

    void LateUpdate()
    {
        if (target == null) return;

        // 计算理想位置
        Vector3 desiredPosition = target.position +
                               target.rotation * new Vector3(0, height, -distance);

        // 镜头碰撞检测
        if (Physics.Linecast(target.position + Vector3.up * height, desiredPosition, out RaycastHit hit, obstacleMask))
        {
            desiredPosition = hit.point + hit.normal * 0.2f;
        }

        // 平滑移动
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            damping * Time.deltaTime);

        // 平滑旋转（保持一定高度注视）
        Quaternion targetRotation = Quaternion.LookRotation(
            (target.position + Vector3.up * height * 0.5f) - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationDamping * Time.deltaTime);
    }
}
