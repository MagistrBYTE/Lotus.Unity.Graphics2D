//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Базовые элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteBaseElement.cs
*		Компонент представляющий базовый элемент интерфейса модуля спрайтов.
*		Реализация компонента базового элемента обеспечивающего начальную инфраструктуру для построения элементов интерфейса.
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
		//! \defgroup Unity2DSpriteBase Базовые элементы интерфейса
		//! Базовые элементы интерфейса предназначены для отображения информации без взаимодействия с пользователем. 
		//! Реализованы базовые элементы, элементы с заголовочной областью и элементы индикации
		//! \ingroup Unity2DSprite
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент представляющий базовый элемент интерфейса модуля спрайтов
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[ExecuteInEditMode]
		[AddComponentMenu(XSpriteEditorSettings.MenuPathBase + "Base Element")]
		public class LotusSpriteBaseElement : LotusSpritePlaceable2D, ILotusBaseElement, ILotusPresentationBackcolor,
			IComparable<ILotusBaseElement>, IComparable<LotusSpriteBaseElement>
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента Sprite Base Element
			/// </summary>
			/// <param name="left">Позиция по X</param>
			/// <param name="top">Позиция по Y</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusSpriteBaseElement Create(Single left, Single top, Single width, Single height, Transform parent = null)
			{
				// 1) Создание объекта
				LotusSpriteBaseElement element = LotusSpriteDispatcher.CreateElement<LotusSpriteBaseElement>("Element", left, top, width, height);

				// 2) Определение в иерархии
				element.SetParent(parent);

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mUserTag = -1;
			[NonSerialized]
			private Boolean mIsDirty = false;

			// Параметры видимости
			[SerializeField]
			internal Int32 mVisibility;
			[SerializeField]
			internal Boolean mIsVisibleElement;

			// Параметры визуального стиля
			[SerializeField]
			internal Int32 mStyleMainIndex;
			[SerializeField]
			internal String mStyleMainName;
			[SerializeField]
			internal Boolean mUseBackground;

			// Конфигурация элемента
			[SerializeField]
			internal Vector4 mPadding;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedParam;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ПАРАМЕТРЫ РАЗМЕЩЕНИЯ
			//
			/// <summary>
			/// Внутренние отступы структурных частей элемента от краев элемента
			/// </summary>
			/// <remarks>
			/// Left - x, Top - y, Right - z, Bottom - w 
			/// </remarks>
			public Vector4 Padding
			{
				get { return mPadding; }
				set
				{
					if (mPadding != value)
					{
						mPadding = value;
						//this.RaisePlacementChanged();
					}
				}
			}

			/// <summary>
			/// Внутренний отступ слева при размещениии структурной части(контента) элемента
			/// </summary>
			public Single PaddingLeft
			{
				get { return mPadding.x; }
				set
				{
					mPadding.x = value;
					//this.RaisePlacementChanged();
				}
			}

			/// <summary>
			/// Внутренний отступ сверху при размещениии структурной части(контента) элемента
			/// </summary>
			public Single PaddingTop
			{
				get { return mPadding.y; }
				set
				{
					mPadding.y = value;
					//this.RaisePlacementChanged();
				}
			}

			/// <summary>
			/// Внутренний отступ справа при размещениии структурной части(контента) элемента
			/// </summary>
			public Single PaddingRight
			{
				get { return mPadding.z; }
				set
				{
					mPadding.z = value;
					//this.RaisePlacementChanged();
				}
			}

			/// <summary>
			/// Внутренний отступ снизу при размещениии структурной части(контента) элемента
			/// </summary>
			public Single PaddingBottom
			{
				get { return mPadding.w; }
				set
				{
					mPadding.w = value;
					//this.RaisePlacementChanged();
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusBaseElement ================================
			/// <summary>
			/// Имя элемента
			/// </summary>
			/// <remarks>
			/// Должно быть уникально в пределах проекта
			/// </remarks>
			public String Name
			{
				get { return (name); }
				set { name = value; }
			}

			/// <summary>
			/// Тэг для определения пользовательских данных
			/// </summary>
			public Int32 UserTag
			{
				get { return mUserTag; }
				set
				{
					mUserTag = value;
				}
			}

			/// <summary>
			/// Статус обновления элемента
			/// </summary>
			public Boolean IsDirty
			{
				get { return mIsDirty; }
				set
				{
					mIsDirty = value;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusVisualStyle ================================
			/// <summary>
			/// Индекс используемого базового визуального стиля из хранилища
			/// </summary>
			public Int32 StyleMainIndex
			{
				get { return (mStyleMainIndex); }
				set
				{
					mStyleMainIndex = value;
				}
			}

			/// <summary>
			/// Имя используемого базового визуального стиля
			/// </summary>
			public String StyleMainName
			{
				get { return mStyleMainName; }
				set
				{
					mStyleMainName = value;
					//mStyleMain = LotusGraphics2DVisualStyleService.(mStyleMainName);
				}
			}

			/// <summary>
			/// Использование общего фонового изображения
			/// </summary>
			public Boolean UseBackground
			{
				get { return (mUseBackground); }
				set
				{
					mUseBackground = value;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusVisibility =================================
			/// <summary>
			/// Видимость элемента
			/// </summary>
			/// <remarks>
			/// Определение видимости элемента пользователем. Используеся первый бит
			/// </remarks>
			public Boolean IsVisible
			{
				get
				{
					Boolean status = (!XPacked.UnpackBoolean(mVisibility, 0));
					return status;
				}
				set
				{
					if (value)
					{
						XPacked.PackBoolean(ref mVisibility, 0, false);
					}
					else
					{
						XPacked.PackBoolean(ref mVisibility, 0, true);
					}

					SetVisibleElement();
				}
			}

			/// <summary>
			/// Битовое поле видимости элемента
			/// </summary>
			/// <remarks>
			/// Если установлены флаги значит элемент не видим, ноль элемент виден.
			/// Применяется для работы с фильтрами.
			/// Можно задавать от четвертого бита
			/// </remarks>
			public Int32 Visibility
			{
				get { return mVisibility; }
				set
				{
					if (mVisibility != value)
					{
						mVisibility = value;
						SetVisibleElement();
					}
				}
			}

			/// <summary>
			/// Видимость непосредственно элемента
			/// </summary>
			/// <remarks>
			/// Определение видимости элемента.
			/// Зависит от фильтров и указания видимости пользователем
			/// </remarks>
			public Boolean IsVisibleSelf
			{
				get { return mVisibility == 0; }
			}

			/// <summary>
			/// Видимость элемента
			/// </summary>
			/// <remarks>
			/// Зависит как от статус самого элемента и так от статуса родительского элемента
			/// </remarks>
			public Boolean IsVisibleElement
			{
				get { return mIsVisibleElement; }
				set { }
			}

			/// <summary>
			/// Прозрачность элемента
			/// </summary>
			public Single Opacity
			{
				get 
				{ 
					return (mSpriteRenderer.color.a); 
				}
				set 
				{ 
					mSpriteRenderer.SetAlphaColor(value);
				}
			}

			/// <summary>
			/// Фоновый цвет для рисования элемента
			/// </summary>
			public Color BackgroundColor
			{
				get
				{
					return (mSpriteRenderer.color);
				}
				set
				{
					mSpriteRenderer.color = value;
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
			public Int32 CompareTo(ILotusBaseElement other)
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
			public Int32 CompareTo(LotusSpriteBaseElement other)
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

			#region ======================================= МЕТОДЫ ILotusVisualStyle ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление визуального стиля элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdateVisualStyle()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование визуального стиля элемента с другого элемента
			/// </summary>
			/// <param name="source">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void CopyVisualStyle(ILotusVisualStyle source)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров отображения элемента по связанному стилю
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetFromOriginalStyle()
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusVisibility ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка флага видимости. Если флаг установлен то элемент невидим
			/// </summary>
			/// <remarks>
			/// Номера флагов
			/// 0 - Обозначение что отображения зависит от установки пользователя
			/// 1 - Обозначение что отображения зависит от родителя
			/// 2 - Обозначение что отображения зависит от структруной части родителя
			/// 3 - Обозначение что отображения зависит от фильтра
			/// </remarks>
			/// <param name="number">Номер флага</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetVisibilityFlags(Int32 number)
			{
				XPacked.PackBoolean(ref mVisibility, number, true);
				SetVisibleElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка флага видимости
			/// </summary>
			/// <param name="number">Номер флага</param>
			//---------------------------------------------------------------------------------------------------------
			public void ClearVisibilityFlags(Int32 number)
			{
				XPacked.PackBoolean(ref mVisibility, number, false);
				SetVisibleElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка видимости элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetVisibleElement()
			{
				mIsVisibleElement = IsVisibleSelf;

				if (mVisibility == 0 && mIsVisibleElement)
				{
					mSpriteRenderer.enabled = true;
				}
				else
				{
					mSpriteRenderer.enabled = false;

					if (this.gameObject.activeSelf)
					{
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавное скрытие элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Hide()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавный показ элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Show()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавное скрытие элемента
			/// </summary>
			/// <param name="duration">Длительность эффекта</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Hide(Single duration)
			{
				StartCoroutine(HideLerpCoroutine(duration));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавный показ элемента
			/// </summary>
			/// <param name="duration">Длительность эффекта</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Show(Single duration)
			{
				StartCoroutine(ShowLerpCoroutine(duration));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение цвета спрайта
			/// </summary>
			/// <param name="duration">Длительность эффекта</param>
			/// <param name="target_color">Целевое значение</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void CrossFadeColor(Single duration, Color target_color)
			{
				StartCoroutine(ColorLerpCoroutine(duration, target_color));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение альфы компоненты цвета спрайта
			/// </summary>
			/// <param name="duration">Длительность эффекта</param>
			/// <param name="target_alpha">Целевое значение</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void CrossFadeAlpha(Single duration, Single target_alpha)
			{
				StartCoroutine(AlphaLerpCoroutine(duration, target_alpha));
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBaseElement ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка/обновление параметров
			/// </summary>
			/// <remarks>
			/// Вызывается центральным диспетчером в момент добавления(регистрации) элемента
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnReset()
			{
				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnUpdate()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnDraw()
			{
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================