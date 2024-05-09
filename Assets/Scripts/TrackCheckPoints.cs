using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class TrackCheckPoints : MonoBehaviour
{
    [SerializeField] List<Transform> carTransformList;
    List<CheckpointSingle> cpSingleList = new();
    Dictionary<Transform,int> nextCPSingleIndexDict;

    public event EventHandler<CarCheckpointEventArgs> onCarCorrectCP;
    public event EventHandler<CarCheckpointEventArgs> onCarWrongCP;

    public class CarCheckpointEventArgs: EventArgs
    {
        public Transform carTransform;
    }
    private void Awake()
    {
        Transform cpListTransform = transform.Find("CheckpointList");
        foreach (Transform cpTransformSingle in cpListTransform)
        {
            CheckpointSingle cpSingle = cpTransformSingle.GetComponent<CheckpointSingle>();
            cpSingle.SetTrackCP(this);
            cpSingleList.Add(cpSingle);
        }

        nextCPSingleIndexDict = new();
        foreach(Transform carTransform in carTransformList)
        {
            nextCPSingleIndexDict.Add(carTransform,0);
        }
    }


    public void resetCP(Transform carTransform)
    {
            Debug.Log(nextCPSingleIndexDict[carTransform]);
            nextCPSingleIndexDict[carTransform] = 0;
    }

    public void CarThroughCP (CheckpointSingle cpSingle, Transform carTransform) 
    {
        int nextCPSingleIndex = nextCPSingleIndexDict[carTransform];

        if (cpSingleList.IndexOf(cpSingle) == nextCPSingleIndex) 
        {

            Debug.Log(nextCPSingleIndexDict[carTransform]);
            nextCPSingleIndexDict[carTransform] = (nextCPSingleIndex+ 1) % cpSingleList.Count;
            onCarCorrectCP?.Invoke(this, new CarCheckpointEventArgs { carTransform  = carTransform});
        }
        else
        {
            Debug.Log("UN------------   correct"); 
            onCarWrongCP?.Invoke(this , new CarCheckpointEventArgs { carTransform = carTransform });

        }
    
    
    
    
    }
}
