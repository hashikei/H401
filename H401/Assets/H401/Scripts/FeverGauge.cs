﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FeverGauge : MonoBehaviour {

    [SerializeField]private Image FGImage;
    private float GAUGE_MAX = 1.0f;   //最大値
    private float decreaseRatio = 0.0f;
    private float gainRatio = 0.0f;
    
    private float feverValue;   //現在フィーバー値

    private _eFeverState feverState;

    [SerializeField]private GameObject FLightPrefab = null;
    private GameObject FLightObject = null;

    [SerializeField]private GameObject levelTableObject = null;
    [SerializeField]private Vector3 lightPosition;

    [SerializeField]private Color FGEmission;


    //private FeverLevelInfo feverLevel;
	// Use this for initialization
	void Start () {
        feverValue = 0.0f;
        FGImage.fillAmount = 0.0f;

        feverState = _eFeverState.NORMAL;

        LevelTables ltScript = levelTableObject.GetComponent<LevelTables>();
        gainRatio = ltScript.FeverGainRatio;
        decreaseRatio = ltScript.FeverDecreaseRatio;

        
	}
	
	// Update is called once per frame
	void Update () {

        if (feverState == _eFeverState.FEVER)
        {

            feverValue -= decreaseRatio;

            if (feverValue < 0.0f)
            {
                ChangeState(_eFeverState.NORMAL);
            }
        }


        FGImage.fillAmount = feverValue;
	}
    public void Gain(int nodeNum, int cap, int path2, int path3)
    {
        if (nodeNum != 0)
            feverValue += gainRatio;
        //MAXになったらフィーバーモードへ
        //今はとりあえず0に戻す

        if (feverState == _eFeverState.FEVER)
            return;

        if(feverValue > GAUGE_MAX)
        {
            ChangeState(_eFeverState.FEVER);
        }

    }

    void ChangeState(_eFeverState state)
    {
        feverState = state;
        switch(feverState)
        {
            case _eFeverState.NORMAL:
                feverValue = 0.0f;
                if (FLightObject != null)
                    Destroy(FLightObject);
                FGImage.material.EnableKeyword("_EMISSION");
                FGImage.material.SetColor("_EmissionColor", Color.black);
                break;
            case _eFeverState.FEVER:
                //中心地点を設定しなければならないらしい


                FLightObject = (GameObject)Instantiate(FLightPrefab,lightPosition,transform.rotation);
                FGImage.material.EnableKeyword("_EMISSION");
                FGImage.material.SetColor("_EmissionColor",FGEmission);
                feverValue = GAUGE_MAX;
                break;
        }
    }
}
