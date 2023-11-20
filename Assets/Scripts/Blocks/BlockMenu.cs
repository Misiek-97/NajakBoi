using Blocks;
using UnityEngine;
using UnityEngine.Serialization;

public class BlockMenu : MonoBehaviour
{
    public static BlockMenu Instance;
    public Block[] blocks;
    [FormerlySerializedAs("tileBtnPrefab")] public GameObject blockBtnPrefab;
    public Transform content;

    public static Block BlockBeingEdited;


    private void Awake()
    {
        if (Instance)
        {
            Destroy(Instance.gameObject);
        }

        transform.position = Input.mousePosition;
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var b in blocks)
        {
            if (b.type == BlockType.Empty) continue;

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
