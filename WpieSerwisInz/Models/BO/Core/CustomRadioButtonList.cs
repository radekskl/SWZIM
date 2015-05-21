using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WpieSerwisInz.Models.BO.Core
{
    public class CustomRadioButtonList
    {
        public List<Answer> Answers { get; set; }

        public string SelectedValue { get; set; }

        public CustomRadioButtonList()
        {
            Answers = new List<Answer>();
        }
    }

    public class Answer
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

}