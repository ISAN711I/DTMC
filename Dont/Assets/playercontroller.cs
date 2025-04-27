using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class playercontroller : MonoBehaviour
{
    public GameObject projectile;
    public float speed = 5f;
    public float startTimeBtwShots = 0.3f;
    public float offset;

    public Sprite spriteUp;
    public Sprite spriteDown;
    public Sprite spriteLeft;
    public Sprite spriteRight;

    private float timeBtwShots;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Handle aiming
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        // Handle movement input
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        // Update facing direction sprite
        UpdateSpriteDirection(moveInput);

        // Handle shooting
        if (timeBtwShots <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(projectile, transform.position, Quaternion.Euler(0f, 0f, rot));
                timeBtwShots = startTimeBtwShots;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // Physics-based movement
        rb.linearVelocity = moveInput * speed;
    }

    void UpdateSpriteDirection(Vector2 direction)
    {
        if (direction == Vector2.zero) return; // Do not change sprite if idle

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            spriteRenderer.sprite = direction.x > 0 ? spriteRight : spriteLeft;
        }
        else
        {
            spriteRenderer.sprite = direction.y > 0 ? spriteUp : spriteDown;
        }
    }
}
