using UnityEngine;

public class CompassController : MonoBehaviour
{
    [Header("UI References")] 
    [SerializeField] private RectTransform compassBarTransform;
    [SerializeField] private RectTransform objectiveMarkerTransform;
    [SerializeField] private RectTransform northMarkerTransform;
    [SerializeField] private RectTransform southMarkerTransform;

    [Header("Scene References")]
    [SerializeField] private Transform cameraObjectTransform;
    [SerializeField] private Transform objectiveObjectTransform;

    private void Update()
    {
        SetMarkerPosition(objectiveMarkerTransform, objectiveObjectTransform.position);
        //SetMarkerPosition(north_markerTransform, Vector3.forward * 1000);
        //SetMarkerPosition(south_markerTransform, Vector3.back * 1000);
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionTotarget = worldPosition - cameraObjectTransform.position;

        float angle = Vector2.SignedAngle(
            new Vector2(directionTotarget.x, directionTotarget.z), 
            new Vector2(cameraObjectTransform.transform.forward.x, cameraObjectTransform.transform.forward.z)
        );

        float compassPositionX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
        markerTransform.anchoredPosition = new Vector2(compassBarTransform.rect.width / 2 * compassPositionX, 0);
    }
}
