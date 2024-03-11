using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathfinding : MonoBehaviour
{
    // Movement
    [Header("Move infos")]
    [SerializeField] private float moveSpeed = 2f;
    private Vector2 moveDir;

    private Knockback knockback;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate() {
        // KHi bi nhan sat thuong thi se khong lam gi
        if (knockback.GettingKnockedBack) { return; }

        rb.MovePosition(rb.position + moveDir * (moveSpeed * Time.fixedDeltaTime));
        // Doi huong khi di chuyen
        if (moveDir.x < 0) {
            spriteRenderer.flipX = true;
        } else if (moveDir.x > 0) {
            spriteRenderer.flipX = false;
        }
    }
    /// <summary>
    /// Set vi tri di chuyen
    /// </summary>
    /// <param name="targetPosition"></param>
    public void MoveTo(Vector2 targetPosition) {
        moveDir = targetPosition;
    }
    /// <summary>
    /// Dung di chuyen
    /// </summary>
    public void StopMoving() {
        moveDir = Vector3.zero;
    }
}
