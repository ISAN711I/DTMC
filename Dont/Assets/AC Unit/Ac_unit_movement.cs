using UnityEngine;

public class Ac_unit_movement : MonoBehaviour
{
    public float followDistance;
    public float moveSpeed;
    public float error;
    private Transform player;
    int rotate_direction;

    public float shot_delay;
    private float shot_delay_timer;

    public float recoil_force;
    public float recoil_time;
    private float recoil_timer;

    public GameObject projectile;
    public GameObject particles;

    void Start()
    {
        // Find the player object and get its transform
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;

        // Add random variation to follow distance, shot delay, and move speed
        followDistance += 2 * (Random.value - .5f);
        rotate_direction = Random.value < 0.5f ? -1 : 1;
        shot_delay += 2 * (Random.value - .5f);
        moveSpeed += 2 * (Random.value - .5f);
    }

    void Update()
    {
        // Get the distance between the enemy and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // Calculate direction to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Calculate the angle to face the player on the Z-axis
        float angle2 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust rotation so the model's front faces the player (when rotation is zero)
        // We assume the model's default orientation is such that facing the camera (0, 0, 0) 
        // is facing right (positive X direction). 
        // If that's not the case, you may need to adjust by adding/subtracting 90 or 180 degrees.
        float rotationAdjustment = 90f;  // Adjust this value based on your model's initial orientation.

        // Rotate the enemy on the Z-axis to face the player
        Quaternion rotation2 = Quaternion.Euler(0f, 0f, angle2 + rotationAdjustment);
        transform.rotation = rotation2;

        if (recoil_timer <= 0)
        {
            if (Mathf.Abs(distance - followDistance) > error)
            {
                // Move the enemy towards or away from the player based on the distance
                if (distance - followDistance < 0)
                {
                    transform.position -= direction * moveSpeed * Time.deltaTime;
                }
                else
                {
                    transform.position += direction * moveSpeed * Time.deltaTime;
                }
            }
            else
            {
                // Add rotating movement when close enough to the player
                Vector3 rotated = new Vector3(direction.y, -direction.x, 0); // For 2D rotation
                float rotate_speed = 2 * shot_delay_timer / shot_delay;
                transform.position += rotate_speed * rotate_direction * rotated * moveSpeed * Time.deltaTime;

                // Shoot a projectile when the shot delay is over
                if (shot_delay_timer <= 0)
                {
                    // Calculate the angle to shoot the projectile in 2D space
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    // Instantiate the projectile and make sure it has the correct Z-axis rotation for 2D
                    Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
                    Instantiate(projectile, transform.position, rotation);
                    Instantiate(particles, transform.position, Quaternion.identity);

                    rotate_direction = Random.value < 0.5f ? -1 : 1;
                    shot_delay_timer = shot_delay;
                    recoil_timer = recoil_time;
                }
                else
                {
                    shot_delay_timer -= Time.deltaTime;
                }
            }
        }
        else
        {
            // Apply recoil to the enemy when it's in recoil state
            transform.position -= direction * recoil_timer * recoil_timer * recoil_force * Time.deltaTime;
            recoil_timer -= Time.deltaTime;
        }
    }
}

