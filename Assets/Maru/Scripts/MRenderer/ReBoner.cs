using System.Collections.Generic;
using Maru.MCore;
using UnityEngine;

namespace Maru.MRenderer {
    public class ReBoner {
        private Dictionary<string, Transform> boneMap;

        public ReBoner(Transform skeleton) {
            boneMap = new Dictionary<string, Transform>();
            MaruUtils.BFSTransform(skeleton, tf => { boneMap[tf.gameObject.name] = tf; });
        }

        public void ReBone(SkinnedMeshRenderer skin) {
            var boneArray = skin.bones;
            for (int idx = 0; idx < boneArray.Length; ++idx) {
                string boneName = boneArray[idx].name;
                boneArray[idx] = boneMap[boneName];
            }

            skin.bones = boneArray;
            skin.rootBone = boneMap[skin.rootBone.name];
        }
    }
}
