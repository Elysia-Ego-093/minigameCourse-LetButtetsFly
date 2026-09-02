using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS_Attack_2 : MonoBehaviour
{
    private GameObject boss;
    private int Damage_id;
    private float Timer = 3f;
    void Start()
    {
        Damage_id = 14;
        Timer = 3f;
    }

    void Update()
    {
        transform.Rotate(new Vector3(1000f, 0, 0));
        if(boss != null)
        {
            transform.position = boss.transform.position;
        }
        if (Timer > 0f)
        {
            Timer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetStatus(GameObject boss)
    {
        this.boss = boss;
    }
    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null && !player.IsSprinting())
            {
                player.Attacked(Damage_id, 10, new Vector2(-5f, 5f));
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), true);
                yield return new WaitForSeconds(0.05f);
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), player.GetComponent<Collider2D>(), false);
            }
        }
    }
}
