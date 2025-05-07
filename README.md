# Hololens Robot Controller

*Placeholder text for general description*

# Documentation

*Placeholder text for general description*

## Scripts

### File Structure

	Scripts/
	├── PinchPointDetector.cs
	├── Display/
	│   └── LidarDisplay.cs
	├── ROS/
	│   ├── PositionManager.cs
	│   ├── QRCodeManager.cs
	│   ├── Network/
	│   │   ├── ConnectionStatusUpdater.cs
	│   │   ├── RosBridgeClient.cs
	│   │   └── WebSocketClient.cs
	│   └── Data/
	│       ├── RosMessage.cs
	│       └── Message/
	│           ├── CostMapMsg.cs
	│           ├── GoalPoseMsg.cs
	│           ├── Msg.cs
	│           ├── PlanMsg.cs
	│           ├── ScanMsg.cs
	│           └── Primitives/
	│               ├── Header.cs
	│               ├── Operation.cs
	│               ├── Time.cs
	│               └── Topic.cs

### Descriptons

**PinchPointDetector.cs**

	/// PinchPointDetector is responsible for detecting pinch gestures using the MRTK HandsAggregatorSubsystem.
	/// It uses the MRTKRayInteractor to determine the pinch point and updates a preview pointer.

**LidarDisplay.cs**

    /// LidarDisplay is a Unity MonoBehaviour that displays Lidar points in the scene recieved from the ROS.

**PositionManager.cs**

	/// PositionManager is responsible for managing the position of the robot 
 	/// in Unity and translating between Unity and ROS coordinate systems.
	/// Contains methods to convert between Unity and ROS poses and vectors.

**QRCodeManager.cs**

	/// QRCodeManager is a Unity MonoBehaviour that manages the detection and tracking of QR codes of the robot.

**ConnectionStatusUpdater.cs**

	/// ConnectionStatusUpdater is a Unity MonoBehaviour that updates the connection status text based on the WebSocket connection state.

**RosBridgeClient.cs**

    /// Handles the messages recieved from the websocket.

**WebSocketClient.cs**

    /// WebSocketClient is a Unity MonoBehaviour that manages the connection to the Robot.
    /// Uses the NativeWebSocket library to handle WebSocket communication

**RosMessage.cs**

    /// RosMessage is a generic class that represents a message in ROS format.

**CostMapMsg.cs**

    /// CostMapMsg is a message type that contains information about a cost map,
    /// including its resolution, dimensions, and origin.

**GoalPoseMsg.cs**

    /// GoalPoseMsg is a message type that contains a pose (position and orientation) in 3D space.

**Msg.cs**

    /// Abstract class that serves as a base for all ROS message types.

**PlanMsg.cs**

    /// An array of poses that represents a Path for a robot to follow

**ScanMsg.cs** 

    /// ScanMsg is a message type used in ROS to represent LIDAR scan data.

**Header.cs**
   
    /// Header is a common message header used in ROS messages.

**Operation.cs**


    /// Defines the operation types for ROS messages.

**Time.cs**

	/// Defines the Time message type used in ROS messages.

**Topic.cs**

    /// Defines the topic names used in ROS messages.
 
## TODOs
 - [x] Implement GUI logger
	 - [ ] Update Documentation
 - [x] Implement and optimize Lidar display
	 - [x] Convert Lidar scans to world points
	 - [x] Implment a display
  	 - [ ] Optimize for large number of scans (Maybe rework the display?)
	 - [ ] Update Documentation
 - [ ] Implement Costmap display
	 - [ ] Convert Costmap JSONs to usable data
	 - [ ] Implment a display
	 - [ ] Update Documentation
 - [x] Iplement gesture controlled robot movement
 	 - [x] Started implementation (RobotGestureControlV2 branch)
 	 - [x] FIX - Pose doesn't work with ROS message, causes recursion error, might have to construct the message manually
	 - [ ] Update Documentation

