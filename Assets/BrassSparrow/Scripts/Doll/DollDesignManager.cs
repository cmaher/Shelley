using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using BrassSparrow.Scripts.Events;
using BrassSparrow.Scripts.UI;
using BrassSparrow.Scripts.UI.ColorPicker;
using Doozy.Engine.UI;
using Maru.MCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions.ColorPicker;
using SysRandom = System.Random;

namespace BrassSparrow.Scripts.Doll {
    public class DollDesignManager : VentBehavior {
        private const string StaticPartsDir = "PolygonFantasyHeroCharacters/Prefabs" +
                                              "/Characters_ModularParts_Static";

        private static readonly string DefaultDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "BrassSparrow"
        );

        private static readonly string DefaultFilename = "doll.ubin";

        public const string NavToPartTypeMenu = "NavTo:PartTypeMenu";
        public const string NavToPartMenu = "NavTo:PartMenu";
        public const string NavToColorTypeMenu = "NavTo:ColorTypeMenu";
        public const string NavToFileMenu = "NavTo:FileMenu";
        public const string ColorApplyToAll = "Color:ApplyToAll";
        public const string GenderToggle = "Gender:Toggle";
        public const string FileSave = "File:Save";
        public const string FileLoad = "File:Load";

        public string channelKey = "DollDesignManager";
        public GameObject masterCanvas;
        public GameObject protoDoll;
        public GameObject dollContainer;
        public GameObject fileMenu;
        public TMP_InputField directoryInput;
        public TMP_InputField filenameInput;

        [Header("Part")] public GameObject protoPartSelector;
        public string partSelectionKey = "DollPartSelection";
        public GameObject partSelectionMenu;
        public UIButton toggleGender;

        [Header("Part Type")] public GameObject protoPartTypeSelector;
        public string partTypeSelectionKey = "DollPartTypeSelection";
        public GameObject partTypeSelectionMenu;

        [Header("Color")] public ColorSwatchUpdater protoColorSwatchUpdater;
        public string colorTypeSelectionKey = "DollColorSelection";
        public EnumComboSlider protoRangeSelector;
        public string rangeSelectionKey = "DollRangeSelection";
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

        private Dictionary<DollColorType, ColorSwatchUpdater> colorButtons;
        private Dictionary<DollRangeType, EnumComboSlider> rangeSliders;

        protected override int EventCapacity => 13;

        private GenderedDollChoices genderedParts;

        protected override void Awake() {
            base.Awake();
            random = Locator.Get(SceneManager.RandomKey) as SysRandom;
            On<EnumSelectedEvent<DollPartType>>(channelKey, PartTypeSelected);
            On<EnumSelectedEvent<DollColorType>>(channelKey, ColorTypeSelected);
            On<EnumDataChangedEvent<DollRangeType, float>>(channelKey, ShaderRangeChanged);
            On<PartSelectedEvent>(PartSelected);
            On<ColorPickerChangedEvent>(channelKey, ColorChanged);
            On<UIComponentEvent>($"{channelKey}:{NavToFileMenu}", ShowFileMenu);
            On<UIComponentEvent>($"{channelKey}:{NavToPartTypeMenu}", ShowPartTypeMenu);
            On<UIComponentEvent>($"{channelKey}:{NavToPartMenu}", ShowPartMenu);
            On<UIComponentEvent>($"{channelKey}:{NavToColorTypeMenu}", ShowColorTypeMenu);
            On<UIComponentEvent>($"{channelKey}:{ColorApplyToAll}", ApplyColorToAllParts);
            On<UIComponentEvent>($"{channelKey}:{GenderToggle}", DoToggleGender);
            On<UIComponentEvent>($"{channelKey}:{FileSave}", SaveDoll);
            On<UIComponentEvent>($"{channelKey}:{FileLoad}", LoadDoll);
        }

        private void Start() {
            menus = new List<GameObject> {
                fileMenu,
                partTypeSelectionMenu,
                partSelectionMenu,
                colorTypeSelectionMenu,
                colorPickerMenu,
            };
            dollGo = Instantiate(protoDoll, dollContainer.transform);
            doll = new WorkingDoll(dollGo, shader);
            genderedParts = RandomGender();
            staticParts = LoadStaticParts();
            RandomizeDoll();

            // Show and build the part type selection menu
            ShowFileMenu();
            directoryInput.text = DefaultDirectory;
            filenameInput.text = DefaultFilename;
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

        private string filePath() {
            return Path.Combine(directoryInput.text, filenameInput.text);
        }

        private void SaveDoll(UIComponentEvent _) {
            var bf = new BinaryFormatter();
            var path = filePath();
            var dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            var file = File.Create(path);
            bf.Serialize(file, doll.ToConfig());
            file.Close();
        }

        private void LoadDoll(UIComponentEvent _) {
            var bf = new BinaryFormatter();
            var file = File.Open(filePath(), FileMode.Open);
            var config = bf.Deserialize(file) as DollConfig;
            doll.SetFromConfig(config);
        }

        private void RandomizeDoll() {
            doll.SetPart(DollPartType.Head, RandomPart(RandomGender().Head));
            doll.SetPart(DollPartType.Eyebrows, RandomPart(RandomGender().Eyebrows));
            doll.SetPart(DollPartType.FacialHair, RandomPart(RandomGender().FacialHair));
            doll.SetPart(DollPartType.Torso, RandomPart(RandomGender().Torso));
            doll.SetPart(DollPartType.ArmUpperRight, RandomPart(RandomGender().ArmUpperRight));
            doll.SetPart(DollPartType.ArmUpperLeft, RandomPart(RandomGender().ArmUpperLeft));
            doll.SetPart(DollPartType.ArmLowerRight, RandomPart(RandomGender().ArmLowerRight));
            doll.SetPart(DollPartType.ArmLowerLeft, RandomPart(RandomGender().ArmLowerLeft));
            doll.SetPart(DollPartType.HandRight, RandomPart(RandomGender().HandRight));
            doll.SetPart(DollPartType.HandLeft, RandomPart(RandomGender().HandLeft));
            doll.SetPart(DollPartType.Hips, RandomPart(RandomGender().Hips));
            doll.SetPart(DollPartType.LegRight, RandomPart(RandomGender().LegRight));
            doll.SetPart(DollPartType.LegLeft, RandomPart(RandomGender().LegLeft));
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
            var namesToParts = doll.AllPartsByName();
            var dict = new Dictionary<string, DollPart>();
            var prefabs = Resources.LoadAll<GameObject>(StaticPartsDir);
            foreach (var p in prefabs) {
                var partName = p.name.Replace("_Static", "");
                var part = namesToParts[partName];
                p.GetComponent<Renderer>().material = doll.GetMaterial(part.Type);
                dict[part.Path] = new DollPart(p, part.Path, part.Type);
            }

            return dict;
        }

        private List<GameObject> BuildTypeButtons<T>(GameObject proto) where T : Enum {
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
            ShowPartMenu();
        }

        private List<DollPart> GetDollChoices(DollPartType type) {
            List<DollPart> choices;
            if (DollPartTypes.IsGendered(type)) {
                choices = genderedParts.Get(type);
            } else {
                choices = doll.Choices.Ungendered.Get(type);
            }

            return choices;
        }

        private void PartSelected(PartSelectedEvent evt) {
            var part = evt.PartSelector.DollPart;
            doll.SetPart(part.Type, part.Path);
        }

        private void ColorTypeSelected(EnumSelectedEvent<DollColorType> evt) {
            ShowMenu(colorPickerMenu);
            selecteDollColorType = evt.Type;
            colorPicker.CurrentColor = doll.GetColor(selectedPartType, selecteDollColorType);
        }

        private void ColorChanged(ColorPickerChangedEvent evt) {
            doll.SetColor(selectedPartType, selecteDollColorType, evt.Color);
        }

        private void ShaderRangeChanged(EnumDataChangedEvent<DollRangeType, float> evt) {
            doll.SetRangeFloat(selectedPartType, evt.Type, evt.Value);
        }

        private void ApplyColorToAllParts(UIComponentEvent _) {
            foreach (DollPartType partType in Enum.GetValues(typeof(DollPartType))) {
                doll.SetColor(partType, selecteDollColorType, colorPicker.CurrentColor);
            }
        }

        private void DoToggleGender(UIComponentEvent evt) {
            if (genderedParts == doll.Choices.Male) {
                genderedParts = doll.Choices.Female;
            } else {
                genderedParts = doll.Choices.Male;
            }

            ShowPartMenu();
        }

        private void ShowMenu(GameObject menu) {
            foreach (var m in menus) {
                if (m != menu) {
                    m.SetActive(false);
                }
            }

            menu.SetActive(true);
        }

        private void ShowFileMenu(UIComponentEvent _ = new UIComponentEvent()) {
            ShowMenu(fileMenu);
        }

        private void ShowPartTypeMenu(UIComponentEvent _) {
            ShowMenu(partTypeSelectionMenu);
            if (!partTypeMenuCreated) {
                var partTypeOptions = BuildTypeButtons<DollPartType>(protoPartTypeSelector);
                Vent.Trigger(new SetItemsEvent {Items = partTypeOptions, Key = partTypeSelectionKey});
                partTypeMenuCreated = true;
            }
        }

        private void ShowPartMenu(UIComponentEvent _ = new UIComponentEvent()) {
            ShowMenu(partSelectionMenu);
            var choices = GetDollChoices(selectedPartType);
            DisplayDollChoices(choices);
            toggleGender.gameObject.SetActive(DollPartTypes.IsGendered(selectedPartType));
            if (toggleGender.gameObject.activeSelf) {
                if (genderedParts == doll.Choices.Male) {
                    toggleGender.SetLabelText("Male\n(Toggle)");
                } else {
                    toggleGender.SetLabelText("Female\n(Toggle)");
                }
            }
        }

        private void ShowColorTypeMenu(UIComponentEvent _) {
            ShowMenu(colorTypeSelectionMenu);
            var colorTypes = Enum.GetValues(typeof(DollColorType));
            var rangeTypes = Enum.GetValues(typeof(DollRangeType));
            var itemsLength = colorTypes.Length + rangeTypes.Length;
            if (!colorTypeMenuCreated) {
                colorButtons = new Dictionary<DollColorType, ColorSwatchUpdater>();
                rangeSliders = new Dictionary<DollRangeType, EnumComboSlider>();

                var colorOptions = BuildTypeButtons<DollColorType>(protoColorSwatchUpdater.gameObject);
                var items = new List<GameObject>(itemsLength);

                // Set initial color swatch color and configure vent
                // relies on the fact that the buttons were created in enum order
                for (var i = 0; i < colorTypes.Length; i++) {
                    var go = colorOptions[i];
                    var colorType = (DollColorType) (object) (i);
                    items.Add(go);
                    var selector = go.GetComponent<ColorSwatchUpdater>();
                    selector.SetColor(doll.GetColor(selectedPartType, colorType));
                    colorButtons[colorType] = selector;
                }

                for (var i = 0; i < rangeTypes.Length; i++) {
                    var rangeType = (DollRangeType) (object) i;
                    var go = Instantiate(protoRangeSelector.gameObject);
                    items.Add(go);
                    var slider = go.GetComponent<EnumComboSlider>();
                    slider.label.text = Enum.GetName(typeof(DollRangeType), rangeType);
                    slider.ValueTrigger = new EnumDataTrigger<DollRangeType, float>(rangeType, channelKey);
                    slider.SetValue(doll.GetRangeFloat(selectedPartType, rangeType));

                    rangeSliders[rangeType] = slider;
                }

                Vent.Trigger(new SetItemsEvent {Items = items, Key = colorTypeSelectionKey});
                colorTypeMenuCreated = true;
            } else {
                foreach (DollColorType type in colorTypes) {
                    var color = doll.GetColor(selectedPartType, type);
                    colorButtons[type].SetColor(color);
                }

                foreach (DollRangeType type in rangeTypes) {
                    var value = doll.GetRangeFloat(selectedPartType, type);
                    rangeSliders[type].SetValue(value);
                }
            }
        }
    }
}
