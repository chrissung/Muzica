using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[AddComponentMenu("UI/Effects/Gradient")]

public class GradientTopBottom : BaseMeshEffect {
   [SerializeField]
   public Color32 topColor = Color.white;
   [SerializeField]
   public Color32 bottomColor = Color.black;

   public override void ModifyMesh(VertexHelper helper)
   {
      if (!IsActive() || helper.currentVertCount == 0)
         return;

      List<UIVertex> vertices = new List<UIVertex>();
      helper.GetUIVertexStream(vertices);

      float bottomY = vertices[0].position.y;
      float topY = vertices[0].position.y;

      for (int i = 1; i < vertices.Count; i++)
      {
         float y = vertices[i].position.y;
         if (y > topY)
         {
            topY = y;
         }
         else if (y < bottomY)
         {
            bottomY = y;
         }
      }

      float uiElementHeight = topY - bottomY;

      UIVertex v = new UIVertex();

      for (int i = 0; i < helper.currentVertCount; i++)
      {
         helper.PopulateUIVertex(ref v, i);
         v.color = Color32.Lerp(bottomColor, topColor, (v.position.y - bottomY) / uiElementHeight);
         helper.SetUIVertex(v, i);
      }
   }
}

