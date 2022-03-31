//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteGridTileTemplate.cs
*		LotusUIGridTileTemplate - шаблонный элемент представляющий сетку для расположения тайловых элементов.
*		Реализация шаблонного элемента представляющего сетку для расположения тайловых элементов.
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
		//! \addtogroup Unity2DSpriteExtended
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// GridTile - шаблонный элемент представляющий сетку для расположения тайловых элементов
		/// </summary>
		/// <remarks>
		/// Реализация шаблонного элемента представляющего сетку для расположения тайловых элементов
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class LotusSpriteGridTileTemplate<TTile, TGridCell> : LotusSpriteGridTileBase where TTile : LotusSpriteTileBase
			where TGridCell : ILotusGridTileCell, new()
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[NonSerialized]
			internal List<TTile> mTiles;

			// Параметры ячеек сетки
			[NonSerialized]
			internal TGridCell[] mCells;

			// События системы анимации
			[NonSerialized]
			internal Boolean mIsPauseAnimation;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Все тайлы
			/// </summary>
			public List<TTile> Tiles
			{
				get { return mTiles; }
			}

			//
			// ПАРАМЕТРЫ ЯЧЕЕК СЕТКИ
			//
			/// <summary>
			/// Параметры ячеек сетки
			/// </summary>
			public TGridCell[] Cells
			{
				get { return mCells; }
			}

			//
			// СОБЫТИЯ СИСТЕМЫ АНИМАЦИИ
			//
			/// <summary>
			/// Пауза анимации
			/// </summary>
			public Boolean IsPauseAnimation
			{
				get { return mIsPauseAnimation; }
				set
				{
					mIsPauseAnimation = value;
					for (Int32 i = 0; i < mTiles.Count; i++)
					{
						mTiles[i].IsPauseAnimation = value;
					}
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusPoolManager ================================
			/// <summary>
			/// Максимальное количество объектов для пула
			/// </summary>
			/// <remarks>
			/// В случае, если по запросу объектов в пуле не будет, то это значение увеличится вдвое и создаться указанное количество объектов
			/// </remarks>
			public virtual Int32 MaxInstances
			{
				get { return (0); }
				set { }
			}

			/// <summary>
			/// Количество объектов в пуле
			/// </summary>
			public virtual Int32 InstanceCount
			{
				get { return (0); }
				set { }
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
				OnInitCells();
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
				OnInitCells();
				OnInitGridBase();
				ComputeSizeTile();
				ComputeSnapTiles();
			}

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование UnityGUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public new void OnGUI()
			{
				OnGUIDrawInfo();
			}
#endif

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование границ ячейки и размеров
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void OnGUIDrawInfo()
			{
				if (mIsDrawCell)
				{
					Rect rect = RectScreen;
					XGUIRender.StrokeColor = mDrawCellColor;
					Color save_contentcolor = GUI.contentColor;
					GUI.contentColor = mDrawCellColor;

					Single x = PaddingLeft;
					Single y = PaddingTop;

					for (Int32 ir = 0; ir < mRowCount; ir++)
					{
						for (Int32 ic = 0; ic < mColumnCount; ic++)
						{
							Vector2 pos = GetWorldScreenFromLocalDesign(x, y);
							Single w = SizeTileX * LotusSpriteDispatcher.ScaledScreenX;
							Single h = SizeTileY * LotusSpriteDispatcher.ScaledScreenY;

							Rect rect_cell = new Rect(pos.x, pos.y, w, h);

							XGUIRender.DrawRect(rect_cell);

							GUI.Label(rect_cell, String.Format("R={0}\nC={1}\nE={2}", this[ic, ir].RowIndex,
								this[ic, ir].ColumnIndex, this[ic, ir].IsEmpty), UnityEditor.EditorStyles.boldLabel);

							x += mSizeTile.x + mSpaceTile.x;
						}

						y += mSizeTile.y + mSpaceTile.y;
						x = PaddingLeft;
					}

					GUI.contentColor = save_contentcolor;
				}

				if (mIsDrawTile)
				{
				}
			}
#endif
			#endregion

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Доступ к ячейки
			/// </summary>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			/// <returns>Данные ячейки</returns>
			//---------------------------------------------------------------------------------------------------------
			public TGridCell this[Int32 column, Int32 row]
			{
				get
				{
					return (mCells[column + mColumnCount * row]);
				}
				set
				{
					mCells[column + mColumnCount * row] = value;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ТАЙЛАМИ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных тайлов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnInitGridBase()
			{
				if (mTiles == null)
					mTiles = new List<TTile>();
				if (mSnapWorldTransformTiles == null)
					mSnapWorldTransformTiles = new CListSnapPoint2D();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка тайлов из дочерних элементов компонента трансформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void SetTilesFromTransformChildren()
			{
				mTiles.Clear();
				for (Int32 i = 0; i < transform.childCount; i++)
				{
					TTile tile = transform.GetChild(i).GetComponent<TTile>();
					if (tile != null)
					{
						mTiles.Add(tile);
						tile.OwnerGrid = this;
						tile.Width = SizeTileX;
						tile.Height = SizeTileY;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение тайла по указанной строки и столбца
			/// </summary>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			/// <returns>Тайл или null если тайла в данной позиции нет</returns>
			//---------------------------------------------------------------------------------------------------------
			public override TTileElemnt GetTileBase<TTileElemnt>(Int32 column, Int32 row)
			{
				for (Int32 i = 0; i < mTiles.Count; i++)
				{
					if (mTiles[i].RowIndex == row && mTiles[i].ColumnIndex == column)
					{
						return mTiles[i] as TTileElemnt;
					}
				}

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
			public override TTileElemnt GetTileOfType<TTileElemnt>(Int32 column, Int32 row)
			{
				for (Int32 i = 0; i < mTiles.Count; i++)
				{
					if (mTiles[i].RowIndex == row && mTiles[i].ColumnIndex == column && mTiles[i] is TTileElemnt)
					{
						return mTiles[i] as TTileElemnt;
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение тайла по указанной строки и столбца
			/// </summary>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			/// <returns>Тайл или null если тайла в данной позиции нет</returns>
			//---------------------------------------------------------------------------------------------------------
			public TTile GetTile(Int32 column, Int32 row)
			{
				for (Int32 i = 0; i < mTiles.Count; i++)
				{
					if (mTiles[i].RowIndex == row && mTiles[i].ColumnIndex == column)
					{
						return mTiles[i];
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление существующего тайла
			/// </summary>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddExsistTile(TTile tile)
			{
				if(mTiles.Contains(tile))
				{
					Debug.Log("This tile <" + tile.Name + "> is exsist");
					return;
				}

				tile.mOwnerGrid = this;
				tile.SetParent(mThisTransform);
				tile.Width = SizeTileX;
				tile.Height = SizeTileY;

				// Добавление в базу
				mTiles.Add(tile);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление существующего тайла и установка его позиции
			/// </summary>
			/// <param name="tile">Тайл</param>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddExsistTile(TTile tile, Int32 column, Int32 row)
			{
				if (mTiles.Contains(tile))
				{
					Debug.Log("This tile <" + tile.Name + "> is exsist");
					return;
				}

				tile.mOwnerGrid = this;
				tile.SetParent(mThisTransform);
				tile.Width = SizeTileX;
				tile.Height = SizeTileY;
				tile.ColumnIndex = column;
				tile.RowIndex = row;

				// Добавление в базу
				mTiles.Add(tile);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание и добавление тайла из прeфаба
			/// </summary>
			/// <param name="prefab">Префаб</param>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			/// <returns>Тайл</returns>
			//---------------------------------------------------------------------------------------------------------
			public TTile AddNewTileFromPrefab(TTile prefab, Int32 column, Int32 row)
			{
				// Создаем игровой объект
				TTile tile = GameObject.Instantiate(prefab);
				tile.mOwnerGrid = this;
				tile.SetParent(mThisTransform);

				// Установка позиции
				tile.Width = SizeTileX;
				tile.Height = SizeTileY;
				tile.ColumnIndex = column;
				tile.RowIndex = row;

				// Добавление в базу
				mTiles.Add(tile);

				return tile;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаления тайла из списка
			/// </summary>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveTile(TTile tile)
			{
				mTiles.Remove(tile);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаления тайла из списка вместе с игровым объектом
			/// </summary>
			/// <param name="tile">Тайл</param>
			//---------------------------------------------------------------------------------------------------------
			public void DeleteTile(TTile tile)
			{
				if (mTiles.IndexOf(tile) > -1)
				{
					XGameObjectDispatcher.Destroy(tile.gameObject);
					mTiles.Remove(tile);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление всех тайлов из списка вместе с игровыми объектами
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void DeleteTiles()
			{
				for (Int32 i = 0; i < mTiles.Count; i++)
				{
					XGameObjectDispatcher.Destroy(mTiles[i].gameObject);
				}

				mTiles.Clear();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление последовательной позиций тайлов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ComputePositionTiles()
			{
				for (Int32 iy = 0; iy < mRowCount; iy++)
				{
					for (Int32 ix = 0; ix < mColumnCount; ix++)
					{
						Int32 index = ix + iy * mColumnCount;
						if (index < mTiles.Count)
						{
							LotusSpriteTileBase tile = mTiles[index];
							tile.Size = mSizeTile;
							tile.SetColumnAndRow(ix, iy);
						}
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ЯЧЕЙКАМИ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных ячеек
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnInitCells()
			{
				mCells = new TGridCell[mColumnCount * mRowCount];

				for (Int32 iy = 0; iy < mRowCount; iy++)
				{
					for (Int32 ix = 0; ix < mColumnCount; ix++)
					{
						mCells[ix + mColumnCount * iy] = new TGridCell();
						mCells[ix + mColumnCount * iy].RowIndex = iy;
						mCells[ix + mColumnCount * iy].ColumnIndex = ix;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сброс данных ячеек
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ResetCells()
			{
				for (Int32 i = 0; i < mCells.Length; i++)
				{
					mCells[i].Reset();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка статуса свободных ячеек
			/// </summary>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			/// <param name="count_column">Количество ячеек по горизонтали</param>
			/// <param name="count_row">Количество ячеек по вертикали</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetEmptyCells(Int32 column, Int32 row, Int32 count_column, Int32 count_row)
			{
				for (Int32 ix = 0; ix < count_column; ix++)
				{
					for (Int32 iy = 0; iy < count_row; iy++)
					{
						this[column + ix, row + iy].IsEmpty = true;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка статуса занятости ячеек
			/// </summary>
			/// <param name="column">Индекс столбца</param>
			/// <param name="row">Индекс строки</param>
			/// <param name="count_column">Количество ячеек по горизонтали</param>
			/// <param name="count_row">Количество ячеек по вертикали</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetOccupiedCells(Int32 column, Int32 row, Int32 count_column, Int32 count_row)
			{
				for (Int32 ix = 0; ix < count_column; ix++)
				{
					for (Int32 iy = 0; iy < count_row; iy++)
					{
						this[column + ix, row + iy].IsEmpty = false;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка статуса ячеек (сводная или занята) в зависимости от нахождения тайла в данной ячейки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetStatusCellsFromTiles()
			{
				// 1) Обнуляем все ячейки
				for (Int32 i = 0; i < mCells.Length; i++)
				{
					mCells[i].IsEmpty = true;
				}

				// 2) Проходим по тайлам
				for (Int32 i = 0; i < mTiles.Count; i++)
				{
					TTile tile = mTiles[i];
					if (tile != null)
					{
						this[tile.ColumnIndex, tile.RowIndex].IsEmpty = false;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количества пустых ячеек
			/// </summary>
			/// <returns>Количества пустых ячеек</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetCountEmptyCells()
			{
				Int32 result = 0;
				for (Int32 i = 0; i < mCells.Length; i++)
				{
					if (mCells[i].IsEmpty)
					{
						result++;
					}
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количества занятых ячеек
			/// </summary>
			/// <returns>Количества занятых ячеек</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetCountOccupiedCells()
			{
				Int32 result = 0;
				for (Int32 i = 0; i < mCells.Length; i++)
				{
					if (mCells[i].IsEmpty)
					{
						result++;
					}
				}

				return result;
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ГРАНИЦАМИ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка горизонтальной границы
			/// </summary>
			/// <remarks>
			/// Граница ставится по нижнему краю ячейки
			/// </remarks>
			/// <param name="column_index">Индекс столбца</param>
			/// <param name="row_index">Индекс строки</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetBorderHorizontal(Int32 column_index, Int32 row_index)
			{
				if (row_index < mRowCount - 1)
				{
					this[column_index, row_index].IsBorderBottom = true;
					this[column_index, row_index + 1].IsBorderTop = true;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка вертикальной границы
			/// </summary>
			/// <remarks>
			/// Граница ставится по правому краю ячейки
			/// </remarks>
			/// <param name="column_index">Индекс столбца</param>
			/// <param name="row_index">Индекс строки</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetBorderVertical(Int32 column_index, Int32 row_index)
			{
				if (column_index < mColumnCount - 1)
				{
					this[column_index, row_index].IsBorderLeft = true;
					this[column_index + 1, row_index].IsBorderRight = true;
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