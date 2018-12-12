using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*By Björn Andersson*/

public class PlatformScript : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The path the platform should follow, in correct order.")]
    Transform[] path;

    [SerializeField]
    [Tooltip("The speed with which the platform moves.")]
    float speed;

    [SerializeField]
    [Tooltip("How long the platform waits at a location before moving towards the next.")]
    float waitTime;

    [SerializeField]
    [Tooltip("How close to the target the platform must be to be considered there.")]
    float doneOffset;

    bool moving = true;

    int target = 0;
    
    void Update()
    {
        if (moving && path != null && target >= 0 && target < path.Length && path[target] != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[target].position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, path[target].position) <= doneOffset)
            {
                StartCoroutine("NextTarget");
            }
        }
    }

    IEnumerator NextTarget()
    {
        moving = false;
        target = (target + 1) % path.Length;
        yield return new WaitForSeconds(waitTime);
        moving = true;
    }
}