using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPivot : MonoBehaviour
{
    public GameObject pivot;

    // Update is called once per frame
    void Update()
    {
        transform.position = pivot.transform.position;
    }
}
