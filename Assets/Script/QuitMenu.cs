using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitMenu : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Return back to main menu!");
        SceneManager.LoadScene(0);
    }
}
