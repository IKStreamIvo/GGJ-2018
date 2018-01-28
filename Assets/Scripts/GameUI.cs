using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour {

    public GameObject healthBar;
    public GameObject EndScreen;

	public void GameOver(int score)
    {
        //Hide health bar
        healthBar.SetActive(false);

        //show game menu + play animation
        EndScreen.SetActive(true);
        StartCoroutine(EndScreen.GetComponent<EndScreen>().CountScore(score));
    }

}
