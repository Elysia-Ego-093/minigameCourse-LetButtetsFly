using UnityEngine;
using UnityEngine.UI;

public class BuffIcon : MonoBehaviour
{
    [Header("Õº±Í")]
    public Image icon;

    [Header("¿‰»¥’⁄’÷")]
    public Image mask;

    private readonly string iconFloderPath = "picture/Buff";
    public float maxTime;
    public string buffName;

    public void SetImage(string name)
    {
        string fullPath = $"{iconFloderPath}/{name}";
        Sprite newSprite = Resources.Load<Sprite>(fullPath);
        icon.sprite = newSprite;
    }
    
}
