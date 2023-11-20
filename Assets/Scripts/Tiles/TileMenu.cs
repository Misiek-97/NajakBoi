
using Tiles;
using UnityEngine;
using UnityEngine.Serialization;

public class TileMenu : MonoBehaviour
{
    public static TileMenu Instance;
    public Block[] tiles;
    public GameObject tileBtnPrefab;
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
        foreach (var t in tiles)
        {
            if (t.type == BlockType.Empty) continue;

            var btnGo = Instantiate(tileBtnPrefab, content);

            var tileBtn = btnGo.GetComponent<TileMenuButton>();
            tileBtn.block = t;
            tileBtn.SetUpButton();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close()
    {
        Destroy(gameObject);
    }
}
