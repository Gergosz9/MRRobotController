using Microsoft.MixedReality.OpenXR;
using UnityEngine;

public class QRCodeManager : MonoBehaviour
{
    [SerializeField]
    private PositionManager positionManager;
    [SerializeField]
    private GUILogger logger;


    bool QRFirstDetected = true;

    void Awake()
    {
        GetComponent<ARMarkerManager>().markersChanged += OnQRCodesChanged;
    }

    void OnQRCodesChanged(ARMarkersChangedEventArgs args)
    {
        if (QRFirstDetected && args.added.Count > 0)
        {
            var qrCode = args.added[0];

            logger.Log($"[QRCodeManager] QR code with the ID {qrCode.trackableId} added.");
            logger.Log($"[QRCodeManager] Pos: {qrCode.transform.position} Rot: {qrCode.transform.rotation}");

            positionManager.RelativeCenter = new Pose(qrCode.transform.position, qrCode.transform.rotation);
            positionManager.CurrentPose = new Pose(qrCode.transform.position, qrCode.transform.rotation);

            logger.Log("[QRCodeManager] Robot position detected. Relative center set.");

            QRFirstDetected = false;
        }
        foreach (ARMarker qrCode in args.added)
        {
            logger.Log($"[QRCodeManager] QR code with the ID {qrCode.trackableId} added.");
            logger.Log($"[QRCodeManager] Pos: {qrCode.transform.position} Rot: {qrCode.transform.rotation}");
        }

        foreach (ARMarker qrCode in args.removed)
            logger.Log($"[QRCodeManager] QR code with the ID {qrCode.trackableId} removed.");

        foreach (ARMarker qrCode in args.updated)
        {
            logger.Log($"[QRCodeManager] QR code with the ID {qrCode.trackableId} updated.");
            logger.Log($"[QRCodeManager] Pos:{qrCode.transform.position} Rot:{qrCode.transform.rotation}");
        }

        if (args.updated.Count > 0)
        {
            var qrCode = args.updated[0];

            logger.Log($"[QRCodeManager] QR code with the ID {qrCode.trackableId} updated.");
            logger.Log($"[QRCodeManager] Pos: {qrCode.transform.position} Rot: {qrCode.transform.rotation}");

            positionManager.CurrentPose = new Pose(qrCode.transform.position, qrCode.transform.rotation);

            logger.Log("[QRCodeManager] Robot position updated by ARMarker.");
        }
    }
}
