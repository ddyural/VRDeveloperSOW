using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    private Vector3 direction;

    public void Init(Vector3 direction)
    {
        this.direction = direction.normalized;
        Destroy(gameObject, lifetime); // selfdestroy
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime; // projectile travels forward
    }
}