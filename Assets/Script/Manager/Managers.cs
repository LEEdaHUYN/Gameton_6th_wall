
using Script.Manager.Core;

public class Managers : Singleton<Managers>
{
    // TODO : 필요한 메니저 클래스 정의.
    
    // UI Manager 
    // UI는 기본적으로 Component 구조를 따르되, 
    private UIManager _uiManager = new UIManager();

    public static UIManager UI
    {
        get { return Instance?._uiManager; }
    }
    
    //GAME Manager 
    // 캐릭터 인벤토리를 메모리 형태로 관리
    private GameManager _gameManager = new GameManager();
    public static GameManager Game
    {
        get { return Instance?._gameManager; }
    }
    // ResourceManager : Addressable
    private ResourceManager _resourceManager = new ResourceManager();

    public static ResourceManager Resource
    {
        get { return Instance?._resourceManager; }
    }
    // Sound Manager

    // Scene Manager
    private SceneManagerEx _sceneManager = new SceneManagerEx();
    public static SceneManagerEx Scene
    {
        get { return Instance?._sceneManager; }
    }

    // Data 처리 방식에 따라 구조가 달라짐. ( 빠른 시일내로 정하기로 )
    private DataManager _dataManager = new DataManager();

    public static DataManager Data
    {
        get { return Instance?._dataManager; }
    }

    // TODO : GPGS 연결 이후 ADMOB Manager 

    protected void Start()
    {
        // Data.Init();
    }
}        


// 중요한건, 2D를 맡는다 쳤을 떄 가장 중요한게, 무인도에서 어떻게 보여질지 Sprite를 찾는 것.
// 3D를 아예 원하는 방식대로 짜라고 그냥 던져주는게 나을 듯 싶기도 함.
// 씬 이동 및 Game Manager에 인벤토리 넣을 땐 관여 해주고 
// 나는 2D  Ingame 다 만든다음에 Lobby 요소를 Backend 통신을 통해 넣어야 할 것 같음 
// 그럼 우선적으로 구현해야할 게, 인게임 내에서 구현해야할 시스템 요소인데.
// 당연하게도 먼저 인벤토리가 있을 것임.
// 해당 인벤토리는 Memory Class로 관리할 예정 그럼 아이템이 존재해야겠는데.
// 이 아이템에 대한 내용을 Interface 로 따서 주면 될 듯 
// 아이템 구조를 먼저 잡고 난 후, 해당 아이템을 렌더링하고 주웠을 때 이벤트등은 알아서 하라 하고
// 걍 Game Manager에게 데이터만 전달해달라고 요청해주자. Invoke 떄려서 .

// 그럼 이 아이템을 먼저 Sprite에 셋팅해주고.
// 그 다음, 일지 시스템 구현  ( 랜덤한 이벤트가 나와야 하는데 이 랜덤 이벤트 구조를 어떻게 확률적으로 띄어줄지 생각해봐야 함 )
// 또 한, 캐릭터 관련해선 어떻게  데이터 관리를 할지도 생각해보면 좋을 듯 .
