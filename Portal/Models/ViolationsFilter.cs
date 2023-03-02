using System;

namespace Portal.Models
{
	public class ViolationsFilter
	{
		/// <summary>Строка поиска</summary>
		public string Search { get; set; }

		/// <summary>Точное соответствие</summary>
		public bool Exact { get; set; }



		/// <summary>Сортировка</summary>
		public string Sort { get; set; }

		/// <summary>Порядок сортировки</summary>
		public string Revert { get; set; }

		public bool IsRevert => Revert == "1";



		/// <summary>Временной диапазон: от</summary>
		public DateTime? From { get; set; }

		/// <summary>Временной диапазон: до</summary>
		public DateTime? To { get; set; }



		/// <summary>Ф.И.О. работника</summary>
		public bool FS1 { get; set; }

		/// <summary>Профессия/должность работника</summary>
		public bool FS2 { get; set; }

		/// <summary>Подразделение</summary>
		public bool FS3 { get; set; }

		/// <summary>Ф.И.О. производителя работ</summary>
		public bool FS4 { get; set; }

		/// <summary>Ф.И.О. руководителя работ</summary>
		public bool FS5 { get; set; }

		/// <summary>Описание нарушения</summary>
		public bool FS6 { get; set; }

		/// <summary>Принятые меры</summary>
		public bool FS7 { get; set; }

		/// <summary>Номер приказа</summary>
		public bool FS8 { get; set; }

		/// <summary>Ф.И.О. выявившего нарушения</summary>
		public bool FS9 { get; set; }



		/// <summary>Дата создания записи</summary>
		public bool FVD1 { get; set; }

		/// <summary>Дата выявления нарушения</summary>
		public bool FVD2 { get; set; }

		/// <summary>Ф.И.О. работника</summary>
		public bool FV1 { get; set; }

		/// <summary>Профессия/должность работника</summary>
		public bool FV2 { get; set; }

		/// <summary>Подразделение</summary>
		public bool FV3 { get; set; }

		/// <summary>Ф.И.О. производителя работ</summary>
		public bool FV4 { get; set; }

		/// <summary>Ф.И.О. руководителя работ</summary>
		public bool FV5 { get; set; }

		/// <summary>Описание нарушения</summary>
		public bool FV6 { get; set; }

		/// <summary>Принятые меры</summary>
		public bool FV7 { get; set; }

		/// <summary>Номер приказа</summary>
		public bool FV8 { get; set; }

		/// <summary>Ф.И.О. выявившего нарушения</summary>
		public bool FV9 { get; set; }
	}
}