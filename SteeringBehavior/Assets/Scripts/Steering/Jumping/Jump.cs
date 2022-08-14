using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : SteeringBase
{
    private JumpPoint _jumpPoint;
    private bool _canAchieve = false;
    private float _maxSpeed;
    private float _maxYSpeed;
    private Kinematic _target;
    private List<Vector3> _jumpTrajectory;
    public Vector3 gravity = new Vector3(0, -9.8f, 0);

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
        float sqrtTerm = Mathf.Sqrt(2 * gravity.y * _jumpPoint.DeltaPosition.y + _maxYSpeed * _maxYSpeed);
        float time = (_maxYSpeed - sqrtTerm) / gravity.y;
        if (!CheckJumpTime(time))
        {
            time = (_maxYSpeed + sqrtTerm)/gravity.y;
            CheckJumpTime(time);
        }
        return _target;
    }
    private bool CheckJumpTime(float time)
    {
        float vx = _jumpPoint.DeltaPosition.x / time;
        float vz = _jumpPoint.DeltaPosition.z / time;
        float speedSq = vx * vx + vz * vz;

        if(speedSq < _maxSpeed * _maxSpeed)
        {
            _target.velocity.x = vx;
            _target.velocity.z = vz;
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

    }
}
