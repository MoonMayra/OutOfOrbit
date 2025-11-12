using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string LoadScreen;
    public Button New_Game_Button;

    void Start()
    {
        if (New_Game_Button != null)
            New_Game_Button.onClick.AddListener(LoadSelectedScene);
    }

    public void LoadSelectedScene()
    {
        if (!string.IsNullOrEmpty(LoadScreen))
            SceneManager.LoadScene(LoadScreen);
    }
}
//

//* Continue latest saved game
//public void ContinueGame()
//{
// Lógica para cargar progreso guardado
// LoadSceneMode word state recorrerla y aplicarla
// Ejemplo: cargar nombre de escena desde PlayerPrefs
//string savedScene = PlayerPrefs.GetString("SavedScene", "Level1");
//SceneManager.LoadScene(savedScene);
//}
// Go to area selector screen
//public void OpenLevels()
//
//   SceneManager.LoadScene("AreaSelector");
//

// Open options menu
//ublic void OpenOptions()
//
// SceneManager.LoadScene("Options");
//

// Show credits screen
//ublic void ShowCredits()
//  {
//   }

// Quit the game application
//ublic void QuitGame()
//
//   Debug.Log("Quit Game");
//  Application.Quit();
//   }
