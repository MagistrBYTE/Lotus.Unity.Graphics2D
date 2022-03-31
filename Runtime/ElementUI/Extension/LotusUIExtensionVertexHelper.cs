//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIExtensionVertexHelper.cs
*		Методы расширения компонента VertexHelper.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Maths;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DUIExtension
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений класса <see cref="VertexHelper"/> 
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionVertexHelper
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавлении линии
			/// </summary>
			/// <param name="this">Набор вершин</param>
			/// <param name="start">Начальная позиция</param>
			/// <param name="end">Конечная позиция</param>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="color">Цвет линии</param>
			//---------------------------------------------------------------------------------------------------------
			public static void AddLine(this VertexHelper @this, Vector2 start, Vector2 end, Single thickness, Color color)
			{
				// Толщина
				Vector2 offset = new Vector2(start.y - end.y, end.x - start.x).normalized * thickness;
				var v1 = start - offset;
				var v2 = start + offset;
				var v3 = end + offset;
				var v4 = end - offset;

				UIVertex[] vbo = new UIVertex[4];
				vbo[0] = UIVertex.simpleVert;
				vbo[0].position = new Vector3(v1.x, v1.y, 0);
				vbo[0].color = color;
				vbo[0].uv0 = new Vector2(0, 1); // BottomLeft

				vbo[1] = UIVertex.simpleVert;
				vbo[1].position = new Vector3(v2.x, v2.y, 0);
				vbo[1].color = color;
				vbo[1].uv0 = Vector2.zero; // TopLeft

				vbo[2] = UIVertex.simpleVert;
				vbo[2].position = new Vector3(v3.x, v3.y, 0);
				vbo[2].color = color;
				vbo[2].uv0 = new Vector2(1, 0); //TopRight

				vbo[3] = UIVertex.simpleVert;
				vbo[3].position = new Vector3(v4.x, v4.y, 0);
				vbo[3].color = color;
				vbo[3].uv0 = new Vector2(1, 1); //BottomRight

				@this.AddUIVertexQuad(vbo);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================