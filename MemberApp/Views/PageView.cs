using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemberApp.Models;

namespace MemberApp.Views
{
    class PageView
    {
        public int GetMenuChoice()
        {
            int menuChoice = 0;
            int nrOfMenuChoices = 6;

            ViewHeader("Medlemsregister");

            Console.WriteLine("- ARKIV ------------------------------------");
            Console.WriteLine("\n0: Avsluta");
            Console.WriteLine("1: Visa kompakt lista över medlemmar");
            Console.WriteLine("2: Visa detaljerad lista över medlemmar\n");
            Console.WriteLine("- MEDLEM -----------------------------------");
            Console.WriteLine("\n3: Lägg till medlem");
            Console.WriteLine("4: Visa medlemsuppgifter");
            Console.WriteLine("5: Ändra medlemsuppgifter");
            Console.WriteLine("6: Ta bort medlem\n");
            Console.WriteLine("============================================");
            Console.WriteLine();

            // Menyvalsloop
            do
            {
                try
                {
                    Console.Write("Ange menyval [0-6]:");
                    menuChoice = int.Parse(Console.ReadLine());

                    if (menuChoice > nrOfMenuChoices)
                        ViewMessage("\nFör stort tal!\n\n", ConsoleColor.Red);
                    else 
                        return menuChoice;
                }
                catch
                {
                    ViewMessage("\nInte ett heltal!\n\n", ConsoleColor.Red);
                }
            } 
            while (true);
        }

        public void ViewMessage(string message, ConsoleColor bgrColor = ConsoleColor.Black, ConsoleColor fgrColor = ConsoleColor.White)
        {
            Console.BackgroundColor = bgrColor;
            Console.ForegroundColor = fgrColor;
            Console.Write(message);
            Console.ResetColor();
        }

        public void ViewHeader(string message, ConsoleColor bgrColor = ConsoleColor.DarkCyan, ConsoleColor fgrColor = ConsoleColor.White)
        {
            Console.BackgroundColor = bgrColor;
            Console.ForegroundColor = fgrColor;
            Console.WriteLine("\n============================================");
            Console.ResetColor();
            Console.WriteLine(" " + message);
            Console.BackgroundColor = bgrColor;
            Console.ForegroundColor = fgrColor;
            Console.WriteLine("============================================\n");
            Console.ResetColor();
        }

        public void CompactMemberList(List<Member> members)
        {
            Console.WriteLine();
            ViewHeader("Medlemslista (kompakt)");

            foreach (Member member in members)
                Console.WriteLine("\n" + member.Id + ". " + member.Name + " (" + member.SSN + ")");

            Console.WriteLine();
        }

        public void MemberNameList(List<Member> members)
        {
            Console.WriteLine();
            ViewHeader("Medlemslista");

            foreach (Member member in members)
                Console.WriteLine(member.Id + ". " + member.Name);

            Console.WriteLine();
        }

        public void DetailedMemberList(List<Member> members)
        {
            ViewHeader("Medlemslista (detaljerad)");

            foreach (Member member in members)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n" + member.Id + ". " + member.Name + " (" + member.SSN + ")");
                Console.ResetColor();

            }
            Console.WriteLine();
        }

        public void ViewMember(Member member)
        {
            ViewHeader("Visa medlem");

            Console.WriteLine(member.Id + ". " + member.Name + " (" + member.SSN + ")");
            Console.WriteLine(".....................................");
            int counter = 1;

            Console.WriteLine("\n.....................................\n");
        }

        public string ReadMemberName(string previousValue = "")
        {
            do
            {
                string name;
                ViewMessage("Ange medlemens namn: ", ConsoleColor.Black, ConsoleColor.Gray);
                name = Console.ReadLine();

                // Om användaren tryckt enter då det inte finns ett tidigare värde
                if (String.IsNullOrWhiteSpace(name) && previousValue == "")
                {
                    ViewMessage("\nFEL! Du måste ange ett giltigt namn\n", ConsoleColor.Red);

                    if (!ContinueOnKeyPressed(true))
                        return null;
                }

                // Om användaren tryckt enter då ett tidigare värde finns
                else if (String.IsNullOrWhiteSpace(name) && previousValue != "")
                    return previousValue;

                // Om användaren angett ett nytt värde
                else
                    return name;
            }
            while (true);
        }

        public string ReadSSN(string previousValue = "")
        {
            string ssn;

            do
            {
                ViewMessage("Ange medlemens personnummer: ", ConsoleColor.Black, ConsoleColor.Gray);
                ssn = Console.ReadLine();

                // Om användaren tryckt enter då det inte finns ett tidigare värde
                if (String.IsNullOrWhiteSpace(ssn) && previousValue == "")
                {
                    ViewMessage("\nFEL! Du måste ange ett giltigt personnummer\n", ConsoleColor.Red);

                    if (!ContinueOnKeyPressed(true))
                        return null;
                }

                // Om användaren tryckt enter då det inte finns ett tidigare värde
                else if (String.IsNullOrWhiteSpace(ssn) && previousValue != "")
                    return previousValue;

                // Om användaren angett ett nytt värde
                else
                    return ssn;
            }
            while (true);
        }

        public bool ContinueOnKeyPressed(bool clear)
        {
            ViewMessage("Tryck tangent för att fortsätta - Esc avslutar", ConsoleColor.DarkBlue);

            // Om användaren vill avsluta
            if (Console.ReadKey().Key == ConsoleKey.Escape)
                return false;
            else
            {
                // Om användaren vill rensa skärmen
                if (clear == true)
                    Console.Clear();
                else
                    Console.WriteLine();

                return true;
            }
        }
    }
}
