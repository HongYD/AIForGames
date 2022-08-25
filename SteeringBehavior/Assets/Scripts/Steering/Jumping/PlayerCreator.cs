using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCreator : MonoBehaviour, Observer, Subject
{
    public bool isPlayerEnmpty;
    public GameObject _playerPrefab;
    private List<GameObject> _cannons;

    private void Start()
    {        
        isPlayerEnmpty = false;
        _cannons = new List<GameObject>();
        GameObject c1 = GameObject.Find("Cannon");
        if (c1 != null)
        {
            Attach(c1);
        }

        GameObject c2 = GameObject.Find("Cannon (1)");
        if (c2 != null)
        {
            Attach(c2);
        }

        GameObject c3 = GameObject.Find("Cannon (2)");
        if (c3 != null)
        {
            Attach(c3);
        }

        GameObject c4 = GameObject.Find("Cannon (3)");
        if (c4 != null)
        {
            Attach(c4);
        }

    }
    public void Receive()
    {
        isPlayerEnmpty = true;
        Notify();
    }

    private void Update()
    {
        if (isPlayerEnmpty)
        {
            GameObject newPlayer =  GameObject.Instantiate(_playerPrefab, new Vector3(this.transform.position.x,2.42f,this.transform.position.z), Quaternion.identity);
            newPlayer.GetComponent<Jump>()._jumpPoint = GameObject.Find("Path/JumpTile").GetComponent<JumpPoint>();
            newPlayer.GetComponent<Jump>().finalTarget = GameObject.Find("Path/Tile (8)").transform;
            isPlayerEnmpty = false;
            Notify(newPlayer);
        }
    }

    public void Attach(GameObject observer)
    {
        _cannons.Add(observer);
    }

    public void Detach(GameObject observer)
    {
        _cannons.Remove(observer);
    }

    public void Notify()
    {
        foreach (var o in _cannons)
        {
            o.GetComponent<Cannon>().Receive();
        }
    }
    public void Notify(GameObject target)
    {
        foreach (var o in _cannons)
        {
            o.GetComponent<Cannon>().Receive(target);
        }
    }
}

