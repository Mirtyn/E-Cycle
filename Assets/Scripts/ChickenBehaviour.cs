using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenBehaviour : MapSettings
{
    [SerializeField] private Animator animator;
    private const string WALKING_BOOL = "Walking";
    private WanderAI wanderAI;
    private DrinkAI drinkAI;
    private EatGrassAI eatGrassAI;

    private float hungerTimer;
    private float hungerTimerChange = 2f;
    private float hunger = 0.0f;
    private float hungerAdditionMin = 0f;
    private float hungerAdditionMax = 4f;
    private float hungerReq = 50f;

    private float hungerReducementMin = 30f;
    private float hungerReducementMax = 55f;

    private float thirst = 0.0f;
    private float thirstAdditionMin = 0.01f;
    private float thirstAdditionMax = 6f;

    private bool isResting = false;
    private float rest = 0.0f;
    private float restAdditionMin = 0.2f;
    private float restAdditionMax = 8f;

    private Vector3 prevFramePos;

    private void OnEnable()
    {
        prevFramePos = this.transform.position;
        wanderAI = this.GetComponent<WanderAI>();
        eatGrassAI = this.GetComponent<EatGrassAI>();
        wanderAI = this.GetComponent<WanderAI>();
    }

    private void Update()
    {
        hunger += Random.Range(hungerAdditionMin, hungerAdditionMax) * Time.deltaTime;
        thirst += Random.Range(thirstAdditionMin, thirstAdditionMax) * Time.deltaTime;
        rest += Random.Range(restAdditionMin, restAdditionMax) * Time.deltaTime;

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

    private void CheckForMode()
    {
        hungerTimer += Time.deltaTime;

        if (hungerTimer >= hungerTimerChange)
        {
            hungerTimer = 0;
            hungerReq = Random.Range(50f, 90f);
        }

        if (hunger >= hungerReq)
        {
            Eat();
        }
        else
        {
            Idle();
        }
    }

    void Idle()
    {
        wanderAI.Wander();
    }
    void Eat()
    {
        if (eatGrassAI.Eat())
        {
            hunger -= Random.Range(hungerReducementMin, hungerReducementMax);
        }
    }
}
