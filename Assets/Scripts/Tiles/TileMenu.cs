
using Tiles;
using UnityEngine;
using UnityEngine.Serialization;

public class TileMenu : MonoBehaviour
{
    public static TileMenu Instance;
    public Tile[] tiles;
    public GameObject tileBtnPrefab;
    public Transform content;

    public static Tile TileBeingEdited;


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
            if (t.type == TileType.Empty) continue;

            var btnGo = Instantiate(tileBtnPrefab, content);

            var tileBtn = btnGo.GetComponent<TileMenuButton>();
            tileBtn.tile = t;
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
