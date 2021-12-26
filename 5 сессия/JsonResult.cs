namespace WindowsApp.Modules
{
    class JsonResult
    {
        public int id { get; set; } // Код штрафа        
        public string car_num { get; set; } // Номер авто        
        public string licence_num { get; set; } // Номер в/у      
        public string create_date { get; set; } // Дата   
        public string photo { get; set; } // Ссылка на фото
    }
}
