//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Подсистема геометрических примитивов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIPrimitiveBase.cs
*		Определение данных для отображения геометрических двухмерных примитивов.
*		Определение основных типов данных для отображения каркасных и заполненных геометрических двухмерных примитивов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DImmedateGUIPrimitive Подсистема геометрических примитивов
		//! Подсистема геометрических двухмерных примитивов обеспечивает отображения базовых геометрических примитивов. 
		//! Подсистема поддерживает рисование примитивов с учетом толщины и штриха линий, простым сглаживанием. 
		//! Подсистема делиться на две части: генерация примитивов и их отображение, в целях производительности напрямую 
		//! рисуются только простые примитивы, остальные примитивы должны быть сначала сгенерированы для отображения
		//! \ingroup Unity2DImmedateGUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс для генерации и хранения геометрии примитива
		/// </summary>
		/// <remarks>
		/// В классе предусмотрены только данные для генерации примитива, данные используемые при отображении 
		/// примитива берутся от рендера <see cref="XGUIRender"/>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CPrimitive2D
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка пересечения двух линий
			/// </summary>
			/// <param name="line_start_1">Начало первой линии</param>
			/// <param name="line_end_1">Конец первой линии</param>
			/// <param name="line_start_2">Начало второй линии</param>
			/// <param name="line_end_2">Конец второй линии</param>
			/// <param name="result">Точка пересечения</param>
			/// <returns>Статус пересечения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 LineToLine(ref Vector2 line_start_1, ref Vector2 line_end_1, ref Vector2 line_start_2, 
				ref Vector2 line_end_2, ref Vector2 result)
			{
				Single x1 = line_start_1.x;
				Single y1 = line_start_1.y;

				Single x2 = line_end_1.x;
				Single y2 = line_end_1.y;

				Single x3 = line_start_2.x;
				Single y3 = line_start_2.y;

				Single x4 = line_end_2.x;
				Single y4 = line_end_2.y;

				// Проверяем параллельность
				Single d = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
				if (Mathf.Approximately(d, 0))
				{
					return -1;
				}

				Single qx = (x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4);
				Single qy = (x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4);

				Single point_x = qx / d;
				Single point_y = qy / d;

				// Проверяем что бы эта точка попала в области отрезков
				Single ddx = x2 - x1;
				Single tx = 0.5f;
				if (!Mathf.Approximately(ddx, 0))
				{
					tx = (point_x - x1) / ddx;
				}

				Single ty = 0.5f;
				Single ddy = y2 - y1;
				if (!Mathf.Approximately(ddy, 0))
				{
					ty = (point_y - y1) / ddy;
				}

				if (tx < 0 || tx > 1 || ty < 0 || ty > 1)
				{
					result = new Vector2(point_x, point_y);
					return 0;
				}

				result = new Vector2(point_x, point_y);

				return 1;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			internal Int32 mID;
			internal Vector2[] mPoints;
			internal Int32 mCountPoint;
			internal Int32 mModeDraw;
			internal TCornerMode mCornerMode;
			internal Boolean mIsClosed;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Идентификатор примитива
			/// </summary>
			/// <remarks>
			/// Идентификатор примитива используется для получения примитива из словаря, должен быть уникальный в пределах контекста использования
			/// </remarks>
			public Int32 ID
			{
				get { return mID; }
				set { mID = value; }
			}

			/// <summary>
			/// Массив точек
			/// </summary>
			public Vector2[] Points
			{
				get { return mPoints; }
				set
				{
					mPoints = value;
				}
			}

			/// <summary>
			/// Количество точек для рисования
			/// </summary>
			/// <remarks>
			/// Если значение меньше 1 то используется размер массива
			/// Также используется для анимации рисования
			/// </remarks>
			public Int32 CountPoint
			{
				get { return mCountPoint; }
				set
				{
					mCountPoint = value;
				}
			}

			/// <summary>
			/// Режим рисования точек
			/// </summary>
			/// <remarks>
			/// Режим рисования определяет каким образом будут интерпретированы данные и как будут нарисованы
			/// </remarks>
			public Int32 ModeDraw
			{
				get { return mModeDraw; }
				set
				{
					mModeDraw = value;
				}
			}

			/// <summary>
			/// Режим обработки углов
			/// </summary>
			public TCornerMode CornerMode
			{
				get { return mCornerMode; }
				set
				{
					mCornerMode = value;
				}
			}

			/// <summary>
			/// Статус замкнутости примитива
			/// </summary>
			public Boolean IsClosed
			{
				get
				{
					return mIsClosed;
				}
				set
				{
					mIsClosed = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitive2D()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="count">Количество точек</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitive2D(Int32 count)
			{
				mPoints = new Vector2[count];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="points">Массив точек</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitive2D(Vector2[] points)
			{
				mPoints = points;
			}
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация данных о простой штриховой линии в массиве
			/// </summary>
			/// <param name="offset">Смещение в массиве точек</param>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			/// <returns>Текущие смещение в массиве</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Int32 GenerateSegment(Int32 offset, Vector2 start, Vector2 end, Int32 segment_size, Single space_size = 0.5f)
			{
				start = new Vector2(start.x, Screen.height - start.y);
				end = new Vector2(end.x, Screen.height - end.y);

				return GenerateSegmentNotTransform(offset, ref start, ref end, segment_size, space_size);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация данных о простой штриховой линии в массиве
			/// </summary>
			/// <param name="offset">Смещение в массиве точек</param>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			/// <returns>Текущие смещение в массиве</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Int32 GenerateSegmentNotTransform(Int32 offset, ref Vector2 start, ref Vector2 end, Int32 segment_size, Single space_size = 0.5f)
			{
				// Длина
				Single length = (end - start).magnitude;

				// Количество сегментов линии
				Int32 count_segment = (Int32)length / segment_size;

				// Есть ли остаток (неполный отрезок)
				Single rest = length / segment_size - count_segment;

				// Увеличиваем размер массива
				Resize(count_segment * 2 + offset);

				// Вектор направления по сегментно
				Vector2 dir = (end - start) / length;

				// Ограничиваем относительный размер пробела
				space_size = Mathf.Clamp(space_size, 0, 1);

				Vector2 current = start;
				for (Int32 i = 0; i < count_segment; i++)
				{
					mPoints[offset] = current;
					current += dir * ((Single)segment_size * (1 - space_size));
					offset++;

					mPoints[offset] = current;
					current += dir * ((Single)segment_size * space_size);
					offset++;
				}

				// Заполняем остаток
				if (Mathf.Abs(rest) * segment_size > 4)
				{
					// Увеличиваем размер массива
					Resize(offset + 2);

					mPoints[offset] = current;
					offset++;

					mPoints[offset] = end;
					offset++;
				}

				return offset;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация данных о линии указанной толщины в массиве
			/// </summary>
			/// <param name="offset">Смещение в массиве точек</param>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="is_last_line">Статус последней линии</param>
			/// <returns>Текущие смещение в массиве</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Int32 GenerateThicknessLine(Int32 offset, Vector2 start, Vector2 end, Single thickness, 
				Boolean is_last_line = false)
			{
				start = new Vector2(start.x, Screen.height - start.y);
				end = new Vector2(end.x, Screen.height - end.y);

				return GenerateThicknessLineNotTransform(offset, ref start, ref end, thickness, is_last_line);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация данных о линии указанной толщины в массиве
			/// </summary>
			/// <param name="offset">Смещение в массиве точек</param>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="is_last_line">Статус последней линии</param>
			/// <returns>Текущие смещение в массиве</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Int32 GenerateThicknessLineNotTransform(Int32 offset, ref Vector2 start, ref Vector2 end, 
				Single thickness, Boolean is_last_line)
			{
				// Вычисляем направление и длину
				Single dx1 = end.x - start.x;
				Single dy1 = end.y - start.y;
				Single len1 = Mathf.Sqrt(dx1 * dx1 + dy1 * dy1);
				Vector2 dir1 = new Vector2(dx1 / len1, dy1 / len1);

				// Толщина линии
				Single wdx1 = thickness * dir1.x;
				Single wdy1 = thickness * dir1.y;

				// Вектор смещения
				Vector2 delta1 = new Vector2(-wdy1, wdx1);

				// Линия образуется прямоугольником вершины которого имеют следующую нумерацию
				// 1---------2
				// |         |
				// -----------
				// |         |
				// 4---------3

				// Прямоугольник
				mPoints[offset] = new Vector2(start.x + delta1.x, start.y + delta1.y);
				mPoints[offset + 1] = new Vector2(end.x + delta1.x, end.y + delta1.y);
				mPoints[offset + 2] = new Vector2(end.x - delta1.x, end.y - delta1.y);
				mPoints[offset + 3] = new Vector2(start.x - delta1.x, start.y - delta1.y);

				// Сопрягаем отдельные линии если установлен соответствующий режим
				if (offset > 3 && mCornerMode > 0)
				{
					// Расположения предыдущей линии
					Int32 index = offset - 4;
					if (mCornerMode == TCornerMode.Bevel) index -= 3;

					CornerTreatment(index);

					// Если это последняя линия то нам надо сделать сопряжение с первой
					if (is_last_line)
					{
						index = offset;
						CornerTreatment(index);
					}
				}

				// Смещаем на четыре вершины которые образуют прямоугольник линии
				offset += 4;

				// Если есть дополнительные треугольники на сопряжение то смещаем еще
				if (is_last_line == false)
				{
					if (mCornerMode == TCornerMode.Bevel) offset += 3;
				}
				else
				{
					// Смещаем только ещё раз если только у нас примитив замкнут
					if (mCornerMode == TCornerMode.Bevel && mIsClosed) offset += 3;
				}

				return offset;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация данных о штриховой линии указанной толщины в массиве
			/// </summary>
			/// <param name="offset">Смещение в массиве точек</param>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			/// <returns>Текущие смещение в массиве</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Int32 GenerateSegmentThicknessLine(Int32 offset, Vector2 start, Vector2 end, Single thickness, Int32 segment_size, Single space_size = 0.5f)
			{
				// Длина
				Single length = (end - start).magnitude;

				// Количество сегментов линии
				Int32 count_segment = (Int32)length / segment_size;
				Single rest = length / segment_size - count_segment;

				// Увеличиваем размер массива
				Resize(count_segment * 4 + offset);

				// Вектор направления по сегментно
				Vector2 dir = (end - start) / length;

				// Ограничиваем размер 
				space_size = Mathf.Clamp(space_size, 0, 1);
				Vector2 current = start;
				for (Int32 i = 0; i < count_segment; i++)
				{
					Vector2 s = current;
					current += dir * ((Single)segment_size * (1 - space_size));

					Vector2 e = current;
					current += dir * ((Single)segment_size * space_size);

					offset = GenerateThicknessLine(offset, s, e, thickness);
				}

				// Заполняем остаток
				if (Mathf.Abs(rest) > 0.1)
				{
					// Увеличиваем размер массива
					Resize(count_segment * 4 + offset + 4);

					offset = GenerateThicknessLine(offset, current, end, thickness);
				}

				return offset;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация данных о штриховой линии указанной толщины в массиве
			/// </summary>
			/// <param name="offset">Смещение в массиве точек</param>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			/// <returns>Текущие смещение в массиве</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Int32 GenerateSegmentThicknessLineNotTransform(Int32 offset, ref Vector2 start, ref Vector2 end, Single thickness, Int32 segment_size, Single space_size = 0.5f)
			{
				// Длина
				Single length = (end - start).magnitude;

				// Количество сегментов линии
				Int32 count_segment = (Int32)length / segment_size;
				Single rest = length / segment_size - count_segment;

				// Увеличиваем размер массива
				Resize(count_segment * 4 + offset);

				// Вектор направления по сегментно
				Vector2 dir = (end - start) / length;

				// Ограничиваем размер 
				space_size = Mathf.Clamp(space_size, 0, 1);
				Vector2 current = start;
				for (Int32 i = 0; i < count_segment; i++)
				{
					Vector2 s = current;
					current += dir * ((Single)segment_size * (1 - space_size));

					Vector2 e = current;
					current += dir * ((Single)segment_size * space_size);

					offset = GenerateThicknessLineNotTransform(offset, ref s, ref e, thickness, false);
				}

				// Заполняем остаток
				if (Mathf.Abs(rest) > 0.1)
				{
					// Увеличиваем размер массива
					Resize(count_segment * 4 + offset + 4);

					offset = GenerateThicknessLineNotTransform(offset, ref current, ref end, thickness, false);
				}

				return offset;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка углов у примитива
			/// </summary>
			/// <remarks>
			/// Применяется при сопряжении углов
			/// </remarks>
			/// <param name="index">Индекс</param>
			//---------------------------------------------------------------------------------------------------------
			protected void CornerTreatment(Int32 index)
			{
				// 1---------2
				// | \       |
				// ----\------
				// |     \   |
				// 4---------3

				// 1---------2
				// |         |
				// -----------
				// |         |
				// 4---------3

				//
				// ПЕРВАЯ ЛИНИЯ
				//
				Vector2 p1_1 = mPoints[index];
				Vector2 p1_2 = mPoints[index + 1];
				Vector2 p1_3 = mPoints[index + 2];
				Vector2 p1_4 = mPoints[index + 3];

				//
				// ВТОРАЯ ЛИНИЯ
				//
				Int32 two_index = index + 4;

				// Если есть дополнительный треугольник для обрамления угла то смещаем еще на три вершины
				Int32 add_index = -1;
				if (mCornerMode == TCornerMode.Bevel)
				{
					add_index = two_index;
					two_index += 3;
				}

				// Если мы вышли за границу значит есть замыкание
				if (two_index >= mPoints.Length)
				{
					two_index = 0;
				}

				Vector2 p2_1 = mPoints[two_index];
				Vector2 p2_2 = mPoints[two_index + 1];
				Vector2 p2_3 = mPoints[two_index + 2];
				Vector2 p2_4 = mPoints[two_index + 3];

				// Смотрим есть ли пересечение линии (p1_1, p1_2) и (p2_1 и p2_2)
				//  4   1       2   3
				//   \   \     /   /
				//    \   \   /   /
				//     3   2 1   4

				Vector2 point_common_2_1 = Vector2.zero;
				if (LineToLine(ref p1_1, ref p1_2, ref p2_1, ref p2_2, ref point_common_2_1) == 1)
				{
					// Совмещаем точки
					p1_2 = point_common_2_1;
					p2_1 = point_common_2_1;

					if (mCornerMode == TCornerMode.SharpJoint)
					{
						// Теперь нам надо соединить (найти общую точку пересечения) линии (p1_4, p1_3) и (p2_4, p2_3)
						Vector2 point_common_3_4 = Vector3.zero;
						LineToLine(ref p1_4, ref p1_3, ref p2_4, ref p2_3, ref point_common_3_4);
						{
							p1_3 = point_common_3_4;
							p2_4 = point_common_3_4;
						}
					}

					if (mCornerMode == TCornerMode.Bevel)
					{
						// Теперь нам надо нарисовать дополнительный треугольник: p1_3, point_common_2_1, p2_4
						mPoints[add_index] = p1_3;
						mPoints[add_index + 1] = point_common_2_1;
						mPoints[add_index + 2] = p2_4;
					}
				}
				else
				{
					// 2 Вариант пересечения
					//     2   3 4   1
					//    /   /   \   \
					//   /   /     \   \
					//  1   4       3   2

					Vector2 point_common_3_4 = Vector2.zero;
					if (LineToLine(ref p1_4, ref p1_3, ref p2_4, ref p2_3, ref point_common_3_4) == 1)
					{
						// Совмещаем точки
						p1_3 = point_common_3_4;
						p2_4 = point_common_3_4;

						if (mCornerMode == TCornerMode.SharpJoint)
						{
							// Теперь нам надо соединить (найти общую точку пересечения) линии (p1_1, p1_2) и (p2_1, p2_2)
							Vector2 point_common_1_2 = Vector3.zero;
							LineToLine(ref p1_1, ref p1_2, ref p2_1, ref p2_2, ref point_common_1_2);
							{
								p1_2 = point_common_1_2;
								p2_1 = point_common_1_2;
							}
						}

						if (mCornerMode == TCornerMode.Bevel)
						{
							// Теперь нам надо нарисовать дополнительный треугольник: p1_2, point_common_3_4, p2_1
							mPoints[add_index] = p1_2;
							mPoints[add_index + 1] = point_common_3_4;
							mPoints[add_index + 2] = p2_1;
						}
					}
				}

				mPoints[index] = p1_1;
				mPoints[index + 1] = p1_2;
				mPoints[index + 2] = p1_3;
				mPoints[index + 3] = p1_4;

				//
				// ВТОРАЯ ЛИНИЯ
				//
				mPoints[two_index] = p2_1;
				mPoints[two_index + 1] = p2_2;
				mPoints[two_index + 2] = p2_3;
				mPoints[two_index + 3] = p2_4;

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменения размера массива
			/// </summary>
			/// <param name="new_count">Новый размер массива</param>
			//---------------------------------------------------------------------------------------------------------
			protected void Resize(Int32 new_count)
			{
				if (mPoints.Length < new_count)
				{
#if UNITY_EDITOR
					//Debug.Log("Было:" + mPoints.Length.ToString() + "; Требуется: " + new_count.ToString());
#endif

					Array.Resize(ref mPoints, new_count);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размера по числе линий с учетом режима их сопряжения
			/// </summary>
			/// <param name="count_lines">Количество линий</param>
			//---------------------------------------------------------------------------------------------------------
			protected void SetSizeFromCount(Int32 count_lines)
			{
				// Увеличиваем размер массива
				Int32 size = count_lines * 4;
				if (mCornerMode == TCornerMode.Bevel)
				{
					size += (count_lines - 1) * 3;
					if (mIsClosed) size += 3;
				}

				if (mPoints.Length < size)
				{
#if UNITY_EDITOR
					//Debug.Log("Было:" + mPoints.Length.ToString() + "; Требуется: " + new_count.ToString());
#endif

					Array.Resize(ref mPoints, size);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение примитива
			/// </summary>
			/// <param name="offset">Смещение</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void MoveTo(Vector2 offset)
			{
				for (Int32 i = 0; i < mPoints.Length; i++)
				{
					mPoints[i] += offset;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РИСОВАНИЯ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование отдельных линий
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void DrawLines()
			{
				XGUIRender.ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINES);
				{
					GL.Color(XGUIRender.StrokeColor);
					Int32 count = CountPoint > 0 ? CountPoint : mPoints.Length;

#if UNITY_EDITOR
					if (mIsClosed == false && count % 2 != 0)
					{
						Debug.Log("Количество вершин должно быть четное");
					}
#endif

					for (Int32 i = 1; i < count; i += 2)
					{
						GL.Vertex(mPoints[i - 1]);
						GL.Vertex(mPoints[i]);
					}

					if(mIsClosed)
					{
						GL.Vertex(mPoints[count - 1]);
						GL.Vertex(mPoints[0]);
					}
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование последовательных линий
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void DrawLinesStrip()
			{
				XGUIRender.ColoredMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.LINE_STRIP);
				{
					GL.Color(XGUIRender.StrokeColor);
					Int32 count = CountPoint > 0 ? CountPoint : mPoints.Length;

					for (Int32 i = 0; i < count; i++)
					{
						GL.Vertex(mPoints[i]);
					}

					if (mIsClosed)
					{
						GL.Vertex(mPoints[0]);
					}
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование отдельных треугольников
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void DrawTriangles()
			{
				XGUIRender.TexturedMaterial2D.color = XGUIRender.StrokeColor;
				XGUIRender.TexturedMaterial2D.SetPass(0);

				GL.PushMatrix();
				GL.LoadPixelMatrix();
				GL.Begin(GL.TRIANGLES);
				{
					if(mCornerMode == TCornerMode.Bevel)
					{
						if(mIsClosed)
						{
							DrawTrianglesCornerModeClosed();
						}
						else
						{
							DrawTrianglesCornerMode();
						}
					}
					else
					{
						DrawTrianglesSeparate();
					}
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование отдельных треугольников
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void DrawTrianglesCornerModeClosed()
			{
				Int32 count = CountPoint > 0 ? CountPoint : mPoints.Length;
				for (Int32 i = 0; i < count; i += 7)
				{
					// 1------2
					// |\     |
					// | \    |
					// |==\===|
					// |   \  |
					// |    \ |
					// 4------3

					GL.TexCoord(XGUIRender.MapUV_TopLeft);
					GL.Vertex(mPoints[i]);

					GL.TexCoord(XGUIRender.MapUV_TopRight);
					GL.Vertex(mPoints[i + 1]);

					GL.TexCoord(XGUIRender.MapUV_BottomRight);
					GL.Vertex(mPoints[i + 2]);

					GL.TexCoord(XGUIRender.MapUV_TopLeft);
					GL.Vertex(mPoints[i]);

					GL.TexCoord(XGUIRender.MapUV_BottomRight);
					GL.Vertex(mPoints[i + 2]);

					GL.TexCoord(XGUIRender.MapUV_BottomLeft);
					GL.Vertex(mPoints[i + 3]);


					GL.TexCoord(XGUIRender.MapUV_BottomLeft);
					GL.Vertex(mPoints[i + 4]);

					GL.TexCoord(XGUIRender.MapUV_TopCenter);
					GL.Vertex(mPoints[i + 5]);

					GL.TexCoord(XGUIRender.MapUV_BottomRight);
					GL.Vertex(mPoints[i + 6]);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование отдельных треугольников
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void DrawTrianglesCornerMode()
			{
				Int32 count = CountPoint > 0 ? CountPoint : mPoints.Length;
				count -= 4;
				for (Int32 i = 0; i < count; i += 7)
				{
					// 1------2
					// |\     |
					// | \    |
					// |==\===|
					// |   \  |
					// |    \ |
					// 4------3

					GL.TexCoord(XGUIRender.MapUV_TopLeft);
					GL.Vertex(mPoints[i]);

					GL.TexCoord(XGUIRender.MapUV_TopRight);
					GL.Vertex(mPoints[i + 1]);

					GL.TexCoord(XGUIRender.MapUV_BottomRight);
					GL.Vertex(mPoints[i + 2]);

					GL.TexCoord(XGUIRender.MapUV_TopLeft);
					GL.Vertex(mPoints[i]);

					GL.TexCoord(XGUIRender.MapUV_BottomRight);
					GL.Vertex(mPoints[i + 2]);

					GL.TexCoord(XGUIRender.MapUV_BottomLeft);
					GL.Vertex(mPoints[i + 3]);


					GL.TexCoord(XGUIRender.MapUV_BottomLeft);
					GL.Vertex(mPoints[i + 4]);

					GL.TexCoord(XGUIRender.MapUV_TopCenter);
					GL.Vertex(mPoints[i + 5]);

					GL.TexCoord(XGUIRender.MapUV_BottomRight);
					GL.Vertex(mPoints[i + 6]);
				}

				Int32 j = count - 4;
				GL.TexCoord(XGUIRender.MapUV_TopLeft);
				GL.Vertex(mPoints[j]);

				GL.TexCoord(XGUIRender.MapUV_TopRight);
				GL.Vertex(mPoints[j + 1]);

				GL.TexCoord(XGUIRender.MapUV_BottomRight);
				GL.Vertex(mPoints[j + 2]);

				GL.TexCoord(XGUIRender.MapUV_TopLeft);
				GL.Vertex(mPoints[j]);

				GL.TexCoord(XGUIRender.MapUV_BottomRight);
				GL.Vertex(mPoints[j + 2]);

				GL.TexCoord(XGUIRender.MapUV_BottomLeft);
				GL.Vertex(mPoints[j + 3]);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование отдельных треугольников
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void DrawTrianglesSeparate()
			{
				Int32 count = CountPoint > 0 ? CountPoint : mPoints.Length;

#if UNITY_EDITOR
				if (count % 4 != 0)
				{
					Debug.Log("Количество вершин должно быть кратное 4");
				}
#endif

				for (Int32 i = 0; i < count; i += 4)
				{
					// 1------2
					// |\     |
					// | \    |
					// |==\===|
					// |   \  |
					// |    \ |
					// 4------3

					GL.TexCoord(XGUIRender.MapUV_TopLeft);
					GL.Vertex(mPoints[i]);

					GL.TexCoord(XGUIRender.MapUV_TopRight);
					GL.Vertex(mPoints[i + 1]);

					GL.TexCoord(XGUIRender.MapUV_BottomRight);
					GL.Vertex(mPoints[i + 2]);

					GL.TexCoord(XGUIRender.MapUV_TopLeft);
					GL.Vertex(mPoints[i]);

					GL.TexCoord(XGUIRender.MapUV_BottomRight);
					GL.Vertex(mPoints[i + 2]);

					GL.TexCoord(XGUIRender.MapUV_BottomLeft);
					GL.Vertex(mPoints[i + 3]);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование примитива
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Draw()
			{
				switch (mModeDraw)
				{
					case GL.LINES:
						{
							DrawLines();
						}
						break;
					case GL.LINE_STRIP:
						{
							DrawLinesStrip();
						}
						break;
					case GL.TRIANGLES:
						{
							DrawTriangles();
						}
						break;
					default:
						break;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Линия
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CPrimitiveLine2D : CPrimitive2D
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Vector2 mStartPoint;
			internal Vector2 mEndPoint;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Начальная точка линии
			/// </summary>
			public Vector2 StartPoint
			{
				get { return mStartPoint; }
				set
				{
					mStartPoint = value;
				}
			}

			/// <summary>
			/// Конечная точка линии
			/// </summary>
			public Vector2 EndPoint
			{
				get { return mEndPoint; }
				set
				{
					mEndPoint = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveLine2D()
			{
				mPoints = new Vector2[3];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveLine2D(Vector2 start, Vector2 end)
			{
				mPoints = new Vector2[2];
				mStartPoint = start;
				mEndPoint = end;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="start_x">Координата начальной точки по X</param>
			/// <param name="start_y">Координата начальной точки по Y</param>
			/// <param name="end_x">Координата конечной точки по X</param>
			/// <param name="end_y">Координата конечной точки по Y</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveLine2D(Single start_x, Single start_y, Single end_x, Single end_y)
			{
				mPoints = new Vector2[2];
				mStartPoint = new Vector2(start_x, start_y);
				mEndPoint = new Vector2(end_x, end_y);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение примитива
			/// </summary>
			/// <param name="offset">Смещение</param>
			//---------------------------------------------------------------------------------------------------------
			public override void MoveTo(Vector2 offset)
			{
				mStartPoint += offset;
				mEndPoint += offset;

				for (Int32 i = 0; i < mPoints.Length; i++)
				{
					mPoints[i] += offset;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация простой линии
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Generate()
			{
				mModeDraw = GL.LINE_STRIP;
				mPoints[0] = new Vector2(mStartPoint.x, Screen.height - mStartPoint.y);
				mPoints[1] = new Vector2(mEndPoint.x, Screen.height - mEndPoint.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация штриховой линии
			/// </summary>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegment(Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.LINES;
				GenerateSegment(0, mStartPoint, mEndPoint, segment_size, space_size);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация линии указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateThickness(Single thickness)
			{
				mModeDraw = GL.TRIANGLES;
				SetSizeFromCount(1);
				GenerateThicknessLine(0, mStartPoint, mEndPoint, thickness);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация штриховой линии указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegmentThickness(Single thickness, Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.TRIANGLES;
				GenerateSegmentThicknessLine(0, mStartPoint, mEndPoint, thickness, segment_size, space_size);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Угол
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CPrimitiveAngle2D : CPrimitive2D
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Vector2 mStartPoint;
			internal Vector2 mPivotPoint;
			internal Vector2 mEndPoint;
			
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Начальная точка угла
			/// </summary>
			public Vector2 StartPoint
			{
				get { return mStartPoint; }
				set
				{
					mStartPoint = value;
				}
			}

			/// <summary>
			/// Опорная точка угла
			/// </summary>
			public Vector2 PivotPoint
			{
				get { return mPivotPoint; }
				set
				{
					mPivotPoint = value;
				}
			}

			/// <summary>
			/// Конечная точка угла
			/// </summary>
			public Vector2 EndPoint
			{
				get { return mEndPoint; }
				set
				{
					mEndPoint = value;
				}
			}

			/// <summary>
			/// Величина угла в градусах
			/// </summary>
			public Single Angle
			{
				get
				{
					return Vector2.Angle(mEndPoint - mPivotPoint, mStartPoint - mPivotPoint);
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveAngle2D()
			{
				mPoints = new Vector2[3];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="start">Начальная точка угла</param>
			/// <param name="pivot">Опорная точка угла</param>
			/// <param name="end">Конечная точка угла</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveAngle2D(Vector2 start, Vector2 pivot, Vector2 end)
			{
				mPoints = new Vector2[3];
				mStartPoint = start;
				mPivotPoint = pivot;
				mEndPoint = end;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение примитива
			/// </summary>
			/// <param name="offset">Смещение</param>
			//---------------------------------------------------------------------------------------------------------
			public override void MoveTo(Vector2 offset)
			{
				mStartPoint += offset;
				mPivotPoint += offset;
				mEndPoint += offset;

				for (Int32 i = 0; i < mPoints.Length; i++)
				{
					mPoints[i] += offset;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация угла образованного двумя линиями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Generate()
			{
				mModeDraw = GL.LINE_STRIP;
				mPoints[0] = new Vector2(mStartPoint.x, Screen.height - mStartPoint.y);
				mPoints[1] = new Vector2(mPivotPoint.x, Screen.height - mPivotPoint.y);
				mPoints[2] = new Vector2(mEndPoint.x, Screen.height - mEndPoint.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация угла образованного двумя штриховыми линиями
			/// </summary>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegment(Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.LINES;
				Int32 offset = GenerateSegment(0, mStartPoint, mPivotPoint, segment_size, space_size);
				GenerateSegment(offset, mPivotPoint, mEndPoint, segment_size, space_size);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация угла образованного двумя линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateThickness(Single thickness)
			{
				mModeDraw = GL.TRIANGLES;
				SetSizeFromCount(2);
				Int32 offset = GenerateThicknessLine(0, mStartPoint, mPivotPoint, thickness);
				offset = GenerateThicknessLine(offset, mPivotPoint, mEndPoint, thickness);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация угла образованного двумя штриховыми линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegmentThickness(Single thickness, Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.TRIANGLES;
				Int32 offset = GenerateSegmentThicknessLine(0, mStartPoint, mPivotPoint, thickness, segment_size, space_size);
				GenerateSegmentThicknessLine(offset, mPivotPoint, mEndPoint, thickness, segment_size, space_size);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Треугольник
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CPrimitiveTriangle2D : CPrimitive2D
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Vector2 mPoint1;
			internal Vector2 mPoint2;
			internal Vector2 mPoint3;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Первая точка треугольника
			/// </summary>
			public Vector2 Point1
			{
				get { return mPoint1; }
				set
				{
					mPoint1 = value;
				}
			}

			/// <summary>
			/// Вторая точка треугольника
			/// </summary>
			public Vector2 Point2
			{
				get { return mPoint2; }
				set
				{
					mPoint2 = value;
				}
			}

			/// <summary>
			/// Третья точка треугольника
			/// </summary>
			public Vector2 Point3
			{
				get { return mPoint3; }
				set
				{
					mPoint3 = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveTriangle2D()
			{
				mIsClosed = true;
				mPoints = new Vector2[3];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="p1">Первая точка</param>
			/// <param name="p2">Вторая точка</param>
			/// <param name="p3">Третья точка</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveTriangle2D(Vector2 p1, Vector2 p2, Vector2 p3)
			{
				mIsClosed = true;
				mPoints = new Vector2[3];
				mPoint1 = p1;
				mPoint2 = p2;
				mPoint3 = p3;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение примитива
			/// </summary>
			/// <param name="offset">Смещение</param>
			//---------------------------------------------------------------------------------------------------------
			public override void MoveTo(Vector2 offset)
			{
				mPoint1 += offset;
				mPoint2 += offset;
				mPoint3 += offset;

				for (Int32 i = 0; i < mPoints.Length; i++)
				{
					mPoints[i] += offset;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация треугольника
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Generate()
			{
				mModeDraw = GL.LINE_STRIP;
				mPoints[0] = new Vector2(mPoint1.x, Screen.height - mPoint1.y);
				mPoints[1] = new Vector2(mPoint2.x, Screen.height - mPoint2.y);
				mPoints[2] = new Vector2(mPoint3.x, Screen.height - mPoint3.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация треугольника со штриховыми линиями
			/// </summary>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegment(Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.LINES;
				Int32 offset = GenerateSegment(0, mPoint1, mPoint2, segment_size, space_size);
				offset = GenerateSegment(offset, mPoint2, mPoint3, segment_size, space_size);
				GenerateSegment(offset, mPoint3, mPoint1, segment_size, space_size);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация треугольника с линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateThickness(Single thickness)
			{
				mModeDraw = GL.TRIANGLES;
				SetSizeFromCount(3);
				Int32 offset = GenerateThicknessLine(0, mPoint1, mPoint2, thickness);
				offset = GenerateThicknessLine(offset, mPoint2, mPoint3, thickness);
				GenerateThicknessLine(offset, mPoint3, mPoint1, thickness, true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			///Генерация треугольника штриховыми линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegmentThickness(Single thickness, Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.TRIANGLES;
				Int32 offset = GenerateSegmentThicknessLine(0, mPoint1, mPoint2, thickness, segment_size, space_size);
				offset = GenerateSegmentThicknessLine(offset, mPoint2, mPoint3, thickness, segment_size, space_size);
				GenerateSegmentThicknessLine(offset, mPoint3, mPoint1, thickness, segment_size, space_size);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Прямоугольник
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CPrimitiveRect2D : CPrimitive2D
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Rect mRect;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Прямоугольник
			/// </summary>
			public Rect Rect
			{
				get { return mRect; }
				set
				{
					mRect = value;
				}
			}

			/// <summary>
			/// Позиция верхнего левого угла прямоугольника по X
			/// </summary>
			public Single X
			{
				get { return mRect.x; }
				set
				{
					mRect.x = value;
				}
			}

			/// <summary>
			/// Позиция верхнего левого угла прямоугольника по Y
			/// </summary>
			public Single Y
			{
				get { return mRect.y; }
				set
				{
					mRect.y = value;
				}
			}

			/// <summary>
			/// Ширина прямоугольника
			/// </summary>
			public Single Width
			{
				get { return mRect.width; }
				set
				{
					mRect.width = value;
				}
			}

			/// <summary>
			/// Высота прямоугольника
			/// </summary>
			public Single Height
			{
				get { return mRect.height; }
				set
				{
					mRect.height = value;
				}
			}

			/// <summary>
			/// Верхняя левая точка прямоугольника
			/// </summary>
			public Vector2 TopLeft
			{
				get { return new Vector2(mRect.x, mRect.y); }
				set
				{
					mRect.x = value.x;
					mRect.y = value.y;
				}
			}

			/// <summary>
			/// Верхняя правая точка прямоугольника
			/// </summary>
			public Vector2 TopRight
			{
				get { return new Vector2(mRect.xMax, mRect.y); }
				set
				{
					mRect.xMax = value.x;
					mRect.y = value.y;
				}
			}

			/// <summary>
			/// Нижняя левая точка прямоугольника
			/// </summary>
			public Vector2 BottomLeft
			{
				get { return new Vector2(mRect.x, mRect.yMax); }
				set
				{
					mRect.x = value.x;
					mRect.yMax = value.y;
				}
			}

			/// <summary>
			/// Нижняя правая точка прямоугольника
			/// </summary>
			public Vector2 BottomRight
			{
				get { return new Vector2(mRect.xMax, mRect.yMax); }
				set
				{
					mRect.xMax = value.x;
					mRect.yMax = value.y;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveRect2D()
			{
				mIsClosed = true;
				mPoints = new Vector2[4];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="rect">Прямоугольник</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveRect2D(Rect rect)
			{
				mIsClosed = true;
				mPoints = new Vector2[4];
				mRect = rect;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="top_left">Верхняя левая точка прямоугольника</param>
			/// <param name="top_right">Верхняя правая точка прямоугольника</param>
			/// <param name="bottom_left">Нижняя левая точка прямоугольника</param>
			/// <param name="bottom_right">Нижняя правая точка прямоугольника</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveRect2D(Vector2 top_left, Vector2 top_right, Vector2 bottom_left, Vector2 bottom_right)
			{
				mIsClosed = true;
				mPoints = new Vector2[4];
				mRect = new Rect(top_left.x, top_left.y, top_right.x - top_left.x, bottom_left.y - top_left.y);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение примитива
			/// </summary>
			/// <param name="offset">Смещение</param>
			//---------------------------------------------------------------------------------------------------------
			public override void MoveTo(Vector2 offset)
			{
				mRect.position += offset;

				for (Int32 i = 0; i < mPoints.Length; i++)
				{
					mPoints[i] += offset;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация прямоугольника
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Generate()
			{
				mModeDraw = GL.LINE_STRIP;
				mPoints[0] = new Vector2(mRect.x, Screen.height - mRect.y);
				mPoints[1] = new Vector2(mRect.xMax, Screen.height - mRect.y);
				mPoints[2] = new Vector2(mRect.xMax, Screen.height - mRect.yMax);
				mPoints[3] = new Vector2(mRect.x, Screen.height - mRect.yMax);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация прямоугольника со штриховыми линиями
			/// </summary>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegment(Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.LINES;
				Vector2 tl = new Vector2(mRect.x, Screen.height - mRect.y);
				Vector2 tr = new Vector2(mRect.xMax, Screen.height - mRect.y);
				Vector2 bl = new Vector2(mRect.x, Screen.height - mRect.yMax);
				Vector2 br = new Vector2(mRect.xMax, Screen.height - mRect.yMax);
				Int32 offset = GenerateSegmentNotTransform(0, ref tl, ref tr, segment_size, space_size);
				offset = GenerateSegmentNotTransform(offset, ref tr, ref br, segment_size, space_size);
				offset = GenerateSegmentNotTransform(offset, ref br, ref bl, segment_size, space_size);
				GenerateSegmentNotTransform(offset, ref bl, ref tl, segment_size, space_size);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация прямоугольника с линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateThickness(Single thickness)
			{
				mModeDraw = GL.TRIANGLES;
				SetSizeFromCount(4);
				Vector2 tl = new Vector2(mRect.x, Screen.height - mRect.y);
				Vector2 tr = new Vector2(mRect.xMax, Screen.height - mRect.y);
				Vector2 bl = new Vector2(mRect.x, Screen.height - mRect.yMax);
				Vector2 br = new Vector2(mRect.xMax, Screen.height - mRect.yMax);
				Int32 offset = GenerateThicknessLineNotTransform(0, ref tl, ref tr, thickness, false);
				offset = GenerateThicknessLineNotTransform(offset, ref tr, ref br, thickness, false);
				offset = GenerateThicknessLineNotTransform(offset, ref br, ref bl, thickness, false);
				GenerateThicknessLineNotTransform(offset, ref bl, ref tl, thickness, true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			///Генерация прямоугольника штриховыми линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegmentThickness(Single thickness, Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.TRIANGLES;
				Vector2 tl = new Vector2(mRect.x, Screen.height - mRect.y);
				Vector2 tr = new Vector2(mRect.xMax, Screen.height - mRect.y);
				Vector2 bl = new Vector2(mRect.x, Screen.height - mRect.yMax);
				Vector2 br = new Vector2(mRect.xMax, Screen.height - mRect.yMax);
				Int32 offset = GenerateSegmentThicknessLineNotTransform(0, ref tl, ref tr, thickness, segment_size, space_size);
				offset = GenerateSegmentThicknessLineNotTransform(offset, ref tr, ref br, thickness, segment_size, space_size);
				offset = GenerateSegmentThicknessLineNotTransform(offset, ref br, ref bl, thickness, segment_size, space_size);
				GenerateSegmentThicknessLineNotTransform(offset, ref bl, ref tl, thickness, segment_size, space_size);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Эллипс
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CPrimitiveEllipse2D : CPrimitive2D
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Vector2 mCenter;
			internal Single mRadiusX;
			internal Single mRadiusY;
			internal Int32 mQuality;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Центр эллипса
			/// </summary>
			public Vector2 Center
			{
				get { return mCenter; }
				set
				{
					mCenter = value;
				}
			}

			/// <summary>
			/// Радиус эллипса по X
			/// </summary>
			public Single RadiusX
			{
				get { return mRadiusX; }
				set
				{
					mRadiusX = value;
				}
			}

			/// <summary>
			/// Радиус эллипса по Y
			/// </summary>
			public Single RadiusY
			{
				get { return mRadiusY; }
				set
				{
					mRadiusY = value;
				}
			}

			/// <summary>
			/// Качество растеризации эллипса
			/// </summary>
			public Int32 Quality
			{
				get { return mQuality; }
				set
				{
					mQuality = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveEllipse2D()
			{
				mIsClosed = true;
				mPoints = new Vector2[4];
				mQuality = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <remarks>
			/// В указанный прямоугольник будет вписан эллипс
			/// </remarks>
			/// <param name="rect">Прямоугольник</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveEllipse2D(Rect rect)
			{
				mQuality = 30;
				mPoints = new Vector2[4];
				mCenter = rect.center;
				mRadiusX = rect.width / 2;
				mRadiusY = rect.height / 2;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="center">Центр эллипса</param>
			/// <param name="radius_x">Радиус эллипса по X</param>
			/// <param name="radius_y">Радиус эллипса по Y</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveEllipse2D(Vector2 center, Single radius_x, Single radius_y)
			{
				mQuality = 30;
				mPoints = new Vector2[4];
				mCenter = center;
				mRadiusX = radius_x;
				mRadiusY = radius_y;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение примитива
			/// </summary>
			/// <param name="offset">Смещение</param>
			//---------------------------------------------------------------------------------------------------------
			public override void MoveTo(Vector2 offset)
			{
				mCenter += offset;

				for (Int32 i = 0; i < mPoints.Length; i++)
				{
					mPoints[i] += offset;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация эллипса
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Generate()
			{
				mIsClosed = true;
				mModeDraw = GL.LINE_STRIP;

				Resize(mQuality);
	
				Single delta_angle = 360.0f / (Single)mQuality;

				for (Int32 i = 0; i < mQuality; i++)
				{
					Single x = Mathf.Cos(i * delta_angle * Mathf.Deg2Rad) * mRadiusX;
					Single y = Mathf.Sin(i * delta_angle * Mathf.Deg2Rad) * mRadiusY;
					mPoints[i] = new Vector2(mCenter.x + x, Screen.height - mCenter.y + y);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация эллипса со штриховыми линиями
			/// </summary>
			/// <param name="angle_offset">Начальное смещение угла</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegment(Single angle_offset)
			{
				mIsClosed = false;
				mModeDraw = GL.LINES;

				Resize(mQuality + 1);

				Single delta_angle = 360.0f / (Single)mQuality;
				Vector2 prev = new Vector2(mCenter.x + Mathf.Cos(angle_offset * Mathf.Deg2Rad) * mRadiusX,
					Screen.height - mCenter.y + Mathf.Sin(angle_offset * Mathf.Deg2Rad) * mRadiusY);

				Int32 index = 0;
				for (Int32 i = 1; i < mQuality + 1; i++)
				{
					Single x = Mathf.Cos((i * delta_angle + angle_offset) * Mathf.Deg2Rad) * mRadiusX;
					Single y = Mathf.Sin((i * delta_angle + angle_offset) * Mathf.Deg2Rad) * mRadiusY;
					Vector2 point = new Vector2(mCenter.x + x, Screen.height - mCenter.y + y);

					if(i == 1)
					{
						mPoints[index] = prev;

						index++;
						mPoints[index] = point;
					}

					if (i > 2 && i % 2 != 0)
					{
						index++;
						mPoints[index] = prev;

						index++;
						mPoints[index] = point;
					}

					prev = point;
				}

				if (mPoints.Length != index + 1)
				{
					Array.Resize(ref mPoints, index + 1);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация эллипса с линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateThickness(Single thickness)
			{
				mIsClosed = true;
				mModeDraw = GL.TRIANGLES;

				SetSizeFromCount(mQuality);
				Single delta_angle = 360.0f / (Single)mQuality;

				Vector2 prev = new Vector2(mCenter.x + mRadiusX, mCenter.y);
				Int32 offset = 0;
				for (Int32 i = 1; i < mQuality + 1; i++)
				{
					Single x = Mathf.Cos(i * delta_angle * Mathf.Deg2Rad) * mRadiusX;
					Single y = Mathf.Sin(i * delta_angle * Mathf.Deg2Rad) * mRadiusY;
					Vector2 to = new Vector2(mCenter.x + x, mCenter.y + y);

					if (i == mQuality)
					{
						offset = GenerateThicknessLine(offset, prev, to, thickness, true);
					}
					else
					{
						offset = GenerateThicknessLine(offset, prev, to, thickness);
					}

					prev = to;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			///Генерация эллипса штриховыми линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="angle_offset">Начальное смещение угла</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegmentThickness(Single thickness, Single angle_offset)
			{
				mModeDraw = GL.TRIANGLES;

				Resize(mQuality * 4);

				Single delta_angle = 360.0f / (Single)mQuality;

				Vector2 prev = new Vector2(mCenter.x + Mathf.Cos(angle_offset * Mathf.Deg2Rad) * mRadiusX,
					mCenter.y + Mathf.Sin(angle_offset * Mathf.Deg2Rad) * mRadiusY);

				Int32 offset = 0;
				for (Int32 i = 1; i < mQuality + 1; i++)
				{
					Single x = Mathf.Cos((i * delta_angle + angle_offset) * Mathf.Deg2Rad) * mRadiusX;
					Single y = Mathf.Sin((i * delta_angle + angle_offset) * Mathf.Deg2Rad) * mRadiusY;
					Vector2 to = new Vector2(mCenter.x + x, mCenter.y + y);

					if (i == 1)
					{
						offset = GenerateThicknessLine(offset, prev, to, thickness);
					}

					if (i > 2 && i % 2 != 0)
					{
						offset = GenerateThicknessLine(offset, prev, to, thickness);
					}

					prev = to;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Сектор
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CPrimitiveSector2D : CPrimitive2D
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Vector2 mCenter;
			internal Single mRadiusX;
			internal Single mRadiusY;
			internal Single mStartAngle;
			internal Single mEndAngle;
			internal Int32 mQuality;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Центр сектора
			/// </summary>
			public Vector2 Center
			{
				get { return mCenter; }
				set
				{
					mCenter = value;
				}
			}

			/// <summary>
			/// Радиус сектора по X
			/// </summary>
			public Single RadiusX
			{
				get { return mRadiusX; }
				set
				{
					mRadiusX = value;
				}
			}

			/// <summary>
			/// Радиус сектора по Y
			/// </summary>
			public Single RadiusY
			{
				get { return mRadiusY; }
				set
				{
					mRadiusY = value;
				}
			}

			/// <summary>
			/// Угол в градусах от которого начинается сектор
			/// </summary>
			/// <remarks>
			/// Углы считаются против часовой стрелки
			/// </remarks>
			public Single StartAngle
			{
				get { return mStartAngle; }
				set
				{
					mStartAngle = value;
				}
			}

			/// <summary>
			/// Угол в градусах где заканчивается сектор
			/// </summary>
			/// <remarks>
			/// Углы считаются против часовой стрелки
			/// </remarks>
			public Single EndAngle
			{
				get { return mEndAngle; }
				set
				{
					mEndAngle = value;
				}
			}

			/// <summary>
			/// Угол сектора в градусах
			/// </summary>
			public Single Angle
			{
				get
				{
					Single angle = mEndAngle - mStartAngle;
					if (angle < 0) angle += 360;
					return angle;
				}
			}

			/// <summary>
			/// Качество растеризации сектора
			/// </summary>
			public Int32 Quality
			{
				get { return mQuality; }
				set
				{
					mQuality = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveSector2D()
			{
				mPoints = new Vector2[4];
				mQuality = 15;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <remarks>
			/// В указанный прямоугольник будет вписан сектор с соответствующими углами
			/// </remarks>
			/// <param name="rect">Прямоугольник</param>
			/// <param name="start_angle">Угол в градусах от которого начинается сектор</param>
			/// <param name="end_angle">Угол в градусах где заканчивается сектор</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveSector2D(Rect rect, Single start_angle, Single end_angle)
			{
				mQuality = 15;
				mPoints = new Vector2[4];
				mCenter = rect.center;
				mRadiusX = rect.width / 2;
				mRadiusY = rect.height / 2;
				mStartAngle = start_angle;
				mEndAngle = end_angle;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="center">Центр сектора</param>
			/// <param name="radius">Радиус сектора</param>
			/// <param name="start_angle">Угол в градусах от которого начинается сектор</param>
			/// <param name="end_angle">Угол в градусах где заканчивается сектор</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveSector2D(Vector2 center, Single radius, Single start_angle, Single end_angle)
			{
				mQuality = 15;
				mPoints = new Vector2[2];
				mCenter = center;
				mRadiusX = radius;
				mRadiusY = radius;
				mStartAngle = start_angle;
				mEndAngle = end_angle;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение примитива
			/// </summary>
			/// <param name="offset">Смещение</param>
			//---------------------------------------------------------------------------------------------------------
			public override void MoveTo(Vector2 offset)
			{
				mCenter += offset;

				for (Int32 i = 0; i < mPoints.Length; i++)
				{
					mPoints[i] += offset;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация сектора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Generate()
			{
				mIsClosed = true;
				mModeDraw = GL.LINE_STRIP;
				Resize(mQuality + 2);

				Single delta_angle = Angle / mQuality;

				mPoints[0] = new Vector2(mCenter.x, Screen.height - mCenter.y);
				for (Int32 i = 0; i < mQuality + 1; i++)
				{
					Single x = Mathf.Cos((i * delta_angle + mStartAngle) * Mathf.Deg2Rad) * mRadiusX;
					Single y = Mathf.Sin((i * delta_angle + mStartAngle) * Mathf.Deg2Rad) * mRadiusY;
					mPoints[i + 1] = new Vector2(mCenter.x + x, Screen.height - mCenter.y + y);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация сектора со штриховыми линиями
			/// </summary>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegment(Int32 segment_size, Single space_size = 0.5f)
			{
				mIsClosed = false;
				mModeDraw = GL.LINES;

				Vector2 center = new Vector2(mCenter.x, Screen.height - mCenter.y);
				Vector2 prev = new Vector2(mCenter.x + Mathf.Cos(mStartAngle * Mathf.Deg2Rad) * mRadiusX, 
					Screen.height - mCenter.y + Mathf.Sin(mStartAngle * Mathf.Deg2Rad) * mRadiusY);

				Int32 offset = GenerateSegmentNotTransform(0, ref center, ref prev, segment_size, space_size);

				Resize(mQuality + offset + offset);

				Single delta_angle = Angle / mQuality;
				for (Int32 i = 1; i < mQuality; i++)
				{
					Single x = Mathf.Cos((i * delta_angle+ mStartAngle) * Mathf.Deg2Rad) * mRadiusX;
					Single y = Mathf.Sin((i * delta_angle + mStartAngle) * Mathf.Deg2Rad) * mRadiusY;
					Vector2 point = new Vector2(mCenter.x + x, Screen.height - mCenter.y + y);

					if (i == 1)
					{
						mPoints[offset] = point;
					}

					if (i > 2 && i % 2 != 0)
					{
						offset++;
						mPoints[offset] = prev;

						offset++;
						mPoints[offset] = point;

					}

					prev = point;
				}

				offset = GenerateSegmentNotTransform(offset, ref prev, ref center, segment_size, space_size);
				Array.Resize(ref mPoints, offset);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация сектора с линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateThickness(Single thickness)
			{
				mIsClosed = true;
				mModeDraw = GL.TRIANGLES;

				SetSizeFromCount(mQuality - 1 + 2);

				Vector2 center = new Vector2(mCenter.x, Screen.height - mCenter.y);
				Vector2 prev = new Vector2(mCenter.x + Mathf.Cos(mStartAngle * Mathf.Deg2Rad) * mRadiusX,
					Screen.height - mCenter.y + Mathf.Sin(mStartAngle * Mathf.Deg2Rad) * mRadiusY);

				Int32 offset = GenerateThicknessLineNotTransform(0, ref center, ref prev, thickness, false);

				Single delta_angle = Angle / mQuality;
				for (Int32 i = 1; i < mQuality; i++)
				{
					Single x = Mathf.Cos((i * delta_angle + mStartAngle) * Mathf.Deg2Rad) * mRadiusX;
					Single y = Mathf.Sin((i * delta_angle + mStartAngle) * Mathf.Deg2Rad) * mRadiusY;
					Vector2 point = new Vector2(mCenter.x + x, Screen.height - mCenter.y + y);

					offset = GenerateThicknessLineNotTransform(offset, ref prev, ref point, thickness, false);

					prev = point;
				}

				GenerateThicknessLineNotTransform(offset, ref prev, ref center, thickness, true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация сектора штриховыми линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegmentThickness(Single thickness, Int32 segment_size, Single space_size = 0.5f)
			{
				mIsClosed = true;
				mModeDraw = GL.TRIANGLES;

				SetSizeFromCount(mQuality + 4);

				Vector2 center = new Vector2(mCenter.x, Screen.height - mCenter.y);
				Vector2 prev = new Vector2(mCenter.x + Mathf.Cos(mStartAngle * Mathf.Deg2Rad) * mRadiusX,
					Screen.height - mCenter.y + Mathf.Sin(mStartAngle * Mathf.Deg2Rad) * mRadiusY);

				Int32 offset = GenerateSegmentThicknessLineNotTransform(0, ref center, ref prev, thickness, segment_size, space_size);

				SetSizeFromCount(mQuality - 1 + offset / 4 * 2);

				Single delta_angle = Angle / mQuality;
				for (Int32 i = 1; i < mQuality; i++)
				{
					Single x = Mathf.Cos((i * delta_angle + mStartAngle) * Mathf.Deg2Rad) * mRadiusX;
					Single y = Mathf.Sin((i * delta_angle + mStartAngle) * Mathf.Deg2Rad) * mRadiusY;
					Vector2 point = new Vector2(mCenter.x + x, Screen.height - mCenter.y + y);

					if (i == 1)
					{
						offset = GenerateThicknessLineNotTransform(offset, ref prev, ref point, thickness, false);
					}

					if (i > 2 && i % 2 != 0)
					{
						offset = GenerateThicknessLineNotTransform(offset, ref prev, ref point, thickness, false);
					}

					prev = point;
				}

				GenerateSegmentThicknessLineNotTransform(offset, ref prev, ref center, thickness, segment_size, space_size);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Кубическая кривая Безье
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CPrimitiveCubicBezier2D : CPrimitive2D
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Vector2 mStartPoint;
			internal Vector2 mControlPoint1;
			internal Vector2 mControlPoint2;
			internal Vector2 mEndPoint;
			internal Int32 mQuality;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Начальная точка кривой
			/// </summary>
			public Vector2 StartPoint
			{
				get { return mStartPoint; }
				set
				{
					mStartPoint = value;
				}
			}

			/// <summary>
			/// Первая управляющая точка кривой
			/// </summary>
			public Vector2 ControlPoint1
			{
				get { return mControlPoint1; }
				set
				{
					mControlPoint1 = value;
				}
			}

			/// <summary>
			/// Вторая управляющая точка кривой
			/// </summary>
			public Vector2 ControlPoint2
			{
				get { return mControlPoint2; }
				set
				{
					mControlPoint2 = value;
				}
			}

			/// <summary>
			/// Конечная точка кривой
			/// </summary>
			public Vector2 EndPoint
			{
				get { return mEndPoint; }
				set
				{
					mEndPoint = value;
				}
			}

			/// <summary>
			/// Качество растеризации кривой
			/// </summary>
			public Int32 Quality
			{
				get { return mQuality; }
				set
				{
					mQuality = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveCubicBezier2D()
			{
				mPoints = new Vector2[4];
				mQuality = 30;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="coeff_wave">Коэффициент определяющий плавность перехода </param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveCubicBezier2D(Vector2 start, Vector2 end, Single coeff_wave = 0.5f)
			{
				mPoints = new Vector2[4];
				mQuality = 30;
				CreateFromStartToEnd(start, end, coeff_wave);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="control_point1">Первая контрольная точка</param>
			/// <param name="control_point2">Вторая контрольная точка</param>
			/// <param name="end">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveCubicBezier2D(Vector2 start, Vector2 control_point1, Vector2 control_point2, Vector2 end)
			{
				mQuality = 30;
				mPoints = new Vector2[4];
				mStartPoint = start;
				mControlPoint1 = control_point1;
				mControlPoint2 = control_point2;
				mEndPoint = end;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на сплайне
			/// </summary>
			/// <param name="time">Положение точки от 0 до 1</param>
			/// <returns>Позиция точки на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Vector2 CalculatePoint(Single time)
			{
				Vector2 start = new Vector2(mStartPoint.x, Screen.height - mStartPoint.y);
				Vector2 control_point1 = new Vector2(mControlPoint1.x, Screen.height - mControlPoint1.y);
				Vector2 control_point2 = new Vector2(mControlPoint2.x, Screen.height - mControlPoint2.y);
				Vector2 end = new Vector2(mEndPoint.x, Screen.height - mEndPoint.y);

				Single u = 1 - time;
				Single tt = time * time;
				Single uu = u * u;
				Single uuu = uu * u;
				Single ttt = tt * time;

				Vector2 point = uuu * start;

				point += 3 * uu * time * control_point1;
				point += 3 * u * tt * control_point2;
				point += ttt * end;

				return point;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение примитива
			/// </summary>
			/// <param name="offset">Смещение</param>
			//---------------------------------------------------------------------------------------------------------
			public override void MoveTo(Vector2 offset)
			{
				mStartPoint += offset;
				mControlPoint1 += offset;
				mControlPoint2 += offset;
				mEndPoint += offset;

				for (Int32 i = 0; i < mPoints.Length; i++)
				{
					mPoints[i] += offset;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание кубической кривой Безье с плавным соединением начальной и конечной точки
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="end">Конечная точка</param>
			/// <param name="coeff_wave">Коэффициент определяющий плавность перехода </param>
			//---------------------------------------------------------------------------------------------------------
			public void CreateFromStartToEnd(Vector2 start, Vector2 end, Single coeff_wave = 0.5f)
			{
				Single cp1x = Math.Abs(start.x - end.x) * coeff_wave;

				if (start.x > end.x) cp1x *= -1;

				mStartPoint = start;
				mControlPoint1 = new Vector2(start.x + cp1x, start.y);
				mControlPoint2 = new Vector2(end.x - cp1x, end.y);
				mEndPoint = end;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание кубической кривой Безье проходящий через заданные точки на равномерно заданном времени
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="point1">Первая точка</param>
			/// <param name="point2">Вторая точка</param>
			/// <param name="end">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void CreateFromPassesPoint(Vector3 start, Vector3 point1, Vector3 point2, Vector3 end)
			{
				mStartPoint = start;
				mControlPoint1.x = (-5 * start.x + 18 * point1.x - 9 * point2.x + 2 * end.x) / 6;
				mControlPoint1.y = (-5 * start.y + 18 * point1.y - 9 * point2.y + 2 * end.y) / 6;
				mControlPoint2.x = (2 * start.x - 9 * point1.x + 18 * point2.x - 5 * end.x) / 6;
				mControlPoint2.y = (2 * start.y - 9 * point1.y + 18 * point2.y - 5 * end.y) / 6;
				mEndPoint = end;
			}
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация кривой
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Generate()
			{
				mModeDraw = GL.LINE_STRIP;

				Vector2 start = new Vector2(mStartPoint.x, Screen.height - mStartPoint.y);
				Vector2 end = new Vector2(mEndPoint.x, Screen.height - mEndPoint.y);

				Resize(mQuality + 1);

				Vector2 prev = start;
				mPoints[0] = start;
				Int32 index = 0;
				for (Int32 i = 1; i < mQuality; i++)
				{
					Single time = (Single)i / mQuality;

					Vector2 point = CalculatePoint(time);

					// Добавляем если длина больше
					if ((point - prev).sqrMagnitude > 4)
					{
						index++;
						mPoints[index] = point;
						prev = point;
					}
				}

				index++;
				mPoints[index] = end;

				Array.Resize(ref mPoints, index + 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация кривой со штриховыми линиями
			/// </summary>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegment(Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.LINES;

				Resize(mQuality + 1);

				Vector2 prev = new Vector2(mStartPoint.x, Screen.height - mStartPoint.y);
				Int32 index = 0;
				mPoints[0] = prev;
				for (Int32 i = 1; i < mQuality + 1; i++)
				{
					Single time = (Single)i / mQuality;

					Vector2 point = CalculatePoint(time);

					// Добавляем если длина больше
					if ((point - prev).sqrMagnitude > 4)
					{
						if (i == 1)
						{
							index++;
							mPoints[index] = point;
						}

						if (i > 2 && i % 2 != 0)
						{
							index++;
							mPoints[index] = prev;
							

							index++;
							mPoints[index] = point;
							
						}

						prev = point;
					}
				}

				Array.Resize(ref mPoints, index + 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация кривой с линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateThickness(Single thickness)
			{
				mModeDraw = GL.TRIANGLES;

				Vector2 start = new Vector2(mStartPoint.x, Screen.height - mStartPoint.y);
				Vector2 end = new Vector2(mEndPoint.x, Screen.height - mEndPoint.y);

				Resize(mQuality * 4);

				Vector2 prev = start;
				Int32 offset = 0;
				for (Int32 i = 1; i < mQuality; i++)
				{
					Single time = (Single)i / mQuality;
					Vector2 point = CalculatePoint(time);

					// Добавляем если длина больше
					if ((point - prev).sqrMagnitude > 4)
					{
						offset = GenerateThicknessLineNotTransform(offset, ref prev, ref point, thickness, false);
						prev = point;
					}
				}

				GenerateThicknessLineNotTransform(offset, ref prev, ref end, thickness, false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			///Генерация кривой штриховыми линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegmentThickness(Single thickness, Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.TRIANGLES;

				Resize(mQuality * 4);

				Vector2 prev = new Vector2(mStartPoint.x, Screen.height - mStartPoint.y);
				Int32 offset = 0;
				for (Int32 i = 1; i < mQuality + 1; i++)
				{
					Single time = (Single)i / mQuality;

					Vector2 point = CalculatePoint(time);

					//Добавляем если длина больше
					if ((point - prev).sqrMagnitude > 4)
					{
						if (i == 1)
						{
							offset = GenerateThicknessLineNotTransform(offset, ref prev, ref point, thickness, false);
						}

						if (i > 2 && i % 2 != 0)
						{
							offset = GenerateThicknessLineNotTransform(offset, ref prev, ref point, thickness, false);
						}

						prev = point;
					}
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Стрелка
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CPrimitiveArrow2D : CPrimitive2D
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Vector2 mStartPoint;
			internal Single mHeadHeight;
			internal Single mHeadWidth;
			internal Vector2 mEndPoint;
			internal Boolean mIsFillArrow;
			internal Vector2 mPointTop;
			internal Vector2 mPointBottom;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Начальная точка стрелки
			/// </summary>
			public Vector2 StartPoint
			{
				get { return mStartPoint; }
				set
				{
					mStartPoint = value;
				}
			}

			/// <summary>
			/// Высота стрелки
			/// </summary>
			public Single HeadHeight
			{
				get { return mHeadHeight; }
				set
				{
					mHeadHeight = value;
				}
			}

			/// <summary>
			/// Ширина стрелки
			/// </summary>
			public Single HeadWidth
			{
				get { return mHeadWidth; }
				set
				{
					mHeadWidth = value;
				}
			}

			/// <summary>
			/// Конечная точка стрелки
			/// </summary>
			/// <remarks>
			/// Это место куда указывает стрелка
			/// </remarks>
			public Vector2 EndPoint
			{
				get { return mEndPoint; }
				set
				{
					mEndPoint = value;
				}
			}

			/// <summary>
			/// Статус заполненной стрелки
			/// </summary>
			public Boolean IsFillArrow
			{
				get { return mIsFillArrow; }
				set
				{
					mIsFillArrow = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveArrow2D()
			{
				mPoints = new Vector2[6];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="start">Начальная точка стрелки</param>
			/// <param name="end">Конечная точка стрелки</param>
			/// <param name="head_height">Высота стрелки</param>
			/// <param name="head_width">Ширина стрелки</param>
			//---------------------------------------------------------------------------------------------------------
			public CPrimitiveArrow2D(Vector2 start, Vector2 end, Single head_height, Single head_width)
			{
				mPoints = new Vector2[6];
				mStartPoint = start;
				mHeadHeight = head_height;
				mHeadWidth = head_width;
				mEndPoint = end;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация простой стрелки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Generate()
			{
				mModeDraw = GL.LINES;

				Single theta = Mathf.Atan2(mStartPoint.y - mEndPoint.y, mStartPoint.x - mEndPoint.x);
				Single sint = Mathf.Sin(theta);
				Single cost = Mathf.Cos(theta);

				mPointTop = new Vector2(mEndPoint.x + (mHeadHeight * cost - mHeadWidth * sint),
										mEndPoint.y + (mHeadHeight * sint + mHeadWidth * cost));

				mPointBottom = new Vector2(mEndPoint.x + (mHeadHeight * cost + mHeadWidth * sint),
									mEndPoint.y - (mHeadWidth * cost - mHeadHeight * sint));

				mPoints[0] = new Vector2(mStartPoint.x, Screen.height - mStartPoint.y);
				mPoints[1] = new Vector2(mEndPoint.x, Screen.height - mEndPoint.y);
				mPoints[2] = new Vector2(mPointTop.x, Screen.height - mPointTop.y);
				mPoints[3] = new Vector2(mEndPoint.x, Screen.height - mEndPoint.y);
				mPoints[4] = new Vector2(mPointBottom.x, Screen.height - mPointBottom.y);
				mPoints[5] = new Vector2(mEndPoint.x, Screen.height - mEndPoint.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация стрелки со штриховыми линиями
			/// </summary>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegment(Int32 segment_size, Single space_size = 0.5f)
			{


			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация стрелки с линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateThickness(Single thickness)
			{
				mModeDraw = GL.TRIANGLES;
				SetSizeFromCount(3);

				Single theta = Mathf.Atan2(mStartPoint.y - mEndPoint.y, mStartPoint.x - mEndPoint.x);
				Single sint = Mathf.Sin(theta);
				Single cost = Mathf.Cos(theta);

				mPointTop = new Vector2(mEndPoint.x + (mHeadHeight * cost - mHeadWidth * sint),
										mEndPoint.y + (mHeadHeight * sint + mHeadWidth * cost));

				mPointBottom = new Vector2(mEndPoint.x + (mHeadHeight * cost + mHeadWidth * sint),
									mEndPoint.y - (mHeadWidth * cost - mHeadHeight * sint));

				Int32 offset = GenerateThicknessLine(0, mPointTop, mEndPoint, thickness, false);
				offset = GenerateThicknessLine(offset, mEndPoint, mPointBottom, thickness, false);
				mCornerMode = 0;
				offset = GenerateThicknessLine(offset, mStartPoint, mEndPoint, thickness);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			///Генерация стрелки штриховыми линиями указанной толщиной
			/// </summary>
			/// <param name="thickness">Толщина линии</param>
			/// <param name="segment_size">Длина сегмента линии</param>
			/// <param name="space_size">Относительный размер пробела в сегменте линии</param>
			//---------------------------------------------------------------------------------------------------------
			public void GenerateSegmentThickness(Single thickness, Int32 segment_size, Single space_size = 0.5f)
			{
				mModeDraw = GL.TRIANGLES;
			}
			#endregion

			#region ======================================= МЕТОДЫ РИСОВАНИЯ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование примитива
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void Draw()
			{
				if (mIsFillArrow)
				{
					XGUIRender.ColoredMaterial2D.SetPass(0);

					GL.PushMatrix();
					GL.LoadPixelMatrix();
					GL.Begin(GL.TRIANGLES);
					{
						GL.Color(XGUIRender.FillColor);
						GL.Vertex(new Vector3(mPointTop.x, Screen.height - mPointTop.y));
						GL.Vertex(new Vector3(mEndPoint.x, Screen.height - mEndPoint.y));
						GL.Vertex(new Vector3(mPointBottom.x, Screen.height - mPointBottom.y));
					}
					GL.End();
					GL.PopMatrix();
				}

				base.Draw();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================