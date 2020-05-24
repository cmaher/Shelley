using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Maru.Scripts.MSerialize;
using UnityEngine;

namespace BrassSparrow.Scripts.Doll {
    public class WorkingDoll {
        public readonly DollChoices Choices;

        private readonly Transform partsRoot;
        private readonly Dictionary<string, DollPart> AllParts;
        private readonly Dictionary<DollPartType, DollPart> ActiveParts;
        private readonly Dictionary<DollPartType, Material> Materials;

        public WorkingDoll(GameObject modularDoll, Shader shader) {
            ActiveParts = new Dictionary<DollPartType, DollPart>(DollPartTypes.Length);
            foreach (DollPartType partType in DollPartTypes.Values) {
                ActiveParts[partType] = null;
            }

            partsRoot = modularDoll.transform.Find("Modular_Characters");
            AllParts = new Dictionary<string, DollPart>(720); // capacity determined experimentally
            Choices = new DollChoices {
                Ungendered = BuildUngenderedDollChoices(),
                Male = BuildGenderedDollChoices("Male"),
                Female = BuildGenderedDollChoices("Female")
            };

            Materials = new Dictionary<DollPartType, Material>(DollPartTypes.Length);
            foreach (DollPartType partType in DollPartTypes.Values) {
                Materials[partType] = new Material(shader);
                Materials[partType].SetFloat(DollRange.BodyArtAmount.Id, 1.0f);
            }

            // Disable everything, by default
            foreach (var part in AllParts.Values) {
                part.Go.SetActive(false);
                part.Go.GetComponent<Renderer>().material = Materials[part.Type];
            }
        }

        public Material GetMaterial(DollPartType type) {
            return Materials[type];
        }

        public void SetPart(DollPartType partType, string path) {
            ActiveParts[partType]?.Go.SetActive(false);
            if (path != null) {
                var newPart = AllParts[path];
                newPart.Go.SetActive(true);
                ActiveParts[partType] = newPart;
            } else {
                ActiveParts[partType] = null;
            }
        }

        public void MirrorPart(DollPartType srcPartType) {
            if (!DollPartTypes.IsMirrorable(srcPartType)) {
                return;
            }

            var destPartType = DollPartTypes.Mirror(srcPartType);
            var srcPart = ActiveParts[srcPartType];
            var destPath = srcPart != null
                ? ActiveParts[srcPartType].MirrorPartPath()
                : null;
            SetPart(destPartType, destPath);

            foreach (DollColorType colorType in DollColorTypes.Values) {
                SetColor(destPartType, colorType, GetColor(srcPartType, colorType));
            }

            foreach (DollRangeType rangeType in DollRangeTypes.Values) {
                SetRangeFloat(destPartType, rangeType, GetRangeFloat(srcPartType, rangeType));
            }
        }

        public void SetColor(DollPartType partType, DollColorType colorType, Color color) {
            var colorSetting = DollColor.Get(colorType);
            Materials[partType].SetColor(colorSetting.Id, color);
        }

        public Color GetColor(DollPartType partType, DollColorType colorType) {
            var colorSetting = DollColor.Get(colorType);
            return Materials[partType].GetColor(colorSetting.Id);
        }

        public void SetRangeFloat(DollPartType partType, DollRangeType rangeType, float value) {
            var setting = DollRange.Get(rangeType);
            Materials[partType].SetFloat(setting.Id, value);
        }

        public float GetRangeFloat(DollPartType partType, DollRangeType rangeType) {
            var setting = DollRange.Get(rangeType);
            return Materials[partType].GetFloat(setting.Id);
        }

        public DollConfig ToConfig() {
            var activeParts = new Dictionary<DollPartType, string>(DollPartTypes.Length);
            foreach (DollPartType type in DollPartTypes.Values) {
                activeParts[type] = ActiveParts[type]?.Path;
            }

            var shaders = new Dictionary<DollPartType, DollShaderConfig>(DollPartTypes.Length);
            foreach (DollPartType partType in DollPartTypes.Values) {
                var colors = new Dictionary<DollColorType, SerializableColor>(DollColorTypes.Length);
                foreach (DollColorType colorType in DollColorTypes.Values) {
                    var color = GetColor(partType, colorType);
                    colors[colorType] = SerializableColor.FromColor(color);
                }

                var ranges = new Dictionary<DollRangeType, float>(DollRangeTypes.Length);
                foreach (DollRangeType rangeType in DollRangeTypes.Values) {
                    ranges[rangeType] = GetRangeFloat(partType, rangeType);
                }

                shaders[partType] = new DollShaderConfig {colors = colors, ranges = ranges};
            }

            return new DollConfig {
                activeParts = activeParts,
                shaders = shaders,
            };
        }

        public void SetFromConfig(DollConfig config) {
            foreach (var entry in config.activeParts) {
                SetPart(entry.Key, entry.Value);
            }

            foreach (var shader in config.shaders) {
                foreach (var color in shader.Value.colors) {
                    SetColor(shader.Key, color.Key, color.Value.ToColor());
                }

                foreach (var range in shader.Value.ranges) {
                    SetRangeFloat(shader.Key, range.Key, range.Value);
                }
            }
        }

        public Dictionary<string, DollPart> AllPartsByName() {
            var namesToParts = new Dictionary<string, DollPart>(AllParts.Count);
            foreach (var part in AllParts.Values) {
                namesToParts[part.Go.name] = part;
            }

            return namesToParts;
        }

        private UngenderedDollChoices BuildUngenderedDollChoices() {
            var path = "All_Gender_Parts";
            var ungenderedroot = partsRoot.Find(path);
            return new UngenderedDollChoices {
                Hair = GetParts("All_01_Hair", DollPartType.Hair, ungenderedroot, path),
                HeadCovering = GetHeadCoveringParts(ungenderedroot, path),
                // Currently, only helmet attachments exist, and they look good not on a helmet.
                // Maybe we want an option to constrain this though?
                HeadAttachment = GetParts("All_02_Head_Attachment/Helmet", DollPartType.HeadAttachment, ungenderedroot,
                    path),
                BackAttachment = GetParts("All_04_Back_Attachment", DollPartType.BackAttachment, ungenderedroot, path),
                ShoulderAttachmentRight = GetParts("All_05_Shoulder_Attachment_Right",
                    DollPartType.ShoulderAttachmentRight, ungenderedroot, path),
                ShoulderAttachmentLeft = GetParts("All_06_Shoulder_Attachment_Left",
                    DollPartType.ShoulderAttachmentLeft, ungenderedroot, path),
                ElbowAttachmentRight = GetParts("All_07_Elbow_Attachment_Right", DollPartType.ElbowAttachmentRight,
                    ungenderedroot, path),
                ElbowAttachmentLeft = GetParts("All_08_Elbow_Attachment_Left", DollPartType.ElbowAttachmentLeft,
                    ungenderedroot, path),
                HipsAttachment = GetParts("All_09_Hips_Attachment", DollPartType.HipsAttachment, ungenderedroot, path),
                KneeAttachmentRight =
                    GetParts("All_10_Knee_Attachement_Right", DollPartType.KneeAttachmentRight, ungenderedroot,
                        path), // sic "Attachement"
                KneeAttachmentLeft =
                    GetParts("All_11_Knee_Attachement_Left", DollPartType.KneeAttachmentLeft, ungenderedroot,
                        path), // sic "Attachement"
                Extra = GetParts("All_12_Extra/Elf_Ear", DollPartType.Extra, ungenderedroot, path),
            };
        }

        private GenderedDollChoices BuildGenderedDollChoices(string genderPrefix) {
            var path = $"{genderPrefix}_Parts";
            var genderRoot = partsRoot.Find(path);
            return new GenderedDollChoices {
                Head = GetHeadParts(genderRoot, path, genderPrefix),
                Eyebrows = GetGenderedParts("01_Eyebrows", DollPartType.Eyebrows, genderRoot, path, genderPrefix),
                FacialHair = GetGenderedParts("02_FacialHair", DollPartType.FacialHair, genderRoot, path, genderPrefix),
                Torso = GetGenderedParts("03_Torso", DollPartType.Torso, genderRoot, path, genderPrefix),
                ArmUpperRight = GetGenderedParts("04_Arm_Upper_Right", DollPartType.ArmUpperRight, genderRoot, path,
                    genderPrefix),
                ArmUpperLeft = GetGenderedParts("05_Arm_Upper_Left", DollPartType.ArmUpperLeft, genderRoot, path,
                    genderPrefix),
                ArmLowerRight = GetGenderedParts("06_Arm_Lower_Right", DollPartType.ArmLowerRight, genderRoot, path,
                    genderPrefix),
                ArmLowerLeft = GetGenderedParts("07_Arm_Lower_Left", DollPartType.ArmLowerLeft, genderRoot, path,
                    genderPrefix),
                HandRight = GetGenderedParts("08_Hand_Right", DollPartType.HandRight, genderRoot, path, genderPrefix),
                HandLeft = GetGenderedParts("09_Hand_Left", DollPartType.HandLeft, genderRoot, path, genderPrefix),
                Hips = GetGenderedParts("10_Hips", DollPartType.Hips, genderRoot, path, genderPrefix),
                LegRight = GetGenderedParts("11_Leg_Right", DollPartType.LegRight, genderRoot, path, genderPrefix),
                LegLeft = GetGenderedParts("12_Leg_Left", DollPartType.LegLeft, genderRoot, path, genderPrefix),
            };
        }

        private List<DollPart> GetParts(string branchName, DollPartType type, Transform root, string rootPath) {
            var branch = root.Find(branchName);
            var choices = new List<DollPart>(branch.childCount);
            foreach (Transform child in branch) {
                var path = $"{rootPath}/{branchName}/{child.transform.name}";
                choices.Add(new DollPart(child.gameObject, path, type));
            }

            IndexParts(AllParts, choices);
            return choices;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<DollPart> GetGenderedParts(string branchName, DollPartType type,
            Transform root, string rootPath, string genderPrefix) {
            return GetParts($"{genderPrefix}_{branchName}", type, root, rootPath);
        }

        private List<DollPart> GetHeadParts(Transform root, string rootPath, string genderPrefix) {
            var headPath = $"{genderPrefix}_00_Head";
            var allElementsPath = $"{genderPrefix}_Head_All_Elements";
            var noElementsPath = $"{genderPrefix}_Head_No_Elements";

            var headBranch = root.Find(headPath);
            var allElements = headBranch.Find(allElementsPath);
            var noElements = headBranch.Find(noElementsPath);

            var choices = new List<DollPart>(allElements.childCount + noElements.childCount);
            foreach (Transform child in allElements) {
                var childGo = child.gameObject;
                var path = $"{rootPath}/{headPath}/{allElementsPath}/{childGo.name}";
                choices.Add(new DollPart(childGo, path, DollPartType.Head, true));
            }

            foreach (Transform child in noElements) {
                var childGo = child.gameObject;
                var path = $"{rootPath}/{headPath}/{noElementsPath}/{childGo.name}";
                choices.Add(new DollPart(childGo, path, DollPartType.Head, false));
            }

            IndexParts(AllParts, choices);
            return choices;
        }

        private List<DollPart> GetHeadCoveringParts(Transform root, string rootPath) {
            var headCoveringPath = "All_00_HeadCoverings";
            var allHairPath = "HeadCoverings_Base_Hair";
            var noFacialPath = "HeadCoverings_No_FacialHair";
            var noHairPath = "HeadCoverings_No_Hair";

            var headCoveringRoot = root.Find(headCoveringPath);
            var allHairRoot = headCoveringRoot.Find(allHairPath);
            var noFacialRoot = headCoveringRoot.Find(noFacialPath);
            var noHairRoot = headCoveringRoot.Find(noHairPath);

            var choices =
                new List<DollPart>(allHairRoot.childCount + noFacialRoot.childCount + noHairRoot.childCount);
            foreach (Transform child in allHairRoot) {
                var go = child.gameObject;
                var path = $"{rootPath}/{headCoveringPath}/{allHairPath}/{go.name}";
                choices.Add(new DollPart(go, path, DollPartType.HeadCovering, true, true));
            }

            foreach (Transform child in noFacialRoot) {
                var go = child.gameObject;
                var path = $"{rootPath}/{headCoveringPath}/{noFacialPath}/{go.name}";
                choices.Add(new DollPart(go, path, DollPartType.HeadCovering, true, false));
            }

            foreach (Transform child in noHairRoot) {
                var go = child.gameObject;
                var path = $"{rootPath}/{headCoveringPath}/{noHairPath}/{go.name}";
                choices.Add(new DollPart(go, path, DollPartType.HeadCovering, false, true));
            }

            IndexParts(AllParts, choices);
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
