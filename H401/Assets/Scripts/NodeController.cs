﻿using UnityEngine;
using System.Collections;
using UniRx;

using DG.Tweening;

/*  リストIDに関して

            (n,n)
     ・・・◇◇◇
    ・・・・・・
     ・・・・・・
    ◇◇◇・・・
     ◇◇◇・・・
    ◇◇◇・・・
    (0,0)
*/

public class NodeController : MonoBehaviour {

    private const float ADJUST_PIXELS_PER_UNIT = 0.01f;     // Pixels Per Unit の調整値

    [SerializeField] private int row = 0;       // 横配置数
    [SerializeField] private int col = 0;       // 縦配置数
    [SerializeField] private GameObject nodePrefab = null;       // パネルのプレハブ
    [SerializeField] private float widthMargin  = 0.0f;  // パネル位置の左右間隔の調整値
    [SerializeField] private float heightMargin = 0.0f;  // パネル位置の上下間隔の調整値

    private GameObject[,]   nodePrefabs;     // パネルのプレハブリスト
    private Node[,]         nodeScripts;     // パネルのnodeスクリプトリスト

    private Vector2 nodeSize = Vector2.zero;    // 描画するパネルのサイズ

    private bool        isDrag          = false;                // マウスドラッグフラグ
    private Vec2Int     beforeTapNodeID = Vec2Int.zero;         // 移動させたいノードのID
    private Vec2Int     afterTapNodeID  = Vec2Int.zero;         // 移動させられるノードのID(移動方向を判定するため)
    private _eSlideDir  slideDir        = _eSlideDir.NONE;      // スライド中の方向

    private Vector2 startTapPos = Vector2.zero;     // タップした瞬間の座標
    private Vector2 tapPos      = Vector2.zero;     // タップ中の座標

    public int Row {
        get { return this.row; }
    }
    
    public int Col {
        get { return this.col; }
    }

    public bool IsDrag {
        set { this.isDrag = value; }
        get { return this.isDrag; }
    }

    public Vec2Int BeforeTapNodeID {
        set { this.beforeTapNodeID = value; }
        get { return this.beforeTapNodeID; }
    }

    public Vec2Int AfterTapNodeID {
        set { this.afterTapNodeID = value; }
        get { return this.afterTapNodeID; }
    }

    public _eSlideDir SlideDir {
        get { return slideDir; }
    }

    void Awake() {
        nodePrefabs = new GameObject[row, col];
        nodeScripts = new Node[row, col];
    }

	// Use this for initialization
	void Start () {
        // ----- パネル準備
        // 描画するパネルの大きさを取得
        Vector3 pos = transform.position;
        nodeSize.x = nodePrefab.GetComponent<SpriteRenderer>().sprite.texture.width * nodePrefab.transform.localScale.x * ADJUST_PIXELS_PER_UNIT;
        nodeSize.y = nodePrefab.GetComponent<SpriteRenderer>().sprite.texture.height * nodePrefab.transform.localScale.y * ADJUST_PIXELS_PER_UNIT;
        nodeSize.x -= widthMargin * ADJUST_PIXELS_PER_UNIT;
        nodeSize.y -= heightMargin * ADJUST_PIXELS_PER_UNIT;

        // パネルを生成
        for(int i = 0; i < col; ++i) {
            // パネルの配置位置を調整(Y座標)
            pos.y = transform.position.y + nodeSize.y * -(col * 0.5f - (i + 0.5f));

            for (int j = 0; j < row; ++j) {
                // パネルの配置位置を調整(X座標)
                pos.x = i % 2 == 0 ? transform.position.x + nodeSize.x * -(row * 0.5f - (j + 0.25f)) : transform.position.x + nodeSize.x * -(row * 0.5f - (j + 0.75f));

                // 生成
        	    nodePrefabs[j,i] = (GameObject)Instantiate(nodePrefab, pos, transform.rotation);
                nodeScripts[j,i] = nodePrefabs[j,i].GetComponent<Node>();
                nodePrefabs[j,i].transform.parent = transform;

                // パネルの位置(リストID)を登録
                nodeScripts[j,i].RegistNodeID(j, i);
            }
        }

        // パネルに情報を登録
        nodeScripts[0,0].SetNodeController(this);

        // ----- インプット処理
        Observable
            .EveryUpdate()
            .Where(_ => Input.GetMouseButton(0))
            .Subscribe(_ => {
                isDrag = true;

                // スライド処理
                if(slideDir != _eSlideDir.NONE) {
                    Vector3 worldTapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    tapPos = new Vector2(worldTapPos.x, worldTapPos.y);
                    SlantMove(nodeSize);
                }
            })
            .AddTo(this.gameObject);
        Observable
            .EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Subscribe(_ => {
                Vector3 worldTapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                startTapPos = new Vector2(worldTapPos.x, worldTapPos.y);
            })
            .AddTo(this.gameObject);
        Observable
            .EveryUpdate()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Subscribe(_ => {
                isDrag   = false;
                slideDir = _eSlideDir.NONE;
            })
            .AddTo(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void SlantMove(Vector2 vec) {
        // スライド対象となるノードの準備
        Vec2Int upNodeID   = afterTapNodeID;    // 上方向への探索ノードID
        Vec2Int downNodeID = beforeTapNodeID;   // 下方向への探索ノードID

        switch (slideDir) {
            case _eSlideDir.LEFT:
            case _eSlideDir.RIGHT:
                // タップしているノードを移動
                Vector2 pos = tapPos;
                pos.y = nodePrefabs[beforeTapNodeID.x,beforeTapNodeID.y].transform.position.y;
                nodeScripts[beforeTapNodeID.x,beforeTapNodeID.y].SlideNode(slideDir, pos);
                
                // タップしているノードより右側のノードを移動
                for(int i = beforeTapNodeID.x + 1, j = 1; i < row; ++i, ++j) {
                    pos = tapPos + vec * j;
                    pos.y = nodePrefabs[i,beforeTapNodeID.y].transform.position.y;
                    nodeScripts[i,beforeTapNodeID.y].SlideNode(slideDir, pos);
                }

                // タップしているノードより左側のノードを移動
                for(int i = beforeTapNodeID.x - 1, j = 1; i >= 0; --i, ++j) {
                    pos = tapPos - vec * j;
                    pos.y = nodePrefabs[i,beforeTapNodeID.y].transform.position.y;
                    nodeScripts[i,beforeTapNodeID.y].SlideNode(slideDir, pos);
                }
                break;

            case _eSlideDir.LEFTUP:
            case _eSlideDir.RIGHTDOWN:
                // 移動させられるノードより、上に位置するノードを移動
                while(upNodeID.x >= 0 && upNodeID.y < col) {
                    nodeScripts[upNodeID.x,upNodeID.y].SlideNode(slideDir, vec);

                    if(upNodeID.y % 2 == 0)
                        --upNodeID.x;
                    ++upNodeID.y;
                }
                // 移動させられるノードより、下に位置するノードを移動
                while(downNodeID.x < row && downNodeID.y >= 0) {
                    nodeScripts[downNodeID.x,downNodeID.y].SlideNode(slideDir, vec);

                    if(downNodeID.y % 2 != 0)
                        ++downNodeID.x;
                    --downNodeID.y;
                }
                break;
                
            case _eSlideDir.RIGHTUP:
            case _eSlideDir.LEFTDOWN:
                // 移動させられるノードより、上に位置するノードを移動
                while(upNodeID.x < row && upNodeID.y < col) {
                    nodeScripts[upNodeID.x,upNodeID.y].SlideNode(slideDir, vec);

                    if(upNodeID.y % 2 != 0)
                        ++upNodeID.x;
                    ++upNodeID.y;
                }
                // 移動させられるノードより、下に位置するノードを移動
                while(downNodeID.x >= 0 && downNodeID.y >= 0) {
                    nodeScripts[downNodeID.x,downNodeID.y].SlideNode(slideDir, vec);

                    if(downNodeID.y % 2 == 0)
                        --downNodeID.x;
                    --downNodeID.y;
                }
                break;

            default:
                break;
        }
    }

    void SlideNodes() {
        Vector2 moveDir = tapPos - startTapPos;     // スライドしている移動方向ベクトル

        switch(slideDir) {
            case _eSlideDir.LEFT:
            case _eSlideDir.RIGHT:
                break;

            case _eSlideDir.LEFTUP:
            case _eSlideDir.RIGHTDOWN:
                break;

            case _eSlideDir.RIGHTUP:
            case _eSlideDir.LEFTDOWN:
                break;

            default:
                break;
        }
    }
    

    //移動したいノードを確定
    //ドラッグを算出し移動したい方向列を確定
    //ドラッグされている間、列ごと移動、
        //タップ点からスワイプ地点まで座標の差分を算出し
        //列のすべてのノードをその位置へ移動させる
    //離すと一番近いノード確定位置まで調整

    public void StartSlideNodes() {
        int subRowID = afterTapNodeID.x - beforeTapNodeID.x;   // ノードIDの差分(横方向)
        int subColID = afterTapNodeID.y - beforeTapNodeID.y;   // ノードIDの差分(縦方向)
        Vector2 vec   = new Vector2(nodePrefabs[afterTapNodeID.x, afterTapNodeID.y].transform.position.x,   // スライド方向ベクトル兼移動量を算出
                                    nodePrefabs[afterTapNodeID.x, afterTapNodeID.y].transform.position.y)
                        - new Vector2(nodePrefabs[beforeTapNodeID.x, beforeTapNodeID.y].transform.position.x,
                                    nodePrefabs[beforeTapNodeID.x, beforeTapNodeID.y].transform.position.y);
        
        // 左にスライド
        if(subRowID == -1 && subColID == 0) {
            slideDir = _eSlideDir.LEFT;
        }
        // 右にスライド
        if(subRowID == 1 && subColID == 0) {
            slideDir = _eSlideDir.RIGHT;
        }
        // 左上にスライド
        if(subColID == 1 && vec.x < 0.0f && vec.y > 0.0f) {
            slideDir = _eSlideDir.LEFTUP;
        }
        // 左下にスライド
        if(subColID == -1 && vec.x < 0.0f && vec.y < 0.0f) {
            slideDir = _eSlideDir.LEFTDOWN;
        }
        // 右上にスライド
        if(subColID == 1 && vec.x > 0.0f && vec.y > 0.0f) {
            slideDir = _eSlideDir.RIGHTUP;
        }
        // 右下にスライド
        if(subColID == -1 && vec.x > 0.0f && vec.y < 0.0f) {
            slideDir = _eSlideDir.RIGHTDOWN;
        }

        SlantMove(vec);
    }

    //public void LoopBackNode(Vector2 id, _eSlideDir slideDir) {
    //    switch(slideDir) {
    //        case _eSlideDir.LEFT:
    //            GameObject outNode = nodePrefabs[(int)id.x,(int)id.y];
    //            Node outNodeScript = nodeScripts[(int)id.x,(int)id.y];
            
    //            // 画面外に出たパネルの位置を調整
    //            Vector3 pos = nodePrefabs[row - 1,(int)id.y].transform.position;
    //            pos.x += nodeSpriteSize.x;
    //            outNode.transform.position = pos;

    //            // 画面内のパネルデータをソート
    //            for(int i = 1; i < row; ++i) {
    //                nodePrefabs[i - 1, (int)id.y] = nodePrefabs[i,(int)id.y];
    //                nodeScripts[i - 1, (int)id.y] = nodeScripts[i,(int)id.y];
    //            }
    //            nodePrefabs[row - 1,(int)id.y] = outNode;
    //            nodeScripts[row - 1,(int)id.y] = outNodeScript;

    //            break;
    //    }
    //}

    public void CheckLink()
    {
        bool bComplete = false;

        //すべてのノードの根本を見る
        for (int i = 0; i < row; i++)
        {
            for(int link = 0 ; link < 6 ; link++)
            {
                if(nodeScripts[i, 0].CheckBit())   //親ノードの方向は↓向きのどちらかにしておく
                {
                    bComplete = true;
                }
                //１走査ごとに閲覧済みフラグを戻す
                ResetCheckedFragAll();
            }

            //すべて見終わったあと、完成済の枝に対して処理をここでする
            if(bComplete)
            {
                //枝に使っているノードにはbCompleteが立っているので、それに対していろいろする
                print("枝が完成しました！");
            }
        }

    }

    //閲覧済みフラグを戻す処理
    public void ResetCheckedFragAll()
    {
        foreach (var nodes in nodeScripts)
        {
            nodes.CheckFlag = false;
        }
    }


    //位置と方向から、指定ノードに隣り合うノードのrowとcolを返す
    //なければ、(-1,-1)を返す
    public Vec2Int GetDirNode(Vec2Int nodeID,_eLinkDir toDir)
    {
        //走査方向のノードのcolとrow

        Vec2Int nextNodeID;

        nextNodeID.x = nodeID.x;
        nextNodeID.y = nodeID.y;

        bool Odd = ((nodeID.y % 2) == 0) ? false : true;

        //次のノード番号の計算
        switch(toDir)
        {
            case _eLinkDir.RU:
                if (Odd)
                    nextNodeID.x++;
                nextNodeID.y++;
                break;
            case _eLinkDir.R:
                nextNodeID.x++;
                break;
            case _eLinkDir.RD:
                if (Odd)
                    nextNodeID.x++;
                nextNodeID.y--;
                break;
            case _eLinkDir.LD:
                if(!Odd)
                    nextNodeID.x--;
                nextNodeID.y--;
                break;
            case _eLinkDir.L:
                nextNodeID.x--;
                break;
            case _eLinkDir.LU:
                if (!Odd)
                    nextNodeID.x--;
                nextNodeID.y++;

                break;
        }

        if (nextNodeID.x < 0 || nextNodeID.x > row ||nextNodeID.y < 0 || nextNodeID.y > col)
        {
            nextNodeID.x = -1;
            nextNodeID.y = -1;
        }

        return nextNodeID;
    }

    //指定した位置のノードのスクリプトをもらう
    public Node GetNodeScript(Vec2Int nodeID)
    {
        return nodeScripts[nodeID.x, nodeID.y];
    }



}
