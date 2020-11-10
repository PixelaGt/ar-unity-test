using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]    //To use this code we will need an ARRayCastManager                                                                        
public class PlaceObject : MonoBehaviour
{
    public GameObject objectToPlace; //Object or game that will be placed in our scene

    private GameObject objectPlaced = null; //Object or game already placed in our scene
    private ARRaycastManager _aRRaycastManager; /* A ray that will be shoot in a specific direction and that
                                                    will colide with other game objects present in a scene */
    private Vector2 positionToPlace; //The coordinates in the Y and X axis where we will place the object.
    private static List<ARRaycastHit> hitsOfARCastManager = new List<ARRaycastHit>(); /*A list of objects that 
                                                                                        were hit by the ARCast manager*/

    // Start is called before the first frame update
    void Awake()
    {
        _aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    /*
     * Get Touch Position
     *
     * This method will check if the user touches a position in the screen, and if so we will obtain those coordinates
     * They will help us to know where to place the object.
     * 
     * returns bool and Vector2
     */
    private bool getTouchPosition(out Vector2 coordinates)
    {
        if (Input.touchCount > 0)
        {
            coordinates = Input.GetTouch(0).position;
            return true;
        }
        coordinates = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        //If there is a touch from the user, we get the touch coordinates.
        if (this.getTouchPosition(out Vector2 coordinates))
        {
            //The function returns true if it hits a game object (or multiple game objects) given certain coordinates
            if (_aRRaycastManager.Raycast(coordinates, hitsOfARCastManager, TrackableType.PlaneWithinPolygon))
            {
                //If the raycast hits an object, we obtain the position where it collides
                var positionToPlace = hitsOfARCastManager[0].pose;
                if (objectPlaced == null)
                {
                    //If the object hasn't been placed in the scene before, we instantiate it and put it where the 
                    //coordinates indicate
                    objectPlaced = Instantiate(objectToPlace, positionToPlace.position, positionToPlace.rotation);
                }
                else
                {
                    //if we've already placed the object, then we just update it's position in the scene
                    objectPlaced.transform.position = positionToPlace.position;
                }
            }
        }
    }
}
