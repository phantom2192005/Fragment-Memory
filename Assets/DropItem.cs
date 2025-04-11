using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject prefab;
    [Range(0f, 1f)]
    public float dropChance = 1f; // Tỷ lệ rơi của item này
}

public class DropItem : MonoBehaviour
{
    [Header("Danh sách vật phẩm có thể rơi")]
    public List<LootItem> lootItems;

    [Header("Số lượng item sẽ rơi (random từ min đến max)")]
    public int minDrop = 1;
    public int maxDrop = 3;

    [Header("Bán kính rơi")]
    public float dropSpread = 1.0f;

    public void DropLoot()
    {
        if (lootItems.Count == 0) return;

        int dropCount = Random.Range(minDrop, maxDrop + 1);
        List<Vector2> usedOffsets = new List<Vector2>();

        for (int i = 0; i < dropCount; i++)
        {
            GameObject loot = GetRandomLoot();
            if (loot == null) continue;

            // Sinh vị trí rơi ngẫu nhiên, tránh trùng vị trí
            Vector2 dropOffset;
            int attempts = 0;
            do
            {
                float angle = Random.Range(-90f, 90f); // Rơi trong nửa hình quạt phía trước
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                float distance = Random.Range(dropSpread * 0.5f, dropSpread);
                dropOffset = direction.normalized * distance;

                attempts++;
                if (attempts > 10) break;
            }
            while (usedOffsets.Exists(pos => Vector2.Distance(pos, dropOffset) < 0.3f));

            usedOffsets.Add(dropOffset);

            Vector3 dropPos = transform.position + (Vector3)dropOffset;
            Instantiate(loot, dropPos, Quaternion.identity);
        }
    }

    private GameObject GetRandomLoot()
    {
        List<LootItem> possibleDrops = new List<LootItem>();
        foreach (var item in lootItems)
        {
            if (Random.value <= item.dropChance)
                possibleDrops.Add(item);
        }

        if (possibleDrops.Count == 0)
            return null;

        return possibleDrops[Random.Range(0, possibleDrops.Count)].prefab;
    }
}
