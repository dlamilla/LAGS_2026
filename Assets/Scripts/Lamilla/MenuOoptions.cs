using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuOoptions : MonoBehaviour
{
    public void ChangeScene(int index)
    {
        FadeINOUT.instance.LoadScene(index);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

 
}
