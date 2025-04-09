using UnityEngine;
using System.Collections.Generic;

public class HitBoxHandler : MonoBehaviour
{
    [SerializeField] public List<GameObject> hitBoxPrefabs = new List<GameObject>();
    private Dictionary<string, Collider2D> hitBoxDict = new Dictionary<string, Collider2D>();
    [SerializeField] SpriteRenderer spriteRenderer;

    private string lastHitBoxName = null;
    private PlayerController player;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();

        if (player == null)
        {
            Debug.LogError("Không tìm thấy PlayerController!");
        }
    }

    public void InitializeHitBoxes(Transform parent)
    {
        foreach (var prefab in hitBoxPrefabs)
        {
            if (prefab != null)
            {
                GameObject newHitBox = Instantiate(prefab, parent);
                newHitBox.SetActive(true);

                Collider2D collider = newHitBox.GetComponent<Collider2D>();
                if (collider != null)
                {
                    hitBoxDict[newHitBox.name] = collider;
                    collider.enabled = false; // Mặc định tắt
                }
                else
                {
                    Debug.LogWarning($"Prefab {prefab.name} không có Collider2D!");
                }
            }
        }
    }

    public void DestroyAllHitBoxs(Transform parent)
    {
        foreach (Transform hitBox in parent)
        {
            Destroy(hitBox.gameObject);
        }
    }


    public void EnableHitBox(string attackName)
    {
        if (player == null)
        {
            Debug.LogError("PlayerController chưa được gán!");
            return;
        }

        string attackDirection = player.GetAttackDirection();
        string hitBoxKey = $"{attackName} {attackDirection} hitBox(Clone)";

        if (hitBoxDict.TryGetValue(hitBoxKey, out Collider2D hitBox))
        {
            hitBox.enabled = true;
            lastHitBoxName = hitBoxKey;
        }
        else
        {
            Debug.LogWarning($"Không tìm thấy HitBox: {hitBoxKey}");
        }
    }

    public void DisableHitBox()
    {
        if (!string.IsNullOrEmpty(lastHitBoxName) && hitBoxDict.TryGetValue(lastHitBoxName, out Collider2D hitBox))
        {
            hitBox.enabled = false;
            lastHitBoxName = null;
        }
    }
}
