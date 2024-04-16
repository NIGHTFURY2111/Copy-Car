using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class TrackCheckPoints : MonoBehaviour
{
    [SerializeField] List<Transform> carTransformList;
    List<CheckpointSingle> cpSingleList = new();
    Dictionary<Transform,int> nextCPSingleIndexDict;
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

    public void CarThroughCP (CheckpointSingle cpSingle, Transform carTransform) 
    {
        int nextCPSingleIndex = nextCPSingleIndexDict[carTransform];

        if (cpSingleList.IndexOf(cpSingle) == nextCPSingleIndex) 
        {
            Debug.Log("correct");
            nextCPSingleIndexDict[carTransform] = (nextCPSingleIndex+ 1) % cpSingleList.Count;
        }
        else
        {
            Debug.Log("UN------------   correct"); 

        }
    
    
    
    
    }
}
