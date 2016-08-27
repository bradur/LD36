// Date   : 27.08.2016 18:17
// Project: LD36
// Author : bradur

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class GenericDialog : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Text txtTitle;
    [SerializeField]
    private Text txtMessage;

    [SerializeField]
    private GameObject exitToMainMenuButton;
    [SerializeField]
    private GameObject dismissButton;
    [SerializeField]
    private Text txtDismissButton;
    [SerializeField]
    private GameObject nextLevelButton;
    [SerializeField]
    private GameObject restartLevelButton;

    public void Init(string title, string message, DialogAction dialogAction, string dismissMessage)
    {
        txtTitle.text = title;
        txtMessage.text = message;
        dismissButton.SetActive(true);
        txtDismissButton.text = dismissMessage;
        exitToMainMenuButton.SetActive(false);
        nextLevelButton.SetActive(false);
        restartLevelButton.SetActive(false);
        if (dialogAction == DialogAction.MainMenu)
        {
            exitToMainMenuButton.SetActive(true);
        }
        else if (dialogAction == DialogAction.NextLevel)
        {
            dismissButton.SetActive(false);
            nextLevelButton.SetActive(true);
        }
        else if (dialogAction == DialogAction.Restart)
        {
            dismissButton.SetActive(false);
            exitToMainMenuButton.SetActive(true);
            restartLevelButton.SetActive(true);
        }
        animator.SetTrigger("Show");
        Time.timeScale = 0f;
    }

    public void GoToMainMenu()
    {
        UIManager.main.OpenMainMenu();
    }

    public void OpenNextLevel()
    {
        GameManager.main.OpenNextLevel();
    }

    public void RestartLevel()
    {
        GameManager.main.RestartLevel();
    }

    public void Dismiss()
    {
        animator.SetTrigger("Hide");
    }

    public void GoBackToPool()
    {
        Time.timeScale = 1f;
        animator.ResetTrigger("Hide");
        animator.ResetTrigger("Show");
        UIManager.main.DestroyDialog(gameObject);
    }
}
