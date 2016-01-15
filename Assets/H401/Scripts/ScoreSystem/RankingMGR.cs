﻿#pragma warning disable 414
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using System;

namespace RankingExtension
{
    public static class InstantiateEx
    {
        // ゲームオブジェクトを生成して、子に登録する関数
        public static GameObject InstantiateChild(this MonoBehaviour Mono, string Name, bool WorldPositionStays = true)
        {
            GameObject ret = null;
            string Pass = "";
            try
            {
                RankingMGR mgr = Mono.GetComponentInParent<RankingMGR>();
                if (mgr.RankingPrefabFolderName != "")
                {
                    Pass += mgr.RankingPrefabFolderName + "/";
                }
            }
            catch { return null; }
            Pass += Name;

            try
            {
                ret = MonoBehaviour.Instantiate(Resources.Load(Pass)) as GameObject;
                ret.transform.SetParent(Mono.transform, WorldPositionStays);
            }
            catch { ret = null; }
            return ret;
        }
    }
}

public class RankingMGR : MonoBehaviour {

    [SerializeField] public string RankingPrefabFolderName = null;
    [SerializeField] public string OnlinePrefabName = null;
    [SerializeField] public string OfflinePrefabName = null;
    [SerializeField] public string BGPrefabName = null;
    [SerializeField] public string ScorePrefabName = null;
    [SerializeField] public string CameraPrefabName = null;
    [SerializeField] public string ReturnButtonName = null;

    GameObject ScoreObj = null;
    GameObject OnlineObj = null;
    GameObject OfflineObj = null;
    GameObject BGObj = null;
    GameObject CameraObj = null;
    GameObject ReturnBtnObj = null;
    ScoreWordMGR Sword = new ScoreWordMGR();
    private bool FlipRanking = false;
    public enum RANKING_MODE : byte{
        OFFLINE = 0,
        ONLINE = 1
    };
    public RANKING_MODE Mode = RANKING_MODE.OFFLINE;

    // ゲームオブジェクトを生成して、子に登録する関数
    private GameObject InstantiateChild(string Name, bool WorldPositionStays = true) {
        GameObject ret = null;
        string Pass = "";
        if(RankingPrefabFolderName != "") {
            Pass += RankingPrefabFolderName + "/";
        }
        Pass += Name;

        try {
            ret = Instantiate(Resources.Load(Pass)) as GameObject;
            ret.transform.SetParent(transform, WorldPositionStays);
        }
        catch(Exception excep) {
            ret = null;
            Debug.LogError("Exception ! : " + Pass);
            Debug.LogException(excep);
        }
        return ret;
    }

    // Use this for initialization
    void Start() {
        // オブジェクト初期化
        ScoreObj = InstantiateChild(ScorePrefabName);
        BGObj = InstantiateChild(BGPrefabName, false);
        CameraObj = InstantiateChild(CameraPrefabName, false);
        ReturnBtnObj = InstantiateChild(ReturnButtonName,false);
        OfflineObj = InstantiateChild(OfflinePrefabName, false);
        OnlineObj = null;   // オンラインオブジェは初期化しない
        Sword.Load();

        // ランキングのフリップ処理
        this.UpdateAsObservable()
            .Select(_ => FlipRanking)   // フリップフラグがONになった瞬間を感知
            .DistinctUntilChanged()
            .Where(_ => _)
            .Select(x => Mode)          // オンラインかオフラインかのフラグを変数に設定
            .Subscribe(x => {
                switch(x) {
                    case RANKING_MODE.OFFLINE:
                        OfflineObj.transform.DOLocalMoveY(1500, 0.5f);
                        Mode = RANKING_MODE.ONLINE;
                        if(OnlineObj != null) {
                            //OnlineObj.gameObject.SetActive(true);
                        }
                        break;
                    case RANKING_MODE.ONLINE:
                        OfflineObj.transform.DOLocalMoveY(0, 0.5f);
                        Mode = RANKING_MODE.OFFLINE;
                        if(OnlineObj != null) {
                            //OnlineObj.gameObject.SetActive(false);
                        }
                        break;
                }
                FlipRanking = false;
            }).AddTo(this);

        // オンラインランキングの初期化
        this.UpdateAsObservable()
            .Where(_ => Mode == RANKING_MODE.ONLINE)
            .Take(1)
            .Subscribe(_ => {
                OnlineObj = InstantiateChild(OnlinePrefabName, false);
            }).AddTo(this);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            Flip();
        }
    }

    // ランキングを切り替えるトリガーを引く関数
    public void Flip() {
        FlipRanking = true;
    }
}
#pragma warning restore 414

