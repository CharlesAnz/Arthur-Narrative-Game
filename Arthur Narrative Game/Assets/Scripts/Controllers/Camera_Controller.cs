using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public Transform target;
    PlayerManager playerManager;

    public Vector3 offset;
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    public float pitch = 2f;

    public float yawSpeed = 100f;

    private float currentZoom = 15f;
    private float currentYaw = 0f;

    private void Start()
    {
        playerManager = PlayerManager.instance;

        target = playerManager.player1.transform;
    }

    void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        currentYaw -= Input.GetAxis("Horizontal") * yawSpeed * Time.deltaTime;

        target = playerManager.activePerson.gameObject.transform;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);
        transform.RotateAround(target.position, Vector3.up, currentYaw);
    }
}
