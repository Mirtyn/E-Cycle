using UnityEngine;
using System.Collections;
using UnityEngine.AI;

internal class OAttackingAI : MonoBehaviour
{
    GameObject player;

    //public float AttackDistance = 1f;
    public float Refresh = 0.1f;

    // -1 = every layer
    //[SerializeField] int LayerMaskInteracteable = -1;

    //Transform target;
    NavMeshAgent agent;
    float timer;

    // Use this for initialization
    void OnEnable()
    {
        player = GameObject.FindWithTag("MyPlayer");
        agent = GetComponent<NavMeshAgent>();
        timer = Refresh;
    }

    public void Attack()
    {
        Debug.DrawLine(gameObject.transform.position, agent.destination, Color.red);
        timer += Time.deltaTime;

        agent.SetDestination(player.transform.position);
        timer = 0;

        //if (timer >= Refresh)
        //{
        //    //Vector3 newPos = CalculatePosForAttack(player.transform.position, AttackDistance, LayerMaskInteracteable);
        //    agent.SetDestination(player.transform.position);
        //    timer = 0;
        //}
    }

    //public static Vector3 CalculatePosForAttack(Vector3 origin, float dist, int layermask)
    //{
    //    Vector2 randDirection = Random.insideUnitCircle * dist;

    //    Vector3 r = origin + new Vector3(randDirection.x, 0, randDirection.y);

    //    NavMeshHit navHit;

    //    NavMesh.SamplePosition(r, out navHit, dist, layermask);

    //    return navHit.position;
    //}
}
