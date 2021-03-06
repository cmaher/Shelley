﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Maru.MCore {
    public static class MaruUtils {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool LayerMaskContains(LayerMask mask, int layer) {
            return mask == (mask | (1 << layer));
        }

        public static List<List<T>> Partition<T>(List<T> coll, int n) {
            var count = coll.Count;
            var ret = new List<List<T>>(count / n);
            List<T> curList = null;

            for (var i = 0; i < count; i++) {
                if (curList == null || curList.Count == n) {
                    var size = Math.Min(n, count - i);
                    curList = new List<T>(size);
                    ret.Add(curList);
                }

                curList.Add(coll[i]);
            }

            return ret;
        }

        public static void BFSTransform(Transform root, Action<Transform> process) {
            var q = new Queue<Transform>();
            var visited = new HashSet<int>();
            q.Enqueue(root);

            while (q.Count > 0) {
                var cur = q.Dequeue();
                var id = cur.GetInstanceID();
                if (!visited.Contains(id)) {
                    process(cur);
                    foreach (Transform child in cur.transform) {
                        q.Enqueue(child);
                    }
                    visited.Add(id);
                }
            }
        }
    }
}
