using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class ChangeArthurWeapon : MonoBehaviour
{
    public float timeTillNextScene;

    public GameObject activePerson;

    public GameObject sword;
    public GameObject torch;

    public GameObject dragonBoss;
    public GameObject gameplayFog;
    public PlayableDirector timeline;
    public GameObject cutsceneCameras;
    public GameObject cutsceneObjects;
    public GameObject UICanvas;
    public GameObject self;

    private double timeKeeper;

    private void Start()
    {
        timeline = GetComponent<PlayableDirector>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            activePerson.SetActive(false);
            dragonBoss.SetActive(false);
            gameplayFog.SetActive(false);
            UICanvas.SetActive(false);
            cutsceneCameras.SetActive(true);
            cutsceneObjects.SetActive(true);

            timeline.Play();
            StartCoroutine(StartGameAfterCutsceneEnds(timeTillNextScene));

            //if (timeline.state == PlayState.Paused && (timeKeeper + Time.deltaTime) >= timeline.duration)
            //{
            //    dragonBoss.SetActive(true);
            //    cutsceneCameras.SetActive(false);
            //    cutsceneObjects.SetActive(false);

            //    sword.SetActive(true);
            //    torch.SetActive(false);
            //    UICanvas.SetActive(true);
            //    activePerson.SetActive(true);

            //    other.transform.position = transform.position + (transform.forward * -6);

            //    other.GetComponent<NavMeshAgent>().SetDestination(transform.position + (transform.forward * -7));

            //    gameObject.GetComponent<BoxCollider>().isTrigger = false;

            //    MusicManager.instance.StartLoop();
            //}
        }
    }

    IEnumerator StartGameAfterCutsceneEnds(float timeTillNextScene)
    {
        yield return new WaitForSeconds(timeTillNextScene);
        CinemachineShake.Instance.ShakeCamera(13f, 9.414f);
        dragonBoss.SetActive(true);
        cutsceneCameras.SetActive(false);
        cutsceneObjects.SetActive(false);

        sword.SetActive(true);
        torch.SetActive(false);
        UICanvas.SetActive(true);
        activePerson.SetActive(true);

        //activePerson.transform.position = transform.position + (transform.forward * -7);
        activePerson.transform.position = activePerson.GetComponent<Player_Stats>().resetPos.position;
        /*
        Vector3 newpos = new Vector3(
            activePerson.transform.position.x,
            40.0f,
            activePerson.transform.position.z);

        activePerson.transform.position = newpos;

        activePerson.GetComponent<NavMeshAgent>().SetDestination(transform.position + (transform.forward * -7));

        */
        gameObject.GetComponent<BoxCollider>().isTrigger = false;

        MusicManager.instance.StartLoop();
        self.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
             transform.position + (transform.forward * -7),
            new Vector3(4, 4, 4));

    }
}
