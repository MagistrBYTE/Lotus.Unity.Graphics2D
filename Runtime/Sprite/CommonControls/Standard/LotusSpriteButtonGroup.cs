//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль работы со спрайтами
// Подраздел: Общие элементы интерфейса
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusSpriteButtonGroup.cs
*		ButtonGroup - компонент для логического группирование кнопок.
*		Реализация компонента для логического группирование кнопок и управления их статусом.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.ComponentModel;
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
		//! \addtogroup Unity2DSpriteControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// ButtonGroup - компонент для логического группирование кнопок
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XSpriteEditorSettings.MenuPathCommon + "Button Group")]
		public class LotusSpriteButtonGroup : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mGroupName;
			[SerializeField]
			internal Boolean mIsAllOff;
			[SerializeField]
			internal Boolean mIsMultiSelect;

			// Служебные данные
			[NonSerialized]
			internal List<LotusSpriteButton> mButtons;
			[NonSerialized]
			internal LotusSpriteButton mSelectedButton;

			// События
			[NonSerialized]
			internal Action<LotusSpriteButton> mOnSelect;
			[NonSerialized]
			internal Action<LotusSpriteButton> mOnUnSelect;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Название группы кнопок
			/// </summary>
			public String GroupName
			{
				get { return mGroupName; }
				set { mGroupName = value; }
			}

			/// <summary>
			/// Статус наличия возможности выключения всех кнопок в группе
			/// </summary>
			public Boolean IsAllOff
			{
				get { return mIsAllOff; }
				set { mIsAllOff = value; }
			}

			/// <summary>
			/// Кнопки в группе
			/// </summary>
			public List<LotusSpriteButton> Buttons
			{
				get { return mButtons; }
			}

			/// <summary>
			/// Количество кнопок в группе
			/// </summary>
			public Int32 CountButtons
			{
				get { return mButtons.Count; }
			}

			/// <summary>
			/// Выбранная кнопка
			/// </summary>
			public LotusSpriteButton SelectedButton
			{
				get { return mSelectedButton; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о выборе элемента. Аргумент - выбранный элемент
			/// </summary>
			public Action<LotusSpriteButton> OnSelect
			{
				get { return mOnSelect; }
				set { mOnSelect = value; }
			}

			/// <summary>
			/// Событие для нотификации о снятия выбора элемента. Аргумент - элемент с которого был снят выбор
			/// </summary>
			public Action<LotusSpriteButton> OnUnSelect
			{
				get { return mOnUnSelect; }
				set { mOnUnSelect = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{
				mButtons = XComponentDispatcher.GetAll<LotusSpriteButton>((component) =>
				{
					return component.ButtonGroup == this;
				});

				for (Int32 i = 0; i < mButtons.Count; i++)
				{
					mButtons[i].OnClickEx.AddListener(RaiseSelectButton);
					mButtons[i].InteractiveSource = TInteractiveSource.Mixed;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отключение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDisable()
			{
				for (Int32 i = 0; i < mButtons.Count; i++)
				{
					mButtons[i].OnClickEx.RemoveListener(RaiseSelectButton);
				}

				mButtons.Clear();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Щелчок на кнопке
			/// </summary>
			/// <param name="sender">Источник события</param>
			//---------------------------------------------------------------------------------------------------------
			protected void RaiseSelectButton(LotusSpriteButton sender)
			{
				if (mOnSelect != null) mOnSelect(sender);
				mSelectedButton = sender;

				for (Int32 i = 0; i < mButtons.Count; i++)
				{
					if (mButtons[i] != sender)
					{
						if (mButtons[i].IsVisualActive)
						{
							mButtons[i].IsVisualActive = false;
							if (mOnUnSelect != null) mOnUnSelect(mButtons[i]);
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление кнопки в группу
			/// </summary>
			/// <param name="button">Добавляемая кнопка</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddButton(LotusSpriteButton button)
			{
				if (mButtons.Contains(button) == false)
				{
					button.OnClickEx.AddListener(RaiseSelectButton);
					button.InteractiveSource = TInteractiveSource.Mixed;
					mButtons.Add(button);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление кнопки из группы
			/// </summary>
			/// <param name="button">Удаляемая кнопка</param>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveButton(LotusSpriteButton button)
			{
				if (mButtons.Contains(button))
				{
					button.OnClickEx.RemoveListener(RaiseSelectButton);
					mButtons.Remove(button);
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