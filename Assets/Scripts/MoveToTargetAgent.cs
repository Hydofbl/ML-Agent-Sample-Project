using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToTargetAgent : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private SpriteRenderer backgroundSR;
    [SerializeField] private Transform agentSpawnPoint;
    [SerializeField] private Transform[] targetSpawnPoints;

    public bool isWallsOpen;

    public override void OnEpisodeBegin()
    {
        if(isWallsOpen)
        {
            transform.localPosition = new Vector3(Random.Range(-3.5f, -1.5f), Random.Range(-3.5f, -3.5f));
            target.localPosition = new Vector3(Random.Range(3.5f, 3.5f), Random.Range(-3.5f, -3.5f));
        }
        else
        {
            transform.localPosition = agentSpawnPoint.localPosition;
            target.localPosition = targetSpawnPoints[1].localPosition;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation((Vector2)target.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        float movementSpeed = 8f;

        transform.position += new Vector3(moveX, moveY) * Time.deltaTime * movementSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continiousActions = actionsOut.ContinuousActions;
        continiousActions[0] = Input.GetAxisRaw("Horizontal");
        continiousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Target"))
        {
            AddReward(10f);
            backgroundSR.color = Color.green;
            EndEpisode();
        }
        else if (collision.CompareTag("Wall"))
        {
            AddReward(-2f);
            backgroundSR.color = Color.red;
            EndEpisode();
        }
    }
}
