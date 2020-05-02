using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using BrassSparrow.Scripts.UI;
using Doozy.Engine;
using Maru.MCore;
using UnityEngine;
using SysRandom = System.Random;

namespace BrassSparrow.Scripts.Doll {
    public class DollDesignManager : MonoBehaviour {
        public const string LocatorKey = "BrassSparrow.Scripts.Doll.DollDesignManager";

        private const string StaticPartsDir = "PolygonFantasyHeroCharacters/Prefabs" +
                                              "/Characters_ModularParts_Static";

        public GameObject protoDoll;
        public GameObject dollContainer;
        public GameObject masterCanvas;
        public string choiceContainerKey;
        public GameObject partSelector;

        private ILocator locator;
        private MessageBus vent;
        private Transform partsRoot;
        private DollChoices dollChoices;
        private SysRandom random;
        private GameObject dollGo;
        private WorkingDoll doll;
        private Dictionary<string, DollPart> staticParts;

        private void Awake() {
            locator = LocatorProvider.Get();
            locator.Set(LocatorKey, this);
            vent = locator.Get(SceneManager.VentKey) as MessageBus;
            random = locator.Get(SceneManager.RandomKey) as SysRandom;
            vent.On<PartSelectorClickEvent>(PartSelected);
        }

        private void Start() {
            dollGo = Instantiate(protoDoll, dollContainer.transform);
            doll = new WorkingDoll(dollGo);
            staticParts = LoadStaticParts();

            var dollParts = RandomDoll();
            doll.SetConfig(dollParts);
            DisplayChoices(doll.Choices.Male.Head);
            // SaveDoll();
            // LoadDoll();
        }

        private void DisplayChoices<T>(IReadOnlyCollection<T> choices) where T : DollPart {
            var choiceGos = new List<GameObject>(choices.Count);
            foreach (var choice in choices) {
                var selectorUI = Instantiate(partSelector);
                var selector = selectorUI.GetComponent<PartSelector>();
                selector.masterCanvas = masterCanvas;
                var part = staticParts[choice.Path];
                selector.SetDollPart(part);
                choiceGos.Add(selectorUI);
            }

            vent.Trigger(new SetItemsEvent {Items = choiceGos, Key = choiceContainerKey});
        }

        private void SaveDoll() {
            var bf = new BinaryFormatter();
            var file = File.Create("DevData/doll.bin");
            bf.Serialize(file, doll.Config);
            file.Close();
        }

        private void LoadDoll() {
            var bf = new BinaryFormatter();
            var file = File.Open("DevData/doll.bin", FileMode.Open);
            var config = bf.Deserialize(file) as DollConfig;
            doll.SetConfig(config);
        }

        private DollConfig RandomDoll() {
            var parts = new DollPartsConfig {
                head = RandomPart(RandomGender().Head),
                eyebrows = RandomPart(RandomGender().Eyebrows),
                facialHair = RandomPart(RandomGender().FacialHair),
                torso = RandomPart(RandomGender().Torso),
                armUpperRight = RandomPart(RandomGender().ArmUpperRight),
                armUpperLeft = RandomPart(RandomGender().ArmUpperLeft),
                armLowerRight = RandomPart(RandomGender().ArmLowerRight),
                armLowerLeft = RandomPart(RandomGender().ArmLowerLeft),
                handRight = RandomPart(RandomGender().HandRight),
                handLeft = RandomPart(RandomGender().HandLeft),
                hips = RandomPart(RandomGender().Hips),
                legRight = RandomPart(RandomGender().LegRight),
                legLeft = RandomPart(RandomGender().LegLeft),
            };
            return new DollConfig {parts = parts};
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private GenderedDollChoices RandomGender() {
            return random.Next(2) == 0 ? doll.Choices.Male : doll.Choices.Female;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string RandomPart(IReadOnlyList<DollPart> choices) {
            if (choices.Count == 0) {
                return default;
            }

            return choices[random.Next(choices.Count)].Path;
        }

        private Dictionary<string, DollPart> LoadStaticParts() {
            var namesToParts = new Dictionary<string, DollPart>();
            foreach (var part in doll.PartsDict.Values) {
                namesToParts[part.Go.name] = part;
            }

            var dict = new Dictionary<string, DollPart>();
            var prefabs = Resources.LoadAll<GameObject>(StaticPartsDir);
            foreach (var p in prefabs) {
                var partName = p.name.Replace("_Static", "");
                var part = namesToParts[partName];
                dict[part.Path] = new DollPart(p, part.Path, part.Type);
            }

            return dict;
        }

        private void PartSelected(PartSelectorClickEvent evt) {
            var part = evt.PartSelector.DollPart;
            var newConfig = doll.Config.Clone();
            var partConfig = newConfig.parts;

            switch (part.Type) {
                case DollPartType.Head:
                    partConfig.head = part.Path;
                    break;
                case DollPartType.Eyebrows:
                    partConfig.eyebrows = part.Path;
                    break;
                case DollPartType.FacialHair:
                    partConfig.facialHair = part.Path;
                    break;
                case DollPartType.Torso:
                    partConfig.torso = part.Path;
                    break;
                case DollPartType.ArmUpperRight:
                    partConfig.armUpperRight = part.Path;
                    break;
                case DollPartType.ArmUpperLeft:
                    partConfig.armUpperLeft = part.Path;
                    break;
                case DollPartType.ArmLowerRight:
                    partConfig.armLowerRight = part.Path;
                    break;
                case DollPartType.ArmLowerLeft:
                    partConfig.armLowerLeft = part.Path;
                    break;
                case DollPartType.HandRight:
                    partConfig.handRight = part.Path;
                    break;
                case DollPartType.HandLeft:
                    partConfig.handLeft = part.Path;
                    break;
                case DollPartType.Hips:
                    partConfig.hips = part.Path;
                    break;
                case DollPartType.LegRight:
                    partConfig.legRight = part.Path;
                    break;
                case DollPartType.LegLeft:
                    partConfig.legLeft = part.Path;
                    break;
                case DollPartType.HeadCovering:
                    partConfig.headCovering = part.Path;
                    break;
                case DollPartType.Hair:
                    partConfig.hair = part.Path;
                    break;
                case DollPartType.HeadAttachment:
                    partConfig.headAttachment = part.Path;
                    break;
                case DollPartType.BackAttachment:
                    partConfig.backAttachment = part.Path;
                    break;
                case DollPartType.ShoulderAttachmentRight:
                    partConfig.shoulderAttachmentRight = part.Path;
                    break;
                case DollPartType.ShoulderAttachmentLeft:
                    partConfig.shoulderAttachmentLeft = part.Path;
                    break;
                case DollPartType.ElbowAttachmentRight:
                    partConfig.elbowAttachmentRight = part.Path;
                    break;
                case DollPartType.ElbowAttachmentLeft:
                    partConfig.elbowAttachmentLeft = part.Path;
                    break;
                case DollPartType.HipsAttachment:
                    partConfig.hipsAttachment = part.Path;
                    break;
                case DollPartType.KneeAttachmentRight:
                    partConfig.kneeAttachmentRight = part.Path;
                    break;
                case DollPartType.KneeAttachmentLeft:
                    partConfig.kneeAttachmentLeft = part.Path;
                    break;
                case DollPartType.Extra:
                    partConfig.extra = part.Path;
                    break;
            }

            doll.SetConfig(newConfig);
        }
    }
}
