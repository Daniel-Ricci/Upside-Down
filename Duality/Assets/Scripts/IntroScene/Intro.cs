using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Intro : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _startText;

    private void Awake()
    {
        StartCoroutine(FlashStartText());
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        }
    }

    private IEnumerator FlashStartText()
    {
        while(true)
        {
            _startText.enabled = true;
            yield return new WaitForSeconds(0.5f);
            _startText.enabled = false;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
