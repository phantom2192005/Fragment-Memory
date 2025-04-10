using UnityEngine;

public class DropLootSimple : MonoBehaviour
{
    [Header("Danh sách vật phẩm có thể rơi")]
    public GameObject[] lootPrefabs;

    [Header("Số lượng item sẽ rơi (random từ min đến max)")]
    public int minDrop = 1;
    public int maxDrop = 3;

    [Header("Khoảng cách rơi")]
    public float dropSpread = 1.0f;

    public void DropLoot()
    {
        if (lootPrefabs.Length == 0) return;

        int dropCount = Random.Range(minDrop, maxDrop + 1);

        for (int i = 0; i < dropCount; i++)
        {
            GameObject loot = lootPrefabs[Random.Range(0, lootPrefabs.Length)];

            Vector2 dropOffset = new Vector2(
                Random.Range(-dropSpread, dropSpread),
                Random.Range(-dropSpread, dropSpread)
            );

            Instantiate(loot, transform.position + (Vector3)dropOffset, Quaternion.identity);
        }
    }
}
