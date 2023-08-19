using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

internal class OFleeingAI : MonoBehaviour
{
    public float FleeRadius;
    public float MinFleeRadius = 8;
    public float MaxFleeRadius = 12;

    public float FleeTimer;
    public float MinFleeTimer = 0.1f;
    public float MaxFleeTimer = 0.8f;

    // -1 = every layer
    [SerializeField] int LayerMaskInteracteable = -1;

    Transform target;
    NavMeshAgent agent;
    float timer;

    // Use this for initialization
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = FleeTimer;
    }

    public void RunAway()
    {
        Debug.DrawLine(gameObject.transform.position, agent.destination, Color.magenta);
        timer += Time.deltaTime;

        if (timer >= FleeTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, FleeRadius, LayerMaskInteracteable);
            agent.SetDestination(newPos);
            timer = 0;
            FleeTimer = Random.Range(MinFleeTimer, MaxFleeTimer);
            FleeRadius = Random.Range(MinFleeRadius, MaxFleeRadius);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector2 randDirection = Random.insideUnitCircle * dist;

        Vector3 r = origin + new Vector3(randDirection.x, 0, randDirection.y);

        NavMeshHit navHit;

        NavMesh.SamplePosition(r, out navHit, dist, layermask);

        return navHit.position;
    }
}
