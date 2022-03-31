//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Методы расширений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIExtensionImage.cs
*		Методы расширения компонента Image.
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
		/// Статический класс реализующий методы расширений компонента <see cref="Image"/> 
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionImage
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение альфы компонента цвета рендера спрайтов
			/// </summary>
			/// <param name="this">Рендер спрайтов</param>
			/// <returns>Альфа компонента цвета</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetAlphaColor(this Image @this)
			{
				return (@this.color.a);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка альфы компонента цвета рендера спрайтов
			/// </summary>
			/// <param name="this">Рендер спрайтов</param>
			/// <param name="alpha">Альфа компонента цвета</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetAlphaColor(this Image @this, Single alpha)
			{
				@this.color = new Color(@this.color.r, @this.color.g, @this.color.b, alpha);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка спрайта компонента изображения с учетом бордюра
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <param name="sprite">Спрайт</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetSprite(this Image @this, Sprite sprite)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif

				if (sprite != null)
				{
					@this.sprite = sprite;

					if (@this.type != Image.Type.Filled && @this.type != Image.Type.Tiled)
					{
						if (sprite.border != Vector4.zero)
						{
							@this.type = Image.Type.Sliced;
						}
						else
						{
							@this.type = Image.Type.Simple;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка статуса доступности компонента изображения
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <param name="is_enabled">Статус доступности</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetEnabledImage(this Image @this, Boolean is_enabled)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif
				// Проверяем на расширенный компонент LotusUIImage
				LotusUIImage image_ex = @this as LotusUIImage;
				if (image_ex != null)
				{
					image_ex.IsEnabled = is_enabled;
				}
				else
				{
					if(is_enabled)
					{
						@this.material = null;
					}
					else
					{
						@this.material = LotusGraphics2DVisualStyleService.MaterialDisableImage;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавный показ изображения
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <param name="duration">Время показа</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ShowImage(this Image @this, Single duration = 0.3f)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif
				// Тогда стандартный механизм
				@this.canvasRenderer.SetAlpha(0);
				@this.CrossFadeAlpha(1, duration, true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавный скрытие изображения
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <param name="duration">Время скрытия</param>
			//---------------------------------------------------------------------------------------------------------
			public static void HideImage(this Image @this, Single duration = 0.3f)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif
				// Тогда стандартный механизм
				@this.CrossFadeAlpha(0, duration, true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение размера бордюра левого края изображения
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <returns>Размера бордюра левого края изображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetBorderLeft(this Image @this)
			{
				Single border = 0;
				if (@this.sprite != null)
				{
					border = @this.sprite.border.x;
				}
				return border;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение размера бордюра верхнего края изображения
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <returns>Размера бордюра верхнего края фонового /returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetBorderTop(this Image @this)
			{
				Single border = 0;

				if (@this.sprite != null)
				{
					border = @this.sprite.border.w;
				}


				return border;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение размера бордюра правого края изображения
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <returns>Размера бордюра правого края изображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetBorderRight(this Image @this)
			{
				Single border = 0;

				if (@this.sprite != null)
				{
					border = @this.sprite.border.z;
				}

				return border;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение размера бордюра нижнего края изображения
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <returns>Размера бордюра нижнего края изображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetBorderBottom(this Image @this)
			{
				Single border = 0;

				if (@this.sprite != null)
				{
					border = @this.sprite.border.y;
				}

				return border;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение ширины изображения без учета бордюра
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <returns>Ширина изображения без учета бордюра</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetContentWidth(this Image @this)
			{
				Single width = @this.rectTransform.rect.width;

				if (@this.sprite != null)
				{
					width = width - (@this.sprite.border.x + @this.sprite.border.z);
				}

				return width;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение высоты изображения без учета бордюра
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <returns>Высота изображения без учета бордюра</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetContentHeight(this Image @this)
			{
				Single height = @this.rectTransform.rect.height;

				if (@this.sprite != null)
				{
					height = height - (@this.sprite.border.w + @this.sprite.border.y);
				}

				return height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма заполнения изображения
			/// </summary>
			/// <param name="this">Компонент изображения</param>
			/// <param name="duration">Время затенения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator FillAmountIteration(this Image @this, Single duration)
			{
				Single time = 0;
				Single start_time = 0;
				while (time < 1)
				{
					start_time += Time.deltaTime;
					time = start_time / duration;
					@this.fillAmount = time;
					yield return null;
				}

				@this.fillAmount = 1;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================