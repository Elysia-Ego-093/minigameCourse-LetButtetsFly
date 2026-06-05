using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun_UI : MonoBehaviour
{
    [Header("Íæ¼Ò")]
    public Player_UI_Manager player_UI;
    private PlayerController player;

    [Header("±àºÅ")]
    public int index;

    [Header("×Óµ¯")]
    public TMP_Text ammoText;

    [Header("Ç¹Ðµ")]
    public Slider Gun;
    public Image GunPicture;
    public Image Background;
    private readonly string ImageFloderPath = "picture/Weapon";

    void Start()
    {
        player = player_UI.player;
    }

    void Update()
    {
        if (player == null || ammoText == null)
        {
            return;
        }
        UpdateAmmo();
        UpdateReload();
        UpdateBackground();
    }

    private void UpdateAmmo()
    {
        if (index >= player.guns.Count)
        {
            ammoText.text = "EMPTY";
            ammoText.color = Color.red;
            GunPicture.color = new Color(255, 255, 255, 0);
            return;
        }
        GunPicture.color = new Color(255, 255, 255, 1);
        Sprite newSprite = Resources.Load<Sprite>($"{ImageFloderPath}/{player.guns[index].data.gunName}");
        GunPicture.sprite = newSprite;
        int currentAmmo = player.guns[index].nowAmmo;
        int AmmoNum = player.guns[index].AmmoNum;
        int maxAmmo = player.guns[index].data.maxAmmo;
        bool needAmmo = player.guns[index].data.needAmmo;
        System.Console.WriteLine(AmmoNum + "\n");
        System.Console.WriteLine(needAmmo + "\n");

        if (needAmmo)
        {
            ammoText.text = $"{currentAmmo} / {AmmoNum}";

            if (currentAmmo == 0)
            {
                ammoText.color = Color.red;
            }
            else if (currentAmmo <= maxAmmo * 0.2f)
            {
                ammoText.color = Color.yellow;
            }
            else
            {
                ammoText.color = Color.white;
            }
        }
        else
        {
            // Unlimited ammo mode
            ammoText.text = "Ammo: Infinity";
            ammoText.color = Color.cyan;
        }
    }

    private void UpdateReload()
    {
        if (index != player.currentGunIndex)
        {
            Gun.value = 0;
            return;
        }
        Gun.maxValue = player.GetMaxAmmoTime();
        Gun.value = player.GetAmmoTime();
    }

    private void UpdateBackground()
    {
        if (index == player.currentGunIndex) Background.color = new Color(255, 255, 0, 0.1f);
        else Background.color = new Color(0, 0, 0, 0.5f);
    }
}
