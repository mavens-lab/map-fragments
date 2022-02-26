using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class switchScene : MonoBehaviour
{
    [SerializeField] string m_sceneName = "auroraXR_Login";
    [SerializeField] int m_sceneNumber = 0;

    public string SceneName { get => m_sceneName; set => m_sceneName = value; }
    public int SceneNumber { get => m_sceneNumber; set => m_sceneNumber = value; }

    public void LoadScenename()
    {
        SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    public void LoadSceneNumber()
    {
        SceneManager.LoadScene(SceneNumber, LoadSceneMode.Single);
    }
}
