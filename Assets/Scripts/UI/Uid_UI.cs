using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Uid_UI : MonoBehaviour
{
    public TMP_Text uidText;
    void Update()
    {
        uidText.text = $"uid: {GameData.Instance.uid}";
    }
}
