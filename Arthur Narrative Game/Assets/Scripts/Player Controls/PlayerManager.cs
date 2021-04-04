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
    
    public delegate void OnCharacterChange();
    public OnCharacterChange onCharacterChangeCallback;
    public GameObject player2;
    public GameObject player1;

    public Player_Controller activePerson;

    

    void Start()
    {
        activePerson = player1.GetComponent<Player_Controller>();
        player1.GetComponent<Player_Controller>().activeCharacter = true;
        
        foreach(Ability ability in player1.GetComponent<Player_Stats>().abilities)
        {
            ability.cooldownTimer = 0;
        }


        if (player2 != null)
        {
            player2.GetComponent<Player_Controller>().activeCharacter = false;
            foreach (Ability ability in player2.GetComponent<Player_Stats>().abilities)
            {
                ability.cooldownTimer = 0;
            }
        }
    }

    void Update()
    {
        
        //press 1 on keyboard to switch to character 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player1.GetComponent<Player_Controller>().activeCharacter = true;
            if (player2 != null) player2.GetComponent<Player_Controller>().activeCharacter = false;
            activePerson = player1.GetComponent<Player_Controller>();


            if (onCharacterChangeCallback != null)
            {
                onCharacterChangeCallback.Invoke();
            }

        }

        //press 2 on keyboard to switch to character 2

        if (player2 != null)
        { 
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
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            activePerson.GetComponent<Player_Stats>().abilities[0].Use(activePerson.gameObject);
        }
        
        if (Input.GetKeyDown(KeyCode.W))        
            activePerson.GetComponent<Player_Stats>().abilities[1].Use(activePerson.gameObject);
        
        if (Input.GetKeyDown(KeyCode.E))
            activePerson.GetComponent<Player_Stats>().abilities[2].Use(activePerson.gameObject);

    }
}
