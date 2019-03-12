using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CS_Menu : MonoBehaviour {

    [SerializeField]
    Text StartText;
    int iStartSize = 27;
    float fMaxSize = 38;
    public void OnStartClick()
    {
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        StartCoroutine("Scale");
    }

    IEnumerator Scale()
    {
        float timer = 0;

        while (true) // this could also be a condition indicating "alive or dead"
        {
            // we scale all axis, so they will have the same value, 
            // so we can work with a float instead of comparing vectors
            while (fMaxSize > StartText.fontSize)
            {
                timer += Time.deltaTime;
                StartText.fontSize += 1;
                yield return null;
            }
            // reset the timer

            yield return new WaitForSeconds(0.5f);

            timer = 0;
            while (27 < StartText.fontSize)
            {
                timer += Time.deltaTime;
                StartText.fontSize -= 1;
                yield return null;
            }

            timer = 0;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
