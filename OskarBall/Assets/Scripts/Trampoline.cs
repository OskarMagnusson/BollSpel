using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*By Björn Andersson*/

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The direction the trampoline will launch the player.")]
    Vector3 launchDirection;

    [SerializeField]
    [Tooltip("The force with which the trampoline will launch the player.")]
    float launchForce;

    private void OnTriggerEnter(Collider other)
    {
        PlayerControls pC = other.GetComponent<PlayerControls>();
        if (pC != null)
        {
            StartCoroutine(pC.Launch(launchDirection, launchForce));
        }
    }
}