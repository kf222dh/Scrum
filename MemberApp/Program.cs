using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemberApp.Views;
using MemberApp.Controllers;

namespace MemberApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MasterController mc = new MasterController();
            mc.CreateControl();
        }
    }
}
