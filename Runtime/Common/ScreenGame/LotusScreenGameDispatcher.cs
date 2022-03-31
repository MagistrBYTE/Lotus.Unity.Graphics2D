//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Подсистема игровых экранов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusScreenGameDispatcher.cs
*		Диспетчер управления игровыми экранами.
*		Реализация диспетчера основной логики по управлению игровыми экранами.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
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
		/// Диспетчер управления игровыми экранами
		/// </summary>
		/// <remarks>
		/// Реализация диспетчера основной логики по управлению игровыми экранами
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("Lotus/Graphics2D/Screen Game/Screen Game Dispatcher")]
		public class LotusScreenGameDispatcher : MonoBehaviour
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание диспетчера управления игровыми экранами
			/// </summary>
			/// <returns>Диспетчер управления игровыми экранами</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LotusScreenGameDispatcher CreateScreenGameDispatcher()
			{
				// 1) Создание объекта
				GameObject go = new GameObject("ScreenGameDispatcher");
				go.layer = XLayer.UI_ID;
				LotusScreenGameDispatcher screen_game_dispatcher = go.AddComponent<LotusScreenGameDispatcher>();

				return screen_game_dispatcher;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsEnabledUIScreen = false;
			[SerializeField]
			internal Boolean mIsStartFirstScreen = false;
			[SerializeField]
			internal Int32 mGroupID;
			[SerializeField]
			internal LotusScreenGame mStartUIScreen;
			[NonSerialized]
			internal List<LotusScreenGame> mUIScreens;
			[NonSerialized]
			internal LotusScreenGame mCurrentUIScreen;
			[NonSerialized]
			internal LotusScreenGame mPrevoisUIScreen;
			[NonSerialized]
			internal LotusScreenGame mNextUIScreen;
			[NonSerialized]
			internal Boolean mSwapProccesBack;		// Действия назад
			[NonSerialized]
			internal Boolean mSwapProccesLock;		// Блокировка для однократной смены экрана
			[NonSerialized]
			internal List<LotusScreenGame> mNavigateUIScreens;
			[NonSerialized]
			internal Int32 mNavigateIndex;

			// События
			[NonSerialized]
			internal Action<String> mOnActivating;
			[NonSerialized]
			internal Action<String> mOnActivated;
			[NonSerialized]
			internal Action<String> mOnDeactivating;
			[NonSerialized]
			internal Action<String> mOnDeactivated;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedScreenParam;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Доступность подсистемы игровых экранов
			/// </summary>
			public Boolean IsEnabledUIScreen
			{
				get { return mIsEnabledUIScreen; }
				set { mIsEnabledUIScreen = value; }
			}

			/// <summary>
			/// Активация при старте 1 экрана
			/// </summary>
			public Boolean IsStartFirstScreen
			{
				get { return mIsStartFirstScreen; }
				set { mIsStartFirstScreen = value; }
			}

			/// <summary>
			/// Идентификатор группы которой управляет данный диспетчер
			/// </summary>
			public Int32 GroupID
			{
				get { return mGroupID; }
				set { mGroupID = value; }
			}

			/// <summary>
			/// Стартовый игровой экран
			/// </summary>
			public LotusScreenGame StartUIScreen
			{
				get { return mStartUIScreen; }
			}

			/// <summary>
			/// Текущий игровой экран
			/// </summary>
			public LotusScreenGame CurrentUIScreen
			{
				get { return mCurrentUIScreen; }
			}

			/// <summary>
			/// Игровые экраны
			/// </summary>
			public List<LotusScreenGame> UIScreens
			{
				get { return mUIScreens; }
			}

			/// <summary>
			/// История навигации игровых экранов
			/// </summary>
			public List<LotusScreenGame> NavigateUIScreens
			{
				get { return mNavigateUIScreens; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации начала активации игрового экрана. Аргумент - имя экрана
			/// </summary>
			public Action<String> OnActivating
			{
				get { return mOnActivating; }
				set { mOnActivating = value; }
			}

			/// <summary>
			/// Событие для нотификации окончания активации игрового экрана. Аргумент - имя экрана
			/// </summary>
			public Action<String> OnActivated
			{
				get { return mOnActivated; }
				set { mOnActivated = value; }
			}

			/// <summary>
			/// Событие для нотификации начала деактивации игрового экрана. Аргумент - имя экрана
			/// </summary>
			public Action<String> OnDeactivating
			{
				get { return mOnDeactivating; }
				set { mOnDeactivating = value; }
			}

			/// <summary>
			/// Событие для нотификации окончания деактивации игрового экрана. Аргумент - имя экрана
			/// </summary>
			public Action<String> OnDeactivated
			{
				get { return mOnDeactivated; }
				set { mOnDeactivated = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация элемента при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void ResetElement()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
				this.ResetElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void ConstructorElement()
			{
				InitScreen();
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

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация игровых экранов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void InitScreen()
			{
				if (mIsEnabledUIScreen)
				{
					// 1) Первый это всегда стартовый экран
					if (mIsStartFirstScreen)
					{
						mCurrentUIScreen = mStartUIScreen;
						mCurrentUIScreen.OnInit();
						mCurrentUIScreen.SetToVisiblePos();
					}

					// 2) Получаем все игровые экраны
					mUIScreens = new List<LotusScreenGame>();
					List<LotusScreenGame> screens = XComponentDispatcher.GetAll<LotusScreenGame>();

					// 3) Берем только экраны свой группы
					for (Int32 i = 0; i < screens.Count; i++)
					{
						if(screens[i].GroupID == GroupID)
						{
							screens[i].OnInit();
							mUIScreens.Add(screens[i]);
						}
					}

					// 4) Сортируем по номерам
					mUIScreens.Sort();

					// 5) Ставим экраны в позиции
					for (Int32 i = 0; i < mUIScreens.Count; i++)
					{
						// Присоединяем диспетчер
						mUIScreens[i].mOwner = this;

						// Корректируем позицию
						mUIScreens[i].CorrectPosition();

						// Для не стартового экрана переводим в невидимую позицию
						if (mUIScreens[i] != mStartUIScreen)
						{
							mUIScreens[i].SetToInvisiblePosStart();
						}
					}

					// 6) Создаем навигацию
					mNavigateUIScreens = new List<LotusScreenGame>();
					mNavigateUIScreens.Add(mCurrentUIScreen);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сортировка игровых экранов по номерам
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SortScreen()
			{
				mUIScreens.Sort();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Активация смены (последовательно вперед) игровых экранов
			/// </summary>
			/// <param name="screen_name">Имя экрана который должен появится</param>
			//---------------------------------------------------------------------------------------------------------
			public void SwapForwardScreen(String screen_name)
			{
				for (Int32 i = 0; i < mUIScreens.Count; i++)
				{
					// 1) Смотрим какой экран активирует данный элемент
					if (mUIScreens[i].name == screen_name)
					{
						SwapForwardScreen(mUIScreens[i]);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Активация смены (последовательно вперед) игровых экранов
			/// </summary>
			/// <param name="next_screen">Следующий игровой экран</param>
			//---------------------------------------------------------------------------------------------------------
			public void SwapForwardScreen(LotusScreenGame next_screen)
			{
				if (mCurrentUIScreen == next_screen)
				{
					return;
				}

				// 1) Включаем процесс
				mSwapProccesLock = false;
				mSwapProccesBack = false;

				if (next_screen.ViewType != TUIScreenGameViewType.Swap)
				{
					mNextUIScreen = next_screen;
				}

				LotusScreenGame current_screen = mCurrentUIScreen;

				// 2) Активируем
				next_screen.Activate(TUIScreenGameModeBehavior.Switch);

				// 3) Текущий экран деактивируем
				if (current_screen != null)
				{
					current_screen.Deactivate(TUIScreenGameModeBehavior.Switch);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возврат назад - активация смены (последовательно назад) игровых экранов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SwapBackwardScreen()
			{
				LotusScreenGame prev_screen = null;

				// 1) Если мы не на первом экране
				if (mNavigateUIScreens.Count > 1)
				{
					prev_screen = mNavigateUIScreens[mNavigateUIScreens.Count - 2];
				}
				else
				{
					if (mPrevoisUIScreen != null)
					{
						prev_screen = mPrevoisUIScreen;
					}
				}
				
				if (prev_screen == null) return;

				SwapBackwardScreen(prev_screen);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возврат назад - активация смены (последовательно назад) игровых экранов
			/// </summary>
			/// <param name="prev_screen">Предыдущий игровой экран</param>
			//---------------------------------------------------------------------------------------------------------
			public void SwapBackwardScreen(LotusScreenGame prev_screen)
			{
				mNextUIScreen = prev_screen;

				// 1) Включаем процесс
				mSwapProccesLock = false;
				mSwapProccesBack = true;

				// 2) Текущий экран деактивируем
				if (mCurrentUIScreen != null)
				{
					mCurrentUIScreen.Deactivate(TUIScreenGameModeBehavior.Switch);
				}

				// 3) Активируем
				mNextUIScreen.Activate(TUIScreenGameModeBehavior.Switch);

				// 4) Удаляем последний игровой экран в навигации
				mNavigateUIScreens.RemoveAt(mNavigateUIScreens.Count - 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конец активации игрового экрана
			/// </summary>
			/// <param name="screen_game">Игровой экран</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnScreenActivated(LotusScreenGame screen_game)
			{
				// 1) Если блокировка не включена
				if (mSwapProccesLock == false)
				{
					// 2) Меняем экраны
					mPrevoisUIScreen = mCurrentUIScreen;
					mCurrentUIScreen = screen_game;

					// 3) Блокировка
					mSwapProccesLock = true;
				}

				// 4) Добавляем в навигацию
				if (!mSwapProccesBack) mNavigateUIScreens.Add(screen_game);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конец деактивации игрового экрана
			/// </summary>
			/// <param name="screen_game">Игровой экран</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnScreenDeactivated(LotusScreenGame screen_game)
			{
				// 1) Если блокировка не включена
				if (mSwapProccesLock == false)
				{
					// 2) Меняем экраны
					mPrevoisUIScreen = mCurrentUIScreen;
					mCurrentUIScreen = mNextUIScreen;

					// 3) Блокировка
					mSwapProccesLock = true;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Активация следующего по списку игрового экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void NextScreen()
			{
				if(mCurrentUIScreen == null) return;

				// 1) Ищем индекс текущего экрана
				for (Int32 i = 0; i < mUIScreens.Count; i++)
				{
					if(mUIScreens[i] == mCurrentUIScreen)
					{
						// 2) Если он не предпоследний
						if(i != mUIScreens.Count - 1)
						{
							SwapForwardScreen(mUIScreens[i + 1]);
							break;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Активация предыдущего по списку игрового экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void PrevoisScreen()
			{
				// Проверка на последний экран
				if (mCurrentUIScreen == null)
				{
					if(mNavigateUIScreens.Count > 1)
					{
						mCurrentUIScreen = mNavigateUIScreens[mNavigateUIScreens.Count - 1];
					}
					else
					{
						return;
					}
				}

				// 1) Ищем индекс текущего экрана
				for (Int32 i = 0; i < mUIScreens.Count; i++)
				{
					if (mUIScreens[i] == mCurrentUIScreen)
					{
						// 2) Если он не предпоследний
						if (i != 0)
						{
							SwapBackwardScreen(mUIScreens[i - 1]);
							break;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление игрового экрана
			/// </summary>
			/// <param name="screen_name">Имя игрового экрана</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddScreen(String screen_name)
			{
				for (Int32 i = 0; i < mUIScreens.Count; i++)
				{
					if (mUIScreens[i].name == screen_name)
					{
						AddScreen(mUIScreens[i]);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление игрового экрана
			/// </summary>
			/// <param name="add_screen">Добавляемый игровой экран</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddScreen(LotusScreenGame add_screen)
			{
				if (mCurrentUIScreen == add_screen) return;

				// 1) Включаем процесс
				mSwapProccesLock = false;
				mSwapProccesBack = false;

				// 2) Текущего экрана нет
				mCurrentUIScreen = null;

				// 3) Неактивный экран активируем
				mNextUIScreen = add_screen;
				mNextUIScreen.Activate(TUIScreenGameModeBehavior.Add);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление игрового экрана
			/// </summary>
			/// <param name="screen_name">Имя игрового экрана</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveScreen(String screen_name)
			{
				for (Int32 i = 0; i < mUIScreens.Count; i++)
				{
					if (mUIScreens[i].name == screen_name)
					{
						RemoveScreen(mUIScreens[i]);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление игрового экрана
			/// </summary>
			/// <param name="current_screen">Текущий игровой экран</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveScreen(LotusScreenGame current_screen)
			{
				// 1) Включаем процесс
				mSwapProccesLock = false;
				mSwapProccesBack = true;

				// 2) Следующего экрана нет
				mNextUIScreen = null;

				// 3) Текущий экран убираем
				mCurrentUIScreen = current_screen;
				mCurrentUIScreen.Deactivate(TUIScreenGameModeBehavior.Remove);

				// 4) Удаляем экран из навигации
				mNavigateUIScreens.Remove(current_screen);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление текущего игрового экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveScreen()
			{
				if (mCurrentUIScreen != null)
				{
					// 1) Включаем процесс
					mSwapProccesLock = false;
					mSwapProccesBack = true;

					// 2) Следующего экрана нет
					mNextUIScreen = null;

					// 3) Текущий экран деактивируем
					mCurrentUIScreen.Deactivate(TUIScreenGameModeBehavior.Remove);

					// 4) Удаляем последний экран из навигации
					mNavigateUIScreens.RemoveAt(mNavigateUIScreens.Count - 1);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление текущего игрового экрана и установка в качестве активного предыдущего экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveScreenAndLastNext()
			{
				if (mCurrentUIScreen != null)
				{
					// 1) Включаем процесс
					mSwapProccesLock = false;
					mSwapProccesBack = true;

					// 2) Следующего экрана нет
					// т.е. не нет а он будут тот который был до этого
					mNextUIScreen = mNavigateUIScreens[mNavigateUIScreens.Count - 2];

					// 3) Текущий экран деактивируем
					mCurrentUIScreen.Deactivate(TUIScreenGameModeBehavior.Remove);

					// 4) Удаляем последний экран из навигации
					mNavigateUIScreens.RemoveAt(mNavigateUIScreens.Count - 1);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ НАВИГАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка навигации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ClearNavigation()
			{
				mNavigateUIScreens.Clear();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление навигации до указанного индекса игрового экрана
			/// </summary>
			/// <param name="index">Индекс до которого удалится навигация</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveNavigation(Int32 index)
			{
				Int32 count = mNavigateUIScreens.Count - index;
				for (Int32 i = 0; i < count; i++)
				{
					mNavigateUIScreens.RemoveAt(mNavigateUIScreens.Count - 1);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление навигации до указанного имени игрового экрана
			/// </summary>
			/// <param name="screen_name">Имя игрового экрана до которого удалится навигация</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveNavigation(String screen_name)
			{
				for (Int32 i = 0; i < mNavigateUIScreens.Count; i++)
				{
					if (mNavigateUIScreens[i].name == screen_name)
					{
						RemoveNavigation(i);
						break;
					}
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