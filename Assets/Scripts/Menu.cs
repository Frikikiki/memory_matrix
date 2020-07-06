using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{   
    public void changeScene(string sceneName) 
    {
        SceneManager.LoadScene(sceneName);
    }
}
