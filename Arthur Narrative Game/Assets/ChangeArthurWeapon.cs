using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeArthurWeapon : MonoBehaviour
{
    public GameObject sword;

    public GameObject torch;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            sword.SetActive(true);
            torch.SetActive(false);

            other.transform.position = transform.position + (transform.forward * -6);

            other.GetComponent<NavMeshAgent>().SetDestination(transform.position + (transform.forward * -7));

            gameObject.GetComponent<BoxCollider>().isTrigger = false;

            MusicManager.instance.StartLoop();
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
             transform.position + (transform.forward * -7),
            new Vector3(4, 4, 4));

    }
}
