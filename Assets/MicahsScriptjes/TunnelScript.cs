using PathCreation;
using PathCreation.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelScript : MonoBehaviour
{
    [HideInInspector] public bool insideTunnel;
    [HideInInspector] public bool onCooldown;
    [SerializeField] private GameObject pathFollower;
    [HideInInspector] public PathCreator currentPath;
    private BirdMovement birdMovement;
    private GameObject currentFollower;
    private PathFollower _pathFollower;
    [SerializeField] private float snapSpeed;
    private Vector3 storedFollowerPos;
    private const float CDamount = 0.5f;
    private float zAngle;
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
        currentFollower = Instantiate(pathFollower);
        _pathFollower = currentFollower.GetComponent<PathFollower>();
        _pathFollower.pathCreator = currentPath;
        _pathFollower.playerOffset = colliderPoint;
        zAngle = transform.localEulerAngles.z;
    }
    private void MoveAlongTunnel()
    {
        // handle tunnel movement and rotation
        float speed = snapSpeed * Time.fixedDeltaTime;
        transform.position = Vector3.Slerp(transform.position, currentFollower.transform.position, speed);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentFollower.transform.rotation, speed);

        // if path follower has stopped moving, exit tunnel
        if (storedFollowerPos == _pathFollower.pathFollowerPos && insideTunnel)
        {
            StartCoroutine(ExitTunnel());
            insideTunnel = false;
        }
        storedFollowerPos = _pathFollower.pathFollowerPos;
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
