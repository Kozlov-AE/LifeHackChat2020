using Server.DialogHandler.Model;
using System;
using System.Collections.Generic;

namespace AnswerCreater
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ClientRequest> requests = new List<ClientRequest>();
            DialogStorage ds = new DialogStorage();
            Console.WriteLine("Начнем создавать вопросы? \n Введите Да и продолжим!");
            bool isBigTrue = true;
            var s = Console.ReadLine().ToLower();
            if (s == "да" || s == "lf")
            {
                do
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Введите вопрос: ");
                    Console.ResetColor();
                    var request = new ClientRequest(Console.ReadLine());
                    bool istrue = true;
                    do
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Введите ответ:");
                        Console.ResetColor();
                        request.AddAnswer(Console.ReadLine());
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Введем еще один? Для продолжени жмите Enter, для отмены введите Нет");
                        Console.ResetColor();
                        if (Console.ReadLine().ToLower() == "нет") istrue = false;
                    }
                    while (istrue);
                    ds.AddRequest(request);
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Еще вопрос? Для отмены введите нет, для продолжения Enter");
                    Console.ResetColor();
                    if (Console.ReadLine().ToLower() == "нет") isBigTrue = false;
                }
                while (isBigTrue);
            }
            else Environment.Exit(0);
            Console.WriteLine("По умолчанию имя файла - base.json, поместите созданный файл куда надо");
            ds.SaveBase();
            Console.WriteLine("Сохранение завершено. Досвидания");
        }
    }
}
