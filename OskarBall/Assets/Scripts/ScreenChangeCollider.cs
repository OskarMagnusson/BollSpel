using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*By Björn Andersson*/

public class ScreenChangeCollider : MonoBehaviour {

    [SerializeField]
    float minXPos, maxXPos, minYPos, maxYPos, targetX, targetY;

    private void OnTriggerEnter(Collider other)
    {
        PlayerControls pC = other.GetComponent<PlayerControls>();
        if (pC != null)
        {
            FindObjectOfType<CameraBehavior>().NewScreen(minXPos, maxXPos, minYPos, maxYPos, targetX, targetY);
            Destroy(this.gameObject);
        }
    }
}
