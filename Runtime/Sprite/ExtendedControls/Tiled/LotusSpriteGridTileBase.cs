//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteGridTileBase.cs
*		GridTileBase - базовый элемент представляющий сетку для расположения тайловых элементов.
*		Реализация базового элемента представляющего сетку для расположения тайловых элементов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
using Lotus.Algorithm;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Unity2DSpriteExtended Расширенные элементы управления
		//! Расширенные и специфичные элементы интерфейса. В данной группе реализованы элементы с иерархическим (древовидным) 
		//! отображением информации, динамические и тайловые элементы
		//! \ingroup Unity2DSprite
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// GridTileBase - базовый UI представляющий сетку для расположения тайловых элементов
		/// </summary>
		/// <remarks>
		/// Реализация базового UI представляющего сетку для расположения тайловых элементов
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XSpriteEditorSettings.MenuPathExtended + "Tiled/GridTileBase")]
		public class LotusSpriteGridTileBase : LotusSpriteElement, ILotusPointerDown, ILotusPointerMove, ILotusPointerUp
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mColumnCount;
			[SerializeField]
			internal Int32 mRowCount;
			[SerializeField]
			internal Vector2 mSpaceTile;
			[NonSerialized]
			internal Vector2 mSizeTile;
			[NonSerialized]
			internal CListSnapPoint2D mSnapWorldTransformTiles;

			// Параметры состояния
			[NonSerialized]
			internal Int32 mRowSelected;
			[NonSerialized]
			internal Int32 mColumnSelected;
			[NonSerialized]
			internal LotusSpriteTileBase mCurrentDraggTile;
			[NonSerialized]
			internal LotusSpriteTileBase mPreviousDraggTile;

#if UNITY_EDITOR
			// Рисование ячеек
			[SerializeField]
			internal Boolean mIsDrawCell;
			[SerializeField]
			internal Color mDrawCellColor = new Color(0.9f, 0.0f, 0.0f, 0.5f);

			// Рисование тайлов
			[SerializeField]
			internal Boolean mIsDrawTile;
			[SerializeField]
			internal Color mDrawTileColor = new Color(0.9f, 0.0f, 0.0f, 0.5f);

			// Рисование таблицы
			[SerializeField]
			internal Boolean mIsDrawGrid;
			[SerializeField]
			internal Color mDrawColorGrid = new Color(0.9f, 0.0f, 0.0f, 0.5f);
#endif
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
			/// Размер тайла в мировых координатах
			/// </summary>
			public Vector2 SizeTileWorld
			{
				get
				{
					return (new Vector2(mSizeTile.x * LotusSpriteDispatcher.ScaledScreenX / LotusSpriteDispatcher.CameraPixelsPerUnit,
						mSizeTile.y * LotusSpriteDispatcher.ScaledScreenY / LotusSpriteDispatcher.CameraPixelsPerUnit));
				}
			}

			/// <summary>
			/// Размер тайла по горизонтали
			/// </summary>
			public Single SizeTileX
			{
				get { return mSizeTile.x; }
			}

			/// <summary>
			/// Размер тайла по горизонтали в мировых координатах
			/// </summary>
			public Single SizeTileXWorld
			{
				get { return (mSizeTile.x * LotusSpriteDispatcher.ScaledScreenX / LotusSpriteDispatcher.CameraPixelsPerUnit); }
			}

			/// <summary>
			/// Размер тайла по вертикали
			/// </summary>
			public Single SizeTileY
			{
				get { return mSizeTile.y; }
			}

			/// <summary>
			/// Размер тайла по вертикали в мировых координатах
			/// </summary>
			public Single SizeTileYWorld
			{
				get { return (mSizeTile.y * LotusSpriteDispatcher.ScaledScreenY / LotusSpriteDispatcher.CameraPixelsPerUnit); }
			}

			/// <summary>
			/// Список позиций для привязки тайла к сетки
			/// </summary>
			/// <remarks>
			/// Вследствии неточностей и округлений не всего получается корректно посчитать позицию тайла в сетки 
			/// по его координатой позиции, поэтому для привязки используется готовый список позиций который 
			/// привязывается корректно, с учетом допусков
			/// </remarks>
			public CListSnapPoint2D SnapWorldTransformTiles
			{
				get { return mSnapWorldTransformTiles; }
			}

			//
			// ПАРАМЕТРЫ СОСТОЯНИЯ
			//
			/// <summary>
			/// Выделенная строка по нажатию
			/// </summary>
			public Int32 RowSelected
			{
				get { return mRowSelected; }
			}

			/// <summary>
			/// Выделенный столбец по нажатию
			/// </summary>
			public Int32 ColumnSelected
			{
				get { return mColumnSelected; }
			}

			/// <summary>
			/// Текущий перемещаемый тайл
			/// </summary>
			public LotusSpriteTileBase CurrentDraggTile
			{
				get { return mCurrentDraggTile; }
				set
				{
					mCurrentDraggTile = value;
				}
			}

			/// <summary>
			/// Предыдущий перемещаемый тайл
			/// </summary>
			public LotusSpriteTileBase PreviousDraggTile
			{
				get { return mPreviousDraggTile; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация элемента при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ResetElement()
			{
				OnInitGridBase();
				ComputeSizeTile();
				ComputeSnapTiles();
			}
#endif
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ConstructorElement()
			{
				OnInitGridBase();
				ComputeSizeTile();
				ComputeSnapTiles();
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusPointer ======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Событие нажатия
			/// </summary>
			/// <param name="pointer_id">Идентификатор указателя</param>
			//---------------------------------------------------------------------------------------------------------
			public void PointerDown(Int32 pointer_id)
			{
				// Проверка на вхождение
				if (ContainsScreen(XInputDispatcher.PositionPointer))
				{
					// Только если элемент доступен 
					if (mIsEnabled)
					{
						Vector2Int pos = GetColumnAndRowFromWorldScreen(XInputDispatcher.PositionPointer);
						mColumnSelected = pos.x;
						mRowSelected = pos.y;

						OnCellDown(mColumnSelected, mRowSelected);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Событие перемещения
			/// </summary>
			/// <param name="pointer_id">Идентификатор указателя</param>
			//---------------------------------------------------------------------------------------------------------
			public void PointerMove(Int32 pointer_id)
			{
				
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Событие отпускания
			/// </summary>
			/// <param name="pointer_id">Идентификатор указателя</param>
			//---------------------------------------------------------------------------------------------------------
			public void PointerUp(Int32 pointer_id)
			{
				// Только если элемент доступен 
				if (mIsEnabled)
				{
					// Информируем о щелчке на элементе только если указатель находиться в его пределах
					// Проверка на вхождение
					if (ContainsScreen(XInputDispatcher.PositionPointer))
					{
						OnCellClick(mColumnSelected, mRowSelected);
					}
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса столбца посредством позиции по X в локальных координатах
			/// </summary>
			/// <param name="position">Позиции по X в локальных координатах</param>
			/// <returns>Индекс столбца</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetColumnFromLocalDesign(Single position)
			{
				Int32 column = 0;
				if (CGUIBaseElement.Approximately(position, PaddingLeft, 1))
				{
					column = 0;
				}
				else
				{
					Single pos_offset = position - PaddingLeft;
					Single index_column = pos_offset / (mSizeTile.x + mSpaceTile.x);
					column = (Int32)(index_column + 0.01f);
				}

				return column;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса строки посредством позиции по Y в локальных координатах
			/// </summary>
			/// <param name="position">Позиции по Y в локальных координатах</param>
			/// <returns>Индекс строки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetRowFromLocalDesign(Single position)
			{
				Int32 row = 0;
				if (CGUIBaseElement.Approximately(position, PaddingTop, 1))
				{
					row = 0;
				}
				else
				{
					Single pos_offset = position - PaddingTop;
					Single index_row = pos_offset / (mSizeTile.y + mSpaceTile.y);
					row = (Int32)(index_row + 0.01f);
				}

				return row;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции указанной строки и столбца позиции в локальных координатах
			/// </summary>
			/// <param name="position">Позиция в локальных координатах</param>
			/// <returns>Позиции строки и столбца</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2Int GetColumnAndRowFromLocalDesign(Vector2 position)
			{
				return new Vector2Int(GetColumnFromLocalDesign(position.x), GetRowFromLocalDesign(position.y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса столбца посредством позиции по X в мировых координатах
			/// </summary>
			/// <param name="position">Позиции по X в мировых координатах</param>
			/// <returns>Индекс столбца</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetColumnFromWorldTransform(Single position)
			{
				// Сначала ищем по привязке с небольшим допуском
				Single epsilon_width = SizeTileXWorld / 4;
				return (mSnapWorldTransformTiles.FindIndexNearestFromPositionX(position, epsilon_width));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса строки посредством позиции по Y в мировых координатах
			/// </summary>
			/// <param name="position">Позиции по Y в мировых координатах</param>
			/// <returns>Индекс строки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetRowFromWorldTransform(Single position)
			{
				// Сначала ищем по привязке с небольшим допуском
				Single epsilon_height = SizeTileYWorld / 4;
				return (mSnapWorldTransformTiles.FindIndexNearestFromPositionY(position, epsilon_height));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции указанной строки и столбца посредством позиции в мировых координатах
			/// </summary>
			/// <param name="position">Позиция в мировых координатах</param>
			/// <returns>Позиции строки и столбца</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2Int GetColumnAndRowFromWorldTransform(Vector2 position)
			{
				// Сначала ищем по привязке с небольшим допуском
				Int32 index = 0;
				Single epsilon_width = SizeTileXWorld / 4;
				Single epsilon_height = SizeTileYWorld / 4;

				index = mSnapWorldTransformTiles.FindIndexNearestFromPosition(position, epsilon_width, epsilon_height);

				// Не нашли обычным образом
				if(index == -1)
				{
					// Ищем через расстояние
					mSnapWorldTransformTiles.ComputeDistance(position);
					index = mSnapWorldTransformTiles.GetMinimumDistanceIndex();
					Debug.Log("GetMinimumDistanceIndex");
				}

				Int32 column = index % mColumnCount;
				Int32 row = index / mColumnCount;

				return new Vector2Int(column, row);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции строки и столбца посредством позиции в экранных координатах
			/// </summary>
			/// <param name="position">Позиция в экранных координатах</param>
			/// <returns>Позиции строки и столбца</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2Int GetColumnAndRowFromWorldScreen(Vector2 position)
			{
				return new Vector2Int(0, 0);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции тайла в локальных координатах указанной строки и столбца
			/// </summary>
			/// <param name="column">Индекс столбца (позиции по X в сетки)</param>
			/// <param name="row">Индекс строки (позиции по Y в сетки)</param>
			/// <returns>Позиция тайла в локальных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2 GetLocalDesignTile(Int32 column, Int32 row)
			{
				Single x = PaddingLeft + column * (mSizeTile.x + mSpaceTile.x);
				Single y = PaddingTop + row * (mSizeTile.y + mSpaceTile.y);

				return new Vector2(x, y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции тайла в мировых координатах трансформации указанной строки и столбца
			/// </summary>
			/// <param name="column">Индекс столбца (позиции по X в сетки)</param>
			/// <param name="row">Индекс строки (позиции по Y в сетки)</param>
			/// <returns>Позиция тайла в мировых координатах трансформации</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2 GetWorldTransformTile(Int32 column, Int32 row)
			{
				return (mSnapWorldTransformTiles[row * mColumnCount + column].Point);
			}
			#endregion

			#region ======================================= МЕТОДЫ РАЗМЕЩЕНИЯ СОДЕРЖИМОГО =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление размеров тайла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeSizeTile()
			{
				Single w = Width - (PaddingLeft + PaddingRight) - ((mColumnCount - 1) * mSpaceTile.x);
				mSizeTile.x = w / mColumnCount;

				Single h = Height - (PaddingTop + PaddingBottom) - ((mRowCount - 1) * mSpaceTile.y);
				mSizeTile.y = h / mRowCount;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление узлов привязки тайлов к мировым координатам
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeSnapTiles()
			{
				if (mSnapWorldTransformTiles != null)
				{
					Vector2 snap = new Vector2(PaddingLeft + SizeTile.x / 2, PaddingTop + SizeTile.y / 2);
					mSnapWorldTransformTiles.Clear();

					// Порядок заполнения
					// 0,1,2,3,4,5,6
					// 7,8,9,10,11,12

					for (Int32 iy = 0; iy < mRowCount; iy++)
					{
						for (Int32 ix = 0; ix < mColumnCount; ix++)
						{
							// Переводим в мировые координаты
							Vector3 position = GetWorldTransformFromLocalDesign(snap.x, snap.y);
							mSnapWorldTransformTiles.Add(position);
							snap.x += mSizeTile.x + mSpaceTile.x;
						}

						snap.y += mSizeTile.y + mSpaceTile.y;
						snap.x = PaddingLeft + SizeTile.x / 2;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение оптимальной ширины тайла при текущем размере
			/// </summary>
			/// <returns>Оптимальная ширина тайла</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetOptimalTileWidth()
			{
				// Считаем ширину
				Single w = Width - (PaddingLeft + PaddingRight) - ((mColumnCount - 1) * SpaceTileX);

				// Размер тайла
				w = w / mColumnCount;

				// Округляем до двух
				w = CGUIBaseElement.RoundToNearest(w, 2);

				return (w);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение оптимальной высоты тайла при текущем размере
			/// </summary>
			/// <returns>Оптимальная высота тайла</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetOptimalTileHeight()
			{
				// Считаем высоту
				Single h = Height - (PaddingTop + PaddingBottom) - ((mRowCount - 1) * SpaceTileY);

				// Размер тайла
				h = h / mRowCount;

				// Округляем до двух
				h = CGUIBaseElement.RoundToNearest(h, 2);

				return (h);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка ширины сетки по оптимальной ширине тайла
			/// </summary>
			/// <param name="optimal_tile_width">Оптимальная ширина тайла</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetWidthGridFromOptimalTileWidth(Single optimal_tile_width)
			{
				Single w = optimal_tile_width * mColumnCount;
				w += (PaddingLeft + PaddingRight) + ((mColumnCount - 1) * SpaceTileX);
				Width = w;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка высоты сетки по оптимальной высоте тайла
			/// </summary>
			/// <param name="optimal_tile_height">Оптимальная высота тайла</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetHeightGridFromOptimalTileHeight(Single optimal_tile_height)
			{
				Single h = optimal_tile_height * mRowCount;
				h += (PaddingTop + PaddingBottom) + ((mRowCount - 1) * SpaceTileY);
				Height = h;
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ТАЙЛАМИ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных тайлов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnInitGridBase()
			{
				if (mSnapWorldTransformTiles == null) mSnapWorldTransformTiles = new CListSnapPoint2D();
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение тайла по указанной строки и столбца
			/// </summary>
			/// <typeparam name="TTileElemnt">Тип тайла</typeparam>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			/// <returns>Тайл или null если тайла в данной позиции нет</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual TTileElemnt GetTileBase<TTileElemnt>(Int32 column, Int32 row) where TTileElemnt : LotusSpriteTileBase
			{
				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение тайла указанного типа по указанной строки и столбца
			/// </summary>
			/// <typeparam name="TTileElemnt">Тип тайла</typeparam>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			/// <returns>Тайл или null если тайла в данной позиции нет</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual TTileElemnt GetTileOfType<TTileElemnt>(Int32 column, Int32 row) where TTileElemnt : LotusSpriteTileBase
			{
				return null;
			}
			#endregion

			#region ======================================= МЕТОДЫ СОБЫТИЙ ТАЙЛОВ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Внешние событие при перемещения указанного тайла
			/// </summary>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnTileDragExternal(LotusSpriteTileBase tile)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Внешние событие при окончании перемещения указанного тайла
			/// </summary>
			/// <param name="tile">Тайл</param>
			/// <param name="column">Индекс столбца куда опустился</param>
			/// <param name="row">Индекс строки куда опустился</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnTileEndDragExternal(LotusSpriteTileBase tile, Int32 column, Int32 row)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Внешние событие окончания анимации цвета тайла
			/// </summary>
			/// <param name="animation_name">Имя анимации</param>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnTileAnimationColorCompleted(String animation_name, LotusSpriteTileBase tile)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Внешние событие окончания анимации вращения тайла
			/// </summary>
			/// <param name="animation_name">Имя анимации (содержит название оси вращения)</param>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnTileAnimationRotationCompleted(String animation_name, LotusSpriteTileBase tile)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Внешние событие окончания анимации масштабирования тайла
			/// </summary>
			/// <param name="animation_name">Имя анимации</param>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnTileAnimationScaleCompleted(String animation_name, LotusSpriteTileBase tile)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Внешние событие окончания анимации перемещения тайла
			/// </summary>
			/// <typeparam name="TTile">Тип тайла</typeparam>
			/// <param name="animation_name">Имя анимации</param>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnTileAnimationTranslateCompleted(String animation_name, LotusSpriteTileBase tile)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Внешние событие окончания анимации спрайтов тайла
			/// </summary>
			/// <typeparam name="TTile">Тип тайла</typeparam>
			/// <param name="animation_name">Имя анимации</param>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnTileAnimationSpriteCompleted(String animation_name, LotusSpriteTileBase tile)
			{
			}
			#endregion

			#region ======================================= МЕТОДЫ СОБЫТИЙ ЯЧЕЕК ======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Внутреннее событие при нажатие ячейку
			/// </summary>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnCellDown(Int32 column, Int32 row)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Внутреннее событие при щелчке на ячейку
			/// </summary>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnCellClick(Int32 column, Int32 row)
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