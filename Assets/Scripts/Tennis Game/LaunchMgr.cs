using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchMgr : MonoBehaviour
{
    public GameObject launcher;

    public GameObject ballToInstantiate;

    public float timeBetweenBall;
    private int nbBallCurrentLevel;
    public float speedBall;

    public bool isRunning = false;

    public bool currentLevelDone = false;

    public float avatarSize;
    public string compteur;

    public Transform goal;
    public GameObject text_compteur;

    public float minHeightZone = -0.5f;
    public float maxHeightZone = 0f;
    public float maxWidthZone = 0.5f;
    public float minWidthZone = 0f;

    //public Transform cubeGoal;
    public static LaunchMgr control;
    public int hitCount = 0;
    
    private void Awake()
    {
        control = this;
    }

    private void Start()
    {
        avatarSize = 1.74f;
        StartCoroutine(Launch());
    }

    /// <summary>
    /// Launch the level with a specific configuration
    /// </summary>
    /// <param name="nbBallPerLevel">Number of ball which be launched during the level</param>
    /// <param name="time"> Time in seconds between two launched balls</param>
    /// <param name="veryeasyMode">Balls launched from the veryeasy difficulty zone</param>
    /// <param name="easyMode">Balls launched from the easy difficulty zone</param>
    /// <param name="mediumMode">Balls launched from the medium difficulty zone</param>
    /// <param name="hardMode">Balls launched from the hard difficulty zone</param>
    /// <param name="speed">The speed of the ball</param>
    int currentLevel = 0;
    public void LaunchBalls(int nbBallPerLevel, float time, int level, float speed)
    {
        currentLevelDone = false;
        nbBallCurrentLevel = nbBallPerLevel;
        timeBetweenBall = time;
        speedBall = speed;
        currentLevel = level;
    }
    /// <summary>
    /// Launch the balls of the current level through a loop time and initialize the speed and the position of the balls
    /// </summary>
    /// <returns></returns>
    public IEnumerator Launch()
    {
        isRunning = true;
        yield return new WaitForSecondsRealtime(5);
        float yPos = Mathf.Max(avatarSize * 1.2f / 1.74f, 0.9f);
        ballToInstantiate.GetComponent<LaunchTrajectory>().speed = speedBall;
        //On instantie des object toutes les [timeBetweenBall] secondes
        for (int i = 0; i < 1000; i++)
        {
            launcher.transform.position = new Vector3(launcher.transform.position.x, yPos, launcher.transform.position.z);
            //cubeGoal.position = new Vector3(cubeGoal.position.x, yPos, cubeGoal.position.z);

            ballToInstantiate.GetComponent<LaunchTrajectory>().maxHeightZone = maxHeightZone;
            ballToInstantiate.GetComponent<LaunchTrajectory>().minHeightZone = minHeightZone;

            ballToInstantiate.GetComponent<LaunchTrajectory>().maxWidthZone = maxWidthZone;
            ballToInstantiate.GetComponent<LaunchTrajectory>().minWidthZone = minWidthZone;
            ballToInstantiate.GetComponent<LaunchTrajectory>().launchMgr = this;
            Instantiate(ballToInstantiate, launcher.transform.position, Quaternion.identity);

            //compteur=string.Format("{0}", nbBallCurrentLevel-(i+1));
            //text_compteur.GetComponent<TextMesh>().text=compteur;
            yield return new WaitForSecondsRealtime(timeBetweenBall);
        }
        currentLevelDone = true;
        isRunning = false;
    }
}
