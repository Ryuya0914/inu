using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffTeam : MonoBehaviour
{
    // チームの色を決める
    public enum TeamColor {
        SOLO = 0,
        REDTEAM = 1,
        BLUETEAM = 2
    };
    // 自分のチームの色
    public TeamColor m_teamColor = TeamColor.REDTEAM;

}
