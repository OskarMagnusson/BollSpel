using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*By Björn Andersson*/

public class CameraBehavior : MonoBehaviour
{
    [SerializeField]
    float minMoveDistance, maxStopDistance, speed;

    delegate void CameraMovement();

    CameraMovement currentCameraMovement;

    PlayerControls pC;

    float minXPos, maxXPos, minYPos, maxYPos, zPos, targetX, targetY;

    bool moving;

    public PlayerControls PC
    {
        set { this.pC = value; }
    }

    void Start()
    {
        zPos = transform.position.z;
        /*
        minXPos = transform.position.x;
        maxXPos = transform.position.x;
        minYPos = transform.position.y;
        maxYPos = transform.position.y;
        */
        currentCameraMovement = DefaultCameraMovement;
    }

    void LateUpdate()
    {
        currentCameraMovement();
    }

    void DefaultCameraMovement()
    {
        if (pC == null)
        {
            print("pc null");
            return;
        }
        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, pC.transform.position, speed);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXPos, maxXPos), Mathf.Clamp(transform.position.y, minYPos, maxYPos), zPos);
            if (transform.position.x - pC.transform.position.x < maxStopDistance && transform.position.y - pC.transform.position.y < maxStopDistance)
            {
                print("stop moving");
                moving = false;
            }
        }
        else
        {
            if (transform.position.x - pC.transform.position.x > minMoveDistance || transform.position.x - pC.transform.position.x < -minMoveDistance || transform.position.y - pC.transform.position.y > minMoveDistance || transform.position.y - pC.transform.position.y < -minMoveDistance)
            {
                print("start moving");
                moving = true;
            }
        }

    }

    void NewScreenMovement()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(targetX, targetY, zPos), speed);
    }

    public void NewScreen(float minXPos, float maxXPos, float minYPos, float maxYPos, float targetX, float targetY)   //Gives the camera new positional restrictions when the screen transitions.
    {
        this.minXPos = minXPos;
        this.maxXPos = maxXPos;
        this.minYPos = minYPos;
        this.maxYPos = maxYPos;
        this.targetX = targetX;
        this.targetY = targetY;
        currentCameraMovement = NewScreenMovement;
        StartCoroutine("ScreenChange");
    }

    IEnumerator ScreenChange()
    {
        Time.timeScale = 0;
        yield return new WaitUntil(() => ScreenChangeComplete());
        currentCameraMovement = DefaultCameraMovement;
        Time.timeScale = 1;
    }

    bool ScreenChangeComplete()
    {
        if (transform.position.x - targetX < maxStopDistance && transform.position.x - targetX > -maxStopDistance && transform.position.y - targetY < maxStopDistance && transform.position.y - targetY > -maxStopDistance)
            return true;
        return false;
    }
}
