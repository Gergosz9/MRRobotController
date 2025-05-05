using Microsoft.MixedReality.OpenXR;
using UnityEngine;

public class QRCodeManager : MonoBehaviour
{
    public PositionManager positionManager;
    public GUILogger logger;
    bool QRFirstDetected = true;

    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<ARMarkerManager>().markersChanged += OnQRCodesChanged;
    }

    void OnQRCodesChanged(ARMarkersChangedEventArgs args)
    {
        if (QRFirstDetected && args.added.Count > 0)
        {
            var qrCode = args.added[0];
            logger.Log($"QR code with the ID {qrCode.trackableId} added.");
            logger.Log($"Pos: {qrCode.transform.position} Rot: {qrCode.transform.rotation}");
            positionManager.RelativeCenter.position = qrCode.transform.position;
            positionManager.RelativeCenter.orientation = qrCode.transform.rotation;
            positionManager.CurrentPose.position = qrCode.transform.position;
            positionManager.CurrentPose.orientation = qrCode.transform.rotation;
            logger.Log("Robot position detected. Relative center set.");
            QRFirstDetected = false;
        }
        foreach (ARMarker qrCode in args.added)
        {
            logger.Log($"QR code with the ID {qrCode.trackableId} added.");
            logger.Log($"Pos: {qrCode.transform.position} Rot: {qrCode.transform.rotation}");
        }

        foreach (ARMarker qrCode in args.removed)
            logger.Log($"QR code with the ID {qrCode.trackableId} removed.");

        foreach (ARMarker qrCode in args.updated)
        {
            logger.Log($"QR code with the ID {qrCode.trackableId} updated.");
            logger.Log($"Pos:{qrCode.transform.position} Rot:{qrCode.transform.rotation}");
        }

        if (args.updated.Count > 0)
        {
            var qrCode = args.updated[0];
            logger.Log($"QR code with the ID {qrCode.trackableId} updated.");
            logger.Log($"Pos: {qrCode.transform.position} Rot: {qrCode.transform.rotation}");
            positionManager.CurrentPose.position = qrCode.transform.position;
            positionManager.CurrentPose.orientation = qrCode.transform.rotation;
            logger.Log("Robot position updated by ARMarker.");
        }
    }
}
