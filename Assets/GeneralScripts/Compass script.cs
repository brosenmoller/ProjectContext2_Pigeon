using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compassscript : MonoBehaviour
{

    public RectTransform compass_barTransform;

    public RectTransform objective_markerTransform;
    public RectTransform north_markerTransform;
    public RectTransform south_markerTransform;

    public Transform cameraObjectTransform;
    public Transform objectiveObjectTransform;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetMarkerPosition(objective_markerTransform, objectiveObjectTransform.position);
        SetMarkerPosition(north_markerTransform, Vector3.forward * 1000);
        SetMarkerPosition(south_markerTransform, Vector3.back * 1000);
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionTotarget = worldPosition - cameraObjectTransform.position;
        float angle = Vector2.Angle(new Vector2(directionTotarget.x, directionTotarget.z), new Vector2(cameraObjectTransform.transform.forward.x, cameraObjectTransform.transform.forward.z));
        float compassPositionX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
        markerTransform.anchoredPosition = new Vector2(compass_barTransform.rect.width / 2 * compassPositionX, 0);
    }
}
