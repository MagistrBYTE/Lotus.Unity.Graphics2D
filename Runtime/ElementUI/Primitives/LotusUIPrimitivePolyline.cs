//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIPrimitivePolyline.cs
*		Компонент векторного примитива полилинии.
*		Реализация компонент обеспечивающего генерацию векторного примитива полилинии.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DUIPrimitive
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Тип сегмента в линии
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TPrimitiveSegmentType
		{
			/// <summary>
			/// Начальный сегмент
			/// </summary>
			Start,

			/// <summary>
			/// Основной сегмент
			/// </summary>
			Middle,

			/// <summary>
			/// Конечный сегмент
			/// </summary>
			End,
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Тип соединения участков линии
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TPrimitiveJoinType
		{
			/// <summary>
			/// Отсутствует
			/// </summary>
			None,

			/// <summary>
			/// Скос
			/// </summary>
			Bevel,

			/// <summary>
			/// Торцовочная
			/// </summary>
			Miter
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент векторного примитива полилинии
		/// </summary>
		/// <remarks>
		/// Реализация компонент обеспечивающего генерацию векторного примитива полилинии
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathPrimitive + "Line")]
		public class LotusUIPrimitivePolyline : LotusUIPrimitiveBase
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Минимальный угол для корректировки соединения
			/// </summary>
			private const Single MinMiterJoin = 15 * Mathf.Deg2Rad;

			/// <summary>
			/// Скосом "хорошо" присоединиться вытесняет вершины отрезок, 
			/// вместо того чтобы просто рендеринг квада для подключения конечных точек. 
			/// Это улучшает внешний вид текстур и прозрачные линии, так как нет перекрытия
			/// </summary>
			private const Single MinBevelNiceJoin = 30 * Mathf.Deg2Rad;

			/// <summary>
			/// Текстурные координаты
			/// </summary>
			private static readonly Vector2 MapUV_TopLeft = Vector2.zero;

			/// <summary>
			/// Текстурные координаты
			/// </summary>
			private static readonly Vector2 MapUV_BottomLeft = new Vector2(0, 1);

			/// <summary>
			/// Текстурные координаты
			/// </summary>
			private static readonly Vector2 MapUV_TopCenter = new Vector2(0.5f, 0);

			/// <summary>
			/// Текстурные координаты
			/// </summary>
			private static readonly Vector2 MapUV_BottomCenter = new Vector2(0.5f, 1);

			/// <summary>
			/// Текстурные координаты
			/// </summary>
			private static readonly Vector2 MapUV_TopRight = new Vector2(1, 0);

			/// <summary>
			/// Текстурные координаты
			/// </summary>
			private static readonly Vector2 MapUV_BottomRight = new Vector2(1, 1);

			/// <summary>
			/// Начальный квад
			/// </summary>
			private static readonly Vector2[] StartQuadUV = new[] { MapUV_TopLeft, MapUV_BottomLeft, MapUV_BottomCenter, MapUV_TopCenter };

			/// <summary>
			/// Центральный квад
			/// </summary>
			private static readonly Vector2[] MiddleQuadUV = new[] { MapUV_TopCenter, MapUV_BottomCenter, MapUV_BottomCenter, MapUV_TopCenter };

			/// <summary>
			/// Конечный квад
			/// </summary>
			private static readonly Vector2[] EndQuadUV = new[] { MapUV_TopCenter, MapUV_BottomCenter, MapUV_BottomRight, MapUV_TopRight };
			#endregion

			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание векторного примитива полилинии
			/// </summary>
			/// <param name="left">Позиция по X примитива</param>
			/// <param name="top">Позиция по Y примитива</param>
			/// <param name="width">Ширина примитива</param>
			/// <param name="height">Высота примитива</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный примитив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusUIPrimitivePolyline CreatePolyline(Single left, Single top, Single width, Single height, RectTransform parent = null)
			{
				// 1) Создание объекта
				LotusUIPrimitivePolyline polyline = LotusElementUIDispatcher.CreateElement<LotusUIPrimitivePolyline>("Polyline",
					left, top, width, height);

				// 2) Определение в иерархии
				polyline.AddPoint(new Vector2(0, 0));
				polyline.AddPoint(new Vector2(width, height));
				polyline.SetParent(parent);

				return polyline;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			internal Rect mUVRect = new Rect(0f, 0f, 1f, 1f);
			[SerializeField]
			internal List<Vector2> mPoints;
			[SerializeField]
			internal Single mLineThickness = 2;
			[SerializeField]
			internal Boolean mLineList = false;
			[SerializeField]
			internal TPrimitiveJoinType mLineJoins = TPrimitiveJoinType.Bevel;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// UV прямоугольник используемый текстурой
			/// </summary>
			public Rect UVRect
			{
				get { return mUVRect; }
				set
				{
					if (mUVRect == value)
					{
						return;
					}

					mUVRect = value;
					SetVerticesDirty();
				}
			}

			/// <summary>
			/// Набор вершин образующих полилинию или список линий
			/// </summary>
			public List<Vector2> Points
			{
				get { return mPoints; }
				set
				{
					if (mPoints == value)
					{
						return;
					}
					mPoints = value;

					SetAllDirty();
				}
			}

			/// <summary>
			/// Толщина линии
			/// </summary>
			public Single LineThickness
			{
				get { return mLineThickness; }
				set
				{
					if (!Mathf.Approximately(mLineThickness, value))
					{
						mLineThickness = value;

						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Набор вершин представляет собой список последовательных отдельных линий
			/// </summary>
			public Boolean LineList
			{
				get { return mLineList; }
				set
				{
					if (mLineList != value)
					{
						mLineList = value;

						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}

			/// <summary>
			/// Тип соединения участков линии
			/// </summary>
			public TPrimitiveJoinType LineJoins
			{
				get { return mLineJoins; }
				set
				{
					if (mLineJoins != value)
					{
						mLineJoins = value;

						UpdateGeometryForced();
						SetAllDirty();
					}
				}
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------

			protected override void Reset()
			{
				if(mPoints == null) mPoints = new List<Vector2>();
				mUVRect = new Rect(0f, 0f, 1f, 1f);
				mLineThickness = 2;
				mLineJoins = TPrimitiveJoinType.Bevel;
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void Awake()
			{
				if (mPoints == null) mPoints = new List<Vector2>();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание массива вершин сегмента линии
			/// </summary>
			/// <param name="start">Начальная позиция линии</param>
			/// <param name="end">Конечная позиция линии</param>
			/// <param name="type">Тип сегмента</param>
			/// <returns>Массив вершин</returns>
			//---------------------------------------------------------------------------------------------------------
			private UIVertex[] CreateLineCap(Vector2 start, Vector2 end, TPrimitiveSegmentType type)
			{
				if (type == TPrimitiveSegmentType.Start)
				{
					var cap_start = start - (end - start).normalized * mLineThickness / 2;
					return CreateLineSegment(cap_start, start, TPrimitiveSegmentType.Start);
				}
				else if (type == TPrimitiveSegmentType.End)
				{
					var cap_end = end + (end - start).normalized * mLineThickness / 2;
					return CreateLineSegment(end, cap_end, TPrimitiveSegmentType.End);
				}

				Debug.LogError("Bad TPrimitiveSegmentType passed in to CreateLineCap. Must be TPrimitiveSegmentType.Start or TPrimitiveSegmentType.End");
				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание массива вершин сегмента линии
			/// </summary>
			/// <param name="start">Начальная позиция линии</param>
			/// <param name="end">Конечная позиция линии</param>
			/// <param name="type">Тип сегмента</param>
			/// <returns>Массив вершин</returns>
			//---------------------------------------------------------------------------------------------------------
			private UIVertex[] CreateLineSegment(Vector2 start, Vector2 end, TPrimitiveSegmentType type)
			{
				var uvs = MiddleQuadUV;
				if (type == TPrimitiveSegmentType.Start)
				{
					uvs = StartQuadUV;
				}
				else if (type == TPrimitiveSegmentType.End)
				{
					uvs = EndQuadUV;
				}

				// Толщина
				Vector2 offset = new Vector2(start.y - end.y, end.x - start.x).normalized * mLineThickness / 2;
				var v1 = start - offset;
				var v2 = start + offset;
				var v3 = end + offset;
				var v4 = end - offset;

				return SetVertexBufferQuad(new[] { v1, v2, v3, v4 }, uvs);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление указанной точки
			/// </summary>
			/// <param name="point">Точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddPoint(Vector2 point)
			{
				mPoints.Add(point);
				UpdateGeometryForced();
				SetAllDirty();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление точки по индексу
			/// </summary>
			/// <param name="index">Индекс обновляемой точки</param>
			/// <param name="point">Точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void UpdatePoint(Int32 index, Vector2 point)
			{
				mPoints[index] = point;
				UpdateGeometryForced();
				SetAllDirty();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление всех точек
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdatePoints()
			{
				UpdateGeometryForced();
				SetAllDirty();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление указанной точки
			/// </summary>
			/// <param name="point">Точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemovePoint(Vector2 point)
			{
				mPoints.Remove(point);
				UpdateGeometryForced();
				SetAllDirty();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление точки по индексу
			/// </summary>
			/// <param name="index">Индекс удаляемой точки</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemovePoint(Int32 index)
			{
				mPoints.RemoveAt(index);
				UpdateGeometryForced();
				SetAllDirty();
			}
			#endregion

			#region ======================================= МЕТОДЫ Graphic ============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация вершин элементом
			/// </summary>
			/// <param name="vertex_helper">Набор вершин для модификации</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnPopulateMesh(VertexHelper vertex_helper)
			{
				if (mPoints == null || mPoints.Count == 0)
				{
					return;
				}

				Vector2[] points_to_draw = mPoints.ToArray();

				var size_x = 1; //rectTransform.rect.width;
				var size_y = 1; //rectTransform.rect.height;

				var offset_x = -rectTransform.pivot.x * rectTransform.rect.width;
				var offset_y = -rectTransform.pivot.y * rectTransform.rect.height;

				// Очищаем набор вершин
				vertex_helper.Clear();

				// Мы будем генерировать квады из которых будет состоять наш линия
				var segments = new List<UIVertex[]>();

				// Если это список отдельных линий
				if (mLineList)
				{
					for (var i = 1; i < points_to_draw.Length; i += 2)
					{
						var start = points_to_draw[i - 1];
						var end = points_to_draw[i];

						// Формируем линию
						start = new Vector2(start.x * size_x + offset_x, start.y * size_y + offset_y);
						end = new Vector2(end.x * size_x + offset_x, end.y * size_y + offset_y);

						if (mLineJoins != TPrimitiveJoinType.None)
						{
							segments.Add(CreateLineCap(start, end, TPrimitiveSegmentType.Start));
						}

						segments.Add(CreateLineSegment(start, end, TPrimitiveSegmentType.Middle));

						if (mLineJoins != TPrimitiveJoinType.None)
						{
							segments.Add(CreateLineCap(start, end, TPrimitiveSegmentType.End));
						}
					}
				}
				else
				{
					for (var i = 1; i < points_to_draw.Length; i++)
					{
						var start = points_to_draw[i - 1];
						var end = points_to_draw[i];

						start = new Vector2(start.x * size_x + offset_x, start.y * size_y + offset_y);
						end = new Vector2(end.x * size_x + offset_x, end.y * size_y + offset_y);

						if (mLineJoins != TPrimitiveJoinType.None && i == 1)
						{
							segments.Add(CreateLineCap(start, end, TPrimitiveSegmentType.Start));
						}

						segments.Add(CreateLineSegment(start, end, TPrimitiveSegmentType.Middle));

						if (mLineJoins != TPrimitiveJoinType.None && i == mPoints.Count - 1)
						{
							segments.Add(CreateLineCap(start, end, TPrimitiveSegmentType.End));
						}
					}
				}

				// Теперь нужно добавить соединения между линиями
				for (var i = 0; i < segments.Count; i++)
				{
					if (!mLineList && i < segments.Count - 1)
					{
						var vec1 = segments[i][1].position - segments[i][2].position;
						var vec2 = segments[i + 1][2].position - segments[i + 1][1].position;

						// Угол между линиями
						var angle = Vector2.Angle(vec1, vec2) * Mathf.Deg2Rad;

						// Положительный знак означает, что линия вращается по часовой стрелки
						var sign = Mathf.Sign(Vector3.Cross(vec1.normalized, vec2.normalized).z);

						// Данные для соединения
						var miter_distance = mLineThickness / (2 * Mathf.Tan(angle / 2));
						var miter_point_a = segments[i][2].position - vec1.normalized * miter_distance * sign;
						var miter_point_b = segments[i][3].position + vec1.normalized * miter_distance * sign;

						var join_type = mLineJoins;
						if (join_type == TPrimitiveJoinType.Miter)
						{
							// Сможем ли мы сделать правильное (не слишком острое соединение)
							if (miter_distance < vec1.magnitude / 2 && miter_distance < vec2.magnitude / 2 && angle > MinMiterJoin)
							{
								segments[i][2].position = miter_point_a;
								segments[i][3].position = miter_point_b;
								segments[i + 1][0].position = miter_point_b;
								segments[i + 1][1].position = miter_point_a;
							}
							else
							{
								join_type = TPrimitiveJoinType.Bevel;
							}
						}

						if (join_type == TPrimitiveJoinType.Bevel)
						{
							if (miter_distance < vec1.magnitude / 2 && miter_distance < vec2.magnitude / 2 && angle > MinBevelNiceJoin)
							{
								if (sign < 0)
								{
									segments[i][2].position = miter_point_a;
									segments[i + 1][1].position = miter_point_a;
								}
								else
								{
									segments[i][3].position = miter_point_b;
									segments[i + 1][0].position = miter_point_b;
								}
							}

							var join = new UIVertex[] { segments[i][2], segments[i][3], segments[i + 1][0], segments[i + 1][1] };

							vertex_helper.AddUIVertexQuad(join);
						}
					}

					vertex_helper.AddUIVertexQuad(segments[i]);
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================