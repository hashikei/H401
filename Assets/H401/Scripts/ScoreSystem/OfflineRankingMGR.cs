﻿using UnityEngine;
using RankingExtension;

public class OfflineRankingMGR : MonoBehaviour {

    [SerializeField]
    public string ScorePrefabName;

    // Use this for initialization
    void Start () {
        GameObject obj = this.InstantiateChild(ScorePrefabName,false);
        ScoreWordMGR.Draw("6512937840",transform, 100f);


    }
	// Update is called once per frame
	void Update () {
	
	}
}
