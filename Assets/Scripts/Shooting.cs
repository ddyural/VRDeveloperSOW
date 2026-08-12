using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject spherePrefab;

    [Header("Shoot Point")]
    public Transform shootPoint;

    [Header("Shooting Settings")]
    public int count = 20;
    public float speed = 10f;
    public float spread = 10f;

    public void Spawn()
    {
        if (shootPoint == null)
        {
            Debug.LogWarning("Shoot Point не назначен!");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // direstion is based on empty object "Shoot"
            Vector3 direction = shootPoint.forward;

            //  random spread
            direction = Quaternion.Euler(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                0f
            ) * direction;

            // create a sphere at the position "Shoot"
            GameObject sphere = Instantiate(
                spherePrefab,
                shootPoint.position,
                Quaternion.identity
            );

            Projectile projectile = sphere.GetComponent<Projectile>();

            if (projectile != null)
            {
                projectile.speed = speed;
                projectile.Init(direction);
            }
        }
    }
}