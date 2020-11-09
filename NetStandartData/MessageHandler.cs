using System;

namespace NetStandartData
{
	public class MessageHandler
	{
      public DateTime Time { get; }
      public string Message { get; }

      public MessageHandler(string message)
      {
         Time = DateTime.Now;
         Message = message;
      }
   }
}