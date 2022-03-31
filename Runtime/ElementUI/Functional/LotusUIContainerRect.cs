//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIContainerRect.cs
*		Компонент для создания, хранение и управление списком дочерних элементов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DUICommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент для создания, хранение и управление списком дочерних элементов
		/// </summary>
		/// <remarks>
		/// <para>
		/// Компонент обеспечивает базовую функциональность для создания, хранения и управления списком дочерних элементов. 
		/// Так как список элементов представляет собой элементы интерфейса, то также поддерживается синхронизация между 
		/// позицией элемента в списке и его дочерней позицией применительно к игровому объекту.
		/// </para>
		/// <para>
		/// Компонент не размещает и не управляет размерами элементов
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[DisallowMultipleComponent]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathFunc + "Container Rect")]
		public class LotusUIContainerRect : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal CContainerRectTransform mContainer;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedContainer;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Контейнер дочерних элементов
			/// </summary>
			//public CContainerSelectableRectTransform Container
			//{
			//	get { return mContainer; }
			//}

			/// <summary>
			/// Выбранный индекс элемента, -1 выбора нет
			/// </summary>
			/// <remarks>
			/// При множественном выборе индекс последнего выбранного элемента
			/// </remarks>
			//public Int32 SelectedIndex
			//{
			//	get { return mContainer.SelectedIndex; }
			//	set
			//	{
			//		mContainer.SelectedIndex = value;
			//	}
			//}

			/// <summary>
			/// Предпоследний выбранный индекс элемента, -1 выбора нет
			/// </summary>
			//public Int32 PrevSelectedIndex
			//{
			//	get { return mContainer.PrevSelectedIndex; }
			//}

			/// <summary>
			/// Выбранный элемент
			/// </summary>
			//public RectTransform SelectedItem
			//{
			//	get { return mContainer.SelectedItem; }
			//}

			/// <summary>
			/// Предпоследний выбранный элемент
			/// </summary>
			//public RectTransform PrevSelectedItem
			//{
			//	get { return mContainer.PrevSelectedItem; }
			//}

			/// <summary>
			/// Режим включения отмены выделения элемента
			/// </summary>
			/// <remarks>
			/// При его включение будет вызваться метод элемента <see cref="ILotusSelectedItem.SetUnselectedItem"/>.
			/// Это может не понадобиться если, например, режим визуального реагирования как у кнопки
			/// </remarks>
			//public Boolean IsEnabledUnselectingItem
			//{
			//	get { return (mContainer.IsEnabledUnselectingItem); }
			//	set { mContainer.IsEnabledUnselectingItem = value; }
			//}

			/// <summary>
			/// Префаб для динамического создания элемента списка
			/// </summary>
			public RectTransform ItemPrefab
			{
				get { return mContainer.ItemPrefab; }
				set { mContainer.ItemPrefab = value; }
			}

			//
			// МНОЖЕСТВЕННЫЙ ВЫБОР
			//
			/// <summary>
			/// Возможность выбора нескольких элементов
			/// </summary>
			//public Boolean IsMultiSelected
			//{
			//	get { return mContainer.IsMultiSelected; }
			//	set { mContainer.IsMultiSelected = value; }
			//}

			/// <summary>
			/// Режим выбора нескольких элементов (первый раз выделение, второй раз снятие выделения)
			/// </summary>
			//public Boolean ModeSelectAddRemove
			//{
			//	get { return mContainer.ModeSelectAddRemove; }
			//	set { mContainer.ModeSelectAddRemove = value; }
			//}

			/// <summary>
			/// При множественном выборе всегда должен быть выбран хотя бы один элемент
			/// </summary>
			//public Boolean AlwaysSelectedItem
			//{
			//	get { return mContainer.AlwaysSelectedItem; }
			//	set { mContainer.AlwaysSelectedItem = value; }
			//}

			/// <summary>
			/// Список выбранных элементов
			/// </summary>
			//public List<RectTransform> SelectedItems
			//{
			//	get { return mContainer.SelectedItems; }
			//}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации об изменение текущего выбранного элемента
			/// </summary>
			//public Action OnCurrentItemСhanged
			//{
			//	get { return mContainer.OnCurrentItemChanged; }
			//	set { mContainer.OnCurrentItemChanged = value; }
			//}

			/// <summary>
			/// Событие для нотификации об изменение индекса выбранного элемента. Аргумент - индекс выбранного элемента
			///// </summary>
			//public Action<Int32> OnSelectedIndexChanged
			//{
			//	get { return mContainer.OnSelectedIndexChanged; }
			//	set { mContainer.OnSelectedIndexChanged = value; }
			//}

			/// <summary>
			/// Событие для нотификации о добавлении элемента к списку выделенных(после добавления). Аргумент - индекс (позиция) добавляемого элемента
			/// </summary>
			//public Action<Int32> OnSelectionAddItem
			//{
			//	get { return mContainer.OnSelectionAddItem; }
			//	set { mContainer.OnSelectionAddItem = value; }
			//}

			/// <summary>
			/// Событие для нотификации о удалении элемента из списка выделенных(после удаления). Аргумент - индекс (позиция) удаляемого элемента
			/// </summary>
			//public Action<Int32> OnSelectionRemovedItem
			//{
			//	get { return mContainer.OnSelectionRemovedItem; }
			//	set { mContainer.OnSelectionRemovedItem = value; }
			//}

			/// <summary>
			/// Событие для нотификации о выборе элемента
			/// </summary>
			/// <remarks>
			/// В основном применяется(должно применятся) для служебных целей
			/// </remarks>
			//public Action<RectTransform> OnSelectedItem
			//{
			//	get { return mContainer.OnSelectedItem; }
			//	set { mContainer.OnSelectedItem = value; }
			//}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация элемента при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------

			protected virtual void ResetElement()
			{
				//if (mContainer == null) mContainer = new CContainerSelectableRectTransform();
				//mContainer.Parent = this.transform;
			}
#endif


#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
				this.ResetElement();
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void ConstructorElement()
			{
				//if (mContainer == null) mContainer = new CContainerSelectableRectTransform();
				//mContainer.Parent = this.transform;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				this.ConstructorElement();
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ТЕКУЩИМ ЭЛЕМЕНТОМ =========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Дублирование текущего элемента и добавление элемента в контейнер
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void DublicateSelectedItem()
			{
				//mContainer.DublicateSelectedItem();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление текущего элемента из контейнера (удаляется объект)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void DeleteSelectedItem()
			{
				//mContainer.DeleteSelectedItem();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение текущего элемента назад
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void MoveSelectedBackward()
			{
				//mContainer.MoveSelectedBackward();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение текущего элемента вперед
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void MoveSelectedForward()
			{
				//mContainer.MoveSelectedForward();
			}
			#endregion

			#region ======================================= МЕТОДЫ С МНОЖЕСТВЕННЫМ ВЫБОРОМ ============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Служебный метод
			/// </summary>
			/// <returns>Список выделенных индексов</returns>
			//---------------------------------------------------------------------------------------------------------
			public String GetSelectedIndexes()
			{
				//return mContainer.GetSelectedIndexes();
				return ("");
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОЛЬЗОВАТЕЛЬСКОГО ВВОДА ============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Щелчок по области контейнера
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnPointerClick(PointerEventData event_data)
			{
				//mContainer.OnPointerClick(event_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Нажатие на область контейнера
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnPointerDown(BaseEventData event_data)
			{
				//mContainer.OnPointerDown(event_data as PointerEventData);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отпускание кнопки мыши/тача
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnPointerUp(PointerEventData event_data)
			{
				//mContainer.OnPointerUp(event_data);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================