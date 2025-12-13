using UnityEngine.SceneManagement;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
