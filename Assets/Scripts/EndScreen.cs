using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour {

    public float countDelay = .1f;
    public TextMeshProUGUI text;

    public IEnumerator CountScore(int score)
    {

        text.SetText(score.ToString());
        yield return null;
        /*for (int i = 0; i <= score; i++)
        {
            text.SetText(i.ToString());
            yield return new WaitForSeconds(countDelay);
        }*/
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(1);
        }
    }
}
