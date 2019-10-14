using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public UIController uIController;
    public FollowerCam followerCam;
    private bool raceStarted = false;
    public List<Stage> stages;
    public int currentStage = -1;

    public GameObject car;
    public CarPhysics carPhysics;

    private void Start()
    {
        raceStarted = false;
        uIController.ShowMenu("MainMenu");
    }

    public void StartRace()
    {
        uIController.ShowMenu("GamePlay");
        NextStage();
        raceStarted = true;
        StartCoroutine(UpdateProgress(
              CalculateForPositiveAxis( stages[currentStage].finishPoint.transform.position,stages[currentStage].positiveAxis)
            - CalculateForPositiveAxis( stages[currentStage].startPoint.transform.position, stages[currentStage].positiveAxis))
            );
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextStage()
    {
        StopAllCoroutines();
        currentStage += 1;
        if (currentStage >= stages.Count)
        {
            return;
        }
        followerCam.offSet.x = stages[currentStage].camXOffset;
        carPhysics.currentStage = stages[currentStage];
        carPhysics.StartCar();
        raceStarted = true;
        uIController.ShowMenu("GamePlay");

        StartCoroutine(UpdateProgress(
             CalculateForPositiveAxis(stages[currentStage].finishPoint.transform.position, stages[currentStage].positiveAxis)
           - CalculateForPositiveAxis(stages[currentStage].startPoint.transform.position, stages[currentStage].positiveAxis))
           );
    }

    private IEnumerator UpdateProgress(float totalDistance)
    {
        float completeRate = 
        (
            CalculateForPositiveAxis( car.transform.position,stages[currentStage].positiveAxis)
            -CalculateForPositiveAxis( stages[currentStage].startPoint.transform.position, stages[currentStage].positiveAxis)
        ) 
        / totalDistance;

        uIController.UpdateProgress(completeRate);
        if (completeRate >= 1f)
        {
            FinishRace();
            uIController.ShowMenu("GameFinish");
        }
        yield return new WaitForSeconds(.02f);
        yield return StartCoroutine(UpdateProgress(totalDistance));
    }

    private void FinishRace()
    {
        car.GetComponent<CarPhysics>().StopCar();
        raceStarted = false;
    }

    public float CalculateForPositiveAxis(Vector3 pos,PositiveAxis positiveAxis)
    {
        switch (positiveAxis)
        {
            case PositiveAxis.X:
                return pos.x;
            case PositiveAxis.Z:
                return pos.z;
            default:
                return pos.x;
        }
    }
}
