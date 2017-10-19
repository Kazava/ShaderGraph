﻿using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Graphing;
using UnityEngine.MaterialGraph;

namespace UnityEditor.MaterialGraph.Drawing.Inspector
{
    public class ShaderLayerView : VisualElement
    {
        public LayeredShaderGraph graph { get; }
        public LayeredShaderGraph.Layer layer { get; }

        public ShaderLayerView(LayeredShaderGraph graph, LayeredShaderGraph.Layer layer)
        {
            this.graph = graph;
            this.layer = layer;

            Add(new IMGUIContainer(ValueField) { name = "value" });
            Add(new Button(OnClickRemove) { name = "remove", text = "Remove" });
        }

        void OnClickRemove()
        {
            graph.RemoveLayer(layer.guid);
            NotifyNodes();
        }

        void ValueField()
        {
            EditorGUI.BeginChangeCheck();

            var newShader = EditorGUILayout.ObjectField("Shader", layer.shader, typeof(Shader), false) as Shader;
            if (newShader != layer.shader)
            {
                if (graph.SetLayer(layer.guid, newShader))
                    NotifyNodes();
            }
        }

        void NotifyNodes()
        {
            foreach (var node in graph.GetNodes<PropertyNode>())
                node.onModified(node, ModificationScope.Graph);
        }
    }
}