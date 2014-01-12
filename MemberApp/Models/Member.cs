using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemberApp.Models
{
    class Member : IComparable, IComparable<Member>
    {
        public Member(string name, int id, string ssn, List<Boat> boats)
        {
            Name = name;
            Id = id;
            SSN = ssn;
            Boats = boats;
        }

        public Member(string name, int id, string ssn)
        {
            Name = name;
            Id = id;
            SSN = ssn;
            Boats = new List<Boat>();
        }

        public Member(string name)
        {
            Name = name;
            Id = new int();
            SSN = String.Empty;
            Boats = new List<Boat>();
        }

        public string Name { get; set; }

        public int Id { get; set; }

        public string SSN { get; set; }

        public List<Boat> Boats { get; set; }

        // Sortera lista över medlemmar
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Member other = obj as Member;

            if (other == null)
                throw new ArgumentException("Objektet är inget Recept");

            if (other.Name == Name)
                return 0;

            return Name.CompareTo(other.Name);
        }

        // Sortera lista över medlemmar
        public int CompareTo(Member other)
        {
            if (other == null)
                return 1;

            if (other.Name == Name)
                return 0;

            return Name.CompareTo(other.Name);
        }
    }
}
