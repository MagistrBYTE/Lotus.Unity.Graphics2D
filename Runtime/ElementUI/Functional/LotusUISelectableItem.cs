//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Модуль компонентов Unity UI
// Подраздел: Общая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusUIImage.cs
*		Визуальный компонент расширяющий возможности базового компонента изображения модуля компонентов Unity UI.
*		Реализация компонента определяющего дополнительные возможности базового компонента изображения и выступающего
*	в качественного основного компонента для вывода изображения.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DUICommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Визуальный компонент расширяющий возможности базового компонента изображения модуля компонентов Unity UI
		/// </summary>
		/// <remarks>
		/// Реализация компонента определяющего дополнительные возможности базового компонента изображения
		/// и выступающего в качественного основного компонента для вывода изображения
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu(XElementUIEditorSettings.MenuPathFunc + "SelectableItem")]
		public class LotusUISelectableItem : Selectable
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Визуальная активность
			[SerializeField]
			internal LotusUIContainerRect Container;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------

			protected override void Reset()
			{
				base.Reset();
				this.Init();
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void Awake()
			{
				base.Awake();
				this.Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnEnable()
			{
				base.OnEnable();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отключение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnDisable()
			{
				base.OnDisable();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных и параметров компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Init()
			{
			}
			#endregion

			#region ======================================= ПЕРЕГРУЖЕНИЫЕ МЕТОДЫ =====================================
			public override void OnPointerDown(PointerEventData eventData)
			{
				base.OnPointerDown(eventData);

				if(Container != null)
				{
					//Container.SelectedItem
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