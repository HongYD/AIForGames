using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, ISubject
{
    Camera mainCamera;
    private List<GameObject> _observers;

    [SerializeField]
    float radius;

    public float Radius { get { return radius; } }

    [SerializeField]
    bool positionToMouse;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        _observers = new List<GameObject>();
        for (int i = 0; i< players.Length; i++)
        {
            Attach(players[i]);
        }
    }

    void Update()
    {
        if (positionToMouse && Input.GetMouseButtonDown(1))
        {
            SetPositionTowardsMouseCursor();
        }
        else
        {
            transform.position = this.transform.position;
        }
    }

    private void SetPositionTowardsMouseCursor()
    {
        //Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = new Vector3(mousePosition.x, 0, mousePosition.z);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit))
        {
            transform.position = new Vector3(hit.point.x, 1.0f, hit.point.z);
            Notify();
        }
    }

    public void Attach(GameObject observer)
    {
        _observers.Add(observer);
    }

    public void Detach(GameObject observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var o in _observers)
        {
            o.GetComponent<PathFindingMovement>().Receive();
        }
    }
}
