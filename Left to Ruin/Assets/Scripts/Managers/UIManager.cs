// Date   : 27.08.2016 18:16
// Project: LD36
// Author : bradur

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public enum DialogAction
{
    None,
    MainMenu,
    DismissDialog,
    NextLevel,
    Restart,
    GameFinished,
    Retry
}

public class UIManager : MonoBehaviour
{

    private Text txtComponent;
    private Color colorVariable;
    private Image imgComponent;

    [SerializeField]
    private GenericDialog dialogPrefab;

    [SerializeField]
    private GenericPool dialogPool;

    private bool dialogIsActive = false;

    private GameObject currentDialog = null;

    public static UIManager main;
    [SerializeField]
    private InventoryItem inventoryItemPrefab;

    [SerializeField]
    private Transform inventoryItemParent;

    [SerializeField]
    private string[] itemNames;
    [SerializeField]
    private Sprite[] itemImages;
    private List<InventoryItem> items = new List<InventoryItem>();

    [SerializeField]
    private Text txtMute;
    [SerializeField]
    private Image imgMuted;
    [SerializeField]
    private Image imgNotMuted;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("UIManager").Length < 1)
        {
            gameObject.tag = "UIManager";
            dialogPool.Init(5, dialogPrefab.gameObject);
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleMute(bool muted)
    {
        if (muted)
        {
            txtMute.text = "Sound OFF (M)";
            imgMuted.enabled = true;
            imgNotMuted.enabled = false;
        } else
        {
            imgMuted.enabled = false;
            imgNotMuted.enabled = true;
            txtMute.text = "Sound ON (M)";
        }
    }

    public void OpenMainMenu()
    {
        dialogIsActive = false;
        SceneManager.LoadScene("menu");
        Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            AddDialog(
                GameManager.main.CurrentLevel.LevelEndDate,
                "I feel I can go no longer. It is time for me to rest.",
                DialogAction.MainMenu,
                "BACK TO GAME (ESC)"
            );
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            AddDialog(
                GameManager.main.CurrentLevel.LevelEndDate,
                "I have made a grave mistake. If only it was possible to trace back my steps and do it all over again!",
                DialogAction.Retry,
                "BACK TO GAME (ESC)"
            );
        }
    }

    public void AddDialog(string title, string text, DialogAction dialogAction, string dismissMessage)
    {
        if (!dialogIsActive)
        {
            dialogIsActive = true;
            GameObject newDialogObject = dialogPool.GetObject();
            newDialogObject.SetActive(true);
            GenericDialog newDialog = newDialogObject.GetComponent<GenericDialog>();
            newDialog.Init(title, text, dialogAction, dismissMessage);
            currentDialog = newDialog.gameObject;
        }
    }

    public void AddItem(Item item)
    {
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab);
        inventoryItem.transform.SetParent(inventoryItemParent, false);
        inventoryItem.Init(item, itemNames[(int)item], itemImages[(int)item], items.Count);
        items.Add(inventoryItem);
    }

    public void RemoveItem(Item item)
    {
        foreach(InventoryItem inventoryItem in items)
        {
            if(inventoryItem.Item == item)
            {
                items.Remove(inventoryItem);
                Destroy(inventoryItem.gameObject);
                break;
            }
        }
        for(int i = 0; i < items.Count; i++)
        {
            items[i].UpdatePosition(i);
        }
    }

    public void ClearItems()
    {
        foreach(InventoryItem inventoryItem in items)
        {
            Destroy(inventoryItem.gameObject);
        }
        items.Clear();
        items = new List<InventoryItem>();
    }

    public void DestroyDialog(GameObject dialog)
    {
        dialogIsActive = false;
        dialogPool.DestroyObject(dialog);
        currentDialog = null;
    }

    public void ClearDialogs()
    {
        if (dialogIsActive)
        {
            if(currentDialog != null)
            {
                dialogPool.DestroyObject(currentDialog);
                currentDialog = null;
                dialogIsActive = false;
            }
        }
    }
}
