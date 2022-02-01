using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject arObject;
    [SerializeField] private Camera arCamera;
    [SerializeField] private ARRaycastManager raycastManager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //test for arcore example 
            Ray ray = arCamera.ScreenPointToRay(Input.mousePosition);
            if (raycastManager.Raycast(ray,hits))
            {
                Pose pose = hits[0].pose;
                Instantiate(arObject,pose.position,pose.rotation);
            }
        }
    }
}
