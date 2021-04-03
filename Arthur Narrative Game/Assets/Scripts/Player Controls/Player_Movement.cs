using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class Player_Movement : MonoBehaviour
{
    Transform target;

    CharacterCombat combat;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();

        agent.updateRotation = false;
    }

    private void Update()
    {
        agent.speed = combat.GetMyStats().moveSpeed.GetValue();

        if (combat.CastTime > 0)
        {
            target = null;
            agent.velocity = Vector3.zero;
        }
        
        if (target != null)
        {
            if(combat.cc_Effects.Count == 0)
            {
                agent.SetDestination(target.position);
                agent.acceleration = 12f;
                agent.angularSpeed = 120f;
            }

            foreach (CC_Effect effect in combat.cc_Effects)
            {
                if (effect.affect != StatusEffects.Root)
                {
                    agent.SetDestination(target.position);
                    agent.acceleration = 12f;
                    agent.angularSpeed = 120f;
                }
            }

            FaceTarget();
        }

    }

    private void LateUpdate()
    {
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
    }

    public void MovetoPoint(Vector3 newPoint)
    {
        agent.stoppingDistance = 0.2f;
        agent.SetDestination(newPoint);
    }
    
    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius;
        agent.updateRotation = false;
        target = newTarget.interactionTransform;
    }

    public void FollowTarget(GameObject newTarget, float stopDistance)
    {
        agent.stoppingDistance = stopDistance * 0.9f;
        target = newTarget.transform;
    }

    public void StopFollowTarget()
    {
        agent.stoppingDistance = 0f;
        target = null;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
