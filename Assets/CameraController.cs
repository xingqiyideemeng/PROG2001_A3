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

        // ��������λ��
        Vector3 desiredPosition = target.position +
                               target.rotation * new Vector3(0, height, -distance);

        // ��ͷ��ײ���
        if (Physics.Linecast(target.position + Vector3.up * height, desiredPosition, out RaycastHit hit, obstacleMask))
        {
            desiredPosition = hit.point + hit.normal * 0.2f;
        }

        // ƽ���ƶ�
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            damping * Time.deltaTime);

        // ƽ����ת������һ���߶�ע�ӣ�
        Quaternion targetRotation = Quaternion.LookRotation(
            (target.position + Vector3.up * height * 0.5f) - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationDamping * Time.deltaTime);
    }
}
