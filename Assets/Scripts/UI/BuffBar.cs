using System.Collections.Generic;
using UnityEngine;

public class BuffBar : MonoBehaviour
{
    [Header("buffͼ��Ԥ����")]
    public GameObject buffIconPrefab;

    [Header("���")]
    public Player_UI_Manager player_UI;

    private PlayerController player;



    private Dictionary<string, BuffIcon> icons = new Dictionary<string, BuffIcon>();
    private Dictionary<string, float> buffs = new Dictionary<string, float>();
    private List<string> buffNames = new List<string>();
    void Start()
    {
        player = player_UI.player;
    }

    
    void Update()
    {
        buffs = player.GetBuffs();
        UpdateBuff();
    }

    private void UpdateBuff()
    {
        List<string> iconsToRemove = new List<string>();
        foreach(var buff in buffs)
        {
            if (icons.ContainsKey(buff.Key))
            {
                if (buff.Value <= 0)
                {
                    Destroy(icons[buff.Key].gameObject);
                    iconsToRemove.Add(buff.Key);
                }
                else icons[buff.Key].mask.fillAmount = 1.0f * buff.Value / icons[buff.Key].maxTime;
            }
            else if (buff.Value > 0) 
            {
                GameObject newBuff = Instantiate(buffIconPrefab, transform.position, Quaternion.identity);
                BuffIcon newIcon = newBuff.GetComponent<BuffIcon>();
                newIcon.buffName = buff.Key;
                newIcon.maxTime = buff.Value;
                newIcon.SetImage(buff.Key);
                newIcon.transform.SetParent(transform, false);
                icons[buff.Key] = newIcon;
                buffNames.Add(buff.Key);
            }
        }
        foreach(var iconToRemove in iconsToRemove)
        {
            icons.Remove(iconToRemove);
            buffNames.Remove(iconToRemove);
        }

        int index = 0;
        foreach(var buffname in buffNames)
        {
            BuffIcon icon = icons[buffname];
            icon.transform.position = new Vector2(transform.position.x + 3.0f * index * icon.transform.localScale.x, transform.position.y);
            index++;
        }
    }
    
}
