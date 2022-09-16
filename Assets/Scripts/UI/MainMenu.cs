using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string _persistentPath;

    private void Start() => _persistentPath = Application.persistentDataPath + "/save.json";

    public void LoadLevel(int sceneIndex) => SceneManager.LoadScene(sceneIndex);

    public void ExitGame() => Application.Quit();

    public void LoadLevelWithNewSaveGame(int sceneIndex)
    {
        if (File.Exists(_persistentPath)) File.Delete(_persistentPath);
        SceneManager.LoadScene(sceneIndex);
    }
}