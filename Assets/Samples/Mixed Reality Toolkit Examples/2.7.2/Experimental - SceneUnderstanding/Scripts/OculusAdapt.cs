using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusAdapt : MonoBehaviour
{
    public List<MeshCollider> walkableFloors;
    public TemporaryPlane temporaryPlane;
    private float timer;

    private void Awake(){
        walkableFloors = new List<MeshCollider>();
    }

    void Start(){
        //temporaryPlane = GetComponentInParent<TemporaryPlane>();
    }

    void Update(){
        if(timer < 5) { timer += Time.deltaTime; }
        else { 
            if(temporaryPlane.gameObject.activeSelf){ 
                Spawn(); 
            } 
        }
    }

    // TD: TODO: find a way to reliably trigger Spawn() when buildings are done generating
    private void FixedUpdate(){
        /*
        foreach(MeshCollider MC in walkableFloors){ 
            if(!MC.gameObject.activeSelf) { return; }
        }
        Spawn();
        */
    }

    public void Add(MeshCollider MC){
        walkableFloors.Add(MC);
    }

    // TD: TODO: spawn OVRPlayerController in a valid walkableFloors location along the bounding box
    public void Spawn(){
        temporaryPlane.gameObject.SetActive(false);


    }
}
