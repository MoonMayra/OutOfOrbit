using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSelectorManager : MonoBehaviour
{
    // ?? Volver al menú principal
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // ?? Ir al Level Selector de la Jungla
    public void GoToJungleLevels()
    {
        SceneManager.LoadScene("LevelSelectorJungle");
    }

    // ?? Ir al Level Selector de la Cueva
    public void GoToCaveLevels()
    {
        SceneManager.LoadScene("LevelSelectorCave");
    }

    // ?? Ir al Level Selector del Laboratorio
    public void GoToLabLevels()
    {
        SceneManager.LoadScene("LevelSelectorLab");
    }
}
