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
namespace Maru.MCore {
    public static class LocatorProvider {
        private static ILocator instance;

        public static void Init(ILocator locator) {
            if (instance != null) {
                throw new InvalidOperationException("Locator already initialized");
            }

            instance = locator;
        }

        public static ILocator Get() {
            return instance;
        }
    }
}
