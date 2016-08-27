// Date   : 27.08.2016 22:15
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public class WorldManager : MonoBehaviour {

    [SerializeField]
    private Transform wallContainer;
    public Transform WallContainer { get { return wallContainer; } }
    [SerializeField]
    private Camera2DFollow cameraFollower;
    public Camera2DFollow CameraFollower { get { return cameraFollower; } }
}
