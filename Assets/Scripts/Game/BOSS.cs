using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BOSS : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected CircleCollider2D col;

    public float maxHP = 10000f;
    private float HP;
    private float MoveSpeed = 3f;

    [Header("“∆∂Ø∑∂Œß")]
    public Vector2 leftLowerCorner;
    public Vector2 RightUpperCorner;

    private float ChangeVelocityTime = 5f;
    private float ChangeVelocityTimer = 0f;

    [Header("BOSS_bullet")]
    public GameObject bullet1;
    public GameObject bullet2;
    public GameObject Boomb1;
    public GameObject Boomb2;
    public GameObject BOSS_SpecialAttack_fire;
    public GameObject BOSS_SpecialAttack_dark;
    public GameObject BOSS_SpecialAttack_0;
    public GameObject BOSS_Explosive_Final;
    private float AttackTimer = 0f;

    [Header("“Ù–ß")]
    private AudioSource audioSource;
    public AudioClip BeginAttack;
    public AudioClip FinalAttack;
    public AudioClip Attack1;
    public AudioClip Attack2;
    public AudioClip Attack3;
    public AudioClip Attack4;
    public AudioClip Attack5;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        HP = maxHP;
        //Game_api.Instance.RequestInitialStatus(2, -1);
        //Game_api.Instance.RequestGameStart();
        AttackTimer = 100f;
        GameData.Instance.TimeFinish = false;
        StartCoroutine(Begin_Attack());
    }

    void Update()
    {
        UpdateSpeed();
        audioSource.volume = GameData.Instance.SoundVolume * 0.3f;
        if (AttackTimer > 0f)
        {
            AttackTimer -= Time.deltaTime;
        }
        else
        {
            AttackTimer = 100f;
            int index = Random.Range(0, 5);
            if (index == 0) StartCoroutine(attack_1());
            if (index == 1) StartCoroutine(attack_2());
            if (index == 2) StartCoroutine(attack_3());
            if (index == 3) StartCoroutine(attack_4());
            if (index == 4) StartCoroutine(attack_5());
        }
    }

    private void UpdateSpeed()
    {
        float vx = rb.velocity.x;
        float vy = rb.velocity.y;
        if (transform.position.x < leftLowerCorner.x) rb.velocity = new Vector2(Mathf.Abs(vx), vy);
        if (transform.position.y < leftLowerCorner.y) rb.velocity = new Vector2(vx, Mathf.Abs(vy));
        if (transform.position.x > RightUpperCorner.x) rb.velocity = new Vector2(-1.0f * Mathf.Abs(vx), vy);
        if (transform.position.y > RightUpperCorner.y) rb.velocity = new Vector2(vx, -1.0f * Mathf.Abs(vy));
        if (ChangeVelocityTimer > 0f)
        {
            ChangeVelocityTimer -= Time.deltaTime;
        }
        else
        {
            ChangeVelocityTimer = ChangeVelocityTime;
            float angle = Random.Range(0f, 2.0f * Mathf.PI);
            float new_x = MoveSpeed * Mathf.Cos(angle);
            float new_y = MoveSpeed * Mathf.Sin(angle);
            rb.velocity = new Vector2(new_x, new_y);
        }
    }

    private void ShootRoundBullets(int BulletCnt)
    {
        for(int i = 0; i < BulletCnt; i++)
        {
            GameObject bullet = Instantiate(bullet1, transform.position + new Vector3(col.offset.x - (col.radius + 1f), 0, 0), Quaternion.identity);
            float angle = 2f / 3f * Mathf.PI + (2f / 3f) * ((float)i / BulletCnt) * Mathf.PI;
            bullet.GetComponent<Bullet>().BOSS_SetStatus(11, 20f, 50f, angle, col);
        }
        
    }
    private void ShootRandomBullets()
    {
        GameObject bullet = Instantiate(bullet1, transform.position + new Vector3(col.offset.x - (col.radius + 1f), 0, 0), Quaternion.identity);
        float angle = Random.Range(2f / 3f * Mathf.PI, 4f / 3f * Mathf.PI);
        bullet.GetComponent<Bullet>().BOSS_SetStatus(11, 20f, 50f, angle, col);
    }

    private IEnumerator ShootBullet2s(int cnt)
    {
        for (int i = 0; i < cnt / 2; i++)
        {
            float angle1 = Random.Range(1f / 3f, 2f / 3f) * Mathf.PI;
            Vector2 BulletPosition1 = (Vector2)transform.position + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * col.radius;
            GameObject Bullet1 = Instantiate(bullet2, BulletPosition1, Quaternion.identity);
            Bullet1.GetComponent<Bullet>().BOSS_SetStatus(12, 10f, 50f, angle1, col);
            float angle2 = Random.Range(4f / 3f, 5f / 3f) * Mathf.PI;
            Vector2 BulletPosition2 = (Vector2)transform.position + new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) * col.radius;
            GameObject Bullet2 = Instantiate(bullet2, BulletPosition2, Quaternion.identity);
            Bullet2.GetComponent<Bullet>().BOSS_SetStatus(12, 10f, 50f, angle2, col);
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void Attack_fire()
    {
        GameObject newAttack = Instantiate(BOSS_SpecialAttack_fire, transform.position + new Vector3(-col.radius, col.radius, 0), Quaternion.identity);
        newAttack.GetComponent<BOSS_Attack>().SetStatus(col);
    }
    private void Attack_dark()
    {
        GameObject newAttack = Instantiate(BOSS_SpecialAttack_dark, transform.position + new Vector3(-col.radius, -col.radius, 0), Quaternion.identity);
        newAttack.GetComponent<BOSS_Attack>().SetStatus(col);
    }

    private void Attack_power()
    {
        GameObject newAttack = Instantiate(BOSS_SpecialAttack_0, transform.position, Quaternion.identity);
        newAttack.GetComponent<BOSS_Attack_2>().SetStatus(gameObject);
    }

    private IEnumerator attack_1()
    {
        audioSource.clip = Attack1;
        audioSource.Play();
        ShootRoundBullets(25);
        yield return new WaitForSeconds(0.3f);
        ShootRoundBullets(30);
        yield return new WaitForSeconds(1f);
        ShootRoundBullets(35);
        yield return new WaitForSeconds(0.3f);
        ShootRoundBullets(30);
        yield return new WaitForSeconds(1f);
        ShootRoundBullets(50);
        AttackTimer = 5f;
    }

    private IEnumerator attack_2()
    {
        audioSource.clip = Attack2;
        audioSource.Play();
        for (int i = 0; i < 100; i++)
        {
            ShootRandomBullets();
            yield return new WaitForSeconds(0.03f);
        }
        AttackTimer = 5f;
    }

    private IEnumerator attack_3()
    {
        audioSource.clip = Attack3;
        audioSource.Play();
        for (int k = 0; k < 2; k++)
        {
            StartCoroutine(ShootBullet2s(50));
            yield return new WaitForSeconds(1f);
        }
        AttackTimer = 5f;
    }

    private IEnumerator attack_4()
    {
        for (int i = 0; i < 30; i++)
        {
            float x0 = Random.Range(0f, 150f);
            GameObject NewBoomb = Instantiate(Boomb2, new Vector2(x0, 100f), Quaternion.identity);
            int cnt = Random.Range(0, 3);
            NewBoomb.GetComponent<BOSS_Boomb_2>().SetStatus(cnt, col);
            yield return new WaitForSeconds(0.05f);
        }
        audioSource.clip = Attack4;
        audioSource.Play();
        for (int i = 0; i < 70; i++)
        {
            float x0 = Random.Range(0f, 150f);
            GameObject NewBoomb = Instantiate(Boomb2, new Vector2(x0, 100f), Quaternion.identity);
            int cnt = Random.Range(0, 3);
            NewBoomb.GetComponent<BOSS_Boomb_2>().SetStatus(cnt, col);
            yield return new WaitForSeconds(0.05f);
        }
        AttackTimer = 5f;
    }
    private IEnumerator attack_5()
    {
        audioSource.clip = Attack5;
        audioSource.Play();
        yield return new WaitForSeconds(4f);
        Attack_fire();
        Attack_dark();
        yield return new WaitForSeconds(3f);
        Attack_power();
        AttackTimer = 8f;
    }
    private IEnumerator Begin_Attack()
    {
        audioSource.clip = BeginAttack;
        audioSource.Play();
        List<BOSS_Boomb_1> Boombs = new List<BOSS_Boomb_1>();
        for(int i = 0; i < 4; i++)
        {
            float angle = (1f / 3f) * Mathf.PI - (2f / 3f) * Mathf.PI / 4f * i;
            GameObject NewBoomb = Instantiate(Boomb1, transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * 20f, Quaternion.identity);
            Boombs.Add(NewBoomb.GetComponent<BOSS_Boomb_1>());
            Boombs[i].SetStatus(col);
        }
        yield return new WaitForSeconds(2f);
        foreach(var boomb in Boombs)
        {
            boomb.Attack();
            yield return new WaitForSeconds(0.5f);
        }
        AttackTimer = 5f;
    }

    private IEnumerator Final_attack()
    {
        audioSource.Stop();
        GameObject explosive = Instantiate(BOSS_Explosive_Final, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        audioSource.clip = FinalAttack;
        audioSource.Play();
        yield return new WaitForSeconds(6f);
        GameObject Player1;
        GameObject Player2;
        for (int i = 0; i < 30; i++)
        {
            AttackTimer = 100f;
            Player1 = GameObject.Find("Player1");
            Player2 = GameObject.Find("Player2");
            if (Player1 != null) Player1.GetComponent<BasePlayerController>().Attacked(33550335, 5f, new Vector2(-2f, 2f));
            if (Player2 != null) Player2.GetComponent<BasePlayerController>().Attacked(33550335, 5f, new Vector2(-2f, 2f));
            yield return new WaitForSeconds(0.1f);
        }
        Player1 = GameObject.Find("Player1");
        Player2 = GameObject.Find("Player2");
        if (Player1 != null) Player1.GetComponent<BasePlayerController>().Attacked(33550336, 33550336f, new Vector2(-2f, 2f));
        if (Player2 != null) Player2.GetComponent<BasePlayerController>().Attacked(33550336, 33550336f, new Vector2(-2f, 2f));
        yield return new WaitForSeconds(6f);
        Game_api.Instance.RequestGameOver();
        SceneManager.LoadScene("BOSS_Lose");
    }


    public void attcked(int id,float atk)
    {
        HP -= atk;
        //Game_api.Instance.RequestStatus(new StatusInformation(2, id, HP, 0f));
    }

    public void TimeFinishAttack()
    {
        GameData.Instance.TimeFinish = true;
        StartCoroutine(Final_attack());
    }

    public void StatusResponse(float currentHP) { HP = currentHP; }
    
    public float getHP()
    {
        return HP;
    }
}
