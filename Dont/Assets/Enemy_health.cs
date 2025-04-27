using UnityEngine;

public class Enemy_health : MonoBehaviour
{
    [Header("Health")]
    public float health;
    public bool invulnerable;

    [Header("Death Effects")]
    public GameObject deatheffect;

    [Header("General Drop")]
    public GameObject drop;
    [Range(0f, 1f)]
    public float dropChance = 0.5f;

    [Header("Coin Drop Settings")]
    public GameObject coinPrefab;
    [Range(0f, 1f)]
    public float coinDropChance = 0.8f;
    public int minCoins = 1;
    public int maxCoins = 5;

    void Update()
    {
        if (health <= 0)
        {
            // Spawn death effect
            if (deatheffect != null)
                Instantiate(deatheffect, transform.position, Quaternion.identity);

            // Drop a power-up or item (optional)
            if (drop != null && Random.value < dropChance)
                Instantiate(drop, transform.position, Quaternion.identity);

            // Drop coins (optional)
            if (coinPrefab != null && Random.value < coinDropChance)
            {
                int coinAmount = Random.Range(minCoins, maxCoins + 1);

                for (int i = 0; i < coinAmount; i++)
                {
                    // Spread coins slightly for a more natural look
                    Vector2 offset = Random.insideUnitCircle * 0.5f;
                    Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0f);
                    Instantiate(coinPrefab, spawnPos, Quaternion.identity);
                }
            }

            // Destroy the enemy
            swaaerh.enemiesAlive--;
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        if(invulnerable != true){
        health -= damage;
        }
    }
}
