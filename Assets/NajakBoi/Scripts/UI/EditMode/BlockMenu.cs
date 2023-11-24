using NajakBoi.Scripts.Blocks;
using UnityEngine;

namespace NajakBoi.Scripts.UI.EditMode
{
    public class BlockMenu : MonoBehaviour
    {
        public static BlockMenu Instance;
        public Block[] blocks;
        public GameObject blockBtnPrefab;
        public Block blockToPlace;
        public Transform content;

        public static Block BlockBeingEdited;


        private void Awake()
        {
            if (Instance)
            {
                Destroy(Instance.gameObject);
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        // Start is called before the first frame update
        void Start()
        {
            SetUpButtons();
        }

        private void SetUpButtons()
        {
            foreach (var b in blocks)
            {
                var btnGo = Instantiate(blockBtnPrefab, content);
                var blockBtn = btnGo.GetComponent<BlockMenuButton>();
                blockBtn.block = b;
                blockBtn.SetUpButton();
            }
        }
    
        public void Close()
        {
            Destroy(gameObject);
        }
    }
}
