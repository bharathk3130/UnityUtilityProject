using UnityEngine;

namespace Clickbait.Utilities
{
    public class RaycastSensor
    {
        public float CastLength = 1f;
        LayerMask _layerMask = ~0;
        
        Vector3 _origin = Vector3.zero;
        Transform _transform;
        
        public enum CastDirection { Forward, Right, Up, Backward, Left, Down }
        CastDirection _castDirection;
        
        RaycastHit _hitInfo;

        public RaycastSensor(Transform playerTransform)
        {
            _transform = playerTransform;
        }

        public void Cast()
        {
            Vector3 worldOrigin = _transform.TransformPoint(_origin);
            Vector3 worldDirection = GetCastDirection();
            
            Physics.Raycast(worldOrigin, worldDirection, out _hitInfo, CastLength, _layerMask, QueryTriggerInteraction.Ignore);
        }
        
        public bool HasDetectedHit() => _hitInfo.collider != null;
        public float GetDistance() => _hitInfo.distance;
        public Vector3 GetNormal() => _hitInfo.normal;
        
        public Vector3 GetHitPosition() => _hitInfo.point;
        public Collider GetHitCollider() => _hitInfo.collider;
        public Transform GetHitTransform() => _hitInfo.transform;
        
        public void SetCastDirection(CastDirection direction) => _castDirection = direction;
        public void SetCastOrigin(Vector3 pos) => _origin = _transform.InverseTransformPoint(pos);
        public void SetLayerMask(LayerMask mask) => _layerMask = mask;
        public void IgnoreLayerOf(GameObject go) => _layerMask &= ~(1 << go.layer);

        Vector3 GetCastDirection()
        {
            return _castDirection switch
            {
                CastDirection.Forward => _transform.forward,
                CastDirection.Right => _transform.right,
                CastDirection.Up => _transform.up,
                CastDirection.Backward => -_transform.forward,
                CastDirection.Left => -_transform.right,
                CastDirection.Down => -_transform.up,
                _ => Vector3.one
            };
        }
        
        public void DrawDebug()
        {
            if (!HasDetectedHit()) return;

            Debug.DrawRay(_hitInfo.point, _hitInfo.normal, Color.red, Time.deltaTime);
            float markerSize = 0.2f;
            Debug.DrawLine(_hitInfo.point + Vector3.up * markerSize, _hitInfo.point - Vector3.up * markerSize, Color.green, Time.deltaTime);
            Debug.DrawLine(_hitInfo.point + Vector3.right * markerSize, _hitInfo.point - Vector3.right * markerSize, Color.green, Time.deltaTime);
            Debug.DrawLine(_hitInfo.point + Vector3.forward * markerSize, _hitInfo.point - Vector3.forward * markerSize, Color.green, Time.deltaTime);
        }
    }
}