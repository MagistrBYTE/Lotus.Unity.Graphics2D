//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIScrollRectGroup.cs
*		Расширенный компонент ScrollRectGroup определяющий фиксированное перемещение контента в ограниченной области.
*		Реализация компонента определяющего на базе основного компонента окна просмотра дополнительную функциональность,
*	направленную на удобное размещение и фиксированное перемещение контента в ограниченной области.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Maths;
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
		/// Расширенный компонент ScrollRectGroup определяющий размещение и перемещение контента в ограниченной области
		/// </summary>
		/// <remarks>
		/// Реализация компонента определяющего на базе основного компонента окна просмотра дополнительную функциональность,
		/// направленную на удобное размещение и фиксированное перемещение контента в ограниченной области
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathFunc + "ScrollRectGroup")]
		public class LotusUIScrollRectGroup : LotusUIScrollRect
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента ScrollRectGroup
			/// </summary>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="rect_parent">Родительский компонент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public new static LotusUIScrollRectGroup Create(Single width, Single height, RectTransform rect_parent = null)
			{
				// 1) Создание объекта
				LotusUIScrollRectGroup scroll_rect_group = LotusElementUIDispatcher.CreateElement<LotusUIScrollRectGroup>("ScrollRectGroup");

				// 2) Данные
				scroll_rect_group.gameObject.AddComponent<RectMask2D>();
				scroll_rect_group.Width = width;
				scroll_rect_group.Height = height;

				// 3) Определение в иерархии
				if (rect_parent != null)
				{
					scroll_rect_group.transform.SetParent(rect_parent, false);
				}

				return scroll_rect_group;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal LayoutGroup mGrouping;
			[SerializeField]
			internal TScrollVisibility mAutoScrollVertical = TScrollVisibility.Disable;
			[SerializeField]
			internal TScrollVisibility mAutoScrollHorizontal = TScrollVisibility.Disable;
			[SerializeField]
			internal RectTransform mScrollVertical;
			[SerializeField]
			internal RectTransform mScrollHorizontal;
			[SerializeField]
			internal Single mScrollSize = 20;
			[NonSerialized]
			internal Vector4 mMargin;
			[NonSerialized]
			internal Func<RectTransform> OnGetScrollHorizontal;
			[NonSerialized]
			internal Func<RectTransform> OnGetScrollVertical;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Группировка элементов контейнера
			/// </summary>
			public LayoutGroup Grouping
			{
				get { return mGrouping; }
			}

			/// <summary>
			/// Режим отображения вертикальной полосы прокрутки
			/// </summary>
			public TScrollVisibility AutoScrollVertical
			{
				get { return mAutoScrollVertical; }
				set
				{
					if (mAutoScrollVertical != value)
					{
						mAutoScrollVertical = value;
						this.RaiseAutoScrollVerticalChanged();
						this.UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Режим отображения горизонтальной полосы прокрутки
			/// </summary>
			public TScrollVisibility AutoScrollHorizontal
			{
				get { return mAutoScrollHorizontal; }
				set
				{
					if (mAutoScrollHorizontal != value)
					{
						mAutoScrollHorizontal = value;
						this.RaiseAutoScrollHorizontalChanged();
						this.UpdatePlacement();
					}
				}
			}

			/// <summary>
			/// Размер полосы прокрутки
			/// </summary>
			public Single ScrollSize
			{
				get { return mScrollSize; }
				set
				{
					if (mScrollSize != value)
					{
						mScrollSize = value;
						this.UpdatePlacement();
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
				base.Reset();
				AddGrouping();
			}
#endif
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void Awake()
			{
				base.Awake();
				AddGrouping();
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Определение необходимости в полосах прокруток
			/// </summary>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator ComputeAutoScroll()
			{
				yield return null;
				this.ComputeAutoScrollVertical();
				this.ComputeAutoScrollHorizontal();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Определение необходимости в вертикальной полосе прокрутки
			/// </summary>
			/// <remarks>
			/// Применяется в том случае если высота контента больше высоты области просмотра
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeAutoScrollVertical()
			{
				// Если элементы занимают всю доступную область
				//if (mGrouping.GroupVerticalAlign == TLayoutGroupVerticalAlign.Stretch)
				//{
				//	return;
				//}

				//if (mAutoScrollVertical == TScrollVisibility.Auto)
				//{
				//	if (mGrouping.LayoutHeight > this.Height)
				//	{
				//		if (mScrollVertical == null)
				//		{
				//			this.AddScrollVertical();
				//		}
				//	}
				//	else
				//	{
				//		if (mScrollVertical != null)
				//		{
				//			StartCoroutine(DeleteScrollVertical());
				//		}
				//	}
				//}

				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Определение необходимости в горизонтальной полосе прокрутки
			/// </summary>
			/// <remarks>
			/// Применяется в том случае если ширина контента больше ширины области просмотра
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeAutoScrollHorizontal()
			{
				// Если контент растянут на всю область просмотра то выходим
				//if (mGrouping.GroupHorizontalAlign == TLayoutGroupHorizontalAlign.Stretch)
				//{
				//	return;
				//}

				//if (mAutoScrollHorizontal == TScrollVisibility.Auto)
				//{
				//	if (mGrouping.LayoutWidth > this.Width)
				//	{
				//		if (mScrollHorizontal == null)
				//		{
				//			this.AddScrollHorizontal();
				//		}
				//	}
				//	else
				//	{
				//		if (mScrollHorizontal != null)
				//		{
				//			StartCoroutine(DeleteScrollHorizontal());
				//		}
				//	}
				//}

				this.UpdatePlacement();
			}
			#endregion

			#region ======================================= CЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение видимости вертикальной полосы прокрутки.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void RaiseAutoScrollVerticalChanged()
			{
				switch (mAutoScrollVertical)
				{
					case TScrollVisibility.Allways:
						{
							if (mScrollVertical == null)
							{
								this.AddScrollVertical();
							}
						}
						break;
					case TScrollVisibility.Auto:
						{
							this.ComputeAutoScrollVertical();
						}
						break;
					case TScrollVisibility.Disable:
						{
							StartCoroutine(DeleteScrollVertical());
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение видимости горизонтальной полосы прокрутки.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void RaiseAutoScrollHorizontalChanged()
			{
				switch (mAutoScrollHorizontal)
				{
					case TScrollVisibility.Allways:
						{
							if (mScrollHorizontal == null)
							{
								this.AddScrollHorizontal();
							}
						}
						break;
					case TScrollVisibility.Auto:
						{
							this.ComputeAutoScrollHorizontal();
						}
						break;
					case TScrollVisibility.Disable:
						{
							StartCoroutine(DeleteScrollHorizontal());
						}
						break;
					default:
						break;
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление положение и размера элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public new void UpdatePlacement()
			{
//				// Размер области просмотра
//				Single view_left = mMargin.x;
//				Single view_top = mMargin.w;
//				Single view_width = this.Width - (mMargin.x + mMargin.z);
//				Single view_height = this.Height - (mMargin.w + mMargin.y);

//				// Если есть горизонтальная полоса прокрутки
//				if (mScrollHorizontal != null)
//				{
//					// Уменьшаем высоту
//					view_height -= mScrollSize;
//					Single scroll_top = this.Height - (mMargin.w + mScrollSize);
//					Single scroll_width = this.Width - (mMargin.x + mMargin.z);
//					if (mScrollVertical != null)
//					{
//						scroll_width -= mScrollSize;
//					}

//					mScrollHorizontal.Set(mMargin.x, scroll_top, scroll_width, mScrollSize);
//				}

//				// Если есть вертикальная полоса прокрутки
//				if (mScrollVertical != null)
//				{
//					// Уменьшаем высоту
//					view_width -= mScrollSize;
//					Single scroll_left = this.Width - (mMargin.x + mScrollSize);
//					Single scroll_height = this.Height - (mMargin.w + mMargin.y);
//					if (mScrollHorizontal != null)
//					{
//						scroll_height -= mScrollSize;
//					}

//					mScrollVertical.Set(scroll_left, mMargin.w, mScrollSize, scroll_height);
//				}

//				// Устанавливаем размеры области просмотра
//				this.Set(view_left, view_top, view_width, view_height);

//				// // Размеры контента не могут быть меньше области просмотра
//				if (mGrouping.Width < view_width)
//				{
//					this.ContentOffsetX = 0;
//					mGrouping.Width = view_width;
//				}
//				if (mGrouping.Height < view_height)
//				{
//					this.ContentOffsetY = 0;
//					mGrouping.Height = view_height;
//				}

//				// Частные случаи макета
//				switch (mGrouping.GroupType)
//				{
//					case TLayoutGroupType.Horizontal:
//						{
//							// Если элементы располагаются горизонтально
//							// и высота элемента растянута то значит там один элемент
//							// и тогда чтобы его не прокручивать по высоте - надо 
//							// установить высоту контента - высоте области просмотра
//							if (mGrouping.GroupVerticalAlign == TLayoutGroupVerticalAlign.Stretch)
//							{
//#if UNITY_EDITOR
//								if (mGrouping.Height > view_height)
//								{
//									Debug.LogWarning("Grouping.Height > view_height");
//								}
//#endif
//								mGrouping.Height = view_height;
//							}
//						}
//						break;
//					case TLayoutGroupType.Vertical:
//						{
//							// Аналогично
//							if (mGrouping.GroupHorizontalAlign == TLayoutGroupHorizontalAlign.Stretch)
//							{
//#if UNITY_EDITOR
//								if (mGrouping.Width > view_width)
//								{
//									Debug.LogWarning("Grouping.Height > view_height");
//								}
//#endif
//								mGrouping.Width = view_width;
//							}
//						}
//						break;
//					case TLayoutGroupType.Grid:
//						break;
//					case TLayoutGroupType.Flow:
//						break;
//					default:
//						break;
//				}

//				mGrouping.UpdateLayout();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Произошло событие добавления элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void EventAddElement()
			{
				this.ComputeAutoScrollVertical();
				this.ComputeAutoScrollHorizontal();
				//mGrouping.UpdateLayout();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Произошло событие обновления элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void EventUpdateElement()
			{
				this.ComputeAutoScrollVertical();
				this.ComputeAutoScrollHorizontal();
				//mGrouping.UpdateLayout();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Произошло событие удаления элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void EventRemoveElement()
			{
				StartCoroutine(ComputeAutoScroll());
				//mGrouping.UpdateLayout();
			}
			#endregion

			#region ======================================= МЕТОДЫ УПРАВЛЕНИЯ СОДЕРЖИМЫМ ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление макета для группирования элементов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void AddGrouping()
			{
				Boolean is_created_container = false;

				if (mGrouping == null)
				{
					//mGrouping = this.FindComponentInChildren<LotusUILayoutGroup>("Container", true);
				}

				if (mGrouping == null)
				{
					//GameObject go_container = new GameObject("Container", typeof(LotusUILayoutGroup));
					//go_container.layer = LayerMask.NameToLayer(XLayer.UI); ;
					//go_container.transform.SetParent(transform, false);
					//mGrouping = go_container.GetComponent<LotusUILayoutGroup>();
					//is_created_container = true;
				}

				if (is_created_container)
				{
					//mGrouping.SetAlignment(THorizontalAlignment.Left, TVerticalAlignment.Top);
					//mGrouping.Set(0, 0, Width, Height);
				}

				//content = mGrouping.UIRect;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление вертикальной полосы прокрутки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void AddScrollVertical()
			{
				if (mScrollVertical == null)
				{
					mScrollVertical = this.FindComponentInChildren<RectTransform>("ScrollHorizontal", true);
				}

				if (mScrollVertical == null)
				{
					Single left = this.Width - (mMargin.x + mScrollSize);
					Single top = mMargin.w;
					Single width = mScrollSize;
					Single height = this.Height - (mMargin.w + mMargin.y + mScrollSize);

					mScrollVertical = OnGetScrollVertical();
					mScrollVertical.name = "ScrollVertical";
					mScrollVertical.SetParent(this.transform as RectTransform);
					mScrollVertical.Set(left, top, width, height);
				}

				mScrollVertical.SetAlignment(THorizontalAlignment.Right, TVerticalAlignment.Stretch);
				verticalScrollbar = mScrollVertical.GetComponentInChildren<Scrollbar>();

				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление горизонтальной полосы прокрутки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void AddScrollHorizontal()
			{
				if (mScrollHorizontal == null)
				{
					mScrollHorizontal = this.FindComponentInChildren<RectTransform>("ScrollHorizontal", true);
				}

				if (mScrollHorizontal == null)
				{
					Single left = mMargin.x;
					Single top = this.Height - (mMargin.w + mScrollSize);
					Single width = this.Width - (mMargin.x + mMargin.z + mScrollSize);
					Single height = mScrollSize;

					mScrollHorizontal = OnGetScrollHorizontal();
					mScrollHorizontal.name = "ScrollHorizontal";
					mScrollHorizontal.SetParent(this.transform as RectTransform);
					mScrollHorizontal.Set(left, top, width, height);
				}

				mScrollHorizontal.SetAlignment(THorizontalAlignment.Stretch, TVerticalAlignment.Bottom);
				this.horizontalScrollbar = mScrollHorizontal.GetComponentInChildren<Scrollbar>();

				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление вертикальной полосы прокрутки
			/// </summary>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected virtual IEnumerator DeleteScrollVertical()
			{
				if (mScrollVertical == null)
				{
					mScrollVertical = this.FindComponentInChildren<RectTransform>("ScrollVertical", true);
				}

				if (mScrollVertical != null)
				{
					XGameObjectDispatcher.Destroy(mScrollVertical.gameObject);
				}

				yield return null;

				mScrollVertical = null;

				this.UpdatePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление горизонтальной полосы прокрутки
			/// </summary>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected virtual IEnumerator DeleteScrollHorizontal()
			{
				if (mScrollHorizontal == null)
				{
					mScrollHorizontal = this.FindComponentInChildren<RectTransform>("ScrollHorizontal", true);
				}

				if (mScrollHorizontal != null)
				{
					XGameObjectDispatcher.Destroy(mScrollHorizontal.gameObject);
				}

				yield return null;

				mScrollHorizontal = null;
				this.UpdatePlacement();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================