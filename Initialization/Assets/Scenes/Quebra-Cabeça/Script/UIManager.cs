using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject victoryPanel;
    public Button undoButton;
    public Button replayButton;
    public Button skipButton;

    private void Awake()
    {
        Instance = this;

        replayButton.onClick.AddListener(() =>
        {
            CommandInvokerr.Instance.Replay(1f);
        });

        skipButton.onClick.AddListener(() =>
        {
            CommandInvokerr.Instance.SkipReplay();
        });
    }

    public void ShowVictory()
    {
        victoryPanel.SetActive(true);
    }
}