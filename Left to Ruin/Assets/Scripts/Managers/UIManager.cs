// Date   : 27.08.2016 18:16
// Project: LD36
// Author : bradur

using UnityEngine;
using UnityEngine.UI;

public enum DialogAction
{
    None,
    Quit,
    MainMenu,
    DismissDialog,
    NextLevel
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

    public static UIManager main;

    void Awake()
    {
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

    public void OpenMainMenu()
    {
        dialogIsActive = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Debug.Log("Escape!");
            AddDialog("8th of August, 1877", "I feel I can go no longer. It is time for me to rest.", DialogAction.Quit, "BACK TO GAME");
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
            newDialog.Init(title, text, dialogAction, "BACK TO GAME");
        }
    }

    public void DestroyDialog(GameObject dialog)
    {
        dialogIsActive = false;
        dialogPool.DestroyObject(dialog);
    }
}
