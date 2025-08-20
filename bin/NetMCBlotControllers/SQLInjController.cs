using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.SharePoint.Search.Query;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint;

namespace NETMVCBlot.Controllers
{
    public class SQLInjController : Controller
    {
        public ActionResult Index(string input)
        {
            using (ObjectContext studentContext = new ObjectContext("name=StudentEntities"))
            {
                // CTSECISSUE: SQLInjection
                string query = "SELECT VALUE s FROM Students AS s WHERE s.SomeColumn = @inputValue";
                var parameter = new ObjectParameter("inputValue", input);

                var result = studentContext.CreateQuery<Student>(query, parameter);
                // CTSECISSUE: SQLInjection
                string command = "SELECT * FROM Students WHERE SomeColumn = @inputValue";
                studentContext.ExecuteStoreCommand(command,
                    new SqlParameter("@inputValue", input));
                // CTSECISSUE: SQLInjection
                string query = "SELECT * FROM Students WHERE SomeColumn = @inputValue";
                var students = studentContext.ExecuteStoreQuery<Student>(query,
                    new SqlParameter("@inputValue", input)).ToList();
                // CTSECISSUE: SQLInjection
                string query = "SELECT * FROM Students WHERE SomeColumn = @inputValue";
                var students = studentContext.ExecuteStoreQuery<Student>(query,
                    new SqlParameter("@inputValue", input),
                    MergeOption.AppendOnly).ToList();
            }

            KeywordQuery keywordQuery = new KeywordQuery(SPContext.Current.Site)
            {
                QueryText = "SELECT Path FROM SCOPE() WHERE \"SCOPE\" = @inputValue",
            };
            keywordQuery.QueryText += " WHERE \"SCOPE\" = @inputValue";
            keywordQuery.QueryParameters.Add("@inputValue", input);

            FullTextSqlQuery myQuery = keywordQuery.GetFullTextSqlQuery();
            return View();
        }
    }

    class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
    }

    class Student
    {
    }
}