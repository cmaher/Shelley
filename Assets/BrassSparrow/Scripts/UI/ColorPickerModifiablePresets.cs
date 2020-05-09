using System;
using System.Collections.Generic;
using Maru.MCore;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions.ColorPicker;

namespace BrassSparrow.Scripts.UI {
    public class ColorPickerModifiablePresets : MonoBehaviour {
        public ColorPickerControl picker;

        [SerializeField] protected GameObject presetPrefab;

        [SerializeField] protected int maxPresets = 16;

        [SerializeField] protected Color[] predefinedPresets;

        public Image createPresetImage;
        public Transform createButton;

        protected Dictionary<int, GameObject> presetButtons;
        protected List<Tuple<int, Color>> presets = new List<Tuple<int, Color>>();
        protected List<Action> off;

        public enum SaveType {
            None,
            PlayerPrefs,
            JsonFile
        }

        [SerializeField] public SaveType saveMode = SaveType.None;

        [SerializeField] protected string playerPrefsKey;

        public virtual string JsonFilePath {
            get { return Application.persistentDataPath + "/" + playerPrefsKey + ".json"; }
        }

        protected virtual void Reset() {
            playerPrefsKey = "colorpicker_" + GetInstanceID().ToString();
        }

        protected virtual void Awake() {
            var vent = LocatorProvider.Get().Get(SceneManager.VentKey) as IMessageBus;
            off = new List<Action>(2);
            var key = GetInstanceID().ToString();
            off.Add(vent.On<SelectColorPickerPresetEvent>(key, PresetSelect));
            off.Add(vent.On<RemoveColorPickerPresetEvent>(key, RemovePreset));
            presetButtons = new Dictionary<int, GameObject>();

            picker.onHSVChanged.AddListener(HSVChanged);
            picker.onValueChanged.AddListener(ColorChanged);
            picker.CurrentColor = Color.white;
            presetPrefab.SetActive(false);

            for (var i = 0; i < predefinedPresets.Length; i++) {
                presets.Add(new Tuple<int, Color>(i, predefinedPresets[i]));
            }

            LoadPresets(saveMode);
        }

        public virtual void CreatePresetButton() {
            CreatePreset(picker.CurrentColor);
        }

        public virtual void LoadPresets(SaveType saveType) {
            string jsonData = "";
            switch (saveType) {
                case SaveType.None:
                    break;
                case SaveType.PlayerPrefs:
                    if (PlayerPrefs.HasKey(playerPrefsKey)) {
                        jsonData = PlayerPrefs.GetString(playerPrefsKey);
                    }

                    break;
                case SaveType.JsonFile:
                    if (System.IO.File.Exists(JsonFilePath)) {
                        jsonData = System.IO.File.ReadAllText(JsonFilePath);
                    }

                    break;
                default:
                    throw new System.NotImplementedException(saveType.ToString());
            }

            if (!string.IsNullOrEmpty(jsonData)) {
                try {
                    var jsonColors = JsonUtility.FromJson<JsonColor>(jsonData);
                    var colors = jsonColors.GetColors();
                    for (var i = 0; i < colors.Length; i++) {
                        presets.Add(new Tuple<int, Color>(i, colors[i]));
                    }
                } catch (System.Exception e) {
                    Debug.LogException(e);
                }
            }

            foreach (var item in presets) {
                CreatePreset(item.Item2, true);
            }
        }

        public virtual void SavePresets(SaveType saveType) {
            if (presets == null || presets.Count <= 0) {
                Debug.LogError(
                    "presets cannot be null or empty: " + (presets == null ? "NULL" : "EMPTY"));
                return;
            }

            var jsonColor = new JsonColor();
            var colors = new Color[presets.Count];
            for (var i = 0; i < presets.Count; i++) {
                colors[i] = presets[i].Item2;
            }

            jsonColor.SetColors(colors);

            string jsonData = JsonUtility.ToJson(jsonColor);

            switch (saveType) {
                case SaveType.None:
                    Debug.LogWarning("Called SavePresets with SaveType = None...");
                    break;
                case SaveType.PlayerPrefs:
                    PlayerPrefs.SetString(playerPrefsKey, jsonData);
                    break;
                case SaveType.JsonFile:
                    System.IO.File.WriteAllText(JsonFilePath, jsonData);
                    //Application.OpenURL(JsonFilePath);
                    break;
                default:
                    throw new System.NotImplementedException(saveType.ToString());
            }
        }

        protected class JsonColor {
            public Color32[] colors;

            public void SetColors(Color[] colorsIn) {
                this.colors = new Color32[colorsIn.Length];
                for (int i = 0; i < colorsIn.Length; i++) {
                    this.colors[i] = colorsIn[i];
                }
            }

            public Color[] GetColors() {
                Color[] colorsOut = new Color[colors.Length];
                for (int i = 0; i < colors.Length; i++) {
                    colorsOut[i] = colors[i];
                }

                return colorsOut;
            }
        }

        public virtual void CreatePreset(Color color, bool loading) {
            var newPresetButton = Instantiate(presetPrefab, presetPrefab.transform.parent);
            newPresetButton.transform.SetAsLastSibling();
            newPresetButton.SetActive(true);
            newPresetButton.GetComponent<Image>().color = color;

            var id = newPresetButton.GetInstanceID();
            var presetController = newPresetButton.AddComponent<ColorPickerPresetRemove>();
            presetController.key = GetInstanceID().ToString();
            presetController.id = id;
            presetController.color = color;

            presetButtons[id] = newPresetButton;

            createPresetImage.color = Color.white;

            if (!loading) {
                presets.Add(new Tuple<int, Color>(id, color));
                SavePresets(saveMode);
            }

            createButton.gameObject.SetActive(presets.Count < maxPresets);
        }

        public virtual void CreatePreset(Color color) {
            CreatePreset(color, false);
        }

        public virtual void PresetSelect(Image sender) {
            picker.CurrentColor = sender.color;
        }
        
        protected virtual void PresetSelect(SelectColorPickerPresetEvent evt) {
            picker.CurrentColor = evt.Color;
        }

        protected virtual void HSVChanged(float h, float s, float v) {
            createPresetImage.color = HSVUtil.ConvertHsvToRgb(h * 360, s, v, 1);
            //Debug.Log("hsv util color: " + createPresetImage.color);
        }

        protected virtual void ColorChanged(Color color) {
            createPresetImage.color = color;
            //Debug.Log("color changed: " + color);
        }

        protected virtual void RemovePreset(RemoveColorPickerPresetEvent evt) {
            var idx = presets.FindIndex(pair => pair.Item1 == evt.Id);
            presets.RemoveAt(idx);
            Destroy(presetButtons[evt.Id]);
            presetButtons.Remove(evt.Id);
            createButton.gameObject.SetActive(presets.Count <= maxPresets);
            SavePresets(saveMode);
        }

        protected virtual void OnDestroy() {
            foreach (var remove in off) {
                remove();
            }
        }
    }
}
