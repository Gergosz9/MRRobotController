using Assets.Scripts.ROS.Network;
using UnityEngine;
using Pose = Assets.Scripts.ROS.Data.Message.Primitives.Pose;


public class PositionManager : MonoBehaviour
{
    public GUILogger logger;
    public WebSocketClient webSocketClient;
    static Pose relativeCenter = new Pose(Vector3.zero, Quaternion.identity);
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

    // Start is called before the first frame update
    void Start()
    {
        currentPose = new Pose();
    }

    /*
     * Send a control message to the robot to reach a given position.
     * goal is the position in the world coordinates.
     */
    public void SendRobotTo(Pose goal)
    {

    }

    public static Pose TranslateToROSCordonates(Pose pose)
    {
        // ROS uses a right-handed coordinate system, Unity uses a left-handed coordinate system.
        Pose shiftedPose = new Pose(pose.position - relativeCenter.position, Quaternion.Inverse(relativeCenter.orientation) * pose.orientation);
        Vector3 rosPosition = new Vector3(shiftedPose.position.z, -shiftedPose.position.x, shiftedPose.position.y);
        Quaternion rosOrientation = new Quaternion(shiftedPose.orientation.z, -shiftedPose.orientation.x, shiftedPose.orientation.y, -shiftedPose.orientation.w);
        return new Pose(rosPosition, rosOrientation);
    }

    public static Pose TranslateToUnityCoordinates(Pose pose)
    {
        // ROS uses a right-handed coordinate system, Unity uses a left-handed coordinate system.
        Vector3 unityPosition = new Vector3(-pose.position.y, pose.position.z, pose.position.x);
        Quaternion unityOrientation = new Quaternion(-pose.orientation.y, pose.orientation.z, pose.orientation.x, -pose.orientation.w);
        return new Pose(unityPosition + relativeCenter.position, unityOrientation * relativeCenter.orientation);
    }
}
