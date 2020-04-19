using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BrassSparrow.Scripts.Models;
using UnityEngine;

namespace BrassSparrow.Scripts.Managers {
    public class BrassSparrowManager : MonoBehaviour {
        public GameObject doll;

        private Transform partsRoot;
        private DollChoices dollChoices;

        private void Start() {
            DisableAll();
            partsRoot = doll.transform.Find("Modular_Characters");
            dollChoices = new DollChoices();
            dollChoices.Ungendered = BuildUngenderedDollChoices();
            dollChoices.Male = BuildGenderedDollChoices("Male");
            dollChoices.Female = BuildGenderedDollChoices("Female");
        }

        private UngenderedDollChoices BuildUngenderedDollChoices() {
            var ungenderedroot = partsRoot.Find("All_Gender_Parts");
            var choices = new UngenderedDollChoices();
            
            // TODO head coversings
            choices.Hair = GetParts("All_01_Hair", ungenderedroot);
            // Currently, only helmet attachments exist, and they look good not on a helmet.
            // Maybe we want an option to constrain this though?
            choices.HeadCovering = GetHeadCoveringParts(ungenderedroot);
            choices.HeadAttachment = GetParts("All_02_Head_Attachment/Helmet", ungenderedroot);
            choices.BackAttachment = GetParts("All_04_Back_Attachment", ungenderedroot);
            choices.ShoulderAttachmentRight = GetParts("All_05_Shoulder_Attachment_Right", ungenderedroot);
            choices.ShoulderAttachmentLeft = GetParts("All_06_Shoulder_Attachment_Left", ungenderedroot);
            choices.ElbowAttachmentRight = GetParts("All_07_Elbow_Attachment_Right", ungenderedroot);
            choices.ElbowAttachmentLeft = GetParts("All_08_Elbow_Attachment_Left", ungenderedroot);
            choices.HipsAttachment = GetParts("All_09_Hips_Attachment", ungenderedroot);
            choices.KneeAttachmentRight = GetParts("All_10_Knee_Attachement_Right", ungenderedroot); // sic "Attachement"
            choices.KneeAttachmentLeft = GetParts("All_11_Knee_Attachement_Left", ungenderedroot); // sic "Attachement"
            choices.Extra = GetParts("All_12_Extra/Elf_Ear", ungenderedroot); // only extras are elf ears
            
            return choices;
        }

        private GenderedDollChoices BuildGenderedDollChoices(string genderPrefix) {
            var genderRoot = partsRoot.Find($"{genderPrefix}_Parts");
            var choices = new GenderedDollChoices();
            
            choices.Head = GetHeadParts(genderRoot, genderPrefix);
            choices.Eyebrows = GetGenderedParts("01_Eyebrows", genderRoot, genderPrefix);
            choices.FacialHair = GetGenderedParts("02_FacialHair", genderRoot, genderPrefix);
            choices.Torso = GetGenderedParts("03_Torso", genderRoot, genderPrefix);
            choices.ArmUpperRight = GetGenderedParts("04_Arm_Upper_Right", genderRoot, genderPrefix);
            choices.ArmUpperLeft = GetGenderedParts("05_Arm_Upper_Left", genderRoot, genderPrefix);
            choices.ArmLowerRight = GetGenderedParts("06_Arm_Lower_Right", genderRoot, genderPrefix);
            choices.ArmLowerLeft = GetGenderedParts("07_Arm_Lower_Left", genderRoot, genderPrefix);
            choices.HandRight = GetGenderedParts("08_Hand_Right", genderRoot, genderPrefix);
            choices.HandLeft = GetGenderedParts("09_Hand_Left", genderRoot, genderPrefix);
            choices.Hips = GetGenderedParts("10_Hips", genderRoot, genderPrefix);
            choices.LegRight = GetGenderedParts("11_Leg_Right", genderRoot, genderPrefix);
            choices.LegLeft = GetGenderedParts("12_Leg_Left", genderRoot, genderPrefix);

            return choices;
        }

        private List<GameObject> GetParts(string branchName, Transform root) {
            var branch = root.Find(branchName);
            var choices = new List<GameObject>(branch.childCount);
            foreach (Transform child in branch) {
                choices.Add(child.gameObject);
            }

            return choices;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private List<GameObject> GetGenderedParts(string branchName, Transform root, string genderPrefix) {
            return GetParts($"{genderPrefix}_{branchName}", root);
        }

        private List<DollHead> GetHeadParts(Transform root, string genderPrefix) {
            var headBranch = root.Find($"{genderPrefix}_00_Head");
            var allElements = headBranch.Find($"{genderPrefix}_Head_All_Elements");
            var noElements = headBranch.Find($"{genderPrefix}_Head_No_Elements");
            
            var choices = new List<DollHead>(allElements.childCount + noElements.childCount);
            foreach (Transform child in allElements) {
                choices.Add(new DollHead(child.gameObject, true));
            }
            foreach (Transform child in noElements) {
                choices.Add(new DollHead(child.gameObject, false));
            }

            return choices;
        }

        private List<DollHeadCovering> GetHeadCoveringParts(Transform root) {
            var headCoveringRoot = root.Find("All_00_HeadCoverings");
            var allHairRoot = headCoveringRoot.Find("HeadCoverings_Base_Hair");
            var noFacialRoot = headCoveringRoot.Find("HeadCoverings_No_FacialHair");
            var noHairRoot = headCoveringRoot.Find("HeadCoverings_No_Hair");
            
            var choices = new List<DollHeadCovering>(allHairRoot.childCount + noFacialRoot.childCount + noHairRoot.childCount);
            foreach (Transform child in allHairRoot) {
                choices.Add(new DollHeadCovering(child.gameObject, true, true));
            }
            foreach (Transform child in noFacialRoot) {
                choices.Add(new DollHeadCovering(child.gameObject, true, false));
            }
            foreach (Transform child in noHairRoot) {
                choices.Add(new DollHeadCovering(child.gameObject, false, true));
            }

            return choices;
        }

        // Probably more efficient to manually iterate over all the parts, but this probably won't be a performance issue
        private void DisableAll() {
            var parts = new Stack<GameObject>(80);
            var visited = new HashSet<int>();
            var partsRoot = doll.transform.Find("Modular_Characters").gameObject;
            parts.Push(partsRoot);
            // dfs walk the 
            while (parts.Count > 0) {
                var part = parts.Pop();
                var goId = part.GetInstanceID();
                if (!visited.Contains(goId)) {
                    foreach (Transform child in part.transform) {
                        parts.Push(child.gameObject);
                    }

                    if (part.transform.childCount == 0) {
                        part.SetActive(false);
                    }

                    visited.Add(goId);
                }
            }
        }
    }
}
