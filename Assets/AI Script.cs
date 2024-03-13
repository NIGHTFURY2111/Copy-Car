
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine.Jobs;
using System;

public class AIScript : Agent
{
    float movex;
    float movez;
    [SerializeField] float speed;
    [SerializeField] Transform goal;
    [SerializeField] Material playaMaterial;

    public override void OnEpisodeBegin()
    {
        //transform.localPosition = Vector3.zero;
        goal.transform.localPosition = new Vector3 (UnityEngine.Random.Range(4f,-4f), 0, UnityEngine.Random.Range(2f,-2f));
        transform.transform.localPosition = new Vector3 (UnityEngine.Random.Range(4f,-4f), 0, UnityEngine.Random.Range(2f,-2f));
        
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Vector3 lastPosition = transform.localPosition;
        switch(actions.DiscreteActions[0]) 
        {
            case 0:
                movex = -1f;
                break;
            case 1:
                movex = 0f;
                break;
            case 2:
                movex = 1f;
                break;
                
        }switch(actions.DiscreteActions[1]) 
        {
            case 0:
                movez = -1f;
                break;
            case 1:
                movez = 0f;
                break;
            case 2:
                movez = 1f;
                break;
                
        }
        transform.localPosition += new Vector3(movex, 0, movez)*speed*Time.deltaTime;
        float diff = (transform.localPosition - goal.localPosition).magnitude;
        //float ldiff  = (lastPosition - goal.position).magnitude;

        AddReward(-diff);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions =  actionsOut.DiscreteActions;
        discreteActions[0] =1 + Convert.ToInt16(Input.GetAxisRaw("Horizontal"));
        discreteActions[1] =1 + Convert.ToInt16(Input.GetAxisRaw("Vertical"));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);
        sensor.AddObservation(goal.localPosition.x);
        sensor.AddObservation(goal.localPosition.z);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "goal")
        {
            SetReward(200f);
            playaMaterial.color = Color.green;
            EndEpisode();
        }
        if (collision.gameObject.tag == "wall")
        {
            SetReward(-200f);
            playaMaterial.color = Color.red;
            EndEpisode();
        }
    }

}
