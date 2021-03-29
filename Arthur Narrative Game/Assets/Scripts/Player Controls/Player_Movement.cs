using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class Player_Movement : MonoBehaviour
{
    Transform target;

    public GameObject moveTarget;

    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (GetComponent<CharacterCombat>().castTime > 0)
        {
            target = null;
            agent.velocity = Vector3.zero;
        }
        if (target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
            agent.acceleration = 12f;
            agent.angularSpeed = 120f;
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
        //moveTarget.transform.position = newPoint;
        agent.updateRotation = true;
        agent.SetDestination(newPoint);
        //agent.angularSpeed = 1000000000000f;
        //agent.acceleration = 1000000000000f;
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
        agent.updateRotation = false;
        target = newTarget.transform;
        //Debug.Log("Target is: " + target);
    }

    public void StopFollowTarget()
    {
        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
        target = null;
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    
    }
}
