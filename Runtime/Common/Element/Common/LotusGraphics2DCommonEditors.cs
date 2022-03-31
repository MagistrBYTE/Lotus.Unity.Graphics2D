﻿//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Подсистема элементов UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DCommonEditors.cs
*		Определение структур данных и общих типов для управляющих элементов редактирования и ввода данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DCommonElement
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Позиция управляющих кнопок счетчика
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TSpinnerButtonLocation
		{
			/// <summary>
			/// Справа по вертикали
			/// </summary>
			RightVertical,

			/// <summary>
			/// Слева по вертикали
			/// </summary>
			LeftVertical,

			/// <summary>
			/// По обеим сторонам
			/// </summary>
			BothSide,

			/// <summary>
			/// Сверху по горизонтали
			/// </summary>
			TopHorizontal,

			/// <summary>
			/// Снизу по горизонтали
			/// </summary>
			BottomHorizontal,
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================