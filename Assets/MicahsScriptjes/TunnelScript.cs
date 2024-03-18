using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class TunnelScript : MonoBehaviour
{
    [HideInInspector] public bool insideTunnel;
    [HideInInspector] public bool onCooldown;
    [SerializeField] private GameObject pathFollower;
    [HideInInspector] public SplineContainer currentPath;
    private BirdMovement birdMovement;
    private GameObject currentFollower;
    private SplineAnimate splineAnimate;
    [SerializeField] private float snapSpeed;
    private const float CDamount = 2f;
    private float zAngle;
    [HideInInspector] public float tunnelPoint;

    void Start()
    {
        birdMovement = GetComponent<BirdMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (insideTunnel && !onCooldown)
        {
            MoveAlongTunnel();
        }
    }

    public void SetPath(float colliderPoint)
    {
        // create path follower at the location that the player has entered the tunnel
        birdMovement.enabled = false;
        tunnelPoint = colliderPoint;
        currentFollower = Instantiate(pathFollower);
        splineAnimate = currentFollower.GetComponent<SplineAnimate>();
        zAngle = transform.localEulerAngles.z;
    }
    private void MoveAlongTunnel()
    {
        // handle tunnel movement and rotation
        float speed = snapSpeed * Time.fixedDeltaTime;
        transform.position = Vector3.Slerp(transform.position, currentFollower.transform.position, speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentFollower.transform.rotation, speed);

        // if follower has neared end of path, exit player from the tunnel
        if(splineAnimate.NormalizedTime + tunnelPoint > 0.99 && insideTunnel)
        {
            StartCoroutine(ExitTunnel());
            insideTunnel = false;
            Destroy(currentFollower);
        }
    }

    private IEnumerator ExitTunnel()
    {
        // make sure that player doesn't re-enter tunnel
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, zAngle);
        onCooldown = true;
        birdMovement.enabled = true;
        yield return new WaitForSeconds(CDamount);
        onCooldown = false;
    }
}
