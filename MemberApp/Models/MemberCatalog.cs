using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MemberApp.Models
{
    class MemberCatalog
    {
        #region Fält

        private string _path;
        private const string _member = "[Member]";
        private const string _memberId = "[MemberID]";
        private const string _ssn = "[SSN]";
        private const string _boatType = "[BoatType]";
        private const string _boatLength = "[BoatLength]";
        private const string _boatInfo = "[BoatInfo]";
        private List<Member> _members;

        #endregion

        public MemberCatalog(string path)
        {
            Path = path;
        }

        public string Path
        {
            get { return this._path; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ogiltig sökväg");
                else
                    this._path = value;
            }
        }

        public List<Member> Members
        {
            get { return this._members; }
            set { this._members = value; }
        }

        public void Load()
        {
            List<Member> members = new List<Member>();

            try
            {
                // Räknare för antal medlemmar och båtar i texfilen
                int memberCounter = -1;
                int boatCounter = -1;

                List<Boat> boats = new List<Boat>();

                // Läser in textfilen och sparar recepten i listan
                using (StreamReader sr = new StreamReader(Path))
                {
                    string line;
                    RecipeMemberStatus status = new RecipeMemberStatus();

                    // Läser textfilen rad för rad
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Kontrollerar aktuell sektion
                        switch (line)
                        {
                            case _member:
                                status = RecipeMemberStatus.Member;
                                // Itererar while-satsen så nästa rad kan sparas under nuvarande status
                                continue;
                            case _memberId:
                                status = RecipeMemberStatus.MemberID;
                                // Itererar while-satsen så nästa rad kan sparas under nuvarande status
                                continue;
                            case _ssn:
                                status = RecipeMemberStatus.SSN;
                                // Itererar while-satsen så nästa rad kan sparas under nuvarande status
                                continue;
                            case _boatType:
                                status = RecipeMemberStatus.BoatType;
                                // Itererar while-satsen så nästa rad kan sparas under nuvarande status
                                continue;
                            case _boatLength:
                                status = RecipeMemberStatus.BoatLength;
                                // Itererar while-satsen så nästa rad kan sparas under nuvarande status
                                continue;
                            case _boatInfo:
                                status = RecipeMemberStatus.BoatInfo;
                                // Itererar while-satsen så nästa rad kan sparas under nuvarande status
                                continue;
                        }

                        // Ny medlem
                        if (status == RecipeMemberStatus.Member)
                        {
                            // Om ej första inläsningen
                            if (memberCounter >= 0)
                            {
                                // Lägger till båtar. Första iterationen har dock inga båtar kunnat läsas in
                                if (boats.Count() > 0)
                                {
                                    members[memberCounter].Boats = boats;
                                    // Nollställer lista och räknare
                                    boats = new List<Boat>();
                                    boatCounter = -1;
                                }
                            }
                            memberCounter++;
                            Member member = new Member(line);
                            members.Add(member);
                        }

                        // Medlemsnr
                        else if (status == RecipeMemberStatus.MemberID)
                            members[memberCounter].Id = Convert.ToInt32(line);

                        // Telefonnummer
                        else if (status == RecipeMemberStatus.SSN)
                            members[memberCounter].SSN = line;

                        // Båttyp
                        else if (status == RecipeMemberStatus.BoatType)
                        {
                            boatCounter++;
                            boats.Add(new Boat(line));
                        }

                        // Båtlängd
                        else if (status == RecipeMemberStatus.BoatLength)
                            boats[boatCounter].Length = Convert.ToDouble(line);

                        // Båtinfo
                        else if (status == RecipeMemberStatus.BoatInfo)
                            boats[boatCounter].Info = line;

                        else
                            throw new ArgumentException("Felutformad textfil");
                    }
                }

                // Lägger till båtar till första medlemmen (då ej streamreadern slutar läsa efter sista raden)
                if (memberCounter >= 0)
                {
                    if (boats.Count() > 0)
                        members[memberCounter].Boats = boats;
                }

                // Sorterar listan över medlemmar
                var sortedMembers = new List<Member>(members);
                sortedMembers.Sort();
                members = sortedMembers;

                Members = members;
            }
            catch (Exception)
            {
                throw new ArgumentException();
            }
        }

        public void Save()
        {
            using (StreamWriter sw = new StreamWriter(Path))
            {
                for (int i = 0; i < Members.Count; i++)
                {
                    sw.WriteLine(_member);
                    sw.WriteLine(Members[i].Name);

                    sw.WriteLine(_memberId);
                    sw.WriteLine(Members[i].Id);

                    sw.WriteLine(_ssn);
                    sw.WriteLine(Members[i].SSN);

                    foreach (Boat boat in Members[i].Boats)
                    {
                        sw.WriteLine(_boatType);
                        sw.WriteLine(boat.Type);
                        sw.WriteLine(_boatLength);
                        sw.WriteLine(boat.Length);
                        sw.WriteLine(_boatInfo);
                        sw.WriteLine(boat.Info);
                    }
                }
            }
        }

        public Member GetMember(int id)
        {
            for (int i = 0; i < Members.Count(); i++)
            {
                if (Members[i].Id == id)
                    return Members[i];
            }
            return null;
        }

        public int GetNextMemberId()
        {
            int highestId = 0;

            foreach (Member member in Members)
            {
                if (member.Id > highestId)
                    highestId = member.Id;
            }
            return highestId + 1;
        }

        public bool MembersExist()
        {
            // Om medlemsregistret inte innehåller några poster.
            if (Members.Count == 0)
                return false;
            else
                return true;
        }
    }
}
