using UnityEngine;

public class CoinController : MonoBehaviour
{
    public Vector3 rot; // 旋转速度
    public AudioClip collectSound; // 单个金币收集音效

    // 静态变量跟踪金币状态
    private static int totalCoins;
    private static int collectedCoins;

    void Start()
    {
        // 只在第一个金币初始化时统计总数
        if (totalCoins == 0)
        {
            totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        }
    }

    void Update()
    {
        transform.Rotate(rot); // 保持旋转功能
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
        // 播放收集音效
        if (collectSound != null)
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        collectedCoins++;
        Destroy(gameObject);

        // 检查是否收集完毕
        if (collectedCoins >= totalCoins)
        {
            // 触发胜利音效
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayVictorySound();
            }
            else
            {
                Debug.LogWarning("AudioManager未找到，请确保已添加到场景");
            }
        }
        Debug.Log($"收集金币 {collectedCoins}/{totalCoins}");

        if (collectedCoins >= totalCoins)
        {
            Debug.Log("尝试播放胜利音效");
            if (AudioManager.Instance == null)
                Debug.LogError("AudioManager实例丢失");
            else
                AudioManager.Instance.PlayVictorySound();
        }
    }
}