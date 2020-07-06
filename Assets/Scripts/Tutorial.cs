using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("main");
        }
    }
    
    public void ChangeScene()
    {
        SceneManager.LoadScene("main");
    }
}
