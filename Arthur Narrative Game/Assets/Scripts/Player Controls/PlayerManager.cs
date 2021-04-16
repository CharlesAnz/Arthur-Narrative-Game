using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

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

    public UI_HealthBar playerHealthBar;
    public UI_HealthBar targetHealthBar;

    public Player_Controller activePerson;

    private CharacterCombat activePersonCombat;

    public bool gameOver = false;
    public PlayableDirector timeline;
    public GameObject cutSceneCamera;
    public GameObject cutsceneCharacters;

    void Start()
    {
        activePerson = player1.GetComponent<Player_Controller>();
        player1.GetComponent<Player_Controller>().activeCharacter = true;

        
        foreach (Ability ability in player1.GetComponent<Player_Stats>().abilities)
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
       
        activePersonCombat = player1.GetComponent<CharacterCombat>();
        

        playerHealthBar.SetMaxHP((int)activePersonCombat.GetMyStats().maxHP.GetValue());
        playerHealthBar.SetCurHP((int)activePersonCombat.GetMyStats().curHP);
        playerHealthBar.SetNameTxt(activePerson.gameObject.name);

        targetHealthBar.SetNameTxt(" ");
        targetHealthBar.SetMaxHP(1);
        targetHealthBar.SetCurHP(0);
        targetHealthBar.gameObject.SetActive(false);

        timeline = GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (gameOver) return;
        
        playerHealthBar.SetCurHP((int)activePerson.GetComponent<CharacterCombat>().GetMyStats().curHP);

        if (activePerson.focus != null)
        {
            if (activePerson.focus.GetType().Equals(typeof(Enemy)))
            {
                CharacterCombat focusedEnemy = activePerson.focus.GetComponent<CharacterCombat>();
                targetHealthBar.gameObject.SetActive(true);

                targetHealthBar.SetMaxHP((int)focusedEnemy.GetMyStats().maxHP.GetValue());
                targetHealthBar.SetCurHP((int)focusedEnemy.GetMyStats().curHP);
                targetHealthBar.SetNameTxt(focusedEnemy.gameObject.name);
            }
        }
        else
        {
            targetHealthBar.SetNameTxt(" ");
            targetHealthBar.SetMaxHP(1);
            targetHealthBar.SetCurHP(0);
            targetHealthBar.gameObject.SetActive(false);
        }


        //press 1 on keyboard to switch to character 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player1.GetComponent<Player_Controller>().activeCharacter = true;
            if (player2 != null) player2.GetComponent<Player_Controller>().activeCharacter = false;
            activePerson = player1.GetComponent<Player_Controller>();

            playerHealthBar.SetMaxHP((int)player1.GetComponent<CharacterCombat>().GetMyStats().maxHP.GetValue());
            playerHealthBar.SetCurHP((int)player1.GetComponent<CharacterCombat>().GetMyStats().curHP);

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

                playerHealthBar.SetMaxHP((int)player2.GetComponent<CharacterCombat>().GetMyStats().maxHP.GetValue());
                playerHealthBar.SetCurHP((int)player2.GetComponent<CharacterCombat>().GetMyStats().curHP);

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


    public void LoseCondition()
    {
        gameOver = true;
    }

    public void WinCondtion(GameObject enemy)
    {
        gameOver = true;

        cutSceneCamera.SetActive(true);
        cutsceneCharacters.SetActive(true);

        enemy.transform.position = new Vector3(48, 28, -80);
        enemy.GetComponent<NavMeshAgent>().enabled = false;
        enemy.GetComponent<CharacterAnimator>().characterAnim.SetTrigger("dead");

        // Play the timeline, a.k.a. the cutscene
        timeline.Play();

        // Calls ShakeCamera function to shake the cinemachine camera. Values are intensity and time
        CinemachineShake.Instance.ShakeCamera(5f, 2.0f);
    }
}
