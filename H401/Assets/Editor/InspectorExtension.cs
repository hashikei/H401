﻿using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
public class Node_Editor : Editor {
    Node tgt;
    public override void OnInspectorGUI() {
        tgt = target as Node;
        EditorGUILayout.LabelField(tgt.ToString());
        base.OnInspectorGUI();
    }
}
[CustomEditor(typeof(DynamoConnecter))]
public class AWS_Editor : Editor {
    public override void OnInspectorGUI() {
        var tgt = target as DynamoConnecter;

        tgt.UseProxy = EditorGUILayout.Toggle("UseProxy", tgt.UseProxy);
        if(tgt.UseProxy) {
            tgt.ProxyHost = EditorGUILayout.TextField("ProxyHost", tgt.ProxyHost);
            tgt.ProxyPort = EditorGUILayout.IntField("Port", tgt.ProxyPort);
            tgt.UserName = EditorGUILayout.TextField("UserName", tgt.UserName);
            tgt.Password = EditorGUILayout.PasswordField("Password", tgt.Password);
            EditorGUILayout.LabelField("DefaultName", DynamoConnecter.DefaultName);
        }
    }
}

[CustomEditor(typeof(Score3D))]
public class Score3D_Editor : Editor {
    public override void OnInspectorGUI() {
        var tgt = target as Score3D;

        EditorGUILayout.HelpBox("This is \"DEBUG ONLY\". \n Reset Value at the time of play", MessageType.Warning, true);
        GUILayoutOption[] options = { GUILayout.MaxWidth(25.0f), GUILayout.MinWidth(25.0f), GUILayout.ExpandWidth(false) };
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("α1", options);
        Score3D.AlphaOneZ = EditorGUILayout.FloatField(Score3D.AlphaOneZ);
        EditorGUILayout.LabelField("α0", options);
        Score3D.AlphaZeroZ = EditorGUILayout.FloatField(Score3D.AlphaZeroZ);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.MinMaxSlider(ref Score3D.AlphaOneZ, ref Score3D.AlphaZeroZ, 0, 150);
    }
}
