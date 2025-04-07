using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [field: SerializeField]
    public List<ItemParameter> itemState;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float duration = 0.3f;

    [SerializeField]
    private Transform targetAbsorb;

    [SerializeField]
    private float AbsorbSpeed = 2.0f;

    public bool canAbsorbAll;


    private void Start()
    {
        //Debug.Log("Start item is call");
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
        if (itemState == null)
        {
            PrepareItemState(InventoryItem.DefaultParametersList);
        }
    }

    public void PrepareItemState(List<ItemParameter> itemStates)
    {
        itemState = new List<ItemParameter>();
        foreach (var itemParameter in itemStates)
        {
            itemState.Add(itemParameter);
        }
    }

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());

    }

    private IEnumerator AnimateItemPickup()
    {
        audioSource.PlayOneShot(audioSource.clip);
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale =
                Vector3.Lerp(startScale, endScale, currentTime / duration);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D triggger)
    {
        if (triggger.tag == "PlayerDetect")
        {
            targetAbsorb = triggger.transform;
        }
    }

    private void Update()
    {
        if (targetAbsorb != null && canAbsorbAll)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetAbsorb.position, AbsorbSpeed * Time.deltaTime);
            if (transform.position == targetAbsorb.transform.position)
            {
                Destroy(gameObject);
            }
        }
    }
}