
/// <summary>
/// OneAction이랑 같이 쓰길 권장합니다.  Text와 BackGround를 넣어주면 됩니다.
/// 약간 업적 시스템과 그 구조가 비슷한데 Condition을 통해 캔 푸드 5개를 갖고 있으면 해당 엔딩을 추가한다고 가정해보면
/// 1일차 때 캔 푸드를 5개 갖고 있다고 해도 해당 엔딩은 실행이 될 수 있습니다. 만약 최종적으로 검토해야 한다면 DayCondition을 마지막 엔딩일차 때 추가하는 걸로
/// 검증해야 할 것 같습니다. 
/// </summary>
    public class AddEnding : TriggerAction
    {
        public string text;
        public string imageName;
        public override void RunAction()
        {
            Managers.Game.AddEnding(text,imageName);
        }
    }
