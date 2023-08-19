using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class OEnemyBehaviour : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    OWanderingAI wanderingAI;
    OFleeingAI fleeingAI;
    OAttackingAI attackingAI;

    public float SightDistance = 10;
    public float StopAIDistance = 40;

    [SerializeField] bool flee = false;
    [SerializeField] bool attackAllowed = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("MyPlayer");
        agent = GetComponent<NavMeshAgent>();
        wanderingAI = this.GetComponent<OWanderingAI>();
        fleeingAI = this.GetComponent<OFleeingAI>();
        attackingAI = this.GetComponent<OAttackingAI>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForMode();
    }

    void CheckForMode()
    {
        RaycastHit hit;

        if (Vector3.Distance(player.transform.position, this.transform.position) >= StopAIDistance)
        {
            Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(0, 20, 0));
        }
        else if (flee)
        {
            Flee();
        }
        else if (attackAllowed && Vector3.Distance(player.transform.position, this.transform.position) <= SightDistance)
        {

            if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), (player.transform.position - transform.position), out hit, SightDistance + (SightDistance / 20)) && hit.collider.gameObject.CompareTag("MyPlayer"))
            {
                Debug.DrawLine(transform.position + new Vector3(0, 1f, 0), hit.point, Color.green);
                Attack();
            }
            else
            {
                Idle();
            }
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        wanderingAI.Wander();
    }

    void Attack()
    {
        attackingAI.Attack();
    }

    void Flee()
    {
        fleeingAI.RunAway();
    }
}
