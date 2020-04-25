using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
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

        private ILocator locator;
        private MessageBus vent;
        private Transform partsRoot;
        private DollChoices dollChoices;
        private SysRandom random;
        private GameObject dollGo;
        private WorkingDoll doll;
        private Dictionary<string, GameObject> staticParts;

        private void Awake() {
            locator = LocatorProvider.Get();
            locator.Set(LocatorKey, this);
            vent = locator.Get(SceneManager.VentKey) as MessageBus;
            random = locator.Get(SceneManager.RandomKey) as SysRandom;
            staticParts = LoadStaticParts();
        }

        private void Start() {
            dollGo = Instantiate(protoDoll, dollContainer.transform);
            doll = new WorkingDoll(dollGo);
            var dollParts = RandomDoll();
            doll.SetConfig(dollParts);
            // SaveDoll();
            // LoadDoll();
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

        private Dictionary<string, GameObject> LoadStaticParts() {
            var dict = new Dictionary<string, GameObject>();
            var prefabs = Resources.LoadAll<GameObject>(StaticPartsDir);
            // TODO instantiate these here and assign to some out-of-the-way container, or lazy load on UI?
            foreach (var p in prefabs) {
                dict[p.name] = p;
            }
            return dict;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string StaticPartsName(string partName) {
            return $"{partName}_Sattic";
        }
    }
}
