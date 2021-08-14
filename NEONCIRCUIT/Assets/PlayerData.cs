using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int deathCount;
    public float time;
    public bool hasReachedCheckpoint;

    public float[] playerPos;
    public float[] playerRot;

    public float HP;
    public float PrimAmmo;
    public float secAmmo;

    public int assignedColor;
    public int seed;
    public int gridseed;

    public PlayerData(Playerstats playerstats)
    {
        time = playerstats.time;
        deathCount = playerstats.deaths;
        hasReachedCheckpoint = playerstats.Checkpoint;
        HP = playerstats.CptHP;
        PrimAmmo = playerstats.CptPrimary;
        secAmmo = playerstats.CptSecondary;

        playerPos = new float[3];
        playerPos[0] = playerstats.gameObject.transform.position.x;
        playerPos[1] = playerstats.gameObject.transform.position.y;
        playerPos[2] = playerstats.gameObject.transform.position.z;

        playerRot = new float[3];
        playerRot[0] = playerstats.gameObject.transform.rotation.x;
        playerRot[1] = playerstats.gameObject.transform.rotation.y;
        playerRot[2] = playerstats.gameObject.transform.rotation.z;

        assignedColor = (int) playerstats.CptColor;

        seed = playerstats.seed;
        gridseed = playerstats.gridseed;


    }
}
