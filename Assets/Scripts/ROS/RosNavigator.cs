using Assets.Scripts.ROS.Data.Message;
using Assets.Scripts.ROS.Data.Message.Primitives;
using Assets.Scripts.ROS.Network;
using MixedReality.Toolkit.Input;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Time = Assets.Scripts.ROS.Data.Message.Primitives.Time;

public class RosNavigator : MonoBehaviour
{
    [SerializeField]
    private WebSocketClient webSocketClient;
    [SerializeField]
    private PinchPointDetector pinchPointDetector;
    [SerializeField]
    private PositionManager positionManager;

    private MRTKRayInteractor lastPinchLocation;
    public void Update()
    {
        if (webSocketClient != null && pinchPointDetector != null)
        {
            if(pinchPointDetector.DetectPinchLocation() != null)
            {
                lastPinchLocation = pinchPointDetector.DetectPinchLocation();
            }
            if (pinchPointDetector.DetectPinchRelease())
            {
                HandleNavigation(lastPinchLocation);
            }
        }
    }

    public void HandleNavigation(MRTKRayInteractor interactor)
    {
        if (interactor != null)
        {
            Vector3 targetPosition = interactor.rayEndPoint;

            Vector3 q = interactor.rayEndPoint - interactor.rayOriginTransform.position;
            Quaternion quaternion = Quaternion.LookRotation(q);

            Pose p = new Pose(targetPosition, quaternion);

            positionManager.SendRobotTo(p);
        }
    }
}
