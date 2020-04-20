using System;
using UnityEngine;

/**
 * The LocatorProvider provides a singleton locator object for IOC across library bounds
 *
 * example
 *
 * var locator = new DictLocator();
 * LocatorProvider.Init(locator);
 * locator.Set("PLAYER_NAME", "Such Name");
 *
 *
 * elsewhere
 *
 * var locator = LocatorProvider.Get()
 * var playerName = locator.Get("PLAYER_NAME");
 */
namespace Maru.MCore
{
    public static class LocatorProvider
    {
        private const string LocatorName = "MARU_CORE_LOCATOR";

        public static void Init(ILocator locator)
        {
            if (GameObject.Find(LocatorName))
            {
                throw new InvalidOperationException("Locator already initialized");
            }
            
            var go = new GameObject(LocatorName);
            var comp = go.AddComponent<LocatorComponent>();
            comp.Locator = locator;
        }
        
        public static ILocator Get()
        {
            return GameObject.Find(LocatorName).GetComponent<LocatorComponent>().Locator;
        }
    }

    internal class LocatorComponent: MonoBehaviour
    {
        public ILocator Locator;
    }
}
