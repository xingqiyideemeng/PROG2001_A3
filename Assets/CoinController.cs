using UnityEngine;

public class CoinController : MonoBehaviour
{
    public Vector3 rot; // ��ת�ٶ�
    public AudioClip collectSound; // ��������ռ���Ч

    // ��̬�������ٽ��״̬
    private static int totalCoins;
    private static int collectedCoins;

    void Start()
    {
        // ֻ�ڵ�һ����ҳ�ʼ��ʱͳ������
        if (totalCoins == 0)
        {
            totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        }
    }

    void Update()
    {
        transform.Rotate(rot); // ������ת����
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    void CollectCoin()
    {
        // �����ռ���Ч
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        collectedCoins++;
        Destroy(gameObject);

        // ����Ƿ��ռ����
        if (collectedCoins >= totalCoins)
        {
            // ����ʤ����Ч
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayVictorySound();
            }
            else
            {
                Debug.LogWarning("AudioManagerδ�ҵ�����ȷ������ӵ�����");
            }
        }
        Debug.Log($"�ռ���� {collectedCoins}/{totalCoins}");

        if (collectedCoins >= totalCoins)
        {
            Debug.Log("���Բ���ʤ����Ч");
            if (AudioManager.Instance == null)
                Debug.LogError("AudioManagerʵ����ʧ");
            else
                AudioManager.Instance.PlayVictorySound();
        }
    }
}