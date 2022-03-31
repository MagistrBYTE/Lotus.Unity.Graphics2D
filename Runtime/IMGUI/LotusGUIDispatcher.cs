//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIDispatcher.cs
*		Центральный диспетчер элементов модуля IMGUI Unity.
*		Центральный диспетчер осуществляет управление модулем IMGUI Unity обеспечивает создание, регистрацию, 
*	поиск и рисование элементов, управляет диспетчером визуальных эффектов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
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
		//! \defgroup Unity2DImmedateGUI МОДУЛЬ IMGUI Unity
		//! Модуль IMGUI представляют собой объектную оболочку над элементами подсистемы IMGUI, обеспечивает ряд визуальных
		//! эффектов и рисование геометрических примитивов через подсистему IMGUI
		//! \ingroup UnityGraphics2D
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер элементов модуля IMGUI Unity
		/// </summary>
		/// <remarks>
		/// Центральный диспетчер осуществляет управление модулем IMGUI Unity обеспечивает создание, регистрацию,
		/// поиск и рисование элементов, управляет диспетчером визуальных эффектов
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XGUIEditorSettings.MenuPath + "GUIDispatcher")]
		[LotusExecutionOrder(10)]
		public class LotusGUIDispatcher : LotusBaseGraphics2DDispatcher<LotusGUIDispatcher>, ILotusBaseGraphics2DDispatcher, 
			ILotusMessageHandler
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Текущий визуальный стиль для рисования
			/// </summary>
			public static readonly GUIStyle CurrentStyle = new GUIStyle(GUIStyle.none);

			/// <summary>
			/// Текущий контент для рисования
			/// </summary>
			public static readonly GUIContent CurrentContent = new GUIContent(GUIContent.none);
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			// Данные для отрисовки элементов GUI
			internal static Color mCurrentColor;
			internal static Color mCurrentColorContent;
			internal static Color mCurrentColorBackground;
			internal static Boolean mCurrentEnabled;
			internal static Int32 mCurrentDepth;

			// Визуальные стили
			internal static String[] mStyleNames;
			internal static List<GUIStyle> mStyles;
			internal static List<Int32> mStylesDefaultFontSize;

			// Элементы интерфейса
			internal static Boolean mIsRemoved;
			internal static Boolean mIsUpdated;
			internal static Int32 mRenderElements;

			// Стандартные стили
			internal static GUIStyle mTextStyle;
			internal static GUIStyle mTextMonoStyle;
			internal static GUIStyle mTextHeaderStyle;
			internal static GUIStyle mTextValueStyle;
			internal static GUIStyle mTextColumnStyle;
			internal static GUIStyle mTextSmallStyle;

			internal static GUIStyle mLabelStyle;
			internal static GUIStyle mLabelMonoStyle;
			internal static GUIStyle mLabelHeaderStyle;
			internal static GUIStyle mLabelValueStyle;
			internal static GUIStyle mLabelColumnStyle;
			internal static GUIStyle mLabelSmallStyle;

			internal static GUIStyle mPanelStyle;
			internal static GUIStyle mPanelGroupStyle;
			internal static GUIStyle mPanelHeaderStyle;
			internal static GUIStyle mPanelValueStyle;
			internal static GUIStyle mPanelColumnStyle;

			internal static GUIStyle mButtonMiniStyle;
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ СВОЙСТВА ======================================
			//
			// ВИЗУАЛЬНЫЙ СКИН
			// 
			/// <summary>
			/// Текущий визуальный скин
			/// </summary>
			public static GUISkin CurrentSkin
			{
				get
				{
					if (Instance.mCurrentSkin == null)
					{
						// Используем системный скин
						Instance.mCurrentSkin = LotusSystemDispatcher.Instance.CurrentSkin;
					}

					return Instance.mCurrentSkin;
				}
				set
				{
					Instance.mCurrentSkin = value;
				}
			}

			/// <summary>
			/// Проверка на существование визуального скина
			/// </summary>
			public static Boolean IsCurrentSkinNull
			{
				get
				{
					return (Instance.mCurrentSkin == null);
				}
			}

			//
			// ЭЛЕМЕНТЫ ИНТЕРФЕЙСА
			//
			/// <summary>
			/// Статус обновления элементов
			/// </summary>
			public static Boolean IsUpdated
			{
				set
				{
					mIsUpdated = value;
				}
				get
				{
					return mIsUpdated;
				}
			}

			//
			// ПРОСТОЙ ТЕКСТ
			//
			/// <summary>
			/// Визуальный стиль для рисования простого текста
			/// </summary>
			public static GUIStyle TextStyle
			{
				get
				{
					if (mTextStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mTextStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования для рисования моноширинного текста
			/// </summary>
			public static GUIStyle TextMonoStyle
			{
				get
				{
					if (mTextMonoStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mTextMonoStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования текста заголовка
			/// </summary>
			public static GUIStyle TextHeaderStyle
			{
				get
				{
					if (mTextHeaderStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mTextHeaderStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования текста значения
			/// </summary>
			public static GUIStyle TextValueStyle
			{
				get
				{
					if (mTextValueStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mTextValueStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования текста столбца
			/// </summary>
			public static GUIStyle TextColumnStyle
			{
				get
				{
					if (mTextColumnStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mTextColumnStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования текста меньшим шрифтом
			/// </summary>
			public static GUIStyle TextSmallStyle
			{
				get
				{
					if (mTextSmallStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mTextSmallStyle);
				}
			}

			//
			// НАДПИСИ
			//
			/// <summary>
			/// Визуальный стиль для рисования простой надписи
			/// </summary>
			public static GUIStyle LabelStyle
			{
				get
				{
					if (mLabelStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mLabelStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования для рисования моноширинной надписи
			/// </summary>
			public static GUIStyle LabelMonoStyle
			{
				get
				{
					if (mLabelMonoStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mLabelMonoStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования надписи заголовка
			/// </summary>
			public static GUIStyle LabelHeaderStyle
			{
				get
				{
					if (mLabelHeaderStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mLabelHeaderStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования надписи значения
			/// </summary>
			public static GUIStyle LabelValueStyle
			{
				get
				{
					if (mLabelValueStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mLabelValueStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования надписи столбца
			/// </summary>
			public static GUIStyle LabelColumnStyle
			{
				get
				{
					if (mLabelColumnStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mLabelColumnStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования надписи меньшим шрифтом
			/// </summary>
			public static GUIStyle LabelSmallStyle
			{
				get
				{
					if (mLabelSmallStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mLabelSmallStyle);
				}
			}

			//
			// ПАНЕЛИ
			//
			/// <summary>
			/// Визуальный стиль для рисования общей панели
			/// </summary>
			public static GUIStyle PanelStyle
			{
				get
				{
					if (mPanelStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mPanelStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования панели группирования
			/// </summary>
			public static GUIStyle PanelGroupStyle
			{
				get
				{
					if (mPanelGroupStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mPanelGroupStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования панели с заголовком
			/// </summary>
			public static GUIStyle PanelHeaderStyle
			{
				get
				{
					if (mPanelHeaderStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mPanelHeaderStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования панели значения
			/// </summary>
			public static GUIStyle PanelValueStyle
			{
				get
				{
					if (mPanelValueStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mPanelValueStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования панели столбца
			/// </summary>
			public static GUIStyle PanelColumnStyle
			{
				get
				{
					if (mPanelColumnStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return (mPanelColumnStyle);
				}
			}

			//
			// КНОПКИ
			//
			/// <summary>
			/// Визуальный стиль для рисования некоторых управляющих кнопок
			/// </summary>
			public static GUIStyle ButtonMiniStyle
			{
				get
				{
					if (mButtonMiniStyle == null)
					{
						LoadDefaultStyles(CurrentSkin);
					}

					return mButtonMiniStyle;
				}
			}
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка стандартных визуальных стилей 
			/// </summary>
			/// <param name="skin">Визуальный скин</param>
			//---------------------------------------------------------------------------------------------------------
			private static void LoadDefaultStyles(GUISkin skin)
			{
				//
				// ПРОСТОЙ ТЕКСТ
				//
				if (mTextStyle == null) mTextStyle = skin.FindStyle(XGUISkin.TEXT_STYLE_NAME);
				if (mTextStyle == null)
				{
					mTextStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.TEXT_STYLE_NAME, "label");
				}

				if (mTextMonoStyle == null) mTextMonoStyle = skin.FindStyle(XGUISkin.TEXT_MONO_STYLE_NAME);
				if (mTextMonoStyle == null)
				{
					mTextMonoStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.TEXT_MONO_STYLE_NAME, "label");
				}

				if (mTextHeaderStyle == null) mTextHeaderStyle = skin.FindStyle(XGUISkin.TEXT_HEADER_STYLE_NAME);
				if (mTextHeaderStyle == null)
				{
					mTextHeaderStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.TEXT_HEADER_STYLE_NAME, "label");
				}

				if (mTextValueStyle == null) mTextValueStyle = skin.FindStyle(XGUISkin.TEXT_VALUE_STYLE_NAME);
				if (mTextValueStyle == null)
				{
					mTextValueStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.TEXT_VALUE_STYLE_NAME, "label");
				}

				if (mTextColumnStyle == null) mTextColumnStyle = skin.FindStyle(XGUISkin.TEXT_COLUMN_STYLE_NAME);
				if (mTextColumnStyle == null)
				{
					mTextColumnStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.TEXT_COLUMN_STYLE_NAME, "label");
				}

				if (mTextSmallStyle == null) mTextSmallStyle = skin.FindStyle(XGUISkin.TEXT_SMALL_STYLE_NAME);
				if (mTextSmallStyle == null)
				{
					mTextSmallStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.TEXT_SMALL_STYLE_NAME, "label");
				}

				//
				// НАДПИСИ
				//
				if (mLabelStyle == null) mLabelStyle = skin.label;

				if (mLabelMonoStyle == null) mLabelMonoStyle = skin.FindStyle(XGUISkin.LABEL_MONO_STYLE_NAME);
				if (mLabelMonoStyle == null)
				{
					mLabelMonoStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.LABEL_MONO_STYLE_NAME, "label");
				}

				if (mLabelHeaderStyle == null) mLabelHeaderStyle = skin.FindStyle(XGUISkin.LABEL_HEADER_STYLE_NAME);
				if (mLabelHeaderStyle == null)
				{
					mLabelHeaderStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.LABEL_HEADER_STYLE_NAME, "label");
				}

				if (mLabelValueStyle == null) mLabelValueStyle = skin.FindStyle(XGUISkin.LABEL_VALUE_STYLE_NAME);
				if (mLabelValueStyle == null)
				{
					mLabelValueStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.LABEL_VALUE_STYLE_NAME, "label");
				}

				if (mLabelColumnStyle == null) mLabelColumnStyle = skin.FindStyle(XGUISkin.LABEL_COLUMN_STYLE_NAME);
				if (mLabelColumnStyle == null)
				{
					mLabelColumnStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.LABEL_COLUMN_STYLE_NAME, "label");
				}

				if (mLabelSmallStyle == null) mLabelSmallStyle = skin.FindStyle(XGUISkin.LABEL_SMALL_STYLE_NAME);
				if (mLabelSmallStyle == null)
				{
					mLabelSmallStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.LABEL_SMALL_STYLE_NAME, "label");
				}

				//
				// ПАНЕЛИ
				//
				if (mPanelStyle == null) mPanelStyle = skin.box;

				if (mPanelGroupStyle == null) mPanelGroupStyle = skin.FindStyle(XGUISkin.PANEL_GROUP_STYLE_NAME);
				if (mPanelGroupStyle == null)
				{
					mPanelGroupStyle = skin.box;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.PANEL_GROUP_STYLE_NAME, "box");
				}

				if (mPanelHeaderStyle == null) mPanelHeaderStyle = skin.FindStyle(XGUISkin.PANEL_HEADER_STYLE_NAME);
				if (mPanelHeaderStyle == null)
				{
					mPanelHeaderStyle = skin.box;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.PANEL_HEADER_STYLE_NAME, "box");
				}

				if (mPanelValueStyle == null) mPanelValueStyle = skin.FindStyle(XGUISkin.PANEL_VALUE_STYLE_NAME);
				if (mPanelValueStyle == null)
				{
					mPanelValueStyle = skin.box;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.PANEL_VALUE_STYLE_NAME, "box");
				}

				if (mPanelColumnStyle == null) mPanelColumnStyle = skin.FindStyle(XGUISkin.PANEL_COLUMN_STYLE_NAME);
				if (mPanelColumnStyle == null)
				{
					mPanelColumnStyle = skin.box;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.PANEL_COLUMN_STYLE_NAME, "box");
				}

				if (mButtonMiniStyle == null) mButtonMiniStyle = skin.FindStyle(XGUISkin.BUTTON_MINI_STYLE_NAME);
				if (mButtonMiniStyle == null)
				{
					mButtonMiniStyle = new GUIStyle(skin.button);
					mButtonMiniStyle.border.left = 1;
					mButtonMiniStyle.border.right = 1;
					mButtonMiniStyle.border.top = 1;
					mButtonMiniStyle.border.bottom = 1;
					mButtonMiniStyle.fontSize = 9;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", XGUISkin.BUTTON_MINI_STYLE_NAME, "button");
				}

			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			internal String mGroupMessage = "GUI";

			// Визуальный стиль
			[SerializeField]
			internal GUISkin mCurrentSkin;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= СВОЙСТВА ILotusMessageHandler =============================
			/// <summary>
			/// Группа которой принадлежит данный обработчик событий
			/// </summary>
			public String MessageHandlerGroup
			{
				get
				{
					return (mGroupMessage);
				}
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование UnityGUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnGUI()
			{
				// Сохраняем текущие состояние
				mCurrentColor = GUI.color;
				mCurrentColorContent = GUI.contentColor;
				mCurrentColorBackground = GUI.backgroundColor;
				mCurrentEnabled = GUI.enabled;
				mCurrentDepth = GUI.depth;

				// Скин
				GUI.skin = LotusSystemDispatcher.Instance.CurrentSkin;

				// Основные цвета будут браться из стилей
				GUI.backgroundColor = Color.white;
				GUI.contentColor = Color.white;
				GUI.color = Color.white;

				// Рисуем все элементы
				mIsPointerOverElement = false;
				mRenderElements = 0;
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					if (Elements[i].IsVisibleElement)
					{
						mIsPointerOverElement = Elements[i].ContainsScreen(Event.current.mousePosition);
						Elements[i].OnDraw();
						mRenderElements++;
					}
				}

				// Рисуем эффекты
				XGUIEffector.OnDraw();

				// Восстанавливаем состояние
				GUI.color = mCurrentColor;
				GUI.contentColor = mCurrentColorContent;
				GUI.backgroundColor = mCurrentColorBackground;
				GUI.enabled = mCurrentEnabled;
				GUI.depth = mCurrentDepth;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Завершение работы приложения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void OnApplicationQuit()
			{
#if UNITY_EDITOR
				// В режиме редактора восстанавливаем размеры шрифтов стилей на размер по умолчанию 
				ResetStylesFont();
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBaseGraphics2DDispatcher =====================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ConstructorDispatcher()
			{
				if (mCurrentSkin == null)
				{
					mCurrentSkin = LotusSystemDispatcher.Instance.CurrentSkin;
				}
				if (mCurrentSkin != null)
				{
					// Считаем размеры полос прокруток
					mSizeScrollVertical = mCurrentSkin.verticalScrollbar.fixedWidth;
					mSizeScrollHorizontal = mCurrentSkin.horizontalScrollbar.fixedHeight;

					// Инициализируем стили
					InitStyles();

					// Инициализируем данные диспетчер визуальных эффектов
					XGUIEffector.OnInit();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение размеров экрана
			/// </summary>
			/// <remarks>
			/// Метод вызывается одни раз в стартовом методе, и в режиме редактора каждый раз когда меняются размеры экрана
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			protected override void ChangeSizeScreenDispatcher()
			{
				// Обновляем размер шрифта
				ComputeStylesFont();

				for (Int32 i = 0; i < Elements.Count; i++)
				{
					Elements[i].UpdatePlacement();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdateDispatcher()
			{
				// Если есть удаляемые элементы
				if (mIsRemoved)
				{
					for (Int32 i = 0; i < RemovedElements.Count; i++)
					{
						Elements.Remove(RemovedElements[i]);
					}

					mIsRemoved = false;
					RemovedElements.Clear();
				}

				// Если есть обновляемые элементы
				if (mIsUpdated)
				{
					Boolean update = false;
					for (Int32 i = 0; i < Elements.Count; i++)
					{
						if (Elements[i].IsDirty)
						{
							update = true;
							Elements[i].OnUpdate();
						}
					}

					mIsUpdated = update;
				}

				// Обновляем данные диспетчер визуальных эффектов
				XGUIEffector.OnUpdate();
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusMessageHandler ===============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Основной метод для обработки сообщения
			/// </summary>
			/// <param name="args">Аргументы сообщения</param>
			/// <returns>Статус успешности обработки сообщений</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 OnMessageHandler(CMessageArgs args)
			{
				return (XMessageHandlerResultCode.NEGATIVE_RESULT);
			}
			#endregion

			#region ======================================= РАБОТА СО СТИЛЯМИ =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация визуальных стилей
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void InitStyles()
			{
				if (mStyles == null)
				{
					mStyles = new List<GUIStyle>();
					mStylesDefaultFontSize = new List<Int32>();

					mStyles.Add(GUIStyle.none);
					mStylesDefaultFontSize.Add(GUIStyle.none.fontSize);
#if UNITY_EDITOR
					mStyles.Add(new GUIStyle(CurrentSkin.box));
					mStylesDefaultFontSize.Add(CurrentSkin.box.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.label));
					mStylesDefaultFontSize.Add(CurrentSkin.label.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.button));
					mStylesDefaultFontSize.Add(CurrentSkin.button.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.toggle));
					mStylesDefaultFontSize.Add(CurrentSkin.toggle.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.horizontalScrollbar));
					mStylesDefaultFontSize.Add(CurrentSkin.horizontalScrollbar.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.horizontalScrollbarThumb));
					mStylesDefaultFontSize.Add(CurrentSkin.horizontalScrollbarThumb.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.horizontalSlider));
					mStylesDefaultFontSize.Add(CurrentSkin.horizontalSlider.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.horizontalSliderThumb));
					mStylesDefaultFontSize.Add(CurrentSkin.horizontalSliderThumb.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.verticalScrollbar));
					mStylesDefaultFontSize.Add(CurrentSkin.verticalScrollbar.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.verticalScrollbarThumb));
					mStylesDefaultFontSize.Add(CurrentSkin.verticalScrollbarThumb.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.verticalSlider));
					mStylesDefaultFontSize.Add(CurrentSkin.verticalSlider.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.verticalSliderThumb));
					mStylesDefaultFontSize.Add(CurrentSkin.verticalSliderThumb.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.window));
					mStylesDefaultFontSize.Add(CurrentSkin.window.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.scrollView));
					mStylesDefaultFontSize.Add(CurrentSkin.scrollView.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.textField));
					mStylesDefaultFontSize.Add(CurrentSkin.textField.fontSize);

					mStyles.Add(new GUIStyle(CurrentSkin.textArea));
					mStylesDefaultFontSize.Add(CurrentSkin.textArea.fontSize);

					for (Int32 i = 0; i < CurrentSkin.customStyles.Length; i++)
					{
						mStyles.Add(new GUIStyle(CurrentSkin.customStyles[i]));
						mStylesDefaultFontSize.Add(CurrentSkin.customStyles[i].fontSize);
					}

#else
					mStyles.Add(CurrentSkin.box);
					mStylesDefaultFontSize.Add(CurrentSkin.box.fontSize);

					mStyles.Add(CurrentSkin.label);
					mStylesDefaultFontSize.Add(CurrentSkin.label.fontSize);

					mStyles.Add(CurrentSkin.button);
					mStylesDefaultFontSize.Add(CurrentSkin.button.fontSize);

					mStyles.Add(CurrentSkin.toggle);
					mStylesDefaultFontSize.Add(CurrentSkin.toggle.fontSize);

					mStyles.Add(CurrentSkin.horizontalScrollbar);
					mStylesDefaultFontSize.Add(CurrentSkin.horizontalScrollbar.fontSize);

					mStyles.Add(CurrentSkin.horizontalScrollbarThumb);
					mStylesDefaultFontSize.Add(CurrentSkin.horizontalScrollbarThumb.fontSize);

					mStyles.Add(CurrentSkin.horizontalSlider);
					mStylesDefaultFontSize.Add(CurrentSkin.horizontalSlider.fontSize);

					mStyles.Add(CurrentSkin.horizontalSliderThumb);
					mStylesDefaultFontSize.Add(CurrentSkin.horizontalSliderThumb.fontSize);

					mStyles.Add(CurrentSkin.verticalScrollbar);
					mStylesDefaultFontSize.Add(CurrentSkin.verticalScrollbar.fontSize);

					mStyles.Add(CurrentSkin.verticalScrollbarThumb);
					mStylesDefaultFontSize.Add(CurrentSkin.verticalScrollbarThumb.fontSize);

					mStyles.Add(CurrentSkin.verticalSlider);
					mStylesDefaultFontSize.Add(CurrentSkin.verticalSlider.fontSize);

					mStyles.Add(CurrentSkin.verticalSliderThumb);
					mStylesDefaultFontSize.Add(CurrentSkin.verticalSliderThumb.fontSize);

					mStyles.Add(CurrentSkin.window);
					mStylesDefaultFontSize.Add(CurrentSkin.window.fontSize);

					mStyles.Add(CurrentSkin.scrollView);
					mStylesDefaultFontSize.Add(CurrentSkin.scrollView.fontSize);

					mStyles.Add(CurrentSkin.textField);
					mStylesDefaultFontSize.Add(CurrentSkin.textField.fontSize);

					mStyles.Add(CurrentSkin.textArea);
					mStylesDefaultFontSize.Add(CurrentSkin.textArea.fontSize);

					for (Int32 i = 0; i < CurrentSkin.customStyles.Length; i++)
					{
						mStyles.Add(CurrentSkin.customStyles[i]);
						mStylesDefaultFontSize.Add(CurrentSkin.customStyles[i].fontSize);
					}
#endif
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление актуального размера шрифта визуальных стилей
			/// </summary>
			/// <remarks>
			/// Помимо обновлением размеров элемента адаптируем к размеру экрана нам также нужно увеличить/уменьшить
			/// размер шрифта. Размер шрифта в данной версии изменяется на средней коэффициент масштаба
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public static void ComputeStylesFont()
			{
				for (Int32 i = 0; i < mStyles.Count; i++)
				{
					mStyles[i].fontSize = Mathf.FloorToInt((Single)mStylesDefaultFontSize[i] * 
						LotusSystemDispatcher.ScaledScreenAverage);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка размера шрифта визуальных стилей по умолчанию (тот который был изначально)
			/// </summary>
			/// <remarks>
			/// Применяется только для режима редактора
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			private static void ResetStylesFont()
			{
				for (Int32 i = 0; i < mStyles.Count; i++)
				{
					mStyles[i].fontSize = mStylesDefaultFontSize[i];
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск визуального стиля по имени
			/// </summary>
			/// <param name="style_name">Имя стиля</param>
			/// <returns>Найденный стиль или стиль по умолчанию</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GUIStyle FindStyle(String style_name)
			{
				if (String.IsNullOrEmpty(style_name) || style_name == "<none>")
				{
					return GUIStyle.none;
				}

				if (mStyles == null)
				{
					InitStyles();
				}

				for (Int32 i = 0; i < mStyles.Count; i++)
				{
					if (String.Compare(mStyles[i].name, style_name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						return mStyles[i];
					}
				}

				return GUIStyle.none;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса имени стиля в списке доступных стилей
			/// </summary>
			/// <param name="style_name">Имя стиля</param>
			/// <returns>Индекс</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 GetStyleIndex(String style_name)
			{
				String[] names = GetStyleNames();

				for (Int32 i = 0; i < names.Length; i++)
				{
					if (String.Compare(style_name, names[i], true) == 0)
					{
						return i;
					}
					else
					{
						Int32 delimetr = names[i].IndexOf('/');

						// Если есть разделитель
						if (delimetr > -1)
						{
							String style_name_original = names[i].Remove(0, delimetr + 1);

							if (String.Compare(style_name, style_name_original, true) == 0)
							{
								return i;
							}
						}
					}
				}

				return 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получить массива имен доступных визуальных стилей
			/// </summary>
			/// <returns>Массив имен визуальных стилей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String[] GetStyleNames()
			{
				if (mStyleNames == null)
				{
					List<String> names_style = new List<String>();

					names_style.Add("<none>");
					names_style.Add(CurrentSkin.box.name);
					names_style.Add(CurrentSkin.label.name);
					names_style.Add(CurrentSkin.button.name);
					names_style.Add(CurrentSkin.toggle.name);
					names_style.Add(CurrentSkin.horizontalScrollbar.name);
					names_style.Add(CurrentSkin.horizontalSlider.name);
					names_style.Add(CurrentSkin.verticalScrollbar.name);
					names_style.Add(CurrentSkin.verticalSlider.name);
					names_style.Add(CurrentSkin.window.name);
					names_style.Add(CurrentSkin.scrollView.name);
					names_style.Add(CurrentSkin.textField.name);
					names_style.Add(CurrentSkin.textArea.name);

					for (Int32 i = 0; i < CurrentSkin.customStyles.Length; i++)
					{
						String add_name = CurrentSkin.customStyles[i].name;
						if (CurrentSkin.customStyles[i].name.IndexOf("Label") > -1)
						{
							add_name = "Labels/" + CurrentSkin.customStyles[i].name;
						}
						if (CurrentSkin.customStyles[i].name.IndexOf("Text") > -1)
						{
							add_name = "Texts/" + CurrentSkin.customStyles[i].name;
						}
						if (CurrentSkin.customStyles[i].name.IndexOf("Panel") > -1)
						{
							add_name = "Panels/" + CurrentSkin.customStyles[i].name;
						}
						if (CurrentSkin.customStyles[i].name.IndexOf("Button") > -1)
						{
							add_name = "Buttons/" + CurrentSkin.customStyles[i].name;
						}
						if (CurrentSkin.customStyles[i].name.IndexOf("Item") > -1)
						{
							add_name = "Items/" + CurrentSkin.customStyles[i].name;
						}
						if (CurrentSkin.customStyles[i].name.IndexOf("Check") > -1)
						{
							add_name = "Checks/" + CurrentSkin.customStyles[i].name;
						}
						if (CurrentSkin.customStyles[i].name.IndexOf("Radio") > -1)
						{
							add_name = "Checks/" + CurrentSkin.customStyles[i].name;
						}
						if (CurrentSkin.customStyles[i].name.IndexOf("Scroll") > -1)
						{
							add_name = "Scrolls/" + CurrentSkin.customStyles[i].name;
						}
						if (CurrentSkin.customStyles[i].name.IndexOf("Slider") > -1)
						{
							add_name = "Sliders/" + CurrentSkin.customStyles[i].name;
						}

						names_style.Add(add_name);
					}

					mStyleNames = names_style.ToArray();
				}

				return mStyleNames;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени стиля по его индексу в скине
			/// </summary>
			/// <param name="index">Индекс</param>
			/// <returns>Имя стиля</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetStyleNameFromIndex(Int32 index)
			{
				String[] style_names = GetStyleNames();
				String style_name = style_names[index];
				Int32 delimetr = style_name.IndexOf('/');

				// Если есть разделитель
				if (delimetr > -1)
				{
					style_name = style_name.Remove(0, delimetr + 1);
				}

				return style_name;
			}
			#endregion

			#region ======================================= РАБОТА C ЭЛЕМЕНТАМИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Регистрация существующего элемента в диспетчере для рисования
			/// </summary>
			/// <param name="element">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RegisterElement(ILotusBaseElement element)
			{
				element.OnReset();
				Elements.Add(element);
				Elements.Sort();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена регистрации элемента по имени
			/// </summary>
			/// <param name="element_name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UnRegisterElement(String element_name)
			{
				mIsRemoved = true;
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					if (Elements[i].Name == element_name)
					{
						RemovedElements.Add(Elements[i]);
						CGUIBaseElement base_element = Elements[i] as CGUIBaseElement;
						if (base_element != null)
						{
							base_element.mIsRegisterDispatcher = false;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена регистрации элемента
			/// </summary>
			/// <param name="element">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UnRegisterElement(ILotusBaseElement element)
			{
				mIsRemoved = true;
				CGUIBaseElement base_element = element as CGUIBaseElement;
				if (base_element != null)
				{
					base_element.mIsRegisterDispatcher = false;
				}

				RemovedElements.Add(element);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================