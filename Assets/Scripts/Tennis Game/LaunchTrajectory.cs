using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchTrajectory : MonoBehaviour
{
    public float speed = 1;
    private bool touched;
    public bool stopForce;
    public LevelTennisMgr levelMgr;
    public LaunchMgr launchMgr;

    public Transform cubeGoal, cubeGoalLeftSide, cubeGoalRightSide;
    private Rigidbody rigidbodyBall;
    private Vector3 vectorForce;
    private bool touchedDestroyerOnce;

    public float minHeightZone;
    public float maxHeightZone;
    public float maxWidthZone;
    public float minWidthZone;

    public LevelTennisGame levelTennisGame;



    private void Start()
    {
        //levelMgr = LevelTennisMgr.instance;
        //cubeGoal = levelMgr.cubeGoal;
        //cubeGoalLeftSide = levelMgr.cubeGoalLeftSide;
        //cubeGoalRightSide = levelMgr.cubeGoalRightSide;

        //Find the direction the ball has to be launched
        vectorForce = FindTrajectoryVector();
        rigidbodyBall = this.GetComponent<Rigidbody>();
        //When the ball is instantianted, add an impulse force to it
        rigidbodyBall.AddForce(vectorForce * speed, ForceMode.Impulse);

    }


    /// <summary>
    /// Find the direction the ball is going to be launch at the beginning
    /// </summary>
    /// <returns></returns>
    private Vector3 FindTrajectoryVector()
    {
        float dlWidth;
        float dlHeight;
        Vector3 trajectory;

        float distance = Vector3.Distance(launchMgr.launcher.transform.position, launchMgr.goal.position); //distance between the aiming zone and the launcher

        //float distanceRightSide = Vector3.Distance(levelMgr.launchMgr.launcher.transform.position, cubeGoalLeftSide.position);
        //float distanceLeftSide = Vector3.Distance(levelMgr.launchMgr.launcher.transform.position, cubeGoalRightSide.position);
        Vector2 randomPos = GetRandomGoal(maxWidthZone, maxHeightZone, minWidthZone, minHeightZone); //get a random position of the aiming trajectory

        dlWidth = Mathf.Sqrt(maxWidthZone * minWidthZone + distance * distance);
        dlHeight = Mathf.Sqrt(maxHeightZone * maxHeightZone + distance * distance);
        trajectory = new Vector3(1, (randomPos[1]) / dlHeight, -(randomPos[0]) / dlWidth);


        Vector3 vectorDir = (new Vector3(launchMgr.goal.position.x + randomPos.x, launchMgr.goal.position.y + randomPos.y, launchMgr.goal.position.z) - launchMgr.launcher.transform.position).normalized;
        return vectorDir;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When the ball collides with the shield, change the color of the shield and increment the number of touched balls
        if (collision.gameObject.tag == "Shield")
        {
            //collision.gameObject.GetComponent<AudioSource>().Play();
            if (gameObject.tag == "Ball")
            {
                Debug.Log("collision");
                stopForce = true;
                //Get the normal of the collision to set a force in the opposite direction for the rebounce on the shield
                vectorForce = collision.GetContact(0).normal;
                rigidbodyBall.AddForce(vectorForce * speed, ForceMode.Impulse);
                //levelMgr.nbBallTouched++;
            }
        }
        //if the ball hits the ground or the wall, reset the gravity on
        //if (!touchedDestroyerOnce)
        //{
        //    stopForce = true;
        //    rigidbodyBall.useGravity = true;
        //     touchedDestroyerOnce = true;
        /// }

    }

    void FixedUpdate()
    {
        //If the ball has touched the shield or the wall reset the gravity
        if (stopForce)
        {
            rigidbodyBall.useGravity = true;
            rigidbodyBall.drag = .1f;
            stopForce = false;
        }
        else
        {
            //Make the ball spin on itself
            if (!touchedDestroyerOnce)
            {
                SpinTheBall();
            }
        }
    }


    /// <summary>
    /// Get a random position of y and z
    /// </summary>
    /// <param name="maxWidth">The distance from the center along z</param>
    /// <param name="maxHeight">The distance from the center along y</param>
    /// <returns></returns>
    private Vector2 GetRandomGoal(float maxWidth, float maxHeight, float minWidth, float minHeight)
    {

        float indexWidth = Random.Range(minWidth, maxWidth);
        float indexHeight = Random.Range(minHeight, maxHeight);
        Debug.Log(indexHeight + " >> " + indexWidth);
        return new Vector2(indexWidth, indexHeight);
    }

    /// <summary>
    /// Make the ball spin on itself
    /// </summary>
    private void SpinTheBall()
    {
        transform.Rotate(Vector3.up * speed, Space.Self);
        transform.Rotate(Vector3.right * speed, Space.Self);
        transform.Rotate(-Vector3.forward * speed, Space.Self);
    }
}
