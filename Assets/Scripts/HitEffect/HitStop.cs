using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public bool canHitStop = true;
    public float duration = 0.1f;
    Coroutine hitStopCoroutine;

    public void Stop()
    {
        if (canHitStop)
        {
            Debug.Log("HitStop is called");

            if (hitStopCoroutine != null)
            {
                StopCoroutine(hitStopCoroutine);
            }

            Time.timeScale = 0.0f;
            hitStopCoroutine = StartCoroutine(Wait(duration));
        }
    }

    IEnumerator Wait(float duration)
    {
        canHitStop = false;
        yield return new WaitForSecondsRealtime(duration);  // Sử dụng thời gian thực
        Time.timeScale = 1.0f;
        canHitStop = true;
    }
}
