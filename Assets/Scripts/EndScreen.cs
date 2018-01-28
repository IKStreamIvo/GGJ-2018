using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreen : MonoBehaviour {

    public float countDelay = .1f;
    public TextMeshProUGUI text;

    public IEnumerator CountScore(int score)
    {
        for (int i = 0; i <= score; i++)
        {
            text.SetText(i.ToString());
            yield return new WaitForSeconds(countDelay);
        }
    }
}
