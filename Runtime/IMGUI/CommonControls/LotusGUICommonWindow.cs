//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUICommonWindow.cs
*		Элементы интерфейса пользователя представляющего собой окна.
*		Реализация элементов интерфейса пользователя представляющего собой окна - панель с заголовком которую можно
*	перемещать.
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
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DImmedateGUIControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс окна с простой надписью
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Window.
		/// Окно поддерживает перетаскивание и режим модальности
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIBaseWindow : CGUIPanelHeader
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			protected static readonly GUIContent ContentClose = new GUIContent("X");
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsModalState;
			[SerializeField]
			internal Boolean mIsDraggable;
			[SerializeField]
			internal Boolean mIsDragHeader;

			// События
			[NonSerialized]
			internal Action mOnClose;

			// Служебные данные
			[NonSerialized]
			internal Int32 mID;
			[NonSerialized]
			internal Rect mRectWorldScreenButtonClose;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Модальное состояние окна
			/// </summary>
			public Boolean IsModalState
			{
				get { return mIsModalState; }
				set { mIsModalState = value; }
			}

			/// <summary>
			/// Возможность перетаскивания окна
			/// </summary>
			public Boolean IsDraggable
			{
				get { return mIsDraggable; }
				set { mIsDraggable = value; }
			}

			/// <summary>
			/// Перетаскивание окна осуществляется только за заголовок
			/// </summary>
			public Boolean IsDragHeader
			{
				get { return mIsDragHeader; }
				set { mIsDragHeader = value; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о закрытии окна
			/// </summary>
			public Action OnClose
			{
				get { return mOnClose; }
				set
				{
					mOnClose = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseWindow()
				: base()
			{
				mHeaderLocation = THeaderLocation.TopSide;
				mStyleMainName = "Box";
				mStyleHeaderName = "Window";
				mRectLocalDesignMain.width = 300;
				mRectLocalDesignMain.height = 350;
				mID = this.GetHashCode();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseWindow(String name)
				: base(name)
			{
				mHeaderLocation = THeaderLocation.TopSide;
				mStyleMainName = "Box";
				mStyleHeaderName = "Window";
				mRectLocalDesignMain.width = 300;
				mRectLocalDesignMain.height = 350;
				mID = this.GetHashCode();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIBaseWindow(String name, Single x, Single y)
				: base(name, x, y)
			{
				mHeaderLocation = THeaderLocation.TopSide;
				mStyleMainName = "Box";
				mStyleHeaderName = "Window";
				mRectLocalDesignMain.width = 300;
				mRectLocalDesignMain.height = 350;
				mID = this.GetHashCode();
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
			public CGUIBaseWindow(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mHeaderLocation = THeaderLocation.TopSide;
				mStyleMainName = "Box";
				mStyleHeaderName = "Window";
				mRectLocalDesignMain.width = 300;
				mRectLocalDesignMain.height = 350;
				mID = this.GetHashCode();
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBasePlaceable2D ==============================
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
				if (!mIsDraggable)
				{
					UpdatePlacementBase();
				}
				else
				{
					switch (mAspectMode)
					{
						case TAspectMode.None:
							{
								mRectWorldScreenMain = mRectLocalDesignMain;
							}
							break;
						case TAspectMode.Proportional:
							{
								mRectWorldScreenMain.x = mRectLocalDesignMain.x * LotusGUIDispatcher.ScaledScreenX;
								mRectWorldScreenMain.y = mRectLocalDesignMain.y * LotusGUIDispatcher.ScaledScreenY;
								mRectWorldScreenMain.width = mRectLocalDesignMain.width * LotusGUIDispatcher.ScaledScreenX;
								mRectWorldScreenMain.height = mRectLocalDesignMain.height * LotusGUIDispatcher.ScaledScreenY;
							}
							break;
						case TAspectMode.WidthControlsHeight:
							{
								mRectWorldScreenMain.x = mRectLocalDesignMain.x * LotusGUIDispatcher.ScaledScreenX;
								mRectWorldScreenMain.y = mRectLocalDesignMain.y * LotusGUIDispatcher.ScaledScreenX;
								mRectWorldScreenMain.width = mRectLocalDesignMain.width * LotusGUIDispatcher.ScaledScreenX;
								mRectWorldScreenMain.height = mRectLocalDesignMain.height * LotusGUIDispatcher.ScaledScreenX;
							}
							break;
						case TAspectMode.HeightControlsWidth:
							{
								mRectWorldScreenMain.x = mRectLocalDesignMain.x * LotusGUIDispatcher.ScaledScreenY;
								mRectWorldScreenMain.y = mRectLocalDesignMain.y * LotusGUIDispatcher.ScaledScreenY;
								mRectWorldScreenMain.width = mRectLocalDesignMain.width * LotusGUIDispatcher.ScaledScreenY;
								mRectWorldScreenMain.height = mRectLocalDesignMain.height * LotusGUIDispatcher.ScaledScreenY;
							}
							break;
						default:
							break;
					}
				}

				// Заголовок (только сверху)
				mRectWorldScreenHeader.x = PaddingLeft;
				mRectWorldScreenHeader.y = PaddingTop;
				mRectWorldScreenHeader.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
				mRectWorldScreenHeader.height = StyleHeader.border.top;

				// Основное поле
				mRectWorldScreenContent.x = PaddingLeft;
				mRectWorldScreenContent.y = StyleHeader.border.top;
				mRectWorldScreenContent.width = mRectWorldScreenMain.width - (PaddingLeft + PaddingRight);
				mRectWorldScreenContent.height = mRectWorldScreenMain.height - (StyleHeader.border.top + PaddingBottom);

				// Кнопка закрыть
				UpdateButtonClosePlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление размера и положения кнопки закрыть
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void UpdateButtonClosePlacement()
			{
				// Кнопка закрыть
				mRectWorldScreenButtonClose.y = 2;
				mRectWorldScreenButtonClose.x = mRectWorldScreenMain.width - StyleHeader.border.top + 2;
				mRectWorldScreenButtonClose.width = StyleHeader.border.top - 4;
				mRectWorldScreenButtonClose.height = StyleHeader.border.top - 4;
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusElement ======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента в качестве дочернего
			/// </summary>
			/// <remarks>
			/// Метод не следует вызывать напрямую
			/// </remarks>
			/// <param name="child">Дочерний элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetChildren(ILotusElement child)
			{
				if (child != null)
				{
					mCountChildren++;
					child.SetVisibilityFlags(1);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена установка элемента в качестве дочернего
			/// </summary>
			/// <remarks>
			/// Метод не следует вызывать напрямую
			/// </remarks>
			/// <param name="child">Дочерний элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public override void UnsetChildren(ILotusElement child)
			{
				if (child != null)
				{
					if (mCountChildren > 0)
					{
						mCountChildren--;
						child.ClearVisibilityFlags(1);
					}
					else
					{
						Debug.LogError("Count children == 0");
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка родительского элемента
			/// </summary>
			/// <remarks>
			/// При абсолютной позиции элемент не меняет своего местоположения относительно экрана
			/// </remarks>
			/// <param name="parent">Родительский элемент</param>
			/// <param name="absolute_pos">Абсолютная позиция элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetParent(ILotusElement parent, Boolean absolute_pos)
			{
				if (parent == null)
				{
					UpdatePlacement();
					mParent.UnsetChildren(this);
					mParent = null;
				}
				else
				{
					if (absolute_pos)
					{

					}
					else
					{
						mParent = parent;
						UpdatePlacement();
						if (mDepth <= mParent.Depth)
						{
							Depth = mParent.Depth + 1;
						}
					}

					mParent.SetChildren(this);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение области для размещения дочерних элементов
			/// </summary>
			/// <returns>Прямоугольник области для размещения дочерних элементов</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Rect GetChildRectContent()
			{
				return mRectWorldScreenContent;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Служебный метод для рисования содержимого окна
			/// </summary>
			/// <param name="id">Идентификатор окна</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnDrawWindow(Int32 id)
			{
				// Основное поле
				GUI.enabled = IsEnabledElement;

				// Кнопка закрыть
				if (GUI.Button(mRectWorldScreenButtonClose, ContentClose))
				{
					LotusGUIDispatcher.UnRegisterElement(mName);

					if (mOnClose != null) mOnClose();
				}

				// Текст
				GUI.Label(mRectWorldScreenContent, mContentText.Text, mStyleContent);

				if (mIsDraggable)
				{
					if (mIsDragHeader)
					{
						GUI.DragWindow(new Rect(0, 0, mRectWorldScreenHeader.width, mRectWorldScreenHeader.height));
					}
					else
					{
						GUI.DragWindow();
					}
				}
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

				LotusGUIDispatcher.CurrentContent.text = mHeaderText.Text;
				LotusGUIDispatcher.CurrentContent.image = mHeaderIcon;

				if (mIsModalState)
				{
					if (mIsDraggable)
					{
						mRectWorldScreenMain = GUI.ModalWindow(mID, mRectWorldScreenMain, OnDrawWindow, LotusGUIDispatcher.CurrentContent, mStyleHeader);
						if (Event.current.type == EventType.MouseDrag)
						{
							UpdatePlacementFromAbsolute();
						}
					}
					else
					{
						GUI.ModalWindow(mID, mRectWorldScreenMain, OnDrawWindow, LotusGUIDispatcher.CurrentContent, mStyleHeader);
					}
				}
				else
				{
					if (mIsDraggable)
					{
						mRectWorldScreenMain = GUI.Window(mID, mRectWorldScreenMain, OnDrawWindow, LotusGUIDispatcher.CurrentContent, mStyleHeader);
						if (Event.current.type == EventType.MouseDrag)
						{
							UpdatePlacementFromAbsolute();
						}
					}
					else
					{
						GUI.Window(mID, mRectWorldScreenMain, OnDrawWindow, LotusGUIDispatcher.CurrentContent, mStyleHeader);
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
				return MemberwiseClone() as CGUIBaseWindow;
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

				CGUIBaseWindow base_window = base_element as CGUIBaseWindow;
				if (base_window != null)
				{
					mIsModalState = base_window.mIsModalState;
					mIsDraggable = base_window.mIsDraggable;
					mIsDragHeader = base_window.mIsDragHeader;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Окно с двумя кнопками
		/// </summary>
		/// <remarks>
		/// Для рисования используется метод GUI.Window.
		/// Реализация простого диалогового окна с двумя кнопками
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CGUIDialogWindow : CGUIBaseWindow
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mOkText;
			[SerializeField]
			internal Texture2D mOkIcon;
			[SerializeField]
			internal String mCancelText;
			[SerializeField]
			internal Texture2D mCancelIcon;

			// События
			internal Action mOnOK;
			internal Action mOnCancel;

			// Служебные данные
			internal Rect mRectWorldScreenButtonOk;
			internal Rect mRectWorldScreenButtonCancel;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Текст кнопки OK
			/// </summary>
			public String OkText
			{
				get { return mOkText; }
				set { mOkText = value; }
			}

			/// <summary>
			/// Текстура изображения иконки кнопки OK
			/// </summary>
			public Texture2D OkIcon
			{
				get { return mOkIcon; }
				set { mOkIcon = value; }
			}

			/// <summary>
			/// Текст кнопки Cancel
			/// </summary>
			public String CancelText
			{
				get { return mCancelText; }
				set { mCancelText = value; }
			}

			/// <summary>
			/// Текстура изображения иконки кнопки Cancel
			/// </summary>
			public Texture2D CancelIcon
			{
				get { return mCancelIcon; }
				set { mCancelIcon = value; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о нажатие на кнопку OK
			/// </summary>
			public Action OnOK
			{
				get { return mOnOK; }
				set
				{
					mOnOK = value;
				}
			}

			/// <summary>
			/// Событие для нотификации о нажатие на кнопку Cancel
			/// </summary>
			public Action OnCancel
			{
				get { return mOnCancel; }
				set
				{
					mOnCancel = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDialogWindow()
				: base()
			{
				mOkText = "OK";
				mCancelText = "Cancel";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDialogWindow(String name)
				: base(name)
			{
				mOkText = "OK";
				mCancelText = "Cancel";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента</param>
			/// <param name="x">Позиция элемента по X в экранных координатах</param>
			/// <param name="y">Позиция элемента по Y в экранных координатах</param>
			//---------------------------------------------------------------------------------------------------------
			public CGUIDialogWindow(String name, Single x, Single y)
				: base(name, x, y)
			{
				mOkText = "OK";
				mCancelText = "Cancel";
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
			public CGUIDialogWindow(String name, Single x, Single y, String text)
				: base(name, x, y, text)
			{
				mOkText = "OK";
				mCancelText = "Cancel"; 
			}
			#endregion

			#region ======================================= МЕТОДЫ ILotusBasePlaceable2D ==============================
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
				base.UpdatePlacement();

				UpdateButtonDialogPlacement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление размера и положения кнопок диалога
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void UpdateButtonDialogPlacement()
			{
				// Размещаем кнопки
				Single patr_10x = mRectWorldScreenMain.width / 10;
				Single patr_10y = mRectWorldScreenMain.height / 10;

				// Кнопка OK
				mRectWorldScreenButtonOk.x = patr_10x;
				mRectWorldScreenButtonOk.y = patr_10y * 8;
				mRectWorldScreenButtonOk.width = patr_10x * 3;
				mRectWorldScreenButtonOk.height = Mathf.Clamp(patr_10y * 1.2f, 30, 70);

				// Кнопка Cancel
				mRectWorldScreenButtonCancel.x = patr_10x * 6;
				mRectWorldScreenButtonCancel.y = patr_10y * 8;
				mRectWorldScreenButtonCancel.width = patr_10x * 3;
				mRectWorldScreenButtonCancel.height = Mathf.Clamp(patr_10y * 1.2f, 30, 70);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Служебный метод для рисования содержимого окна
			/// </summary>
			/// <param name="id">Идентификатор окна</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnDrawWindow(Int32 id)
			{
				// Основное поле
				GUI.enabled = IsEnabledElement;

				// Кнопка закрыть
				if (GUI.Button(mRectWorldScreenButtonClose, ContentClose))
				{
					LotusGUIDispatcher.UnRegisterElement(mName);

					if (mOnClose != null) mOnClose();
				}

				// Текст
				GUI.Label(mRectWorldScreenContent, mContentText.Text , mStyleMain);

				// Кнопка ОК
				LotusGUIDispatcher.CurrentContent.text = mOkText;
				LotusGUIDispatcher.CurrentContent.image = OkIcon;
				if (GUI.Button(mRectWorldScreenButtonOk, LotusGUIDispatcher.CurrentContent))
				{
					if (mOnOK != null) mOnOK();
				}

				// Кнопка отмены
				LotusGUIDispatcher.CurrentContent.text = mCancelText;
				LotusGUIDispatcher.CurrentContent.image = mCancelIcon;
				if (GUI.Button(mRectWorldScreenButtonCancel, LotusGUIDispatcher.CurrentContent))
				{
					if (mOnCancel != null) mOnCancel();
				}

				if (mIsDraggable)
				{
					if (mIsDragHeader)
					{
						GUI.DragWindow(new Rect(0, 0, mRectWorldScreenHeader.width, mRectWorldScreenHeader.height));
					}
					else
					{
						GUI.DragWindow();
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
				return MemberwiseClone() as CGUIDialogWindow;
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

				CGUIDialogWindow dialog_window = base_element as CGUIDialogWindow;
				if (dialog_window != null)
				{
					mOkText = dialog_window.mOkText;
					mOkIcon = dialog_window.mOkIcon;
					mCancelText = dialog_window.mCancelText;
					mCancelIcon = dialog_window.mCancelIcon;
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