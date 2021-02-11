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
        if (target != null)
        {
            agent.SetDestination(target.position);
            FaceTarget();
        }
    }

    /*
    public void MovetoPoint(Vector3 newPoint)
    {
        moveTarget.transform.position = newPoint;
        FollowTarget(moveTarget);
    }
    */
    public void FollowTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius * 0.8f;
        agent.updateRotation = false;
        target = newTarget.interactionTransform;
    }

    public void FollowTarget(GameObject newTarget, float stopDistance)
    {
        agent.stoppingDistance = stopDistance * 0.8f;
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
