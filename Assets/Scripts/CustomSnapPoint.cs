using System.Collections.Generic;
using UnityEngine;

namespace CustomSnapTool
{
    public class CustomSnapPoint : MonoBehaviour
    {
        [SerializeField] private float _dotRadius = 0.1f;
        [field: SerializeField] public ConnectionType ConnectionType { get; } = ConnectionType.Default;

        public Dictionary<ConnectionType, Color> ConnectionColors { get; } = new()
        {
            {ConnectionType.Default, Color.green},
            {ConnectionType.Bridge, Color.blue},
            {ConnectionType.Road, Color.yellow}
        };


        private void OnDrawGizmos()
        {
            Gizmos.color = GetPointColor();
            Gizmos.DrawSphere(transform.position, _dotRadius);
        }

        public Color GetPointColor()
        {
            var color = Color.green;
            if (ConnectionColors.ContainsKey(ConnectionType))
                color = ConnectionColors[ConnectionType];
            return color;
        }
    }

    public enum ConnectionType
    {
        Default = 0,
        Road = 1,
        Bridge = 2
    }
}