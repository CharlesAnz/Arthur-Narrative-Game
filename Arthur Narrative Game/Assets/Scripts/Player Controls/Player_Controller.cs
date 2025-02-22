﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//[RequireComponent(typeof(Player_Movement))]
public class Player_Controller : MonoBehaviour
{
    public LayerMask moveMask;

    public Interactable focus;
    PlayerManager playerManager;

    public bool activeCharacter;

    Camera cam;

    Player_Movement movement;

    public Text posText;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance;

        cam = Camera.main;
        movement = GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        posText.text = transform.position.ToString();
        if (transform.position.y <= 0)
        {
            Vector3 newpos = new Vector3(transform.position.x, 40.0f, transform.position.z);
            transform.position = newpos;
        }
        
        if (activeCharacter == false && focus == null)
        {
            movement.FollowTarget(playerManager.activePerson.gameObject, 3);

            return;
        }

        if (GetComponent<Player_Stats>().dead || playerManager.gameOver) return;

        if (GetComponent<CharacterCombat>().CastTime > 0) return;

        if (EventSystem.current.IsPointerOverGameObject()) return;

        //shoots ray from mouse position, then moves player to target position
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                //Left mouse button click move
                RemoveFocus();
                movement.MovetoPoint(hit.point);

            }
        }

        //shoots ray from mouse position, if it hits an Interactable object, then that becomes the player's focus
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }

    //Sets the player's focus
    //if there was a focus already, then stop focusing on old focus
    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OnDefocused();

            focus = newFocus;

            //Moves character to focus location
            movement.FollowTarget(newFocus);
        }

        //Tells interactable that they are being focused by the player
        newFocus.OnFocused(transform);
    }

    //removes the player's current focus
    public void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();

        focus = null;
        movement.StopFollowTarget();
        //Debug.Log("focus removed should stop following target");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 2;
        Gizmos.DrawRay(transform.position, direction);
    }
}
