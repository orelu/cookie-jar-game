using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuHandeler : MonoBehaviour
{

    public GameObject menuObject;

    public string sceneToSwitchTo = "Game Scene";
    

    public void switchScene() {
        SceneManager.LoadScene(sceneToSwitchTo);
        AudioManager.Instance.PlayAlt3();
    }

    public void switchSceneToMenu() {
        SceneManager.LoadScene("Menu Scene");
        AudioManager.Instance.PlayAlt3();
    }

    public void openMenu() {
        menuObject.SetActive(true);
        AudioManager.Instance.PlayAlt3();
    }

    public void closeMenu() {
        menuObject.SetActive(false);
        AudioManager.Instance.PlayAlt3();
    }

    void Start() {
        if (menuObject!=null) {
            menuObject.SetActive(false);
        }
        
    }
}
