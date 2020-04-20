using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using Maru.MCore;
using UnityEngine;
using SysRandom = System.Random;

namespace BrassSparrow.Scripts.Doll {
    public class DollDesignManager : MonoBehaviour {
        public static readonly string LocatorKey = "BrassSparrow.Scripts.Doll.DollDesignManager";

        public GameObject dollContainer;

        private ILocator locator;
        private MessageBus vent;
        private DollManager dollManager;
        private Transform partsRoot;
        private DollChoices dollChoices;
        private SysRandom random;
        private DeferredEvent<DollManager.EvtOnStarted> dollManagerStarted;
        private Doll doll;

        private void Awake() {
            locator = LocatorProvider.Get();
            locator.Set(LocatorKey, this);
            vent = locator.Get(SceneManager.VentKey) as MessageBus;
            random = locator.Get(SceneManager.RandomKey) as SysRandom;
            dollManagerStarted = new DeferredEvent<DollManager.EvtOnStarted>(vent);
        }

        private IEnumerator Start() {
            dollManager = locator.Get(DollManager.LocatorKey) as DollManager;
            
            yield return dollManagerStarted;
            dollChoices = dollManager.DollChoices;
            doll = dollManager.NewDoll(dollContainer);
            var dollParts = RandomDoll();
            dollManager.ResetDoll(doll, dollParts);
            // SaveDoll();
            // LoadDoll();
        }

        private void SaveDoll() {
            var config = doll.ToConfig();
            var bf = new BinaryFormatter();
            var file = File.Create("DevData/doll.bin");
            bf.Serialize(file, config);
            file.Close();
        }

        private void LoadDoll() {
            var bf = new BinaryFormatter();
            var file = File.Open("DevData/doll.bin", FileMode.Open);
            var config = bf.Deserialize(file) as DollConfig;
            dollManager.ResetDoll(doll, config);
        }

        private DollParts RandomDoll() {
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
