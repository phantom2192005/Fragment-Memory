using TMPro;
using UnityEngine;

public class FontFixerTMP : MonoBehaviour
{
    public TMP_FontAsset[] fontAssets;

    private void Start()
    {
        foreach (var fontAsset in fontAssets)
        {
            if (fontAsset != null && fontAsset.material != null)
            {
                fontAsset.material.mainTexture.filterMode = FilterMode.Point;
            }
        }
    }
}
