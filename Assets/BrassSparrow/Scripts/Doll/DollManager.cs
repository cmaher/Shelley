using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Maru.MCore;
using UnityEngine;

namespace BrassSparrow.Scripts.Doll {
    public class DollManager : MonoBehaviour {
        public const string LocatorKey = "BrassSparrow.Scripts.Doll.DollManager";

        public GameObject protoDoll;

        [System.NonSerialized] public DollChoices DollChoices;

        // Messaging
        public struct EvtOnStarted { }

        private static readonly EvtOnStarted OnStarted = new EvtOnStarted();

        private ILocator locator;
        private IMessageBus vent;
        private Transform partsRoot;
        private GameObject rigRoot;
        private Dictionary<string, DollPart> partsDict;
        private Dictionary<string, DollHead> headsDict;
        private Dictionary<string, DollHeadCovering> headCoveringsDict;
        private Animator protoAnimator;

        private void Awake() {
            locator = LocatorProvider.Get();
            locator.Set(LocatorKey, this);
            vent = locator.Get(SceneManager.VentKey) as MessageBus;
            partsDict = new Dictionary<string, DollPart>();
            headsDict = new Dictionary<string, DollHead>();
            headCoveringsDict = new Dictionary<string, DollHeadCovering>();
        }

        private void Start() {
            partsRoot = protoDoll.transform.Find("Modular_Characters");
            rigRoot = protoDoll.transform.Find("Root").gameObject;
            protoAnimator = protoDoll.GetComponent<Animator>();

            DollChoices = new DollChoices {
                Ungendered = BuildUngenderedDollChoices(),
                Male = BuildGenderedDollChoices("Male"),
                Female = BuildGenderedDollChoices("Female")
            };

            vent.Trigger(OnStarted);
        }

        // Sets the new doll objects on the given game object
        public Doll NewDoll(GameObject go) {
            // Copy the animator
            var animator = go.AddComponent<Animator>();
            animator.runtimeAnimatorController = protoAnimator.runtimeAnimatorController;
            animator.avatar = protoAnimator.avatar;
            animator.applyRootMotion = protoAnimator.applyRootMotion;
            animator.updateMode = protoAnimator.updateMode;
            animator.cullingMode = protoAnimator.cullingMode;

            // Copy the rigging
            var rigGo = Instantiate(rigRoot, go.transform, false);

            // Add a new object for the parts to go under (necessary for scaling)
            var parts = new GameObject("Parts");
            parts.transform.localScale = partsRoot.localScale;
            parts.transform.parent = go.transform;

            return new Doll {
                Go = go,
                RigGo = rigGo,
                PartsGo = parts
            };
        }

        // Modifies the passed-in parts with the new instances and sets them on the doll
        public void ResetDoll(Doll doll, DollParts dollParts) {
            // destroy all existing parts
            // not terribly efficient, but should be fine
            if (doll.Parts != null) {
                foreach (var part in doll.Parts) {
                    if (part != null) {
                        Destroy(part.Go);
                    }
                }
            }

            var parent = doll.PartsGo.transform;
            var newParts = dollParts.ShallowClone();
            // Clone the part GameObject so as not to interfere with the referenced part
            foreach (var part in newParts) {
                if (part != null) {
                    var name = part.Go.name;
                    part.Go = Instantiate(part.Go, parent, false);
                    part.Go.name = name;
                }
            }

            doll.Parts = newParts;
        }

        public void ResetDoll(Doll doll, DollConfig config) {
            var parts = new DollParts();
            var partsConfig = config.parts;
            if (partsConfig.head != null) {
                parts.Head = headsDict[partsConfig.head];
            }

            if (partsConfig.eyebrows != null) {
                parts.Eyebrows = partsDict[partsConfig.eyebrows];
            }

            if (partsConfig.facialHair != null) {
                parts.FacialHair = partsDict[partsConfig.facialHair];
            }

            if (partsConfig.torso != null) {
                parts.Torso = partsDict[partsConfig.torso];
            }

            if (partsConfig.armUpperRight != null) {
                parts.ArmUpperRight = partsDict[partsConfig.armUpperRight];
            }

            if (partsConfig.armUpperLeft != null) {
                parts.ArmUpperLeft = partsDict[partsConfig.armUpperLeft];
            }

            if (partsConfig.armLowerRight != null) {
                parts.ArmLowerRight = partsDict[partsConfig.armLowerRight];
            }

            if (partsConfig.armLowerLeft != null) {
                parts.ArmLowerLeft = partsDict[partsConfig.armLowerLeft];
            }

            if (partsConfig.handRight != null) {
                parts.HandRight = partsDict[partsConfig.handRight];
            }

            if (partsConfig.handLeft != null) {
                parts.HandLeft = partsDict[partsConfig.handLeft];
            }

            if (partsConfig.hips != null) {
                parts.Hips = partsDict[partsConfig.hips];
            }

            if (partsConfig.legRight != null) {
                parts.LegRight = partsDict[partsConfig.legRight];
            }

            if (partsConfig.legLeft != null) {
                parts.LegLeft = partsDict[partsConfig.legLeft];
            }

            if (partsConfig.headCovering != null) {
                parts.HeadCovering = headCoveringsDict[partsConfig.headCovering];
            }

            if (partsConfig.hair != null) {
                parts.Hair = partsDict[partsConfig.hair];
            }

            if (partsConfig.headAttachment != null) {
                parts.HeadAttachment = partsDict[partsConfig.headAttachment];
            }

            if (partsConfig.backAttachment != null) {
                parts.BackAttachment = partsDict[partsConfig.backAttachment];
            }

            if (partsConfig.shoulderAttachmentRight != null) {
                parts.ShoulderAttachmentRight = partsDict[partsConfig.shoulderAttachmentRight];
            }

            if (partsConfig.shoulderAttachmentLeft != null) {
                parts.ShoulderAttachmentLeft = partsDict[partsConfig.shoulderAttachmentLeft];
            }

            if (partsConfig.elbowAttachmentRight != null) {
                parts.ElbowAttachmentRight = partsDict[partsConfig.elbowAttachmentRight];
            }

            if (partsConfig.elbowAttachmentLeft != null) {
                parts.ElbowAttachmentLeft = partsDict[partsConfig.elbowAttachmentLeft];
            }

            if (partsConfig.hipsAttachment != null) {
                parts.HipsAttachment = partsDict[partsConfig.hipsAttachment];
            }

            if (partsConfig.kneeAttachmentRight != null) {
                parts.KneeAttachmentRight = partsDict[partsConfig.kneeAttachmentRight];
            }

            if (partsConfig.kneeAttachmentLeft != null) {
                parts.KneeAttachmentLeft = partsDict[partsConfig.kneeAttachmentLeft];
            }

            if (partsConfig.extra != null) {
                parts.Extra = partsDict[partsConfig.extra];
            }
            
            ResetDoll(doll, parts);
        }

        private UngenderedDollChoices BuildUngenderedDollChoices() {
            var path = "All_Gender_Parts";
            var ungenderedroot = partsRoot.Find(path);
            return new UngenderedDollChoices {
                Hair = GetParts("All_01_Hair", ungenderedroot, path),
                HeadCovering = GetHeadCoveringParts(ungenderedroot, path),
                // Currently, only helmet attachments exist, and they look good not on a helmet.
                // Maybe we want an option to constrain this though?
                HeadAttachment = GetParts("All_02_Head_Attachment/Helmet", ungenderedroot, path),
                BackAttachment = GetParts("All_04_Back_Attachment", ungenderedroot, path),
                ShoulderAttachmentRight = GetParts("All_05_Shoulder_Attachment_Right", ungenderedroot, path),
                ShoulderAttachmentLeft = GetParts("All_06_Shoulder_Attachment_Left", ungenderedroot, path),
                ElbowAttachmentRight = GetParts("All_07_Elbow_Attachment_Right", ungenderedroot, path),
                ElbowAttachmentLeft = GetParts("All_08_Elbow_Attachment_Left", ungenderedroot, path),
                HipsAttachment = GetParts("All_09_Hips_Attachment", ungenderedroot, path),
                KneeAttachmentRight =
                    GetParts("All_10_Knee_Attachement_Right", ungenderedroot, path), // sic "Attachement"
                KneeAttachmentLeft =
                    GetParts("All_11_Knee_Attachement_Left", ungenderedroot, path), // sic "Attachement"
                Extra = GetParts("All_12_Extra/Elf_Ear", ungenderedroot, path),
            };
        }

        private GenderedDollChoices BuildGenderedDollChoices(string genderPrefix) {
            var path = $"{genderPrefix}_Parts";
            var genderRoot = partsRoot.Find(path);
            return new GenderedDollChoices {
                Head = GetHeadParts(genderRoot, path, genderPrefix),
                Eyebrows = GetGenderedParts("01_Eyebrows", genderRoot, path, genderPrefix),
                FacialHair = GetGenderedParts("02_FacialHair", genderRoot, path, genderPrefix),
                Torso = GetGenderedParts("03_Torso", genderRoot, path, genderPrefix),
                ArmUpperRight = GetGenderedParts("04_Arm_Upper_Right", genderRoot, path, genderPrefix),
                ArmUpperLeft = GetGenderedParts("05_Arm_Upper_Left", genderRoot, path, genderPrefix),
                ArmLowerRight = GetGenderedParts("06_Arm_Lower_Right", genderRoot, path, genderPrefix),
                ArmLowerLeft = GetGenderedParts("07_Arm_Lower_Left", genderRoot, path, genderPrefix),
                HandRight = GetGenderedParts("08_Hand_Right", genderRoot, path, genderPrefix),
                HandLeft = GetGenderedParts("09_Hand_Left", genderRoot, path, genderPrefix),
                Hips = GetGenderedParts("10_Hips", genderRoot, path, genderPrefix),
                LegRight = GetGenderedParts("11_Leg_Right", genderRoot, path, genderPrefix),
                LegLeft = GetGenderedParts("12_Leg_Left", genderRoot, path, genderPrefix),
            };
        }

        private List<DollPart> GetParts(string branchName, Transform root, string rootPath) {
            var branch = root.Find(branchName);
            var choices = new List<DollPart>(branch.childCount);
            foreach (Transform child in branch) {
                var path = $"{rootPath}/{branchName}/{child.transform.name}";
                choices.Add(new DollPart(child.gameObject, path));
            }

            IndexParts(partsDict, choices);
            return choices;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<DollPart> GetGenderedParts(string branchName, Transform root, string rootPath,
            string genderPrefix) {
            return GetParts($"{genderPrefix}_{branchName}", root, rootPath);
        }

        private List<DollHead> GetHeadParts(Transform root, string rootPath, string genderPrefix) {
            var headPath = $"{genderPrefix}_00_Head";
            var allElementsPath = $"{genderPrefix}_Head_All_Elements";
            var noElementsPath = $"{genderPrefix}_Head_No_Elements";

            var headBranch = root.Find(headPath);
            var allElements = headBranch.Find(allElementsPath);
            var noElements = headBranch.Find(noElementsPath);

            var choices = new List<DollHead>(allElements.childCount + noElements.childCount);
            foreach (Transform child in allElements) {
                var childGo = child.gameObject;
                var path = $"{rootPath}/{headPath}/{allElementsPath}/{childGo.name}";
                choices.Add(new DollHead(childGo, path, true));
            }

            foreach (Transform child in noElements) {
                var childGo = child.gameObject;
                var path = $"{rootPath}/{headPath}/{noElementsPath}/{childGo.name}";
                choices.Add(new DollHead(childGo, path, false));
            }

            IndexParts(headsDict, choices);
            return choices;
        }

        private List<DollHeadCovering> GetHeadCoveringParts(Transform root, string rootPath) {
            var headCoveringPath = "All_00_HeadCoverings";
            var allHairPath = "HeadCoverings_Base_Hair";
            var noFacialPath = "HeadCoverings_No_FacialHair";
            var noHairPath = "HeadCoverings_No_Hair";

            var headCoveringRoot = root.Find(headCoveringPath);
            var allHairRoot = headCoveringRoot.Find(allHairPath);
            var noFacialRoot = headCoveringRoot.Find(noFacialPath);
            var noHairRoot = headCoveringRoot.Find(noHairPath);

            var choices =
                new List<DollHeadCovering>(allHairRoot.childCount + noFacialRoot.childCount + noHairRoot.childCount);
            foreach (Transform child in allHairRoot) {
                var go = child.gameObject;
                var path = $"{rootPath}/{headCoveringPath}/{allHairPath}/{go.name}";
                choices.Add(new DollHeadCovering(go, path, true, true));
            }

            foreach (Transform child in noFacialRoot) {
                var go = child.gameObject;
                var path = $"{rootPath}/{headCoveringPath}/{noFacialPath}/{go.name}";
                choices.Add(new DollHeadCovering(go, path, true, false));
            }

            foreach (Transform child in noHairRoot) {
                var go = child.gameObject;
                var path = $"{rootPath}/{headCoveringPath}/{noHairPath}/{go.name}";
                choices.Add(new DollHeadCovering(go, path, false, true));
            }

            IndexParts(headCoveringsDict, choices);
            return choices;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void IndexParts<T>(Dictionary<string, T> dict, IEnumerable<T> parts) where T : DollPart {
            foreach (var part in parts) {
                dict[part.Path] = part;
            }
        }
    }
}
