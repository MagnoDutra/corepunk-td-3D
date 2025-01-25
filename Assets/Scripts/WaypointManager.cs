using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;

    private void Start()
    {

    }

    public Transform[] GetWaypoints() => waypoints;
}
