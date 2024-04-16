using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointSingle : MonoBehaviour
{
    TrackCheckPoints tcp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ReturnParent>(out ReturnParent rp))
            tcp.CarThroughCP(this, rp.pc.transform);
            //Debug.Log( rp.pc.name);
    }

    public void SetTrackCP(TrackCheckPoints tcp)
    {
        this.tcp = tcp; 
    }
}
