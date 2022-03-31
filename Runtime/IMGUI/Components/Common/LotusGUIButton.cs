//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль IMGUI Unity
// Подраздел: Компоненты IMGUI Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGUIButton.cs
*		Компонент представляющий кнопку подсистемы IMGUI Unity.
*		Реализация компонента определяющего кнопку с расширенным событием и определением источника события.
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
using UnityEngine.Events;
//---------------------------------------------------------------------------------------------------------------------
using Lotus.Core;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DImmedateGUIComponent
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Тип события
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class LotusGUIButtonClickedEvent : UnityEvent<LotusGUIButton>
		{
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент представляющий кнопку подсистемы IMGUI Unity
		/// </summary>
		/// <remarks>
		/// Реализация компонента определяющего кнопку с расширенным событием и определением источника события
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XGUIEditorSettings.MenuPath + "Button")]
		public class LotusGUIButton : LotusGUILabel, ILotusVirtualButton
		{
			#region ======================================= МЕТОДЫ СОЗДАНИЯ ЭЛЕМЕНТА ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента Button
			/// </summary>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public new static LotusGUIButton CreateElement()
			{
				// 1) Создание объекта
				GameObject go = new GameObject("GUIButton");
				LotusGUIButton element = go.AddComponent<LotusGUIButton>();

				// 2) Конструктор элемента
				element.OnCreate();

				return element;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// События
			[SerializeField]
			internal UnityEvent mOnClick;
			[SerializeField]
			internal LotusGUIButtonClickedEvent mOnClickSender;

			// Служебные данные
			internal Int32 mLastPressedFrame = -5;
			internal Int32 mReleasedFrame = -5;
			internal Boolean mPressed;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedButton;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о щелчке на кнопку
			/// </summary>
			public UnityEvent OnClick
			{
				get { return mOnClick; }
				set { mOnClick = value; }
			}

			/// <summary>
			/// Событие для нотификации о щелчке на кнопку. Аргумент - источник события
			/// </summary>
			public LotusGUIButtonClickedEvent OnClickSender
			{
				get { return mOnClickSender; }
				set { mOnClickSender = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА IVirtualButton ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Статус удержания нажатой кнопки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsButtonPressed
			{
				get { return mPressed; }
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Статус нажатия кнопки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsButtonDown
			{
				get { return mLastPressedFrame - Time.frameCount == -1; }
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Статус отпускания кнопки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsButtonUp
			{
				get { return mReleasedFrame == Time.frameCount - 1; }
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор элемента
			/// </summary>
			/// <remarks>
			/// Вызывается только в процессе добавления компонента к игровому объекта (метод Reset)
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void OnCreate()
			{
				base.OnCreate();
				mStyleMainName = "Button";
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

				GUI.backgroundColor = mBackgroundColor;

				LotusGUIDispatcher.CurrentContent.text = mTextLocalize;
				LotusGUIDispatcher.CurrentContent.image = mCaptionIcon;

				if (GUI.Button(mRectWorldScreenMain, LotusGUIDispatcher.CurrentContent, mStyleMain))
				{
					if (mOnClick != null) mOnClick.Invoke();
					if (mOnClickSender != null) mOnClickSender.Invoke(this);

					mPressed = true;
					mLastPressedFrame = Time.frameCount;
				}
				if (Event.current.type == EventType.MouseUp && mRectWorldScreenMain.Contains(Event.current.mousePosition))
				{
					mPressed = false;
					mReleasedFrame = Time.frameCount;
				}

				GUI.backgroundColor = Color.white;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================