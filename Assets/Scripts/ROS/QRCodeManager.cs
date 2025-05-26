using Microsoft.MixedReality.OpenXR;
using UnityEngine;


/// <summary>
/// QRCodeManager is a Unity MonoBehaviour that manages the detection and tracking of QR codes of the robot.
/// </summary>
public class QRCodeManager : MonoBehaviour
{
    [SerializeField]
    private string qrCodeIdString = "R0";

    void Awake()
    {
        GetComponent<ARMarkerManager>().markersChanged += OnQRCodesChanged;
    }

    void OnQRCodesChanged(ARMarkersChangedEventArgs args)
    {
        foreach (var qrCode in args.updated)
        {
            if (qrCode.GetDecodedString() == qrCodeIdString)
            {
                Pose qrPose = new Pose(qrCode.transform.position, Quaternion.Euler(0, qrCode.transform.rotation.eulerAngles.y, 0) * Quaternion.Euler(0, -90, 0));
                Debug.Log("QR Code detected: " + qrCode.GetDecodedString() + " at position: " + qrPose.position.ToString());
                PositionManager.Base_link = qrPose;
                Debug.Log("Link base updated by ARManager");
            }
        }
    }
}
