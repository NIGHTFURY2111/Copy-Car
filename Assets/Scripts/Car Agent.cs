using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CarAgent : Agent
{
    PrometeoCarController pcc;
    private void Start()
    {
        pcc = gameObject.GetComponent<PrometeoCarController>();
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        pcc.inputs = actions.DiscreteActions.Array;


        //Debug.Log(pcc.inputs[0] + " " + pcc.inputs[1] + " " + pcc.inputs[2]);

    }


    public override void CollectObservations(VectorSensor sensor)
    {   
        //position
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);
        
        //rotation
        sensor.AddObservation(transform.localRotation.y);
        
        //speed
        sensor.AddObservation(pcc.carSpeed);
        
        //misc values
        sensor.AddObservation(pcc._driftingAxis);
        sensor.AddObservation(pcc._steeringAxis);
        sensor.AddObservation(pcc._throttleAxis);

        //wheel torques
        sensor.AddObservation(pcc.frontLeftCollider.brakeTorque);
        sensor.AddObservation(pcc.frontLeftCollider.motorTorque);
        
        sensor.AddObservation(pcc.frontRightCollider.brakeTorque);
        sensor.AddObservation(pcc.frontRightCollider.motorTorque);
        
        sensor.AddObservation(pcc.rearLeftCollider.motorTorque);
        sensor.AddObservation(pcc.rearLeftCollider.brakeTorque);
        
        sensor.AddObservation(pcc.rearRightCollider.motorTorque );
        sensor.AddObservation(pcc.rearRightCollider.brakeTorque);
        

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        {
            discreteActions[0] = 1;
        }
        else if (Input.GetKey(KeyCode.S)) 
        {
            discreteActions[0] = 2;
        }
        else 
        { 
            discreteActions[0] = 0; 
        }


        if (Input.GetKey(KeyCode.A))
        {
            discreteActions[1] = 1;
        }
        else if (Input.GetKey(KeyCode.D)) 
        {
            discreteActions[1] = 2;
        }
        else 
        { 
            discreteActions[1] = 0; 
        }


        if (Input.GetKey(KeyCode.Space))
        {
            discreteActions[2] = 1;
        }
        else 
        { 
            discreteActions[2] = 0; 
        }

    }

}
