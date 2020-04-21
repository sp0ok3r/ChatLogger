using HtmlTags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLogger
{
    public class GenerateHtmlChat
    {
        public static void wtf()
        {

            var tag = new HtmlTag("div");
            tag.Add("h1").Text("Task Assignements");
            var table = new TableTag();
            tag.Append(table);

           // table.AddClass("table");

            table.AddHeaderRow(row =>
            {
                row.Header("Task");
            });
            

        }
    }
}
