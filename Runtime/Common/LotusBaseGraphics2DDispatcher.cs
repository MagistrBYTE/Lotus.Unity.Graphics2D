//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusBaseGraphics2DDispatcher.cs
*		Шаблон паттерна Singleton для базового диспетчера двухмерной графики.
*		Базовый диспетчер двухмерной графики обеспечивает параметры разработки двухмерной графики.
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
using System.Reflection;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для базового диспетчера двухмерной графики
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusBaseGraphics2DDispatcher : ILotusSingleton
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Ширина экрана при разработке
			/// </summary>
			Int32 DesignWidth { get; set; }

			/// <summary>
			/// Высота экрана при разработке
			/// </summary>
			Int32 DesignHeight { get; set; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void ConstructorDispatcher();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void UpdateDispatcher();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблон паттерна Singleton для базового диспетчера двухмерной графики
		/// </summary>
		/// <remarks>
		/// Базовый диспетчер двухмерной графики обеспечивает параметры разработки двухмерной графики
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class LotusBaseGraphics2DDispatcher<TMonoBehaviour> : MonoBehaviour 
			where TMonoBehaviour : MonoBehaviour, ILotusBaseGraphics2DDispatcher
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			// Глобальный экземпляр
			internal static TMonoBehaviour mInstance;

			// Параметры разработки 
			internal static Single mScaledScreenX = 1;
			internal static Single mScaledScreenY = 1;
			internal static Boolean mIsUpdateScaled = false;
			internal static Vector2 mDraggMinOffset = new Vector2(6, 6);
			internal static Single mSizeScrollVertical = 24;
			internal static Single mSizeScrollHorizontal = 24;

			// Параметры состояния
			internal static Boolean mIsPointerOverElement;
#if UNITY_EDITOR
			internal static System.Type mTypeGameView;
			internal static System.Reflection.MethodInfo mGetSizeOfMainGameView;
			internal static Int32 mLastScreenWidth;
			internal static Int32 mLastScreenHeight;
			internal static Int32 mCurrentScreenWidth;
			internal static Int32 mCurrentScreenHeight;
#endif

			/// <summary>
			/// Элементы графического интерфейса
			/// </summary>
			/// <remarks>
			/// Список содержит все зарегистрированные элементы которые будут нарисованы
			/// </remarks>
			public static readonly List<ILotusBaseElement> Elements = new List<ILotusBaseElement>(64);

			/// <summary>
			/// Элементы графического интерфейса которые будут удалены на следующий кадр
			/// </summary>
			/// <remarks>
			/// Мы не можем сразу удалить элементы, мы должные об этом проинформировать диспетчер и только потом удалить
			/// </remarks>
			protected static readonly List<ILotusBaseElement> RemovedElements = new List<ILotusBaseElement>(32);
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ СВОЙСТВА ======================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Глобальный экземпляр
			/// </summary>
			public static TMonoBehaviour Instance
			{
				get
				{
					if (mInstance == null)
					{
						CreateInstance();
						mInstance.ConstructorDispatcher();
					}

					return mInstance;
				}
			}

			//
			// ПАРАМЕТРЫ РАЗРАБОТКИ
			//
			/// <summary>
			/// Текущая ширина экрана
			/// </summary>
			public static Single ScreenWidth
			{
				get
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						if (mCurrentScreenWidth != 0)
						{
							return mCurrentScreenWidth;
						}
						else
						{
							return ((Int32)GetSizeOfMainGameView().x);
						}
					}
					else
					{
						return ((Int32) GetSizeOfMainGameView().x);
					}
#else
					return (Screen.width);
#endif
				}
			}

			/// <summary>
			/// Текущая высота экрана
			/// </summary>
			public static Single ScreenHeight
			{
				get
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						if (mCurrentScreenHeight != 0)
						{
							return mCurrentScreenHeight;
						}
						else
						{
							return ((Int32)GetSizeOfMainGameView().y);
						}
					}
					else
					{
						return ((Int32)GetSizeOfMainGameView().y);
					}
#else
					return (Screen.height);
#endif
				}
			}

			/// <summary>
			/// Ширина экрана при разработке
			/// </summary>
			public static Int32 DesignScreenWidth
			{
				get
				{
					return (Instance.DesignWidth);
				}
			}

			/// <summary>
			/// Высота экрана при разработке
			/// </summary>
			public static Int32 DesignScreenHeight
			{
				get
				{
					return (Instance.DesignHeight);
				}
			}

			/// <summary>
			/// Масштаб ширины экрана по отношению к ширине экрана при разработке
			/// </summary>
			public static Single ScaledScreenX
			{
				get
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						return mScaledScreenX;
					}
					else
					{
						return 1.0f;
					}
#else
					return (mScaledScreenX);
#endif
				}
			}

			/// <summary>
			/// Масштаб высоты экрана по отношению к высоте экрана при разработке
			/// </summary>
			public static Single ScaledScreenY
			{
				get
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						return mScaledScreenY;
					}
					else
					{
						return 1.0f;
					}
#else
					return (mScaledScreenY);
#endif
				}
			}

			/// <summary>
			/// Средний коэффициент масштаба
			/// </summary>
			public static Single ScaledScreenAverage
			{
				get
				{
					return (ScaledScreenX + ScaledScreenY) / 2;
				}
			}

			/// <summary>
			/// Статус обновления масштаба экрана в режиме разработке
			/// </summary>
			public static Boolean IsUpdateScaled
			{
				get
				{
					return (mIsUpdateScaled);
				}
			}

			/// <summary>
			/// Минимальное смещение после которого активируется режим перетаскивания
			/// </summary>
			public static Vector2 DraggMinOffset
			{
				get { return mDraggMinOffset; }
				set { mDraggMinOffset = value; }
			}

			/// <summary>
			/// Размер(ширина) вертикальной полосы прокрутки
			/// </summary>
			public static Single SizeScrollVertical
			{
				get { return mSizeScrollVertical; }
				set { mSizeScrollVertical = value; }
			}

			/// <summary>
			/// Размер(высота) горизонтальной полосы прокрутки
			/// </summary>
			public static Single SizeScrollHorizontal
			{
				get { return mSizeScrollHorizontal; }
				set { mSizeScrollHorizontal = value; }
			}

			//
			// ПАРАМЕТРЫ СОСТОЯНИЯ
			//
			/// <summary>
			/// Указатель находится над на элементом
			/// </summary>
			/// <remarks>
			/// Истинное значение указывает что данные поступающие от ввода распространяются
			/// на элемент и их не надо использовать для других целей
			/// </remarks>
			public static Boolean IsPointerOverElement
			{
				get { return mIsPointerOverElement; }
			}

			//
			// ПАРАМЕТРЫ ПЕРЕМЕЩЕНИЯ
			//
			/// <summary>
			/// Положение указателя в экранных координатах при перемещении элемента когда была нажата левая кнопка мыши
			/// </summary>
			public static Vector2 PointerDraggLeftDownScreen;

			/// <summary>
			/// Положение указателя в экранных координатах в режиме разработке при перемещении элемента когда была нажата левая кнопка мыши
			/// </summary>
			public static Vector2 PointerDraggLeftDownDesign;

			/// <summary>
			/// Текущий перетаскиваемый элемент
			/// </summary>
			public static ILotusElement DraggingElement;

			/// <summary>
			/// Предыдущий перетаскиваемый элемент
			/// </summary>
			public static ILotusElement DraggingElementPrev;

			//
			// ПАРАМЕТРЫ УКАЗАТЕЛЯ В РЕЖИМЕ РАЗРАБОТКЕ
			//
			/// <summary>
			/// Положение указателя в экранных координатах в режиме разработке
			/// </summary>
			public static Vector2 PositionPointer;

			/// <summary>
			/// Смещение указателя в режиме разработке (Стандартные экранные координаты)
			/// </summary>
			public static Vector2 DeltaPointer;

			/// <summary>
			/// Смещение указателя в режиме разработке (Перевёрнутые экранные координаты)
			/// </summary>
			public static Vector2 DeltaPointerInv;
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение размера окна игры редактора
			/// </summary>
			/// <returns>Размера окна</returns>
			//---------------------------------------------------------------------------------------------------------
			internal static Vector2 GetSizeOfMainGameView()
			{
#if UNITY_EDITOR
				if (mTypeGameView == null)
				{
					mTypeGameView = Type.GetType("UnityEditor.GameView,UnityEditor");
				}

				if (mGetSizeOfMainGameView == null)
				{
					mGetSizeOfMainGameView = mTypeGameView.GetMethod("GetSizeOfMainGameView", BindingFlags.NonPublic | BindingFlags.Static);
				}

				return (Vector2)mGetSizeOfMainGameView.Invoke(null, null);

#else
				return (new Vector2(Screen.width, Screen.height));
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка глобального экземпляра
			/// </summary>
			/// <param name="instances">Массив экземпляров</param>
			//---------------------------------------------------------------------------------------------------------
			protected static void SetInstance(TMonoBehaviour[] instances)
			{
				for (Int32 i = 0; i < instances.Length; i++)
				{
					// Если это не префаб
					if (!instances[i].gameObject.IsPrefab())
					{
						// Если это не основной глобальный экземпляр до мы его удаляем
						if (instances[i].IsMainInstance)
						{
							mInstance = instances[i];
						}
						else
						{
							String name_mono = typeof(TMonoBehaviour).Name;
							switch (instances[i].DestroyMode)
							{
								case TSingletonDestroyMode.None:
									{
										Debug.LogWarningFormat("Instance: {0} > 1, object: {1}", name_mono,
											instances[i].gameObject.name);
									}
									break;
								case TSingletonDestroyMode.GameObject:
									{
										Debug.LogWarningFormat("Instance: {0} > 1, delete the object: {1}", name_mono,
											instances[i].gameObject.name);

										Destroy(instances[i].gameObject);
									}
									break;
								case TSingletonDestroyMode.Component:
									{
										Debug.LogWarningFormat("Instance: {0} > 1, delete the component: {1}", name_mono,
											instances[i].gameObject.name);

										Destroy(instances[i]);
									}
									break;
								default:
									break;
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание глобального экземпляра
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected static void CreateInstance()
			{
				// Если глобальный экземпляр пустой
				if (mInstance == null)
				{
					// Получаем все компоненты данного типа
					TMonoBehaviour[] instances = UnityEngine.Object.FindObjectsOfType<TMonoBehaviour>();
					if (instances.Length == 1)
					{
						// Все хорошо
						mInstance = instances[0];
						return;
					}
					else
					{
						// Почему-то компонентов много, мы не берем случай когда это сделано пользователем.
						// Такой случай может быть при загрузки другой сцены
						if (instances.Length > 1)
						{
							SetInstance(instances);
						}
					}
				}

				// Глобальный экземпляр мы до сих пор не нашли
				// Рассмотрим вариант когда игровой объект выключен
				if (mInstance == null)
				{
					// Получаем все компоненты данного типа
					TMonoBehaviour[] instances = UnityEngine.Resources.FindObjectsOfTypeAll<TMonoBehaviour>();
					if (instances.Length == 1)
					{
						// Все хорошо
						mInstance = instances[0];
						return;
					}
					else
					{
						// Почему-то компонентов много, мы не берем случай когда это сделано пользователем.
						// Такой случай может быть при загрузки другой сцены
						if (instances.Length > 1)
						{
							for (Int32 i = 0; i < instances.Length; i++)
							{
								SetInstance(instances);
							}
						}
					}
				}

				// Ну значит его точно нет
				if (mInstance == null)
				{
					Type type_singleton = typeof(TMonoBehaviour);
					GameObject go = new GameObject(type_singleton.Name.Replace("Lotus", ""), type_singleton);
					mInstance = go.GetComponent<TMonoBehaviour>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка и удаление дубликатов глобального экземпляра
			/// </summary>
			/// <remarks>
			/// Во время разработки глобальные экземпляры являются таковыми только в пределах активной сцены.
			/// Однако если использовать методику при которой есть главной сцена и второстепенные сцены загружаемые/выгружаемые по мере
			/// необходимости может возникнуть ситуация, когда в приложение работают сразу несколько экземпляров.
			/// Это неправильно. Для этого данный метод удаляет все дубликаты и оставляет только один - основной экземпляр 
			/// свойство которого <see cref="LotusBaseGraphics2DDispatcher.IsMainInstance"/> равно True
			/// </remarks>
			/// <returns>Статус удаления дубликата</returns>
			//---------------------------------------------------------------------------------------------------------
			protected static Boolean CheckDublicate()
			{
				// Получаем все компоненты данного типа
				TMonoBehaviour[] instances = UnityEngine.Object.FindObjectsOfType<TMonoBehaviour>();
				if (instances.Length == 1)
				{
					// Все хорошо
					mInstance = instances[0];
					return false;
				}
				else
				{
					// Почему-то компонентов много, мы не берем случай когда это сделано пользователем.
					// Такой случай может быть при загрузки другой сцены
					if (instances.Length > 1)
					{
						SetInstance(instances);

						return true;
					}

					return false;
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsMainInstance;
			[SerializeField]
			internal TSingletonDestroyMode mDestroyMode;
			[SerializeField]
			internal Boolean mIsDontDestroy;

			// Параметры разработки
			[SerializeField]
			internal Int32 mDesignScreenWidth;
			[SerializeField]
			internal Int32 mDesignScreenHeight;
			#endregion

			#region ======================================= СВОЙСТВА ILotusSingleton ==================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус основного экземпляра
			/// </summary>
			public Boolean IsMainInstance
			{
				get
				{
					return mIsMainInstance;
				}
				set
				{
					mIsMainInstance = value;
				}
			}

			/// <summary>
			/// Статус удаления игрового объекта
			/// </summary>
			/// <remarks>
			/// При дублировании будет удалятся либо непосредственного игровой объект либо только компонент
			/// </remarks>
			public TSingletonDestroyMode DestroyMode
			{
				get
				{
					return mDestroyMode;
				}
				set
				{
					mDestroyMode = value;
				}
			}

			/// <summary>
			/// Не удалять объект когда загружается новая сцена
			/// </summary>
			public Boolean IsDontDestroy
			{
				get
				{
					return mIsDontDestroy;
				}
				set
				{
					mIsDontDestroy = value;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ILotusBaseGraphics2DDispatcher ===================
			//
			// ПАРАМЕТРЫ РАЗРАБОТКИ
			//
			/// <summary>
			/// Ширина экрана при разработке
			/// </summary>
			virtual public Int32 DesignWidth
			{
				get { return (mDesignScreenWidth); }
				set { mDesignScreenWidth = value; }
			}

			/// <summary>
			/// Высота экрана при разработке
			/// </summary>
			virtual public Int32 DesignHeight
			{
				get { return (mDesignScreenHeight); }
				set { mDesignScreenHeight = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация диспетчера при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void ResetDispatcher()
			{
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
#if UNITY_EDITOR
				ResetDispatcher();
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				// Считаем масштаб
				if (mDesignScreenWidth == 0) mDesignScreenWidth = 1280;
				if (mDesignScreenHeight == 0) mDesignScreenHeight = 800;

#if UNITY_EDITOR
				if (mCurrentScreenWidth == 0 || mCurrentScreenHeight == 0)
				{
					Vector2 res = GetSizeOfMainGameView();

					// Текущий размер экрана
					mCurrentScreenWidth = mLastScreenWidth = (Int32)res.x;
					mCurrentScreenHeight = mLastScreenHeight = (Int32)res.y;

					// Cчитаем масштаб
					mScaledScreenX = (Single)mLastScreenWidth / (Single)mDesignScreenWidth;
					mScaledScreenY = (Single)mLastScreenHeight / (Single)mDesignScreenHeight;
				}
				else
				{
					// Повторно считаем масштаб
					mScaledScreenX = (Single)mLastScreenWidth / (Single)mDesignScreenWidth;
					mScaledScreenY = (Single)mLastScreenHeight / (Single)mDesignScreenHeight;
				}
#else
				// Масштаб
				mScaledScreenX = (Single)Screen.width / (Single)mDesignScreenWidth;
				mScaledScreenY = (Single)Screen.height / (Single)mDesignScreenHeight;
#endif

				// Первичная инициализация
				if (!CheckDublicate())
				{
					ConstructorDispatcher();
					ChangeSizeScreenDispatcher();
				}

				if (mIsDontDestroy)
				{
					GameObject.DontDestroyOnLoad(this.gameObject);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
#if UNITY_EDITOR
				// Получаем текущий размер экрана
				Vector2 res = GetSizeOfMainGameView();
				mCurrentScreenWidth = (Int32)res.x;
				mCurrentScreenHeight = (Int32)res.y;

				// Если он не равен предыдущему
				mIsUpdateScaled = false;
				if (mCurrentScreenWidth != mLastScreenWidth || mCurrentScreenHeight != mLastScreenHeight)
				{
					// Обновляем
					mLastScreenWidth = mCurrentScreenWidth;
					mLastScreenHeight = mCurrentScreenHeight;

					// Повторно считаем масштаб
					mScaledScreenX = (Single)mLastScreenWidth / (Single)mDesignScreenWidth;
					mScaledScreenY = (Single)mLastScreenHeight / (Single)mDesignScreenHeight;
					mIsUpdateScaled = true;

					// Обновляем размеры экрана
					ChangeSizeScreenDispatcher();
				}
#endif
				UpdateDispatcher();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр после (после Update)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void LateUpdate()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				Vector2 mouse_point = Input.mousePosition;
				mouse_point.x = (mouse_point.x) / ScaledScreenX;
				mouse_point.y = (Screen.height - mouse_point.y) / ScaledScreenY;
				DeltaPointer = mouse_point - PositionPointer;
				PositionPointer = mouse_point;
#else
				if (Input.touchCount > 0)
				{
					Vector2 touch_point = Input.touches[0].position;
					touch_point.y = (touch_point.x) / ScaledScreenX;
					touch_point.y = (Screen.height - touch_point.y) / ScaledScreenY;
					PositionPointer = touch_point;
					DeltaPointer = Input.touches[0].deltaPosition;
					DeltaPointer.x = DeltaPointer.x / ScaledScreenX;
					DeltaPointer.y = DeltaPointer.y / ScaledScreenY;
				}
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBaseGraphics2DDispatcher =====================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ConstructorDispatcher()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение размеров экрана
			/// </summary>
			/// <remarks>
			/// Метод вызывается одни раз в стартовом методе, и в режиме редактора каждый раз когда меняются размеры экрана
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void ChangeSizeScreenDispatcher()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление диспетчера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdateDispatcher()
			{

			}
			#endregion

			#region ======================================= РАБОТА C ЭЛЕМЕНТАМИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на статус регистрации элемента
			/// </summary>
			/// <param name="element">Элемент</param>
			/// <returns>Cтатус регистрации элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsRegister(ILotusBaseElement element)
			{
				return (Elements.Contains(element));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление существующего элемента к списку элементов диспечера
			/// </summary>
			/// <param name="element">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void AddElement(ILotusBaseElement element)
			{
				if (element != null)
				{
					if (Elements.Contains(element) == false)
					{
						Elements.Add(element);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление существующего элемента из списка элементов диспечера
			/// </summary>
			/// <param name="element">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveElement(ILotusBaseElement element)
			{
				if (element != null)
				{
					if (Elements.Contains(element))
					{
						Elements.Remove(element);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление существующего элемента из списка элементов диспечера
			/// </summary>
			/// <param name="element_name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveElement(String element_name)
			{
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					if (Elements[i].Name == element_name)
					{
						Elements.RemoveAt(i);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сортировка элементов по приоритету рисования
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void SortElements()
			{
				Elements.Sort();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение элемента по имени
			/// </summary>
			/// <param name="element_name">Имя элемента</param>
			/// <returns>Найденный элемент или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static ILotusBaseElement GetElement(String element_name)
			{
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					if (Elements[i].Name == element_name)
					{
						return (Elements[i]);
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск определенного типа элемента по имени
			/// </summary>
			/// <typeparam name="TElementUI">Тип элемента</typeparam>
			/// <param name="element_name">Имя элемента</param>
			/// <returns>Найденный элемент или null если элемент найти не удалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TElementUI FindElement<TElementUI>(String element_name) where TElementUI : class, ILotusBaseElement
			{
				TElementUI element = default(TElementUI);
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					if (Elements[i] != null)
					{
						if (Elements[i].Name == element_name)
						{
							element = Elements[i] as TElementUI;
							break;
						}
					}
				}

				return element;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск определенного типа элемента по вхождению точки в экранных координатах в его область
			/// </summary>
			/// <typeparam name="TElementUI">Тип элемента</typeparam>
			/// <param name="position">Позиция точки в экранных координатах</param>
			/// <returns>Найденный элемент или null если элемент найти не удалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TElementUI FindElementFromScreenPosition<TElementUI>(Vector2 position) where TElementUI : class, ILotusBaseElement
			{
				TElementUI element = null;
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					if (Elements[i] != null)
					{
						if (Elements[i].ContainsScreen(position) && Elements[i] is TElementUI)
						{
							element = Elements[i] as TElementUI;
							break;
						}
					}
				}

				return element;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск определенного типа элемента по вхождению точки в перевёрнутых экранных координатах в его область
			/// </summary>
			/// <remarks>
			/// Позиция точки должна быть инвертирована, то есть начала координат по Y внизу экрана
			/// Такая позиция передаётся стандартными аргументами событий подсистемы EventSystems, например <see cref="PointerEventData"/>
			/// </remarks>
			/// <typeparam name="TElementUI">Тип элемента</typeparam>
			/// <param name="position">Позиция точки в перевёрнутых экранных координатах</param>
			/// <returns>Найденный элемент или null если элемент найти не удалось</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TElementUI FindElementFromScreenInvertedYPosition<TElementUI>(Vector2 position) where TElementUI : class, ILotusPlaceable2D
			{
				TElementUI element = null;
				//if (mElements != null)
				//{
				//	for (Int32 i = 0; i < mElements.Count; i++)
				//	{
				//		if (mElements[i] != null)
				//		{
				//			if (mElements[i].CheckContainsPointWorldScreenInv(position) && mElements[i] is TElementUI)
				//			{
				//				element = mElements[i] as TElementUI;
				//				break;
				//			}
				//		}
				//	}
				//}

				return element;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление состояние элементов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateElements()
			{
#if UNITY_EDITOR
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					return;
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление визуальных стилей элементов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateElementsVisualStyle()
			{
#if UNITY_EDITOR
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					//
					// TODO
					//
					//List<ILotusVisualStyle> list = XComponentDispatcher.GetAll<ILotusVisualStyle>();
					//if (list.Count > 0)
					//{
					//	for (Int32 i = 0; i < list.Count; i++)
					//	{
					//		if (list[i] != null)
					//		{
					//			list[i].UpdateVisualStyle();
					//		}
					//	}
					//}
				}
#endif

				for (Int32 i = 0; i < Elements.Count; i++)
				{
					if (Elements[i] != null)
					{
						ILotusVisualStyle element = Elements[i] as ILotusVisualStyle;
						if (element != null)
						{
							element.UpdateVisualStyle();
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных элементов.(Например после загрузки базы локализации)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateElementsData()
			{
#if UNITY_EDITOR
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					//
					// TODO
					//
					//List<ILotusDataExchange> list = XComponentDispatcher.GetAllInterface<ILotusUIDataExchange>();
					//if (list.Count > 0)
					//{
					//	for (Int32 i = 0; i < list.Count; i++)
					//	{
					//		if (list[i] != null)
					//		{
					//			list[i].UpdateData();
					//		}
					//	}
					//}
				}
#endif
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					if (Elements[i] != null)
					{
						ILotusDataExchange element = Elements[i] as ILotusDataExchange;
						if (element != null)
						{
							//element.UpdateData();
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление позиции элементов имеющих указанный родительский элемент
			/// </summary>
			/// <param name="parent_element">Родительский элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void FromParentComputePositionElements(ILotusElement parent_element)
			{
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					ILotusElement element = Elements[i] as ILotusElement;

					if (element != null && element.IParent == parent_element)
					{
						element.UpdatePlacement();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установление статуса видимости элементов имеющих указанный родительский элемент
			/// </summary>
			/// <param name="parent_element">Родительский элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void FromParentSetVisibleElements(ILotusElement parent_element)
			{
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					ILotusElement element = Elements[i] as ILotusElement;

					if (element != null && element.IParent == parent_element)
					{
						element.SetVisibleElement();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установление статуса доступности элементов имеющих указанный родительский элемент
			/// </summary>
			/// <param name="parent_element">Родительский элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void FromParentSetEnabledElements(ILotusElement parent_element)
			{
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					ILotusElement element = Elements[i] as ILotusElement;

					if (element != null && element.IParent == parent_element)
					{
						element.SetEnabledElement();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование элементов имеющих указанный родительский элемент
			/// </summary>
			/// <param name="parent_element">Родительский элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void FromParentDrawElements(ILotusElement parent_element)
			{
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					ILotusElement element = Elements[i] as ILotusElement;

					if (element != null && element.IsVisible && element.IParent == parent_element)
					{
						element.OnDraw();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса порядка рисования первого элемента от указанного родительского элемента
			/// </summary>
			/// <param name="parent_element">Родительский элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetFirstDepthFromParent(ILotusElement parent_element)
			{
				Int32 first = 10000;
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					ILotusElement element = Elements[i] as ILotusElement;

					if (element != null && element.IsVisible && element.IParent == parent_element)
					{
						if (element.Depth < first)
						{
							first = element.Depth;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса порядка рисования последнего элемента от указанного родительского элемента
			/// </summary>
			/// <param name="parent_element">Родительский элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetLastDepthFromParent(ILotusElement parent_element)
			{
				Int32 first = -10000;
				for (Int32 i = 0; i < Elements.Count; i++)
				{
					ILotusElement element = Elements[i] as ILotusElement;

					if (element != null && element.IsVisible && element.IParent == parent_element)
					{
						if (element.Depth > first)
						{
							first = element.Depth;
						}
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