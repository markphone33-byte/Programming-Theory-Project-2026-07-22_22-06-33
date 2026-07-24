using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    private Button button;
    void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(this.StartGame);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
