using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    // Start is called before the first frame update
    private List<Vector3> _posList;
    private int _index;
    public GameObject bullet;
    public Bullet(List<Vector3> posList, Vector3 initPos, GameObject _bullet)
    {
        _posList = new List<Vector3>();
        _posList.AddRange(posList);
        _index = 0;
        bullet = GameObject.Instantiate(_bullet,initPos,Quaternion.identity);
        Debug.Log("enable: " + _index);
    }

    ~Bullet()
    {
        _posList.Clear();
        _index = 0;
        bullet = null;
    }

    public void OnUpdate()
    {
        if( _index < _posList.Count)
        {
            bullet.transform.position = _posList[_index];
            _index++;
        }
        if(_index >= _posList.Count)
        {
            GameObject.Destroy(bullet);
        }
    }
}
