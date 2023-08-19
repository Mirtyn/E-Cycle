using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private const string WALKING_BOOL = "Walking";
    //NavMeshAgent agent;
    private WanderAI wanderAI;
    //OFleeingAI fleeingAI;
    //OAttackingAI attackingAI;

    //public float SightDistance = 10;
    //public float StopAIDistance = 40;

    //[SerializeField] bool flee = false;
    //[SerializeField] bool attackAllowed = true;

    // Start is called before the first frame update

    private Vector3 prevFramePos;
    void Start()
    {
        //animator = transform.GetChild(1).GetComponent<Animator>();
        prevFramePos = this.transform.position;
        //agent = GetComponent<NavMeshAgent>();
        wanderAI = this.GetComponent<WanderAI>();
        //fleeingAI = this.GetComponent<OFleeingAI>();
        //attackingAI = this.GetComponent<OAttackingAI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (prevFramePos != this.transform.position)
        {
            animator.SetBool(WALKING_BOOL, true);
        }
        else
        {
            animator.SetBool(WALKING_BOOL, false);
        }
        prevFramePos = this.transform.position;

        CheckForMode();
    }

    void CheckForMode()
    {
        //RaycastHit hit;

        //if (Vector3.Distance(player.transform.position, this.transform.position) >= StopAIDistance)
        //{
        //    Debug.DrawLine(this.transform.position, this.transform.position + new Vector3(0, 20, 0));
        //}
        //else if (flee)
        //{
        //    Flee();
        //}
        //else if (attackAllowed && Vector3.Distance(player.transform.position, this.transform.position) <= SightDistance)
        //{

        //    if (Physics.Raycast(transform.position + new Vector3(0, 1f, 0), (player.transform.position - transform.position), out hit, SightDistance + (SightDistance / 20)) && hit.collider.gameObject.CompareTag("MyPlayer"))
        //    {
        //        Debug.DrawLine(transform.position + new Vector3(0, 1f, 0), hit.point, Color.green);
        //        Attack();
        //    }
        //    else
        //    {
        //        Idle();
        //    }
        //}
        //else
        //{
        //    Idle();
        //}
        Idle();
    }

    void Idle()
    {
        wanderAI.Wander();
    }

    //void Attack()
    //{
    //    attackingAI.Attack();
    //}

    //void Flee()
    //{
    //    fleeingAI.RunAway();
    //}
}
