using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamScript : MonoBehaviour
{
    // チームの色を決める
    public enum TeamColor {
        REDTEAM = 0,
        BLUETEAM = 1
    };
    // 自分のチームの色
    public TeamColor m_teamColor = TeamColor.REDTEAM;

    

}
