using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Tao cac trang thai cho enemy
public enum State
{
    Roaming,
    Attacking
}

public class EnemyAI : MonoBehaviour
{
    // Roam
    [Header("# Roam infos")]
    [SerializeField] private float roamChangeDirFloat = 2f;
    private Vector2 roamPosition;
    private float timeRoaming = 0f;


    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;



    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }

    private void Start()
    {
        // Dat che do mac dinh la roam
        state = State.Roaming;
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        // Cap nhat state
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Roaming()
    {
        // THoi gian de roam
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPosition);
        // NEu trong vun  tan cong thi tan cong
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }
        // Tiep tuc roam
        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        // Neu ngoai vung tan con thi khong tan cong
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
        }
        // Tan cong
        if (attackRange != 0 && canAttack)
        {

            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            else
            {
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
