
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CarAgent : Agent
{
    int CheckpointReward;
    public float wallCollidedForSeconds;
    // need to add a  onStart
    // need to make it face the right direction (watch codemonkey for context)
    PrometeoCarController pcc;
    private float wallCollidePenalty;
    
    [SerializeField] private TrackCheckPoints TrackCheckPoints;
    private void Start()
    {
        pcc = gameObject.GetComponent<PrometeoCarController>();
        TrackCheckPoints.onCarCorrectCP += onCarCorrectCP;
        TrackCheckPoints.onCarWrongCP += onCarCorrectCP;

    }

    public override void OnEpisodeBegin()
    {
        CheckpointReward = 1;
        wallCollidePenalty = 0f;
        pcc.carRigidbody.velocity = Vector3.zero;
        pcc.DecelerateCar();
        transform.position = new Vector3 (Random.Range(-6.5f,6.5f), 0, Random.Range(-11,11));
        transform.rotation = Quaternion.Euler(0,180,0);
        TrackCheckPoints.resetCP(this.transform);
    }

    public void onCarCorrectCP(object sender, TrackCheckPoints.CarCheckpointEventArgs e)
    {
        if (e.carTransform == transform)
        {
            AddReward(CheckpointReward++);
        }
    }
    public void onCarWrongCP(object sender, TrackCheckPoints.CarCheckpointEventArgs e)
    {
        if (e.carTransform == transform)
        {
            AddReward(-CheckpointReward);
        }
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        pcc.inputs[0] = actions.DiscreteActions.Array[0];
        pcc.inputs[1] = actions.DiscreteActions.Array[1];
        pcc.inputs[2] = 0;


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
        //sensor.AddObservation(pcc.frontLeftCollider.brakeTorque);
        //sensor.AddObservation(pcc.frontLeftCollider.motorTorque);
        
        //sensor.AddObservation(pcc.frontRightCollider.brakeTorque);
        //sensor.AddObservation(pcc.frontRightCollider.motorTorque);
        
        //sensor.AddObservation(pcc.rearLeftCollider.motorTorque);
        //sensor.AddObservation(pcc.rearLeftCollider.brakeTorque);
        
        //sensor.AddObservation(pcc.rearRightCollider.motorTorque );
        //sensor.AddObservation(pcc.rearRightCollider.brakeTorque);
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


        //if (Input.GetKey(KeyCode.Space))
        //{
        //    discreteActions[2] = 1;
        //}
        //else 
        //{ 
        //    discreteActions[2] = 0; 
        //}

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            AddReward(-0.5f);
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            //Debug.Log(wallCollidePenalty);
            AddReward(wallCollidePenalty);
            wallCollidePenalty -= 0.1f* Time.deltaTime;
        }
        if (wallCollidePenalty < -wallCollidedForSeconds/10)   EndEpisode();
    }

}
