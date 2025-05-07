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

            //string message = ConstructGoalPoseMsg(targetPosition, quaternion);
            //webSocketClient.SendMessage(message);
        }
    }

    private string ConstructGoalPoseMsg(Vector3 targetPosition, Quaternion targetRotation)
    {
        Time time = new Time((uint)DateTime.Now.Second, (uint)DateTime.Now.Millisecond);

        Header header = new Header(time, "GoalPose");

        GoalPoseMsg goalPoseMsg = new GoalPoseMsg(header, new Pose(targetPosition, targetRotation));

        RosMessage<GoalPoseMsg> rosMessage = new RosMessage<GoalPoseMsg>(Operation.publish, "/goal_pose", goalPoseMsg);

        string jsonString = JsonConvert.SerializeObject(rosMessage, Formatting.Indented);
        return jsonString;
    }
}
