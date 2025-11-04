using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Trace,
        Attack,
        RunAway
    }
    public EnemyState state = EnemyState.Idle;

    public int maxHP = 5;
    public int currentHP;

    public float moveSpeed = 2f;
    public float traceRange = 15f;
    public float attackRange = 6f;
    public float attackCooldown = 1.5f;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private float lastAttackTime;

    private Transform player;

    public Slider slider;

    private bool hasRunAway = false;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastAttackTime = -attackCooldown;
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        switch (state)
        {
            case EnemyState.Idle:
                if (dist < traceRange)
                    state = EnemyState.Trace;
                break;

            case EnemyState.Trace:
                if (dist < attackRange)
                    state = EnemyState.Attack;
                else if (dist > traceRange)
                    state = EnemyState.Idle;
                else
                    TracePlayer();
                break;

            case EnemyState.Attack:
                if (dist > attackRange)
                    state = EnemyState.Trace;
                else if (currentHP <= maxHP * 0.2f && !hasRunAway)
                {
                    state = EnemyState.RunAway;
                    hasRunAway = true;
                }
                else
                    AttackPlayer();
                break;

            case EnemyState.RunAway:
                if (dist > traceRange)
                    state = EnemyState.Idle;
                else
                    RunAwayFromPlayer();
                break;
        }
    }

    void TracePlayer()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(player.position);
    }

    void AttackPlayer()
    {
        if(Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootProjectile();
        }
    }

    void RunAwayFromPlayer()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + dir);
    }


    void ShootProjectile()
    {
        if(projectilePrefab != null && firePoint != null)
        {
            transform.LookAt(player.position);
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            EnemyProjectile ep = proj.GetComponent<EnemyProjectile>();
            if(ep != null)
            {
                Vector3 dir = (player.position - firePoint.position).normalized;
                ep.SetDirection(dir);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        slider.value = (float)currentHP / maxHP;
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
