using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAnimator : MonoBehaviour
{
    const float locoAnimSmoothTime = 0.1f;

    public Animator characterAnim;
    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterAnim = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        characterAnim.SetFloat("speedPercent", speedPercent, locoAnimSmoothTime, Time.deltaTime);
    }
}
