using UnityEngine.EventSystems;
using UnityEngine;


//[RequireComponent(typeof(Player_Movement))]
public class Player_Controller : MonoBehaviour
{
    public LayerMask moveMask;
    public Interactable focus;
    PlayerManager playerManager;

    public bool activeCharacter;

    Camera cam;

    Player_Movement movement;

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
        if (activeCharacter == false && focus == null)
        {
            movement.FollowTarget(playerManager.activePerson.gameObject, 3);
            
            return; 
        }

        if (EventSystem.current.IsPointerOverGameObject()) return;

        //shoots ray from mouse position, then moves player to target position
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                //Left mouse button click move

                movement.moveTarget.transform.position = new Vector3(
                    hit.point.x, 
                    movement.moveTarget.transform.position.y, 
                    hit.point.z);

                RemoveFocus();

                movement.FollowTarget(movement.moveTarget.gameObject, (float)1.2);

                
            
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
                    //Debug.Log("We hit: " + hit.collider.name + " " + hit.point);
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
    void RemoveFocus()
    {
        if (focus != null)
            focus.OnDefocused();

        focus = null;
        movement.StopFollowTarget();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 2;
        Gizmos.DrawRay(transform.position, direction);
    }
}
