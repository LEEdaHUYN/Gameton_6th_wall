using System;
using UnityEngine;

//Define을 Partical로 정의한 이유 - 차후 협업 시 Define 코드는 수정이 빈번히 일어날 수 있으므로
// Partical로 Enum, 그외 데이터등을 분류하는게 나아보임. 컨플릭트를 줄이기 위한 방편.
public partial class Define
{
    public static Vector2 ScreenSize => new Vector2(1080, 1920);
}