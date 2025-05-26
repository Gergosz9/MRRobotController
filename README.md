# Interactive Robot Control Using HoloLens  

**Authors:** Gergő Szalay, Marcell Telek  
**Consultant:** Dr. Attila Hideg  
**Location:** Budapest, 2025

### Budapest University of Technology and Economics (BME)  
****Faculty of Electrical Engineering and Informatics****  
****Department of Automation and Applied Informatics****  
---

## Table of Contents  

1. [Introduction](#introduction)  
2. [Objective](#objective)  
3. [Implementation](#implementation)  
   - [Unity Scene](#unity-scene)  
   - [Application Structure](#application-structure)  
   - [Classes](#classes)  
   - [JSON Serialization](#json-serialization)  
4. [Application Usage](#application-usage)  
   - [Graphical User Interface](#graphical-user-interface)  
   - [Display](#display)  
   - [Control](#control)  
5. [Summary](#summary)  

---

## Introduction  

### Objective  

The goal of this project is to create an interactive augmented reality (AR) application in Unity for the Microsoft HoloLens 2 mixed reality headset. The application allows users to control a robot running the Robot Operating System (ROS) over a network. The relative position of the robot and the headset is determined using a QR code placed on the robot. The HoloLens displays data captured by the robot's LIDAR sensor, aligning it with the real environment.

---

## Implementation  

### Unity Scene  

We aimed to use the building blocks provided by the Mixed Reality Toolkit (MRTK) wherever possible. For other functionality, we created custom GameObjects with attached scripts. Below are the key objects used in the application:  

- **MRTK XR Rig**: Handles mixed reality input simulation for testing within Unity.  
- **Pinch Point Detector**: Detects pinch gestures using MRTK's ray interactors.  
- **ROS Navigator**: Processes interactions and sends navigation commands to the robot.  
- **QR Code Manager**: Recognizes QR codes and tracks the robot's coordinates.  
- **WebSocket Manager**: Manages communication with ROS.  
- **Lidar Display**: Visualizes scanned LIDAR points in 3D space.  
- **Costmap Display**: Displays the robot’s movement costmap.  
- **Path Display**: Shows the planned movement paths.  
- **Hand Menu**: Provides the application’s graphical user interface (GUI).  
- **GUI Logger**: Displays real-time logs.  

---

### Application Structure  

#### Classes Overview  

- `Msg`: Abstract base class for all ROS message types.  
- `Header`: Stores metadata for ROS messages.  
- `Operation`: Defines publish and subscribe operations.  
- `Time`: Represents time formats in ROS.  
- `Topic`: Manages ROS topics such as LIDAR scans, costmaps, and navigation data.  
- `Transform & TransformStamped`: Handles 3D position and rotation transformations.  
- `PoseStamped`: Represents positional data with timestamps.  
- `CostMapMsg`: Stores obstacle and movement cost data.  
- `GoalPoseMsg`: Defines a goal position for the robot.  
- `PlanMsg & PathMsg`: Represent planned movement paths.  
- `ScanMsg`: Processes LIDAR scan data.  
- `RosMessage & TFMessage`: Provide structured communication with ROS.  
- `WebSocketClient`: Implements WebSocket communication.  
- `RosBridgeClient`: Converts data between Unity and ROS formats.  
- `ConnectionStatusUpdater`: Displays connection status on the GUI.  
- `QRCodeManager`: Handles QR code detection and robot localization.  
- `PositionManager`: Manages coordinate systems and transformations.  
- `RosNavigator`: Handles pinch gestures for robot control.  
- `PinchPointDetector`: Detects pinch gestures using MRTK.  
- `GUILogger`: Logs messages in the GUI for debugging.  

---

### JSON Serialization  

The project uses optimized `JsonConverter` classes for efficient serialization and deserialization of ROS messages in Unity. This improves performance and reduces latency.  

#### Key Converters  

- `TimeJsonConverter` & `HeaderJsonConverter`: Handle ROS timestamps and message headers.  
- `PoseJsonConverter` & `TransformJsonConverter`: Convert Unity position and orientation data to ROS-compatible formats.  
- `CostMapMsgJsonConverter`, `GoalPoseMsgJsonConverter`, `PathMsgJsonConverter`, `ScanMsgJsonConverter`: Handle specialized ROS messages for navigation and sensor data.  

---

## Application Usage  

### Graphical User Interface  

The application’s GUI is implemented using MRTK’s Hand Menu, which appears when the user raises their palm and disappears when they lower their hand. The menu includes options for enabling/disabling various visualizations and initiating the WebSocket connection.  

#### Features  

- **Lidar**: Displays scanned points from the robot’s LIDAR sensor.  
- **Costmap**: Shows the calculated movement costmap.  
- **Path**: Visualizes the planned movement path.  
- **Pivots**: Displays reference points for coordinate systems.  
- **Debug Log**: Enables an on-screen debugging log panel.  
---

### Control  

To control the robot:  

1. Establish a network connection.  
2. Scan the QR code on the robot.  
3. Use hand-tracking rays to select a target position.  
4. Pinch fingers together to place the robot at the desired location.  
5. Release fingers to send the command and start movement.  

---

## Summary  

This project developed an interactive augmented reality (AR) application for Microsoft HoloLens 2, enabling remote control of a ROS-based robot. The Unity-powered system integrates MRTK, WebSocket communication, and real-time visualization of LIDAR data, costmaps, and planned paths.  

Users control the robot through hand gestures, selecting destinations via pinch interactions. The application optimizes JSON processing for performance, ensuring low latency and reliable communication.  

Challenges included limited documentation for MRTK3, complex coordinate system conversions between ROS and Unity, and real-time rendering optimizations. These obstacles were successfully addressed, resulting in a functional and efficient AR interface.  

---

## References  

- **Microsoft Mixed Reality Toolkit (MRTK3) Official Repository**  
  [GitHub - MixedRealityToolkit](https://github.com/MixedRealityToolkit/MixedRealityToolkit-Unity)  
- **ROS & Unity Coordinate System Conversion**  
  [GitHub - ROSSharp](https://github.com/siemens/ros-sharp/wiki/Dev_ROSUnityCoordinateSystemConversion)  
