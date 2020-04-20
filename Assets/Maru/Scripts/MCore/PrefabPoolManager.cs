using UnityEngine;
using System.Collections.Generic;

namespace Maru.MCore
{
    /// Simple pooling for Unity.
    ///   Author: Martin "quill18" Glaude (quill18@quill18.com)
    ///   Extended: Simon "Draugor" Wagner (https://www.twitter.com/Draugor_/)
    ///   Latest Version: https://gist.github.com/quill18/5a7cfffae68892621267
    ///   License: CC0 (http://creativecommons.org/publicdomain/zero/1.0/)
    ///   UPDATES:
    ///     2018-01-04: - Added Extension Method for Despawn on GameObjects 
    ///                 - Changed the Member Lookup so it doesn't require a PoolMemberComponent anymore.
    ///                     - for that i added a HashSet which contains all PoolMemberIDs  (HashSet has O(1) contains operator)
    ///                     - PoolMemberIDs are just ints from GameObject.getInstanceID() which are unique for the GameObject 
    ///                        over the runtime of the game
    ///                 - Changed PoolDictionary from (Prefab, Pool) to (int, Pool) using Prefab.GetInstanceID
    /// 	2015-04-16: Changed Pool to use a Stack generic.
    /// https://gist.githubusercontent.com/Draugor/00f2a47e5f649945fe4466dea7697024/raw/42292aace55b0b27414d361cbd8e5beacb7348bb/SimplePool.cs
    /// 
    /// Usage:
    /// 
    ///   There's no need to do any special setup of any kind.
    /// 
    ///   Instead of calling Instantiate(), use this:
    ///       SimplePool.Spawn(somePrefab, somePosition, someRotation);
    /// 
    ///   Instead of destroying an object, use this:
    ///       SimplePool.Despawn(myGameObject);
    ///   or this:
    ///       myGameObject.Despawn();
    /// 
    ///   If desired, you can preload the pool with a number of instances:
    ///       SimplePool.Preload(somePrefab, 20);
    /// 
    /// Remember that Awake and Start will only ever be called on the first instantiation
    /// and that member variables won't be reset automatically.  You should reset your
    /// object yourself after calling Spawn().  (i.e. You'll have to do things like set
    /// the object's HPs to max, reset animation states, etc...)
    public class PrefabPoolManager
    {
        // You can avoid resizing of the Stack's internal data by
        // setting this to a number equal to or greater to what you
        // expect most of your pool sizes to be.
        // Note, you can also use Preload() to set the initial size
        // of a pool -- this can be handy if only some of your pools
        // are going to be exceptionally large (for example, your bullets.)
        public const int DEFAULT_POOL_SIZE = 3;

        /// <summary>
        /// The Pool class represents the pool for a particular prefab.
        /// </summary>
        public class PrefabPool
        {
            // We append an id to the name of anything we instantiate.
            // This is purely cosmetic.
            private int _nextId = 1;

            // The structure containing our inactive objects.
            // Why a Stack and not a List? Because we'll never need to
            // pluck an object from the start or middle of the array.
            // We'll always just grab the last one, which eliminates
            // any need to shuffle the objects around in memory.
            private readonly Stack<GameObject> inactive;

            //A Hashset which contains all GetInstanceIDs from the instantiated GameObjects 
            //so we know which GameObject is a member of this pool.
            public readonly HashSet<int> MemberIDs;

            // The prefab that we are pooling
            private readonly GameObject prefab;

            // Constructor
            public PrefabPool(GameObject prefab, int initialQty)
            {
                this.prefab = prefab;
                // If Stack uses a linked list internally, then this
                // whole initialQty thing is a placebo that we could
                // strip out for more minimal code. But it can't *hurt*.
                inactive = new Stack<GameObject>(initialQty);
                MemberIDs = new HashSet<int>();
            }

            // Spawn an object from our pool
            public GameObject Spawn(Vector3 pos, Quaternion rot)
            {
                GameObject obj;
                if (inactive.Count == 0)
                {
                    // We don't have an object in our pool, so we
                    // instantiate a whole new object.
                    obj = GameObject.Instantiate(prefab, pos, rot);
                    obj.name = prefab.name + " (" + (_nextId++) + ")";

                    // Add the unique GameObject ID to our MemberHashset so we know this GO belongs to us.
                    MemberIDs.Add(obj.GetInstanceID());
                }
                else
                {
                    // Grab the last object in the inactive array
                    obj = inactive.Pop();

                    if (obj == null)
                    {
                        // The inactive object we expected to find no longer exists.
                        // The most likely causes are:
                        //   - Someone calling Destroy() on our object
                        //   - A scene change (which will destroy all our objects).
                        //     NOTE: This could be prevented with a DontDestroyOnLoad
                        //	   if you really don't want this.
                        // No worries -- we'll just try the next one in our sequence.

                        return Spawn(pos, rot);
                    }
                }

                obj.transform.position = pos;
                obj.transform.rotation = rot;
                obj.SetActive(true);
                return obj;
            }

            // Return an object to the inactive pool.
            public void Despawn(GameObject obj)
            {
                obj.SetActive(false);

                // Since Stack doesn't have a Capacity member, we can't control
                // the growth factor if it does have to expand an internal array.
                // On the other hand, it might simply be using a linked list 
                // internally.  But then, why does it allow us to specify a size
                // in the constructor? Maybe it's a placebo? Stack is weird.
                inactive.Push(obj);
            }
        }

        // All of our pools
        public static Dictionary<int, PrefabPool> _pools;

        /// <summary>
        /// Initialize our dictionary.
        /// </summary>
        private static void Init(GameObject prefab = null, int qty = DEFAULT_POOL_SIZE)
        {
            if (_pools == null)
                _pools = new Dictionary<int, PrefabPool>();

            if (prefab != null)
            {
                //changed from (prefab, Pool) to (int, Pool) which should be faster if we have 
                //many different prefabs.
                var prefabID = prefab.GetInstanceID();
                if (!_pools.ContainsKey(prefabID))
                    _pools[prefabID] = new PrefabPool(prefab, qty);
            }
        }

        /// <summary>
        /// If you want to preload a few copies of an object at the start
        /// of a scene, you can use this. Really not needed unless you're
        /// going to go from zero instances to 100+ very quickly.
        /// Could technically be optimized more, but in practice the
        /// Spawn/Despawn sequence is going to be pretty darn quick and
        /// this avoids code duplication.
        /// </summary>
        static public void Preload(GameObject prefab, int qty = 1)
        {
            Init(prefab, qty);
            // Make an array to grab the objects we're about to pre-spawn.
            var obs = new GameObject[qty];
            for (int i = 0; i < qty; i++)
                obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);

            // Now despawn them all.
            for (int i = 0; i < qty; i++)
                Despawn(obs[i]);
        }

        /// <summary>
        /// Spawns a copy of the specified prefab (instantiating one if required).
        /// NOTE: Remember that Awake() or Start() will only run on the very first
        /// spawn and that member variables won't get reset.  OnEnable will run
        /// after spawning -- but remember that toggling IsActive will also
        /// call that function.
        /// </summary>
        public static GameObject Spawn(GameObject prefab, Vector3 pos, Quaternion rot)
        {
            Init(prefab);
            return _pools[prefab.GetInstanceID()].Spawn(pos, rot);
        }

        /// <summary>
        /// Spawns a copy of the specified prefab (instantiating one if required).
        /// Under the parent's transform
        /// </summary>
        public static GameObject Spawn(GameObject prefab, GameObject parent)
        {
            Init(prefab);
            var go = _pools[prefab.GetInstanceID()].Spawn(Vector3.zero, Quaternion.identity);
            go.transform.parent = parent.transform;    
            return go;
        }

        /// <summary>
        /// Despawn the specified gameobject back into its pool.
        /// </summary>
        static public void Despawn(GameObject obj)
        {
            PrefabPool p = null;
            foreach (var pool in _pools.Values)
            {
                if (pool.MemberIDs.Contains(obj.GetInstanceID()))
                {
                    p = pool;
                    break;
                }
            }

            if (p == null)
            {
                Debug.Log("Object '" + obj.name + "' wasn't spawned from a pool. Destroying it instead.");
                GameObject.Destroy(obj);
            }
            else
            {
                p.Despawn(obj);
            }
        }
    }

    public static class PrefabPoolGameObjectExtensions
    {
        public static void Despawn(this GameObject go)
        {
            PrefabPoolManager.Despawn(go);
        }
    }
}
