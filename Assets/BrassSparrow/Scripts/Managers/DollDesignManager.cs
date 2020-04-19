using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BrassSparrow.Scripts.Models;
using UnityEngine;
using Random = System.Random;

namespace BrassSparrow.Scripts.Managers {
    public class DollDesignManager : MonoBehaviour {
        public GameObject doll;

        private Transform partsRoot;

        private DollChoices dollChoices;

        // TODO Share with other objects and save seed? PCG Random?
        private Random random;

        private void Start() {
            random = new Random();
            partsRoot = doll.transform.Find("Modular_Characters");
            dollChoices = new DollChoices {
                Ungendered = BuildUngenderedDollChoices(),
                Male = BuildGenderedDollChoices("Male"),
                Female = BuildGenderedDollChoices("Female")
            };
            
            var dollParts = RandomDollParts();
            ApplyDollParts(dollParts);
        }

        private void ApplyDollParts(DollParts dollParts) {
            DisableAll();
            dollParts.Head.GameObject.SetActive(true);
            dollParts.Eyebrows.SetActive(true);
            if (dollParts.FacialHair != null) {
                dollParts.FacialHair.SetActive(true);
            }
            dollParts.Torso.SetActive(true);
            dollParts.ArmUpperRight.SetActive(true);
            dollParts.ArmUpperLeft.SetActive(true);
            dollParts.ArmLowerRight.SetActive(true);
            dollParts.ArmLowerLeft.SetActive(true);
            dollParts.HandRight.SetActive(true);
            dollParts.HandLeft.SetActive(true);
            dollParts.Hips.SetActive(true);
            dollParts.LegRight.SetActive(true);
            dollParts.LegLeft.SetActive(true);
        }

        private DollParts RandomDollParts() {
            random.Next(10);
            return new DollParts {
                Head = RandomPart(RandomGender().Head),
                Eyebrows = RandomPart(RandomGender().Eyebrows),
                FacialHair = RandomPart(RandomGender().FacialHair),
                Torso = RandomPart(RandomGender().Torso),
                ArmUpperRight = RandomPart(RandomGender().ArmUpperRight),
                ArmUpperLeft = RandomPart(RandomGender().ArmUpperLeft),
                ArmLowerRight = RandomPart(RandomGender().ArmLowerRight),
                ArmLowerLeft = RandomPart(RandomGender().ArmLowerLeft),
                HandRight = RandomPart(RandomGender().HandRight),
                HandLeft = RandomPart(RandomGender().HandLeft),
                Hips = RandomPart(RandomGender().Hips),
                LegRight = RandomPart(RandomGender().LegRight),
                LegLeft = RandomPart(RandomGender().LegLeft),
            };
        }

        private UngenderedDollChoices BuildUngenderedDollChoices() {
            var ungenderedroot = partsRoot.Find("All_Gender_Parts");
            return new UngenderedDollChoices {
                Hair = GetParts("All_01_Hair", ungenderedroot),
                HeadCovering = GetHeadCoveringParts(ungenderedroot),
                // Currently, only helmet attachments exist, and they look good not on a helmet.
                // Maybe we want an option to constrain this though?
                HeadAttachment = GetParts("All_02_Head_Attachment/Helmet", ungenderedroot),
                BackAttachment = GetParts("All_04_Back_Attachment", ungenderedroot),
                ShoulderAttachmentRight = GetParts("All_05_Shoulder_Attachment_Right", ungenderedroot),
                ShoulderAttachmentLeft = GetParts("All_06_Shoulder_Attachment_Left", ungenderedroot),
                ElbowAttachmentRight = GetParts("All_07_Elbow_Attachment_Right", ungenderedroot),
                ElbowAttachmentLeft = GetParts("All_08_Elbow_Attachment_Left", ungenderedroot),
                HipsAttachment = GetParts("All_09_Hips_Attachment", ungenderedroot),
                KneeAttachmentRight = GetParts("All_10_Knee_Attachement_Right", ungenderedroot), // sic "Attachement"
                KneeAttachmentLeft = GetParts("All_11_Knee_Attachement_Left", ungenderedroot), // sic "Attachement"
                Extra = GetParts("All_12_Extra/Elf_Ear", ungenderedroot),
            };
        }

        private GenderedDollChoices BuildGenderedDollChoices(string genderPrefix) {
            var genderRoot = partsRoot.Find($"{genderPrefix}_Parts");
            return new GenderedDollChoices {
                Head = GetHeadParts(genderRoot, genderPrefix),
                Eyebrows = GetGenderedParts("01_Eyebrows", genderRoot, genderPrefix),
                FacialHair = GetGenderedParts("02_FacialHair", genderRoot, genderPrefix),
                Torso = GetGenderedParts("03_Torso", genderRoot, genderPrefix),
                ArmUpperRight = GetGenderedParts("04_Arm_Upper_Right", genderRoot, genderPrefix),
                ArmUpperLeft = GetGenderedParts("05_Arm_Upper_Left", genderRoot, genderPrefix),
                ArmLowerRight = GetGenderedParts("06_Arm_Lower_Right", genderRoot, genderPrefix),
                ArmLowerLeft = GetGenderedParts("07_Arm_Lower_Left", genderRoot, genderPrefix),
                HandRight = GetGenderedParts("08_Hand_Right", genderRoot, genderPrefix),
                HandLeft = GetGenderedParts("09_Hand_Left", genderRoot, genderPrefix),
                Hips = GetGenderedParts("10_Hips", genderRoot, genderPrefix),
                LegRight = GetGenderedParts("11_Leg_Right", genderRoot, genderPrefix),
                LegLeft = GetGenderedParts("12_Leg_Left", genderRoot, genderPrefix),
            };
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

            var choices =
                new List<DollHeadCovering>(allHairRoot.childCount + noFacialRoot.childCount + noHairRoot.childCount);
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

        // Probably more efficient to manually iterate over all the parts, but thia is fast enough right now
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GenderedDollChoices RandomGender() {
            return random.Next(2) == 0 ? dollChoices.Male : dollChoices.Female;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T RandomPart<T>(List<T> choices) {
            if (choices.Count == 0) {
                return default;
            }

            return choices[random.Next(choices.Count)];
        }
    }
}
