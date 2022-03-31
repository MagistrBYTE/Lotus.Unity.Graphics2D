//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIEffector.cs
*		Диспетчер визуальных эффектов модуля IMGUI Unity.
*		Реализация диспетчера некоторых визуальных эффектов модуля IMGUI Unity который обеспечивает затемнение экрана,
*	рисование рамки выбора, акцентирование на элементе и кинематографический режим.
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
		//! \addtogroup Unity2DImmedateGUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Диспетчер визуальных эффектов модуля IMGUI Unity
		/// </summary>
		/// <remarks>
		/// Реализация диспетчера некоторых визуальных эффектов модуля IMGUI Unity который обеспечивает затемнение экрана,
		/// рисование рамки выбора, акцентирование на элементе и кинематографический режим
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XGUIEffector
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Выбор региона
			private static Boolean mSelectingIsSupport = false;
			private static Boolean mSelectingStarting = false;
			private static Vector2 mSelectingStartPoint;
			private static Vector2 mSelectingLeftUpPoint;
			private static Boolean mSelectingRightToLeft = false;
			private static Color mSelectingColor = Color.red;
			private static Color mSelectingColorRightToLeft = Color.blue;
			private static Rect mSelectingRect;
			private static Single mSelectingDragCorrect = 10;
			private static Rect mSelectingRectCorrect;

			// Кинематографический режим
			private static Boolean mCinemaIsSupport = false;
			private static Boolean mCinemaIsVisible = true;
			private static Boolean mCinemaStarting = false;
			private static Boolean mCinemaEnding = false;
			private static GUIStyle mCinemaStyle;
			private static Single mCinemaAreaSize;
			private static Rect mCinemaRectTop;
			private static Rect mCinemaRectBottom;
			private static Single mCinemaStartTime;
			private static Single mCinemaDuration = 0.4f;

			// Акцент на элементе
			private static Boolean mHighlightIsSupport = true;
			private static Boolean mHighlightStarting = false;
			private static Boolean mHighlightEnding = false;
			private static Boolean mHighlightIsVisible = false;
			private static Texture2D mHighlightTexture;
			private static Rect mHighlightRectElement;
			private static Rect mHighlightRectTop;
			private static Rect mHighlightRectBottom;
			private static Rect mHighlightRectLeft;
			private static Rect mHighlightRectRight;
			private static Single mHighlightStartTime;
			private static Single mHighlightDuration = 0.4f;

			// Затемнение экрана
			private static Boolean mFadeIsSupport = true;
			private static Boolean mFadeStarting = false;
			private static Boolean mFadeEnding = false;
			private static Boolean mFadeIsVisible = false;
			private static Texture2D mFadeTexture;
			private static Rect mFadeScreen;
			private static Color mFadeColor = Color.white;
			private static Single mFadeStartTime;
			private static Single mFadeDuration = 0.4f;
			private static Action mOnFadeCompleted;
			private static Action mOnUnFadeCompleted;

			// Курсор в центре экрана
			private static Boolean mCursorShow;
			private static Texture2D mCursorTexture;
			private static Rect mCursorRect;
			private static Vector2 mCursorHotSpot;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ВЫБОР РЕГИОНА
			//
			/// <summary>
			/// Поддержка процесса выделения региона
			/// </summary>
			public static Boolean SelectingIsSupport
			{
				get { return mSelectingIsSupport; }
				set { mSelectingIsSupport = value; }
			}

			/// <summary>
			/// Начало процесса выделения региона
			/// </summary>
			public static Boolean SelectingStarting
			{
				get { return mSelectingStarting; }
			}

			/// <summary>
			/// Статус выделения справа налево
			/// </summary>
			public static Boolean SelectingRightToLeft
			{
				get { return mSelectingRightToLeft; }
			}

			/// <summary>
			/// Цвет рамки выделения
			/// </summary>
			public static Color SelectingColor
			{
				get { return mSelectingColor; }
				set { mSelectingColor = value; }
			}

			/// <summary>
			/// Цвет рамки выделения в режиме выделения справа налево
			/// </summary>
			public static Color SelectingColorRightToLeft
			{
				get { return mSelectingColorRightToLeft; }
				set { mSelectingColorRightToLeft = value; }
			}

			/// <summary>
			/// Прямоугольник выделения региона в экранных координатах
			/// </summary>
			public static Rect SelectingRect
			{
				get { return mSelectingRect; }
			}

			//
			// КИНЕМАТОГРАФИЧЕСКИЙ РЕЖИМ
			//
			/// <summary>
			/// Поддержка кинематографического режима
			/// </summary>
			public static Boolean CinemaIsSupport
			{
				get { return mCinemaIsSupport; }
				set { mCinemaIsSupport = value; }
			}

			/// <summary>
			/// Показ кинематографического режима
			/// </summary>
			public static Boolean CinemaIsVisible
			{
				get { return mCinemaIsVisible; }
			}

			/// <summary>
			/// Визуальный стиль для рисования кинематографического режима
			/// </summary>
			public static GUIStyle CinemaStyle
			{
				get { return mCinemaStyle; }
				set { mCinemaStyle = value; }
			}

			/// <summary>
			/// Размер области кинематографического режима
			/// </summary>
			public static Single CinemaAreaSize
			{
				get { return mCinemaAreaSize; }
				set { mCinemaAreaSize = value; }
			}

			/// <summary>
			/// Верхний прямоугольник кинематографического режима
			/// </summary>
			public static Rect CinemaRectTop
			{
				get { return mCinemaRectTop; }
			}

			/// <summary>
			/// Нижний прямоугольник кинематографического режима
			/// </summary>
			public static Rect CinemaRectBottom
			{
				get { return mCinemaRectBottom; }
			}

			/// <summary>
			/// Время показа прямоугольников кинематографического режима
			/// </summary>
			public static Single CinemaDuration
			{
				get { return mCinemaDuration; }
				set { mCinemaDuration = value; }
			}

			//
			// АКЦЕНТ НА ЭЛЕМЕНТЕ
			//
			/// <summary>
			/// Поддержка акцента(выделения)
			/// </summary>
			public static Boolean HighlightIsSupport
			{
				get { return mHighlightIsSupport; }
				set { mHighlightIsSupport = value; }
			}

			/// <summary>
			/// Акцент (выделение) прямоугольной области
			/// </summary>
			public static Boolean HighlightIsVisible
			{
				get { return mHighlightIsVisible; }
			}

			/// <summary>
			/// Прямоугольник элемента для выделения
			/// </summary>
			public static Rect HighlightRectElement
			{
				get { return mHighlightRectElement; }
				set { mHighlightRectElement = value; }
			}

			/// <summary>
			/// Верхний прямоугольник акцента(выделения)
			/// </summary>
			public static Rect HighlightRectTop
			{
				get { return mHighlightRectTop; }
			}

			/// <summary>
			/// Нижний прямоугольник акцента(выделения)
			/// </summary>
			public static Rect HighlightRectBottom
			{
				get { return mHighlightRectBottom; }
			}

			/// <summary>
			/// Время показа прямоугольников акцента(выделения)
			/// </summary>
			public static Single HighlightDuration
			{
				get { return mHighlightDuration; }
				set { mHighlightDuration = value; }
			}

			//
			// ЗАТЕМНЕНИЕ ЭКРАНА
			//
			/// <summary>
			/// Поддержка затемнения экрана
			/// </summary>
			public static Boolean FadeIsSupport
			{
				get { return mFadeIsSupport; }
				set { mFadeIsSupport = value; }
			}

			/// <summary>
			/// Начало процесса затемнения экрана
			/// </summary>
			public static Boolean FadeStarting
			{
				get { return mFadeStarting; }
			}

			/// <summary>
			/// Окончание процесса затемнения экрана
			/// </summary>
			public static Boolean FadeEnding
			{
				get { return mFadeEnding; }
			}

			/// <summary>
			/// Статус затемнения экрана
			/// </summary>
			public static Boolean FadeIsVisible
			{
				get { return mFadeIsVisible; }
			}

			/// <summary>
			/// Текстура для затемнения экрана
			/// </summary>
			public static Texture2D FadeTexture
			{
				get { return mFadeTexture; }
				set { mFadeTexture = value; }
			}

			/// <summary>
			/// Прямоугольник затемнения экрана
			/// </summary>
			public static Rect FadeScreen
			{
				get { return mFadeScreen; }
				set { mFadeScreen = value; }
			}

			/// <summary>
			/// Цвет модуляции текстуры для затемнения экрана
			/// </summary>
			public static Color FadeColor
			{
				get { return mFadeColor; }
				set { mFadeColor = value; }
			}

			/// <summary>
			/// Продолжительность затемнения экрана
			/// </summary>
			public static Single FadeDuration
			{
				get { return mFadeDuration; }
				set { mFadeDuration = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании процесса затемнения экрана
			/// </summary>
			public static Action OnFadeCompleted
			{
				get { return mOnFadeCompleted; }
				set { mOnFadeCompleted = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании процесса отмены затемнения экрана
			/// </summary>
			public static Action OnUnFadeCompleted
			{
				get { return mOnUnFadeCompleted; }
				set { mOnUnFadeCompleted = value; }
			}

			/// <summary>
			/// Показать/скрыть курсор
			/// </summary>
			public static Boolean CursorShow
			{
				get { return mCursorShow; }
				set
				{
					mCursorShow = value;
					if(mCursorShow)
					{
						Cursor.lockState = CursorLockMode.Locked;
						Cursor.visible = true;
						Cursor.SetCursor(mCursorTexture, mCursorHotSpot, CursorMode.Auto);
					}
					else
					{
						Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
					}
				}
			}

			/// <summary>
			/// Текстура курсора
			/// </summary>
			public static Texture2D CursorTexture
			{
				get { return mCursorTexture; }
				set
				{
					mCursorTexture = value;
					mCursorHotSpot.x = (Single)mCursorTexture.width / 2;
					mCursorHotSpot.y = (Single)mCursorTexture.height / 2;
				}
			}

			/// <summary>
			/// Прямоугольник курсора
			/// </summary>
			public static Rect CursorRect
			{
				get { return mCursorRect; }
				set
				{
					mCursorRect = value;
				}
			}
			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
				InitSelectingRegion();
				InitCinemaMode();
				InitHighlightElement();
				InitFadeScreen();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных диспетчера каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnUpdate()
			{
				// Поддержка выделения рамкой
				if (mSelectingIsSupport)
				{
					if (Input.GetMouseButtonDown(0))
					{
						StartSelectingRegion();
					}

					// Поддержка выделения рамкой
					if (Input.GetMouseButton(0) && XInputDispatcher.IsDeltaMove)
					{
						ProcessSelectingRegion();
					}

					// Левая кнопка мыши
					if (Input.GetMouseButtonUp(0))
					{
						EndSelectingRegion();
					}
				}

				// Поддержка кинематографического режима
				if (mCinemaIsSupport)
				{
					ProcessCinemaMode();
				}

				// Поддержка акцентирования(выделения элемента)
				if (mHighlightIsSupport)
				{
					ProcessHighlightElement();
				}

				// Поддержка затемнения экрана
				if (mFadeIsSupport)
				{
					ProcessFadeScreen();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование визуальных эффектов диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnDraw()
			{
				// Рисуем рамку выделения
				if (mSelectingIsSupport && mSelectingStarting)
				{
					if (mSelectingRightToLeft)
					{
						XGUIRender.DrawRectSelection(ref mSelectingRect, ref mSelectingColorRightToLeft);
					}
					else
					{
						XGUIRender.DrawRectSelection(ref mSelectingRect, ref mSelectingColor);
					}
				}

				// Рисуем кинематографический режим
				if (mCinemaIsSupport && mCinemaIsVisible)
				{
					GUI.Label(mCinemaRectTop, "", mCinemaStyle);
					GUI.Label(mCinemaRectBottom, "", mCinemaStyle);
				}

				// Рисуем режим выделения элемента
				if (mHighlightIsSupport && mHighlightIsVisible)
				{
					GUI.DrawTexture(mHighlightRectTop, mHighlightTexture, ScaleMode.StretchToFill, true);
					GUI.DrawTexture(mHighlightRectBottom, mHighlightTexture, ScaleMode.StretchToFill, true);
					GUI.DrawTexture(mHighlightRectLeft, mHighlightTexture, ScaleMode.StretchToFill, true);
					GUI.DrawTexture(mHighlightRectRight, mHighlightTexture, ScaleMode.StretchToFill, true);
				}

				// Рисуем затенение экрана
				if (mFadeIsSupport && mFadeIsVisible)
				{
					XGUIRender.DrawTexture(ref mFadeScreen, mFadeTexture, ref mFadeColor);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ВЫДЕЛЕНИЕМ РЕГИОНА ========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных для работы с выделением региона
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void InitSelectingRegion()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало операции выделения региона
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void StartSelectingRegion()
			{
				mSelectingStarting = true;
				mSelectingStartPoint = XInputDispatcher.PositionPointerLeftDown;
				mSelectingRectCorrect.x = mSelectingStartPoint.x - mSelectingDragCorrect / 2;
				mSelectingRectCorrect.y = mSelectingStartPoint.y - mSelectingDragCorrect / 2;
				mSelectingRectCorrect.width = mSelectingDragCorrect;
				mSelectingRectCorrect.height = mSelectingDragCorrect;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Операция выделения региона (вызывается в MouseMove)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void ProcessSelectingRegion()
			{
				// Если есть выход за пределы корректировочного прямоугольника
				Vector2 mouse_position = XInputDispatcher.PositionPointer;
				if (!mSelectingRectCorrect.Contains(mouse_position))
				{
					if (mSelectingStartPoint.x < mouse_position.x)
					{
						mSelectingLeftUpPoint.x = mSelectingStartPoint.x;
						mSelectingRightToLeft = false;
					}
					else
					{
						mSelectingLeftUpPoint.x = mouse_position.x;
						mSelectingRightToLeft = true;
					}

					if (mSelectingStartPoint.y < mouse_position.y)
					{
						mSelectingLeftUpPoint.y = mSelectingStartPoint.y;
					}
					else
					{
						mSelectingLeftUpPoint.y = mouse_position.y;
					}

					mSelectingRect.x = mSelectingLeftUpPoint.x;
					mSelectingRect.y = mSelectingLeftUpPoint.y;
					mSelectingRect.width = Math.Abs(mSelectingStartPoint.x - mouse_position.x);
					mSelectingRect.height = Math.Abs(mSelectingStartPoint.y - mouse_position.y);

					mSelectingStarting = true;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание операции выделения региона
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void EndSelectingRegion()
			{
				mSelectingStarting = false;
				mSelectingRect = Rect.zero;
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С КИНЕМАТОГРАФИЧЕСКИМ РЕЖИМОМ ===============
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных кинематографического режима
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void InitCinemaMode()
			{
				if(mCinemaStyle == null)
				{
					mCinemaStyle = LotusGUIDispatcher.CurrentSkin.box;
				}
				if(Mathf.Approximately(mCinemaAreaSize, 0))
				{
					mCinemaAreaSize = LotusGUIDispatcher.ScreenHeight / 10;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало операции для показа кинематографического режима
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void StartCinemaMode()
			{
				mCinemaEnding = false;
				mCinemaStarting = true;
				mCinemaRectTop = new Rect(0, 0, Screen.width, 0);
				mCinemaRectBottom = new Rect(0, Screen.height, Screen.width, 0);
				mCinemaStartTime = Time.unscaledTime;
				mCinemaIsVisible = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Операция показа кинематографического режима
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void ProcessCinemaMode()
			{
				// Начало
				if (mCinemaStarting)
				{
					// Считаем время
					Single delta_time = (Time.unscaledTime - mCinemaStartTime) / mCinemaDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mCinemaStarting = false;
					}

					mCinemaRectTop.height = Mathf.Lerp(0, mCinemaAreaSize, delta_time);
					mCinemaRectBottom.y = LotusGUIDispatcher.ScreenHeight - mCinemaRectTop.height;
					mCinemaRectBottom.height = mCinemaRectTop.height;
				}

				// Окончание
				if (mCinemaEnding)
				{
					// Считаем время
					Single delta_time = (Time.unscaledTime - mCinemaStartTime) / mCinemaDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mCinemaEnding = false;
						mCinemaIsVisible = false;
					}

					Single pos = Mathf.Lerp(mCinemaAreaSize, 0, delta_time);
					mCinemaRectTop.height = pos;
					mCinemaRectBottom.y = LotusGUIDispatcher.ScreenHeight - pos;
					mCinemaRectBottom.height = pos;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание операции показа кинематографического режима
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void EndCinemaMode()
			{
				mCinemaEnding = true;
				mCinemaStarting = false;
				mCinemaStartTime = Time.unscaledTime;
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ВЫДЕЛЕНИЕМ ЭЛЕМЕНТА =======================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных акцентирования (выделения) элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void InitHighlightElement()
			{
				if(mHighlightTexture == null)
				{
					mHighlightTexture = XTexture2D.BlackAlpha25;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало операции для акцентирования (выделения) элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void StartHighlightElement()
			{
				mHighlightEnding = false;
				mHighlightStarting = true;
				mHighlightRectTop = new Rect(0, 0, Screen.width, 0);
				mHighlightRectBottom = new Rect(0, Screen.height, Screen.width, 0);
				mHighlightRectLeft = new Rect(0, mHighlightRectElement.y, 0, mHighlightRectElement.height);
				mHighlightRectRight = new Rect(Screen.width, mHighlightRectElement.y, 0, mHighlightRectElement.height);
				mHighlightStartTime = Time.unscaledTime;
				mHighlightIsVisible = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Операция показа акцентирования (выделения) элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void ProcessHighlightElement()
			{
				// Начало
				if (mHighlightStarting)
				{
					// Считаем время
					Single delta_time = (Time.unscaledTime - mHighlightStartTime) / mHighlightDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mHighlightStarting = false;
					}

					mHighlightRectTop.height = Mathf.Lerp(0, mHighlightRectElement.y, delta_time);

					mHighlightRectBottom.y = Mathf.Lerp(Screen.height, mHighlightRectElement.yMax, delta_time);
					mHighlightRectBottom.height = Screen.height - mHighlightRectBottom.y;

					mHighlightRectLeft.y = mHighlightRectTop.yMax;
					mHighlightRectLeft.width = Mathf.Lerp(0, mHighlightRectElement.x, delta_time);
					mHighlightRectLeft.yMax = mHighlightRectBottom.y;

					mHighlightRectRight.y = mHighlightRectTop.yMax;
					mHighlightRectRight.x = Mathf.Lerp(Screen.width, mHighlightRectElement.xMax, delta_time);
					mHighlightRectRight.width = Screen.width - mHighlightRectRight.x;
					mHighlightRectRight.yMax = mHighlightRectBottom.y;
				}

				// Окончание
				if (mHighlightEnding)
				{
					// Считаем время
					Single delta_time = (Time.unscaledTime - mHighlightStartTime) / mHighlightDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mHighlightEnding = false;
						mHighlightIsVisible = false;
					}

					mHighlightRectTop.height = Mathf.Lerp(mHighlightRectElement.y, 0, delta_time);

					mHighlightRectBottom.y = Mathf.Lerp(mHighlightRectElement.yMax, Screen.height, delta_time);
					mHighlightRectBottom.height = Screen.height - mHighlightRectBottom.y;

					mHighlightRectLeft.y = mHighlightRectTop.yMax;
					mHighlightRectLeft.width = Mathf.Lerp(mHighlightRectElement.x, 0, delta_time);
					mHighlightRectLeft.yMax = mHighlightRectBottom.y;

					mHighlightRectRight.y = mHighlightRectTop.yMax;
					mHighlightRectRight.x = Mathf.Lerp(mHighlightRectElement.xMax, Screen.width, delta_time);
					mHighlightRectRight.width = Screen.width - mHighlightRectRight.x;
					mHighlightRectRight.yMax = mHighlightRectBottom.y;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание операции акцентирования (выделения) элемента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void EndHighlightElement()
			{
				mHighlightEnding = true;
				mHighlightStarting = false;
				mHighlightStartTime = Time.unscaledTime;
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ЗАТЕМНЕНИЕМ ЭКРАНА ========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных затемнения экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void InitFadeScreen()
			{
				mFadeScreen = new Rect(0, 0, Screen.width, Screen.height);
				mFadeTexture = XTexture2D.BlackAlpha50;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало операции для затемнения экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void StartFadeScreen()
			{
				mFadeEnding = false;
				mFadeStarting = true;
				mFadeIsVisible = true;
				mFadeStartTime = Time.unscaledTime;
				mFadeColor.a = 0.0f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало операции для затемнения экрана
			/// </summary>
			/// <param name="duration">Время затенения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void StartFadeScreen(Single duration = 1.4f)
			{
				mFadeStartTime = Time.unscaledTime;
				mFadeColor.a = 0.0f;
				mFadeEnding = false;
				mFadeStarting = true;
				mFadeIsVisible = true;
				mFadeDuration = duration;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Начало операции для затемнения экрана
			/// </summary>
			/// <param name="texture_fade">Текстура затенения</param>
			/// <param name="duration">Время затенения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void StartFadeScreen(Texture2D texture_fade, Single duration = 1.4f)
			{
				mFadeStartTime = Time.unscaledTime;
				mFadeColor.a = 0.0f;
				mFadeTexture = texture_fade;
				mFadeEnding = false;
				mFadeStarting = true;
				mFadeIsVisible = true;
				mFadeDuration = duration;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Операция показа затемнения экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void ProcessFadeScreen()
			{
				// Начало
				if (mFadeStarting)
				{
					// Считаем время
					Single delta_time = (Time.unscaledTime - mFadeStartTime) / mFadeDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mFadeStarting = false;

						// Вызываем событие
						if (mOnFadeCompleted != null) mOnFadeCompleted();
					}

					mFadeColor.a = delta_time;
				}

				// Окончание
				if (mFadeEnding)
				{
					// Считаем время
					Single delta_time = (Time.unscaledTime - mFadeStartTime) / mFadeDuration;

					if (delta_time > 1)
					{
						delta_time = 1;
						mFadeEnding = false;
						mFadeIsVisible = false;

						// Вызываем событие
						if (mOnUnFadeCompleted != null) mOnUnFadeCompleted();
					}

					mFadeColor.a = 1.0f - delta_time;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Окончание операции затемнения экрана
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void EndFadeScreen()
			{
				mFadeEnding = true;
				mFadeStarting = false;
				mFadeStartTime = Time.unscaledTime;
				mFadeColor.a = 1.0f;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================