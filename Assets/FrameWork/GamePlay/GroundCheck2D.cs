using UnityEngine;
using Utility.Attribute;

public class GroundCheck2D : MonoBehaviour
{
    [CustomHeader("Gizmos相关", ColorConst.Cyan, alignment : TextAnchor.MiddleLeft)]
    [SerializeField] private bool _drawGizmos = true;
    [SerializeField] private Color _gizmosColor = Color.red;
    
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private LayerMask _checkLayerMask;
    [SerializeField] private float _checkRadius;
    
    private void OnDrawGizmos()
    {
        if (_drawGizmos && _checkPoint)
        {
            Gizmos.color = _gizmosColor;
            Gizmos.DrawWireSphere(_checkPoint.position, _checkRadius);    
        }
    }

    public bool IsGround() => Physics2D.OverlapCircle(_checkPoint.position, _checkRadius, _checkLayerMask);
}
