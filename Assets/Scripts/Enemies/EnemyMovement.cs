using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    EnemyStats enemy;
    Transform player;

    [Header("Separation")]
    public float separationRadius = 1.5f;
    public float separationForce = 2f;

    void Start()
    {
        enemy = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        Vector2 direction = player.transform.position - transform.position;

        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemy.currentMoveSpeed * Time.deltaTime);

        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        foreach (Collider2D neighbor in neighbors)
        {
            if (neighbor != null && neighbor.gameObject != gameObject && neighbor.CompareTag("Enemy"))
            {
                Vector2 pushDir = transform.position - neighbor.transform.position;
                transform.position += (Vector3)(pushDir.normalized * separationForce * Time.deltaTime);
            }
        }

        if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
    }
}