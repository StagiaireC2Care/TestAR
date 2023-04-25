using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelTennisGame
{
    public int nbBallPerLevel;
    public float timeBetweenBall;
    public int currentLevel;
    public float speedBall;
    public int nbBallMissed;

    public LevelTennisGame() {
        this.nbBallPerLevel = 100;
        this.timeBetweenBall = 4f;
        this.currentLevel=1;
        this.speedBall = 2;
        this.nbBallMissed = 0;
    }
}

public class FinishPercentageWin
{
    public float percent;

    public FinishPercentageWin(float percent) {
        this.percent = percent;
    }

}

public class LevelTennisMgr : MonoBehaviour
{
    public static LevelTennisMgr instance;
    public LaunchMgr launchMgr;

    private bool gameDone;

    public Transform cubeGoal,cubeGoalLeftSide, cubeGoalRightSide;

    public LevelTennisGame configLevel;
    public int nbBallTouched;

    public int instantiatedBalls;

    private Coroutine coroutineLaunch;

    public GameObject CanvasDone;
    private float previousFixedTimeStep;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
        previousFixedTimeStep = Time.fixedDeltaTime;
        Time.fixedDeltaTime = 0.05f;
    }

    private void OnDestroy()
    {
        Time.fixedDeltaTime = previousFixedTimeStep;
    }

    void Start() {
        configLevel = new LevelTennisGame();
        CanvasDone.SetActive(false);
        LaunchConfigExercise();
    }

    void Update() {
        CheckNextMove();
    }

    /// <summary>
    /// The listener from the web controler
    /// </summary>
    private void OnEnable()
    {
        //Listener of the ball launcher level
        //EventManager.StartListening("nb_ball", () => NbBalls(NetClientManager.floatMsg.value));
        //EventManager.StartListening("sec_between_launch", () => FrequencyBalls(NetClientManager.floatMsg.value));
        ////EventManager.StartListening("Amplitude des mouvements", () => ModeZone(NetClientManager.strMsg.value));
        //EventManager.StartListening("difficulty_mode", () => ModeZone(NetClientManager.strMsg.value));
        //EventManager.StartListening("speed_ball", () => SpeedBall(NetClientManager.strMsg.value));

    }

    /// <summary>
    /// Configure the number of balls of the level
    /// </summary>
    /// <param name="nb">number of balls</param>
    private void NbBalls(float nb) {
        configLevel.nbBallPerLevel = (int)nb;
        LaunchConfigExercise();
    }

    /// <summary>
    /// Configure the frequency of the balls launched
    /// </summary>
    /// <param name="Timeseconds">Launch a ball each Timeseconds</param>
    private void FrequencyBalls(float timeSeconds) {
        configLevel.timeBetweenBall = timeSeconds;
        LaunchConfigExercise();
    }

    /// <summary>
    /// Configure the difficulty zone
    /// </summary>
    /// <param name="zone">Zone easy, zone medium, zone hard</param>
    private void ModeZone(string zone) {

        if(zone=="oneway_zone"||zone=="very_easy_zone"||zone=="coup_droit"){
            configLevel.currentLevel=1;
        }
        else if (zone == "easy_zone" || zone == "Petits") {
            configLevel.currentLevel=2;
        }
        else if (zone == "medium_zone" || zone == "Moyens") {
            configLevel.currentLevel=3;
        }

        else if(zone == "hard_zone" || zone == "difficile"){
            configLevel.currentLevel=4;

        }
        else{
            configLevel.currentLevel=5;
        }
        LaunchConfigExercise();
    }

    /// <summary>
    /// Configure the speed of the ball
    /// </summary>
    /// <param name="speed">Slow speed : </param>
    private void SpeedBall(string speed) {
        if (speed == "label_slow_speed" || speed == "Lente") {
            configLevel.speedBall = 2f;
        }
        else if (speed == "label_medium_speed" || speed == "Moyenne")
        {
            configLevel.speedBall = 3f;
        }
        else if(speed == "label_fast_speed" || speed == "Rapide")
        {
            configLevel.speedBall = 4f;
        }
        else{
            configLevel.speedBall = 5f;
        }
        LaunchConfigExercise();
    }
    
    /// <summary>
    /// Configure the next level to start - nb of balls, the difficulty mode, the time between each ball
    /// </summary>
    private void LaunchConfigExercise() {
        nbBallTouched = 0;
        instantiatedBalls = configLevel.nbBallPerLevel;
        launchMgr.LaunchBalls(configLevel.nbBallPerLevel, configLevel.timeBetweenBall,configLevel.currentLevel, configLevel.speedBall);
        gameDone = false;
        LaunchCoroutineStart();
    }

    /// <summary>
    /// Get the number of missed balls during the level and check what level has to be launched next
    /// </summary>
    private void CheckNextMove() {
        if (launchMgr.currentLevelDone == true && instantiatedBalls == 0 && !gameDone) {
            configLevel.nbBallMissed = configLevel.nbBallPerLevel - nbBallTouched;
            Debug.Log("nombre de balles loupées pendant le niveau : " + configLevel.nbBallMissed + " / " + configLevel.nbBallPerLevel);
            float percentWin = Mathf.Round(100 - (float)configLevel.nbBallMissed / (float)configLevel.nbBallPerLevel * 100f);
            float nbtouched = configLevel.nbBallPerLevel - configLevel.nbBallMissed;
            Debug.Log("pourcentage de win : " + percentWin + "%");
            Debug.Log("jeu fini!");
            gameDone = true;
            launchMgr.currentLevelDone = false;
           // CanvasDone.GetComponentInChildren<Text>().text = Localisation.GetLoc("label_done_congrats") +"\n"+ Localisation.GetLoc("label_score") +" : "+ nbtouched + " / " + configLevel.nbBallPerLevel ;
            CanvasDone.SetActive(true);
            StartCoroutine(FinishLevel(percentWin));
        }
    }

    /// <summary>
    /// Sends a JSON message to the controller.
    /// </summary>
    /// <param name="elapsedTime">Time elapsed when level completed.</param>
    /// <param name="levelCompleted">Number of the completed level.</param>
    public void SendJson(float percentage) {
        FinishPercentageWin finishPercentageWin = new FinishPercentageWin(percentage);
       // NetClientManager.control.SendActivityData("level_completed", JsonUtility.ToJson(finishPercentageWin));
    }

    /// <summary>
    /// Check if the coroutine which launches the ball is already start, if so, stop it and relaunch it
    /// </summary>
    public void LaunchCoroutineStart() {
        CanvasDone.SetActive(false);
        if (launchMgr.isRunning) {
            StopCoroutine(coroutineLaunch);
            coroutineLaunch = StartCoroutine(launchMgr.Launch());
        }
        else {
            coroutineLaunch = StartCoroutine(launchMgr.Launch());
        }
    }

    /// <summary>
    /// Activate the final canvas at the end and sen the percentage data to the web controller
    /// </summary>
    /// <param name="percentToSend"></param>
    /// <returns></returns>
    IEnumerator FinishLevel(float percentToSend) {
        CanvasDone.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        SendJson(percentToSend);
        yield return new WaitForSecondsRealtime(6f);
        //BackMgr.DoCoroutine();
    }
}
