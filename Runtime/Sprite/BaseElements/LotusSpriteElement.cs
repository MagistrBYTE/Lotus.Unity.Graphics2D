//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteElement.cs
*		Компонент представляющий основной элемент интерфейса модуля спрайтов.
*		Реализация компонента основного элемента модуля спрайтов для построения полноценных адаптивных элементов интерфейса.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DSpriteBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент представляющий основной элемент интерфейса модуля спрайтов
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[ExecuteInEditMode]
		[AddComponentMenu(XSpriteEditorSettings.MenuPathBase + "Element")]
		public class LotusSpriteElement : LotusSpriteBaseElement, ILotusElement, ILotusScreenGameVisual,
			IComparable<ILotusElement>, IComparable<LotusSpriteElement>
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента Sprite Element
			/// </summary>
			/// <param name="left">Позиция по X</param>
			/// <param name="top">Позиция по Y</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public new static LotusSpriteElement Create(Single left, Single top, Single width, Single height, Transform parent = null)
			{
				// 1) Создание объекта
				LotusSpriteElement element = LotusSpriteDispatcher.CreateElement<LotusSpriteElement>("Element", left, top, width, height);

				// 2) Определение в иерархии
				element.SetParent(parent);

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsEnabled = true;
			[NonSerialized]
			internal Boolean mIsEnabledElement;
			#endregion

			#region ======================================= СВОЙСТВА ILotusEnabling ===================================
			/// <summary>
			/// Доступность элемента
			/// </summary>
			public Boolean IsEnabled
			{
				get { return mIsEnabled; }
				set
				{
					if (mIsEnabled != value)
					{
						mIsEnabled = value;
					}
				}
			}

			/// <summary>
			/// Статус доступности элемента
			/// </summary>
			/// <remarks>
			/// Зависит как от статус самого элемента и так от статуса родительского элемента
			/// </remarks>
			public Boolean IsEnabledElement
			{
				get
				{
					return mIsEnabledElement;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusElement ====================================
			/// <summary>
			/// Родительский элемент
			/// </summary>
			public ILotusElement IParent
			{
				get
				{
					return (mParent as ILotusElement);
				}
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов по порядку рисования
			/// </summary>
			/// <param name="other">Элемент</param>
			/// <returns>Статус сравнения элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(ILotusElement other)
			{
				if (this.IsVisibleElement == false)
				{
					if (other.IsVisibleElement == false)
					{
						return (this.Depth.CompareTo(other.Depth));
					}
					else
					{
						return (-1);
					}
				}
				else
				{
					if (other.IsVisibleElement == false)
					{
						return 1;
					}
					else
					{
						return (this.Depth.CompareTo(other.Depth));
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов по порядку рисования
			/// </summary>
			/// <param name="other">Элемент</param>
			/// <returns>Статус сравнения элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(LotusSpriteElement other)
			{
				if (this.IsVisibleElement == false)
				{
					if (other.IsVisibleElement == false)
					{
						return (this.Depth.CompareTo(other.Depth));
					}
					else
					{
						return (-1);
					}
				}
				else
				{
					if (other.IsVisibleElement == false)
					{
						return 1;
					}
					else
					{
						return (this.Depth.CompareTo(other.Depth));
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusEnabling =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка видимости элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetEnabledElement()
			{
				if (mParent == null)
				{
					mIsEnabledElement = mIsEnabled;
				}
				else
				{
					ILotusEnabling element = mParent as ILotusEnabling;
					if (element != null)
					{
						mIsEnabledElement = mIsEnabled && element.IsEnabled;
					}
					else
					{
						mIsEnabledElement = mIsEnabled;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение статуса доступности элемента
			/// </summary>
			/// <returns>Статус доступности элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			internal Boolean GetEnabledElement()
			{
				if (mParent == null)
				{
					return mIsEnabled;
				}
				else
				{
					ILotusEnabling element = mParent as ILotusEnabling;
					if (element != null)
					{
						return mIsEnabled && element.IsEnabled;
					}
					else
					{
						return mIsEnabled;
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusElement ======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента в качестве дочернего
			/// </summary>
			/// <remarks>
			/// Метод не следует вызывать напрямую
			/// </remarks>
			/// <param name="child">Дочерний элемент></param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetChildren(ILotusElement child)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена установка элемента в качестве дочернего
			/// </summary>
			/// <remarks>
			/// Метод не следует вызывать напрямую
			/// </remarks>
			/// <param name="child">Дочерний элемент></param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UnsetChildren(ILotusElement child)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка родительского элемента
			/// </summary>
			/// <remarks>
			/// При абсолютной позиции элемент не меняет своего местоположения относительно экрана
			/// </remarks>
			/// <param name="parent">Родительский элемент</param>
			/// <param name="absolute_pos">Абсолютная позиция элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetParent(ILotusElement parent, Boolean absolute_pos)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение области для размещения дочерних элементов
			/// </summary>
			/// <returns>Прямоугольник области для размещения дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Rect GetChildRectContent()
			{
				return (RectScreen);
			}
			#endregion

			#region ======================================= МЕТОДЫ IScreenGameVisual ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка игрового экрана в указанную позицию
			/// </summary>
			/// <param name="position">Позиция в локальных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGamePosition(Vector2 position)
			{
				Location = position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка видимости игрового экрана 
			/// </summary>
			/// <param name="visible">Видимость игрового экрана</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGameVisible(Boolean visible)
			{
				IsVisible = visible;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка прозрачности игрового экрана 
			/// </summary>
			/// <param name="opacity">Прозрачность игрового экрана</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGameOpacity(Single opacity)
			{
				mSpriteRenderer.SetAlphaColor(opacity);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка смещения глубины показа(порядка рисования) игрового экрана
			/// </summary>
			/// <param name="depth_offset">Смещение по глубине</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetScreenGameDepthOffset(Int32 depth_offset)
			{
				if (ParentElement != null)
				{
					Single depth = -mThisTransform.GetSiblingIndex() - depth_offset;
					if (depth > -1f)
					{
						depth = 1f;
					}

					mThisTransform.position = new Vector3(mThisTransform.localPosition.x,
						mThisTransform.localPosition.y, depth);
				}
				else
				{
					Single depth = Mathf.Clamp(mThisTransform.position.z - 1,
						LotusSpriteDispatcher.MinPositionDepth, LotusSpriteDispatcher.MaxPositionDepth);

					mThisTransform.position = new Vector3(mThisTransform.position.x,
						mThisTransform.position.y, depth);
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