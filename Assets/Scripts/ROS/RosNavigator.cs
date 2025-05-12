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
            q.y = 0;
            Quaternion quaternion = Quaternion.LookRotation(q);

            Pose p = PositionManager.ConvertToROSPose(new Pose(targetPosition, quaternion));

            string message = ConstructGoalPoseMsg(p);
            webSocketClient.SendMessage(message);
        }
    }

    private string ConstructGoalPoseMsg(Pose pose)
    {
        var rosMessage = new RosMessage<GoalPoseMsg>(
            Operation.publish,
            "/goal_pose",
            new GoalPoseMsg(
                new Header(
                    new Time((uint)DateTime.Now.Second, (uint)DateTime.Now.Millisecond),
                    "GoalPose"
                ),
                pose
            )
        );

        return JsonConvert.SerializeObject(rosMessage, Formatting.None);
    }
}
