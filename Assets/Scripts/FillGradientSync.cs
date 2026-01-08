using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class FillGradientSync : MonoBehaviour
{
    public Image image;

    void Update()
    {
        if (image != null && image.material != null)
        {
            image.material.SetFloat("_Fill", image.fillAmount);
        }
    }
}