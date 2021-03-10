using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Singleton class for controling player characters

    #region Singleton
    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    //No longer need "OnCharacterChange" due to having only 1 character
    /*
    public delegate void OnCharacterChange();
    public OnCharacterChange onCharacterChangeCallback;
    public GameObject player2;
    */
    public GameObject player1;
    

    public Player_Controller activePerson;

    

    void Start()
    {
        activePerson = player1.GetComponent<Player_Controller>();
        player1.GetComponent<Player_Controller>().activeCharacter = true;
        //player2.GetComponent<Player_Controller>().activeCharacter = false;
    }

    void Update()
    {
        #region PlayerChange
        //No need to change characters so this is taken out

        /*
        //press 1 on keyboard to switch to character 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player1.GetComponent<Player_Controller>().activeCharacter = true;
            //player2.GetComponent<Player_Controller>().activeCharacter = false;
            activePerson = player1.GetComponent<Player_Controller>();

            
            if (onCharacterChangeCallback != null)
            {
                onCharacterChangeCallback.Invoke();
            }
            
        }

        //press 2 on keyboard to switch to character 2
        
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player1.GetComponent<Player_Controller>().activeCharacter = false;
            player2.GetComponent<Player_Controller>().activeCharacter = true;
            activePerson = player2.GetComponent<Player_Controller>();

            if (onCharacterChangeCallback != null)
            {
                onCharacterChangeCallback.Invoke();
            }
        }
        */
        #endregion 

        if (Input.GetKeyDown(KeyCode.Q))
        {
            activePerson.GetComponent<Player_Stats>().abilities[0].Use(activePerson.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            activePerson.GetComponent<Player_Stats>().abilities[1].Use(activePerson.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            activePerson.GetComponent<Player_Stats>().abilities[2].Use(activePerson.gameObject);
        }

    }
}
