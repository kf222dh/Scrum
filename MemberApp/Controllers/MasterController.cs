using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemberApp.Views;
using MemberApp.Models;

namespace MemberApp.Controllers
{
    class MasterController
    {
        #region Fält
        private PageView _pv;
        private MemberCatalog _mc;
        #endregion

        public void CreateControl()
        {
            _pv = new PageView();
            _mc = new MemberCatalog("../../Members.txt");
            _mc.Load();

            if (_mc.Members == null)
            {
                _pv.ViewMessage("FEL! Medlemsregistret kunde inte laddas\n", ConsoleColor.Red);
                Console.Read();
            }
            else
            {
                do
                {
                    // Rensar skärmen varje gång huvudmenyn visas
                    Console.Clear();

                    switch (_pv.GetMenuChoice())
                    {
                        // Avsluta
                        case 0:
                            return;

                        case 1:
                            #region Kompakt lista

                            if (_mc.MembersExist())
                                _pv.CompactMemberList(_mc.Members);
                            else
                                _pv.ViewMessage("Det finns inga medlemmar att visa\n", ConsoleColor.Yellow, ConsoleColor.Black);

                            // Avslutar om escape har trycks ned
                            if (!_pv.ContinueOnKeyPressed(true))
                                return;
                            else
                                continue;

                            #endregion

                        case 2:
                            #region Detaljerad lista

                            if (_mc.MembersExist())
                                _pv.DetailedMemberList(_mc.Members);
                            else
                                _pv.ViewMessage("Det finns inga medlemmar att visa\n", ConsoleColor.Yellow, ConsoleColor.Black);

                            // Avslutar om escape har trycks ned
                            if (!_pv.ContinueOnKeyPressed(true))
                                return;
                            else
                                continue;

                            #endregion

                        case 3:
                            #region Lägg till medlem

                            Member memberToCreate = CreateMember();

                            if (memberToCreate != null)
                                SaveMember(memberToCreate);
                            else
                                _pv.ViewMessage("\nFEL! Medlemmen kunde inte skapas\n\n", ConsoleColor.Red);

                            if (!_pv.ContinueOnKeyPressed(true))
                                return;

                            break;

                            #endregion

                        case 4:
                            #region Visa medlem

                            if (_mc.MembersExist())
                            {
                                _pv.MemberNameList(_mc.Members);
                                bool cancel = false;

                                do
                                {
                                    try
                                    {
                                        _pv.ViewMessage("Ange medlemsnr - 0 avbryter: ", ConsoleColor.Black, ConsoleColor.Gray);
                                        int id = int.Parse(Console.ReadLine());

                                        // 0 avbryter
                                        if (id == 0)
                                        {
                                            cancel = true;
                                            break;
                                        }

                                        Member memberToView = _mc.GetMember(id);

                                        // Om en användare finns med angivet id
                                        if (memberToView != null)
                                        {
                                            _pv.ViewMember(memberToView);
                                            // Avbryter loopen
                                            cancel = true;
                                        }
                                        else
                                            _pv.ViewMessage("Det fanns ingen medlem kopplad till medlemsnumret\n", ConsoleColor.Red);
                                    }
                                    catch (Exception)
                                    {
                                        _pv.ViewMessage("Du måste ange ett heltal\n", ConsoleColor.Red);
                                    }
                                }
                                while (cancel == false);
                            }
                            else
                                _pv.ViewMessage("Det finns inga medlemmar att visa\n", ConsoleColor.Yellow, ConsoleColor.Black);

                            // Avslutar om escape har trycks ned
                            if (!_pv.ContinueOnKeyPressed(true))
                                return;
                            else
                                continue;
                            #endregion'

                        case 5:
                            #region Ändra medlem

                            if (_mc.MembersExist())
                            {
                                // Visa befintliga medlemmar
                                _pv.MemberNameList(_mc.Members);

                                bool cancel = false;

                                do
                                {
                                    try
                                    {
                                        _pv.ViewMessage("Ange medlemsnr för medlem du vill ändra - 0 avbryter: ", ConsoleColor.Black, ConsoleColor.Gray);
                                        int id = int.Parse(Console.ReadLine());

                                        if (id == 0)
                                        {
                                            cancel = true;
                                            break;
                                        }

                                        Member member = _mc.GetMember(id);

                                        // Om en medlem finns med angivet id
                                        if (member != null)
                                        {
                                            EditMember(member);
                                            SaveMember(member);
                                            // Avbryter loopen
                                            cancel = true;
                                        }
                                        else
                                            _pv.ViewMessage("Det fanns ingen medlem kopplad till medlemsnumret\n", ConsoleColor.Red);
                                    }
                                    catch (Exception)
                                    {
                                        _pv.ViewMessage("Du måste ange ett heltal\n", ConsoleColor.Red);
                                    }
                                }
                                while (cancel == false);
                            }
                            else
                                _pv.ViewMessage("Det finns inga medlemmar att ändra\n", ConsoleColor.Yellow, ConsoleColor.Black);

                            // Avslutar om escape har trycks ned
                            if (!_pv.ContinueOnKeyPressed(true))
                                return;
                            else
                                continue;
                            #endregion

                        case 6:
                            #region Ta bort medlem

                            if (_mc.MembersExist())
                            {
                                _pv.MemberNameList(_mc.Members);
                                bool cancel = false;

                                do
                                {
                                    try
                                    {
                                        _pv.ViewMessage("Ange medlemsnr för borttagning - 0 avbryter: ", ConsoleColor.Black, ConsoleColor.Gray);
                                        int id = int.Parse(Console.ReadLine());

                                        if (id == 0)
                                        {
                                            cancel = true;
                                            break;
                                        }

                                        Member member = _mc.GetMember(id);

                                        // Om en medlem finns med angivet id
                                        if (member != null)
                                        {
                                            DeleteMember(member);
                                            _mc.Save();
                                            // Avbryter loopen
                                            cancel = true;
                                        }
                                        else
                                            _pv.ViewMessage("Det fanns ingen medlem kopplad till medlemsnumret\n", ConsoleColor.Red);
                                    }
                                    catch (Exception)
                                    {
                                        _pv.ViewMessage("Du måste ange ett heltal\n", ConsoleColor.Red);
                                    }
                                }
                                while (cancel == false);
                            }
                            else
                                _pv.ViewMessage("Det finns inga medlemmar att ta bort\n", ConsoleColor.Yellow, ConsoleColor.Black);

                            // Avslutar om escape har trycks ned
                            if (!_pv.ContinueOnKeyPressed(true))
                                return;
                            else
                                continue;
                            #endregion

                        case 7:
                            #region Lägg till båt

                            if (_mc.MembersExist())
                            {
                                // Lista medlemmar
                                _pv.MemberNameList(_mc.Members);
                                bool cancel = false;

                                do
                                {
                                    try
                                    {
                                        _pv.ViewMessage("Ange medlemsnr för registrering - 0 avbryter: ", ConsoleColor.Black, ConsoleColor.Gray);

                                        int id = int.Parse(Console.ReadLine());

                                        if (id == 0)
                                        {
                                            cancel = true;
                                            break;
                                        }

                                        Member memberToAddBoat = _mc.GetMember(id);

                                        // Om en användare finns med angivet id
                                        if (memberToAddBoat != null)
                                        {
                                            // Visar befintliga båtar registrerade på medlemmen
                                            _pv.ViewMember(memberToAddBoat);

                                            // Skapa ny båt
                                            Boat boatToCreate = CreateBoat();

                                            if (boatToCreate != null)
                                            {
                                                memberToAddBoat.Boats.Add(boatToCreate);
                                                SaveMember(memberToAddBoat, "Båten registrerades");
                                                // Avbryter loopen
                                                cancel = true;
                                            }
                                            else
                                                _pv.ViewMessage("\n\nFEL! Båten kunde inte registreras\n\n", ConsoleColor.Red);
                                        }
                                        // Om ingen användare med id:t finns
                                        else
                                            _pv.ViewMessage("Det finns inga medlemmar att lägga till båtar på\n", ConsoleColor.Red);
                                    }
                                    catch (Exception)
                                    {
                                        _pv.ViewMessage("Du måste ange ett heltal\n", ConsoleColor.Red);
                                    }
                                }
                                while (cancel == false);
                            }
                            else
                                _pv.ViewMessage("Det finns inga medlemmar att visa\n", ConsoleColor.Yellow, ConsoleColor.Black);

                            // Avslutar om escape har trycks ned
                            if (!_pv.ContinueOnKeyPressed(true))
                                return;
                            else
                                continue;
                            #endregion

                        case 8:
                            #region Ändra båt

                            if (_mc.MembersExist())
                            {
                                // Visa befintliga medlemmar
                                _pv.MemberNameList(_mc.Members);

                                bool cancel = false;

                                do
                                {
                                    try
                                    {
                                        _pv.ViewMessage("Ange medlemsnr för ändring - 0 avbryter: ", ConsoleColor.Black, ConsoleColor.Gray);
                                        int id = int.Parse(Console.ReadLine());

                                        if (id == 0)
                                        {
                                            cancel = true;
                                            break;
                                        }

                                        Member member = _mc.GetMember(id);

                                        // Om en medlem finns med angivet id
                                        if (member != null)
                                        {
                                            _pv.ViewHeader("Välj båt att ändra");

                                            Boat boat = GetBoat(member);
                                            if (boat != null)
                                            {
                                                EditBoat(member, boat);
                                                SaveMember(member, "Båten ändrades");
                                                // Avbryter loopen
                                                cancel = true;
                                            }
                                        }
                                        else
                                            _pv.ViewMessage("Det fanns ingen medlem kopplad till medlemsnumret\n", ConsoleColor.Red);
                                    }
                                    catch (Exception)
                                    {
                                        _pv.ViewMessage("Du måste ange ett heltal\n", ConsoleColor.Red);
                                    }
                                }
                                while (cancel == false);
                            }
                            else
                                _pv.ViewMessage("Det finns inga medlemmar att ändra båtar för\n", ConsoleColor.Yellow, ConsoleColor.Black);

                            // Avslutar om escape har trycks ned
                            if (!_pv.ContinueOnKeyPressed(true))
                                return;
                            else
                                continue;
                            #endregion

                        case 9:
                            #region Ta bort båt

                            if (_mc.MembersExist())
                            {
                                // Visa lista över medlemmar
                                _pv.MemberNameList(_mc.Members);

                                bool cancel = false;

                                do
                                {
                                    try
                                    {
                                        _pv.ViewMessage("Ange medlemsnr för borttagning - 0 avbryter: ", ConsoleColor.Black, ConsoleColor.Gray);

                                        int id = int.Parse(Console.ReadLine());

                                        if (id == 0)
                                        {
                                            cancel = true;
                                            break;
                                        }

                                        Member member = _mc.GetMember(id);

                                        // Om medlem finns med angivet id
                                        if (member != null)
                                        {
                                            if (member.Boats.Count() == 0)
                                                _pv.ViewMessage("\nDet finns ingen båtar registrerade på medlemen\n\n", ConsoleColor.Red);

                                            else
                                            {
                                                _pv.ViewHeader("Välj båt att ta bort");
                                                DeleteBoat(member, GetBoat(member));
                                                _mc.Save();
                                                // Avbryter loopen
                                                cancel = true;
                                            }
                                        }
                                        else
                                            _pv.ViewMessage("Det fanns ingen medlem kopplad till medlemsnumret\n", ConsoleColor.Red);
                                    }
                                    catch (Exception)
                                    {
                                        _pv.ViewMessage("Du måste ange ett heltal\n", ConsoleColor.Red);
                                    }
                                } while (cancel == false);
                            }
                            else
                                _pv.ViewMessage("Det finns inga medlemmar att ta bort båtar för\n", ConsoleColor.Yellow, ConsoleColor.Black);

                            // Avslutar om escape har trycks ned
                            if (!_pv.ContinueOnKeyPressed(true))
                                return;
                            else
                                continue;
                            #endregion

                        // Undantag
                        default:
                            throw new ArgumentException();

                    }
                }
                while (true);
            }
        }

        private void SaveMember(Member member, string message = "Medlemmen sparades")
        {
            if (_mc.GetMember(member.Id) == null)
            {
                _mc.Members.Add(member);

                // Sorterar listan efter att ny medlem lagts till
                var sortedMembers = new List<Member>(_mc.Members);
                sortedMembers.Sort();
                _mc.Members = sortedMembers;
            }
            _mc.Save();
            _pv.ViewMessage("\n" + message + "!\n\n", ConsoleColor.Green, ConsoleColor.Black);
        }

        private Boat CreateBoat()
        {
            _pv.ViewHeader("Skapa ny båt");
            string boatType = _pv.ReadBoatType();

            if (boatType == null)
                return null;

            double length = _pv.ReadBoatLength();

            string info = _pv.ReadBoatInfo();

            Boat boat = new Boat(boatType, length, info);

            return boat;
        }

        private Member CreateMember()
        {
            _pv.ViewHeader("Skapa ny medlem");
            string name = _pv.ReadMemberName();

            if (name == null)
                return null;

            string ssn = _pv.ReadSSN();

            if (ssn == null)
                return null;

            Member member = new Member(name, _mc.GetNextMemberId(), ssn);

            return member;
        }

        private void DeleteBoat(Member member, Boat boat)
        {
            _pv.ViewMessage("\nÄr du säker? [j=ja, övriga=nej]", ConsoleColor.Yellow, ConsoleColor.Black);

            if (Console.ReadKey().Key == ConsoleKey.J)
            {
                try
                {
                    member.Boats.Remove(boat);
                    _pv.ViewMessage("\n\nBåten togs bort\n", ConsoleColor.Green, ConsoleColor.Black);
                }
                catch (Exception)
                {
                    _pv.ViewMessage("\n\nBåten kunde inte tas bort\n", ConsoleColor.Red);
                }
            }
            Console.WriteLine();
        }

        private void DeleteMember(Member member)
        {
            _pv.ViewMessage("\nÄr du säker? [j=ja, övriga=nej]", ConsoleColor.Yellow, ConsoleColor.Black);

            if (Console.ReadKey().Key == ConsoleKey.J)
            {
                try
                {
                    _mc.Members.Remove(member);
                    _pv.ViewMessage("\n\nMedlemmen togs bort\n", ConsoleColor.Green, ConsoleColor.Black);
                }
                catch (Exception)
                {
                    _pv.ViewMessage("\n\nMedlemmen kunde inte tas bort\n", ConsoleColor.Red);
                }
            }
            Console.WriteLine();
        }

        private void EditBoat(Member member, Boat boat)
        {
            _pv.ViewHeader("Ändra båt - Tom rad anger tidigare värde");

            string type = _pv.ReadBoatType(boat.Type);
            double length = _pv.ReadBoatLength(boat.Length);
            string info = _pv.ReadBoatInfo(boat.Info);

            // Ersätt befintlig båt med uppdaterad båt
            int index = member.Boats.IndexOf(boat);
            member.Boats[index] = new Boat(type, length, info);

        }

        private void EditMember(Member member)
        {
            _pv.ViewHeader("Ändra medlem - Tom rad anger tidigare värde");

            string name = _pv.ReadMemberName(member.Name);
            string ssn = _pv.ReadSSN(member.SSN);

            // Ersätt befintlig medlem med uppdaterad medlem
            int index = _mc.Members.IndexOf(member);
            _mc.Members[index] = new Member(name, member.Id, ssn, member.Boats);
            // Sortera användare
            _mc.Members.Sort();
        }

        private Boat GetBoat(Member member)
        {
            _pv.ViewMemberBoats(member);

            do
            {
                try
                {
                    _pv.ViewMessage(String.Format("Välj båt [1-{0}]", member.Boats.Count()), ConsoleColor.Black, ConsoleColor.Gray);
                    int index = int.Parse(Console.ReadLine());

                    if (index == 0)
                        return null;

                    // Index förskjuts då 0 avbryter.
                    return member.Boats[index - 1];
                }
                catch
                {
                    _pv.ViewMessage("Du måste ange ett heltal inom givet intervall\n", ConsoleColor.Red);
                }
            }
            while (true);
        }
    }
}
