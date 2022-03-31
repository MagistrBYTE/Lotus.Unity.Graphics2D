﻿//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Подсистема элементов UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DCommonWindow.cs
*		Определение структур данных и общих типов для оконных элементов интерфейса пользователя.
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
		/// Режим показа окна
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TWindowShowMode
		{
			/// <summary>
			/// Простое появление
			/// </summary>
			Swap,

			/// <summary>
			/// Путем движения
			/// </summary>
			Move,

			/// <summary>
			/// Плавное затухание
			/// </summary>
			Fade,

			/// <summary>
			/// В виде конверта
			/// </summary>
			Convert
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================