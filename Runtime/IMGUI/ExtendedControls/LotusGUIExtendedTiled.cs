//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIExtendedTiled.cs
*		Плиточные элементы интерфейса пользователя с организацией по сетки.
*		Реализация плиточных элементов интерфейса пользователя которые располагаются по сетки и поддерживают расширенную
*	анимацию параметров.
*		В основном они планируется использоваться в качестве базовых элементов для организации игровых систем и механик
*	в двухмерных играх.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DImmedateGUIExtended
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый плиточный элемент (тайл)
		/// </summary>
		/// <remarks>
		/// Базовый плиточный элемент (тайл) поддерживает только расположение по сетки
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIBaseTileItem : CGUIBaseElement, ILotusDraggable, IComparable<CGUIBaseTileItem>
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mLayer;
			[SerializeField]
			internal Int32 mColumnIndex;
			[SerializeField]
			internal Int32 mRowIndex;

			// Параметры системы анимации
			[NonSerialized]
			internal Single mStartTime;
			[NonSerialized]
			internal Vector2 mLocationStarting;
			[NonSerialized]
			internal Vector2 mLocationTarget;

			// Параметры перетаскивания
			[NonSerialized]
			internal Boolean mIsDragging;
			[NonSerialized]
			internal Boolean mIsDownDragging;
			[NonSerialized]
			internal Vector2 mDragStartPosition;

			// Служебные данные
			[NonSerialized]
			internal CGUIGridTile mOwner;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Слой расположения
			/// </summary>
			public Int32 Layer
			{
				get { return mLayer; }
				set { mLayer = value; }
			}

			/// <summary>
			/// Индекс столбца
			/// </summary>
			public Int32 ColumnIndex
			{
				get
				{
					return mColumnIndex;
				}
				set
				{
					mColumnIndex = value;
					if (mOwner != null)
					{
						this.mRectWorldScreenMain.x = mOwner.GetPositionColumn(mColumnIndex);
					}
				}
			}

			/// <summary>
			/// Индекс строки
			/// </summary>
			public Int32 RowIndex
			{
				get
				{
					return mRowIndex;
				}
				set
				{
					mRowIndex = value;
					if (mOwner != null)
					{
						this.mRectWorldScreenMain.y = mOwner.GetPositionRow(mRowIndex);
					}
				}
			}

			/// <summary>
			/// Менеджер тайлов
			/// </summary>
			public CGUIGridTile TileManager
			{
				get { return mOwner; }
				set { mOwner = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusDraggable ==================================
			/// <summary>
			/// Статус перетаскивания элемента
			/// </summary>
			public Boolean IsDragging
			{
				get { return mIsDragging; }
			}

			/// <summary>
			/// Статус нажатия на перетаскиваемый элемент
			/// </summary>
			public Boolean IsDownDragging
			{
				get { return mIsDownDragging; }
			}

			/// <summary>
			/// Стартовая точки от которой началось перетаскивание
			/// </summary>
			public Vector2 DragStartPositionScreen
			{
				get { return mDragStartPosition; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseTileItem()
				: base()
			{
				mStyleMainName = "Label";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseTileItem(String name)
				: base(name)
			{
				mStyleMainName = "Label";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseTileItem(String name, Single x, Single y)
				: base(name)
			{
				mStyleMainName = "Label";
				mRectWorldScreenMain.x = x;
				mRectWorldScreenMain.y = y;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнения элементов списка по приоритету
			/// </summary>
			/// <param name="other">Элемент списка</param>
			/// <returns>Статус сравнения элементов списка</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CGUIBaseTileItem other)
			{
				if (this.Depth > other.Depth)
				{
					return 1;
				}
				else
				{
					if (this.Depth < other.Depth)
					{
						return -1;
					}
					else
					{
						return 0;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return mName;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusDraggable ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перетаскивание тайла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void DraggingTile()
			{
				switch (Event.current.type)
				{
					case EventType.MouseDown:
						{
							// Если есть попадание
							if (mRectWorldScreenMain.Contains(Event.current.mousePosition))
							{
								mIsDownDragging = true;
								mDragStartPosition = Event.current.mousePosition;
							}
						}
						break;
					case EventType.MouseUp:
						{
							mIsDownDragging = false;

							// Если есть нажатие в области перемещения
							if (mIsDragging)
							{
								mIsDragging = false;
								OnDragTileEnd();
							}
						}
						break;
					case EventType.MouseDrag:
						{
							// Если есть нажатие в области перемещения
							if (mIsDownDragging && mIsDragging == false)
							{
								if (Mathf.Abs(mDragStartPosition.y - Event.current.mousePosition.y) > LotusGUIDispatcher.DraggMinOffset.y ||
									Mathf.Abs(mDragStartPosition.x - Event.current.mousePosition.x) > LotusGUIDispatcher.DraggMinOffset.x)
								{
									mIsDragging = true;
									OnDragTileStart();
								}
							}

							// Если перемещение
							if (mIsDragging)
							{
								mRectWorldScreenMain.x += Event.current.delta.x;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
								mRectWorldScreenMain.y += Event.current.delta.y;
#else
								mRectWorldScreenMain.y -= (Event.current.delta.y);
#endif
							}
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало перетаскивание тайла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnDragTileStart()
			{
				mDepth += 1000;
				mOwner.mTiles.Sort();
				mRectWorldScreenMain.width += 10;
				mRectWorldScreenMain.height += 10;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание перетаскивание тайла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnDragTileEnd()
			{
				// 1)Считаем позицию куда-мы попали
				Int32 column = mOwner.GetColumnFromPosition(Event.current.mousePosition.x);
				Int32 row = mOwner.GetRowFromPosition(Event.current.mousePosition.y);

				column = Mathf.Clamp(column, 0, mOwner.ColumnCount - 1);
				row = Mathf.Clamp(row, 0, mOwner.RowCount - 1);

				// 2)Есть ли в этой позиции тайл
				CGUIBaseTileItem tile = mOwner.GetTileFromColumnAndRow(column, row);
				if(tile != null && tile != this)
				{
					// 3) Меняемся
					//tile.SetColumnAndRow(mColumnIndex, mRowIndex);
					tile.StartMove(mColumnIndex, mRowIndex);
				}

				// 4) Обновляем позицию
				SetColumnAndRow(column, row);

				CGUITileContentItem item = this as CGUITileContentItem;
				if (item != null)
				{
					item.Text = "C=" + mColumnIndex.ToString() + "; R=" + mRowIndex.ToString();
				}

				mDepth -= 1000;
				mOwner.mTiles.Sort();
				mRectWorldScreenMain.width -= 10;
				mRectWorldScreenMain.height -= 10;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
				// Считаем время
				Single delta_time = (Time.time - mStartTime) / 0.4f;
				if (delta_time > 1)
				{
					delta_time = 1;
					IsDirty = false;

					mRectWorldScreenMain.x = mLocationTarget.x;
					mRectWorldScreenMain.y = mLocationTarget.y;
					mColumnIndex = mOwner.GetColumnFromPosition(mRectWorldScreenMain.x);
					mRowIndex = mOwner.GetRowFromPosition(mRectWorldScreenMain.y);

					CGUITileContentItem item = this as CGUITileContentItem;
					if (item != null)
					{
						item.Text = "C=" + mColumnIndex.ToString() + "; R=" + mRowIndex.ToString();
					}

					return;
				}

				mRectWorldScreenMain.x = XEasing.CubeIn(mLocationStarting.x, mLocationTarget.x, delta_time);
				mRectWorldScreenMain.y = XEasing.CubeOut(mLocationStarting.y, mLocationTarget.y, delta_time);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.Box(mRectWorldScreenMain, "", mStyleMain);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUIBaseTileItem;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);

				CGUIBaseTileItem base_tile = base_element as CGUIBaseTileItem;
				if (base_tile != null)
				{
					mLayer = base_tile.mLayer;
					mColumnIndex = base_tile.mColumnIndex;
					mRowIndex = base_tile.mRowIndex;

					mLocationStarting = base_tile.mLocationStarting;
					mLocationTarget = base_tile.mLocationTarget;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАЗМЕЩЕНИЯ ТАЙЛА ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса столбца (позиции по X в менеджере тайлов)
			/// </summary>
			/// <returns>Индекс столбца (позиции по X в менеджере тайлов)</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetColumnIndex()
			{
				if (mOwner != null)
				{
					return mOwner.GetColumnFromPosition(mRectWorldScreenMain.x);
				}

				return 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса строки (позиции по Y в менеджере тайлов)
			/// </summary>
			/// <returns>Индекс строки (позиции по Y в менеджере тайлов)</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetRowIndex()
			{
				if (mOwner != null)
				{
					return mOwner.GetRowFromPosition(mRectWorldScreenMain.y);
				}

				return 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка столбца и строки тайла
			/// </summary>
			/// <param name="column">Индекс столбца (позиции по X в менеджере тайлов)</param>
			/// <param name="row">Индекс строки (позиции по Y в менеджере тайлов)</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetColumnAndRow(Int32 column, Int32 row)
			{
				if (mOwner != null)
				{
					mColumnIndex = column;
					mRowIndex = row;
					this.mRectWorldScreenMain.x = mOwner.GetPositionColumn(mColumnIndex);
					this.mRectWorldScreenMain.y = mOwner.GetPositionRow(mRowIndex);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление значений столбца и строки по позиции тайла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateFromPosition()
			{
				mColumnIndex = GetColumnIndex();
				mRowIndex = GetRowIndex();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление позиции тайла по значению столбца и строки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateFromColumnAndRow()
			{
				if (mOwner != null)
				{
					this.mRectWorldScreenMain.x = mOwner.GetPositionColumn(mColumnIndex);
					this.mRectWorldScreenMain.y = mOwner.GetPositionRow(mRowIndex);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ АНИМАЦИИ ТАЙЛОВ ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт перемещения тайла к указанной позиции
			/// </summary>
			/// <param name="column">Индекс столбца (позиции по X в менеджере тайлов)</param>
			/// <param name="row">Индекс строки (позиции по Y в менеджере тайлов)</param>
			//---------------------------------------------------------------------------------------------------------
			public void StartMove(Int32 column, Int32 row)
			{
				IsDirty = true;
				mStartTime = Time.time;
				mLocationStarting.x = mRectWorldScreenMain.x;
				mLocationStarting.y = mRectWorldScreenMain.y;
				mLocationTarget.x = mOwner.GetPositionColumn(column);
				mLocationTarget.y = mOwner.GetPositionRow(row);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Плиточный элемент (тайл) с содержимым
		/// </summary>
		/// <remarks>
		/// Плиточный элемент (тайл) содержащий базовый контент: текст и иконку изображения
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUITileContentItem : CGUIBaseTileItem
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mText;
			[SerializeField]
			internal Texture2D mIcon;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Основной текст элемента
			/// </summary>
			public String Text
			{
				get { return mText; }
				set { mText = value; }
			}

			/// <summary>
			/// Текстура иконки элемента
			/// </summary>
			public Texture2D Icon
			{
				get { return mIcon; }
				set { mIcon = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUITileContentItem()
				: base()
			{
				mText = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITileContentItem(String name)
				: base(name)
			{
				mText = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITileContentItem(String name, Single x, Single y)
				: base(name, x, y)
			{
				mText = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITileContentItem(String name, Single x, Single y, String text)
				: base(name, x, y)
			{
				mText = text;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			/// <param name="text">Текст элемента</param>
			/// <param name="style_name">Имя стиля элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITileContentItem(String name, Single x, Single y, String text, String style_name)
				: base(name, x, y)
			{
				mStyleMainName = style_name;
				mText = text;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusDataExchange =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка данных
			/// </summary>
			/// <param name="text">Текст</param>
			/// <param name="icon">Иконка изображения</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetData(String text, Texture2D icon)
			{
				mText = text;
				mIcon = icon;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusVisualStyle ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров отображения элемента по связанному стилю
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void SetFromOriginalStyle()
			{
				mStyleMain = LotusGUIDispatcher.FindStyle(mStyleMainName);
				mStyleMainName = mStyleMain.name;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.depth = mDepth;
				DraggingTile();

				LotusGUIDispatcher.CurrentContent.text = mText;
				LotusGUIDispatcher.CurrentContent.image = mIcon;

				GUI.Box(mRectWorldScreenMain, LotusGUIDispatcher.CurrentContent, mStyleMain);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUITileContentItem;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);

				CGUITileContentItem content_tile = base_element as CGUITileContentItem;
				if (content_tile != null)
				{
					mText = content_tile.mText;
					mIcon = content_tile.mIcon;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С КОНТЕНТОМ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимального размера элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeSizeFromContent()
			{
				ComputeSizeFromContent(mStyleMain);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной ширины элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeWidthFromContent()
			{
				ComputeWidthFromContent(mStyleMain);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной высоты элемента по содержимому
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeHeightFromContent()
			{
				ComputeHeightFromContent(mStyleMain);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимального размера элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeSizeFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = Text;
				LotusGUIDispatcher.CurrentContent.image = mIcon;

				Vector2 size = style.CalcSize(LotusGUIDispatcher.CurrentContent);
				mRectWorldScreenMain.width = RoundToNearest(size.x, 10);
				mRectWorldScreenMain.height = RoundToNearest(size.y, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной ширины элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeWidthFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mText;
				LotusGUIDispatcher.CurrentContent.image = mIcon;

				Single min_width = 0, max_width = 0;
				style.CalcMinMaxWidth(LotusGUIDispatcher.CurrentContent, out min_width, out max_width);
				mRectWorldScreenMain.width = RoundToNearest((min_width + max_width) / 2, 10);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление оптимальной высоты элемента по содержимому на основании стиля
			/// </summary>
			/// <param name="style">Стиль для отображения элемента</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ComputeHeightFromContent(GUIStyle style)
			{
				LotusGUIDispatcher.CurrentContent.text = mText;
				LotusGUIDispatcher.CurrentContent.image = mIcon;

				Single height = style.CalcHeight(LotusGUIDispatcher.CurrentContent, mRectWorldScreenMain.width - (PaddingLeft + PaddingRight));
				mRectWorldScreenMain.height = RoundToNearest(height, 10);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Основной плиточный элемент (тайл)
		/// </summary>
		/// <remarks>
		/// Плиточный элемент интерфейса располагается по сетке, и поддерживает анимацию спрайтов, динамическое перемещение и мигание, а также перемещение пользователем.
		/// Для комплексного управления анимацией плиточный элемент поддерживает технологию исполнения задачи
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUITileItem : CGUIBaseTileItem, ILotusTask
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Параметры системы анимации
			internal TDynamicParam mChangeParam;
			[SerializeField]
			internal Boolean mIsPause;
			[SerializeField]
			internal Single mDuration;
			[SerializeField]
			internal Int32 mGroupIndex;
			[SerializeField]
			internal Int32 mGroupSpriteIndex;
			[SerializeField]
			internal Int32 mStorageSpriteIndex;
			[SerializeField]
			internal Int32 mStartFrame;
			[SerializeField]
			internal Int32 mTargetFrame;
			[SerializeField]
			internal Color mColorStarting;
			[SerializeField]
			internal Color mColorTarget;

			// Данные по исполнению задачи
			[NonSerialized]
			internal Int32 mTaskOrder;

			// Служебные данные
			[NonSerialized]
			internal Int32 mCurrentFrameIndex;
			[NonSerialized]
			internal Color mCurrentColor;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ПАРАМЕТРЫ СИСТЕМЫ АНИМАЦИИ
			//
			/// <summary>
			/// Набор параметров для изменения
			/// </summary>
			public TDynamicParam ChangeParam
			{
				get { return mChangeParam; }
				set { mChangeParam = value; }
			}

			/// <summary>
			/// Продолжительность изменения параметров в секундах
			/// </summary>
			public Single Duration
			{
				get { return mDuration; }
				set { mDuration = value; }
			}

			/// <summary>
			/// Индекс группы анимационных цепочек
			/// </summary>
			public Int32 GroupIndex
			{
				get { return mGroupIndex; }
				set
				{
					mGroupIndex = value;
					mStorageSpriteIndex = LotusTweenDispatcher.SpriteStorage.GetStorageSpriteIndex(mGroupIndex, mGroupSpriteIndex);
				}
			}

			/// <summary>
			/// Имя группы анимационных цепочек
			/// </summary>
			public String GroupName
			{
				get
				{
					if (mGroupIndex == -1)
					{
						return (String.Empty);
					}
					else
					{
						return LotusTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex].Name;
					}
				}
			}

			/// <summary>
			/// Индекс анимационной цепочки в группе
			/// </summary>
			public Int32 GroupSpriteIndex
			{
				get { return mGroupSpriteIndex; }
				set
				{
					if (value < 0) value = 0;
					if (value > LotusTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex].Count - 1)
					{
						value = LotusTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex].Count - 1;
					}
					mGroupSpriteIndex = value;
					mStorageSpriteIndex = LotusTweenDispatcher.SpriteStorage.GetStorageSpriteIndex(mGroupIndex, mGroupSpriteIndex);
				}
			}

			/// <summary>
			/// Имя анимационной цепочки из группы
			/// </summary>
			public String GroupSpriteName
			{
				get
				{
					if (mGroupSpriteIndex == -1)
					{
						return String.Empty;
					}
					else
					{
						return (LotusTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex][mGroupSpriteIndex].Name);
					}
				}
			}

			/// <summary>
			/// Индекс анимационной цепочки из хранилища анимаций спрайтов
			/// </summary>
			public Int32 StorageSpriteIndex
			{
				get { return mStorageSpriteIndex; }
				set
				{
					mStorageSpriteIndex = value;
					LotusTweenDispatcher.SpriteStorage.GetGroupIndexAndSpriteIndex(mStorageSpriteIndex, ref mGroupIndex, ref mGroupSpriteIndex);
				}
			}

			/// <summary>
			/// Начальный кадр
			/// </summary>
			public Int32 StartFrame
			{
				get { return mStartFrame; }
				set { mStartFrame = value; }
			}

			/// <summary>
			/// Целевой кадр
			/// </summary>
			public Int32 TargetFrame
			{
				get { return mTargetFrame; }
				set { mTargetFrame = value; }
			}

			/// <summary>
			/// Текущий проигрываемый спрайт
			/// </summary>
			public Sprite FrameSprite
			{
				get
				{
					if (mStorageSpriteIndex == -1)
					{
						return null;
					}
					else
					{
						return (LotusTweenDispatcher.SpriteStorage.GetFrameSprite(mGroupIndex, mGroupSpriteIndex, mCurrentFrameIndex));
					}
				}
			}

			/// <summary>
			/// Начальная позиция
			/// </summary>
			public Vector2 LocationStarting
			{
				get { return mLocationStarting; }
				set { mLocationStarting = value; }
			}

			/// <summary>
			/// Конечная позиция
			/// </summary>
			public Vector2 LocationTarget
			{
				get { return mLocationTarget; }
				set { mLocationTarget = value; }
			}

			/// <summary>
			/// Начальный цвет
			/// </summary>
			public Color ColorStarting
			{
				get { return mColorStarting; }
				set { mColorStarting = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusTask =======================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			public virtual Boolean IsTaskCompleted
			{
				get { return IsDirty == false; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUITileItem()
				: base()
			{
				mStyleMainName = "Label";
				mDuration = 0.4f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUITileItem(String name)
				: base(name)
			{
				mStyleMainName = "Label";
				mDuration = 0.4f;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusTask =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальня работа по подготовки к выполнению задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void RunTask()
			{
				// Флаг изменения позиции
				if (mChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					mRectWorldScreenMain.position = mLocationStarting;
				}

				// Флаг изменения цвета
				if (mChangeParam.IsFlagSet(TDynamicParam.ColorText))
				{
					mCurrentColor = mColorStarting;
				}

				// Флаг анимации спрайта
				if (mChangeParam.IsFlagSet(TDynamicParam.AnimationSprite))
				{
					mCurrentFrameIndex = mStartFrame = 0;
					mTargetFrame = LotusTweenDispatcher.SpriteStorage.GetFrameCount(mGroupIndex, mGroupSpriteIndex) - 1;
				}

				IsDirty = true;
				mStartTime = Time.time;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи. Метод будет вызываться каждый раз в зависимости от способа
			/// и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ExecuteTask()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void StopTask()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ResetTask()
			{

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
				// Считаем время
				Single delta_time = (Time.time - mStartTime) / mDuration;
				if (delta_time > 1)
				{
					delta_time = 1;
					IsDirty = false;
				}

				// Флаг изменения позиции
				if (mChangeParam.IsFlagSet(TDynamicParam.Location))
				{
					mRectWorldScreenMain.x = Mathf.Lerp(mLocationStarting.x, mLocationTarget.x, delta_time);
					mRectWorldScreenMain.y = Mathf.Lerp(mLocationStarting.y, mLocationTarget.y, delta_time);
				}

				// Флаг изменения цвета
				if (mChangeParam.IsFlagSet(TDynamicParam.ColorText))
				{
					mCurrentColor = Color.Lerp(mColorStarting, mColorTarget, delta_time);
				}

				// Флаг анимации спрайта
				if (mChangeParam.IsFlagSet(TDynamicParam.AnimationSprite))
				{
					mCurrentFrameIndex = mStartFrame + (Int32)((mTargetFrame - mStartFrame) * delta_time);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.depth = mDepth;
				DraggingTile();

				GUI.Box(mRectWorldScreenMain, "", mStyleMain);

				// Флаг анимации спрайта
				if (mChangeParam.IsFlagSet(TDynamicParam.AnimationSprite))
				{
					Sprite sprite = FrameSprite;
					if (sprite != null)
					{
						Rect texture_coord = sprite.textureRect;
						texture_coord.x /= sprite.texture.width;
						texture_coord.y /= sprite.texture.height;
						texture_coord.width /= sprite.texture.width;
						texture_coord.height /= sprite.texture.height;
						GUI.DrawTextureWithTexCoords(mRectWorldScreenMain, sprite.texture, texture_coord, true);
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного дубликата элемента
			/// </summary>
			/// <returns>Полный дубликат элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override CGUIBaseElement Duplicate()
			{
				return MemberwiseClone() as CGUITileItem;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Копирование данных с другого элемента
			/// </summary>
			/// <param name="base_element">Источник данных</param>
			//---------------------------------------------------------------------------------------------------------
			public override void CopyFrom(CGUIBaseElement base_element)
			{
				base.CopyFrom(base_element);

				CGUITileItem tile_item = base_element as CGUITileItem;
				if (tile_item != null)
				{
					mChangeParam = tile_item.mChangeParam;
					mIsPause = tile_item.mIsPause;
					mDuration = tile_item.mDuration;

					mGroupIndex = tile_item.mGroupIndex;
					mGroupSpriteIndex = tile_item.mGroupSpriteIndex;
					mStorageSpriteIndex = tile_item.mStorageSpriteIndex;
					mStartFrame = tile_item.mStartFrame;
					mTargetFrame = tile_item.mTargetFrame;

					mColorStarting = tile_item.mColorStarting;
					mColorTarget = tile_item.mColorTarget;

					mTaskOrder = tile_item.mTaskOrder;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Сетка для размещения и управления тайлами
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIGridTile : CGUIElement
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Допуск применяемый при вычислении позиции тайлов
			/// </summary>
			public const Single DELTA = 1;
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Строки и столбцы
			[SerializeField]
			internal Int32 mColumnCount;
			[SerializeField]
			internal Int32 mRowCount;
			[SerializeField]
			internal Vector2 mSpaceTile;
			[NonSerialized]
			internal Vector2 mSizeTile;

			// Дочерние элемента
			[SerializeField]
			internal List<CGUIBaseTileItem> mTiles;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Количество столбцов
			/// </summary>
			public Int32 ColumnCount
			{
				get { return mColumnCount; }
				set
				{
					if (mColumnCount != value)
					{
						mColumnCount = value;
						ComputeSizeTile();
					}
				}
			}

			/// <summary>
			/// Количество строк
			/// </summary>
			public Int32 RowCount
			{
				get { return mRowCount; }
				set
				{
					if (mRowCount != value)
					{
						mRowCount = value;
						ComputeSizeTile();
					}
				}
			}

			/// <summary>
			/// Расстояние между тайлами
			/// </summary>
			public Vector2 SpaceTile
			{
				get { return mSpaceTile; }
				set
				{
					if (mSpaceTile != value)
					{
						mSpaceTile = value;
						ComputeSizeTile();
					}
				}
			}

			/// <summary>
			/// Расстояние между тайлами по горизонтали
			/// </summary>
			public Single SpaceTileX
			{
				get { return mSpaceTile.x; }
				set
				{
					if (mSpaceTile.x != value)
					{
						mSpaceTile.x = value;
						ComputeSizeTile();
					}
				}
			}

			/// <summary>
			/// Расстояние между тайлами по вертикали
			/// </summary>
			public Single SpaceTileY
			{
				get { return mSpaceTile.y; }
				set
				{
					if (mSpaceTile.y != value)
					{
						mSpaceTile.y = value;
						ComputeSizeTile();
					}
				}
			}

			/// <summary>
			/// Размер тайла
			/// </summary>
			public Vector2 SizeTile
			{
				get { return mSizeTile; }
			}

			/// <summary>
			/// Размер тайла по горизонтали
			/// </summary>
			public Single SizeTileX
			{
				get { return mSizeTile.x; }
			}

			/// <summary>
			/// Размер тайла по вертикали
			/// </summary>
			public Single SizeTileY
			{
				get { return mSizeTile.y; }
			}

			/// <summary>
			/// Все тайлы для отображения
			/// </summary>
			public List<CGUIBaseTileItem> Tiles
			{
				get { return mTiles; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIGridTile()
				: base()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIGridTile(String name)
				: base(name)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIGridTile(String name, Single x, Single y)
				: base(name, x, y)
			{
	
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusBasePlaceable2D ============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление позиции и размеров элемента по относительным данным
			/// </summary>
			/// <remarks>
			/// На основании относительной позиции элемента считается его абсолютная позиция в экранных координатах
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdatePlacement()
			{
				UpdatePlacementBase();
				UpdatePositionAndSizeTiles();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка/обновление параметров
			/// </summary>
			/// <remarks>
			/// Вызывается центральным диспетчером в момент добавления(регистрации) элемента
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void OnReset()
			{
				base.OnReset();
				this.ComputeSizeTile();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnUpdate()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void OnDraw()
			{
				GUI.enabled = IsEnabledElement;
				GUI.depth = mDepth;

				GUI.Box(mRectWorldScreenMain, "", mStyleMain);

				for (Int32 i = 0; i < mTiles.Count; i++)
				{
					mTiles[i].OnDraw();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ТАЙЛАМИ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение экранной позиции указанного столбца
			/// </summary>
			/// <param name="column">Индекс столбца (позиции по X в менеджере тайлов)</param>
			/// <returns>Экранная позиция столбца</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetPositionColumn(Int32 column)
			{
				Single x = column * (mSizeTile.x + mSpaceTile.x) + mRectWorldScreenMain.x + PaddingLeft;
				if (column == 0)
				{
					x = mRectWorldScreenMain.x + PaddingLeft;
				}

				return x;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение экранной позиции указанной строки
			/// </summary>
			/// <param name="row">Индекс строки (позиции по Y в менеджере тайлов)</param>
			/// <returns>Экранная позиция строки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetPositionRow(Int32 row)
			{
				Single y = row * (mSizeTile.y + mSpaceTile.y) + mRectWorldScreenMain.y + PaddingTop;
				if (row == 0)
				{
					y = mRectWorldScreenMain.y + PaddingTop;
				}

				return y;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение тайла посредством указанного индекса столбца и строки
			/// </summary>
			/// <param name="column">Индекс столбца (позиции по X в менеджере тайлов)</param>
			/// <param name="row">Индекс строки (позиции по Y в менеджере тайлов)</param>
			/// <returns>Найденный тайл или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseTileItem GetTileFromColumnAndRow(Int32 column, Int32 row)
			{
				CGUIBaseTileItem tile = null;
				for (Int32 i = 0; i < mTiles.Count; i++)
				{
					if(mTiles[i].ColumnIndex == column && mTiles[i].RowIndex == row)
					{
						tile = mTiles[i];
						break;
					}
				}
				return tile;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса столбца посредством экранной позиции
			/// </summary>
			/// <param name="position">Экранная позиция</param>
			/// <returns>Индекс столбца</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetColumnFromPosition(Single position)
			{
				Int32 column = 0;
				if (Approximately(position, mRectWorldScreenMain.x + PaddingLeft, DELTA))
				{
					column = 0;
				}
				else
				{
					Single pos_offset = position - (mRectWorldScreenMain.x + PaddingLeft);
					Single index_column = pos_offset / (mSizeTile.x + mSpaceTile.x);
					column = (Int32)(index_column + 0.01f);
				}

				return column;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса строки посредством экранной позиции
			/// </summary>
			/// <param name="position">Экранная позиция</param>
			/// <returns>Индекс строки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetRowFromPosition(Single position)
			{
				Int32 row = 0;
				if (Approximately(position, mRectWorldScreenMain.y + PaddingTop, DELTA))
				{
					row = 0;
				}
				else
				{
					Single pos_offset = position - (mRectWorldScreenMain.y + PaddingTop);
					Single index_row = pos_offset / (mSizeTile.y + mSpaceTile.y);
					row = (Int32)(index_row + 0.01f);
				}

				return row;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание и добавление нового тайла
			/// </summary>
			/// <remarks>
			/// Созданный тайл автоматически помещается в свободную ячейку
			/// </remarks>
			/// <typeparam name="TTile">Тип тайла</typeparam>
			/// <returns>Созданный тайл</returns>
			//---------------------------------------------------------------------------------------------------------
			public TTile AddNewTile<TTile>() where TTile : CGUIBaseTileItem, new()
			{
				TTile tile = new TTile();
				tile.TileManager = this;
				tile.SetVisibilityFlags(1);
				tile.IsRegisterDispatcher = true;
				mTiles.Add(tile);
				ComputePositionTiles();
				return tile;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание и добавление нового тайла из префаба
			/// </summary>
			/// <remarks>
			/// Созданный тайл автоматически помещается в свободную ячейку
			/// </remarks>
			/// <typeparam name="TTile">Тип тайла</typeparam>
			/// <param name="prefab">Префаб тайла</param>
			/// <returns>Созданный тайл</returns>
			//---------------------------------------------------------------------------------------------------------
			public TTile AddNewTileFromPrefab<TTile>(TTile prefab) where TTile : CGUIBaseTileItem, new()
			{
				TTile tile = prefab.Duplicate() as TTile;
				tile.TileManager = this;
				tile.SetVisibilityFlags(1);
				tile.IsRegisterDispatcher = true;
				mTiles.Add(tile);
				ComputePositionTiles();
				return tile;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление существующего тайла
			/// </summary>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(CGUITileItem tile)
			{
				tile.TileManager = this;
				mTiles.Add(tile);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление тайла по индексу
			/// </summary>
			/// <param name="index">Индекс удаляемого тайла</param>
			//---------------------------------------------------------------------------------------------------------
			public void Remove(Int32 index)
			{
				mTiles.RemoveAt(index);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление тайла
			/// </summary>
			/// <param name="tile">Удаляемый тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public void Remove(CGUITileItem tile)
			{
				mTiles.Remove(tile);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление всех тайлов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Clear()
			{
				mTiles.Clear();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление размеров тайла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeSizeTile()
			{
				Single w = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight) - (mColumnCount - 1) * mSpaceTile.x;
				mSizeTile.x = w / mColumnCount;

				Single h = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom) - (mRowCount - 1) * mSpaceTile.y;
				mSizeTile.y = h / mRowCount;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление последовательной позиций тайлов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ComputePositionTiles()
			{
				for (Int32 ix = 0; ix < mColumnCount; ix++)
				{
					for (Int32 iy = 0; iy < mRowCount; iy++)
					{
						Int32 index = ix + iy * mColumnCount;
						if (index < mTiles.Count)
						{
							CGUIBaseTileItem tile = mTiles[index];
							if (ix == 0)
							{
								tile.mRectWorldScreenMain.x = mRectWorldScreenMain.x + PaddingLeft;
							}
							else
							{
								tile.mRectWorldScreenMain.x = ix * (mSizeTile.x + mSpaceTile.x) + mRectWorldScreenMain.x + PaddingLeft;
							}
							if (iy == 0)
							{
								tile.mRectWorldScreenMain.y = mRectWorldScreenMain.y + PaddingTop;
							}
							else
							{
								tile.mRectWorldScreenMain.y = iy * (mSizeTile.y + mSpaceTile.y) + mRectWorldScreenMain.y + PaddingTop;
							}

							tile.mColumnIndex = ix;
							tile.mRowIndex = iy;
							tile.mRectWorldScreenMain.width = mSizeTile.x;
							tile.mRectWorldScreenMain.height = mSizeTile.y;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление позиции и размеров тайлов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdatePositionAndSizeTiles()
			{
				Single w = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight) - (mColumnCount - 1) * mSpaceTile.x;
				mSizeTile.x = w / mColumnCount;

				Single h = mRectWorldScreenMain.height - (PaddingTop + PaddingBottom) - (mRowCount - 1) * mSpaceTile.y;
				mSizeTile.y = h / mRowCount;

				for (Int32 i = 0; i < mTiles.Count; i++)
				{
					mTiles[i].mRectWorldScreenMain.x = this.GetPositionColumn(mTiles[i].ColumnIndex);
					mTiles[i].mRectWorldScreenMain.y = this.GetPositionRow(mTiles[i].RowIndex);
					mTiles[i].mRectWorldScreenMain.width = mSizeTile.x;
					mTiles[i].mRectWorldScreenMain.height = mSizeTile.y;
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