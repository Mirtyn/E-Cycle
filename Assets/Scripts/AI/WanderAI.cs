using UnityEngine;
using System.Collections;
using UnityEngine.AI;

internal class WanderAI : MapSettings
{
    private float wanderRadius;
    [SerializeField] private float minWanderRadius = 1;
    [SerializeField] private float maxWanderRadius = 6;

    private float wanderTimer;
    [SerializeField] private float minWanderTimer = 3;
    [SerializeField] private float maxWanderTimer = 8;

    // -1 = every layer
    [SerializeField] int LayerMaskInteracteable = -1;

    NavMeshAgent agent;
    float timer;

    // Use this for initialization
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    public void Wander()
    {
        Debug.DrawLine(gameObject.transform.position, agent.destination, Color.white);
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, LayerMaskInteracteable);
            agent.SetDestination(newPos);
            timer = 0;
            wanderTimer = Random.Range(minWanderTimer, maxWanderTimer);
            wanderRadius = Random.Range(minWanderRadius, maxWanderRadius);
        }
    }

    private static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector2 randDirection = Random.insideUnitCircle * dist;

        Vector3 r = origin + new Vector3(randDirection.x, 0, randDirection.y);

        NavMeshHit navHit;

        NavMesh.SamplePosition(r, out navHit, dist, layermask);

        return navHit.position;
    }
}
