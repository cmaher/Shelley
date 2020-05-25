using System.Collections.Generic;
using Maru.MCore;
using UnityEngine;

namespace Shelley.Scripts.ShelleyStudio.UI {
    [DisallowMultipleComponent]
    public class PagingCollectionView : VentBehavior {
        public int itemsPerPage; // Assumed to have some type of layout
        public int currentPage = 0;
        public List<GameObject> items; // Assumes ownership of items
        public string ventKey; // used to filter events

        private List<List<GameObject>> pagedItems;
        private int lastPage;

        protected override int EventCapacity => 2;

        protected override void Awake() {
            base.Awake();
            On<SetItemsEvent>(ventKey, SetItems);
            On<PaginationEvent>(ventKey, Paginate);
        }

        private void Start() {
            SetItems(new SetItemsEvent {Items = items, Key = ventKey});
            DisplayCurrentPage();
        }

        private void SetItems(SetItemsEvent evt) {
            if (items == evt.Items) {
                return;
            }

            currentPage = 0;
            
            if (items != null) {
                foreach (var item in items) {
                    Destroy(item);
                }
            }

            items = evt.Items;
            pagedItems = MaruUtils.Partition(items, itemsPerPage);
            lastPage = currentPage;

            foreach (var item in evt.Items) {
                item.transform.SetParent(transform, false);
            }

            // disable all items not on the current page
            for (var i = 0; i < pagedItems.Count; i++) {
                if (i != currentPage) {
                    foreach (var item in pagedItems[i]) {
                        item.SetActive(false);
                    }
                }
            }
        }

        /*
         * TODO pagination can be made nicer with UI views and animation
         */
        private void Paginate(PaginationEvent evt) {
            currentPage = lastPage + evt.AdvancePages;
            var maxPage = pagedItems.Count - 1;
            if (currentPage > maxPage) {
                currentPage = maxPage;
            }

            if (currentPage < 0) {
                currentPage = 0;
            }
            
            if (lastPage != currentPage) {
                foreach (var item in pagedItems[lastPage]) {
                    item.SetActive(false);
                }

                DisplayCurrentPage();
                lastPage = currentPage;
            }
        }

        private void DisplayCurrentPage() {
            if (pagedItems != null) {
                foreach (var item in pagedItems[currentPage]) {
                    item.SetActive(true);
                }
            }
        }
    }

    public struct SetItemsEvent : IKeyedEvent {
        public List<GameObject> Items;
        public string Key;

        public string GetEventKey() {
            return Key;
        }
    }

    public struct PaginationEvent : IKeyedEvent {
        public string Key;
        public int AdvancePages;

        public string GetEventKey() {
            return Key;
        }
    }
}
