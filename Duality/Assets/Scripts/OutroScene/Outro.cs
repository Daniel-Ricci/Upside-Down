using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Outro : MonoBehaviour
{
    private void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("Intro", LoadSceneMode.Single);
        }
    }
}
