using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.VisualScripting;

internal class OldFleeingAI : MonoBehaviour
{
    public float runDistance;
    public float minRunDistance = 7;
    public float maxRunDistance = 15;

    public float runTime;
    public float minRunTime = 0.25f;
    public float maxRunTime = 1.5f;

    bool turn = false;
    bool Restart = false;

    // -1 = every layer
    [SerializeField] int LayerMaskInteracteable = -1;

    Transform target;
    NavMeshAgent agent;
    float timer;
    [SerializeField] float directionY;
    Vector3 newPos;

    GameObject player;

    // Use this for initialization
    void OnEnable()
    {
        player = GameObject.FindWithTag("MyPlayer");
        agent = GetComponent<NavMeshAgent>();
        timer = runTime;
        directionY = Random.Range(0, 360);
        turn = true;
    }

    public void RunAway()
    {
        Debug.DrawLine(gameObject.transform.position, agent.destination, Color.magenta);

        timer += Time.deltaTime;

        

        //var r = this.transform.rotation.eulerAngles;

        //r.y = Mathf.LerpAngle(this.transform.rotation.eulerAngles.y, direction.y, 0.1f);

        

        CheckTime();
    }

    void CheckTime()
    {
        if (timer >= runTime)
        {
            newPos = RandomRotate();

            agent.SetDestination(newPos);
            
            runTime = Random.Range(minRunTime, maxRunTime);
            runDistance = Random.Range(minRunDistance, maxRunDistance);
            //if (Vector3.Distance(newPos, player.transform.position) <= 2)
            //{
            //    CheckTime();
            //}
            
            timer = 0;
            
        }
    }

    public Vector3 RandomRotate()
    {
        directionY = Random.Range(0, 360);

        if (Restart)
        {
            startRot = this.transform.rotation;
            startTime = time;
            endTime = time + duration;
            Restart = false;
        }

        turn = true;

        if (turn)
        {
            while (turn)
            {
                TurnEnemy();
            }
        }

        Vector3 newP = RandomNavForward(transform.position, runDistance, LayerMaskInteracteable, this.transform);

        return newP;
                
    }

    public static Vector3 RandomNavForward(Vector3 origin, float dist, int layermask, Transform transform)
    {
        Vector3 randDirection = origin + transform.forward * dist;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    float time = 0;
    float duration = 0.5f;

    float endTime;
    Quaternion startRot;
    float startTime;

    void TurnEnemy()
    {
        time += Time.deltaTime;

        float currentLength = (endTime - startTime) / time;

        this.transform.rotation = Quaternion.Euler(
            this.transform.rotation.eulerAngles.x,
            Mathf.LerpAngle(startRot.eulerAngles.y, directionY, currentLength),
            this.transform.rotation.eulerAngles.z);

        if (time >= endTime)
        {
            turn = false;
            Restart = true;
        }
    }
}
