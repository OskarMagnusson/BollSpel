using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*By Björn Andersson*/

public class LevelGoalTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        PlayerControls pC = other.GetComponent<PlayerControls>();
        if (pC != null)
        {
            GameManager.LoadNextLevel();
        }
    }
}