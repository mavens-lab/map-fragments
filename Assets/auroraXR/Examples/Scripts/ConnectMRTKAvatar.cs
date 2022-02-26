using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Teleport;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using System.Linq;

public class ConnectMRTKAvatar : MonoBehaviour
{
    enum GameObjectType {
        TeleportPointer,
        PokePointer,
        GrabPointer,
        GazeCursor,
        Camera
    }

    [SerializeField]
    string objectSearchString;

    [SerializeField]
    GameObjectType objectType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    System.Type getGameObjectType()
    {
        switch (objectType)
        {
            case GameObjectType.TeleportPointer:
                return typeof(ParabolicTeleportPointer);
            case GameObjectType.PokePointer:
                return typeof(PokePointer);
            case GameObjectType.GrabPointer:
                return typeof(SpherePointer);
            case GameObjectType.GazeCursor:
                return typeof(AnimatedCursor);
            case GameObjectType.Camera:
                return typeof(Camera);
            default:
                return typeof(Camera);
        }
    }

    // Update is called once per frame
    void Update()
    {
        var playspace = GameObject.Find("MixedRealityPlayspace");
        var matchingComponents = playspace.GetComponentsInChildren(getGameObjectType());
        var component = matchingComponents.FirstOrDefault(c => c.gameObject.name.Contains(objectSearchString));

        if (component == null)
        {
            //gameObject.SetActive(false);
        } else
        {
            //gameObject.SetActive(true);
            transform.position = component.transform.position;
            transform.rotation = component.transform.rotation;
        }
    }
}
