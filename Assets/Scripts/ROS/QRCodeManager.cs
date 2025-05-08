using Microsoft.MixedReality.OpenXR;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;


/// <summary>
/// QRCodeManager is a Unity MonoBehaviour that manages the detection and tracking of QR codes of the robot.
/// </summary>
public class QRCodeManager : MonoBehaviour
{
    [SerializeField]
    private PositionManager positionManager;


    bool QRFirstDetected = true;
    TrackableId qrCodeId;

    void Awake()
    {
        GetComponent<ARMarkerManager>().markersChanged += OnQRCodesChanged;
    }

    void OnQRCodesChanged(ARMarkersChangedEventArgs args)
    {
        if (QRFirstDetected && args.added.Count > 0)
        {
            var qrCode = args.added[0];

            Debug.Log($"[QRCodeManager] QR code with the ID {qrCode.trackableId} added.");
            Debug.Log($"[QRCodeManager] Pos: {qrCode.transform.position} Rot: {qrCode.transform.rotation}");
            qrCodeId = qrCode.trackableId;

            positionManager.RelativeCenter = new Pose(qrCode.transform.position, qrCode.transform.rotation);
            positionManager.CurrentPose = new Pose(qrCode.transform.position, qrCode.transform.rotation);

            Debug.Log("[QRCodeManager] Robot position detected. Relative center set.");

            QRFirstDetected = false;
        }
        foreach (ARMarker qrCode in args.added)
        {
            Debug.Log($"[QRCodeManager] QR code with the ID {qrCode.trackableId} added.");
            Debug.Log($"[QRCodeManager] Pos: {qrCode.transform.position} Rot: {qrCode.transform.rotation}");
        }

        foreach (ARMarker qrCode in args.removed)
            Debug.Log($"[QRCodeManager] QR code with the ID {qrCode.trackableId} removed.");

        foreach (ARMarker qrCode in args.updated)
        {
            Debug.Log($"[QRCodeManager] QR code with the ID {qrCode.trackableId} updated.");
            Debug.Log($"[QRCodeManager] Pos:{qrCode.transform.position} Rot:{qrCode.transform.rotation}");
            if (qrCode.trackableId == qrCodeId)
            {
                positionManager.CurrentPose = new Pose(qrCode.transform.position, qrCode.transform.rotation);
                Debug.Log("[QRCodeManager] Robot position updated by ARMarker.");
            }
        }
    }
}
