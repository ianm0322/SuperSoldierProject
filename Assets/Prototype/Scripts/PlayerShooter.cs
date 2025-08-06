using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<Vector3, Vector3, RaycastHit> _onRayHit;

    private class Comparer : IComparer<RaycastHit>
    {
        public int Compare(RaycastHit x, RaycastHit y)
        {
            return x.distance.CompareTo(y.distance);
        }
    }
    public void Shoot()
    {
        int count = Physics.RaycastNonAlloc(_fireTransform.position, _fireTransform.forward, _hitResults, _hitDistance, LayerMask.GetMask(LayerDefines.Platform, LayerDefines.Enemy));

        Vector3 start = _fireTransform.position;
        Vector3 end = _fireTransform.forward * _hitDistance;

        Array.Sort(_hitResults, 0, count, new Comparer());
        for (int i = 0; i < count; i++)
        {
            var hitResult = _hitResults[i];

            _onRayHit?.Invoke(start, end, hitResult);

            if (hitResult.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.OnDamange();
            }

            break;
        }

        Debug.DrawRay(_fireTransform.position, _fireTransform.forward * _hitDistance, Color.red, 3f);
    }

    [SerializeField]
    private Transform _fireTransform;

    [SerializeField]
    private float _hitDistance = 120;

    private RaycastHit[] _hitResults = new RaycastHit[8];
}

public interface IDamageable
{
    void OnDamange();
}