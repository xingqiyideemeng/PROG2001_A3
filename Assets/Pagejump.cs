using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pagejump : MonoBehaviour
{
    // Start is called before the first frame update
    public void LoadMyScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void LoadMyScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }
    // Update is called once per frame

}

