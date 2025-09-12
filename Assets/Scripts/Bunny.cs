using UnityEngine;

public class Bunny : MonoBehaviour
{
    public int energy = 10;
    public float age = 0;
    public float maxAge = 20;
    public bool isAlive = true;
    public float speed = 1f;
    public float visionRange = 5f;

    private Vector3 destination;
    private float h;

    private void Start()
    {
        destination = transform.position;
    }

    public void Simulate(float h)
    {
        if (!isAlive) return;

        this.h = h;

        Food nearestFood = FindNearestFood();
        Debug.Log(nearestFood);
        if (nearestFood != null)
        {
            destination = nearestFood.transform.position;
        }
        else if (Vector3.Distance(transform.position, destination) < 0.1f)
        {
            SelectNewDestination();
        }

        Move();
        Age();
        CheckState();
    }

    void SelectNewDestination()
    {
        Vector3 direction = new Vector3(
            Random.Range(-visionRange, visionRange),
            Random.Range(-visionRange, visionRange),
            0
        );

        Vector3 targetPoint = transform.position + direction;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, visionRange);

        if (hit.collider != null)
        {
            float offset = transform.localScale.magnitude * 0.5f;
            destination = hit.point - (Vector2)direction.normalized * offset;
        }
        else
        {
            destination = targetPoint;
        }
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * h
        );

        Collider2D foodHit = Physics2D.OverlapCircle(transform.position, 0.1f);
        if (foodHit != null)
        {
            Food food = foodHit.GetComponent<Food>();
            if (food != null)
            {
                energy += food.nutrition;
                Destroy(food.gameObject);
            }
        }

        energy--;
    }

    void Age()
    {
        age += h;
    }

    void CheckState()
    {
        if (energy <= 0 || age > maxAge)
        {
            isAlive = false;
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(destination, 0.2f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, destination);
    }

    Food FindNearestFood()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, visionRange);
        Debug.Log($"Bunny {name} encontró {hits.Length} colliders en su rango");
        Food nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            Food food = hit.GetComponent<Food>();
            if (food != null)
            {
                float dist = Vector2.Distance(transform.position, food.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = food;
                }
            }
        }

        return nearest;
    }
}
