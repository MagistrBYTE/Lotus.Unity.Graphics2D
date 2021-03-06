//=====================================================================================================================
// Проект: LotusPlatform
// Раздел: Общий модуль 2D графики
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file LotusGraphics2DBaseNumbers.cs
*		Определение типов данных и интерфейсов для реализации управления числовыми данными.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 27.03.2022
//=====================================================================================================================
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//=====================================================================================================================
namespace Lotus
{
	namespace Graphics2D
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Unity2DCommonBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для реализации управления числовыми данными
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusNumber
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Значение числовой величины
			/// </summary>
			Single NumberValue { get; set; }

			/// <summary>
			/// Максимальное значение числовой величины
			/// </summary>
			Single MaxValue { get; set; }

			/// <summary>
			/// Минимальное значение числовой величины
			/// </summary>
			Single MinValue { get; set; }

			/// <summary>
			/// Нормализованное значение числовой величины в частях от 0 до 1
			/// </summary>
			Single Percent { get; }

			/// <summary>
			/// Статус минимального значения числовой величины
			/// </summary>
			Boolean IsMinValue { get; }

			/// <summary>
			/// Статус максимального значения числовой величины
			/// </summary>
			Boolean IsMaxValue { get; }
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для реализации управления числовыми данными с параметрами для текстового форматирования и отображения 
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusNumberPresent : ILotusNumber
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Формат отображения числовой величины
			/// </summary>
			String FormatValue { get; set; }

			/// <summary>
			/// Дополнительное обозначение числовой величины
			/// </summary>
			String SuffixValue { get; set; }
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения изменяемой числовой величины
		/// </summary>
		/// <remarks>
		/// Применяется если требуется изменить числовой величину на определенное значение в течении определенного промежутка времени
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusNumberChange
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт смещения величины
			/// </summary>
			/// <param name="time">Время в течение которого должно произойти смещение</param>
			/// <param name="offset">Смещение величины</param>
			//---------------------------------------------------------------------------------------------------------
			void StartChangeValueOffset(Single time, Single offset);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт изменение величины
			/// </summary>
			/// <param name="time">Время в течение которого должно произойти изменение величины</param>
			/// <param name="target">Целевое значение величины</param>
			//---------------------------------------------------------------------------------------------------------
			void StartChangeValueTarget(Single time, Single target);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт изменение числовой величины до максимального значения
			/// </summary>
			/// <param name="time">Время в течение которого должно произойти изменение величины</param>
			//---------------------------------------------------------------------------------------------------------
			void StartChangeValueToMax(Single time);
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определение временного процесса
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ILotusTimeProcess
		{
			/// <summary>
			/// Время процесса
			/// </summary>
			Single ProcessTime { get; set; }

			/// <summary>
			/// Текущие время процесса
			/// </summary>
			Single CurrentTime { get; set; }
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================