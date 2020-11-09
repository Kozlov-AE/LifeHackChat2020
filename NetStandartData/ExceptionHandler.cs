using System;

namespace NetStandartData
{
   /// <summary>Данные при ошибке</summary>
	public class ExceptionHandler
	{
      public ExceptionHandler(string message)
      {
         Message = message;
         Date = DateTime.Now;
      }
      public string Message { get; }
      public DateTime Date { get; }
   }
}