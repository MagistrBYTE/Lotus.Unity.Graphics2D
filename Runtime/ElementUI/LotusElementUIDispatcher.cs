//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusElementUIDispatcher.cs
*		Центральный диспетчер модуля компонентов Unity UI.
*		Центральный диспетчер осуществляет управление модулем компонентов Unity UI, обеспечивает создание, поиск и управление
*	элементами, обеспечивает работу с окнами, представляет основные параметры для разработки интерфейса, включая 
*	локализацию и комплексное управление интерфейсом пользователя.
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
		//! \defgroup Unity2DUI МОДУЛЬ КОМПОНЕНТОВ Unity UI
		//! Модуль компонентов Unity UI представляет расширенный набор визуальных компонентов, комплексных элементов 
		//! интерфейса, а также обеспечивает гибкое и удобное управление всеми компонентами пользовательского интерфейса
		//! \ingroup UnityGraphics2D
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер модуля компонентов Unity UI
		/// </summary>
		/// <remarks>
		/// Центральный диспетчер осуществляет управление модулем компонентов Unity UI, обеспечивает создание, поиск и 
		/// управление элементами, обеспечивает работу с окнами, представляет основные параметры для разработки интерфейса, 
		/// включая локализацию и комплексное управление интерфейсом пользователя
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPath + "LotusElementUIDispatcher")]
		[LotusExecutionOrder(100)]
		public class LotusElementUIDispatcher : LotusBaseGraphics2DDispatcher<LotusElementUIDispatcher>,
			ILotusBaseGraphics2DDispatcher
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			// Управление окнами
			internal static Boolean mIsModalPanel;
			internal static Image mUIImageModalPanel;
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ СВОЙСТВА ======================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Основная канва для Unity UI
			/// </summary>
			public static Canvas MainCanvas
			{
				get
				{
					if (Instance.mMainCanvas == null)
					{
						Instance.GetRootCanvas();
					}

					return Instance.mMainCanvas;
				}
			}

			/// <summary>
			/// Компонент для масштабирования элементов Unity UI
			/// </summary>
			public static CanvasScaler CanvasScaler
			{
				get
				{
					if (Instance.mCanvasScaler == null)
					{
						if (Instance.mMainCanvas == null)
						{
							Instance.GetRootCanvas();
						}

						Instance.mCanvasScaler = Instance.mMainCanvas.GetComponent<CanvasScaler>();
					}
					return Instance.mCanvasScaler;
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Canvas mMainCanvas;
			[SerializeField]
			internal CanvasScaler mCanvasScaler;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================

			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация диспетчера при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------

			protected override void ResetDispatcher()
			{

			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void ConstructorDispatcher()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{
				// Инициализируем подсистемe окон
				InitWindow();
			}

#if UNITY_EDITOR
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
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdateDispatcher()
			{

			}

			#endregion

			#region ======================================= МЕТОДЫ ISerializationCallbackReceiver =====================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс перед сериализацией объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnBeforeSerialize()
			{
				//if (mCanvasScaler != null)
				//{
				//	switch (mCanvasScaler.uiScaleMode)
				//	{
				//		case CanvasScaler.ScaleMode.ConstantPixelSize:
				//			break;
				//		case CanvasScaler.ScaleMode.ScaleWithScreenSize:
				//			{
				//				mDesignScreenWidth = (Int32)mCanvasScaler.referenceResolution.x;
				//				mDesignScreenHeight = (Int32)mCanvasScaler.referenceResolution.y;
				//			}
				//			break;
				//		case CanvasScaler.ScaleMode.ConstantPhysicalSize:
				//			break;
				//		default:
				//			break;
				//	}
				//}

				//XSystemGUIDispatcher.mDesignScreenWidth = mDesignScreenWidth;
				//XSystemGUIDispatcher.mDesignScreenHeight = mDesignScreenHeight;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс после сериализацией объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnAfterDeserialize()
			{
				//if (mCanvasScaler != null)
				//{
				//	switch (mCanvasScaler.uiScaleMode)
				//	{
				//		case CanvasScaler.ScaleMode.ConstantPixelSize:
				//			break;
				//		case CanvasScaler.ScaleMode.ScaleWithScreenSize:
				//			{
				//				mDesignScreenWidth = (Int32)mCanvasScaler.referenceResolution.x;
				//				mDesignScreenHeight = (Int32)mCanvasScaler.referenceResolution.y;
				//			}
				//			break;
				//		case CanvasScaler.ScaleMode.ConstantPhysicalSize:
				//			break;
				//		default:
				//			break;
				//	}
				//}

				//XSystemGUIDispatcher.mDesignScreenWidth = mDesignScreenWidth;
				//XSystemGUIDispatcher.mDesignScreenHeight = mDesignScreenHeight;
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ЭЛЕМЕНТАМИ ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента и добавление его к основной канве в качестве дочернего
			/// </summary>
			/// <typeparam name="TElementUI">Тип элемента</typeparam>
			/// <param name="element_name">Имя элемента</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TElementUI CreateElement<TElementUI>(String element_name) where TElementUI : MonoBehaviour
			{
				GameObject go_element = new GameObject(element_name);
				go_element.layer = XLayer.UI_ID;

				go_element.transform.EnsureComponent<RectTransform>();

				TElementUI element = go_element.AddComponent<TElementUI>();

				// Добавляем на канву
				go_element.transform.SetParent(MainCanvas.transform, false);

				return element;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента и добавление его к основной канве в качестве дочернего
			/// </summary>
			/// <typeparam name="TElementUI">Тип элемента</typeparam>
			/// <param name="element_name">Имя элемента</param>
			/// <param name="left">Позиция по X</param>
			/// <param name="top">Позиция по Y</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TElementUI CreateElement<TElementUI>(String element_name, Single left, Single top, 
				Single width, Single height) where TElementUI : MonoBehaviour
			{
				GameObject go_element = new GameObject(element_name);
				go_element.layer = XLayer.UI_ID;

				RectTransform rect = go_element.transform.EnsureComponent<RectTransform>();
				rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, left, width);
				rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, top, height);

				TElementUI element = go_element.AddComponent<TElementUI>();
				
				// Добавляем на канву
				go_element.transform.SetParent(MainCanvas.transform, false);

				rect.Set(left, top, width, height);

				return element;
			}


			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск основной канвы для размещения элементов интерфейса или при необходимости её создание
			/// </summary>
			/// <returns>Основная канва</returns>
			//---------------------------------------------------------------------------------------------------------
			public Canvas GetRootCanvas()
			{
				// Пробуем найти по стандартному имени
				Canvas canvas = XComponentDispatcher.Find<Canvas>(nameof(Canvas));
				if (canvas == null)
				{
					Canvas[] all_canvas = Resources.FindObjectsOfTypeAll<Canvas>();
					if (all_canvas != null && all_canvas.Length > 0)
					{
						canvas = all_canvas[0];
					}
				}

				// Если все еще не найден, то создаем
				if (canvas == null)
				{
					GameObject go_canvas = new GameObject(nameof(Canvas));
					canvas = go_canvas.AddComponent<Canvas>();
					mCanvasScaler = go_canvas.AddComponent<CanvasScaler>();
					mCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
					go_canvas.AddComponent<GraphicRaycaster>();
					canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				}

				mMainCanvas = canvas;
				if (mCanvasScaler == null)
				{
					mCanvasScaler = mMainCanvas.GetComponent<CanvasScaler>();
					mCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
				}

				// Проверка на EventSystem, пробуем найти по стандартному имени
				EventSystem event_system = XComponentDispatcher.Find<EventSystem>(nameof(EventSystem));
				if (event_system == null)
				{
					EventSystem[] all_event_system = Resources.FindObjectsOfTypeAll<EventSystem>();
					if (all_event_system != null && all_event_system.Length > 0)
					{
						event_system = all_event_system[0];
					}
				}

				// Если все еще не найден, то создаем
				if (event_system == null)
				{
					GameObject go_event_system = new GameObject(nameof(EventSystem));
					go_event_system.AddComponent<EventSystem>();
					go_event_system.AddComponent<StandaloneInputModule>();
				}

				return canvas;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительное обновление редактора и сцены для отображения изменений. Только для режима редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void ForceUpdateEditor()
			{
#if UNITY_EDITOR
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					Canvas.ForceUpdateCanvases();
					if (UnityEditor.SceneView.currentDrawingSceneView != null)
					{
						UnityEditor.SceneView.currentDrawingSceneView.Repaint();
					}
				}
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ОКНАМИ ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация подсистемы управления окнами
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void InitWindow()
			{
				// Создаем модальную панель
			   mUIImageModalPanel = XComponentDispatcher.Find<Image>("_ModalPanel");
				if (mUIImageModalPanel == null)
				{
					GameObject modal = new GameObject("_ModalPanel", typeof(Image));
					modal.transform.SetParent(MainCanvas.transform, false);
					modal.transform.SetSiblingIndex(0);
					mUIImageModalPanel = modal.GetComponent<Image>();
					mUIImageModalPanel.rectTransform.Set(0, 0, DesignScreenWidth, DesignScreenHeight);
				}

				if (LotusGraphics2DVisualStyleService.SpriteModalPanel != null)
				{
					mUIImageModalPanel.SetSprite(LotusGraphics2DVisualStyleService.SpriteModalPanel);
				}
				else
				{
					mUIImageModalPanel.SetSprite(LotusGraphics2DVisualStyleService.MaskOpacity20);
				}
				mUIImageModalPanel.enabled = false;

				//List<LotusUIWindowBase> windows = XComponentDispatcher.GetAll<LotusUIWindowBase>();

				//for (Int32 i = 0; i < windows.Count; i++)
				//{
				//	LotusUIWindowBase window = windows[i];
				//	if (window != null)
				//	{
				//		window.IsVisible = false;
				//	}
				//}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показа модельной панели
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void ShowModalPanel()
			{
				if (mIsModalPanel == false)
				{
					mUIImageModalPanel.rectTransform.SetAsLastSibling();
					mUIImageModalPanel.rectTransform.Set(0, 0, DesignScreenWidth, DesignScreenHeight);
					mUIImageModalPanel.enabled = true;
					mIsModalPanel = true;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Скрытие модельной панели
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void HideModalPanel()
			{
				mUIImageModalPanel.rectTransform.SetAsFirstSibling();
				mUIImageModalPanel.enabled = false;
				mIsModalPanel = false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показать простое окно
			/// </summary>
			/// <param name="window_name">Имя окна</param>
			/// <param name="on_close">Обработчик события закрытия окна</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ShowWindow(String window_name, Action on_close = null)
			{
				//LotusUIWindowBase window_base = FindElement<LotusUIWindowBase>(window_name);
				//if (window_base != null)
				//{
				//	window_base.ShowWindow(on_close);
				//}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показать диалоговое окно
			/// </summary>
			/// <param name="window_name">Имя окна</param>
			/// <param name="on_dialog">Обработчик результата события окна</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ShowWindowDialog(String window_name, Action<Int32> on_dialog)
			{
				//LotusUIWindowDialog window_dialog = FindElement<LotusUIWindowDialog>(window_name);
				//if (window_dialog != null)
				//{
				//	window_dialog.ShowDialog(on_dialog);
				//}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показать диалоговое окно
			/// </summary>
			/// <param name="window_name">Имя окна</param>
			/// <param name="is_modal">Статус модальности</param>
			/// <param name="on_dialog">Обработчик результата события окна</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ShowWindowDialog(String window_name, Boolean is_modal, Action<Int32> on_dialog)
			{
				//LotusUIWindowDialog window_dialog = FindElement<LotusUIWindowDialog>(window_name);
				//if (window_dialog != null)
				//{
				//	window_dialog.IsModalState = is_modal;
				//	window_dialog.ShowDialog(on_dialog);
				//}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Показать диалоговое окно процесса
			/// </summary>
			/// <param name="window_name">Имя окна</param>
			/// <param name="on_completed">Обработчик события окончания процесса</param>
			/// <param name="on_abort">Обработчик события прервания процесса</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ShowWindowProcess(String window_name, Action on_completed = null, Action<Single> on_abort = null)
			{
				//LotusUIWindowWaitProcess window_wait = FindElement<LotusUIWindowWaitProcess>(window_name);
				//if (window_wait != null)
				//{
				//	window_wait.ShowProcess(on_completed, on_abort);
				//}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================