using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using BrassSparrow.Scripts.Core;
using BrassSparrow.Scripts.UI;
using BrassSparrow.Scripts.UI.ColorPicker;
using UnityEngine;
using SysRandom = System.Random;

namespace BrassSparrow.Scripts.Doll {
    public class DollDesignManager : BrassSparrowBehavior {
        private const string StaticPartsDir = "PolygonFantasyHeroCharacters/Prefabs" +
                                              "/Characters_ModularParts_Static";

        public string listenKey;
        public GameObject masterCanvas;
        public GameObject protoDoll;
        public GameObject dollContainer;

        public GameObject protoPartSelector;
        public string partSelectionKey = "DollPartSelection";
        public GameObject partSelectionMenu;

        public GameObject protoPartTypeSelector;
        public string partTypeSelectionKey = "DollPartTypeSelection";
        public GameObject partTypeSelectionMenu;

        public string backEventKey = "DollDesignManager:Back";
        
        public Shader shader;

        private Transform partsRoot;
        private SysRandom random;
        private GameObject dollGo;
        private WorkingDoll doll;
        private Dictionary<string, DollPart> staticParts;
        private Dictionary<DollPartType, Material> materials;

        protected override int EventCapacity => 4;

        protected override void Awake() {
            base.Awake();
            random = Locator.Get(SceneManager.RandomKey) as SysRandom;
            On<PartTypeSelectedEvent>(PartTypeSelected);
            On<PartSelectedEvent>(PartSelected);
            On<UIComponentEvent>(backEventKey, ShowPartTypes);
            On<ColorPickerChangedEvent>(listenKey, ColorChanged);
        }

        private void Start() {
            dollGo = Instantiate(protoDoll, dollContainer.transform);
            doll = new WorkingDoll(dollGo, shader);
            staticParts = LoadStaticParts();

            var dollParts = RandomDoll();
            doll.SetConfig(dollParts);
            SetTypeOptions();

            // SaveDoll();
            // LoadDoll();
        }

        private void DisplayChoices<T>(IReadOnlyCollection<T> choices) where T : DollPart {
            var choiceGos = new List<GameObject>(choices.Count);
            foreach (var choice in choices) {
                var selectorUI = Instantiate(protoPartSelector);
                var selector = selectorUI.GetComponent<PartSelector>();
                selector.masterCanvas = masterCanvas;
                var part = staticParts[choice.Path];
                selector.SetDollPart(part);
                choiceGos.Add(selectorUI);
            }

            Vent.Trigger(new SetItemsEvent {Items = choiceGos, Key = partSelectionKey});
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
                p.GetComponent<Renderer>().material = doll.Materials[part.Type];
                dict[part.Path] = new DollPart(p, part.Path, part.Type);
            }

            return dict;
        }

        private void SetTypeOptions() {
            var capacity = Enum.GetNames(typeof(DollPartType)).Length;
            var options = new List<GameObject>(capacity);
            for (var i = 0; i < capacity; i++) {
                GameObject go = Instantiate(protoPartTypeSelector);
                var comp = go.GetComponent<PartTypeSelector>();
                comp.type = (DollPartType) i;
                comp.label = Enum.GetName(typeof(DollPartType), comp.type);
                options.Add(go);
            }

            Vent.Trigger(new SetItemsEvent {Items = options, Key = partTypeSelectionKey});
        }

        private void PartTypeSelected(PartTypeSelectedEvent evt) {
            List<DollPart> choices;
            if (DollPartTypes.IsGendered(evt.Type)) {
                // TODO set this from UI
                var genderedParts = doll.Choices.Male;
                choices = genderedParts.Get(evt.Type);
            } else {
                choices = doll.Choices.Ungendered.Get(evt.Type);
            }

            partTypeSelectionMenu.SetActive(false);
            partSelectionMenu.SetActive(true);
            DisplayChoices(choices);
        }

        private void PartSelected(PartSelectedEvent evt) {
            var part = evt.PartSelector.DollPart;
            var newConfig = doll.Config.Clone();
            newConfig.parts.Set(part.Type, part.Path);
            doll.SetConfig(newConfig);
        }

        private void ColorChanged(ColorPickerChangedEvent evt) {
                doll?.Materials[DollPartType.Head]?.SetColor("_Color_Skin", evt.Color);
        }

        private void ShowPartTypes(UIComponentEvent _) {
            partSelectionMenu.SetActive(false);
            partTypeSelectionMenu.SetActive(true);
        }
    }
}
