using Assets.Scripts.ROS.Network;
using UnityEngine;


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
        // Translate the goal position to the robot's local coordinates.
        Pose translatedGoal = TranslateToROSPose(goal);
        // Send the control message to the robot.
        string message = JsonConvert.SerializeObject(new RosMessage<GoalPoseMsg>{
            op = "publish",
            topic = "/goal_pose",
            msg = new GoalPoseMsg{
                header = new Header{
                    stamp = new TimeStamp{
                        sec = (int)Time.time,
                        nsec = (int)((Time.time - (int)Time.time) * 1e9)
                    },
                    frame_id = "base_link"
                },
                pose = translatedGoal
            }
        });
        webSocketClient.SendMessage(message);
        logger.Log("Sending goal: " + translatedGoal.ToString());

    }

    public static Pose TranslateToROSPose(Pose pose)
    {
        // ROS uses a right-handed coordinate system, Unity uses a left-handed coordinate system.
        Pose shiftedPose = new Pose(pose.position - relativeCenter.position, Quaternion.Inverse(relativeCenter.rotation) * pose.rotation);
        Vector3 rosPosition = new Vector3(shiftedPose.position.z, -shiftedPose.position.x, shiftedPose.position.y);
        Quaternion rosrotation = new Quaternion(shiftedPose.rotation.z, -shiftedPose.rotation.x, shiftedPose.rotation.y, -shiftedPose.rotation.w);
        return new Pose(rosPosition, rosrotation);
    }

    public static Pose TranslateToUnityPose(Pose pose)
    {
        // ROS uses a right-handed coordinate system, Unity uses a left-handed coordinate system.
        Vector3 unityPosition = new Vector3(-pose.position.y, pose.position.z, pose.position.x);
        Quaternion unityrotation = new Quaternion(-pose.rotation.y, pose.rotation.z, pose.rotation.x, -pose.rotation.w);
        return new Pose(unityPosition + relativeCenter.position, unityrotation * relativeCenter.rotation);
    }

    public static Vector3 TranslateToUnityVector(Vector3 vector)
    {
        // ROS uses a right-handed coordinate system, Unity uses a left-handed coordinate system.
        return new Vector3(-vector.y, vector.z, vector.x) + relativeCenter.position;
    }

    public static Vector3 TranslateToROSVector(Vector3 vector)
    {
        // ROS uses a right-handed coordinate system, Unity uses a left-handed coordinate system.
        Vector3 translated = vector - relativeCenter.position;
        return new Vector3(translated.z, -translated.x, translated.y);
    }
}
