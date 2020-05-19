using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using BrassSparrow.Scripts.Core;
using BrassSparrow.Scripts.UI;
using BrassSparrow.Scripts.UI.ColorPicker;
using Maru.MCore;
using UnityEngine;
using UnityEngine.UI.Extensions.ColorPicker;
using SysRandom = System.Random;

namespace BrassSparrow.Scripts.Doll {
    public class DollDesignManager : BrassSparrowBehavior {
        private const string StaticPartsDir = "PolygonFantasyHeroCharacters/Prefabs" +
                                              "/Characters_ModularParts_Static";

        public const string NavToPartTypeMenu = "NavTo:PartTypeMenu";
        public const string NavToPartMenu = "NavTo:PartMenu";
        public const string NavToColorTypeMenu = "NavTo:ColorTypeMenu";
        public const string ColorApplyToAll = "Color:ApplyToAll";

        public string channelKey = "DollDesignManager";
        public GameObject masterCanvas;
        public GameObject protoDoll;
        public GameObject dollContainer;

        [Header("Part")] public GameObject protoPartSelector;
        public string partSelectionKey = "DollPartSelection";
        public GameObject partSelectionMenu;

        [Header("Part Type")] public GameObject protoPartTypeSelector;
        public string partTypeSelectionKey = "DollPartTypeSelection";
        public GameObject partTypeSelectionMenu;

        [Header("Color")] public ColorTypeSelector protoColorTypeSelector;
        public string colorTypeSelectionKey = "DollColorSelection";
        public GameObject colorTypeSelectionMenu;
        public GameObject colorPickerMenu;
        public ColorPickerControl colorPicker;
        public Shader shader;

        private Transform partsRoot;
        private SysRandom random;
        private GameObject dollGo;
        private WorkingDoll doll;
        private Dictionary<string, DollPart> staticParts;
        private Dictionary<DollPartType, Material> materials;

        private List<GameObject> menus;
        private DollPartType selectedPartType;
        private DollColorType selecteDollColorType;
        private bool partTypeMenuCreated = false;
        private bool colorTypeMenuCreated = false;

        protected override int EventCapacity => 8;

        protected override void Awake() {
            base.Awake();
            random = Locator.Get(SceneManager.RandomKey) as SysRandom;
            On<EnumSelectedEvent<DollPartType>>(channelKey, PartTypeSelected);
            On<EnumSelectedEvent<DollColorType>>(channelKey, ColorTypeSelected);
            On<PartSelectedEvent>(PartSelected);
            On<UIComponentEvent>($"{channelKey}:{NavToPartTypeMenu}", ShowPartTypeMenu);
            On<UIComponentEvent>($"{channelKey}:{NavToPartMenu}", ShowPartMenu);
            On<UIComponentEvent>($"{channelKey}:{NavToColorTypeMenu}", ShowColorTypeMenu);
            On<ColorPickerChangedEvent>(channelKey, ColorChanged);
            On<UIComponentEvent>($"{channelKey}:{ColorApplyToAll}", ApplyColorToAllParts);
        }

        private void Start() {
            menus = new List<GameObject> {
                partTypeSelectionMenu,
                partSelectionMenu,
                colorTypeSelectionMenu,
                colorPickerMenu,
            };
            dollGo = Instantiate(protoDoll, dollContainer.transform);
            doll = new WorkingDoll(dollGo, shader);
            staticParts = LoadStaticParts();

            var dollParts = RandomDoll();
            doll.SetConfig(dollParts);

            // Show and build the part type selection menu
            ShowMenu(partTypeSelectionMenu);
            var partTypeOptions = GetTypeOptions<DollPartType>(protoPartTypeSelector);
            Vent.Trigger(new SetItemsEvent {Items = partTypeOptions, Key = partTypeSelectionKey});

            // SaveDoll();
            // LoadDoll();
        }

        private void DisplayDollChoices<T>(IReadOnlyCollection<T> choices) where T : DollPart {
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

        private List<GameObject> GetTypeOptions<T>(GameObject proto) where T : Enum {
            var capacity = Enum.GetNames(typeof(T)).Length;
            var options = new List<GameObject>(capacity);
            for (var i = 0; i < capacity; i++) {
                var go = Instantiate(proto);
                var comp = go.AddComponent<EnumSelectorButton>();
                var type = (T) (object) i;
                comp.selection = new EnumSelection<T>(type, channelKey);
                // TODO label provider for each enum (maybe just Map<T, String>)
                comp.label = Enum.GetName(typeof(T), type);
                options.Add(go);
            }

            return options;
        }

        private void PartTypeSelected(EnumSelectedEvent<DollPartType> evt) {
            selectedPartType = evt.Type;
            var choices = GetDollChoices(selectedPartType);
            ShowMenu(partSelectionMenu);
            DisplayDollChoices(choices);
        }

        private List<DollPart> GetDollChoices(DollPartType type) {
            List<DollPart> choices;
            if (DollPartTypes.IsGendered(type)) {
                // TODO set this from UI
                var genderedParts = doll.Choices.Male;
                choices = genderedParts.Get(type);
            } else {
                choices = doll.Choices.Ungendered.Get(type);
            }

            return choices;
        }

        private void PartSelected(PartSelectedEvent evt) {
            var part = evt.PartSelector.DollPart;
            var newConfig = doll.Config.Clone();
            newConfig.parts.Set(part.Type, part.Path);
            doll.SetConfig(newConfig);
        }

        private void ColorTypeSelected(EnumSelectedEvent<DollColorType> evt) {
            ShowMenu(colorPickerMenu);
            selecteDollColorType = evt.Type;
            var colorSetting = DollColor.Get(selecteDollColorType);
            colorPicker.CurrentColor = doll.Materials[selectedPartType].GetColor(colorSetting.Id);
        }

        private void ColorChanged(ColorPickerChangedEvent evt) {
            var colorSetting = DollColor.Get(selecteDollColorType);
            doll?.Materials[selectedPartType]?.SetColor(colorSetting.Id, evt.Color);
            Vent.Trigger(new DollColorChangedEvent {
                Key = channelKey,
                PartType = selectedPartType,
                ColorType = selecteDollColorType,
                Color = evt.Color
            });
        }

        private void ApplyColorToAllParts(UIComponentEvent _) {
            var colorSetting = DollColor.Get(selecteDollColorType);
            foreach (DollPartType partType in Enum.GetValues(typeof(DollPartType))) {
                doll.Materials[partType].SetColor(colorSetting.Id, colorPicker.CurrentColor);
            }
        }

        private void ShowMenu(GameObject menu) {
            foreach (var m in menus) {
                if (m != menu) {
                    m.SetActive(false);
                }
            }

            menu.SetActive(true);
        }

        private void ShowPartTypeMenu(UIComponentEvent _) {
            ShowMenu(partTypeSelectionMenu);
            if (!partTypeMenuCreated) {
                var partTypeOptions = GetTypeOptions<DollPartType>(protoPartTypeSelector);
                Vent.Trigger(new SetItemsEvent {Items = partTypeOptions, Key = partTypeSelectionKey});
            }
        }

        private void ShowPartMenu(UIComponentEvent _) {
            ShowMenu(partSelectionMenu);
            var choices = GetDollChoices(selectedPartType);
            DisplayDollChoices(choices);
        }

        private void ShowColorTypeMenu(UIComponentEvent _) {
            ShowMenu(colorTypeSelectionMenu);
            if (!colorTypeMenuCreated) {
                var colorOptions = GetTypeOptions<DollColorType>(protoColorTypeSelector.gameObject);
                var colorTypes = Enum.GetValues(typeof(DollColorType));

                // Set initial color swatch color and configure vent
                // relies on the fact that the buttons were created in enum order
                for (var i = 0; i < colorTypes.Length; i++) {
                    var selector = colorOptions[i].GetComponent<ColorTypeSelector>();
                    selector.listenKey = channelKey;
                    var colorSetting = DollColor.Get((DollColorType) colorTypes.GetValue(i));
                    selector.SetColor(doll.Materials[selectedPartType].GetColor(colorSetting.Id));
                }

                Vent.Trigger(new SetItemsEvent {Items = colorOptions, Key = colorTypeSelectionKey});

                colorTypeMenuCreated = true;
            }
        }
    }

    public struct DollColorChangedEvent : IKeyedEvent {
        public string Key;
        public DollPartType PartType;
        public DollColorType ColorType;
        public Color Color;

        public string GetEventKey() {
            return Key;
        }
    }
}
