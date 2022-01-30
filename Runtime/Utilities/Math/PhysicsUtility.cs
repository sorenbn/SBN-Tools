using UnityEngine;

namespace SBN.Utilities.Physics
{
    public static class PhysicsUtility
    {
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static bool Contains(this LayerMask mask, GameObject gameObject)
        {
            return mask == (mask | (1 << gameObject.layer));
        }

        public static LayerMask AddLayer(ref this LayerMask mask, int layer)
        {
            mask |= (1 << layer);
            return mask;
        }

        public static LayerMask RemoveLayer(ref this LayerMask mask, int layer)
        {
            mask &= ~(1 << layer);
            return mask;
        }
    }
}
