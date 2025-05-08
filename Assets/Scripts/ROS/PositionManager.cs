using Assets.Scripts.ROS.Data.Message;
using Assets.Scripts.ROS.Data.Message.Primitives;
using Assets.Scripts.ROS.Network;
using Newtonsoft.Json;
using System;
using UnityEngine;
using Time = Assets.Scripts.ROS.Data.Message.Primitives.Time;


/// <summary>
/// PositionManager is responsible for managing the position of the robot in Unity and translating between Unity and ROS coordinate systems.
/// Contains methods to convert between Unity and ROS poses and vectors.
/// </summary>
public class PositionManager : MonoBehaviour
{
    [SerializeField]
    static Pose relativeCenter = new Pose(Vector3.zero, Quaternion.identity);
    [SerializeField]
    private WebSocketClient webSocketClient;

    public Pose RelativeCenter
    {
        get { return relativeCenter; }
        set { relativeCenter = value; }
    }

    Pose currentPose;
    public Pose CurrentPose
    {
        get { return currentPose; }
        set
        {
            currentPose = value;
        }
    }

    void Start()
    {
        currentPose = new Pose();
    }

    public void SendRobotTo(Pose goal)
    {
        Pose translatedGoal = TranslateToROSPose(goal);

        var rosMessage = new RosMessage<GoalPoseMsg>(
            Operation.publish,
            "/goal_pose",
            new GoalPoseMsg(
                new Header(
                    new Time((uint)DateTime.Now.Second, (uint)DateTime.Now.Millisecond*1000),
                    "map"
                ),
                translatedGoal
            )
        );

        string message = JsonConvert.SerializeObject(rosMessage);
        webSocketClient.SendMessage(message);

    }

    public static Pose TranslateToUnityPose(Pose pose)
    {
        Pose unityPose = new Pose(TranslateToUnityVector(pose.position), TranslateToUnityQuarternion(pose.rotation));
        return unityPose;
    }

    public static Pose TranslateToROSPose(Pose pose)
    {
        Pose rosPose = new Pose(TranslateToROSVector(pose.position), TranslateToROSQuaternion(pose.rotation));
        return rosPose;
    }

    public static Vector3 TranslateToUnityVector(Vector3 vector)
    {
        Vector3 unityVector = new Vector3(vector.y, vector.z, -vector.x);
        return unityVector+relativeCenter.position;
    }

    public static Vector3 TranslateToROSVector(Vector3 vector)
    {
        Vector3 rosVector = new Vector3(-vector.z, vector.x, 0f);
        return rosVector-relativeCenter.position;
    }

    public static Quaternion TranslateToUnityQuarternion(Quaternion quaternion)
    {
        Quaternion unityQuaternion = new Quaternion(quaternion.y, quaternion.z, -quaternion.x, quaternion.w);
        return unityQuaternion;
    }

    public static Quaternion TranslateToROSQuaternion(Quaternion quaternion)
    {
        Quaternion rosQuaternion = new Quaternion(-quaternion.z, quaternion.x, quaternion.y, quaternion.w);
        return rosQuaternion;
    }

    public static Vector3 TranslateLidarToUnityVector(float range, float angle)
    {
        Vector3 rospoint = new Vector3(
            range * Mathf.Cos(angle),
            range * Mathf.Sin(angle),
            0f
        );
        Vector3 translated = TranslateToUnityVector(rospoint);
        return translated;
    }
}
