//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Подсистема игровых экранов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusScreenGame.cs
*		Компонент представляющий игровой экран.
*		Реализация компонента представляющий собой игровой экран. Игровой экран интерфейса пользователя – набор
*	взаимосвязанных элементов UI располагаемых на экране. Игровой экран это логическая концепция, непосредственное
*	отображения осуществляет соответствующая технология.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;
using UnityEngine;
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
		//! \addtogroup Unity2DCommonScreenGame
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент представляющий игровой экран
		/// </summary>
		/// <remarks>
		/// Реализация компонента представляющий собой игровой экран.
		/// Игровой экран интерфейса пользователя – набор взаимосвязанных элементов UI располагаемых на экране.
		/// Игровой экран это логическая концепция, непосредственное отображения осуществляет соответствующая технология.
		/// Поддерживается технология отображения через компонентную оболочку IMGUI Unity и компонентов Unity UI
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("Lotus/Graphics2D/Screen Game/Screen Game")]
		public class LotusScreenGame : MonoBehaviour, IComparable<LotusScreenGame>
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание игрового экрана
			/// </summary>
			/// <returns>Созданный игровой экран</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusScreenGame CreateScreenGame()
			{
				// 1) Создание объекта
				GameObject go = new GameObject("ScreenGame");
				go.layer = XLayer.UI_ID;
				LotusScreenGame screen_game = go.AddComponent<LotusScreenGame>();

				return screen_game;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Int32 mNumberWindow;
			[SerializeField]
			internal Int32 mGroupID;
			[SerializeField]
			internal Boolean mIsDeactivatedChild;
			[SerializeField]
			internal Boolean mIsCorrectPosition;
			[SerializeField]
			internal Vector2 mInvisiblePosStart;
			[SerializeField]
			internal Vector2 mVisiblePos;
			[SerializeField]
			internal Vector2 mInvisiblePosNext;
			[SerializeField]
			internal TUIScreenGameViewType mViewType;
			[SerializeField]
			internal TUIScreenGameModeBehavior mModeBehavior;
			[SerializeField]
			internal Button mButtonActivate;
			[SerializeField]
			internal CTweenVector2D mTweenAnimator;
			[NonSerialized]
			internal ILotusScreenGameVisual mViewElement;
			[NonSerialized]
			internal LotusScreenGameDispatcher mOwner;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedScreenGame;
			[SerializeField]
			internal Boolean mExpandedTween;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Номер окна
			/// </summary>
			public Int32 NumberWindow
			{
				get { return mNumberWindow; }
				set { mNumberWindow = value; }
			}

			/// <summary>
			/// Идентификатор группы которому принадлежит данное окно
			/// </summary>
			public Int32 GroupID
			{
				get { return mGroupID; }
				set { mGroupID = value; }
			}

			/// <summary>
			/// Деактивация/активация дочерних объектов вместе с игровым экраном
			/// </summary>
			public Boolean IsDeactivatedChild
			{
				get { return mIsDeactivatedChild; }
				set { mIsDeactivatedChild = value; }
			}

			/// <summary>
			/// Корректировка позиции игрового экрана при смене разрешения экрана
			/// </summary>
			public Boolean IsCorrectPosition
			{
				get { return mIsCorrectPosition; }
				set { mIsCorrectPosition = value; }
			}

			/// <summary>
			/// Позиция при невидимости экрана в начале
			/// </summary>
			public Vector2 InvisiblePosStart
			{
				get { return mInvisiblePosStart; }
				set { mInvisiblePosStart = value; }
			}

			/// <summary>
			/// Целевая позиция видимого экрана
			/// </summary>
			public Vector2 VisiblePos
			{
				get { return mVisiblePos; }
				set { mVisiblePos = value; }
			}

			/// <summary>
			/// Позиция при невидимости экрана в конце (после переключения)
			/// </summary>
			public Vector2 InvisiblePosNext
			{
				get { return mInvisiblePosNext; }
				set { mInvisiblePosNext = value; }
			}

			/// <summary>
			/// Тип появления/скрытия игрового экрана (для активного экрана)
			/// </summary>
			public TUIScreenGameViewType ViewType
			{
				get { return mViewType; }
				set { mViewType = value; }
			}

			/// <summary>
			/// Режим переключение игровых экранов
			/// </summary>
			public TUIScreenGameModeBehavior ModeBehavior
			{
				get { return mModeBehavior; }
				set { mModeBehavior = value; }
			}

			/// <summary>
			/// Активатор игрового экрана
			/// </summary>
			public Button ButtonActivate
			{
				get { return mButtonActivate; }
				set { mButtonActivate = value; }
			}

			/// <summary>
			/// Аниматор для перемещения игрового экрана
			/// </summary>
			public CTweenVector2D TweenAnimator
			{
				get { return mTweenAnimator; }
			}

			//
			// ПАРАМЕТРЫ ОТОБРАЖЕНИЯ
			//
			/// <summary>
			/// Визуальный элемент который непосредственно реализует отображение игрового экрана
			/// </summary>
			public ILotusScreenGameVisual ViewElement
			{
				get
				{
#if UNITY_EDITOR
					if (mViewElement == null)
					{
						mViewElement = GetComponent<ILotusScreenGameVisual>();
					}
					return mViewElement;
#else
					return (mViewElement);
#endif
				}
				set
				{
					mViewElement = value;
				}
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				OnInit();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{
				mTweenAnimator.Name = name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
				mTweenAnimator.UpdateAnimation();
				if (mTweenAnimator.IsPlay)
				{
					ViewElement.SetScreenGamePosition(mTweenAnimator.Value);
				}
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнения игровых экранов по номеру
			/// </summary>
			/// <param name="other">Игровой экран</param>
			/// <returns>Статус сравнения игровых экранов по номеру</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(LotusScreenGame other)
			{
				return (mNumberWindow.CompareTo(other.NumberWindow));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return name;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная безопасная инициализация данных игрового экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnInit()
			{
				mViewElement = GetComponent<ILotusScreenGameVisual>();

				if(mButtonActivate != null)
				{
					mButtonActivate.onClick.AddListener(Activate);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Активация игрового экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Activate()
			{
				if(mOwner != null)
				{
					mOwner.SwapForwardScreen(this);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Активация игрового экрана
			/// </summary>
			/// <param name="switch_mode">Режим переключение игровых экранов</param>
			//---------------------------------------------------------------------------------------------------------
			public void Activate(TUIScreenGameModeBehavior switch_mode)
			{
				switch (mViewType)
				{
					case TUIScreenGameViewType.Swap:
						{
							// 1) Ставим в изначально невидимую позицию
							ViewElement.SetScreenGamePosition(mInvisiblePosStart);
							ViewElement.SetScreenGameDepthOffset(500);

							// 2) Экран будет активироваться
							OnActivating();
							if (mOwner.mOnActivating != null) mOwner.mOnActivating(name);

							// 3) Активируем
							ViewElement.SetScreenGameVisible(true);
							ViewElement.SetScreenGamePosition(mVisiblePos);

							// 4) Экран активировался
							OnActivated();
							if (mOwner.mOnActivated != null) mOwner.mOnActivated(name);

							// 5) Информируем диспетчер
							mOwner.OnScreenActivated(this);

							if(switch_mode == TUIScreenGameModeBehavior.Switch) ViewElement.SetScreenGameDepthOffset(-500);
						}
						break;
					case TUIScreenGameViewType.Move:
						{
							// 1) Ставим в изначально невидимую позицию
							ViewElement.SetScreenGamePosition(mInvisiblePosStart);
							ViewElement.SetScreenGameDepthOffset(1000);

							// 2) Экран будет активироваться
							OnActivating();
							if (mOwner.mOnActivating != null) mOwner.mOnActivating(name);

							// 3) Активируем
							ViewElement.SetScreenGameVisible(true);
							mTweenAnimator.StartValue = mInvisiblePosStart;
							mTweenAnimator.TargetValue = mVisiblePos;
							mTweenAnimator.StartAnimation();
							mTweenAnimator.OnAnimationCompleted = (String screen_name) =>
							{
								// 4) Экран активировался
								OnActivated();
								if (mOwner.mOnActivated != null) mOwner.mOnActivated(name);

								// 5) Информируем диспетчер
								mOwner.OnScreenActivated(this);
								if (switch_mode == TUIScreenGameModeBehavior.Switch) ViewElement.SetScreenGameDepthOffset(-500);
							};
						}
						break;
					case TUIScreenGameViewType.Fade:
						{
							// 1) Ставим в изначально видимую позицию
							ViewElement.SetScreenGamePosition(mVisiblePos);
							ViewElement.SetScreenGameVisible(true);
							ViewElement.SetScreenGameDepthOffset(1000);

							// 2) Экран будет активироваться
							OnActivating();
							if (mOwner.mOnActivating != null) mOwner.mOnActivating(name);

							// 3) Активируем
							StartCoroutine(ShowScreenGameCoroutine(() =>
							{
								// 4) Экран активировался
								OnActivated();
								if (mOwner.mOnActivated != null) mOwner.mOnActivated(name);

								// 5) Информируем диспетчер
								mOwner.OnScreenActivated(this);
								if (switch_mode == TUIScreenGameModeBehavior.Switch) ViewElement.SetScreenGameDepthOffset(-500);
							}));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деактивация игрового экрана
			/// </summary>
			/// <param name="switch_mode">Режим переключение игровых экранов</param>
			//---------------------------------------------------------------------------------------------------------
			public void Deactivate(TUIScreenGameModeBehavior switch_mode)
			{
				switch (mViewType)
				{
					case TUIScreenGameViewType.Swap:
						{
							// 1) Ставим в изначально видимую позицию
							ViewElement.SetScreenGamePosition(mVisiblePos);

							// 2) Экран будет деактивироваться
							OnDeactivating();
							if (mOwner.mOnDeactivating != null) mOwner.mOnDeactivating(name);

							// 3) Деактивируем
							ViewElement.SetScreenGameVisible(false);
							ViewElement.SetScreenGamePosition(mInvisiblePosNext);

							// 4) Экран деактивировался
							OnDeactivated();
							if (mOwner.mOnDeactivated != null) mOwner.mOnDeactivated(name);

							// 5) Информируем диспетчер
							mOwner.OnScreenDeactivated(this);
							if (switch_mode == TUIScreenGameModeBehavior.Remove) ViewElement.SetScreenGameDepthOffset(-500);
						}
						break;
					case TUIScreenGameViewType.Move:
						{
							// 1) Ставим в изначально видимую позицию
							ViewElement.SetScreenGamePosition(mVisiblePos);

							// 2) Экран будет деактивироваться
							OnDeactivating();
							if (mOwner.mOnDeactivating != null) mOwner.mOnDeactivating(name);

							// 3) Деактивируем
							mTweenAnimator.StartValue = mVisiblePos;
							mTweenAnimator.TargetValue = mInvisiblePosNext;
							mTweenAnimator.StartAnimation();
							mTweenAnimator.OnAnimationCompleted = (String screen_name) =>
							{
								// 4) Экран деактивировался
								ViewElement.SetScreenGameVisible(false);
								OnDeactivated();
								if (mOwner.mOnDeactivated != null) mOwner.mOnDeactivated(name);

								// 5) Информируем диспетчер
								mOwner.OnScreenDeactivated(this);
								if (switch_mode == TUIScreenGameModeBehavior.Remove) ViewElement.SetScreenGameDepthOffset(-500);
							};
						}
						break;
					case TUIScreenGameViewType.Fade:
						{
							// 1) Ставим в изначально видимую позицию
							ViewElement.SetScreenGamePosition(mVisiblePos);

							// 2) Экран будет активироваться
							OnDeactivating();
							if (mOwner.mOnDeactivating != null) mOwner.mOnDeactivating(name);

							// 3) Деактивируем
							StartCoroutine(HideScreenGameCoroutine(() =>
							{
								// 4) Экран деактивировался
								ViewElement.SetScreenGameVisible(false);
								OnDeactivated();
								if (mOwner.mOnDeactivated != null) mOwner.mOnDeactivated(name);

								// 5) Информируем диспетчер
								mOwner.OnScreenDeactivated(this);
								if (switch_mode == TUIScreenGameModeBehavior.Remove) ViewElement.SetScreenGameDepthOffset(-500);
							}));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка игрового экрана в видимую позицию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetToVisiblePos()
			{
				ViewElement.SetScreenGamePosition(mVisiblePos);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка игрового экрана в невидимую позицию в начале
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetToInvisiblePosStart()
			{
				ViewElement.SetScreenGamePosition(mInvisiblePosStart);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка игрового экрана в невидимую позицию в конце
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SetToInvisiblePosNext()
			{
				ViewElement.SetScreenGamePosition(mInvisiblePosNext);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка смещения глубины показа(порядка рисования) игрового экрана
			/// </summary>
			/// <param name="depth_offset">Смещение порядка рисования</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetDepthOffset(Int32 depth_offset)
			{
				ViewElement.SetScreenGameDepthOffset(depth_offset);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Корректировка позиции игрового окна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void CorrectPosition()
			{
				if (mIsCorrectPosition)
				{
					mInvisiblePosNext.x *= LotusSystemDispatcher.ScaledScreenX;
					mInvisiblePosNext.y *= LotusSystemDispatcher.ScaledScreenY;
					mInvisiblePosStart.x *= LotusSystemDispatcher.ScaledScreenX;
					mInvisiblePosStart.y *= LotusSystemDispatcher.ScaledScreenY;
					mVisiblePos.x *= LotusSystemDispatcher.ScaledScreenX;
					mVisiblePos.y *= LotusSystemDispatcher.ScaledScreenY;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОКАЗА/СКРЫТИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма скрытия игрового экрана
			/// </summary>
			/// <param name="on_completed">Обработчик события окончания скрытия игрового экрана</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator HideScreenGameCoroutine(Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / mTweenAnimator.Duration;

					ViewElement.SetScreenGameOpacity(Mathf.Lerp(1, 0, time));
					yield return null;
				}

				ViewElement.SetScreenGameOpacity(0);
				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма показа игрового экрана
			/// </summary>
			/// <param name="on_completed">Обработчик события окончания показа игрового экрана</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			protected IEnumerator ShowScreenGameCoroutine(Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				//mOpacity = 0;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / mTweenAnimator.Duration;

					ViewElement.SetScreenGameOpacity(Mathf.Lerp(0, 1, time));
					yield return null;
				}

				ViewElement.SetScreenGameOpacity(1);
				on_completed();
			}
			#endregion

			#region ======================================= ОБРАБОТКА СОБЫТИЙ =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало активации игрового экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnActivating()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание активации игрового экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnActivated()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало деактивации игрового экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnDeactivating()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание деактивации игрового экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnDeactivated()
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