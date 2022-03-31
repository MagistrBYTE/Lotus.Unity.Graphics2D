//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Методы расширений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIExtensionRectTransform.cs
*		Методы расширения компонента RectTransform.
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
		//! \defgroup Unity2DUIExtension Методы расширений
		//! Методы расширения модуля компонентов Unity UI
		//! \ingroup Unity2DUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений компонента <see cref="RectTransform"/> 
		/// </summary>
		/// <remarks>
		/// <para>
		/// Еще одной задачей является корректное преобразование между экранными/локальными координатами и координатами во 
		/// время разработки. Подсистема Unity UI автоматические обеспечивает преобразование координат, однако 
		/// если используется относительные координаты (например позиция тайла) то для преобразования нужно использовать другие методы 
		/// </para>
		/// <para>
		/// Семантика методов
		/// [Local/World][Система координат]
		/// Left и X - синонимы
		/// Top и Y - синонимы
		/// Префикс/суффикс Position всегда относится к верхнему левому углу
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionRectTransform
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Набор вершин прямоугольника элемента в мировых координатах 
			/// </summary>
			static private readonly Vector3[] WolrdCorners = new Vector3[4] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero};
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получения горизонтального выравнивания
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Горизонтальное выравнивание</returns>
			//---------------------------------------------------------------------------------------------------------
			public static THorizontalAlignment GetHorizontalAlignment(this RectTransform @this)
			{
				if (XMath.Approximately(@this.anchorMin.x, 0, 0.01f) && XMath.Approximately(@this.anchorMax.x, 0, 0.01f))
				{
					return THorizontalAlignment.Left;
				}
				if (XMath.Approximately(@this.anchorMin.x, 0.5f, 0.01f) && XMath.Approximately(@this.anchorMax.x, 0.5f, 0.01f))
				{
					return THorizontalAlignment.Center;
				}
				if (XMath.Approximately(@this.anchorMin.x, 1f, 0.01f) && XMath.Approximately(@this.anchorMax.x, 1f, 0.01f))
				{
					return THorizontalAlignment.Right;
				}
				if (XMath.Approximately(@this.anchorMin.x, 0f, 0.01f) && XMath.Approximately(@this.anchorMax.x, 1f, 0.01f))
				{
					return THorizontalAlignment.Stretch;
				}

				return THorizontalAlignment.Unknow;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка горизонтального выравнивания
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="alignment">Горизонтальное выравнивание</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetHorizontalAlignment(this RectTransform @this, THorizontalAlignment alignment)
			{
				Single left = @this.GetLeft();
				Single width = @this.rect.width;

				switch (alignment)
				{
					case THorizontalAlignment.Left:
						{
							@this.anchorMin = new Vector2(0, @this.anchorMin.y);
							@this.anchorMax = new Vector2(0, @this.anchorMax.y);
						}
						break;
					case THorizontalAlignment.Center:
						{
							@this.anchorMin = new Vector2(0.5f, @this.anchorMin.y);
							@this.anchorMax = new Vector2(0.5f, @this.anchorMax.y);
						}
						break;
					case THorizontalAlignment.Right:
						{
							@this.anchorMin = new Vector2(1f, @this.anchorMin.y);
							@this.anchorMax = new Vector2(1f, @this.anchorMax.y);
						}
						break;
					case THorizontalAlignment.Stretch:
						{
							@this.anchorMin = new Vector2(0f, @this.anchorMin.y);
							@this.anchorMax = new Vector2(1f, @this.anchorMax.y);
						}
						break;
					case THorizontalAlignment.Unknow:
						break;
					default:
						break;
				}

				@this.SetWidth(width);
				@this.SetLeft(left);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получения вертикального выравнивания
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Вертикальное выравнивание</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TVerticalAlignment GetVerticalAlignment(this RectTransform @this)
			{
				if (XMath.Approximately(@this.anchorMin.y, 0, 0.01f) && XMath.Approximately(@this.anchorMax.y, 0, 0.01f))
				{
					return TVerticalAlignment.Bottom;
				}
				if (XMath.Approximately(@this.anchorMin.y, 0.5f, 0.01f) && XMath.Approximately(@this.anchorMax.y, 0.5f, 0.01f))
				{
					return TVerticalAlignment.Middle;
				}
				if (XMath.Approximately(@this.anchorMin.y, 1f, 0.01f) && XMath.Approximately(@this.anchorMax.y, 1f, 0.01f))
				{
					return TVerticalAlignment.Top;
				}
				if (XMath.Approximately(@this.anchorMin.y, 0f, 0.01f) && XMath.Approximately(@this.anchorMax.y, 1f, 0.01f))
				{
					return TVerticalAlignment.Stretch;
				}

				return TVerticalAlignment.Unknow;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка вертикального выравнивания
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="alignment">Вертикальное выравнивание</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetVerticalAlignment(this RectTransform @this, TVerticalAlignment alignment)
			{
				Single top = @this.GetTop();
				Single height = @this.rect.height;

				switch (alignment)
				{
					case TVerticalAlignment.Top:
						{
							@this.anchorMin = new Vector2(@this.anchorMin.x, 1);
							@this.anchorMax = new Vector2(@this.anchorMax.x, 1);
						}
						break;
					case TVerticalAlignment.Middle:
						{
							@this.anchorMin = new Vector2(@this.anchorMin.x, 0.5f);
							@this.anchorMax = new Vector2(@this.anchorMax.x, 0.5f);
						}
						break;
					case TVerticalAlignment.Bottom:
						{
							@this.anchorMin = new Vector2(@this.anchorMin.x, 0.0f);
							@this.anchorMax = new Vector2(@this.anchorMax.x, 0.0f);
						}
						break;
					case TVerticalAlignment.Stretch:
						{
							@this.anchorMin = new Vector2(@this.anchorMin.x, 0);
							@this.anchorMax = new Vector2(@this.anchorMax.x, 1);
						}
						break;
					case TVerticalAlignment.Unknow:
						break;
					default:
						break;
				}

				@this.SetHeight(height);
				@this.SetTop(top);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка выравнивания
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="h_align">Горизонтальное выравнивание</param>
			/// <param name="v_align">Вертикальное выравнивание</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetAlignment(this RectTransform @this, THorizontalAlignment h_align, TVerticalAlignment v_align)
			{
				SetHorizontalAlignment(@this, h_align);
				SetVerticalAlignment(@this, v_align);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка предпоследним в иерархии родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetAsPreLastSibling(this RectTransform @this)
			{
				Transform parent = @this.parent;
				Int32 child_count = parent.childCount;
				Int32 index = @this.GetSiblingIndex();

				if (child_count > 1)
				{
					index = child_count - 2;
					@this.SetSiblingIndex(index);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение вверх по иерархии родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ToFront(this RectTransform @this)
			{
				@this.SetSiblingIndex(@this.GetSiblingIndex() + 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение вниз по иерархии родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ToBack(this RectTransform @this)
			{
				@this.SetSiblingIndex(@this.GetSiblingIndex() - 1);
			}
			#endregion

			#region ======================================= Local coordinate ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции левого-верхнего угла по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X левого-верхнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLeft(this RectTransform @this, Single left)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}
				Single x = left + @this.pivot.x * @this.sizeDelta.x - width_parent * @this.anchorMin.x;
				@this.anchoredPosition = new Vector2(x, @this.anchoredPosition.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Позиция по X левого-верхнего угла</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetLeft(this RectTransform @this)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}
				return @this.anchoredPosition.x - @this.pivot.x * @this.sizeDelta.x + width_parent * @this.anchorMin.x;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции левого-верхнего угла по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="top">Позиция по Y левого-верхнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetTop(this RectTransform @this, Single top)
			{
				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					Single y = -top - (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					@this.anchoredPosition = new Vector2(@this.anchoredPosition.x, y);
				}
				else
				{
					RectTransform rect_parent = @this.parent as RectTransform;
					Single height_parent = 0;
					if (rect_parent != null)
					{
						height_parent = rect_parent.rect.height;
					}
					Single y = -top - (1.0f - @this.pivot.y) * @this.sizeDelta.y + height_parent * (1.0f - @this.anchorMin.y);
					@this.anchoredPosition = new Vector2(@this.anchoredPosition.x, y);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Позиция по Y левого-верхнего угла</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetTop(this RectTransform @this)
			{
				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					return (-(@this.anchoredPosition.y + (1.0f - @this.pivot.y) * @this.sizeDelta.y));
				}
				else
				{
					RectTransform rect_parent = @this.parent as RectTransform;
					Single height_parent = 0;
					if (rect_parent != null)
					{
						height_parent = rect_parent.rect.height;
					}

					return (-(@this.anchoredPosition.y + (1.0f - @this.pivot.y) * @this.sizeDelta.y - height_parent * (1.0f - @this.anchorMin.y)));
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции правого угла по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="right">Позиция по X правого угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetRight(this RectTransform @this, Single right)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}
				Single left = @this.anchoredPosition.x - @this.pivot.x * @this.sizeDelta.x + width_parent * @this.anchorMin.x;
				Single width = width_parent - (left + right);

				if (XMath.Approximately(@this.anchorMin.x, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.x, 1.0f, XMath.Eplsilon_3f))
				{
					@this.sizeDelta = new Vector2(-(left + right), @this.sizeDelta.y);
				}
				else
				{
					@this.sizeDelta = new Vector2(width, @this.sizeDelta.y);
				}

				Single x = left + @this.pivot.x * @this.sizeDelta.x - width_parent * @this.anchorMin.x;
				@this.anchoredPosition = new Vector2(x, @this.anchoredPosition.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции правого угла по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Позиция по X правого угла</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetRight(this RectTransform @this)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}

				Single left = @this.anchoredPosition.x - @this.pivot.x * @this.sizeDelta.x + width_parent * @this.anchorMin.x;
				return width_parent - (left + @this.rect.width);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции нижнего угла по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="bottom">Позиция по Y нижнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetBottom(this RectTransform @this, Single bottom)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;

				Single height_parent = 0;
				if (rect_parent != null)
				{
					height_parent = rect_parent.rect.height;
				}
				Single parent_size = height_parent * (1.0f - @this.anchorMin.y);
				Single y = 0;

				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					y = @this.anchoredPosition.y + size;
					y = -y;
					Single top = y;

					@this.sizeDelta = new Vector2(@this.sizeDelta.x, -(top + bottom));

					size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					top = -top;
					y = top - size;
				}
				else
				{
					y = @this.anchoredPosition.y + size - parent_size;
					y = -y;
					Single top = y;
					Single height = height_parent - (y + bottom);

					@this.sizeDelta = new Vector2(@this.sizeDelta.x, height);

					size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					top = -top;
					y = top - size + parent_size;
				}

				@this.anchoredPosition = new Vector2(@this.anchoredPosition.x, y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции нижнего угла по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Позиция по Y нижнего угла</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetBottom(this RectTransform @this)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;

				Single height_parent = 0;
				if (rect_parent != null)
				{
					height_parent = rect_parent.rect.height;
				}
				Single parent_size = height_parent * (1.0f - @this.anchorMin.y);
				Single y = 0;

				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					y = @this.anchoredPosition.y + size;
					y = -y;
				}
				else
				{
					y = @this.anchoredPosition.y + size - parent_size;
					y = -y;
				}

				return height_parent - (y + @this.rect.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X левого-верхнего угла</param>
			/// <param name="top">Позиция по Y левого-верхнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetPosition(this RectTransform @this, Single left, Single top)
			{
				RectTransform rect_parent = @this.parent as RectTransform;

				Single height_parent = 0;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					height_parent = rect_parent.rect.height;
					width_parent = rect_parent.rect.width;
				}

				Single x = left + @this.pivot.x * @this.sizeDelta.x - width_parent * @this.anchorMin.x;
				Single y = 0;

				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;

					top = -top;
					y = top - size;
				}
				else
				{
					Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					Single parent_size = height_parent * (1.0f - @this.anchorMin.y);

					top = -top;
					y = top - size + parent_size;
				}

				@this.anchoredPosition = new Vector2(x, y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка ширины (сохраняется текущие положение по X)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="width">Ширина</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetWidth(this RectTransform @this, Single width)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 100;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}
				Single left = @this.anchoredPosition.x - @this.pivot.x * @this.sizeDelta.x + width_parent * @this.anchorMin.x;

				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.x, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.x, 1.0f, XMath.Eplsilon_3f))
				{
					Single right = width_parent - width - left;
					@this.sizeDelta = new Vector2(-(left + right), @this.sizeDelta.y);
				}
				else
				{
					@this.sizeDelta = new Vector2(width, @this.sizeDelta.y);
				}

				Single x = left + @this.pivot.x * @this.sizeDelta.x - width_parent * @this.anchorMin.x;
				@this.anchoredPosition = new Vector2(x, @this.anchoredPosition.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка ширины
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="width">Ширина</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetImmediateWidth(this RectTransform @this, Single width)
			{
				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.x, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.x, 1.0f, XMath.Eplsilon_3f))
				{
					// Получаем родителя
					RectTransform rect_parent = @this.parent as RectTransform;
					Single width_parent = 0;
					if (rect_parent != null)
					{
						width_parent = rect_parent.rect.width;
					}

					Single left = @this.GetLeft();
					Single right = width_parent - width - left;

					@this.sizeDelta = new Vector2(-(left + right), @this.sizeDelta.y);
				}
				else
				{
					@this.sizeDelta = new Vector2(width, @this.sizeDelta.y);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты (сохраняется текущие положение по Y)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="height">Высота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetHeight(this RectTransform @this, Single height)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;

				Single height_parent = 0;
				if (rect_parent != null)
				{
					height_parent = rect_parent.rect.height;
				}
				Single parent_size = height_parent * (1.0f - @this.anchorMin.y);
				Single y = 0;

				// Растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
					XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					Single top = -(@this.anchoredPosition.y + size);
					Single bottom = height_parent - height - top;

					@this.sizeDelta = new Vector2(@this.sizeDelta.x, -(top + bottom));

					size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					top = -top;
					y = top - size;
				}
				else
				{
					// Получили высоту
					Single top = @this.anchoredPosition.y + size - parent_size;

					// Установили размер
					@this.sizeDelta = new Vector2(@this.sizeDelta.x, height);

					size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					y = top - size + parent_size;
				}

				@this.anchoredPosition = new Vector2(@this.anchoredPosition.x, y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="height">Высота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetImmediateHeight(this RectTransform @this, Single height)
			{
				// Растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
					XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					// Получаем родителя
					RectTransform rt = @this.parent as RectTransform;
					Single height_parent = rt.rect.height;

					Single top = @this.GetTop();
					Single bottom = height_parent - height - top;

					@this.sizeDelta = new Vector2(@this.sizeDelta.x, -(top + bottom));
				}
				else
				{
					@this.sizeDelta = new Vector2(@this.sizeDelta.x, height);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты и ширины (сохраняются текущие положение)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetSize(this RectTransform @this, Single width, Single height)
			{
				@this.SetWidth(width);
				@this.SetHeight(height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты и ширины
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetSizeImmediate(this RectTransform @this, Single width, Single height)
			{
				@this.SetImmediateWidth(width);
				@this.SetImmediateHeight(height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты и ширины
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="size">Размеры</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetSizeImmediate(this RectTransform @this, Vector2 size)
			{
				@this.SetImmediateWidth(size.x);
				@this.SetImmediateHeight(size.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размеров
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X верхнего/левого-верхнего угла от уровня родительской области</param>
			/// <param name="top">Позиция по Y верхнего/левого-верхнего угла от уровня родительской области</param>
			/// <param name="width">Ширина объекта</param>
			/// <param name="height">Высота объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Set(this RectTransform @this, Single left, Single top, Single width, Single height)
			{
				@this.SetWidth(width);
				@this.SetHeight(height);
				@this.SetLeft(left);
				@this.SetTop(top);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размеров
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="pos_size">Отступ и размеры
			/// От левого края - Vector.x
			/// От верхнего края - Vector.y
			/// Ширина - Vector.z
			/// Высота - Vector.w</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Set(this RectTransform @this, Vector4 pos_size)
			{
				@this.SetWidth(pos_size.z);
				@this.SetHeight(pos_size.w);
				@this.SetLeft(pos_size.x);
				@this.SetTop(pos_size.y);
			}
			#endregion

			#region ======================================= Local Design coordinate ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции левого-верхнего угла по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X левого-верхнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignLeft(this RectTransform @this, Single left)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}
				Single x = left + @this.pivot.x * @this.sizeDelta.x - width_parent * @this.anchorMin.x;
				@this.anchoredPosition = new Vector2(x, @this.anchoredPosition.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Позиция по X левого-верхнего угла</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetLocalDesignLeft(this RectTransform @this)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}
				return @this.anchoredPosition.x - @this.pivot.x * @this.sizeDelta.x + width_parent * @this.anchorMin.x;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции левого-верхнего угла по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="top">Позиция по Y левого-верхнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignTop(this RectTransform @this, Single top)
			{
				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					Single y = -top - (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					@this.anchoredPosition = new Vector2(@this.anchoredPosition.x, y);
				}
				else
				{
					RectTransform rect_parent = @this.parent as RectTransform;
					Single height_parent = 0;
					if (rect_parent != null)
					{
						height_parent = rect_parent.rect.height;
					}
					Single y = -top - (1.0f - @this.pivot.y) * @this.sizeDelta.y + height_parent * (1.0f - @this.anchorMin.y);
					@this.anchoredPosition = new Vector2(@this.anchoredPosition.x, y);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Позиция по Y левого-верхнего угла</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetLocalDesignTop(this RectTransform @this)
			{
				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					return (-(@this.anchoredPosition.y + (1.0f - @this.pivot.y) * @this.sizeDelta.y));
				}
				else
				{
					RectTransform rect_parent = @this.parent as RectTransform;
					Single height_parent = 0;
					if (rect_parent != null)
					{
						height_parent = rect_parent.rect.height;
					}

					return (-(@this.anchoredPosition.y + (1.0f - @this.pivot.y) * @this.sizeDelta.y - height_parent * (1.0f - @this.anchorMin.y)));
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции правого угла по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="right">Позиция по X правого угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignRight(this RectTransform @this, Single right)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}
				Single left = @this.anchoredPosition.x - @this.pivot.x * @this.sizeDelta.x + width_parent * @this.anchorMin.x;
				Single width = width_parent - (left + right);

				if (XMath.Approximately(@this.anchorMin.x, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.x, 1.0f, XMath.Eplsilon_3f))
				{
					@this.sizeDelta = new Vector2(-(left + right), @this.sizeDelta.y);
				}
				else
				{
					@this.sizeDelta = new Vector2(width, @this.sizeDelta.y);
				}

				Single x = left + @this.pivot.x * @this.sizeDelta.x - width_parent * @this.anchorMin.x;
				@this.anchoredPosition = new Vector2(x, @this.anchoredPosition.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции правого угла по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Позиция по X правого угла</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetLocalDesignRight(this RectTransform @this)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}

				Single left = @this.anchoredPosition.x - @this.pivot.x * @this.sizeDelta.x + width_parent * @this.anchorMin.x;
				return width_parent - (left + @this.rect.width);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции нижнего угла по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="bottom">Позиция по Y нижнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignBottom(this RectTransform @this, Single bottom)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;

				Single height_parent = 0;
				if (rect_parent != null)
				{
					height_parent = rect_parent.rect.height;
				}
				Single parent_size = height_parent * (1.0f - @this.anchorMin.y);
				Single y = 0;

				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					y = @this.anchoredPosition.y + size;
					y = -y;
					Single top = y;

					@this.sizeDelta = new Vector2(@this.sizeDelta.x, -(top + bottom));

					size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					top = -top;
					y = top - size;
				}
				else
				{
					y = @this.anchoredPosition.y + size - parent_size;
					y = -y;
					Single top = y;
					Single height = height_parent - (y + bottom);

					@this.sizeDelta = new Vector2(@this.sizeDelta.x, height);

					size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					top = -top;
					y = top - size + parent_size;
				}

				@this.anchoredPosition = new Vector2(@this.anchoredPosition.x, y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции нижнего угла по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Позиция по Y нижнего угла</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetLocalDesignBottom(this RectTransform @this)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;

				Single height_parent = 0;
				if (rect_parent != null)
				{
					height_parent = rect_parent.rect.height;
				}
				Single parent_size = height_parent * (1.0f - @this.anchorMin.y);
				Single y = 0;

				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					y = @this.anchoredPosition.y + size;
					y = -y;
				}
				else
				{
					y = @this.anchoredPosition.y + size - parent_size;
					y = -y;
				}

				return height_parent - (y + @this.rect.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X левого-верхнего угла</param>
			/// <param name="top">Позиция по Y левого-верхнего угла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignPosition(this RectTransform @this, Single left, Single top)
			{
				RectTransform rect_parent = @this.parent as RectTransform;

				Single height_parent = 0;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					height_parent = rect_parent.rect.height;
					width_parent = rect_parent.rect.width;
				}

				Single x = left + @this.pivot.x * @this.sizeDelta.x - width_parent * @this.anchorMin.x;
				Single y = 0;

				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;

					top = -top;
					y = top - size;
				}
				else
				{
					Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					Single parent_size = height_parent * (1.0f - @this.anchorMin.y);

					top = -top;
					y = top - size + parent_size;
				}

				@this.anchoredPosition = new Vector2(x, y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка ширины (сохраняется текущие положение по X)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="width">Ширина</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignWidth(this RectTransform @this, Single width)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 100;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}
				Single left = @this.anchoredPosition.x - @this.pivot.x * @this.sizeDelta.x + width_parent * @this.anchorMin.x;

				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.x, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.x, 1.0f, XMath.Eplsilon_3f))
				{
					Single right = width_parent - width - left;
					@this.sizeDelta = new Vector2(-(left + right), @this.sizeDelta.y);
				}
				else
				{
					@this.sizeDelta = new Vector2(width, @this.sizeDelta.y);
				}

				Single x = left + @this.pivot.x * @this.sizeDelta.x - width_parent * @this.anchorMin.x;
				@this.anchoredPosition = new Vector2(x, @this.anchoredPosition.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка ширины
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="width">Ширина</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignImmediateWidth(this RectTransform @this, Single width)
			{
				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.x, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.x, 1.0f, XMath.Eplsilon_3f))
				{
					// Получаем родителя
					RectTransform rect_parent = @this.parent as RectTransform;
					Single width_parent = 0;
					if (rect_parent != null)
					{
						width_parent = rect_parent.rect.width;
					}

					Single left = @this.GetLeft();
					Single right = width_parent - width - left;

					@this.sizeDelta = new Vector2(-(left + right), @this.sizeDelta.y);
				}
				else
				{
					@this.sizeDelta = new Vector2(width, @this.sizeDelta.y);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты (сохраняется текущие положение по Y)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="height">Высота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignHeight(this RectTransform @this, Single height)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;

				Single height_parent = 0;
				if (rect_parent != null)
				{
					height_parent = rect_parent.rect.height;
				}
				Single parent_size = height_parent * (1.0f - @this.anchorMin.y);
				Single y = 0;

				// Растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
					XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					Single top = -(@this.anchoredPosition.y + size);
					Single bottom = height_parent - height - top;

					@this.sizeDelta = new Vector2(@this.sizeDelta.x, -(top + bottom));

					size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					top = -top;
					y = top - size;
				}
				else
				{
					// Получили высоту
					Single top = @this.anchoredPosition.y + size - parent_size;

					// Установили размер
					@this.sizeDelta = new Vector2(@this.sizeDelta.x, height);

					size = (1.0f - @this.pivot.y) * @this.sizeDelta.y;
					y = top - size + parent_size;
				}

				@this.anchoredPosition = new Vector2(@this.anchoredPosition.x, y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="height">Высота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignImmediateHeight(this RectTransform @this, Single height)
			{
				// Растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
					XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					// Получаем родителя
					RectTransform rt = @this.parent as RectTransform;
					Single height_parent = rt.rect.height;

					Single top = @this.GetTop();
					Single bottom = height_parent - height - top;

					@this.sizeDelta = new Vector2(@this.sizeDelta.x, -(top + bottom));
				}
				else
				{
					@this.sizeDelta = new Vector2(@this.sizeDelta.x, height);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты и ширины (сохраняются текущие положение)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignSize(this RectTransform @this, Single width, Single height)
			{
				@this.SetWidth(width);
				@this.SetHeight(height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты и ширины
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignSizeImmediate(this RectTransform @this, Single width, Single height)
			{
				@this.SetImmediateWidth(width);
				@this.SetImmediateHeight(height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты и ширины
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="size">Размеры</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesignSizeImmediate(this RectTransform @this, Vector2 size)
			{
				@this.SetImmediateWidth(size.x);
				@this.SetImmediateHeight(size.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размеров
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X верхнего/левого-верхнего угла от уровня родительской области</param>
			/// <param name="top">Позиция по Y верхнего/левого-верхнего угла от уровня родительской области</param>
			/// <param name="width">Ширина объекта</param>
			/// <param name="height">Высота объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesign(this RectTransform @this, Single left, Single top, Single width, Single height)
			{
				@this.SetWidth(width);
				@this.SetHeight(height);
				@this.SetLeft(left);
				@this.SetTop(top);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размеров
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="pos_size">Отступ и размеры
			/// От левого края - Vector.x
			/// От верхнего края - Vector.y
			/// Ширина - Vector.z
			/// Высота - Vector.w</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetLocalDesign(this RectTransform @this, Vector4 pos_size)
			{
				@this.SetWidth(pos_size.z);
				@this.SetHeight(pos_size.w);
				@this.SetLeft(pos_size.x);
				@this.SetTop(pos_size.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Прямоугольник от уровня родительской области</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect GetLocalDesignRect(this RectTransform @this)
			{
				Rect rect = new Rect(@this.GetLeft(), @this.GetTop(), @this.rect.width,
					@this.rect.height);

				return rect;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции от уровня родительской области по X по указанной позиции в условных координатах
			/// </summary>
			/// <remarks>
			/// В качестве аргумента метода необходимо передать результат метода <see cref="RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, Vector2, Camera, out Vector2)"/>
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="x">Позиция по X в условных координах</param>
			/// <returns>Позиция по X от уровня родительской области</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetLocalDesignLeftFromLocalTransform(this RectTransform @this, Single x)
			{
				return ((@this.pivot.x * @this.rect.width) + x);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции от уровня родительской области по Y по указанной позиции в условных координатах
			/// </summary>
			/// <remarks>
			/// В качестве аргумента метода необходимо передать результат метода <see cref="RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, Vector2, Camera, out Vector2)"/>
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="y">Позиция по Y в условных координах</param>
			/// <returns>Позиция по Y от уровня родительской области</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetLocalDesignTopFromLocalTransform(this RectTransform @this, Single y)
			{
				return (-y + ((1.0f - @this.pivot.y) * @this.rect.height));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции от уровня родительской области по указанной позиции в условных координатах
			/// </summary>
			/// <remarks>
			/// В качестве аргумента метода необходимо передать результат метода <see cref="RectTransformUtility.ScreenPointToLocalPointInRectangle(RectTransform, Vector2, Camera, out Vector2)"/>
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="x">Позиция по X в условных координах</param>
			/// <param name="y">Позиция по Y в условных координах</param>
			/// <returns>Позиция от уровня родительской области</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 GetLocalDesignPositionFromLocalTransform(this RectTransform @this, Single x, Single y)
			{
				Single left = (@this.pivot.x * @this.rect.width) + x;
				Single top = -y + ((1.0f - @this.pivot.y) * @this.rect.height);

				return (new Vector2(left, top));
			}
			#endregion

			#region ======================================= World Design coordinate ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции по X в экранных координатах
			/// </summary>
			/// <remarks>
			/// (Здесь по экранными координатами понимаются координаты и размеры экрана в режиме разработки)
			/// </remarks>
			/// <param name="@this">Прямоугольник трансформации</param>
			/// <returns>Позиция по X в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			private static Single GetWorldDesignX(this RectTransform @this)
			{
				if (@this.parent != null)
				{
					Canvas canvas = @this.parent.GetComponent<Canvas>();
					RectTransform rect_parent = @this.parent.GetComponent<RectTransform>();
					if (rect_parent != null)
					{
						if (canvas != null && canvas.isRootCanvas)
						{
							return @this.GetLeft();
						}
						else
						{
							return @this.GetLeft() + GetWorldDesignX(rect_parent);
						}
					}
				}

				return @this.GetLeft();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции по Y в экранных координатах
			/// </summary>
			/// <remarks>
			/// (Здесь по экранными координатами понимаются координаты и размеры экрана в режиме разработки)
			/// </remarks>
			/// <param name="@this">Прямоугольник трансформации</param>
			/// <returns>Позиция по Y в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			private static Single GetWorldDesignY(this RectTransform @this)
			{
				if (@this.parent != null)
				{
					Canvas canvas = @this.parent.GetComponent<Canvas>();
					RectTransform rect_parent = @this.parent.GetComponent<RectTransform>();
					if (rect_parent != null)
					{
						if (canvas != null && canvas.isRootCanvas)
						{
							return @this.GetTop();
						}
						else
						{
							return @this.GetTop() + GetWorldDesignY(rect_parent);
						}
					}
				}

				return @this.GetTop();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника в экранных координатах
			/// </summary>
			/// <param name="@this">Прямоугольник трансформации</param>
			/// <returns>Прямоугольник в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect GetWorldDesignRect(this RectTransform @this)
			{
				Rect rect = new Rect
				(
					@this.GetWorldDesignX(),
					@this.GetWorldDesignY(),
					@this.rect.width,
					@this.rect.height
				);

				return rect;
			}
			#endregion

			#region ======================================= Local Screen coordinate ===================================
			#endregion

			#region ======================================= World Screen coordinate ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции по X в экранных координатах
			/// </summary>
			/// <param name="@this">Прямоугольник трансформации</param>
			/// <returns>Позиция по X в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetWorldScreenX(this RectTransform @this)
			{
				return GetWorldDesignX(@this) * @this.lossyScale.x;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции по Y в экранных координатах
			/// </summary>
			/// <param name="@this">Прямоугольник трансформации</param>
			/// <returns>Позиция по Y в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetWorldScreenY(this RectTransform @this)
			{
				return GetWorldDesignY(@this) * @this.lossyScale.y;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника в экранных координатах
			/// </summary>
			/// <param name="@this">Прямоугольник трансформации</param>
			/// <returns>Прямоугольник в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect GetWorldScreenRect(this RectTransform @this)
			{
				Rect rect = new Rect
				(
					@this.GetWorldScreenX(),
					@this.GetWorldScreenY(),
					@this.rect.width * @this.lossyScale.x,
					@this.rect.height * @this.lossyScale.y
				);

				return rect;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции по X в экранных координатах с учетом дополнительной позиции по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X от уровня родительской области</param>
			/// <returns>Получение позиции по X в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetWorldScreenLeftFromLocalDesign(this RectTransform @this, Single left)
			{
				@this.GetWorldCorners(WolrdCorners);
				Single l = WolrdCorners[0].x;
				Single r = WolrdCorners[2].x;
				Single w = r - l;
				Single percent = left / @this.rect.width;
				return (l + w * percent);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции по Y в экранных координатах с учетом дополнительной позиции по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="top">Позиция по Y от уровня родительской области</param>
			/// <returns>Позиция по Y в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetWorldScreenTopFromLocalDesign(this RectTransform @this, Single top)
			{
				@this.GetWorldCorners(WolrdCorners);
				Single t = WolrdCorners[0].y;
				Single b = WolrdCorners[2].y;
				Single h = b - t;
				Single y = Screen.height - (t - h * (top / @this.rect.height) + @this.rect.height * @this.lossyScale.y);
				return (y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в экранных координатах с учетом дополнительной позиции от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X от уровня родительской области</param>
			/// <param name="top">Позиция по Y от уровня родительской области</param>
			/// <returns>Позиция в экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 GetWorldScreenFromLocalDesign(this RectTransform @this, Single left, Single top)
			{
				@this.GetWorldCorners(WolrdCorners);
				Single l = WolrdCorners[0].x;
				Single r = WolrdCorners[2].x;
				Single w = r - l;
				Single x = l + w * (left / @this.rect.width);

				Single t = WolrdCorners[0].y;
				Single b = WolrdCorners[2].y;
				Single h = b - t;
				Single y = Screen.height - (t - h * (top / @this.rect.height) + @this.rect.height * @this.lossyScale.y);

				return new Vector2(x, y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в перевёрнутых экранных координатах с учетом дополнительной позиции от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X от уровня родительской области</param>
			/// <param name="top">Позиция по Y от уровня родительской области</param>
			/// <returns>Позиция в перевёрнутых экранных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 GetWorldScreenInvFromLocalDesign(this RectTransform @this, Single left, Single top)
			{
				@this.GetWorldCorners(WolrdCorners);
				Single l = WolrdCorners[0].x;
				Single r = WolrdCorners[2].x;
				Single w = r - l;
				Single x = l + w * (left / @this.rect.width);

				Single t = WolrdCorners[0].y;
				Single b = WolrdCorners[2].y;
				Single h = b - t;
				Single y = t - h * (top / @this.rect.height) + @this.rect.height * @this.lossyScale.y;

				return new Vector2(x, y);
			}
			#endregion

			#region ======================================= Local Anchored coordinate =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в условных закрепленных координат по X с учетом дополнительной позиции по X от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X от уровня родительской области</param>
			/// <returns>Позиция по X в условных закрепленных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetLocalAnchoredLeftFromLocalDesign(this RectTransform @this, Single left)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}

				return (left - (width_parent * @this.anchorMin.x) + (@this.pivot.x * @this.sizeDelta.x));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в условных закрепленных координат по Y с учетом дополнительной позиции по Y от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="top">Позиция по Y от уровня родительской области</param>
			/// <returns>Позиция по Y в условных закрепленных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetLocalAnchoredTopFromLocalDesign(this RectTransform @this, Single top)
			{
				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					return (-top - (1.0f - @this.pivot.y) * @this.sizeDelta.y);
				}
				else
				{
					RectTransform rect_parent = @this.parent as RectTransform;
					Single height_parent = 0;
					if (rect_parent != null)
					{
						height_parent = rect_parent.rect.height;
					}

					return (-top - (1.0f - @this.pivot.y) * @this.sizeDelta.y + height_parent * (1.0f - @this.anchorMin.y));
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в условных закрепленных координат с учетом дополнительной позиции от уровня родительской области
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X от уровня родительской области</param>
			/// <param name="top">Позиция по Y от уровня родительской области</param>
			/// <returns>Позиция в условных закрепленных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 GetLocalAnchoredFromLocalDesign(this RectTransform @this, Single left, Single top)
			{
				RectTransform rect_parent = @this.parent as RectTransform;
				Single width_parent = 0;
				if (rect_parent != null)
				{
					width_parent = rect_parent.rect.width;
				}

				Single x = (left - (width_parent * @this.anchorMin.x) + (@this.pivot.x * @this.sizeDelta.x));

				// Если растянутый элемент
				if (XMath.Approximately(@this.anchorMin.y, 0.0f, XMath.Eplsilon_3f) &&
				   XMath.Approximately(@this.anchorMax.y, 1.0f, XMath.Eplsilon_3f))
				{
					return new Vector2(x, (-top - (1.0f - @this.pivot.y) * @this.sizeDelta.y));
				}
				else
				{
					Single height_parent = 0;
					if (rect_parent != null)
					{
						height_parent = rect_parent.rect.height;
					}

					return new Vector2(x, (-top - (1.0f - @this.pivot.y) * @this.sizeDelta.y + height_parent * (1.0f - @this.anchorMin.y)));
				}
			}
			#endregion

			#region ======================================= World Anchored coordinate =================================
			#endregion

			#region ======================================= Local Transform coordinate ================================
			#endregion

			#region ======================================= World Transform coordinate ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 GetWorldTransformPosition(this RectTransform @this)
			{
				Single offset_x = @this.pivot.x * @this.rect.width * @this.lossyScale.x;
				Single offset_y = (1.0f - @this.pivot.y) * @this.rect.height * @this.lossyScale.y;

				return (new Vector2(@this.position.x + offset_x, @this.position.y - offset_y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="position">Позиция позиции левого-верхнего угла </param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetWorldTransformPosition(this RectTransform @this, Vector3 position)
			{
				Single offset_x = @this.pivot.x * @this.rect.width * @this.lossyScale.x;
				Single offset_y = (1.0f - @this.pivot.y) * @this.rect.height * @this.lossyScale.y;

				@this.position = new Vector3(position.x + offset_x, position.y - offset_y, position.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <remarks>
			/// Позиция верхнего левого угла считается только для данного прямоугольника.
			/// Данный метод не предназначения для позиционирования других прямоугольников
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в экранных координатах</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromWorldScreen(this RectTransform @this, Single left, Single top)
			{
				return (GetWorldTransformPositionFromWorldScreen(@this, Camera.current, left, top));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <remarks>
			/// Позиция верхнего левого угла считается только для данного прямоугольника.
			/// Данный метод не предназначения для позиционирования других прямоугольников
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="screen_pos">Позиция в экранных координатах</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromWorldScreen(this RectTransform @this, Vector2 screen_pos)
			{
				return (GetWorldTransformPositionFromWorldScreen(@this, Camera.current, screen_pos.x, screen_pos.y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <remarks>
			/// Позиция верхнего левого угла считается только для данного прямоугольника.
			/// Данный метод не предназначения для позиционирования других прямоугольников
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="screen_pos">Позиция в экранных координатах</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromWorldScreen(this RectTransform @this, Camera camera, Vector2 screen_pos)
			{
				return (GetWorldTransformPositionFromWorldScreen(@this, camera, screen_pos.x, screen_pos.y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <remarks>
			/// Позиция верхнего левого угла считается только для данного прямоугольника.
			/// Данный метод не предназначения для позиционирования других прямоугольников
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в экранных координатах</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromWorldScreen(this RectTransform @this, Camera camera, Single left, Single top)
			{
				Vector2 screen_pos = new Vector2(left, Screen.height - top);
				Vector3 position = Vector3.zero;
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(@this, screen_pos, camera, out position))
				{
					Single offset_x = @this.pivot.x * @this.rect.width * @this.lossyScale.x;
					Single offset_y = (1.0f - @this.pivot.y) * @this.rect.height * @this.lossyScale.y;

					return (new Vector3(position.x + offset_x, position.y - offset_y, @this.position.z));
				}

				return (Vector3.zero);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в экранных координатах</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromWorldScreen(this RectTransform @this, Single left, Single top)
			{
				return (GetWorldTransformFromWorldScreen(@this, Camera.current, left, top));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="screen_pos">Позиция в экранных координатах</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromWorldScreen(this RectTransform @this, Vector2 screen_pos)
			{
				return (GetWorldTransformFromWorldScreen(@this, Camera.current, screen_pos.x, screen_pos.y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="screen_pos">Позиция в экранных координатах</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromWorldScreen(this RectTransform @this, Camera camera, Vector2 screen_pos)
			{
				return (GetWorldTransformFromWorldScreen(@this, camera, screen_pos.x, screen_pos.y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в экранных координатах</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromWorldScreen(this RectTransform @this, Camera camera, Single left, Single top)
			{
				Vector2 screen_pos = new Vector2(left, Screen.height - top);
				Vector3 position = Vector3.zero;
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(@this, screen_pos, camera, out position))
				{
					return (position);
				}

				return (Vector3.zero);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <remarks>
			/// Позиция верхнего левого угла считается только для данного прямоугольника.
			/// Данный метод не предназначения для позиционирования других прямоугольников
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в перевернутых экранных координатах</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromWorldScreenInv(this RectTransform @this, Single left, Single top)
			{
				return (GetWorldTransformPositionFromWorldScreenInv(@this, Camera.current, new Vector2(left, top)));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <remarks>
			/// Позиция верхнего левого угла считается только для данного прямоугольника.
			/// Данный метод не предназначения для позиционирования других прямоугольников
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в перевернутых экранных координатах</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromWorldScreenInv(this RectTransform @this, Camera camera, Single left, Single top)
			{
				return (GetWorldTransformPositionFromWorldScreenInv(@this, camera, new Vector2(left, top)));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <remarks>
			/// Позиция верхнего левого угла считается только для данного прямоугольника.
			/// Данный метод не предназначения для позиционирования других прямоугольников
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="screen_pos">Позиция в перевернутых экранных координатах</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromWorldScreenInv(this RectTransform @this, Vector2 screen_pos)
			{
				return (GetWorldTransformPositionFromWorldScreenInv(@this, Camera.current, screen_pos));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <remarks>
			/// Позиция верхнего левого угла считается только для данного прямоугольника.
			/// Данный метод не предназначения для позиционирования других прямоугольников
			/// </remarks>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="screen_pos">Позиция в перевернутых экранных координатах</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromWorldScreenInv(this RectTransform @this, Camera camera, Vector2 screen_pos)
			{
				Vector3 position = Vector3.zero;
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(@this, screen_pos, camera, out position))
				{
					Single offset_x = @this.pivot.x * @this.rect.width * @this.lossyScale.x;
					Single offset_y = (1.0f - @this.pivot.y) * @this.rect.height * @this.lossyScale.y;

					return (new Vector3(position.x + offset_x, position.y - offset_y, @this.position.z));
				}

				return (Vector3.zero);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в перевернутых экранных координатах</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromWorldScreenInv(this RectTransform @this, Single left, Single top)
			{
				return (GetWorldTransformFromWorldScreenInv(@this, Camera.current, new Vector2(left, top)));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="left">Позиция по X в экранных координатах</param>
			/// <param name="top">Позиция по Y в перевернутых экранных координатах</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromWorldScreenInv(this RectTransform @this, Camera camera, Single left, Single top)
			{
				return (GetWorldTransformFromWorldScreenInv(@this, camera, new Vector2(left, top)));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="screen_pos">Позиция в перевернутых экранных координатах</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromWorldScreenInv(this RectTransform @this, Vector2 screen_pos)
			{
				return (GetWorldTransformFromWorldScreenInv(@this, Camera.current, screen_pos));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="screen_pos">Позиция в перевернутых экранных координатах</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromWorldScreenInv(this RectTransform @this, Camera camera, Vector2 screen_pos)
			{
				Vector3 position = Vector3.zero;
				if (RectTransformUtility.ScreenPointToWorldPointInRectangle(@this, screen_pos, camera, out position))
				{
					return (position);
				}

				return (Vector3.zero);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X относительно родительской области</param>
			/// <param name="top">Позиция по Y относительно родительской области</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromLocalDesign(this RectTransform @this, Single left, Single top)
			{
				// 1) Переводим в актуальное экраное пространство
				Vector2 screen_pos = @this.GetWorldScreenInvFromLocalDesign(left, top);
				return (@this.GetWorldTransformFromWorldScreenInv(Camera.current, screen_pos));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="left">Позиция по X относительно родительской области</param>
			/// <param name="top">Позиция по Y относительно родительской области</param>
			/// <returns>Позиция в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformFromLocalDesign(this RectTransform @this, Camera camera, Single left, Single top)
			{
				// 1) Переводим в актуальное экраное пространство
				Vector2 screen_pos = @this.GetWorldScreenInvFromLocalDesign(left, top);
				return (@this.GetWorldTransformFromWorldScreenInv(camera, screen_pos));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="left">Позиция по X относительно родительской области</param>
			/// <param name="top">Позиция по Y относительно родительской области</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromLocalDesign(this RectTransform @this, Single left, Single top)
			{
				// 1) Переводим в актуальное экраное пространство
				Vector2 screen_pos = @this.GetWorldScreenInvFromLocalDesign(left, top);
				return (@this.GetWorldTransformPositionFromWorldScreenInv(Camera.current, screen_pos));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции левого-верхнего угла в условных координатах
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="camera">Камера</param>
			/// <param name="left">Позиция по X относительно родительской области</param>
			/// <param name="top">Позиция по Y относительно родительской области</param>
			/// <returns>Позиция левого-верхнего угла в условных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 GetWorldTransformPositionFromLocalDesign(this RectTransform @this, Camera camera, Single left, Single top)
			{
				// 1) Переводим в актуальное экраное пространство
				Vector2 screen_pos = @this.GetWorldScreenInvFromLocalDesign(left, top);
				return (@this.GetWorldTransformPositionFromWorldScreenInv(camera, screen_pos));
			}
			#endregion

			#region ======================================= Move ======================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции позиции
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_pos">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveLocalDesignToLinearIteration(this RectTransform @this, Single duration, Vector2 target_pos)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 current_pos = new Vector2(@this.GetLeft(), @this.GetTop());
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					Vector2 position = Vector2.Lerp(current_pos, target_pos, time);
					@this.SetPosition(position.x, position.y);
					yield return null;
				}

				@this.SetPosition(target_pos.x, target_pos.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции позиции
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_pos">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания изменения позиции</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveLocalDesignToLinearIteration(this RectTransform @this, Single duration, Vector2 target_pos, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 current_pos = new Vector2(@this.GetLeft(), @this.GetTop());
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					Vector2 position = Vector2.Lerp(current_pos, target_pos, time);
					@this.SetPosition(position.x, position.y);
					yield return null;
				}

				@this.SetPosition(target_pos.x, target_pos.y);
				if (on_completed != null)
					on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции позиции (зависит от привязок)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_pos">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveAnchoredPositionToLinearIteration(this RectTransform @this, Single duration, Vector2 target_pos)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 current_pos = @this.anchoredPosition;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					@this.anchoredPosition = Vector2.Lerp(current_pos, target_pos, time);
					yield return null;
				}

				@this.anchoredPosition = target_pos;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции позиции (зависит от привязок)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_pos">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания изменения позиции</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveAnchoredPositionToLinearIteration(this RectTransform @this, Single duration, Vector2 target_pos, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 current_pos = @this.anchoredPosition;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					@this.anchoredPosition = Vector2.Lerp(current_pos, target_pos, time);
					yield return null;
				}

				@this.anchoredPosition = target_pos;
				if (on_completed != null)
					on_completed();
			}
			#endregion

			#region ======================================= Size ======================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции размеров
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="duration">Время изменения размеров</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeLocalDesignToLinearIteration(this RectTransform @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 current_size = new Vector2(@this.rect.width, @this.rect.height);
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					Vector2 size = Vector2.Lerp(current_size, target_size, time);
					@this.SetSize(size.x, size.y);
					yield return null;
				}

				@this.SetSize(target_size.x, target_size.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции размеров
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="duration">Время изменения размеров</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeLocalDesignToLinearIteration(this RectTransform @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 current_size = new Vector2(@this.rect.width, @this.rect.height);
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					Vector2 size = Vector2.Lerp(current_size, target_size, time);
					@this.SetSize(size.x, size.y);
					yield return null;
				}

				@this.SetSize(target_size.x, target_size.y);
				if (on_completed != null)
					on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции размеров (зависит от привязок)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="duration">Время изменения размеров</param>
			/// <param name="target_size">Целевой размер</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeSizeDeltaToLinearIteration(this RectTransform @this, Single duration, Vector2 target_size)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 current_size = new Vector2(@this.rect.width, @this.rect.height);
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					Vector2 size = Vector2.Lerp(current_size, target_size, time);
					@this.SetImmediateWidth(size.x);
					@this.SetImmediateHeight(size.y);
					yield return null;
				}

				@this.SetImmediateWidth(target_size.x);
				@this.SetImmediateHeight(target_size.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма линейной интерполяции размеров (зависит от привязок)
			/// </summary>
			/// <param name="@this">Компонент трансформации</param>
			/// <param name="duration">Время изменения размеров</param>
			/// <param name="target_size">Целевой размер</param>
			/// <param name="on_completed">Обработчик события окончания изменения размера</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator ResizeSizeDeltaToLinearIteration(this RectTransform @this, Single duration, Vector2 target_size, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector2 current_size = new Vector2(@this.rect.width, @this.rect.height);
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					Vector2 size = Vector2.Lerp(current_size, target_size, time);
					@this.SetImmediateWidth(size.x);
					@this.SetImmediateHeight(size.y);
					yield return null;
				}

				@this.SetImmediateWidth(target_size.x);
				@this.SetImmediateHeight(target_size.y);
				if (on_completed != null)
					on_completed();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================