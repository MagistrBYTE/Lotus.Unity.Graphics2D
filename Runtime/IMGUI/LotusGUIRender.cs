//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIRender.cs
*		Рендер геометрических примитивов.
*		Рендер для рисования геометрических векторных примитивов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DImmedateGUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Рендер геометрических примитивов
		/// </summary>
		/// <remarks>
		/// Рендер для рисования геометрических векторных примитивов
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XGUIRender
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя шейдера для рисования линий
			/// </summary>
			private const String ShaderColorBlend = "Lotus/Render2D/Color Blend";

			/// <summary>
			/// Имя шейдера для рисования линий с текстурой
			/// </summary>
			private const String ShaderTextureBlend = "Lotus/Render2D/Texture Blend";

			/// <summary>
			/// Белый цвет
			/// </summary>
			private static Color ColorWhite = Color.white;

			/// <summary>
			/// Текстурные координаты (0, 1)
			/// </summary>
			public static readonly Vector2 MapUV_TopLeft = new Vector2(0, 1);

			/// <summary>
			/// Текстурные координаты
			/// </summary>
			public static readonly Vector2 MapUV_TopCenter = new Vector2(0.5f, 1);

			/// <summary>
			/// Текстурные координаты (1, 1)
			/// </summary>
			public static readonly Vector2 MapUV_TopRight = new Vector2(1, 1);

			/// <summary>
			/// Текстурные координаты (0, 0)
			/// </summary>
			public static readonly Vector2 MapUV_BottomLeft = new Vector2(0, 0);

			/// <summary>
			/// Текстурные координаты (1, 0)
			/// </summary>
			public static readonly Vector2 MapUV_BottomRight = new Vector2(1, 0);
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			private static Material mColoredMaterial2D = null;
			private static Material mTexturedMaterial2D = null;

			// Переменные состояния
			private static Vector2 mCurrentPoint;
			private static Boolean mAntiAlias;
			private static Color mStrokeColor = Color.red;
			private static Color mFillColor = Color.green;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ГЕОМЕТРИЧЕСКИЕ ПРИМИТИВЫ
			//
			/// <summary>
			/// Словарь сгенерированных примитивов
			/// </summary>
			public readonly static Dictionary<Int32, CPrimitive2D> Primitives = new Dictionary<Int32, CPrimitive2D>();

			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Материал для рисования одноцветных 2D примитивов
			/// </summary>
			public static Material ColoredMaterial2D
			{
				get
				{
					if (mColoredMaterial2D == null)
					{
						OnInit();
					}

					return mColoredMaterial2D;
				}
			}

			/// <summary>
			/// Материал для рисования текстурированных 2D примитивов
			/// </summary>
			public static Material TexturedMaterial2D
			{
				get
				{
					if (mTexturedMaterial2D == null)
					{
						OnInit();
					}

					return mTexturedMaterial2D;
				}
			}

			//
			// ПЕРЕМЕННЫЕ СОСТОЯНИЯ
			//
			/// <summary>
			/// Текущая позиция пера при последовательном рисовании
			/// </summary>
			public static Vector2 CurrentPoint
			{
				get { return mCurrentPoint; }
				set
				{
					mCurrentPoint = value;
				}
			}

			/// <summary>
			/// Статус сглаживания линий
			/// </summary>
			/// <remarks>
			/// Применяется при расширенных метода рисования каркасных примитивов.
			/// Свойство должно устанавливаться непосредственно перед рисованием примитива
			/// </remarks>
			public static Boolean AntiAlias
			{
				get { return mAntiAlias; }
				set
				{
					if (mAntiAlias != value)
					{
						mAntiAlias = value;
						if (mAntiAlias)
						{
							mTexturedMaterial2D.mainTexture = XTexture2D.White3х3Alpha10;
						}
						else
						{
							mTexturedMaterial2D.mainTexture = Texture2D.whiteTexture;
						}
					}
				}
			}

			/// <summary>
			/// Цвет линий при рисовании каркасных примитивов
			/// </summary>
			/// <remarks>
			/// Свойство должно устанавливаться непосредственно перед рисованием примитива
			/// </remarks>
			public static Color StrokeColor
			{
				get { return mStrokeColor; }
				set
				{
					mStrokeColor = value;
				}
			}

			/// <summary>
			/// Цвет заливки при рисовании заполненных примитивов
			/// </summary>
			/// <remarks>
			/// Свойство должно устанавливаться непосредственно перед рисованием примитива
			/// </remarks>
			public static Color FillColor
			{
				get { return mFillColor; }
				set
				{
					mFillColor = value;
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация рендера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
				// Создаем материалы по умолчанию
				if (mColoredMaterial2D == null)
				{
					mColoredMaterial2D = new Material(Shader.Find(ShaderColorBlend));
				}

				if (mTexturedMaterial2D == null)
				{
					mTexturedMaterial2D = new Material(Shader.Find(ShaderTextureBlend));
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ПРИМИТИВАМИ ===============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение примитива по его идентификатору
			/// </summary>
			/// <param name="id">Идентификатор примитива</param>
			/// <returns>Найденный примитив или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CPrimitive2D GetPrimitive2D(Int32 id)
			{
				CPrimitive2D primitive = null;
				Primitives.TryGetValue(id, out primitive);
				return primitive;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление существующего примитива
			/// </summary>
			/// <param name="id">Идентификатор примитива</param>
			/// <param name="primitive">Примитив</param>
			/// <returns>Статус успешности добавления примитива</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean AddPrimitive2D(Int32 id, CPrimitive2D primitive)
			{
				if (!Primitives.ContainsKey(id))
				{
					Primitives.Add(id, primitive);
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление примитива по его идентификатору
			/// </summary>
			/// <param name="id">Идентификатор примитива</param>
			/// <returns>Статус успешности удаления примитива</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean RemovePrimitive2D(Int32 id)
			{
				if (Primitives.ContainsKey(id))
				{
					Primitives.Remove(id);
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование примитива
			/// </summary>
			/// <param name="id">Идентификатор примитива</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawPrimitive2D(Int32 id)
			{
				CPrimitive2D primitive = null;
				if (Primitives.TryGetValue(id, out primitive))
				{
					primitive.Draw();
				}
			}
			#endregion

			#region ======================================= ГЕНЕРАЦИИ 2D ПРИМИТИВОВ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация штриховой линии
			/// </summary>
			/// <param name="id">Идентификатор линии</param>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			/// <returns>Статус успешности добавления примитива</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GenerateLineSegment(Int32 id, Vector2 start, Vector2 end, Int32 segment_size, Single space_size = 0.5f)
			{
				if (Primitives.ContainsKey(id)) return false;

				CPrimitiveLine2D line = new CPrimitiveLine2D(start, end);
				line.ID = id;
				line.GenerateSegment(segment_size, space_size);
				Primitives.Add(id, line);

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация угла образованного двумя штриховыми линиями
			/// </summary>
			/// <param name="id">Идентификатор угла</param>
			/// <param name="start">Начальная точка угла</param>
			/// <param name="pivot">Опорная точка угла</param>
			/// <param name="end">Конечная точка угла</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			/// <returns>Статус успешности добавления примитива</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GenerateAngleSegment(Int32 id, Vector2 start, Vector2 pivot, Vector2 end, Int32 segment_size,
				Single space_size = 0.5f)
			{
				if (Primitives.ContainsKey(id)) return false;

				CPrimitiveAngle2D angle = new CPrimitiveAngle2D(start, pivot, end);
				angle.ID = id;
				angle.GenerateSegment(segment_size, space_size);
				Primitives.Add(id, angle);

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация угла образованного двумя линиями указанной толщиной 
			/// </summary>
			/// <param name="id">Идентификатор угла</param>
			/// <param name="start">Начальная точка</param>
			/// <param name="pivot">Опорная точка угла</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="thickness">Толщина линии</param>
			/// <returns>Статус успешности добавления примитива</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GenerateAngleThickness(Int32 id, Vector2 start, Vector2 pivot, Vector2 end, Single thickness)
			{
				if (Primitives.ContainsKey(id)) return false;

				CPrimitiveAngle2D angle = new CPrimitiveAngle2D(start, pivot, end);
				angle.ID = id;
				angle.GenerateThickness(thickness);
				Primitives.Add(id, angle);

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация треугольника со штриховыми линиями
			/// </summary>
			/// <param name="id">Идентификатор треугольника</param>
			/// <param name="p1">Первая точка</param>
			/// <param name="p2">Вторая точка</param>
			/// <param name="p3">Третья точка</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			/// <returns>Статус успешности добавления примитива</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GenerateTriangleSegment(Int32 id, Vector2 p1, Vector2 p2, Vector2 p3, Int32 segment_size,
				Single space_size = 0.5f)
			{
				if (Primitives.ContainsKey(id)) return false;

				CPrimitiveTriangle2D triangle = new CPrimitiveTriangle2D(p1, p2, p3);
				triangle.ID = id;
				triangle.GenerateSegment(segment_size, space_size);
				Primitives.Add(id, triangle);

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация прямоугольника со штриховыми линиями
			/// </summary>
			/// <param name="id">Идентификатор прямоугольника</param>
			/// <param name="rect">Прямоугольник</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			/// <returns>Статус успешности добавления примитива</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GenerateRectSegment(Int32 id, Rect rect, Int32 segment_size, Single space_size = 0.5f)
			{
				if (Primitives.ContainsKey(id)) return false;

				CPrimitiveRect2D rectangle = new CPrimitiveRect2D(rect);
				rectangle.ID = id;
				rectangle.GenerateSegment(segment_size, space_size);
				Primitives.Add(id, rectangle);

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация окружности
			/// </summary>
			/// <param name="id">Идентификатор окружности</param>
			/// <param name="center">Центр окружности</param>
			/// <param name="radius">Радиус окружности</param>
			/// <returns>Статус успешности добавления примитива</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GenerateCircle(Int32 id, Vector2 center, Single radius)
			{
				if (Primitives.ContainsKey(id)) return false;

				CPrimitiveEllipse2D circle = new CPrimitiveEllipse2D(center, radius, radius);
				circle.ID = id;
				Primitives.Add(id, circle);

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация окружности со штриховыми линиями
			/// </summary>
			/// <param name="id">Идентификатор окружности</param>
			/// <param name="center">Центр окружности</param>
			/// <param name="radius">Радиус окружности</param>
			/// <param name="angle_offset">Начальное смещение угла</param>
			/// <returns>Статус успешности добавления примитива</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GenerateCircleSegment(Int32 id, Vector2 center, Single radius, Single angle_offset)
			{
				if (Primitives.ContainsKey(id)) return false;

				CPrimitiveEllipse2D circle = new CPrimitiveEllipse2D(center, radius, radius);
				circle.GenerateSegment(angle_offset);
				circle.ID = id;
				Primitives.Add(id, circle);

				return true;
			}
			#endregion

			#region ======================================= РИСОВАНИЕ Line ============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование линии
			/// </summary>
			/// <param name="start_x">Координата начальной точки по X</param>
			/// <param name="start_y">Координата начальной точки по Y</param>
			/// <param name="end_x">Координата конечной точки по X</param>
			/// <param name="end_y">Координата конечной точки по Y</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawLine(Single start_x, Single start_y, Single end_x, Single end_y)
			{
				Vector2 start = new Vector2(start_x, start_y);
				Vector2 end = new Vector2(end_x, end_y);
				DrawLine(ref start, ref end);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование линии
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawLine(Vector2 start, Vector2 end)
			{
				DrawLine(ref start, ref end);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование линии
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawLine(ref Vector2 start, ref Vector2 end)
			{
				Vector2 p1 = new Vector2(start.x, Screen.height - start.y);
				Vector2 p2 = new Vector2(end.x, Screen.height - end.y);

				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINES);
				{
					GL.Color(mStrokeColor);
					GL.Vertex(p1);
					GL.Vertex(p2);
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование линии
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="color_start">Начальный цвет</param>
			/// <param name="color_end">Конечный цвет</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawLine(Vector2 start, Vector2 end, Color color_start, Color color_end)
			{
				DrawLine(ref start, ref end, ref color_start, ref color_end);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование линии
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="color_start">Начальный цвет</param>
			/// <param name="color_end">Конечный цвет</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawLine(ref Vector2 start, ref Vector2 end, ref Color color_start, ref Color color_end)
			{
				Vector2 p1 = new Vector2(start.x, Screen.height - start.y);
				Vector2 p2 = new Vector2(end.x, Screen.height - end.y);

				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINES);
				{
					GL.Color(color_start);
					GL.Vertex(p1);

					GL.Color(color_end);
					GL.Vertex(p2);
				}
				GL.End();
				GL.PopMatrix();
			}
			#endregion

			#region ======================================= РИСОВАНИЕ Angle ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование угла образованного двумя линиями
			/// </summary>
			/// <param name="start">Начальная точка угла</param>
			/// <param name="pivot">Опорная точка угла</param>
			/// <param name="end">Конечная точка угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawAngle(Vector2 start, Vector2 pivot, Vector2 end)
			{
				DrawAngle(ref start, ref pivot, ref end);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование угла образованного двумя линиями
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="pivot">Опорная точка угла</param>
			/// <param name="end">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawAngle(ref Vector2 start, ref Vector2 pivot, ref Vector2 end)
			{
				Vector2 p1 = new Vector2(start.x, Screen.height - start.y);
				Vector2 p2 = new Vector2(pivot.x, Screen.height - pivot.y);
				Vector2 p3 = new Vector2(end.x, Screen.height - end.y);

				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINE_STRIP);
				{
					GL.Color(mStrokeColor);
					GL.Vertex(p1);
					GL.Vertex(p2);
					GL.Vertex(p3);
				}
				GL.End();
				GL.PopMatrix();
			}
			#endregion

			#region ======================================= РИСОВАНИЕ Triangle ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование треугольника
			/// </summary>
			/// <param name="p1">Первая точка</param>
			/// <param name="p2">Вторая точка</param>
			/// <param name="p3">Третья точка</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTriangle(Vector2 p1, Vector2 p2, Vector2 p3)
			{
				DrawTriangle(ref p1, ref p2, ref p3);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование треугольника
			/// </summary>
			/// <param name="p1">Первая точка</param>
			/// <param name="p2">Вторая точка</param>
			/// <param name="p3">Третья точка</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTriangle(ref Vector2 p1, ref Vector2 p2, ref Vector2 p3)
			{
				Vector2 rp1 = new Vector2(p1.x, Screen.height - p1.y);
				Vector2 rp2 = new Vector2(p2.x, Screen.height - p2.y);
				Vector2 rp3 = new Vector2(p3.x, Screen.height - p3.y);

				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINE_STRIP);
				{
					GL.Color(mStrokeColor);
					GL.Vertex(rp1);
					GL.Vertex(rp2);
					GL.Vertex(rp3);
					GL.Vertex(rp1);
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование заполненного треугольника
			/// </summary>
			/// <param name="p1">Первая точка</param>
			/// <param name="p2">Вторая точка</param>
			/// <param name="p3">Третья точка</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTriangleFill(Vector2 p1, Vector2 p2, Vector2 p3)
			{
				DrawTriangleFill(ref p1, ref p2, ref p3);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование заполненного треугольника
			/// </summary>
			/// <param name="p1">Первая точка</param>
			/// <param name="p2">Вторая точка</param>
			/// <param name="p3">Третья точка</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTriangleFill(ref Vector2 p1, ref Vector2 p2, ref Vector2 p3)
			{
				Vector2 rp1 = new Vector2(p1.x, Screen.height - p1.y);
				Vector2 rp2 = new Vector2(p2.x, Screen.height - p2.y);
				Vector2 rp3 = new Vector2(p3.x, Screen.height - p3.y);

				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.TRIANGLES);
				{
					GL.Color(mFillColor);
					GL.Vertex(rp1);
					GL.Vertex(rp2);
					GL.Vertex(rp3);
				}
				GL.End();
				GL.PopMatrix();
			}
			#endregion

			#region ======================================= РИСОВАНИЕ Rect ============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование прямоугольника
			/// </summary>
			/// <param name="rect">Прямоугольник</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawRect(Rect rect)
			{
				DrawRect(ref rect);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование прямоугольника
			/// </summary>
			/// <param name="rect">Прямоугольник</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawRect(ref Rect rect)
			{
				Vector2 p1 = new Vector2(rect.x, Screen.height - rect.y);
				Vector2 p2 = new Vector2(rect.xMax, Screen.height - rect.y);
				Vector2 p3 = new Vector2(rect.xMax, Screen.height - rect.yMax);
				Vector2 p4 = new Vector2(rect.x, Screen.height - rect.yMax);

				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINES);
				{
					GL.Color(mStrokeColor);

					GL.Vertex(p1);
					GL.Vertex(p2);

					GL.Vertex(p2);
					GL.Vertex(p3);

					GL.Vertex(p3);
					GL.Vertex(p4);

					GL.Vertex(p4);
					GL.Vertex(p1);
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование заполненного прямоугольника
			/// </summary>
			/// <param name="rect">Прямоугольник</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawRectFill(Rect rect)
			{
				DrawRectFill(ref rect);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование заполненного прямоугольника
			/// </summary>
			/// <param name="rect">Прямоугольник</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawRectFill(ref Rect rect)
			{
				Vector2 p1 = new Vector2(rect.x, Screen.height - rect.y);
				Vector2 p2 = new Vector2(rect.xMax, Screen.height - rect.y);
				Vector2 p3 = new Vector2(rect.xMax, Screen.height - rect.yMax);
				Vector2 p4 = new Vector2(rect.x, Screen.height - rect.yMax);

				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.TRIANGLES);
				{
					GL.Color(mFillColor);
					GL.Vertex(p1);
					GL.Vertex(p2);
					GL.Vertex(p3);

					GL.Vertex(p1);
					GL.Vertex(p3);
					GL.Vertex(p4);
				}

				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование заполненного прямоугольника
			/// </summary>
			/// <param name="rect">Прямоугольник</param>
			/// <param name="color_lt">Цвет левого верхнего угла</param>
			/// <param name="color_rt">Цвет правого верхнего угла</param>
			/// <param name="color_rb">Цвет правого нижнего угла</param>
			/// <param name="color_lb">Цвет левого нижнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawRectFill(Rect rect, Color color_lt, Color color_rt, Color color_rb, Color color_lb)
			{
				DrawRectFill(ref rect, ref color_lt, ref color_rt, ref color_rb, ref color_lb);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование заполненного прямоугольника
			/// </summary>
			/// <param name="rect">Прямоугольник</param>
			/// <param name="color_lt">Цвет левого верхнего угла</param>
			/// <param name="color_rt">Цвет правого верхнего угла</param>
			/// <param name="color_rb">Цвет правого нижнего угла</param>
			/// <param name="color_lb">Цвет левого нижнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawRectFill(ref Rect rect, ref Color color_lt, ref Color color_rt, ref Color color_rb, ref Color color_lb)
			{
				Vector2 p1 = new Vector2(rect.x, Screen.height - rect.y);
				Vector2 p2 = new Vector2(rect.xMax, Screen.height - rect.y);
				Vector2 p3 = new Vector2(rect.xMax, Screen.height - rect.yMax);
				Vector2 p4 = new Vector2(rect.x, Screen.height - rect.yMax);

				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.TRIANGLES);
				{
					GL.Color(color_lt);
					GL.Vertex(p1);
					GL.Color(color_rt);
					GL.Vertex(p2);
					GL.Color(color_rb);
					GL.Vertex(p3);

					GL.Color(color_lt);
					GL.Vertex(p1);
					GL.Color(color_rb);
					GL.Vertex(p3);
					GL.Color(color_lb);
					GL.Vertex(p4);
				}

				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование каркаса прямоугольника представляющего рамку выбора
			/// </summary>
			/// <param name="rect">Прямоугольник</param>
			/// <param name="color">Цвет каркаса</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawRectSelection(ref Rect rect, ref Color color)
			{
				Vector2 p1 = new Vector2(rect.x, Screen.height - rect.y);
				Vector2 p2 = new Vector2(rect.xMax, Screen.height - rect.y);
				Vector2 p3 = new Vector2(rect.xMax, Screen.height - rect.yMax);
				Vector2 p4 = new Vector2(rect.x, Screen.height - rect.yMax);

				Vector2 p1_outer = new Vector2(rect.x - 1, Screen.height - (rect.y - 1));
				Vector2 p2_outer = new Vector2(rect.xMax + 1, Screen.height - (rect.y - 1));
				Vector2 p3_outer = new Vector2(rect.xMax + 1, Screen.height - (rect.yMax + 1));
				Vector2 p4_outer = new Vector2(rect.x - 1, Screen.height - (rect.yMax + 1));

				Vector2 p1_inner = new Vector2(rect.x + 1, Screen.height - (rect.y + 1));
				Vector2 p2_inner = new Vector2(rect.xMax - 1, Screen.height - (rect.y + 1));
				Vector2 p3_inner = new Vector2(rect.xMax - 1, Screen.height - (rect.yMax - 1));
				Vector2 p4_inner = new Vector2(rect.x + 1, Screen.height - (rect.yMax - 1));

				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINES);
				{

					GL.Color(color);

					// Основная рамка
					GL.Vertex(p1);
					GL.Vertex(p2);

					GL.Vertex(p2);
					GL.Vertex(p3);

					GL.Vertex(p3);
					GL.Vertex(p4);

					GL.Vertex(p4);
					GL.Vertex(p1);

					// внешняя рамка
					GL.Color(new Color(color.r, color.g, color.b, 0.3f));

					GL.Vertex(p1_outer);
					GL.Vertex(p2_outer);

					GL.Vertex(p2_outer);
					GL.Vertex(p3_outer);


					GL.Vertex(p3_outer);
					GL.Vertex(p4_outer);

					GL.Vertex(p4_outer);
					GL.Vertex(p1_outer);


					// внутренняя рамка
					GL.Color(new Color(color.r, color.g, color.b, 0.3f));

					GL.Vertex(p1_inner);
					GL.Vertex(p2_inner);

					GL.Vertex(p2_inner);
					GL.Vertex(p3_inner);


					GL.Vertex(p3_inner);
					GL.Vertex(p4_inner);

					GL.Vertex(p4_inner);
					GL.Vertex(p1_inner);
				}
				GL.End();
				GL.PopMatrix();
			}
			#endregion

			#region ======================================= РИСОВАНИЕ Polyline ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование списка линий
			/// </summary>
			/// <param name="list_points">Список точек</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawPolyline(IList<Vector2> list_points)
			{
				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINES);
				{
					GL.Color(mStrokeColor);
					for (Int32 i = 1; i < list_points.Count; i++)
					{
						Vector2 p1 = new Vector2(list_points[i - 1].x, Screen.height - list_points[i - 1].y);
						Vector2 p2 = new Vector2(list_points[i].x, Screen.height - list_points[i].y);
						GL.Vertex(p1);
						GL.Vertex(p2);

					}
				}
				GL.End();
				GL.PopMatrix();

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование списка линий с поочередной сменой цвета
			/// </summary>
			/// <param name="list_points">Список точек</param>
			/// <param name="alternative">Альтернативный цвет линий</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawPolyline(IList<Vector2> list_points, Color alternative)
			{
				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINES);
				{
					for (Int32 i = 1; i < list_points.Count; i++)
					{
						Vector2 p1 = new Vector2(list_points[i - 1].x, Screen.height - list_points[i - 1].y);
						Vector2 p2 = new Vector2(list_points[i].x, Screen.height - list_points[i].y);

						if (i % 2 == 0)
						{
							GL.Color(mStrokeColor);
							GL.Vertex(p1);
							GL.Vertex(p2);
						}
						else
						{
							GL.Color(alternative);
							GL.Vertex(p1);
							GL.Vertex(p2);
						}
					}
				}
				GL.End();
				GL.PopMatrix();

			}
			#endregion

			#region ======================================= РИСОВАНИЕ Polygon =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование замкнутого списка линий
			/// </summary>
			/// <param name="list_points">Список точек</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawPolygon(IList<Vector2> list_points)
			{
				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINES);
				{
					GL.Color(mStrokeColor);
					for (Int32 i = 1; i < list_points.Count; i++)
					{
						Vector2 p1 = new Vector2(list_points[i - 1].x, Screen.height - list_points[i - 1].y);
						Vector2 p2 = new Vector2(list_points[i].x, Screen.height - list_points[i].y);
						GL.Vertex(p1);
						GL.Vertex(p2);

					}

					GL.Vertex(new Vector2(list_points[list_points.Count - 1].x, Screen.height - list_points[list_points.Count - 1].y));
					GL.Vertex(new Vector2(list_points[0].x, Screen.height - list_points[0].y));
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование замкнутого списка линий с поочередной сменой цвета
			/// </summary>
			/// <param name="list_points">Список точек</param>
			/// <param name="alternative">Альтернативный цвет линий</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawPolygon(IList<Vector2> list_points, Color alternative)
			{
				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINES);
				{
					for (Int32 i = 1; i < list_points.Count; i++)
					{
						Vector2 p1 = new Vector2(list_points[i - 1].x, Screen.height - list_points[i - 1].y);
						Vector2 p2 = new Vector2(list_points[i].x, Screen.height - list_points[i].y);

						if (i % 2 == 0)
						{
							GL.Color(mStrokeColor);
							GL.Vertex(p1);
							GL.Vertex(p2);
						}
						else
						{
							GL.Color(alternative);
							GL.Vertex(p1);
							GL.Vertex(p2);
						}

					}

					if (list_points.Count % 2 == 0)
					{
						GL.Color(mStrokeColor);
						GL.Vertex(new Vector2(list_points[list_points.Count - 1].x, Screen.height - list_points[list_points.Count - 1].y));
						GL.Vertex(new Vector2(list_points[0].x, Screen.height - list_points[0].y));
					}
					else
					{
						GL.Color(alternative);
						GL.Vertex(new Vector2(list_points[list_points.Count - 1].x, Screen.height - list_points[list_points.Count - 1].y));
						GL.Vertex(new Vector2(list_points[0].x, Screen.height - list_points[0].y));
					}
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование замкнутого заполненного списка линий
			/// </summary>
			/// <param name="list_points">Список точек</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawPolygonFill(IList<Vector2> list_points)
			{
				Vector2 first = new Vector2(list_points[0].x, Screen.height - list_points[0].y);
				ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.TRIANGLES);
				{
					GL.Color(mFillColor);
					for (Int32 i = 1; i < list_points.Count; i++)
					{
						Vector2 p1 = new Vector2(list_points[i - 1].x, Screen.height - list_points[i - 1].y);
						Vector2 p2 = new Vector2(list_points[i].x, Screen.height - list_points[i].y);
						GL.Vertex(first);
						GL.Vertex(p1);
						GL.Vertex(p2);
					}

					GL.Vertex(new Vector2(list_points[list_points.Count - 1].x, Screen.height - list_points[list_points.Count - 1].y));
					GL.Vertex(new Vector2(list_points[0].x, Screen.height - list_points[0].y));
				}
				GL.End();
				GL.PopMatrix();
			}
			#endregion

			#region ======================================= РИСОВАНИЕ Texture =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование текстуры
			/// </summary>
			/// <param name="x">Позиция текстуры по X</param>
			/// <param name="y">Позиция текстуры по Y</param>
			/// <param name="texture">Текстура</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTexture(Int32 x, Int32 y, Texture texture)
			{
				Rect rect = new Rect(x, y, texture.width, texture.height);
				DrawTexture(ref rect, texture, ref ColorWhite);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование текстуры
			/// </summary>
			/// <param name="screen_rect">Прямоугольник вывода</param>
			/// <param name="texture">Текстура</param>
			/// <param name="color">Цвет модуляции</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTexture(Rect screen_rect, Texture texture, Color color)
			{
				DrawTexture(ref screen_rect, texture, ref color);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование текстуры
			/// </summary>
			/// <param name="screen_rect">Прямоугольник вывода</param>
			/// <param name="texture">Текстура</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTexture(Rect screen_rect, Texture texture)
			{
				DrawTexture(ref screen_rect, texture, ref ColorWhite);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование текстуры
			/// </summary>
			/// <param name="screen_rect">Прямоугольник вывода</param>
			/// <param name="texture">Текстура</param>
			/// <param name="color">Цвет модуляции</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTexture(ref Rect screen_rect, Texture texture, ref Color color)
			{
				Vector2 p1 = new Vector2(screen_rect.x, Screen.height - screen_rect.y);
				Vector2 p2 = new Vector2(screen_rect.xMax, Screen.height - screen_rect.y);
				Vector2 p3 = new Vector2(screen_rect.xMax, Screen.height - screen_rect.yMax);
				Vector2 p4 = new Vector2(screen_rect.x, Screen.height - screen_rect.yMax);

				TexturedMaterial2D.mainTexture = texture;
				TexturedMaterial2D.color = color;
				TexturedMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.TRIANGLES);
				{
					GL.TexCoord(MapUV_TopLeft);
					GL.Vertex(p1);

					GL.TexCoord(MapUV_TopRight);
					GL.Vertex(p2);

					GL.TexCoord(MapUV_BottomRight);
					GL.Vertex(p3);


					GL.TexCoord(MapUV_TopLeft);
					GL.Vertex(p1);

					GL.TexCoord(MapUV_BottomRight);
					GL.Vertex(p3);

					GL.TexCoord(MapUV_BottomLeft);
					GL.Vertex(p4);
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование текстуры с указанной степенью прозрачности
			/// </summary>
			/// <param name="screen_rect">Прямоугольник вывода</param>
			/// <param name="texture">Текстура</param>
			/// <param name="alpha">Степень прозрачности текстуры</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTextureAlpha(Rect screen_rect, Texture texture, Single alpha)
			{
				DrawTextureAlpha(ref screen_rect, texture, alpha);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование текстуры с указанной степенью прозрачности
			/// </summary>
			/// <param name="screen_rect">Прямоугольник вывода</param>
			/// <param name="texture">Текстура</param>
			/// <param name="alpha">Степень прозрачности текстуры</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTextureAlpha(ref Rect screen_rect, Texture texture, Single alpha)
			{
				Vector2 p1 = new Vector2(screen_rect.x, Screen.height - screen_rect.y);
				Vector2 p2 = new Vector2(screen_rect.xMax, Screen.height - screen_rect.y);
				Vector2 p3 = new Vector2(screen_rect.xMax, Screen.height - screen_rect.yMax);
				Vector2 p4 = new Vector2(screen_rect.x, Screen.height - screen_rect.yMax);

				TexturedMaterial2D.mainTexture = texture;
				TexturedMaterial2D.color = new Color(1, 1, 1, alpha);
				TexturedMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.TRIANGLES);
				{
					GL.TexCoord(MapUV_TopLeft);
					GL.Vertex(p1);

					GL.TexCoord(MapUV_TopRight);
					GL.Vertex(p2);

					GL.TexCoord(MapUV_BottomRight);
					GL.Vertex(p3);

					GL.TexCoord(MapUV_TopLeft);
					GL.Vertex(p1);

					GL.TexCoord(MapUV_BottomRight);
					GL.Vertex(p3);

					GL.TexCoord(MapUV_BottomLeft);
					GL.Vertex(p4);
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование текстуры
			/// </summary>
			/// <param name="screen_rect">Прямоугольник вывода</param>
			/// <param name="texture">Текстура</param>
			/// <param name="angle">Угол поворота в градусах </param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTextureRotate(Rect screen_rect, Texture texture, Single angle)
			{
				DrawTextureRotate(ref screen_rect, texture, ref ColorWhite, angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование текстуры
			/// </summary>
			/// <param name="screen_rect">Прямоугольник вывода</param>
			/// <param name="texture">Текстура</param>
			/// <param name="color">Цвет модуляции</param>
			/// <param name="angle">Угол поворота в градусах </param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTextureRotate(Rect screen_rect, Texture texture, Color color, Single angle)
			{
				DrawTextureRotate(ref screen_rect, texture, ref color, angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование текстуры
			/// </summary>
			/// <param name="screen_rect">Прямоугольник вывода</param>
			/// <param name="texture">Текстура</param>
			/// <param name="color">Цвет модуляции</param>
			/// <param name="angle">Угол поворота в градусах </param>
			//---------------------------------------------------------------------------------------------------------
			public static void DrawTextureRotate(ref Rect screen_rect, Texture texture, ref Color color, Single angle)
			{
				// 1---------2
				// |         |
				// |         |
				// 4---------3
				//
				Vector2 center = screen_rect.center;
				Vector2 p1 = new Vector2(screen_rect.x, screen_rect.y) - center;
				Vector2 p2 = new Vector2(screen_rect.xMax, screen_rect.y) - center;
				Vector2 p3 = new Vector2(screen_rect.xMax, screen_rect.yMax) - center;
				Vector2 p4 = new Vector2(screen_rect.x, screen_rect.yMax) - center;

				Single cos = Mathf.Cos(angle * Mathf.Deg2Rad);
				Single sin = Mathf.Sin(angle * Mathf.Deg2Rad);

				Vector2 tp1 = new Vector2(p1.x * cos - p1.y * sin, p1.x * sin + p1.y * cos) + center;
				tp1.y = Screen.height - tp1.y;
				Vector2 tp2 = new Vector2(p2.x * cos - p2.y * sin, p2.x * sin + p2.y * cos) + center;
				tp2.y = Screen.height - tp2.y;
				Vector2 tp3 = new Vector2(p3.x * cos - p3.y * sin, p3.x * sin + p3.y * cos) + center;
				tp3.y = Screen.height - tp3.y;
				Vector2 tp4 = new Vector2(p4.x * cos - p4.y * sin, p4.x * sin + p4.y * cos) + center;
				tp4.y = Screen.height - tp4.y;

				TexturedMaterial2D.mainTexture = texture;
				TexturedMaterial2D.color = color;
				TexturedMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.TRIANGLES);
				{
					GL.TexCoord(MapUV_TopLeft);
					GL.Vertex(tp1);

					GL.TexCoord(MapUV_TopRight);
					GL.Vertex(tp2);

					GL.TexCoord(MapUV_BottomRight);
					GL.Vertex(tp3);


					GL.TexCoord(MapUV_TopLeft);
					GL.Vertex(tp1);

					GL.TexCoord(MapUV_BottomRight);
					GL.Vertex(tp3);

					GL.TexCoord(MapUV_BottomLeft);
					GL.Vertex(tp4);
				}
				GL.End();
				GL.PopMatrix();

			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================