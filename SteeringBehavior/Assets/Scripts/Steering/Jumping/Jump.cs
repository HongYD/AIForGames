using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : SteeringBase
{
    public JumpPoint _jumpPoint;
    private bool _canAchieve = false;
    public float _maxYSpeed = 5.0f;
    private Kinematic _target;
    private List<Vector3> _jumpTrajectory;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);
    private float time;

    private void Start()
    {
        _jumpTrajectory = new List<Vector3>();
    }
    private void Update()
    {
        GetSteeringOutput();
    }

    protected override void GetSteeringOutput()
    {
        _target = CalculateTarget();
        if (!_canAchieve)
        {
            ScheduleJumpAction(_jumpTrajectory);
        }
    }

    private Kinematic CalculateTarget()
    {
        _target = new Kinematic();
        _target.position = _jumpPoint.jumpLocation;
        float sqrtTerm = Mathf.Sqrt(2 * gravity.y * _jumpPoint.deltaPosition.y + _maxYSpeed * _maxYSpeed);
        time = (_maxYSpeed - sqrtTerm) / gravity.y;
        if (!CheckJumpTime(time))
        {
            time = (_maxYSpeed + sqrtTerm)/gravity.y;
            CheckJumpTime(time);
        }
        return _target;
    }
    private bool CheckJumpTime(float time)
    {
        float vx = _jumpPoint.deltaPosition.x / time;
        float vz = _jumpPoint.deltaPosition.z / time;
        float speedSq = vx * vx + vz * vz;

        if(speedSq < maxSpeed * maxSpeed)
        {
            _target.velocity.x = vx;
            _target.velocity.z = vz;
            _target.velocity.y = _maxYSpeed;
            _canAchieve = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ScheduleJumpAction(List<Vector3> jumpTrajectory)
    {
        jumpTrajectory.Clear();
        Vector3 p0 = _jumpPoint.jumpLocation;
        float i = 0;
        while (i < time)
        {
            Vector3 pt = p0 + _target.velocity * i + (gravity * i * i) / 2;
            jumpTrajectory.Add(pt);
            i += Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        if (_jumpTrajectory != null &&_jumpTrajectory.Count > 0)
        {
            for(int i = 0; i < _jumpTrajectory.Count; i++)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(_jumpTrajectory[i], 0.1f);
            }
        }
    }
}
