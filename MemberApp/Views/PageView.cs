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
            int nrOfMenuChoices = 9;

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
            Console.WriteLine("- BÅT --------------------------------------");
            Console.WriteLine("\n7: Lägg till ny båt");
            Console.WriteLine("8: Ändra båtuppgifter");
            Console.WriteLine("9: Ta bort båt\n");
            Console.WriteLine("============================================");
            Console.WriteLine();

            // Menyvalsloop
            do
            {
                try
                {
                    Console.Write("Ange menyval [0-9]:");
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
                Console.WriteLine("\n" + member.Id + ". " + member.Name + " (" + member.SSN + ")" + " - " + "Antal båtar: " + member.Boats.Count());

            Console.WriteLine();
        }

        public void MemberNameList(List<Member> members)
        {
            Console.WriteLine();
            ViewHeader("Medlemslista");

            foreach (Member member in members)
                Console.WriteLine(member.Id + ". " + member.Name + " - " + "Antal båtar: " + member.Boats.Count());

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

                if (member.Boats.Count() > 0)
                {
                    Console.WriteLine("\nBÅTAR");
                    Console.WriteLine(".....................................");


                    foreach (Boat boat in member.Boats)
                    {
                        Console.WriteLine("\nTYP: " + boat.Type);
                        Console.WriteLine("LÄNGD: " + boat.Length + " fot");
                        Console.WriteLine("INFO: " + boat.Info);
                    }
                    Console.WriteLine("\n.....................................");
                }
                else
                    Console.WriteLine("Inga båtar finns registrerade");
            }
            Console.WriteLine();
        }

        public void ViewMember(Member member)
        {
            ViewHeader("Visa medlem");

            Console.WriteLine(member.Id + ". " + member.Name + " (" + member.SSN + ")");
            Console.WriteLine("\nBÅTAR (" + member.Boats.Count() + " st)");
            Console.WriteLine(".....................................");
            int counter = 1;

            foreach (Boat boat in member.Boats)
            {
                Console.WriteLine("\nBåtnummer: " + counter);
                Console.WriteLine("TYP: " + boat.Type);
                Console.WriteLine("LÄNGD: " + boat.Length);
                Console.WriteLine("INFO: " + boat.Info);
                counter++;
            }
            Console.WriteLine("\n.....................................\n");
        }

        public string ReadBoatInfo(string previousValue = "")
        {
            ViewMessage("Båtens information: ", ConsoleColor.Black, ConsoleColor.Gray);

            string info = Console.ReadLine();

            // Om användaren tryckt enter då ett tidigare värde finns
            if (String.IsNullOrWhiteSpace(info) && previousValue != "")
                return previousValue;
            else
                return info;
        }

        public double ReadBoatLength(double previousValue = double.NaN)
        {
            do
            {
                string length;
                ViewMessage("Båtens längd: ", ConsoleColor.Black, ConsoleColor.Gray);

                try
                {
                    length = Console.ReadLine();

                    // Om användaren tryckt enter då det inte finns ett tidigare värde
                    if (String.IsNullOrWhiteSpace(length) && previousValue == double.NaN)
                        ViewMessage("\nFEL! Du måste ange en längd\n", ConsoleColor.Red);

                    // Om användaren tryckt enter då ett tidigare värde finns
                    else if (String.IsNullOrWhiteSpace(length) && previousValue != double.NaN)
                        return previousValue;

                    // Om användaren angett ett nytt värde
                    else
                        return double.Parse(length);

                }
                catch (Exception)
                {
                    ViewMessage("\nFEL! Du måste ange en giltig längd\n", ConsoleColor.Red);
                }
            }
            while (true);
        }

        public string ReadBoatType(string previousValue = "")
        {
            string type;

            ViewMessage("BÅTTYPER\n", ConsoleColor.Black, ConsoleColor.Gray);
            int counter = 1;

            // Skriver ut tillgängliga båttyper
            foreach (BoatType bt in Enum.GetValues(typeof(BoatType)))
            {
                ViewMessage(counter + ". " + bt.ToString() + "\n", ConsoleColor.Black, ConsoleColor.Gray);
                counter++;
            }

            do
            {
                ViewMessage("\nAnge båtens typ: ", ConsoleColor.Black, ConsoleColor.Gray);

                // Försöker tolka indata som tal
                try
                {
                    string input = Console.ReadLine();

                    // Om användaren vill ange tidigare värde
                    if (String.IsNullOrWhiteSpace(input) && previousValue != "")
                        return previousValue;

                    int typeNr = int.Parse(input);

                    switch (typeNr)
                    {
                        case 1:
                            type = BoatType.Segelbåt.ToString();
                            return type;
                        case 2:
                            type = BoatType.Motorseglare.ToString();
                            return type;
                        case 3:
                            type = BoatType.Motorbåt.ToString();
                            return type;
                        case 4:
                            type = BoatType.Kajak.ToString();
                            return type;
                        case 5:
                            type = BoatType.Kanot.ToString();
                            return type;
                        case 6:
                            type = BoatType.Övrigt.ToString();
                            return type;
                        default:
                            ViewMessage("\nFEL! Du måste ange en siffra inom givet intervall\n", ConsoleColor.Red);
                            break;
                    }
                }
                catch (Exception)
                {
                    ViewMessage("\nFEL! Du måste ange en siffra\n", ConsoleColor.Red);
                }
            }
            while (true);
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
                ViewMessage("Ange medlemens Telefonnummer: ", ConsoleColor.Black, ConsoleColor.Gray);
                ssn = Console.ReadLine();

                // Om användaren tryckt enter då det inte finns ett tidigare värde
                if (String.IsNullOrWhiteSpace(ssn) && previousValue == "")
                {
                    ViewMessage("\nFEL! Du måste ange ett giltigt Telefonnummer\n", ConsoleColor.Red);

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

        public void ViewMemberBoats(Member member)
        {
            Console.WriteLine("0: Avbryt");
            Console.WriteLine("-------------------------------");

            for (int i = 0; i < member.Boats.Count(); i++)
            {
                Boat boat = member.Boats[i];
                Console.WriteLine(String.Format("{0}. {1} ({2}) - {3}", i + 1, boat.Type, boat.Length, boat.Info));
            }

            Console.WriteLine("-------------------------------");
        }
    }
}
