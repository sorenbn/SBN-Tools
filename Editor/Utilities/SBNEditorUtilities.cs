using UnityEditorInternal;
using UnityEngine;

namespace SBN.EditorTools
{
    public static class SBNEditorUtilities
    {
        public static void MoveComponentToTop(GameObject obj, Component target)
        {
            var componentCount = obj.GetComponents<Component>().Length;
            MoveComponentUp(target, componentCount);
        }

        public static void MoveComponentToBottom(GameObject obj, Component target)
        {
            var componentCount = obj.GetComponents<Component>().Length;
            MoveComponentDown(target, componentCount);
        }

        public static void MoveComponentUp(Component target, int count = 1)
        {
            for (int i = 0; i < count; i++)
                ComponentUtility.MoveComponentUp(target);
        }

        public static void MoveComponentDown(Component target, int count = 1)
        {
            for (int i = 0; i < count; i++)
                ComponentUtility.MoveComponentDown(target);
        }
    }
}