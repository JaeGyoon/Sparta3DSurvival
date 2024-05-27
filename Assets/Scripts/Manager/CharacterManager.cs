using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance;

    public static CharacterManager Instance
    {
        get
        {
            // [방어코드 : 다른 씬에서 Instance를 참조하려고 할 때 해동 인스턴스가 없다면 캐릭터 매니저를 생성]
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
