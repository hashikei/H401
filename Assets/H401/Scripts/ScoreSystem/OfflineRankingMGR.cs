﻿using UnityEngine;
using RankingExtension;

public class OfflineRankingMGR : MonoBehaviour {

    /// <summary>
    /// 数値の位置を調整する為の変数群
    /// </summary>
    [System.Serializable]
    private class ScorePositon {
        [SerializeField,Range(5,20)]
        public float WordHeight = 0f;
        [SerializeField, Range(10, 40)]
        public float HeadMargin = 0f;
        [SerializeField, Range(0, 30)]
        public float Margin = 0f;
        [SerializeField, Range(0, 100)]
        public float XPostiion = 0.0f;
    }
    [SerializeField]
    ScorePositon ScorePos;

    /// <summary>
    ///  順位の位置を調整するための変数群
    /// </summary>
    [System.Serializable]
    private class RankPositon {
        [SerializeField,Range(5,25)]
        public float WordHeight = 0f;
        [SerializeField, Range(0, 100)]
        public float XPostiion = 0.0f;
    }
    [SerializeField]
    RankPositon RankPos;

    /// <summary>
    /// トップのサイズ倍率を調整するための変数群
    /// </summary>
    [System.Serializable]
    private class RankZoom {
        private float[] ZoomVal = new float[10];

        [SerializeField, Range(1, 2)]
        float first = 1.0f;
        [SerializeField, Range(1, 2)]
        float secound = 1.0f;
        [SerializeField, Range(1, 2)]
        float Third = 1.0f;

        /// <summary>
        ///  初期化
        /// </summary>
        public void Start() {
            ZoomVal[0] = first;
            ZoomVal[1] = secound;
            ZoomVal[2] = Third;
            for(int n = 3; n < 10; n++) {
                ZoomVal[n] = 1.0f;
            }
        }

        /// <summary>
        /// getインデクサ
        /// </summary>
        /// <param name="idx">何位を取得するか</param>
        /// <returns>float型で倍率</returns>
        public float this[int idx]
        {
            get
            {
                if(idx < 1 || 10 < idx) {
                    return 0f;
                }
                return ZoomVal[idx - 1];
            }
        }
    }
    [SerializeField]
    RankZoom ZoomOption;

    // Use this for initialization
    void Start() {

        // スコア管理オブジェクトを取得
        var ScoreMgr = transform.parent.GetComponentInChildren<ScoreManager>();
        var localCanvas = GetComponentInChildren<RectTransform>();

        // 変数宣言
        ZoomOption.Start();
        string ScoreString = "";
        float Ypos = 0f;
        Ypos -= ScorePos.HeadMargin;

        // スコアの表示
        for(int n = 1; n < 11; n++) {
            Ypos -= ScorePos.Margin * ZoomOption[n == 1 ? 10 : n - 1] + (ScorePos.WordHeight * ZoomOption[n]);  // マージン追加

            // スコア数値を描画する
            ScoreString = ScoreMgr.GetScore(n).ToString();                                              // スコアの値を取得
            var Canv = ScoreWordMGR.Draw(ScoreString, localCanvas.transform, (ScorePos.WordHeight * ZoomOption[n]));      // 描画
            var CanvRectTrans = Canv.GetComponentInChildren<RectTransform>();                           // 位置を調整
            CanvRectTrans.anchorMax = new Vector2(0.5f, 1.0f);
            CanvRectTrans.anchorMin = new Vector2(0.5f, 1.0f);
            CanvRectTrans.pivot = new Vector2(1.0f, 0.5f);
            CanvRectTrans.anchoredPosition = new Vector2(ScorePos.XPostiion * ZoomOption[n], Ypos);

            // 順位を描画する
            Canv = ScoreWordMGR.DrawRank(n, localCanvas.transform, RankPos.WordHeight * ZoomOption[n]);
            CanvRectTrans = Canv.GetComponentInChildren<RectTransform>();
            CanvRectTrans.anchorMax = new Vector2(0.5f, 1.0f);
            CanvRectTrans.anchorMin = new Vector2(0.5f, 1.0f);
            CanvRectTrans.pivot = new Vector2(0.0f, 0.5f);
            CanvRectTrans.anchoredPosition = new Vector2(-RankPos.XPostiion * ZoomOption[n], Ypos);

        }
    }
    // Update is called once per frame
    void Update() {

    }
}
