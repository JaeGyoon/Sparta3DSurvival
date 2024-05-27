using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;

    public static CharacterManager Instance
    {
        get
        {
            // [����ڵ� : �ٸ� ������ Instance�� �����Ϸ��� �� �� �ص� �ν��Ͻ��� ���ٸ� ĳ���� �Ŵ����� ����]
            if (instance == null)
            {                
                instance = new GameObject("New Character Manager").AddComponent<CharacterManager>();
            }

            return instance;
        }
    }


    public Player player;
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    private void Awake()
    {
        if ( instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if ( instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
