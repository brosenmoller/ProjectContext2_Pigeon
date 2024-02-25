using UnityEngine;

public class CompassController : MonoBehaviour
{
    [Header("UI References")] 
    [SerializeField] private RectTransform compass_barTransform;
    [SerializeField] private RectTransform objective_markerTransform;
    [SerializeField] private RectTransform north_markerTransform;
    [SerializeField] private RectTransform south_markerTransform;

    [Header("Scene References")]
    [SerializeField] private Transform cameraObjectTransform;
    [SerializeField] private Transform objectiveObjectTransform;

    private void Update()
    {
        SetMarkerPosition(objective_markerTransform, objectiveObjectTransform.position);
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
        markerTransform.anchoredPosition = new Vector2(compass_barTransform.rect.width / 2 * compassPositionX, 0);
    }
}
