using UnityEngine;

public class mainCamrera : MonoBehaviour
{
    PlayerController player;
    
    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
