//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Расширенные элементы управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteTileBase.cs
*		TileBase - элемент представляющий базовый тайловый элемент.
*		Реализация элемент представляющий тайловый элемент с базовой функциональностью.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
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
		/// TileBase - элемент представляющий базовый тайловый элемент
		/// </summary>
		/// <remarks>
		/// Реализация элемент представляющий тайловый элемент с базовой функциональностью
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XSpriteEditorSettings.MenuPathExtended + "Tiled/Tile Base")]
		public class LotusSpriteTileBase : LotusSpriteElement, IComparable<LotusSpriteTileBase>, ILotusTask, 
			ILotusPoolObject, ILotusDraggable, ILotusPointerDown, ILotusPointerMove, ILotusPointerUp
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента TileBase
			/// </summary>
			/// <param name="left">Позиция по X</param>
			/// <param name="top">Позиция по Y</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <param name="parent">Родительский элемент</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusSpriteTileBase CreateTileBase(Single left, Single top, Single width, Single height, Transform parent = null)
			{
				// 1) Создание объекта
				LotusSpriteTileBase element = LotusSpriteDispatcher.CreateElement<LotusSpriteTileBase>("TileBase", left, top, width, height);

				// 2) Определение в иерархии
				element.SetParent(parent);

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mLayer;
			[SerializeField]
			internal Int32 mColumnIndex;
			[SerializeField]
			internal Int32 mRowIndex;
			[SerializeField]
			internal Int32 mSizeTileX = 1;
			[SerializeField]
			internal Int32 mSizeTileY = 1;
			[SerializeField]
			internal Boolean mOnPlacementGrid;
			[SerializeField]
			internal CTweenVector2D mTweenTranslate;
			[NonSerialized]
			internal LotusSpriteGridTileBase mOwnerGrid;

			// Данные по исполнению задачи
			internal Int32 mTaskOrder;

			// Параметры перетаскивания
			[NonSerialized]
			internal Boolean mIsDragging;
			[NonSerialized]
			internal Boolean mIsDownDragging;
			[NonSerialized]
			internal Vector2 mDragStartPosition;
			[NonSerialized]
			internal Vector2 mDragOffsetPosition;
			[NonSerialized]
			internal Boolean mIsMoveDirect = true;
			[NonSerialized]
			internal Boolean mIsOverOwner;
			[NonSerialized]
			internal Boolean mIsDraggEvent;

			// Параметры состояния
			[NonSerialized]
			internal Boolean mIsPoolObject;
			[NonSerialized]
			internal Boolean mIsUpdateColomunAndRowAfterTranslate;
			[NonSerialized]
			internal Int32 mColumnIndexPrev;
			[NonSerialized]
			internal Int32 mRowIndexPrev;

			// События логики
			internal Action<LotusSpriteTileBase> mOnTileClick;
			internal Action<LotusSpriteTileBase> mOnTileDown;
			internal Action<LotusSpriteTileBase, Vector2> mOnTileMove;
			internal Func<LotusSpriteTileBase, Vector2, Boolean> mOnTileSupportDrag;
			internal Func<LotusSpriteTileBase, TDirection2D, Boolean> mOnTileBeginDrag;
			internal Action<LotusSpriteTileBase> mOnTileDrag;
			internal Action<LotusSpriteTileBase, Int32, Int32> mOnTileEndDrag;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Слой расположения тайла
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
				get { return mColumnIndex;}
				set
				{
					mColumnIndex = value;
					if (mOnPlacementGrid)
					{
						this.SetColumnAndRow(mColumnIndex, mRowIndex);
					}
				}
			}

			/// <summary>
			/// Индекс строки
			/// </summary>
			public Int32 RowIndex
			{
				get { return mRowIndex; }
				set
				{
					mRowIndex = value;
					if (mOnPlacementGrid)
					{
						this.SetColumnAndRow(mColumnIndex, mRowIndex);
					}
				}
			}

			/// <summary>
			/// Индекс столбца и строки
			/// </summary>
			public Vector2Int ColumnAndRow
			{
				get { return new Vector2Int(mColumnIndex, mRowIndex); }
				set
				{
					mColumnIndex = value.x;
					mRowIndex = value.y;
					if (mOnPlacementGrid)
					{
						this.SetColumnAndRow(mColumnIndex, mRowIndex);
					}
				}
			}

			/// <summary>
			/// Размер тайла в ячейках по X (количество столбцов)
			/// </summary>
			/// <remarks>
			/// Тайл может занимать как одну ячейку так и несколько
			/// </remarks>
			public Int32 SizeTileX
			{
				get { return mSizeTileX; }
				set
				{
					mSizeTileX = value;
					if (mOnPlacementGrid)
					{
						this.SetColumnAndRow(mColumnIndex, mRowIndex);
					}
				}
			}

			/// <summary>
			/// Размер тайла в ячейках по Y (количество строк)
			/// </summary>
			/// <remarks>
			/// Тайл может занимать как одну ячейку так и несколько
			/// </remarks>
			public Int32 SizeTileY
			{
				get { return mSizeTileY; }
				set
				{
					mSizeTileY = value;
					if (mOnPlacementGrid)
					{
						this.SetColumnAndRow(mColumnIndex, mRowIndex);
					}
				}
			}

			/// <summary>
			/// Размер тайла в ячейках
			/// </summary>
			public Vector2Int SizeTile
			{
				get { return new Vector2Int(mSizeTileX, mSizeTileY); }
				set
				{
					mSizeTileX = value.x;
					mSizeTileY = value.y;
				}
			}

			/// <summary>
			/// Размещение тайла по сетки
			/// </summary>
			/// <remarks>
			/// Тайл может размещаться по сетки или располагаться в свободном месте
			/// </remarks>
			public Boolean OnPlacementGrid
			{
				get { return mOnPlacementGrid; }
				set
				{
					mOnPlacementGrid = value;
					if (mOnPlacementGrid)
					{
						this.SetColumnAndRow(mColumnIndex, mRowIndex);
					}
				}
			}

			/// <summary>
			/// Аниматор перемещения
			/// </summary>
			public CTweenVector2D TweenTranslate
			{
				get { return mTweenTranslate; }
			}

			/// <summary>
			/// Владелец элемента
			/// </summary>
			public LotusSpriteGridTileBase OwnerGrid
			{
				get { return mOwnerGrid; }
				set { mOwnerGrid = value; }
			}

			//
			// ПАРАМЕТРЫ СОСТОЯНИЯ
			//
			/// <summary>
			/// Пауза анимации
			/// </summary>
			public virtual Boolean IsPauseAnimation
			{
				get { return mTweenTranslate.IsPause; }
				set
				{
					mTweenTranslate.IsPause = value;
				}
			}

			/// <summary>
			/// Обновить значение строки и столбца после окончание перемещения 
			/// </summary>
			public Boolean IsUpdateColomunAndRowAfterTranslate
			{
				get { return mIsUpdateColomunAndRowAfterTranslate; }
				set
				{
					mIsUpdateColomunAndRowAfterTranslate = value;
				}
			}

			//
			// СОБЫТИЯ ЛОГИКИ
			//
			/// <summary>
			/// Событие для нотификации при щелчке на тайл. Аргумент - тайл
			/// </summary>
			public Action<LotusSpriteTileBase> OnTileClick
			{
				get { return mOnTileClick; }
				set
				{
					mOnTileClick = value;
				}
			}

			/// <summary>
			/// Событие для нотификации при нажатие на тайл. Аргумент - тайл
			/// </summary>
			public Action<LotusSpriteTileBase> OnTileDown
			{
				get { return mOnTileDown; }
				set
				{
					mOnTileDown = value;
				}
			}

			/// <summary>
			/// Событие для нотификации о перемещении тайла. Аргументы - тайл и позиция указателя
			/// </summary>
			/// <remarks>
			/// Только здесь нужно реализовывать перемещение тайла
			/// </remarks>
			public Action<LotusSpriteTileBase, Vector2> OnTileMove
			{
				get { return mOnTileMove; }
				set
				{
					mOnTileMove = value;
				}
			}

			/// <summary>
			/// Событие для нотификации о определении возможности переместить указанный тайл. Аргументы - тайл и позиция указатель
			/// </summary>
			/// <remarks>
			/// Метод должен определять принципиальную возможность переместить указанный тайл
			/// </remarks>
			public Func<LotusSpriteTileBase, Vector2, Boolean> OnTileSupportDrag
			{
				get { return mOnTileSupportDrag; }
				set
				{
					mOnTileSupportDrag = value;
				}
			}

			/// <summary>
			/// Событие для нотификации о начале перемещения указанного тайла. Аргументы - тайл и направление перемещения
			/// </summary>
			/// <remarks>
			/// Метод должен определять возможность переместить тайл по указанному направлению
			/// </remarks>
			public Func<LotusSpriteTileBase, TDirection2D, Boolean> OnTileBeginDrag
			{
				get { return mOnTileBeginDrag; }
				set
				{
					mOnTileBeginDrag = value;
				}
			}

			/// <summary>
			/// Событие для нотификации о перемещении указанного тайла. Аргумент - тайл
			/// </summary>
			public Action<LotusSpriteTileBase> OnTileDrag
			{
				get { return mOnTileDrag; }
				set
				{
					mOnTileDrag = value;
				}
			}

			/// <summary>
			/// Событие для нотификации об окончании перемещения указанного тайла. Аргументы - тайл, столбец и строка куда он опустился
			/// </summary>
			public Action<LotusSpriteTileBase, Int32, Int32> OnTileEndDrag
			{
				get { return mOnTileEndDrag; }
				set
				{
					mOnTileEndDrag = value;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusTask =======================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			/// <remarks>
			/// Свойство обязательное для реализации
			/// </remarks>
			public virtual Boolean IsTaskCompleted
			{
				get { return (mTweenTranslate.IsTaskCompleted); }
			}

			/// <summary>
			/// Порядок выполнения задачи
			/// </summary>
			/// <remarks>
			/// Обычно, выполнение задачи происходит в порядке их добавления.
			/// Однако задачи можно отсортировать по указанному критерию
			/// </remarks>
			public virtual Int32 TaskItemOrder
			{
				get { return mTaskOrder; }
				set { mTaskOrder = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusPoolObject =================================
			/// <summary>
			/// Статус объекта из пула
			/// </summary>
			/// <remarks>
			/// Позволяет определять был ли объект взят из пула и значит его надо вернуть или создан обычным образом
			/// </remarks>
			public Boolean IsPoolObject
			{
				get { return mIsPoolObject; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusDraggable ==================================
			/// <summary>
			/// Статус перетаскивания тайла
			/// </summary>
			public Boolean IsDragging
			{
				get { return mIsDragging; }
			}

			/// <summary>
			/// Статус нажатия на перетаскиваемый тайл
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

			/// <summary>
			/// Перемещение тайла осуществляется по движению указателя 
			/// </summary>
			/// <remarks>
			/// Тайл можно перемещать либо свободно ли используя внешнее событие <see cref="OnTileMove"/>
			/// </remarks>
			public Boolean IsMoveDirect
			{
				get { return mIsMoveDirect; }
				set { mIsMoveDirect = value; }
			}

			/// <summary>
			/// Возможность перетаскивания тайла в другую сетку
			/// </summary>
			public Boolean IsOverOwner
			{
				get { return mIsOverOwner; }
				set { mIsOverOwner = value; }
			}

			/// <summary>
			/// Посылать события о перемещение тайла сетки
			/// </summary>
			/// <remarks>
			/// В случае если установлен статус <see cref="IsOverOwner"/> это может сильно снижать производительность 
			/// так как каждый раз нужно искать на сетку на которой тайл перемещается 
			/// </remarks>
			public Boolean IsDraggEvent
			{
				get { return mIsDraggEvent; }
				set { mIsDraggEvent = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных базового тайла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void OnInitTileBase()
			{
				if (mTweenTranslate == null)
				{
					mTweenTranslate = new CTweenVector2D();
				}
				mTweenTranslate.Name = "Translate";
				mTweenTranslate.OnAnimationCompleted = AnimationTranslateCompletedTileInternal;
			}

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация элемента при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ResetElement()
			{
				OnInitTileBase();
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void ConstructorElement()
			{
				OnInitTileBase();
				XInputDispatcher.RegisterPointerDown(this);
				XInputDispatcher.RegisterPointerMove(this);
				XInputDispatcher.RegisterPointerUp(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public new void Update()
			{
#if UNITY_EDITOR
				OnUpdateInEditorMode();
#endif
				if (mTweenTranslate.IsPlay)
				{
					mTweenTranslate.UpdateAnimation();
					this.mThisTransform.position = new Vector3(mTweenTranslate.Value.x, mTweenTranslate.Value.y,
						this.mThisTransform.position.z);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void DestructorElement()
			{
				XInputDispatcher.UnRegisterPointerDown(this);
				XInputDispatcher.UnRegisterPointerMove(this);
				XInputDispatcher.UnRegisterPointerUp(this);
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов списка в зависимости от их видимости
			/// </summary>
			/// <param name="other">Элемент списка</param>
			/// <returns>Статус сравнения элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(LotusSpriteTileBase other)
			{
				if (this.IsVisibleElement == false)
				{
					if (other.IsVisibleElement == false)
					{
						return (Depth.CompareTo(other.Depth));
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
						return (1);
					}
					else
					{
						return (Depth.CompareTo(other.Depth));
					}
				}
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
				mTweenTranslate.StartAnimation();
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
				if (mTweenTranslate.IsPlay)
				{
					mTweenTranslate.UpdateAnimation();
					this.mThisTransform.position = mTweenTranslate.Value;
				}
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
				mTweenTranslate.StopAnimation();
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
				mTweenTranslate.StopAnimation();
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusPoolObject ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдо-конструктор
			/// </summary>
			/// <remarks>
			/// Вызывается диспетчером пула в момент взятия объекта из пула
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnPoolTake()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдо-деструктор
			/// </summary>
			/// <remarks>
			/// Вызывается диспетчером пула в момент попадания объекта в пул
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnPoolRelease()
			{

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
				if(ContainsScreen(XInputDispatcher.PositionPointer))
				{
					// Только если тайл доступен 
					if (mIsEnabled)
					{
						// Информируем о нажатие
						if (mOnTileDown != null)
							mOnTileDown(this);

						OnBeginDrag();
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
				OnDrag();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Событие отпускания
			/// </summary>
			/// <param name="pointer_id">Идентификатор указателя</param>
			//---------------------------------------------------------------------------------------------------------
			public void PointerUp(Int32 pointer_id)
			{
				if (mIsEnabled)
				{
					OnEndDrag();

					// Информируем о щелчке на тайле только если указатель находиться в его пределах
					if (mOnTileClick != null)
					{
						if (ContainsScreen(XInputDispatcher.PositionPointer))
						{
							mOnTileClick(this);
						}
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПЕРЕТАСКИВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на возможность перемещения тайла пользователем
			/// </summary>
			/// <returns>Статус</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsDraggStarting()
			{
				Single delta_x = Mathf.Abs(mDragStartPosition.x - XInputDispatcher.PositionPointer.x);
				Single delta_y = Mathf.Abs(mDragStartPosition.y - XInputDispatcher.PositionPointer.y);
				return ((delta_y > LotusSpriteDispatcher.DraggMinOffset.y) || (delta_x > LotusSpriteDispatcher.DraggMinOffset.x));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало перемещения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnBeginDrag()
			{
				// Если есть проверка на поддержку перемещения
				if (mOnTileSupportDrag != null)
				{
					if (mOnTileSupportDrag(this, XInputDispatcher.PositionPointer))
					{
						// Перемещение разрешено
						mIsDownDragging = true;
						mDragStartPosition = XInputDispatcher.PositionPointer;
					}
				}
				else
				{
					// Перемещаем по умолчанию
					mIsDownDragging = true;
					mDragStartPosition = XInputDispatcher.PositionPointer;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс перемещения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrag()
			{
				// Первый раз проверяем возможность переместить
				if (!mIsDragging)
				{
					if (mIsDownDragging)
					{
						// Проверка на перемещение
						if (IsDraggStarting())
						{
							// Начался реальный процесс перемещения
							mIsDragging = true;
							mDragOffsetPosition.x = LeftScreen - mDragStartPosition.x;
							mDragOffsetPosition.y = TopScreen - mDragStartPosition.y;

							Debug.Log("DragOffsetPosition = " + mDragOffsetPosition.ToString());

							TDirection2D dir = XInputDispatcher.GetDirectionFromInputClassic(XInputDispatcher.PositionPointer.x - mDragStartPosition.x,
								XInputDispatcher.PositionPointer.y - mDragStartPosition.y);

							// Вызываем логику тайла по началу перемещения
							if (mOnTileBeginDrag != null)
							{
								if (mOnTileBeginDrag(this, dir))
								{
									// Размещаем поверх
									this.SetAsLastSibling();

									// Информируем сетку
									if (mOwnerGrid != null)
									{
										mOwnerGrid.mCurrentDraggTile = this;
									}
								}
								else
								{
									mIsDragging = false;
									mIsDownDragging = false;
									return;
								}
							}
							else
							{
								// Размещаем поверх
								this.SetAsLastSibling();
							}
						}
					}
				}

				// Если перемещение
				if (mIsDragging)
				{
					// Перемещаем тайл
					if (mIsMoveDirect)
					{
						LocationScreen = XInputDispatcher.PositionPointer + mDragOffsetPosition;
					}
					else
					{
						// Перемещаем тайл конкретной логикой
						if (mOnTileMove != null)
						{
							mOnTileMove(this, XInputDispatcher.PositionPointer);
						}
					}

					// Если нам надо генерировать событие - перемещение тайла
					if (mIsDraggEvent)
					{
						// Вызываем логику перемещения тайла
						if (mOnTileDrag != null)
						{
							mOnTileDrag(this);
						}

						// Если мы можем перемеситься в другую сетку
						if (mIsOverOwner)
						{
							LotusSpriteGridTileBase owner_grid =
								LotusSpriteDispatcher.FindElementFromScreenPosition<LotusSpriteGridTileBase>(XInputDispatcher.PositionPointer);
							if (owner_grid != null)
							{
								owner_grid.OnTileDragExternal(this);
							}
						}
						else
						{
							if (mOwnerGrid != null) mOwnerGrid.OnTileDragExternal(this);
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание перемещения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEndDrag()
			{
				mIsDownDragging = false;

				// Если есть еще перемещение
				if (mIsDragging)
				{
					mIsDragging = false;

					LotusSpriteGridTileBase current_grid = mOwnerGrid;
					if (mIsOverOwner)
					{
						// Если мы можем перемеситься в другую сетку
						// Ищем сетку куда попал центр тайла
						current_grid = LotusSpriteDispatcher.FindElementFromScreenPosition<LotusSpriteGridTileBase>(this.LocationScreen);
					}

					// Выходим если нет
					if (current_grid == null) return;

					// 1) Считаем позицию куда-мы попали
					Vector2 original_pos = new Vector2(mThisTransform.position.x, mThisTransform.position.y);
					Vector2Int position = current_grid.GetColumnAndRowFromWorldTransform(original_pos);

					Int32 column = position.x;
					Int32 row = position.y;

					// 2) Вызываем логику окончания перемещения тайла
					if (mOnTileEndDrag != null)
					{
						mOnTileEndDrag(this, column, row);
					}

					// 3) Информируем сетку
					if (mIsOverOwner)
					{
						current_grid.OnTileEndDragExternal(this, column, row);
					}

					if (mOwnerGrid != null)
					{
						mOwnerGrid.mPreviousDraggTile = mOwnerGrid.mCurrentDraggTile;
						mOwnerGrid.mCurrentDraggTile = null;
					}
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса столбца (позиции по X в сетки) на основе позиции тайла
			/// </summary>
			/// <returns>Индекс столбца (позиции по X в сетки)</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetColumnIndex()
			{
				return GetColumnAndRow().x;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса строки (позиции по Y в сетки) на основе позиции тайла
			/// </summary>
			/// <returns>Индекс строки (позиции по Y в сетки)</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetRowIndex()
			{
				return GetColumnAndRow().y;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции тайла в сетки на основе позиции тайла
			/// </summary>
			/// <returns>Позиция в сетки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2Int GetColumnAndRow()
			{
				Vector2 pos = new Vector2(mThisTransform.position.x, mThisTransform.position.y);
				return mOwnerGrid.GetColumnAndRowFromWorldTransform(pos);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка координат тайла по определенной позиции в сетки
			/// </summary>
			/// <param name="column">Индекс столбца (позиции по X в сетки)</param>
			/// <param name="row">Индекс строки (позиции по Y в сетки)</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetColumnAndRow(Int32 column, Int32 row)
			{
#if UNITY_EDITOR
				if (UnityEditor.PrefabUtility.GetPrefabInstanceStatus(this.gameObject) != UnityEditor.PrefabInstanceStatus.NotAPrefab)
				{
					return;
				}
#endif
				if (mOwnerGrid == null) return;

				Vector2 pos = Vector2.zero;
				mColumnIndex = column;
				mRowIndex = row;
				pos = mOwnerGrid.GetLocalDesignTile(mColumnIndex, mRowIndex);

				this.Left = pos.x;
				this.Top = pos.y;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка тайла по сетки с указанной позицией и размерами
			/// </summary>
			/// <param name="column">Индекс столбца (позиции по X в сетки)</param>
			/// <param name="row">Индекс строки (позиции по Y в сетки)</param>
			/// <param name="width">Ширина тайла</param>
			/// <param name="heigth">Высота тайла</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetPlacementGrid(Int32 column, Int32 row, Single width, Single heigth)
			{
				mOnPlacementGrid = true;
				SetColumnAndRow(column, row);
				Width = width;
				Height = heigth;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление значений столбца и строки по позиции тайла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateFromPosition()
			{
				mColumnIndex = this.GetColumnIndex();
				mRowIndex = this.GetRowIndex();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление позиции тайла по значению столбца и строки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateFromColumnAndRow()
			{
				SetColumnAndRow(mColumnIndex, mRowIndex);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохраннее текущей позиции столбца и строки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SaveColumnAndRow()
			{
				mColumnIndexPrev = mColumnIndex;
				mRowIndexPrev = mRowIndex;
			}
			#endregion

			#region ======================================= МЕТОДЫ АНИМИРОВАНИЯ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение тайла в заданную позицию в сетки
			/// </summary>
			/// <param name="dest_column">Индекс столбца</param>
			/// <param name="dest_row">Индекс строки</param>
			//---------------------------------------------------------------------------------------------------------
			public void AnimationTranslate(Int32 dest_column, Int32 dest_row)
			{
				if (mTweenTranslate.IsPlay == false)
				{
					mTweenTranslate.StartValue = mThisTransform.position;
					mTweenTranslate.TargetValue = mOwnerGrid.GetWorldTransformTile(dest_column, dest_row);
					mTweenTranslate.StartAnimation();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание анимации перемещения тайла
			/// </summary>
			/// <param name="animation_name">Имя анимации</param>
			//---------------------------------------------------------------------------------------------------------
			protected void AnimationTranslateCompletedTileInternal(String animation_name)
			{
				if (mIsUpdateColomunAndRowAfterTranslate)
				{
					this.UpdateFromPosition();
				}

				if (mOwnerGrid != null)
					mOwnerGrid.OnTileAnimationTranslateCompleted(animation_name, this);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================