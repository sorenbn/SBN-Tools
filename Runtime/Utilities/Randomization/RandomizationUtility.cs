using SBN.Utilities.Randomization.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace SBN.Utilities.Randomization
{
    public static class RandomizationUtility
    {
        /// <summary>
        /// Returns a weighted random item index from the list
        /// </summary>
        public static int GetRandomWeightedIndex(IList<IWeightable> weightables)
        {
            float total = 0.0f;

            for (int i = 0; i < weightables.Count; i++)
                total += weightables[i].Weight;

            float rndWeight = Random.Range(0, total);
            int chosenIndex = 0;

            for (int i = 0; i < weightables.Count; i++)
            {
                if (rndWeight < weightables[i].Weight)
                {
                    chosenIndex = i;
                    break;
                }

                rndWeight -= weightables[i].Weight;
            }

            return chosenIndex;
        }

        /// <summary>
        /// Returns a weighted random item from the list
        /// </summary>
        public static T GetRandomWeightedItem<T>(IList<IWeightable> weightables) where T : IWeightable
        {
            float total = 0.0f;

            for (int i = 0; i < weightables.Count; i++)
                total += weightables[i].Weight;

            float rndWeight = Random.Range(0, total);

            for (int i = 0; i < weightables.Count; i++)
            {
                if (rndWeight < weightables[i].Weight)
                    return (T)weightables[i];

                rndWeight -= weightables[i].Weight;
            }

            return default(T);
        }
    }
}
