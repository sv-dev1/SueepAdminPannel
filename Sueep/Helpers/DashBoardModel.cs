using Sueep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sueep.Helpers
{
    public class DashBoardModel
    {
        public string SearchString  { get; set; }
        public List<Getmodel> GetModel { get; set; }
    }
    public class SueeperViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SelectedValue { get; set; }
    }
}
