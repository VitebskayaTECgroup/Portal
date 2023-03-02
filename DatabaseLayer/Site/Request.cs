using LinqToDB.Mapping;
using System;

namespace DatabaseLayer.Site
{
    [Table(Name = "Autos")]
    public class Request
    {
        // Данные

        [Column(Name = "A_ID"), Identity]
        public int Id { get; set; }

        [Column(Name = "A_target")]
        public string Target { get; set; }

        [Column(Name = "A_adress")]
        public string Location { get; set; }

        [Column(Name = "A_data")]
        public DateTime Date { get; set; }

        [Column(Name = "A_start")]
        public DateTime TimeStart { get; set; }

        [Column(Name = "A_end")]
        public DateTime TimeEnd { get; set; }

        [Column(Name = "A_answer_auto")]
        public int? CarId { get; set; }

        [Column(Name = "A_answer_driver")]
        public int? DriverId { get; set; }

        [Column(Name = "A_answer_comment")]
        public string Comment { get; set; }


        // Информация о создателе заявки

        [Column(Name = "A_create")]
        public int? CreationUserId { get; set; }

        [Column(Name = "A_create_data")]
        public DateTime CreationDate { get; set; }


        // Информация о ответе и отвечающем пользователе

        [Column(Name = "A_answer")]
        public int? DecisionCode { get; set; }

        [Column(Name = "A_answer_create")]
        public int? DecisionUserId { get; set; }

        [Column(Name = "A_answer_create_data")]
        public DateTime? DecisionDate { get; set; }

        [Column(Name = "A_answer_data")]
        public DateTime? DateAccepted { get; set; }
    }
}