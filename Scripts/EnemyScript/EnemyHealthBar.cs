using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Image fillImage; 
    public void SetHealth(float current, float max)
    {
        
        if (fillImage != null && max > 0)
            fillImage.fillAmount = current / max;
        else
            Debug.LogWarning("fillImage is not assigned!");
    }

}
