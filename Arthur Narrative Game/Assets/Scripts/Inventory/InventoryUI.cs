using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    PlayerManager playerManager;

    public Inventory playerInventory;
    Player_Controller playerController;

    public InventorySlot[] slots;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance;
        playerInventory = playerManager.player2.GetComponent<Inventory>();

        //Update the Inventory UI every time that the inventory is modified
        playerInventory.onItemChangedCallback += UpdateUI;

        playerInventory = playerManager.player1.GetComponent<Inventory>();
        playerController = playerManager.player1.GetComponent<Player_Controller>();

        //Update the Inventory UI every time that the inventory is modified
        playerInventory.onItemChangedCallback += UpdateUI;

        //Update the Inventory UI every time that the play switches characters
        playerManager.onCharacterChangeCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].playerController = playerController;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
        
    }

    void UpdateUI()
    {
        playerInventory = playerManager.activePerson.GetComponent<Inventory>();
        playerController = playerManager.activePerson.GetComponent<Player_Controller>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].playerController = playerController;
            if (i < playerInventory.items.Count)
            {
                slots[i].AddItem(playerInventory.items[i]);
                
            }
            else
            {
                slots[i].ClearSlot();
            }

        }
        
    
    
    }
}
